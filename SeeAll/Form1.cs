using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;
using SeeAll.control;
using SeeAll.control.communication;
using SeeAll.model;

namespace SeeAll
{
    public partial class Form1 : Form
    {
        private Thread showConne;
        private Thread startWorkCpu;
        private Thread startWorkCycle;
        private int timeCloseApp = 100;

        private NLog.Logger logger = LogManager.GetCurrentClassLogger();

        public Form1()
        {
            // TestLoggerShow();    // TEST LOGGER !!!!!

            InitializeComponent();

            this.Text = Application.ProductName;

            var ds = TimeSpan.FromSeconds(Properties.Settings.Default.timerStandartCycle);

            // startAutomaticallyCheckBox = true --> Automatic start of the program
            if (Properties.Settings.Default.startAutomaticallyCheckBox)
            {
                startAutomaticallyCheckBox.Checked = Properties.Settings.Default.startAutomaticallyCheckBox;
                startAutomaticallyCheckBox.ForeColor = Color.Purple;
                startWork();
            }            
        }        

        private void startWorkIndexBtn_Click(object sender, EventArgs e)
        {
            logger.Info("Start of the program");
            startWork();
        }

        private void startWork()
        {
            if (startWorkCpu == null)
            {
                startWorkCpu = new Thread(startWorkCpuTable);
                startWorkCpu.Start();
            }
            if (startWorkCycle == null)
            {
                startWorkCycle = new Thread(startWorkCycleTable);
                startWorkCycle.Start();
            }
            if (showConne == null)
            {
                showConne = new Thread(showConnection);
                showConne.Start();
            }
        }

        private void startWorkCpuTable()
        {
            logger.Info("Start Thread WorkCpu()");
            new WorkCpu();
        }
        private void startWorkCycleTable()
        {
            logger.Info("Start Thread WorkCycleTable()");
            new WorkCycleTable();
        }

        
        private void showConnection()
        {
            logger.Info("Start Thread online start-window");
            while (true)
            {
                // STOP
                if (stopThreadAll) return;

                try
                {
                    runIndexBtn.Invoke(new MethodInvoker(delegate ()
                    {
                        if (WorkCpu.workCpuStatus)
                            runIndexBtn.BackColor = Color.Green;
                        else
                            runIndexBtn.BackColor = Color.Red;
                    }));
                    connectToCpuIndexBtn.Invoke(new MethodInvoker(delegate ()
                    {
                        if (LoadCpu.statusConnCpu)
                            connectToCpuIndexBtn.BackColor = Color.Green;
                        else
                            connectToCpuIndexBtn.BackColor = Color.Red;
                    }));
                    connectToSqlIndexBtn.Invoke(new MethodInvoker(delegate ()
                    {
                        if (WorkCpu.statusConnSql)
                            connectToSqlIndexBtn.BackColor = Color.Green;
                        else
                            connectToSqlIndexBtn.BackColor = Color.Red;
                    }));
                    idIndexLabel.Invoke(new MethodInvoker(delegate ()
                    {
                        idIndexLabel.Text = WorkCpu.idMaxSql.ToString();
                    }));
                    writeIndexLabel.Invoke(new MethodInvoker(delegate ()
                    {
                        writeIndexLabel.Text = WorkCpu.writeIndex.ToString();
                    }));
                    readIndexLabel.Invoke(new MethodInvoker(delegate ()
                    {
                        readIndexLabel.Text = WorkCpu.readIndex.ToString();
                    }));
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Exception online start-window");
                    continue;
                }
                Thread.Sleep(1000);
            }

        }

        private void settingsWorkIndexBtn_Click(object sender, EventArgs e)
        {
            new SettingsForm().Show();
        }
                        
        
        #region Closing form
        public static bool stopThreadAll = false;
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            stopThreadAll = true;
            Thread.Sleep(timeCloseApp);
            FormClosingThread(showConne);
            FormClosingThread(startWorkCpu);
            FormClosingThread(startWorkCycle);
            Thread.Sleep(timeCloseApp);
            killProcess();
        }

        private void FormClosingThread(Thread thread)
        {
            if (thread != null)
            {
                thread.Abort();                
            }
        }

        private void killProcess()
        {
            List<string> name = new List<string> { Application.ProductName };//процесс, который нужно убить
            System.Diagnostics.Process[] etc = System.Diagnostics.Process.GetProcesses();//получим процессы
            foreach (System.Diagnostics.Process anti in etc)//обойдем каждый процесс
            {
                foreach (string s in name)
                {
                    if (anti.ProcessName.ToLower().Contains(s.ToLower())) //найдем нужный и убьем
                    {
                        anti.Kill();
                        name.Remove(s);
                    }
                }
            }
        }
        #endregion

        private void startAutomaticallyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.startAutomaticallyCheckBox = startAutomaticallyCheckBox.Checked;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Количественный состав:
        /// Trace — вывод всего подряд.На тот случай, если Debug не позволяет локализовать ошибку.В нем полезно отмечать вызовы разнообразных блокирующих и асинхронных операций.
        /// Debug — журналирование моментов вызова «крупных» операций.Старт/остановка потока, запрос пользователя и т.п.
        /// Info — разовые операции, которые повторяются крайне редко, но не регулярно. (загрузка конфига, плагина, запуск бэкапа)
        /// Warning — неожиданные параметры вызова, странный формат запроса, использование дефолтных значений в замен не корректных.Вообще все, что может свидетельствовать о не штатном использовании.
        /// Error — повод для внимания разработчиков. Тут интересно окружение конкретного места ошибки.
        /// Fatal — тут и так понятно. Выводим все до чего дотянуться можем, так как дальше приложение работать не будет.
        /// </summary>
        private void TestLoggerShow()
        {
            logger.Trace("trace message");
            logger.Debug("debug message");
            logger.Info("info message");
            logger.Warn("warn message");
            logger.Error("error message");
            logger.Fatal("fatal message");
        }
    }
}

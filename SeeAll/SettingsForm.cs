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
using S7.Net;
using SeeAll.control.communication;
using SeeAll.model;

namespace SeeAll
{
    public partial class SettingsForm : Form
    {
        public static bool statusConnLimitsCpu = false;    // status connection
        private NLog.Logger logger = LogManager.GetCurrentClassLogger();
        private LoadCpu loadCpu = null;

        public SettingsForm()
        {
            InitializeComponent();

            loadCpu = new LoadCpu();

            cpuTypeComboBox.Text = GetCpuTypeConnect.GetStrCpuType(Properties.Settings.Default.CpuType);
            ipTypeTextBox.Text = Properties.Settings.Default.IpCpu;
            rackCpuTextBox.Text = Properties.Settings.Default.RackCpu.ToString();
            slotCpuTextBox.Text = Properties.Settings.Default.SlotCpu.ToString();
            timeSleepExceptionBox.Text = Properties.Settings.Default.timerException.ToString();
            timerStandartCycleTextBox.Text = Properties.Settings.Default.timerStandartCycle.ToString();
            timerMicroDowntimeTextBox.Text = Properties.Settings.Default.timerMicroDowntime.ToString();

            dbLimitCpuTextBox.Text = Properties.Settings.Default.DataBlockLimit.ToString();
            dbDatetimeCpuTextBox.Text = Properties.Settings.Default.DataBlockDatetime.ToString();
            shiftTransitionTextBox.Text = Properties.Settings.Default.shiftTransition;  

            new Thread(cycleWord).Start();
        }

        private void cycleWord()
        {
            while (true)
            {
                Thread.Sleep(2000);
                checkConnCpuBtn.BackColor = SystemColors.Control;
                checkConnBdBtn.BackColor = SystemColors.Control;
            }
        }

        // checkConnCpuBtn
        private void checkConnCpuBtn_Click(object sender, EventArgs e)
        {
            string errorString;
            try
            {
                using (var plc = new Plc(
                    GetCpuTypeConnect.GetCpu(Properties.Settings.Default.CpuType),
                    Properties.Settings.Default.IpCpu,
                    Properties.Settings.Default.RackCpu,
                    Properties.Settings.Default.SlotCpu))   //"172.17.132.200"       "127.0.0.1"
                {
                    plc.Open();
                    if (plc.IsConnected)
                    {
                        statusConnLimitsCpu = true;
                        checkConnCpuBtn.BackColor = Color.Green;
                    }
                    else
                    {
                        statusConnLimitsCpu = false;
                        checkConnCpuBtn.BackColor = Color.Red;                        
                    }
                    errorString = plc.LastErrorString;
                    logger.Warn(errorString);   // Write error log
                }
            }
            catch (Exception ex)
            {
                checkConnCpuBtn.BackColor = Color.Red;
                logger.Error(ex.Message, " TEST connect Cpu to Entity");   // Write error log
                errorString = ex.Message;
            }
            errViewTextBox.Text = errorString;  // Show error on screen
        }

        private void demoCpuBtn_Click(object sender, EventArgs e)
        {
            cpuTypeComboBox.Text = "S71200";
            ipTypeTextBox.Text = "192.168.0.1";
            rackCpuTextBox.Text = "0";
            slotCpuTextBox.Text = "1";
            timeSleepExceptionBox.Text = "1000";
        }

        // SAVE
        private void saveBtn_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.CpuType = GetCpuTypeConnect.GetIntCpuType(cpuTypeComboBox.Text);
            Properties.Settings.Default.IpCpu = ipTypeTextBox.Text;
            Properties.Settings.Default.RackCpu = (short)Convert.ToInt32(rackCpuTextBox.Text);
            Properties.Settings.Default.SlotCpu = (short)Convert.ToInt32(slotCpuTextBox.Text);
            Properties.Settings.Default.timerException = Convert.ToInt32(timeSleepExceptionBox.Text);
            Properties.Settings.Default.timerStandartCycle = Convert.ToInt32(timerStandartCycleTextBox.Text);
            Properties.Settings.Default.timerMicroDowntime = Convert.ToInt32(timerMicroDowntimeTextBox.Text);

            Properties.Settings.Default.DataBlockLimit = Convert.ToInt16(dbLimitCpuTextBox.Text);
            Properties.Settings.Default.DataBlockDatetime = Convert.ToInt16(dbDatetimeCpuTextBox.Text);
            if (CheckShiftTransition(shiftTransitionTextBox.Text))
            {
                Properties.Settings.Default.shiftTransition = shiftTransitionTextBox.Text;
                shiftTransitionTextBox.BackColor = Control.DefaultBackColor;
            }
            else
            {
                shiftTransitionTextBox.BackColor = Color.Red;
            }
            Properties.Settings.Default.Save();
            
            logger.Info("SAVE config --> " + GetCpuTypeConnect.GetIntCpuType(cpuTypeComboBox.Text));
            logger.Info("SAVE config --> " + ipTypeTextBox.Text);
            logger.Info("SAVE config --> " + (short)Convert.ToInt32(rackCpuTextBox.Text));
            logger.Info("SAVE config --> " + (short)Convert.ToInt32(slotCpuTextBox.Text));
            logger.Info("SAVE config --> " + (short)Convert.ToInt32(timeSleepExceptionBox.Text));
        }

        private bool CheckShiftTransition(string value)
        {
            string[] resultString = value.Split(',');
            foreach (var item in resultString)
            {
                int res;
                if (!Int32.TryParse(item, out res))
                {
                    return false;   // err
                }
                if (Convert.ToInt32(item) > 24)
                    return false;   // err
            }
            return true; // ok
        }
       
        // показать write and read
        private void showSettindBtn_Click(object sender, EventArgs e)
        {
            LimitsCpu limitsCpu = loadCpu.ReadLimits();
            if (limitsCpu == null)
            {
                showSettindBtn.BackColor = Color.Red;
                return;   // error connect
            }
            showSettindBtn.BackColor = Color.Green;
            writeSetLabel.Text = limitsCpu.PositionWrite.ToString();
            readSetLabel.Text = limitsCpu.PositionRead.ToString();

        }

        private void readSettingBtn_Click(object sender, EventArgs e)
        {
            loadCpu.WritePositionLimitsCpu(Convert.ToInt32(readSetLabel.Text));   // write PositionWrite (+1)
        }

        public bool CheckConnection()
        {
            using (var db = new SqlContext())
            {
                try
                {
                    db.Database.Connection.Open();
                    if (db.Database.Connection.State == ConnectionState.Open)
                    {
                        errViewTextBox.Text =
                            "db.Database.Connection.State - " + db.Database.Connection.State
                            + "ConnectionString: " + db.Database.Connection.ConnectionString
                            + "DataBase: " + db.Database.Connection.Database
                            + "DataSource: " + db.Database.Connection.DataSource
                            + "ServerVersion: " + db.Database.Connection.ServerVersion
                            + "TimeOut: " + db.Database.Connection.ConnectionTimeout
                            + "db.Database.Connection.State - " + db.Database.Connection.State;

                        logger.Debug("ConnectionString: " + db.Database.Connection.ConnectionString);
                        logger.Debug("DataBase: " + db.Database.Connection.Database);
                        logger.Debug("DataSource: " + db.Database.Connection.DataSource);
                        logger.Debug("ServerVersion: " + db.Database.Connection.ServerVersion);
                        logger.Debug("TimeOut: " + db.Database.Connection.ConnectionTimeout);
                        db.Database.Connection.Close();
                        return true;
                    }
                    else
                    {
                        errViewTextBox.Text = "db.Database.Connection.State - " + db.Database.Connection.State;  // Show error on screen
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    errViewTextBox.Text = ex.Message;
                    logger.Error(ex, "CheckConnection() - false");
                }
                return false;
            }
        }

        private void checkConnBdBtn_Click(object sender, EventArgs e)
        {
            if (CheckConnection())
            {
                checkConnBdBtn.BackColor = Color.Green;
            }
            else
            {
                checkConnBdBtn.BackColor = Color.Red;
            }
        }

        // autoRunWincheckBox
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (autoRunWincheckBox.Checked)
            {
                // ткрываем нужную ветку в реестре   
                // @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run\"  

                Microsoft.Win32.RegistryKey Key =
                    Microsoft.Win32.Registry.LocalMachine.OpenSubKey(
                    "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run\\", true);

                //добавляем первый параметр - название ключа  
                // Второй параметр - это путь к   
                // исполняемому файлу нашей программы.  
                Key.SetValue(Application.ProductName, System.Reflection.Assembly.GetExecutingAssembly().Location);
                Key.Close();
            }
            else
            {
                //удаляем  
                Microsoft.Win32.RegistryKey key =
                Microsoft.Win32.Registry.LocalMachine.OpenSubKey(
                    "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                key.DeleteValue(Application.ProductName, false);
                key.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var dateTime = loadCpu.ReadDatetime(Convert.ToInt32(startByteAdrSettings.Text));
            if (dateTime != null)
            {
                dateTimeSettingsLabel.Text = dateTime.DateTime.ToString();
            }
        }
    }
}

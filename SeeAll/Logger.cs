using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using S7.Net;

namespace SeeAll
{
    class Logger
    {
        private static int errorID = 1;
        private static string endString = "\r\n";
        private static string endMsg = "\r\n\r\n";

        private static string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"log.txt";


        public static void writeMsg(string msg)
        {
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, true))
                {
                    file.WriteLine("Время: " + DateTime.Now);
                    file.WriteLine("Программное сообщение: " + msg);
                    file.WriteLine(endMsg);
                    saveErr(endMsg);
                    file.Close();
                }
            }
            catch (Exception)
            {
                Thread.Sleep(Properties.Settings.Default.LogExceptionTime);
                writeMsg(msg);
            }
        }


        public static void writeError(Exception exception, string msg = " --> ")
        {
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, true))
                {
                    file.WriteLine("Ошибка № " + errorID + endString);
                    file.WriteLine("Время: " + DateTime.Now);
                    file.WriteLine("Тип: " + exception.GetType());
                    file.WriteLine("Системное сообщение: " + exception.Message);
                    file.WriteLine("Программное сообщение: " + msg);
                    file.WriteLine("Стэк вызовов: " + exception.StackTrace.Replace("   ", ""));

                    file.WriteLine(endMsg);
                    saveErr(exception.Message + "\r\n" + endMsg);
                    file.Close();
                }
                errorID++;
            }
            catch
            {
                Thread.Sleep(Properties.Settings.Default.LogExceptionTime);
                writeError(exception, msg);
            }
        }

        public static void saveErr(string msg)
        {
            Properties.Settings.Default.SaveLog_3 = Properties.Settings.Default.SaveLog_2;
            Properties.Settings.Default.SaveLog_2 = Properties.Settings.Default.SaveLog_1;
            Properties.Settings.Default.SaveLog_1 = "Время: " + DateTime.Now + " Программное сообщение: " + msg;
            Properties.Settings.Default.Save();
        }

        public static void WriteErrTextbox(TextBox tbox, string valueErr1, string valueErr2, string valueErr3)
        {
            tbox.Clear();
            tbox.Text =
                valueErr1 + Environment.NewLine +
                valueErr2 + Environment.NewLine +
                valueErr3 + Environment.NewLine;
        }

        public static void LogConnectCpu(S7.Net.Plc plc)
        {
            writeMsg("Cpu LastErrorString -> " + plc.LastErrorString + "\n"
                            + "CpuType -> " + Properties.Settings.Default.CpuType.ToString() + "\n"
                            + "IpCpu -> " + Properties.Settings.Default.IpCpu + "\n"
                            + "RackCpu -> " + Properties.Settings.Default.RackCpu + "\n"
                            + "SlotCpu -> " + Properties.Settings.Default.SlotCpu);
        }
    }
}

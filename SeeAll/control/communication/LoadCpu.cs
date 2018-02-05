using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using S7.Net;
using SeeAll.model;

namespace SeeAll.control.communication
{
    static class LoadCpu
    {
        public static bool statusConnCpu = false;    // status connection
        private static int byteShag = 12; // Bytes
        private static int indexShag = 1; 
        public static int timeOpenConnectCpu = 100;
        private static int countFor = 5;
        public static bool checkWriteReadEqually = false;    // check
        private static string errorString = "";
        public static DateTime defaultErrDateTime = new DateTime(1955, 1, 1, 1, 1, 1);        
        private static NLog.Logger logger = LogManager.GetCurrentClassLogger();

        // loading Datetime from the CPU
        static public Model_dateTime ReadDateTimeCpu(int startByteAdr)
        {
            for (int i = 0; i < countFor; i++)  // counter
            {
                Model_dateTime model_dateTime = ReadDateTimeCpuLogics(startByteAdr);
                if (model_dateTime != null)
                {
                    statusConnCpu = true;
                    // IF year, month, day = 0 THERE Id_DateTime = -1;
                    if (model_dateTime.Id_DateTime == -1)
                    {
                        statusConnCpu = true;
                        logger.Warn("{My coment} if (model_dateTime.Id_DateTime == -1)");
                        return model_dateTime;   // There are data
                    }                    
                    return model_dateTime;   // There are data
                }                
                Thread.Sleep(Properties.Settings.Default.LoadCpuExceptionTime);     //msec
            }
            statusConnCpu = false;
            return null;   // There aren't data
        }

        static public Model_dateTime ReadDateTimeCpuLogics(int startByteAdr)
        {
            indexShag = startByteAdr - 1;
            Model_dateTime modelDateTime = null;
            int year = 0, month = 0, day = 0, hour = 0, minute = 0, second = 0;
            int addShagBytes = indexShag * byteShag;
            try
            {
                using (var plc = new Plc(
                    GetCpuTypeConnect.GetCpuType(Properties.Settings.Default.CpuType),
                    Properties.Settings.Default.IpCpu,
                    Properties.Settings.Default.RackCpu,
                    Properties.Settings.Default.SlotCpu))   //"172.17.132.200"       "127.0.0.1"
                {
                    Thread.Sleep(timeOpenConnectCpu);
                    plc.Open();
                    if (plc.IsConnected)
                    {    
                        modelDateTime = new Model_dateTime();

                        year = Convert.ToInt32(plc.Read(DataType.DataBlock, Properties.Settings.Default.DataBlockDatetime, 0 + addShagBytes, VarType.Int, 1));
                        month = Convert.ToInt32(plc.Read(DataType.DataBlock, Properties.Settings.Default.DataBlockDatetime, 2 + addShagBytes, VarType.Int, 1));
                        day = Convert.ToInt32(plc.Read(DataType.DataBlock, Properties.Settings.Default.DataBlockDatetime, 4 + addShagBytes, VarType.Int, 1));
                        hour = Convert.ToInt32(plc.Read(DataType.DataBlock, Properties.Settings.Default.DataBlockDatetime, 6 + addShagBytes, VarType.Int, 1)); 
                        minute = Convert.ToInt32(plc.Read(DataType.DataBlock, Properties.Settings.Default.DataBlockDatetime, 8 + addShagBytes, VarType.Int, 1));
                        second = Convert.ToInt32(plc.Read(DataType.DataBlock, Properties.Settings.Default.DataBlockDatetime, 10 + addShagBytes, VarType.Int, 1));

                        // IF year, month, day = 0 THERE Id_DateTime = -1;
                        if ((year == 0)||(month == 0)||(day == 0))
                        {
                            modelDateTime.Id_DateTime = -1;
                        }
                        
                        modelDateTime.Id_DateTime = Convert.ToInt64(
                                                            normalIntToString(year)
                                                            + normalIntToString(month)
                                                            + normalIntToString(day)
                                                            + normalIntToString(hour)
                                                            + normalIntToString(minute)
                                                            + normalIntToString(second)
                                                            );
                        modelDateTime.DateTime = new DateTime(year, month, day, hour, minute, second);
                    }
                    else
                    {
                        modelDateTime = null;
                        logger.Warn(plc.LastErrorString);
                    }
                }
            }
            catch (Exception ex)
            {
                modelDateTime = null;
                try
                {
                    logger.Error(ex, "startByteAdr = " + startByteAdr);
                    logger.Error(ex, " | year - " + year);
                    logger.Error(ex, " | month - " + month);
                    logger.Error(ex, " | day - " + day);
                    logger.Error(ex, " | hour - " + hour);
                    logger.Error(ex, " | minute - " + minute);
                    logger.Error(ex, " | second - " + second);
                }
                catch (Exception ex1)
                {
                    logger.Error(ex1, "{My coment} datetime load with CPU - failure");
                }                         
            }
            return modelDateTime;
        }

        public static LimitsCpu ReadLimitsCpu()
        {            
            for (int i = 0; i < countFor; i++)      // counter
            {
                LimitsCpu limitsCpu = ReadLimitsCpuLogics();
                if (limitsCpu != null)
                {
                    statusConnCpu = true;
                    checkWREqually(limitsCpu);      // for check
                    return limitsCpu;               // There are data
                }
                Thread.Sleep(Properties.Settings.Default.LoadCpuExceptionTime);     //msec
            }
            statusConnCpu = false;
            return null;   // There aren't data
        }

        public static LimitsCpu ReadLimitsCpuLogics()
        {
            LimitsCpu limitsCpu = null;
            try
            {
                using (var plc = new Plc(
                    GetCpuTypeConnect.GetCpuType(Properties.Settings.Default.CpuType),
                    Properties.Settings.Default.IpCpu,
                    Properties.Settings.Default.RackCpu,
                    Properties.Settings.Default.SlotCpu))   //"172.17.132.200"       "127.0.0.1"
                {
                    Thread.Sleep(timeOpenConnectCpu);
                    plc.Open();
                    if (plc.IsConnected)
                    {
                        limitsCpu = new LimitsCpu();
                        limitsCpu.PositionWrite = Convert.ToInt32(plc.Read(DataType.DataBlock, Properties.Settings.Default.DataBlockLimit, 0, VarType.Int, 1));
                        limitsCpu.PositionRead = Convert.ToInt32(plc.Read(DataType.DataBlock, Properties.Settings.Default.DataBlockLimit, 2, VarType.Int, 1));
                        limitsCpu.PositionMin = Convert.ToInt32(plc.Read(DataType.DataBlock, Properties.Settings.Default.DataBlockLimit, 4, VarType.Int, 1));
                        limitsCpu.PositionMax = Convert.ToInt32(plc.Read(DataType.DataBlock, Properties.Settings.Default.DataBlockLimit, 6, VarType.Int, 1));
                    }
                    else
                    {
                        limitsCpu = null;
                        errorString = plc.LastErrorString;
                        logger.Warn(errorString);
                    }
                }
            }
            catch (Exception ex)
            {
                limitsCpu = null;
                try
                {
                    logger.Error(ex, " | PositionWrite - " + limitsCpu.PositionWrite);
                    logger.Error(ex, " | PositionRead - " + limitsCpu.PositionRead);
                    logger.Error(ex, " | PositionMin - " + limitsCpu.PositionMin);
                    logger.Error(ex, " | PositionMax - " + limitsCpu.PositionMax);
                }
                catch (Exception ex1)
                {
                    logger.Error(ex1, "{My coment} limitsCpu load with CPU - failure");
                }
            }
            return limitsCpu;
        }

        public static void WritePositionLimitsCpu(int newPositionRead)
        {
            try
            {
                using (var plc = new Plc(
                    GetCpuTypeConnect.GetCpuType(Properties.Settings.Default.CpuType),
                    Properties.Settings.Default.IpCpu,
                    Properties.Settings.Default.RackCpu,
                    Properties.Settings.Default.SlotCpu))   //"172.17.132.200"       "127.0.0.1"
                {
                    Thread.Sleep(timeOpenConnectCpu);
                    plc.Open();
                    if (plc.IsConnected)
                    {
                        statusConnCpu = true;
                        plc.Write("DB" + Properties.Settings.Default.DataBlockLimit + ".DBW2", newPositionRead);
                    }
                    else
                    {
                        statusConnCpu = false;
                        logger.Warn(plc.LastErrorString);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "plc.Write = DB" + Properties.Settings.Default.DataBlockLimit + ".DBW2" + newPositionRead);
                statusConnCpu = false;
            }
        }

        // IF value = 0..9 THERE value = "0" + value
        static public string normalIntToString(int value)
        {
            if (value <= 9)
            {
                return "0" + value;
            }
            else
            {
                return value.ToString();
            }
        }

        /// <summary>
        /// if(PositionRead == PositionWrite) checkWriteReadEqually = true;
        /// </summary>
        private static void checkWREqually(LimitsCpu limitsCpu)
        {
            if (limitsCpu.PositionRead == limitsCpu.PositionWrite)
            {
                checkWriteReadEqually = true;
            }
            else
            {
                checkWriteReadEqually = false;
            }
        }
    }
}

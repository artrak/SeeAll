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
    class LoadingPLC
    {
        public static bool statusConnectionCpu = false;          // status connection
        public bool checkWriteReadEqually = false;               // check whether you can write or not

        // loading Datetime from the CPU
        public Model_dateTime ReadDatetime(int startByteAdress)
        {
            for (int i = 0; i < LoadingPLCSettings.numberOfConnectionAttempts; i++)  // counter
            {
                Model_dateTime model_dateTime = ReadDatetimeLogics(startByteAdress);
                if (model_dateTime != null)
                {
                    statusConnectionCpu = true;
                    // IF year, month, day = 0 THERE Id_DateTime = -1;
                    if (model_dateTime.Id_DateTime == -1)
                    {
                        statusConnectionCpu = true;
                        //TODO need a logger
                        return model_dateTime;   // There are data
                    }
                    else
                        return model_dateTime;   // There are data
                }
                Thread.Sleep(Properties.Settings.Default.LoadCpuExceptionTime);     //msec
            }
            statusConnectionCpu = false;
            return null;   // There aren't data
        }

        private Model_dateTime ReadDatetimeLogics(int startByteAdress)
        {
            int dataBlock = Properties.Settings.Default.DataBlockDatetime;
            LoadingPLCSettings.indexStep = startByteAdress - 1;
            Model_dateTime modelDateTime = null;
            //int year = 0, month = 0, day = 0, hour = 0, minute = 0, second = 0;
            int[] dateTimeArr = new int[6];
            int addShagBytes = LoadingPLCSettings.indexStep * LoadingPLCSettings.byteStep;
            try
            {
                using (var plc = new Plc(
                    GetCpuTypeConnect.GetCpuType(Properties.Settings.Default.CpuType),
                    Properties.Settings.Default.IpCpu,
                    Properties.Settings.Default.RackCpu,
                    Properties.Settings.Default.SlotCpu))   //"172.17.132.200"       "127.0.0.1"
                {
                    Thread.Sleep(LoadingPLCSettings.timeWaitForOpenConnectionCpu);
                    plc.Open();
                    if (plc.IsConnected)
                    {
                        modelDateTime = new Model_dateTime();

                        for (int i = 0; i < dateTimeArr.Length; i++)
                        {
                            dateTimeArr[i] = GetPlcRead(plc, dataBlock, i * 2 + addShagBytes);
                        }
                        modelDateTime.Id_DateTime = getIdDateTimeForReadDatetime(dateTimeArr);
                        modelDateTime.DateTime = getDateTimeForReadDatetime(dateTimeArr);
                    }
                    else
                    {
                        modelDateTime = null;
                        //TODO need a logger
                    }
                }
            }
            catch (Exception ex)
            {
                modelDateTime = null;
                //TODO need a logger
            }
            return modelDateTime;
        }

        private long getIdDateTimeForReadDatetime(int[] dateTimeArray)
        {
            // IF year, month, day = 0 THERE Id_DateTime = -1;
            if ((dateTimeArray[0] == 0) || (dateTimeArray[1] == 0) || (dateTimeArray[2] == 0))
            {
                return -1;
            }

            string strDtId = "";
            foreach (var itemDt in dateTimeArray)
            {
                strDtId += NormalIntToString(itemDt);
            }
            return Convert.ToInt64(strDtId);
        }
        private DateTime getDateTimeForReadDatetime(int[] dateTimeArr)
        {
            return new DateTime(dateTimeArr[0], dateTimeArr[1], dateTimeArr[2], dateTimeArr[3], dateTimeArr[4], dateTimeArr[5]);
        }

        public LimitsCpu ReadLimits()
        {
            for (int i = 0; i < LoadingPLCSettings.numberOfConnectionAttempts; i++)      // counter
            {
                LimitsCpu limitsCpu = ReadLimitsLogics();
                if (limitsCpu != null)
                {
                    statusConnectionCpu = true;
                    CheckWriteReadEqually(limitsCpu);      // for check
                    return limitsCpu;               // There are data
                }
                Thread.Sleep(Properties.Settings.Default.LoadCpuExceptionTime);     //msec
            }
            statusConnectionCpu = false;
            //TODO need a logger
            return null;   // There aren't data
        }

        private LimitsCpu ReadLimitsLogics()
        {
            LimitsCpu limitsCpu = null;
            try
            {
                using (var plc = new Plc(
                    GetCpuTypeConnect.GetCpuType(Properties.Settings.Default.CpuType),
                    Properties.Settings.Default.IpCpu,
                    Properties.Settings.Default.RackCpu,
                    Properties.Settings.Default.SlotCpu))   //"172.17.132.200"
                {
                    Thread.Sleep(LoadingPLCSettings.timeWaitForOpenConnectionCpu);
                    plc.Open();
                    if (plc.IsConnected)
                    {

                        limitsCpu = new LimitsCpu();
                        limitsCpu.PositionWrite = GetPlcRead(plc, Properties.Settings.Default.DataBlockLimit, 0);
                        limitsCpu.PositionRead = GetPlcRead(plc, Properties.Settings.Default.DataBlockLimit, 2);
                        limitsCpu.PositionMin = GetPlcRead(plc, Properties.Settings.Default.DataBlockLimit, 4);
                        limitsCpu.PositionMax = GetPlcRead(plc, Properties.Settings.Default.DataBlockLimit, 6);
                    }
                    else
                    {
                        limitsCpu = null;
                        //TODO need a logger
                    }
                }
            }
            catch (Exception ex)
            {
                limitsCpu = null;
                //TODO need a logger
            }
            return limitsCpu;
        }

        private int GetPlcRead(Plc plc, int dataBlock, int startByteAdress)
        {
            return Convert.ToInt32(plc.Read(DataType.DataBlock, Properties.Settings.Default.DataBlockLimit, startByteAdress, VarType.Int, 1));
        }

        public void WritePositionLimitsCpu(int newPositionRead)
        {
            try
            {
                using (var plc = new Plc(
                    GetCpuTypeConnect.GetCpuType(Properties.Settings.Default.CpuType),
                    Properties.Settings.Default.IpCpu,
                    Properties.Settings.Default.RackCpu,
                    Properties.Settings.Default.SlotCpu))   //"172.17.132.200"       "127.0.0.1"
                {
                    Thread.Sleep(LoadingPLCSettings.timeWaitForOpenConnectionCpu);
                    plc.Open();
                    if (plc.IsConnected)
                    {
                        statusConnectionCpu = true;
                        plc.Write(LoadingPLCSettings.dbPLC + Properties.Settings.Default.DataBlockLimit + LoadingPLCSettings.dbwPLC, newPositionRead);
                    }
                    else
                    {
                        statusConnectionCpu = false;
                        //TODO need a logger
                    }
                }
            }
            catch (Exception ex)
            {
                statusConnectionCpu = false;
                //TODO need a logger
            }
        }

        // IF value = 0..9 THERE value = "0" + value
        public string NormalIntToString(int value)
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
        private void CheckWriteReadEqually(LimitsCpu limitsCpu)
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
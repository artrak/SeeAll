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
    class LoadCpu
    {
        private int byteStep = 12;                               // step in bytes
        private int indexShag = 1;                               // to calculate the addition of a step in bytes
        public static bool statusConnCpu = false;                       // status connection
        public int timeWaitForOpenConnectCpu = 100;              // waiting time for an open Cpu connection
        private int numberOfConnectionAttempts = 5;              // number of connection attempts
        public bool checkWriteReadEqually = false;               // check whether you can write or not
        public DateTime defaultErrDateTime = new DateTime(1955, 1, 1, 1, 1, 1);
        private string dbTable = "DB";
        private string dbwTable = ".DBW2";

        // loading Datetime from the CPU
        public Model_dateTime ReadDatetime(int startByteAdr)
        {
            for (int i = 0; i < numberOfConnectionAttempts; i++)  // counter
            {
                Model_dateTime model_dateTime = ReadDatetimeLogics(startByteAdr);
                if (model_dateTime != null)
                {
                    statusConnCpu = true;
                    // IF year, month, day = 0 THERE Id_DateTime = -1;
                    if (model_dateTime.Id_DateTime == -1)
                    {
                        statusConnCpu = true;
                        //TODO need a logger
                        return model_dateTime;   // There are data
                    }    
                    else                
                        return model_dateTime;   // There are data
                }                
                Thread.Sleep(Properties.Settings.Default.LoadCpuExceptionTime);     //msec
            }
            statusConnCpu = false;
            return null;   // There aren't data
        }

        private Model_dateTime ReadDatetimeLogics(int startByteAdr)
        {
            int dataBlock = Properties.Settings.Default.DataBlockDatetime;
            indexShag = startByteAdr - 1;
            Model_dateTime modelDateTime = null;
            //int year = 0, month = 0, day = 0, hour = 0, minute = 0, second = 0;
            int[] dateTimeArr = new int[6];
            int addShagBytes = indexShag * byteStep;
            try
            {
                using (var plc = new Plc(
                    GetCpuTypeConnect.GetCpu(Properties.Settings.Default.CpuType),
                    Properties.Settings.Default.IpCpu,
                    Properties.Settings.Default.RackCpu,
                    Properties.Settings.Default.SlotCpu))   //"172.17.132.200"       "127.0.0.1"
                {
                    Thread.Sleep(timeWaitForOpenConnectCpu);
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

       private long getIdDateTimeForReadDatetime(int[] dateTimeArr)
        {
            // IF year, month, day = 0 THERE Id_DateTime = -1;
            if ((dateTimeArr[0] == 0) || (dateTimeArr[1] == 0) || (dateTimeArr[2] == 0))
            {
                return -1;
            }

            string strDtId = "";
            foreach (var itemDt in dateTimeArr)
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
            for (int i = 0; i < numberOfConnectionAttempts; i++)      // counter
            {
                LimitsCpu limitsCpu = ReadLimitsLogics();
                if (limitsCpu != null)
                {
                    statusConnCpu = true;
                    CheckWREqually(limitsCpu);      // for check
                    return limitsCpu;               // There are data
                }
                Thread.Sleep(Properties.Settings.Default.LoadCpuExceptionTime);     //msec
            }
            statusConnCpu = false;
            //TODO need a logger
            return null;   // There aren't data
        }

        private LimitsCpu ReadLimitsLogics()
        {
            LimitsCpu limitsCpu = null;
            try
            {
                using (var plc = new Plc(
                    GetCpuTypeConnect.GetCpu(Properties.Settings.Default.CpuType),
                    Properties.Settings.Default.IpCpu,
                    Properties.Settings.Default.RackCpu,
                    Properties.Settings.Default.SlotCpu))   //"172.17.132.200"
                {
                    Thread.Sleep(timeWaitForOpenConnectCpu);
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

        private int GetPlcRead(Plc plc, int dataBlock, int startByteAdr)
        {
            return Convert.ToInt32(plc.Read(DataType.DataBlock, Properties.Settings.Default.DataBlockLimit, startByteAdr, VarType.Int, 1));
        }

        public void WritePositionLimitsCpu(int newPositionRead)
        {
            try
            {
                using (var plc = new Plc(
                    GetCpuTypeConnect.GetCpu(Properties.Settings.Default.CpuType),
                    Properties.Settings.Default.IpCpu,
                    Properties.Settings.Default.RackCpu,
                    Properties.Settings.Default.SlotCpu))   //"172.17.132.200"       "127.0.0.1"
                {
                    Thread.Sleep(timeWaitForOpenConnectCpu);
                    plc.Open();
                    if (plc.IsConnected)
                    {
                        statusConnCpu = true;
                        plc.Write(dbTable + Properties.Settings.Default.DataBlockLimit + dbwTable, newPositionRead);
                    }
                    else
                    {
                        statusConnCpu = false;
                        //TODO need a logger
                    }
                }
            }
            catch (Exception ex)
            {
                statusConnCpu = false;
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
        private void CheckWREqually(LimitsCpu limitsCpu)
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
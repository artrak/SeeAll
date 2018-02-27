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
    public class LoadingPLC
    {

        public int byteStep { get; set; }
        public string dbPLC { get; set; }
        public string dbwPLC { get; set; }
        public int waitingTime { get; set; }            // waiting time for open connection PLC
        public int numberAttempts { get; set; }         // number of connection attempts
        public bool statusConnection { get; set; }   // status of connection to PLC
        public bool equalityCheck { get; set; }         // equality check of write and read position PLC

        public LoadingPLC()
        {
            byteStep = 12;
            dbPLC = "DB";
            dbwPLC = ".DBW2";
            waitingTime = 100;
            numberAttempts = 5;
            statusConnection = false;
            equalityCheck = false;
        }

        // loading Datetime from the CPU
        public Model_dateTime ReadDateTime(int startByteAdress)
        {
            for (int i = 0; i < numberAttempts; i++)  // counter
            {
                Model_dateTime model_dateTime = ReadDatetimeLogics(startByteAdress);
                if (model_dateTime != null)
                {
                    statusConnection = true;
                    return model_dateTime;
                }
                Thread.Sleep(Properties.Settings.Default.LoadCpuExceptionTime);     //msec
            }
            statusConnection = false;
            return null;   // There aren't data
        }

        private Model_dateTime ReadDatetimeLogics(int startByteAdress)
        {
            int dataBlock = Properties.Settings.Default.DataBlockDatetime;

            int[] dateTimeArray = new int[6];
            int addStepBytes = (--startByteAdress) * byteStep;
            try
            {
                using (var plc = new Plc(
                    GetCpuTypeConnect.GetCpu(Properties.Settings.Default.CpuType),
                    Properties.Settings.Default.IpCpu,
                    Properties.Settings.Default.RackCpu,
                    Properties.Settings.Default.SlotCpu))   //"172.17.132.200"       "127.0.0.1"
                {
                    Thread.Sleep(waitingTime);
                    plc.Open();
                    if (plc.IsConnected)
                    {
                        Model_dateTime modelDateTime = new Model_dateTime();

                        for (int i = 0; i < dateTimeArray.Length; i++)
                        {
                            dateTimeArray[i] = GetPlcRead(plc, dataBlock, i * 2 + addStepBytes); // every two bytes
                        }
                        modelDateTime.Id_DateTime = getIdDateTimeForReadDatetime(dateTimeArray);
                        modelDateTime.DateTime = getDateTimeForReadDatetime(dateTimeArray);

                        return modelDateTime;
                    }
                    else
                    {
                        return null;
                        //TODO need a logger
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
                //TODO need a logger
            }
            
        }

        private long getIdDateTimeForReadDatetime(int[] dateTimeArray)
        {
            // IF year, month, day = 0 THERE Id_DateTime = -1;
            if ((dateTimeArray[0] == 0) || (dateTimeArray[1] == 0) || (dateTimeArray[2] == 0))
            {
                return -1;
            }

            string stringIdDateTime = "";
            foreach (var item in dateTimeArray)
            {
                stringIdDateTime += NormalIntToString(item);
            }
            return Convert.ToInt64(stringIdDateTime);
        }

        private DateTime getDateTimeForReadDatetime(int[] dateTimeArray)
        {
            return new DateTime(dateTimeArray[0], dateTimeArray[1], dateTimeArray[2], dateTimeArray[3], dateTimeArray[4], dateTimeArray[5]);
        }

        public LimitsCpu ReadLimits()
        {
            for (int i = 0; i < numberAttempts; i++)      // counter
            {
                LimitsCpu limitsCpu = ReadLimitsLogics();
                if (limitsCpu != null)
                {
                    statusConnection = true;
                    CheckWriteReadEqually(limitsCpu);      // for check
                    return limitsCpu;                      // There are data
                }
                Thread.Sleep(Properties.Settings.Default.LoadCpuExceptionTime);     //msec
            }
            statusConnection = false;
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
                    Thread.Sleep(waitingTime);
                    plc.Open();
                    if (plc.IsConnected)
                    {

                        limitsCpu = new LimitsCpu();
                        limitsCpu.PositionWrite = GetPlcRead(plc, Properties.Settings.Default.DataBlockLimit, 0);
                        limitsCpu.PositionRead = GetPlcRead(plc, Properties.Settings.Default.DataBlockLimit, 2);
                        limitsCpu.PositionMin = GetPlcRead(plc, Properties.Settings.Default.DataBlockLimit, 4);
                        limitsCpu.PositionMax = GetPlcRead(plc, Properties.Settings.Default.DataBlockLimit, 6);

                        return limitsCpu;
                    }
                    else
                    {
                        return null;
                        //TODO need a logger
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
                //TODO need a logger
            } 
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
                    GetCpuTypeConnect.GetCpu(Properties.Settings.Default.CpuType),
                    Properties.Settings.Default.IpCpu,
                    Properties.Settings.Default.RackCpu,
                    Properties.Settings.Default.SlotCpu))   //"172.17.132.200"       "127.0.0.1"
                {
                    Thread.Sleep(waitingTime);
                    plc.Open();
                    if (plc.IsConnected)
                    {
                        statusConnection = true;
                        plc.Write(dbPLC + Properties.Settings.Default.DataBlockLimit + dbwPLC, newPositionRead);
                    }
                    else
                    {
                        statusConnection = false;
                        //TODO need a logger
                    }
                }
            }
            catch (Exception ex)
            {
                statusConnection = false;
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
                equalityCheck = true;
            }
            else
            {
                equalityCheck = false;
            }
        }
    }
}
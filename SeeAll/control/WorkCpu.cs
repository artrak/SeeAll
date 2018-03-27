using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using SeeAll.control.communication;
using SeeAll.model;

namespace SeeAll.control
{
    class WorkCpu
    {
        public static bool statusConnSql = false;    // status connection
        public static bool workCpuStatus = false;
        public static long idMaxSql = 0; //
        public static int writeIndex = 0;
        public static int readIndex = 0;
        private int addPositionIndex = 1;
        private bool firsStartWorkCpu = false;

        private static long noConnectionLimitTimeSec = 600000;    // sec
        private bool isNoConnectionTimeSec = true;
        
        public WorkCpu()
        {
            LoadingPLC loadPLC = new LoadingPLC();
            while (true)
                if (GoWorkCpu(loadPLC))   // 0 --> err
                    break;
        }

        private bool GoWorkCpu(LoadingPLC loadPLC)
        {
            // STOP
            if (Form1.stopThreadAll)
                return false;

            // Sretp 1
            int nextIdCpu = GetNextIdCpu(loadPLC);
            if (nextIdCpu == -1)
                return false;
            // Step 2
            Model_dateTime modelDTBase = WriteIndexCpu(loadPLC, nextIdCpu);
            if (modelDTBase == null)
                return false;
            // Step 3
            WriteDTSql(loadPLC, modelDTBase, nextIdCpu);
            // FINISH Work with CPU
            return true;    // all right
        }

        private int GetNextIdCpu(LoadingPLC loadPLC)
        {
            LimitsCpu limitsCpu = loadPLC.ReadLimits();

            if (limitsCpu == null)
                return -1;                                            // error connect

            if ((limitsCpu.PositionRead < limitsCpu.PositionMin)
                || (limitsCpu.PositionRead > limitsCpu.PositionMax))
            {
                loadPLC.WritePositionLimitsCpu(limitsCpu.PositionMin);  // write PositionRead
                return -1;
            }
            else
            {
                // new PositionRead + 1
                int newIndexDb = LocationIndexDb(loadPLC, limitsCpu);
                // if there isn't data
                if (newIndexDb < limitsCpu.PositionMin)
                {
                    FindMaxIdFromSql();
                    return -1;
                }
                else
                    return newIndexDb;
            }
        }

        private Model_dateTime WriteIndexCpu(LoadingPLC loadPLC, int newIndexDb)
        {
            // loading data all CPU--------
            Model_dateTime modelDTBase = loadPLC.ReadDateTime(newIndexDb);
            if (modelDTBase.Id_DateTime == -1)
            {
                loadPLC.WritePositionLimitsCpu(newIndexDb);   // write PositionWrite (+1)
                return null;
            }
            //------------------------------
            idMaxSql = modelDTBase.Id_DateTime;  // save ID to static

            return modelDTBase;
        }

        // ok <<< Write id and DateTime to SQL
        private void WriteDTSql(LoadingPLC loadPLC, Model_dateTime modelDTBase, int newIndexDb)
        {
            WriteDateTimeToSql(loadPLC, modelDTBase, newIndexDb);
        }
         

        private void FindMaxIdFromSql()
        {
            if (idMaxSql == 0)
            {
                using (var db = new SqlContext())
                {
                    statusConnSql = true;
                    try
                    {
                        // find MAX ID in the SQL
                        idMaxSql = db.model_dateTime.Max(p => p.Id_DateTime);
                    }
                    catch (Exception ex)
                    {
                        //TODO need a logger
                        Thread.Sleep(Properties.Settings.Default.timerException);
                        statusConnSql = false;
                    }
                }
            }
        }

        private void WriteDateTimeToSql(LoadingPLC loadPLC, Model_dateTime modelDTBase, int newIndexDb)
        {
            using (SqlContext db = new SqlContext())
            {
                statusConnSql = true;
                try
                {
                    // check id for duplicates
                    Model_dateTime myEntity = db.model_dateTime.FirstOrDefault(p => p.Id_DateTime == modelDTBase.Id_DateTime);

                    if (myEntity == null)
                    {
                        // add to BD SQL -------------------------WRITE-----<-----<-----<
                        db.model_dateTime.Add(modelDTBase);
                        db.SaveChanges();
                    }
                    loadPLC.WritePositionLimitsCpu(newIndexDb);   // write PositionRead
                }
                catch (Exception ex)
                {
                    Thread.Sleep(Properties.Settings.Default.timerException);
                    try
                    {
                        // add to BD SQL -------------------------WRITE-----<-----<-----<
                        db.model_dateTime.Add(modelDTBase);
                        db.SaveChanges();
                        statusConnSql = true;
                    }
                    catch (Exception ex2)
                    {
                        //TODO need a logger
                        statusConnSql = false;
                    }
                    //TODO need a logger
                    Thread.Sleep(Properties.Settings.Default.timerException);
                    statusConnSql = false;
                }
            }
        }

        private int LocationIndexDb(LoadingPLC loadPLC, LimitsCpu limitsCpu)
        {
            // last index BD
            if (limitsCpu.PositionRead == limitsCpu.PositionMax)
            {
                if (limitsCpu.PositionRead != limitsCpu.PositionWrite)
                {
                    limitsCpu.PositionRead = limitsCpu.PositionMin;
                    return limitsCpu.PositionMin;
                }
                else
                    return limitsCpu.PositionRead;
            }
            // not to do
            if (limitsCpu.PositionRead == limitsCpu.PositionWrite)
            {
                return limitsCpu.PositionRead;  // - addPositionIndex;
            }
            // ok
            if (limitsCpu.PositionRead >= limitsCpu.PositionMin - addPositionIndex)
            {
                if (CheckDateTimeCpu(loadPLC, limitsCpu))
                    return limitsCpu.PositionWrite + addPositionIndex;  // idex > Write --> new value (will Write < Read)
                else
                    return limitsCpu.PositionRead + addPositionIndex;   // (will Write > Read)
            }
            else
            {
                // bad
                return limitsCpu.PositionMin - addPositionIndex;
            }
        }

        /// <summary>
        /// Write перегнал Read (1 цикл)
        /// </summary>
        /// <returns>
        /// false - продолжать с Read (+2)
        /// true - продолжать с Write (+2)
        /// </returns>
        private bool CheckDateTimeCpu(LoadingPLC loadPLC, LimitsCpu limitsCpu)
        {
            //----------------------------------------------------------------
            // loading data all CPU
            var tempDtRead = loadPLC.ReadDateTime(limitsCpu.PositionRead);
            var tempDtWrite = loadPLC.ReadDateTime(limitsCpu.PositionWrite);
            if ((tempDtRead == null) || (tempDtWrite == null))
            {
                return false;
            }
            long dtRead = tempDtRead.Id_DateTime;
            long dtWrite = tempDtWrite.Id_DateTime;

            // no connections > 1 hour
            if ((!isNoConnectionTimeSec) && (dtRead < dtWrite))
            {
                // after (перегнал)                
                return false;                
            }
            //----------------------------------------------------------------
            long dtdtReadAfterWrite;
            if ((limitsCpu.PositionWrite + addPositionIndex > limitsCpu.PositionMax) || (limitsCpu.PositionWrite < limitsCpu.PositionMin))
            {
                var tempDtdtReadAfterWrite = loadPLC.ReadDateTime(limitsCpu.PositionMin);
                if (tempDtdtReadAfterWrite == null)
                {
                    return false;
                }
                else
                {
                    dtdtReadAfterWrite = tempDtdtReadAfterWrite.Id_DateTime;
                }
            }
            else
            {
                var tempDtdtReadAfterWrite = loadPLC.ReadDateTime(limitsCpu.PositionWrite + addPositionIndex);
                if (tempDtdtReadAfterWrite == null)
                {
                    return false;
                }
                else
                {
                    dtdtReadAfterWrite = tempDtdtReadAfterWrite.Id_DateTime;
                }
            }

            if (dtdtReadAfterWrite == 0)
            {
                // before
                return false;
            }

            // no connections > 1 hour
            if (isNoConnectionTimeSec)
            {
                // after (перегнал)
                if ((dtRead < dtWrite) && (dtRead > dtdtReadAfterWrite))
                {
                    // true - Is present to SQL (dtdtReadAfterWrite)
                    if (!IsCheckPresent(dtdtReadAfterWrite))
                    {
                        return true;
                    }
                }
            }
            // before
            return false;
        }

        /// <summary>
        /// Проверка-на-присудствие-записи-в-SQL
        /// </summary>
        /// <param name="valueCheck"></param>
        /// <returns>true - Is present, false - Not present</returns>
        private bool IsCheckPresent(long valueCheck)
        {            
            using (SqlContext db = new SqlContext())
            {
                try
                {
                    Model_dateTime myEntity = db.model_dateTime.FirstOrDefault(p => p.Id_DateTime == valueCheck);
                    WorkCpu.statusConnSql = true;
                    if (myEntity == null)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    //TODO need a logger
                    return false;    
                }
            }
        }

        
        private DateTime tempNoConnectionTimeSec = DateTime.Now;
        /// <summary>
        /// Makes the status, for a long time there was no connection with the CPU
        /// isNoConnectionTimeSec = false - No Connection
        /// </summary>
        private void NoConnectionTimeSec(LoadCpu loadCpu)
        {
            if (LoadCpu.statusConnCpu)
            {
                isNoConnectionTimeSec = false;
            }
            else
            {
                // no connection to CPU   
                long differenceTime = Convert.ToInt64(DateTime.Now.Subtract(tempNoConnectionTimeSec).TotalSeconds);
                if (differenceTime >= noConnectionLimitTimeSec)
                {
                    isNoConnectionTimeSec = true;
                }
            }
        }
    }
}

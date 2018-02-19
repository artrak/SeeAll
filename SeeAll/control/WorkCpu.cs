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
        public static long idMaxSql = 0;
        public static int writeIndex = 0;
        public static int readIndex = 0;
        private int addPositionIndex = 1;
        private bool firsStartWorkCpu = false;

        private static long noConnectionLimitTimeSec = 600000;    // sec
        private bool isNoConnectionTimeSec = true;

        private NLog.Logger logger = LogManager.GetCurrentClassLogger();
        
        public WorkCpu()
        {
            LoadCpu loadCpu = new LoadCpu();

            while (true)
            {
                // STOP
                if (Form1.stopThreadAll) return;

                workCpuStatus = true;

                // Makes the status, for a long time there was no connection with the CPU
                NoConnectionTimeSec(loadCpu);

                // select limits---------------
                LimitsCpu limitsCpu = loadCpu.ReadLimits();
                if (limitsCpu == null)
                    continue;   // error connect
                if ((limitsCpu.PositionRead < limitsCpu.PositionMin)
                    ||(limitsCpu.PositionRead > limitsCpu.PositionMax))
                {
                    loadCpu.WritePositionLimitsCpu(limitsCpu.PositionMin);   // write PositionRead
                    continue;
                }
                //------------------------------

                writeIndex = limitsCpu.PositionWrite;
                readIndex = limitsCpu.PositionRead;

                
                // new PositionRead + 1
                int newIndexDb = LocationIndexDb(loadCpu, limitsCpu);
                // if there isn't data
                if (newIndexDb < limitsCpu.PositionMin)
                {
                    FindMaxIdFromSql();
                    continue;
                }
                else
                {
                    // loading data all CPU--------
                    Model_dateTime modelDTBase = loadCpu.ReadDatetime(newIndexDb);
                    if (modelDTBase == null)
                        continue;   // error connect
                    if (modelDTBase.Id_DateTime == -1)
                    {
                        loadCpu.WritePositionLimitsCpu(newIndexDb);   // write PositionWrite (+1)
                        continue;
                    }
                    //------------------------------

                    idMaxSql = modelDTBase.Id_DateTime;  // save ID to static

                    // ok <<< Write id and DateTime to SQL
                    WriteDateTimeToSql(loadCpu, modelDTBase, newIndexDb);
                }
                
                if (Form1.stopThreadAll) return;    // STOP
                Thread.Sleep(Properties.Settings.Default.timerWorkCycle);
            }
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
                        logger.Error(ex.Message, "FindMaxIdFromSql() --> idMaxSql");
                        Thread.Sleep(Properties.Settings.Default.timerException);
                        statusConnSql = false;
                    }
                }
            }
        }

        private void WriteDateTimeToSql(LoadCpu loadCpu, Model_dateTime modelDTBase, int newIndexDb)
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
                    loadCpu.WritePositionLimitsCpu(newIndexDb);   // write PositionRead
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
                        logger.Error(ex2.Message, " --- WriteDateTimeToSql(Model_dateTime modelDTBase, int newIndexDb)  --> no load MAX Id");
                        statusConnSql = false;
                    }                    
                    logger.Error(ex.Message, " --- WriteDateTimeToSql(Model_dateTime modelDTBase, int newIndexDb)  --> no load MAX Id");                    
                    Thread.Sleep(Properties.Settings.Default.timerException);
                    statusConnSql = false;
                }
            }
        }

        private int LocationIndexDb(LoadCpu loadCpu, LimitsCpu limitsCpu)
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
                if (CheckDateTimeCpu(loadCpu, limitsCpu))
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
        private bool CheckDateTimeCpu(LoadCpu loadCpu, LimitsCpu limitsCpu)
        {
            //----------------------------------------------------------------
            // loading data all CPU
            var tempDtRead = loadCpu.ReadDatetime(limitsCpu.PositionRead);
            var tempDtWrite = loadCpu.ReadDatetime(limitsCpu.PositionWrite);
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
                var tempDtdtReadAfterWrite = loadCpu.ReadDatetime(limitsCpu.PositionMin);
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
                var tempDtdtReadAfterWrite = loadCpu.ReadDatetime(limitsCpu.PositionWrite + addPositionIndex);
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
                    logger.Error(ex.Message, "CheckDateTimeCpu() --> DateTimWritePp=" + valueCheck);
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

        // TEST--------------------TEST-----------------
        public void TESTaddToBd()
        {
            Model_dateTime modelDTBase = new Model_dateTime();
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(10000);
                modelDTBase.Id_DateTime = 423432 + i;
                modelDTBase.DateTime = DateTime.Now;

                using (SqlContext db = new SqlContext())
                {
                    try
                    {
                        db.model_dateTime.Add(modelDTBase);
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }
    }
}

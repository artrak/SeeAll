using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SeeAll.control.communication;
using SeeAll.model;

namespace SeeAll.control
{
    public class WorkingPLC
    {
        public bool statusConnectionSql { get; set; } 
        public bool workingPLCStatus { get; set; } 
        public long idMaxSql { get; set; } 
        public int writeIndex { get; set; }
        public int readIndex { get; set; }

        private int addPositionIndex { get; set; }
        private bool firstStartWorkingPLC { get; set; }
        private long noConnectionLimitTime { get; set; } 
        private bool isNoConnectionTime { get; set; }
        private DateTime tempNoConnectionTime { get; set; }

        public WorkingPLC()
        {
            statusConnectionSql = false;
            workingPLCStatus = false;
            idMaxSql = 0;
            writeIndex = 0;
            readIndex = 0;

            addPositionIndex = 1;
            firstStartWorkingPLC = false;
            noConnectionLimitTime = 600000;
            isNoConnectionTime = true;
            tempNoConnectionTime = DateTime.Now;

            LoadingPLC loadPLC = new LoadingPLC();
            while (true)
            {
                if (!WorkPLC(loadPLC))
                {
                    break;
                }
            } // 0 --> error                
        }

        private bool WorkPLC(LoadingPLC loadPLC)
        {
            // START work with PLC
            // Step 1
            int nextIdPLC = GetNextIdPLC(loadPLC);
            if (nextIdPLC == -1)
            {
                return false;
            }

            // Step 2
            Model_dateTime modelDateTime = WriteIndexPLC(loadPLC, nextIdPLC);
            if (modelDateTime == null)
            {
                return false;
            }
            
            // Step 3
            WriteDateTimeSql(loadPLC, modelDateTime, nextIdPLC);

            // FINISH work with PLC
            return true;    // all right
        }

        private int GetNextIdPLC(LoadingPLC loadPLC)
        {
            LimitsCpu limitsCpu = loadPLC.ReadLimits();

            if (limitsCpu == null)
            {
                return -1;
            }                        // error connect

            if ((limitsCpu.PositionRead < limitsCpu.PositionMin) || 
                (limitsCpu.PositionRead > limitsCpu.PositionMax))
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
                    FindMaxIdSql();
                    return -1;
                }
                else
                {
                    return newIndexDb;
                }
            }
        }

        private Model_dateTime WriteIndexPLC(LoadingPLC loadPLC, int newIndexDb)
        {
            // loading data all PLC --------
            Model_dateTime modelDateTime = loadPLC.ReadDateTime(newIndexDb);
            if (modelDateTime.Id_DateTime == -1)
            {
                loadPLC.WritePositionLimitsCpu(newIndexDb);   // write PositionWrite (+1)
                return null;
            }
            //------------------------------
            idMaxSql = modelDateTime.Id_DateTime;  // save ID 

            return modelDateTime;
        }

        private void WriteDateTimeSql(LoadingPLC loadPLC, Model_dateTime modelDateTime, int newIndexDb)
        {
            try
            {
                SaveDateTimeSql(loadPLC, modelDateTime, newIndexDb);
            }
            catch (Exception ex)
            {
                SaveDateTimeSql(loadPLC, modelDateTime, newIndexDb);
            }
        }

        private void SaveDateTimeSql(LoadingPLC loadPLC, Model_dateTime modelDateTime, int newIndexDb)
        {
            using (SqlContext db = new SqlContext())
            {
                statusConnectionSql = true;
                try
                {
                    // check id for duplicates
                    Model_dateTime model_DateTime = db.model_dateTime.FirstOrDefault(p => p.Id_DateTime == modelDateTime.Id_DateTime);

                    if (model_DateTime == null)
                    {
                        // add to BD SQL -------------------------WRITE-----<-----<-----<
                        db.model_dateTime.Add(modelDateTime);
                        db.SaveChanges();
                    }
                    loadPLC.WritePositionLimitsCpu(newIndexDb);   // write PositionRead
                }
                catch (Exception ex)
                {
                    //TODO need a logger
                    statusConnectionSql = false;
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
                {
                    return limitsCpu.PositionRead;
                }
            }
            // not to do
            if (limitsCpu.PositionRead == limitsCpu.PositionWrite)
            {
                return limitsCpu.PositionRead;  // - addPositionIndex;
            }
            // ok
            if (limitsCpu.PositionRead >= limitsCpu.PositionMin - addPositionIndex)
            {
                if (CheckDateTimePLC(loadPLC, limitsCpu))
                {
                    return limitsCpu.PositionWrite + addPositionIndex;  // idex > Write --> new value (will Write < Read)
                }
                else
                {
                    return limitsCpu.PositionRead + addPositionIndex;   // (will Write > Read)
                }   
            }
            else
            {
                return limitsCpu.PositionMin - addPositionIndex; // bad
            }
        }

        private void FindMaxIdSql()
        {
            if (idMaxSql == 0)
            {
                using (var db = new SqlContext())
                {
                    statusConnectionSql = true;
                    try
                    {
                        // find MAX ID in the SQL
                        idMaxSql = db.model_dateTime.Max(p => p.Id_DateTime);
                    }
                    catch (Exception ex)
                    {
                        //TODO need a logger
                        Thread.Sleep(Properties.Settings.Default.timerException);
                        statusConnectionSql = false;
                    }
                }
            }
        }

        /// <summary>
        /// positionWrite > positionRead (1 cycle)
        /// </summary>
        /// <returns>
        /// false - continue positionRead (+2)
        /// true - continue positionWrite (+2)
        /// </returns>
        private bool CheckDateTimePLC(LoadingPLC loadPLC, LimitsCpu limitsCpu)
        {
            //----------------------------------------------------------------
            // loading all dates PLC
            var tempPositionRead = loadPLC.ReadDateTime(limitsCpu.PositionRead);
            var tempPositionWrite = loadPLC.ReadDateTime(limitsCpu.PositionWrite);

            if ((tempPositionRead == null) || (tempPositionWrite == null))
            {
                return false;
            }

            long idPositionRead = tempPositionRead.Id_DateTime;
            long idPositionWrite = tempPositionWrite.Id_DateTime;

            // no connections > 1 hour
            if ((!isNoConnectionTime) && (idPositionRead < idPositionWrite))
            {
                // after (перегнал)                
                return false;
            }
            //----------------------------------------------------------------

            long idPositionReadAfterWrite;
            if ((limitsCpu.PositionWrite + addPositionIndex > limitsCpu.PositionMax) || (limitsCpu.PositionWrite < limitsCpu.PositionMin))
            {
                var tempIdPositionReadAfterWrite = loadPLC.ReadDateTime(limitsCpu.PositionMin);
                if (tempIdPositionReadAfterWrite == null)
                {
                    return false;
                }
                else
                {
                    idPositionReadAfterWrite = tempIdPositionReadAfterWrite.Id_DateTime;
                }
            }
            else
            {
                var tempPositionReadAfterWrite = loadPLC.ReadDateTime(limitsCpu.PositionWrite + addPositionIndex);
                if (tempPositionReadAfterWrite == null)
                {
                    return false;
                }
                else
                {
                    idPositionReadAfterWrite = tempPositionReadAfterWrite.Id_DateTime;
                }
            }

            if (idPositionReadAfterWrite == 0)
            {
                // before
                return false;
            }

            // no connections > 1 hour
            if (isNoConnectionTime)
            {
                // after (перегнал)
                if ((idPositionRead < idPositionWrite) && (idPositionRead > idPositionReadAfterWrite))
                {
                    // true - Is present to SQL (dtdtReadAfterWrite)
                    if (!CheckIsPresent(idPositionReadAfterWrite))
                    {
                        return true;
                    }
                }
            }
            // before
            return false;
        }

        /// <summary>
        /// Проверка на наличие записи в SQL
        /// </summary>
        /// <param name="checkValue"></param>
        /// <returns>true - present, false - no present</returns>
        private bool CheckIsPresent(long checkValue)
        {
            using (SqlContext db = new SqlContext())
            {
                try
                {
                    Model_dateTime modelDateTime = db.model_dateTime.FirstOrDefault(p => p.Id_DateTime == checkValue);
                    statusConnectionSql = true;
                    if (modelDateTime == null)
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

        /// <summary>
        /// Makes the status, for a long time there was no connection with the CPU
        /// isNoConnectionTime = false - No Connection
        /// </summary>
        //private void NoConnectionTime(LoadingPLC loadPLC)
        //{
        //    if (loadPLC.statusConnection)
        //    {
        //        isNoConnectionTime = false;
        //    }
        //    else
        //    {
        //        // no connection to CPU   
        //        long differenceTime = Convert.ToInt64(DateTime.Now.Subtract(tempNoConnectionTime).TotalSeconds);
        //        if (differenceTime >= noConnectionLimitTime)
        //        {
        //            isNoConnectionTime = true;
        //        }
        //    }
        //}
    }
}

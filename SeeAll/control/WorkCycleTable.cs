using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using SeeAll.control.communication;
using SeeAll.model;

namespace SeeAll.control
{
    class WorkCycleTable
    {
        ///private long idCpu = 0;
        private List<int> transitionSmenPattern = new List<int>();
        private NLog.Logger logger = LogManager.GetCurrentClassLogger();
        public static long idFinal = 0;
        private DateTime dateTimeDefoult;

        /// <summary>
        /// 1. if MAX ID_wheel > MAX ID_cycle => OK (go to work)
        /// 2. if marker SMENA time => (go to work2)
        /// 3. 
        /// </summary>
        public WorkCycleTable()
        {
            dateTimeDefoult = new DateTime(1985, 1, 1, 1, 1, 1);    // datetime for error

            string[] result = Properties.Settings.Default.shiftTransition.ToString().Split(',');
            foreach (var itemHour in result)
            {
                transitionSmenPattern.Add(Convert.ToInt32(itemHour));   // itemHour = 7,15,23
            }

            WorkCycle();
        }

        private void WorkCycle()
        {
            while (true)
            {
                // STOP
                if (Form1.stopThreadAll) return;

                using (var db = new SqlContext())
                {
                    try
                    {
                        Thread.Sleep(Properties.Settings.Default.timerWorkCycle);
                        long idLast_DateTime = CheckEqualityMaxId(db);

                        WorkCpu.statusConnSql = true;
                        switch (idLast_DateTime)
                        {
                            case -1:
                                NoRecords_CalcTransitionCmena(db, idLast_DateTime);    // return "-1" - (idDateTimeMax = idCycleMax) новых id нет для переноса, проверка на переход смен
                                break;
                            case -2: continue;                                          // return "-2" - нет записей в таб. Model_dateTime (таб. пустая)
                            case -3:
                                NoMaxIdTableCycle_CalcTransitionCmena(db);             // return "-3" - нет записей в таб. Model_CycleDateTime (таб. пустая)
                                break;
                            default:
                                YesRecords_CalcTransitionCmena(db, idLast_DateTime);  // return id - последний записанный id (можно переносить новые id с таб. Model_dateTime в таб. Model_CycleDateTime 
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message, " WorkCycle()");
                        Thread.Sleep(Properties.Settings.Default.timerException);
                        WorkCpu.statusConnSql = false;
                    }
                }
                //System.Diagnostics.Debug.Print(++flagDebug + " ----- " + System.DateTime.Now + "\n");
            }
        }

        /// <summary>
        /// Проверка на новые ID
        /// return idCycleMax - последний записанный id (можно переносить новые id с таб. Model_dateTime в таб. Model_CycleDateTime 
        /// return -1 - новых id нет для переноса (idDateTimeMax = idCycleMax)
        /// return -2 - нет записей в таб. Model_dateTime 
        /// return -3 - нет записей в таб. Model_CycleDateTime 
        /// return -4 - расчет простоя до и после начала смен
        /// </summary>
        /// <returns></returns>
        private long CheckEqualityMaxId(SqlContext db)
        {
            long idDateTimeMax;
            long idDateTimeMax_last;
            long idCycleMax;
            try
            {
                idDateTimeMax = db.model_dateTime.DefaultIfEmpty().Max(p => p.Id_DateTime);     // find MAX ID of table dateTime in the SQL
                //TODO corect for downtime 05/02/2018
                idDateTimeMax_last = db.model_dateTime.DefaultIfEmpty()        // find MAX table ID_cycle
                    .Where(p => p.Id_DateTime < idDateTimeMax)
                    .Max(p => p.Id_DateTime);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, "CheckEqualityMaxId(SqlContext db) --> idDateTimeMax");
                WorkCpu.statusConnSql = false;
                Thread.Sleep(Properties.Settings.Default.timerException);
                return -2;  // no records
            }
            try
            {
                // find MAX table ID_cycle
                //.Where(p => p.FlagDownTimeSmena == false)
                idCycleMax = db.model_CycleDateTime.DefaultIfEmpty().Max(p => p.Id_DateTime);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, "CheckEqualityMaxId(SqlContext db) --> idCycleMax");
                WorkCpu.statusConnSql = false;
                Thread.Sleep(Properties.Settings.Default.timerException);
                return -3;  // no records
            }
            if (idDateTimeMax_last > idCycleMax)
            {
                try
                {
                    // get next id of Id_DateTime after idCycleMax
                    return db.model_dateTime.DefaultIfEmpty()
                           .Where(p => p.Id_DateTime > idCycleMax)
                           .Min(p => p.Id_DateTime);
                    //return db.model_dateTime.FirstOrDefault(idDb => idDb.Id_DateTime > idCycleMax).Id_DateTime;         // yes, переносим с таб. в таб. (нужна проверка на переход смен) 
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message, "CheckEqualityMaxId(SqlContext db)");
                    WorkCpu.statusConnSql = false;
                    Thread.Sleep(Properties.Settings.Default.timerException);
                    return -2;  // no records
                }
            }
            else
                return -1;          // no next records (нужна проверка на переход смен)
        }



        /// <summary>
        /// case -3  нет записей в таб. Model_CycleDateTime (таб. пустая)
        /// </summary>
        private void NoMaxIdTableCycle_CalcTransitionCmena(SqlContext db)
        {
            Model_CycleDateTime m_CycleDateTime = new Model_CycleDateTime();

            m_CycleDateTime.Id_DateTime = db.model_dateTime.Min(p => p.Id_DateTime);
            m_CycleDateTime.CycleTime = "00:00:00";
            db.model_CycleDateTime.Add(m_CycleDateTime);

            SaveChangesSql(db, m_CycleDateTime.Id_Cycle, "NoMaxIdTableCycle_CalcTransitionCmena(SqlContext db)");
        }

        private bool isFirstNoRecords_CalcTransitionCmena = false;
        /// <summary>
        /// return "-1" - (idDateTimeMax = idCycleMax) 
        /// новых id нет для переноса, проверка на переход смен
        /// </summary>
        /// <param name="db">пердаем SqlContext</param>
        /// <param name="idCycleMax">миксим. id а таб. Model_CycleDateTime</param>
        private void NoRecords_CalcTransitionCmena(SqlContext db, long idMaxCycle)
        {
            // PositionRead == PositionWrite ==> считаем простой;
            if ((!LoadCpu.checkWriteReadEqually) && (isFirstNoRecords_CalcTransitionCmena))
            {
                isFirstNoRecords_CalcTransitionCmena = true;
                return;
            }

            long idCycleMax = db.model_CycleDateTime.DefaultIfEmpty()
                    //.Where(p => p.FlagDownTimeSmena == false)
                    .Max(p => p.Id_DateTime);

            Model_dateTime m_dateTime = db.model_dateTime.FirstOrDefault(idDb => idDb.Id_DateTime > idCycleMax);
            if (m_dateTime == null)
            {
                return;
            }
            List<DateTime> downtimeArr = calcTransitionCmena(db, m_dateTime.DateTime, DateTime.Now);
            CalcDowntimeCmenaToSql(db, idMaxCycle, idCycleMax, downtimeArr, false);  // calc and insert DowntimeCmena to BD SQL
        }

        /// <summary>
        /// return id - последний записанный id 
        /// (можно переносить новые id с таб. Model_dateTime в таб. Model_CycleDateTime 
        /// </summary>
        private void YesRecords_CalcTransitionCmena(SqlContext db, long idMaxCycle)
        {
            var lastDateTime = db.model_dateTime.FirstOrDefault(oldDt => oldDt.Id_DateTime == idMaxCycle);
            if (lastDateTime == null)
            {
                // when the transition shifts 
                // if nextDateTime = 7, 15, 23 hours there + 1 sec.
                long tempMaxCycle = idMaxCycle - 1;
                lastDateTime = db.model_dateTime.FirstOrDefault(oldDt => oldDt.Id_DateTime == tempMaxCycle);
                if (lastDateTime == null) return;
            }

            long min_DateTime;
            try
            {
                min_DateTime = db.model_dateTime.DefaultIfEmpty()
                                           .Where(p => p.Id_DateTime > lastDateTime.Id_DateTime)
                                           .Min(p => p.Id_DateTime);
            }
            catch (Exception)
            {
                return;
            }
            if (min_DateTime == null)
            {
                return;
            }
            var next_DateTime = db.model_dateTime.FirstOrDefault(nextDt => nextDt.Id_DateTime == min_DateTime);
            if (next_DateTime == null) return;

            // if nextDateTime = 7, 15, 23 hours there + 1 sec.
            next_DateTime.DateTime = protectingTransitionSmena(next_DateTime.DateTime);

            List<DateTime> downtimeArr = calcTransitionCmena(db, lastDateTime.DateTime, next_DateTime.DateTime);
            CalcDowntimeCmenaToSql(db, lastDateTime.Id_DateTime, next_DateTime.Id_DateTime, downtimeArr, true);
        }

        /// <summary>
        /// определение времени простоев между двумя записями учитывая переходы смен
        /// </summary>
        /// <param name="db"></param>
        /// <param name="idCycleMax"></param>
        /// <returns>Масив DateTime - начало, переходы смен, конец</returns>
        private List<DateTime> calcTransitionCmena(SqlContext db, DateTime lastDateTime, DateTime nextDateTime)
        {
            List<DateTime> downtimeArr = new List<DateTime>();
            downtimeArr.Add(lastDateTime);

            DateTime flagLastDateTime = lastDateTime;
            DateTime flagNextDateTime = lastDateTime;
            //DateTime smenPatternDateTime;
            int flagNextDay = 0;
            while (true)
            {
                foreach (int item in transitionSmenPattern)
                {
                    // Собираем переход следующей смены
                    flagNextDateTime = new DateTime(flagLastDateTime.Year, flagLastDateTime.Month, flagLastDateTime.Day, item, 0, 0);
                    flagNextDateTime = flagNextDateTime.AddDays(flagNextDay);  // + Day (+ flagNextDay)

                    if (flagNextDateTime >= nextDateTime)
                    {
                        // The time don't may be more current time.
                        if ((flagNextDateTime > DateTime.Now) && (downtimeArr.Count > 1))
                        {
                            return downtimeArr;
                        }
                        // последняя запись
                        downtimeArr.Add(nextDateTime);
                        return downtimeArr;
                    }
                    if ((flagNextDateTime > flagLastDateTime))
                    {
                        flagNextDay = 0;
                        // время до начала следующей смены
                        downtimeArr.Add(flagNextDateTime);
                        flagLastDateTime = flagNextDateTime;
                    }
                }
                flagNextDay = 1;
            }
        }

        // calc and insert DowntimeCmena to BD SQL
        private void CalcDowntimeCmenaToSql(SqlContext db, long lastId, long nextId, List<DateTime> downtimeArr, bool flagYesRecords)
        {
            for (int i = 0; i < downtimeArr.Count - 1; i++)
            {
                if ((!flagYesRecords) && (i == downtimeArr.Count - 1)) return;  // ели нет последнего колеса, выход, не записывая первую запись

                Model_CycleDateTime mCycleDateTime = new Model_CycleDateTime();

                // признак простоя перехода смен
                foreach (var itemSmenPattern in transitionSmenPattern)
                {
                    if ((downtimeArr[i].Hour == itemSmenPattern) && (downtimeArr[i].Second == 0) && (downtimeArr[i].Minute == 0))
                    {
                        mCycleDateTime.FlagDownTimeSmena = true;
                        break;
                    }
                    else
                    {
                        mCycleDateTime.FlagDownTimeSmena = false;
                    }
                }

                mCycleDateTime.Id_DateTime = Convert.ToInt64(
                                                                LoadCpu.normalIntToString(downtimeArr[i].Year)
                                                                + LoadCpu.normalIntToString(downtimeArr[i].Month)
                                                                + LoadCpu.normalIntToString(downtimeArr[i].Day)
                                                                + LoadCpu.normalIntToString(downtimeArr[i].Hour)
                                                                + LoadCpu.normalIntToString(downtimeArr[i].Minute)
                                                                + LoadCpu.normalIntToString(downtimeArr[i].Second)
                                                                );

                //TODO проверка на дубликаты в БД
                var lastDateTime = db.model_CycleDateTime.FirstOrDefault(oldDt => oldDt.Id_DateTime == mCycleDateTime.Id_DateTime);
                //var lastDateTime = db.model_CycleDateTime
                //    .Where(p => p.Id_DateTime == mCycleDateTime.Id_DateTime)
                //    .Where(p => p.FlagDownTimeSmena == false)
                //    .FirstOrDefault();

                if (lastDateTime != null)
                {
                    // если Id существует в БД  // ПРОВЕРКА НА ДУБЛИКАТЫ!!!!!
                    continue;
                }

                TimeSpan downtime = downtimeArr[i + 1].Subtract(downtimeArr[i]);
                mCycleDateTime.CycleTime = downtime.ToString("hh':'mm':'ss");

                // > 1 min (in sec) - простой
                if (downtime > TimeSpan.FromSeconds(Properties.Settings.Default.timerStandartCycle))
                {
                    mCycleDateTime.DownTime =
                        (downtime - TimeSpan.FromSeconds(Properties.Settings.Default.timerStandartCycle))
                        .ToString("hh':'mm':'ss");
                }

                //если меньше 2 min тогда микропростой
                if (downtime < TimeSpan.FromSeconds(Properties.Settings.Default.timerMicroDowntime))    // 2 min
                    mCycleDateTime.Microsimple = true;
                else
                    mCycleDateTime.Microsimple = false;

                db.model_CycleDateTime.Add(mCycleDateTime);
                //db.SaveChanges();
                SaveChangesSql(db, mCycleDateTime.Id_Cycle, "CalcDowntimeCmenaToSql(SqlContext db, long lastId, long nextId, List<DateTime> downtimeArr, bool flagYesRecords)");
            }
        }
        /// <summary>
        /// Writing data into the database SQL
        /// </summary>
        private void SaveChangesSql(SqlContext db, long id, string methodNameForLog)
        {
            try
            {
                db.SaveChanges();
                WorkCpu.statusConnSql = true;
                idFinal = id;
            }
            catch (Exception ex)
            {
                Logger.writeError(ex, " --> " + methodNameForLog);
                Thread.Sleep(Properties.Settings.Default.timerException);
                WorkCpu.statusConnSql = false;
                throw ex;
            }
        }

        // if nextDateTime = 7, 15, 23 hours there + 1 sec.
        private DateTime protectingTransitionSmena(DateTime smenaDateTime)
        {
            foreach (int hourItem in transitionSmenPattern)
            {
                if ((smenaDateTime.Hour == hourItem) && (smenaDateTime.Minute == 0) && (smenaDateTime.Second == 0))
                {
                    smenaDateTime = smenaDateTime.AddSeconds(1);  // + 1 sec.
                }
            }
            return smenaDateTime;
        }
    }
}
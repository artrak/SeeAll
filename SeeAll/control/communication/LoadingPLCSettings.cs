using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeAll.control.communication
{
    public static class LoadingPLCSettings
    {
        public static int byteStep;                                 // step in bytes
        public static string dbPLC;                                 
        public static string dbwPLC;
        public static int timeWaitForOpenConnectionCpu;             // waiting time for an open Cpu connection
        public static int numberOfConnectionAttempts;               // number of connection attempts

        static LoadingPLCSettings()
        {
            byteStep = 12;
            dbPLC = "DB";
            dbwPLC = ".DBW2";
            timeWaitForOpenConnectionCpu = 100;
            numberOfConnectionAttempts = 5;
        }
    }
}

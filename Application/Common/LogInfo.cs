using log4net;
using System;

namespace CommonData
{
   public class LogInfo
    {

        public static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //public static readonly log4net.ILog log = log4net.LogManager.GetLogger("InOutInfoLogger");
        public static void LogException(Exception ex)
        {
            log.Error(ex.ToString());
        }
        public static void Info(string pInput)
        {
            log.Info(pInput);
        }
    }
}

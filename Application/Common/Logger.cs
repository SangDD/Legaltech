namespace Common
{
	using System;
	using System.Reflection;
	using log4net;

	public static class Logger
	{
        //private static readonly ILog s_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ILog s_log = LogManager.GetLogger("LogError");
        public static ILog Log()
		{
			return s_log;
		}

		public static void LogException(Exception ex)
		{
			s_log.Error(ex.ToString());
		}
 
    }
}

namespace Common
{
	using System;
	using System.Configuration;

	public class Configuration
	{
		public static string connectionString;
        public static string Host;

        public static void GetConfigAppSetting()
		{
			try
			{
                connectionString = ConfigurationManager.ConnectionStrings["ConnectionStringDB"].ConnectionString;
                Host = CommonFuc.GetConfig("HostLegal");
            }
            catch (Exception ex)
			{
				Logger.LogException(ex);
			}
		}
 
	}
}

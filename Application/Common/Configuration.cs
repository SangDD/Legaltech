namespace CommonData
{
	using System;
	using System.Configuration;

	public class Configuration
	{
        public static string connectionString;

		public static void GetConfigAppSetting()
		{
			try
			{
                connectionString = ConfigurationManager.ConnectionStrings["ConnectionStringDB"].ConnectionString;
			}
			catch (Exception ex)
			{
                LogInfo.LogException(ex);
			}
		}

		public static string GetConnectionString()
		{
			return connectionString;
		}
	}
}

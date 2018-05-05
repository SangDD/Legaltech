namespace Common
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
                connectionString = ConfigurationManager.ConnectionStrings["ConnectionStringDBKuongNgan"].ConnectionString;
			}
			catch (Exception ex)
			{
				Logger.LogException(ex);
			}
		}
 
	}
}

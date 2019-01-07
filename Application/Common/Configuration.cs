namespace Common
{
	using System;
	using System.Configuration;

	public class Configuration
	{
		public static string connectionString;
        public static string Host;
        public static string LinkPathlaw;


        public static void GetConfigAppSetting()
		{
			try
			{
                connectionString = ConfigurationManager.ConnectionStrings["ConnectionStringDB"].ConnectionString;
                Host = CommonFuc.GetConfig("HostLegal");
                LinkPathlaw = CommonFuc.GetConfig("LinkLegal");
                Common.BaseDir = System.Configuration.ConfigurationManager.AppSettings["BaseDir"].ToString();
                Common.BaseUrl = System.Configuration.ConfigurationManager.AppSettings["BaseUrl"].ToString();
            }
            catch (Exception ex)
			{
				Logger.LogException(ex);
			}
		}
 
	}
}

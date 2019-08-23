namespace Common
{
	using System;
	using System.Configuration;

	public class Configuration
	{
		public static string connectionString;
        public static string Host;
        public static string LinkPathlaw;

        public static string cellphone_business;
        public static string urlweb_business;
        public static string emailfrom_business;
        public static string namereply;
        public static string address1;
        public static string address2;

        public static void GetConfigAppSetting()
		{
			try
			{
                connectionString = ConfigurationManager.ConnectionStrings["ConnectionStringDB"].ConnectionString;
                Host = CommonFuc.GetConfig("HostLegal");
                LinkPathlaw = CommonFuc.GetConfig("LinkLegal");

                cellphone_business = CommonFuc.GetConfig("Cellphone_Business");
                urlweb_business = CommonFuc.GetConfig("UrlWeb_Business");
                emailfrom_business = CommonFuc.GetConfig("EMailFrom_Business");
                namereply = CommonFuc.GetConfig("NameReply");
                address1 = CommonFuc.GetConfig("Address1");
                address2 = CommonFuc.GetConfig("Address2");

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

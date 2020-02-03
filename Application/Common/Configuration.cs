namespace Common
{
    using System;
    using System.Configuration;

    public class Configuration
    {
        public static string connectionString;
        //public static string Host;
        public static string LinkPathlaw;

        public static string cellphone_business;
        public static string urlweb_business;
        public static string emailfrom_business;
        public static string namereply;
        public static string address1;
        public static string address2;

        public static string Encripted;

        public static string EMailPass;
        public static string EMailPass_Business;


        public static void GetConfigAppSetting()
        {
            try
            {
                Encripted = CommonFuc.GetConfig("Encripted");
                if (Encripted == "1")
                {
                    connectionString = CommonFuc.DecryptString(ConfigurationManager.ConnectionStrings["ConnectionStringDB"].ConnectionString);
                    EMailPass = CommonFuc.DecryptString(CommonFuc.GetConfig("EMailPass"));
                    EMailPass_Business = CommonFuc.DecryptString(CommonFuc.GetConfig("EMailPass_Business"));

                    //Common.BaseDir = CommonFuc.DecryptString(System.Configuration.ConfigurationManager.AppSettings["BaseDir"].ToString());
                }
                else
                {
                    EMailPass = CommonFuc.GetConfig("EMailPass");
                    EMailPass_Business = CommonFuc.GetConfig("EMailPass_Business");
                    connectionString = ConfigurationManager.ConnectionStrings["ConnectionStringDB"].ConnectionString;
                }

                Common.BaseDir = System.Configuration.ConfigurationManager.AppSettings["BaseDir"].ToString();


                //Host = CommonFuc.GetConfig("HostLegal");
                LinkPathlaw = CommonFuc.GetConfig("LinkLegal");

                cellphone_business = CommonFuc.GetConfig("Cellphone_Business");
                urlweb_business = CommonFuc.GetConfig("UrlWeb_Business");
                emailfrom_business = CommonFuc.GetConfig("EMailFrom_Business");
                namereply = CommonFuc.GetConfig("NameReply");
                address1 = CommonFuc.GetConfig("Address1");
                address2 = CommonFuc.GetConfig("Address2");

                Common.BaseUrl = System.Configuration.ConfigurationManager.AppSettings["BaseUrl"].ToString();
                Common.BaseDir = System.Configuration.ConfigurationManager.AppSettings["BaseDir"].ToString();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

    }
}

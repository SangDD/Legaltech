using Common;
using Common.CommonData;
using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApps.CommonFunction
{
    public class AppsCommon
    {
        public static string GetCurrentLang()
        {
            try
            {
                var culture = "";
                var httpCookie = HttpContext.Current.Request.Cookies["language"];
                var queryLanguage = "";
                if (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["lang"]))
                {
                    queryLanguage = HttpContext.Current.Request.QueryString["lang"].ToString().ToUpper();
                    if (queryLanguage == "EN-GB")
                    {
                        culture = "en-GB";
                    }
                }
                else
                {
                    if (httpCookie != null)
                    {
                        culture = httpCookie.Value;
                    }
                    else
                    {
                        culture = ConfigurationManager.AppSettings["DefaultLang"].ToString();
                    }
                }
                if (culture.ToUpper().Equals("EN-GB"))
                    culture = Language.LangEN;
                else if (culture.ToUpper().Equals("EN-US") || culture.ToUpper().Equals("VI-VN"))
                    culture = Language.LangVI;
                else culture = Language.LangVI;
                return culture.ToUpper();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Language.LangVI;
            }
        }
    }
  
     
    public enum LangCode
    {
        VI_VN,
        EN_US
    }

    public static class FileAutoVersioningHelper
    {
        private readonly static string _version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public static string GeneratePath(string fileName)
        {
            try
            {
                return string.Format("{0}?v={1}", fileName, _version);
            }
            catch
            {
                return string.Format("{0}?v={1}", fileName, DateTime.Now.ToString("dd-MM-yyyy HH"));
            }
        }
    }

    public static class UrlHelperExtensions
    {
        public static string Stylesheet(this UrlHelper helper, string fileName)
        {

            return helper.Content(string.Format("{0}", FileAutoVersioningHelper.GeneratePath(fileName)));
        }

        public static string Script(this UrlHelper helper, string fileName)
        {
            return helper.Content(string.Format("{0}", FileAutoVersioningHelper.GeneratePath(fileName)));
        }
    }


    public class DaiDienChuDon
    {
        public static string DaiDienPL = "DDPL";
        public static string DaiDienSH = "DDSH";
        public static string DaiDienUyQuyen = "UQUQ";
    }
}
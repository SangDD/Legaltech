using Common;
using Common.CommonData;
using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BussinessFacade.ModuleTrademark;
using System.Data;

namespace WebApps.CommonFunction
{
    public class AppsCommon
    {

        #region HungTD: các biến lưu số lượng cảnh báo của phần quản lý đơn

        /// <summary>
        /// lưu tạm
        /// </summary>
        public static int W_LUUTAM = 0;

        /// <summary>
        /// đơn chờ phân loại
        /// </summary>
        public static int W_CHOPHANLOAI = 0;

        /// <summary>
        /// đã phân cho l;uật sư
        /// </summary>
        public static int W_PHANCHOLUATSU = 0;

    
        /// <summary>
        /// Luật sư đã comfirm đơn
        /// </summary>
        public static int W_LUATSUDACOMFIRM = 0;

      
        /// <summary>
        /// Chờ khách hàng comfirm
        /// </summary>
        public static int W_CHOKHACHHANGCOMFIRM = 0;

        /// <summary>
        /// Khách hàng đã comfirm
        /// </summary>
        public static int W_KHACHHANGDACOMFIRM = 0;

        /// <summary>
        /// 
        /// </summary>
        public static int W_DAGUILENCUC = 0;


        #endregion


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

        public static String RenderRazorViewToString(ControllerContext controllerContext, String viewName, Object model = null)
        {
            try
            {
                controllerContext.Controller.ViewData.Model = model;

                using (var sw = new StringWriter())
                {
                    var ViewResult = ViewEngines.Engines.FindPartialView(controllerContext, viewName);
                    var ViewContext = new ViewContext(controllerContext, ViewResult.View, controllerContext.Controller.ViewData, controllerContext.Controller.TempData, sw);
                    ViewResult.View.Render(ViewContext, sw);
                    ViewResult.ViewEngine.ReleaseView(controllerContext, ViewResult.View);
                    return sw.GetStringBuilder().ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return "";
            }
        }

        public static void GetWarningData(string p_usertype)
        {
            try
            {
                Application_Header_BL _AppBL = new Application_Header_BL();
                DataSet _Ds = new DataSet();
                _Ds = _AppBL.GetWarningData(p_usertype);
                if(_Ds.Tables.Count> 0 && _Ds.Tables[0].Rows.Count > 0)
                {
                    W_LUUTAM = Convert.ToInt32(_Ds.Tables[0].Rows[0]["W_LUUTAM"].ToString());
                    W_CHOPHANLOAI = Convert.ToInt32(_Ds.Tables[0].Rows[0]["W_CHOPHANLOAI"].ToString());
                    W_PHANCHOLUATSU = Convert.ToInt32(_Ds.Tables[0].Rows[0]["W_PHANCHOLUATSU"].ToString());
                    W_LUATSUDACOMFIRM = Convert.ToInt32(_Ds.Tables[0].Rows[0]["W_LUATSUDACOMFIRM"].ToString());
                    W_CHOKHACHHANGCOMFIRM = Convert.ToInt32(_Ds.Tables[0].Rows[0]["W_CHOKHACHHANGCOMFIRM"].ToString());
                    W_KHACHHANGDACOMFIRM = Convert.ToInt32(_Ds.Tables[0].Rows[0]["W_KHACHHANGDACOMFIRM"].ToString());
                    W_DAGUILENCUC = Convert.ToInt32(_Ds.Tables[0].Rows[0]["W_DAGUILENCUC"].ToString());
                }
            }
            catch (Exception ex)
            {
 
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
        public static string DaiDienUyQuyen = "DDUQ";
    }

       

}
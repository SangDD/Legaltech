﻿using Common;
using Common.CommonData;
using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BussinessFacade.ModuleTrademark;
using System.Data;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;

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


    public class ConvertData
    {

        #region Định dạng datetime

        public const string strDate = "dd/MM/yyyy";
        public const string strDate_Time = "dd/MM/yyyy HH:mm";

        public static string ConvertDate2String(DateTime p_date)
        {
            try
            {
                return p_date.ToString(strDate);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return "";
            }
        }

        public static DateTime ConvertString2Date(string str)
        {
            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.CurrentCulture;
            try
            {
                return DateTime.ParseExact(str, strDate, provider); // strDate 
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return DateTime.MinValue;
            }
        }

        public static DateTime Convert_String_2DateTime(string p_str_datetime)
        {
            try
            {
                string[] _arr_au_date = p_str_datetime.Trim().Split('-');
                string[] _arr_time = _arr_au_date[0].Split(':');
                DateTime _dt_au = ConvertData.ConvertString2Date(_arr_au_date[1].Trim());
                DateTime _dt = new DateTime(_dt_au.Year, _dt_au.Month, _dt_au.Day, Convert.ToInt16(_arr_time[0]), Convert.ToInt16(_arr_time[1]), 0);
                return _dt;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return DateTime.MinValue;
            }
        }

        public static DateTime ConvertString2Date_dd_MM_yyyy(string str)
        {
            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.CurrentCulture;
            try
            {
                return DateTime.ParseExact(str, "dd-MM-yyyy", provider); // strDate 
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Convert string to datetime format dd/MM/yyyy HH:mm
        /// </summary>
        /// <param name="str">ví dụ 17/01/2015 09:10</param>
        /// <returns></returns>
        public static DateTime ConvertString2DateWithTime(string str)
        {
            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.CurrentCulture;
            try
            {
                return DateTime.ParseExact(str, strDate_Time, provider);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }


        public const string strDate_1 = "yyyy/MM/dd";
        public static string ConvertDate2String_yyyyMMdd(DateTime p_date)
        {
            try
            {
                return p_date.ToString(strDate_1);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return "";
            }
        }
        #endregion
         
        #region Convert DataTable

        public static void ConvertArrayListToDataTable(ArrayList arrayList, ref DataTable p_dt)
        {
            //DataTable dt = new DataTable();

            if (arrayList.Count != 0)
            {
                ConvertObjectToDataTableSchema(arrayList[0], ref p_dt);
                FillData(arrayList, p_dt);
            }

            //return p_dt;
        }

        public static void ConvertObjectToDataTableSchema(Object o, ref DataTable dt)
        {
            //DataTable dt = new DataTable();
            PropertyInfo[] properties = o.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                DataColumn dc = new DataColumn(property.Name);
                dc.DataType = property.PropertyType; dt.Columns.Add(dc);
            }
            //return dt;
        }

        private static void FillData(ArrayList arrayList, DataTable dt)
        {
            foreach (Object o in arrayList)
            {
                DataRow dr = dt.NewRow();
                PropertyInfo[] properties = o.GetType().GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    dr[property.Name] = property.GetValue(o, null);
                }
                dt.Rows.Add(dr);
            }
        }

        public static DataTable ConvertToDatatable<T>(IList<T> data, bool p_isAdd_STT = true)
        {
            try
            {
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
                DataTable table = new DataTable();
                for (int i = 0; i < props.Count; i++)
                {
                    PropertyDescriptor prop = props[i];
                    table.Columns.Add(prop.Name, prop.PropertyType);
                }

                int SoThuTu = 0;
                object[] values;
                if (p_isAdd_STT == true)
                {
                    table.Columns.Add("STT");
                    values = new object[props.Count + 1];
                }
                else
                    values = new object[props.Count];

                foreach (T item in data)
                {
                    SoThuTu++;
                    for (int i = 0; i < values.Length - 1; i++)
                    {
                        values[i] = props[i].GetValue(item);
                    }

                    if (p_isAdd_STT == true)
                    {
                        values[values.Length - 1] = SoThuTu.ToString();
                    }

                    table.Rows.Add(values);
                }
                return table;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataTable();
            }

        }

        public static DataSet ConvertToDataSet<T>(IList<T> data, bool p_isAdd_STT = true)
        {
            try
            {
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
                DataTable table = new DataTable();
                for (int i = 0; i < props.Count; i++)
                {
                    PropertyDescriptor prop = props[i];
                    table.Columns.Add(prop.Name, prop.PropertyType);
                }

                int SoThuTu = 0;
                object[] values;
                if (p_isAdd_STT == true)
                {
                    table.Columns.Add("STT");
                    values = new object[props.Count + 1];
                }
                else
                {
                    values = new object[props.Count];
                }

                foreach (T item in data)
                {
                    SoThuTu++;
                    for (int i = 0; i < values.Length - 1; i++)
                    {
                        values[i] = props[i].GetValue(item);
                    }

                    if (p_isAdd_STT == true)
                    {
                        values[values.Length - 1] = SoThuTu.ToString();
                    }

                    table.Rows.Add(values);
                }

                DataSet _ds = new DataSet();
                _ds.Tables.Add(table);
                return _ds;
            }
            catch (Exception)
            {
                return new DataSet();
            }

        }

        #endregion
    }
}
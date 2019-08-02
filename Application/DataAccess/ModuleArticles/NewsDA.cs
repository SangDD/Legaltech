using Common;
using ObjectInfos;
using Oracle.DataAccess.Client;
using System;
using System.Data;

namespace DataAccess.ModuleArticles
{
    public class NewsDA
    {

        public DataSet ArticlesGetById(decimal pID, string pLanguage)
        {
            try
            {
                DataSet ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_NEWS.PROC_NEW_GET_BY_ID",
                 new OracleParameter("P_ID", OracleDbType.Decimal, pID, ParameterDirection.Input),
                 new OracleParameter("P_LANGUAGE", OracleDbType.Varchar2, pLanguage, ParameterDirection.Input),
                 new OracleParameter("P_CUSOR", OracleDbType.RefCursor, ParameterDirection.Output));

                return ds;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }
        public DataSet ArticlesGetByCaseCode(string p_Case_Code, string pLanguage)
        {
            try
            {
                DataSet ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_NEWS.PROC_NEW_GET_BY_Case_Code",
                 new OracleParameter("p_Case_Code", OracleDbType.Varchar2, p_Case_Code, ParameterDirection.Input),
                 new OracleParameter("P_LANGUAGE", OracleDbType.Varchar2, pLanguage, ParameterDirection.Input),
                 new OracleParameter("P_CUSOR", OracleDbType.RefCursor, ParameterDirection.Output));

                return ds;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        

        public DataSet ArticlesGetByPage(string pLanguage, string pTitle, DateTime pNgayCongBo, int pStart, int pEnd, ref decimal pTotalRecord)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("P_TOTALRECORD", OracleDbType.Decimal, ParameterDirection.Output);
                DataSet ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_NEWS.PROC_NEW_GET_BY_PAGE",
                 new OracleParameter("P_LANGUAGE", OracleDbType.Varchar2, pLanguage, ParameterDirection.Input),
                 new OracleParameter("P_TITLE", OracleDbType.Varchar2, pLanguage, ParameterDirection.Input),
                 new OracleParameter("P_NGAYCONGBO", OracleDbType.Date, pNgayCongBo, ParameterDirection.Input),
                 new OracleParameter("P_START", OracleDbType.Int32, pStart, ParameterDirection.Input),
                 new OracleParameter("P_END", OracleDbType.Int32, pEnd, ParameterDirection.Input),
                 new OracleParameter("P_CUSOR", OracleDbType.RefCursor, ParameterDirection.Output),
                 paramReturn);
                pTotalRecord = Convert.ToDecimal(paramReturn.Value.ToString());
                return ds;

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public DataSet ArticlesGetPage(decimal pID, int pFrom, int pTo, string pLanguage, string pOrderBy, ref decimal pTotalRecord)
        {
            try
            {
                var paramReturn = new OracleParameter("P_RECORD", OracleDbType.Int32, ParameterDirection.Output);
                DataSet ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_NEWS.PROC_NEW_GET_PAGE",
                new OracleParameter("P_ID", OracleDbType.Decimal, pID, ParameterDirection.Input),
                new OracleParameter("P_FROM", OracleDbType.Int32, pFrom, ParameterDirection.Input),
                new OracleParameter("P_TO", OracleDbType.Int32, pTo, ParameterDirection.Input),
                new OracleParameter("P_LANGUAGE", OracleDbType.Varchar2, pLanguage, ParameterDirection.Input),
                new OracleParameter("P_ORDERBY", OracleDbType.Varchar2, pOrderBy, ParameterDirection.Input),
                new OracleParameter("P_CUSOR", OracleDbType.RefCursor, ParameterDirection.Output),
                paramReturn);
                pTotalRecord = Convert.ToDecimal(paramReturn.Value.ToString());
                return ds;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public decimal ArticlesInsert(NewsInfo pInfo)
        {
            try
            {
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_NEWS.PROC_NEWS_INSERT",
                    new OracleParameter("P_TITLE", OracleDbType.Varchar2, pInfo.Title, ParameterDirection.Input),
                    new OracleParameter("P_TITLE_EN", OracleDbType.Varchar2, pInfo.Title_EN, ParameterDirection.Input),
                    new OracleParameter("P_HEADER", OracleDbType.Varchar2, pInfo.Header, ParameterDirection.Input),
                    new OracleParameter("P_HEADER_EN", OracleDbType.Varchar2, pInfo.Header_EN, ParameterDirection.Input),
                    new OracleParameter("P_IMAGEHEADER", OracleDbType.Varchar2, pInfo.Imageheader, ParameterDirection.Input),
                    new OracleParameter("P_LANGUAGECODE", OracleDbType.Varchar2, pInfo.Languagecode, ParameterDirection.Input),
                    new OracleParameter("P_CONTENT", OracleDbType.Clob, pInfo.Content, ParameterDirection.Input),
                    new OracleParameter("P_CONTENT_EN", OracleDbType.Clob, pInfo.Content_En, ParameterDirection.Input),
                    new OracleParameter("P_STATUS", OracleDbType.Decimal, pInfo.Status, ParameterDirection.Input),
                    new OracleParameter("P_CATEGORIES_ID", OracleDbType.Varchar2, pInfo.Categories_Id, ParameterDirection.Input),
                    new OracleParameter("P_ARTICLES_TYPE", OracleDbType.Varchar2, pInfo.Articles_Type, ParameterDirection.Input),
                    new OracleParameter("P_CREATEDBY", OracleDbType.Varchar2, pInfo.Createdby, ParameterDirection.Input),
                    new OracleParameter("P_CREATEDDATE", OracleDbType.Date, pInfo.Createddate, ParameterDirection.Input),
                    new OracleParameter("P_COUNTRYID", OracleDbType.Decimal, pInfo.Country_Id, ParameterDirection.Input),
                    paramReturn);
                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal ArticlesUpdate(NewsInfo pInfo)
        {
            try
            {
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_NEWS.PROC_NEWS_UPDATE",
                    new OracleParameter("P_ID", OracleDbType.Decimal, pInfo.Id, ParameterDirection.Input),
                    new OracleParameter("P_TITLE", OracleDbType.Varchar2, pInfo.Title_EN, ParameterDirection.Input),
                    new OracleParameter("P_TITLE_EN", OracleDbType.Varchar2, pInfo.Title, ParameterDirection.Input),
                    new OracleParameter("P_HEADER", OracleDbType.Varchar2, pInfo.Header, ParameterDirection.Input),
                    new OracleParameter("P_HEADER_EN", OracleDbType.Varchar2, pInfo.Header_EN, ParameterDirection.Input),
                    new OracleParameter("P_IMAGEHEADER", OracleDbType.Varchar2, pInfo.Imageheader, ParameterDirection.Input),
                    new OracleParameter("P_LANGUAGECODE", OracleDbType.Varchar2, pInfo.Languagecode, ParameterDirection.Input),
                    new OracleParameter("P_CONTENT", OracleDbType.Clob, pInfo.Content, ParameterDirection.Input),
                    new OracleParameter("P_CONTENT_EN", OracleDbType.Clob, pInfo.Content_En, ParameterDirection.Input),
                    new OracleParameter("P_STATUS", OracleDbType.Decimal, pInfo.Status, ParameterDirection.Input),
                    new OracleParameter("P_CATEGORIES_ID", OracleDbType.Varchar2, pInfo.Categories_Id, ParameterDirection.Input),
                    new OracleParameter("P_ARTICLES_TYPE", OracleDbType.Varchar2, pInfo.Articles_Type, ParameterDirection.Input),
                    new OracleParameter("P_MODIFIEDBY", OracleDbType.Varchar2, pInfo.Modifiedby, ParameterDirection.Input),
                    new OracleParameter("P_MODIFIEDDATE", OracleDbType.Date, pInfo.Modifieddate, ParameterDirection.Input),
                    new OracleParameter("P_COUNTRYID", OracleDbType.Decimal, pInfo.Country_Id, ParameterDirection.Input),
                    paramReturn);
                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal ArticlesDeleted(NewsInfo pInfo)
        {
            try
            {
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_NEWS.PROC_NEWS_DELETED",
                    new OracleParameter("P_ID", OracleDbType.Decimal, pInfo.Id, ParameterDirection.Input),
                    new OracleParameter("P_LANGUAGECODE", OracleDbType.Varchar2, pInfo.Languagecode, ParameterDirection.Input),
                    new OracleParameter("P_MODIFIEDBY", OracleDbType.Varchar2, pInfo.Modifiedby, ParameterDirection.Input),
                    new OracleParameter("P_MODIFIEDDATE", OracleDbType.Date, pInfo.Modifieddate, ParameterDirection.Input),
                    paramReturn);
                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public DataSet NewsHomeSearch(string p_key_search, string p_from, string p_to, string p_sort_type, ref decimal p_total_record)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_total_record", OracleDbType.Decimal, ParameterDirection.Output);
                DataSet _ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_NEWS.PROC_NEW_HOME_SHOW",
                    new OracleParameter("p_key_search", OracleDbType.Varchar2, p_key_search, ParameterDirection.Input),
                    new OracleParameter("p_from", OracleDbType.Varchar2, p_from, ParameterDirection.Input),
                    new OracleParameter("p_to", OracleDbType.Varchar2, p_to, ParameterDirection.Input),
                    new OracleParameter("p_sort_type", OracleDbType.Varchar2, p_sort_type, ParameterDirection.Input),
                    paramReturn,
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output)
                   );
                p_total_record = Convert.ToDecimal(paramReturn.Value.ToString());
                return _ds;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        /// <summary>
        /// Các bài viết tĩnh
        /// </summary>
        /// <param name="p_key_search"></param>
        /// <param name="p_from"></param>
        /// <param name="p_to"></param>
        /// <param name="p_sort_type"></param>
        /// <param name="p_total_record"></param>
        /// <returns></returns>
        public DataSet NewsStatic(string pLanguage)
        {
            try
            {

                DataSet _ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_NEWS.PROC_NEWS_STATIC",
                    new OracleParameter("pLanguage", OracleDbType.Varchar2, pLanguage, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output)
                   );
                return _ds;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }


    }
}

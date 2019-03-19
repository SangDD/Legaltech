using Common;
using ObjectInfos;
using Oracle.DataAccess.Client;
using System;
using System.Data;

namespace DataAccess
{
    public class Sys_Pages_DA
    {

        public DataSet Sys_Pages_GetById(decimal p_id)
        {
            try
            {
                DataSet ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_sys_pages.PROC_NEW_GET_BY_ID",
                 new OracleParameter("p_id", OracleDbType.Decimal, p_id, ParameterDirection.Input),
                 new OracleParameter("P_CUSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                return ds;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public DataSet Sys_Pages_GetBy_Code(string p_code)
        {
            try
            {
                DataSet ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_sys_pages.PROC_NEW_GET_BY_CODE",
                 new OracleParameter("p_code", OracleDbType.Varchar2, p_code, ParameterDirection.Input),
                 new OracleParameter("P_CUSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                return ds;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public decimal Sys_Pages_Insert(Sys_Pages_Info pInfo)
        {
            try
            {
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_sys_pages.PROC_INSERT",
                    new OracleParameter("p_code", OracleDbType.Varchar2, pInfo.Code, ParameterDirection.Input),
                    new OracleParameter("P_HEADER", OracleDbType.Varchar2, pInfo.Header, ParameterDirection.Input),
                    new OracleParameter("P_IMAGEHEADER", OracleDbType.Varchar2, pInfo.Imageheader, ParameterDirection.Input),
                    new OracleParameter("P_CONTENT", OracleDbType.Clob, pInfo.Content, ParameterDirection.Input),
                    new OracleParameter("P_CONTENT_EN", OracleDbType.Clob, pInfo.Content_En, ParameterDirection.Input),
                    new OracleParameter("P_STATUS", OracleDbType.Decimal, pInfo.Status, ParameterDirection.Input),
                    new OracleParameter("P_CREATED_BY", OracleDbType.Varchar2, pInfo.Created_By, ParameterDirection.Input),
                    paramReturn);
                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal Sys_Pages_Update(Sys_Pages_Info pInfo)
        {
            try
            {
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_sys_pages.PROC_UPDATE",
                    new OracleParameter("p_id", OracleDbType.Decimal, pInfo.Id, ParameterDirection.Input),
                    new OracleParameter("p_code", OracleDbType.Varchar2, pInfo.Code, ParameterDirection.Input),
                    new OracleParameter("P_HEADER", OracleDbType.Varchar2, pInfo.Header, ParameterDirection.Input),
                    new OracleParameter("P_IMAGEHEADER", OracleDbType.Varchar2, pInfo.Imageheader, ParameterDirection.Input),
                    new OracleParameter("P_CONTENT", OracleDbType.Clob, pInfo.Content, ParameterDirection.Input),
                    new OracleParameter("P_CONTENT_EN", OracleDbType.Clob, pInfo.Content_En, ParameterDirection.Input),
                    new OracleParameter("P_STATUS", OracleDbType.Decimal, pInfo.Status, ParameterDirection.Input),
                    new OracleParameter("P_MODIFIED_BY", OracleDbType.Varchar2, pInfo.Modified_By, ParameterDirection.Input),
                    paramReturn);
                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal Sys_Pages_Deleted(Sys_Pages_Info pInfo)
        {
            try
            {
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_sys_pages.PROC_DELETED",
                    new OracleParameter("p_id", OracleDbType.Decimal, pInfo.Id, ParameterDirection.Input),
                    new OracleParameter("P_MODIFIED_BY", OracleDbType.Varchar2, pInfo.Modified_By, ParameterDirection.Input),
                    paramReturn);
                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public DataSet Sys_Pages_Search(string p_key_search, string p_from, string p_to, string p_orderby, ref decimal p_total_record)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("P_RECORD", OracleDbType.Decimal, ParameterDirection.Output);
                DataSet _ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_sys_pages.proc_search",
                    new OracleParameter("p_key_search", OracleDbType.Varchar2, p_key_search, ParameterDirection.Input),
                    new OracleParameter("p_from", OracleDbType.Varchar2, p_from, ParameterDirection.Input),
                    new OracleParameter("p_to", OracleDbType.Varchar2, p_to, ParameterDirection.Input),
                    new OracleParameter("p_orderby", OracleDbType.Varchar2, p_orderby, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output),
                    paramReturn
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

    }
}

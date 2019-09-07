using Common;
using ObjectInfos;
using ObjectInfos.ModuleTrademark;
using Oracle.DataAccess.Client;
using System;
using System.Data;
namespace DataAccess
{
    public class A05_DA
    {
        public decimal Insert(A05_Info pInfo)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_A05.PROC_APP_DETAIL_A05_INSERT",
                    paramReturn,
                    new OracleParameter("P_CASE_CODE", OracleDbType.Varchar2, pInfo.Case_Code, ParameterDirection.Input),
                    new OracleParameter("P_APP_HEADER_ID", OracleDbType.Decimal, pInfo.App_Header_Id, ParameterDirection.Input),
                    new OracleParameter("P_LANGUAGE_CODE", OracleDbType.Varchar2, pInfo.Language_Code, ParameterDirection.Input),
                    new OracleParameter("P_IMG_URL", OracleDbType.Varchar2, pInfo.IMG_URL, ParameterDirection.Input),
                    new OracleParameter("P_TRANGTHAIDANGKY", OracleDbType.Varchar2, pInfo.TRANGTHAIDANGKY, ParameterDirection.Input),
                    new OracleParameter("P_SODANGKY", OracleDbType.Varchar2, pInfo.SODANGKY, ParameterDirection.Input),
                    new OracleParameter("P_NUOCDANGKY", OracleDbType.Varchar2, pInfo.NUOCDANGKY, ParameterDirection.Input),
                    new OracleParameter("P_TCQLDL_TEN", OracleDbType.Varchar2, pInfo.TCQLDL_TEN, ParameterDirection.Input),
                    new OracleParameter("P_TCQLDL_DIACHI", OracleDbType.Varchar2, pInfo.TCQLDL_DIACHI, ParameterDirection.Input),
                    new OracleParameter("P_TCQLDL_DIENTHOAI", OracleDbType.Varchar2, pInfo.TCQLDL_DIENTHOAI, ParameterDirection.Input),
                    new OracleParameter("P_TCQLDL_FAX", OracleDbType.Varchar2, pInfo.TCQLDL_FAX, ParameterDirection.Input),
                    new OracleParameter("P_TCQLDL_EMAIL", OracleDbType.Varchar2, pInfo.TCQLDL_EMAIL, ParameterDirection.Input),
                    new OracleParameter("P_SANPHAM_TEN", OracleDbType.Varchar2, pInfo.SANPHAM_TEN, ParameterDirection.Input),
                    new OracleParameter("P_SANPHAM_TOMTAT", OracleDbType.Varchar2, pInfo.SANPHAM_TOMTAT, ParameterDirection.Input)                     
                    );
                var result = Convert.ToDecimal(paramReturn.Value.ToString());
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }


        public decimal UpDate(A05_Info pInfo)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_A05.PROC_APP_DETAIL_A05_UPDATE",
                    paramReturn,
                    new OracleParameter("P_CASE_CODE", OracleDbType.Varchar2, pInfo.Case_Code, ParameterDirection.Input),
                    new OracleParameter("P_ID", OracleDbType.Varchar2, pInfo.Id, ParameterDirection.Input),
                    new OracleParameter("P_APP_HEADER_ID", OracleDbType.Decimal, pInfo.App_Header_Id, ParameterDirection.Input),
                    new OracleParameter("P_LANGUAGE_CODE", OracleDbType.Varchar2, pInfo.Language_Code, ParameterDirection.Input),
                    new OracleParameter("P_IMG_URL", OracleDbType.Varchar2, pInfo.IMG_URL, ParameterDirection.Input),
                    new OracleParameter("P_TRANGTHAIDANGKY", OracleDbType.Date, pInfo.TRANGTHAIDANGKY, ParameterDirection.Input),
                    new OracleParameter("P_SODANGKY", OracleDbType.Varchar2, pInfo.SODANGKY, ParameterDirection.Input),
                    new OracleParameter("P_NUOCDANGKY", OracleDbType.Varchar2, pInfo.NUOCDANGKY, ParameterDirection.Input),
                    new OracleParameter("P_TCQLDL_TEN", OracleDbType.Date, pInfo.TCQLDL_TEN, ParameterDirection.Input),
                    new OracleParameter("P_TCQLDL_DIACHI", OracleDbType.Varchar2, pInfo.TCQLDL_DIACHI, ParameterDirection.Input),
                    new OracleParameter("P_TCQLDL_DIENTHOAI", OracleDbType.Varchar2, pInfo.TCQLDL_DIENTHOAI, ParameterDirection.Input),
                    new OracleParameter("P_TCQLDL_FAX", OracleDbType.Varchar2, pInfo.TCQLDL_FAX, ParameterDirection.Input),
                    new OracleParameter("P_TCQLDL_EMAIL", OracleDbType.Varchar2, pInfo.TCQLDL_EMAIL, ParameterDirection.Input),
                    new OracleParameter("P_SANPHAM_TEN", OracleDbType.Varchar2, pInfo.SANPHAM_TEN, ParameterDirection.Input),
                    new OracleParameter("P_SANPHAM_TOMTAT", OracleDbType.Varchar2, pInfo.SANPHAM_TOMTAT, ParameterDirection.Input) 

                    );
                var result = Convert.ToDecimal(paramReturn.Value.ToString());
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public int Deleted(decimal p_app_header_id, string pAppCode, string pLanguage)
        {
            try
            {
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_A05.PROC_APP_DETAIL_A05_DELETE",
                    new OracleParameter("P_APP_HEADER_ID", OracleDbType.Decimal, p_app_header_id, ParameterDirection.Input),
                    new OracleParameter("P_APPCODE", OracleDbType.Varchar2, pAppCode, ParameterDirection.Input),
                    new OracleParameter("P_LANGUAGE_CODE", OracleDbType.Varchar2, pLanguage, ParameterDirection.Input),
                    paramReturn);

                var result = Convert.ToInt32(paramReturn.Value.ToString());
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public DataSet GetByID(decimal p_app_header_id, string p_language_code)
        {
            try
            {
                DataSet _ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_A05.PROC_GETBY_ID",
                    new OracleParameter("P_APP_HEADER_ID", OracleDbType.Decimal, p_app_header_id, ParameterDirection.Input),
                    new OracleParameter("P_LANGUAGE_CODE", OracleDbType.Varchar2, p_language_code, ParameterDirection.Input),
                    new OracleParameter("P_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("P_CURSORHEADER", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("P_CURSOR_DOC", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("P_CURSOR_FEE", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("P_CURSOR_OTHER_MASTER", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("P_CURSOR_OTHER_DOC", OracleDbType.RefCursor, ParameterDirection.Output) 
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

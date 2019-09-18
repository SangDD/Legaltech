using Common;
using ObjectInfos;
using ObjectInfos.ModuleTrademark;
using Oracle.DataAccess.Client;
using System;
using System.Data;
namespace DataAccess
{
    public class C07_DA
    {
        public decimal Insert(C07_Info pInfo)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_C07.PROC_APP_DETAIL_C07_INSERT",
                    paramReturn,
                    new OracleParameter("P_CASE_CODE", OracleDbType.Varchar2, pInfo.Case_Code, ParameterDirection.Input),
                    new OracleParameter("P_APP_HEADER_ID", OracleDbType.Decimal, pInfo.App_Header_Id, ParameterDirection.Input),
                    new OracleParameter("P_LANGUAGE_CODE", OracleDbType.Varchar2, pInfo.Language_Code, ParameterDirection.Input),
                    new OracleParameter("P_LOGOURL", OracleDbType.Varchar2, pInfo.LOGOURL, ParameterDirection.Input),
                    new OracleParameter("P_SODK_QUOCTE", OracleDbType.Varchar2, pInfo.SODK_QUOCTE, ParameterDirection.Input),
                    new OracleParameter("P_NGAY_DK_QUOCTE", OracleDbType.Date, pInfo.NGAY_DK_QUOCTE, ParameterDirection.Input),
                    new OracleParameter("P_NGAY_UT_DKQT", OracleDbType.Date, pInfo.NGAY_UT_DKQT, ParameterDirection.Input),
                    new OracleParameter("P_CHUNH_TEN", OracleDbType.Date, pInfo.CHUNH_TEN, ParameterDirection.Input),
                    new OracleParameter("P_CHUNH_DIACHI", OracleDbType.Varchar2, pInfo.CHUNH_DIACHI, ParameterDirection.Input),
                    new OracleParameter("P_YC_DK_NH_CHUYENDOI", OracleDbType.Varchar2, pInfo.YC_DK_NH_CHUYENDOI, ParameterDirection.Input)
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


        public decimal UpDate(C07_Info pInfo)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_C07.PROC_APP_DETAIL_C07_UPDATE",
                    paramReturn,
                    new OracleParameter("P_ID", OracleDbType.Varchar2, pInfo.C07_Id, ParameterDirection.Input),
                    new OracleParameter("P_CASE_CODE", OracleDbType.Varchar2, pInfo.Case_Code, ParameterDirection.Input),
                    new OracleParameter("P_APP_HEADER_ID", OracleDbType.Decimal, pInfo.App_Header_Id, ParameterDirection.Input),
                    new OracleParameter("P_LANGUAGE_CODE", OracleDbType.Varchar2, pInfo.Language_Code, ParameterDirection.Input),
                    new OracleParameter("P_LOGOURL", OracleDbType.Varchar2, pInfo.LOGOURL, ParameterDirection.Input),
                    new OracleParameter("P_SODK_QUOCTE", OracleDbType.Varchar2, pInfo.SODK_QUOCTE, ParameterDirection.Input),
                    new OracleParameter("P_NGAY_DK_QUOCTE", OracleDbType.Date, pInfo.NGAY_DK_QUOCTE, ParameterDirection.Input),
                    new OracleParameter("P_NGAY_UT_DKQT", OracleDbType.Date, pInfo.NGAY_UT_DKQT, ParameterDirection.Input),
                    new OracleParameter("P_CHUNH_TEN", OracleDbType.Date, pInfo.CHUNH_TEN, ParameterDirection.Input),
                    new OracleParameter("P_CHUNH_DIACHI", OracleDbType.Varchar2, pInfo.CHUNH_DIACHI, ParameterDirection.Input),
                    new OracleParameter("P_YC_DK_NH_CHUYENDOI", OracleDbType.Varchar2, pInfo.YC_DK_NH_CHUYENDOI, ParameterDirection.Input)

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
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_C07.PROC_APP_DETAIL_C07_DELETE",
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
                DataSet _ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_C07.PROC_GETBY_ID",
                    new OracleParameter("P_APP_HEADER_ID", OracleDbType.Decimal, p_app_header_id, ParameterDirection.Input),
                    new OracleParameter("P_LANGUAGE_CODE", OracleDbType.Varchar2, p_language_code, ParameterDirection.Input),
                    new OracleParameter("P_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("P_CURSORHEADER", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("P_CURSOR_DOC", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("P_CURSOR_FEE", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("P_CURSOR_OTHER_MASTER", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursor_class", OracleDbType.RefCursor, ParameterDirection.Output),
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
        public DataSet AppTM06DKQTGetByID(string pAppHeaderID, string pLanguage, int pStatus)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_total_record", OracleDbType.Decimal, ParameterDirection.Output);
                DataSet _ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_APP_GET_DATA.PROC_APP_TM06DKQT_GET_BY_ID",
                    new OracleParameter("P_APP_HEADER_ID", OracleDbType.Varchar2, pAppHeaderID, ParameterDirection.Input),
                    new OracleParameter("P_LANGUAGE", OracleDbType.Varchar2, pLanguage, ParameterDirection.Input),
                    new OracleParameter("P_STATUS", OracleDbType.Decimal, pStatus, ParameterDirection.Input),
                    new OracleParameter("P_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("P_C_DOC", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("P_C_CLASS_DETAIL", OracleDbType.RefCursor, ParameterDirection.Output)
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

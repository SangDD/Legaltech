using Common;
using ObjectInfos;
using ObjectInfos.ModuleTrademark;
using Oracle.DataAccess.Client;
using System;
using System.Data;
namespace DataAccess
{
    public class C08_DA
    {
        public decimal Insert(C08_Info pInfo)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_C08.PROC_APP_DETAIL_C08_INSERT",
                    new OracleParameter("P_CASE_CODE", OracleDbType.Varchar2, pInfo.Case_Code, ParameterDirection.Input),
                    new OracleParameter("P_APP_HEADER_ID", OracleDbType.Decimal, pInfo.App_Header_Id, ParameterDirection.Input),
                    new OracleParameter("P_LANGUAGE_CODE", OracleDbType.Varchar2, pInfo.Language_Code, ParameterDirection.Input),
                    new OracleParameter("P_SO_DON_DK_QTNH", OracleDbType.Varchar2, pInfo.SO_DON_DK_QTNH, ParameterDirection.Input),
                    new OracleParameter("P_SO_DK_QTNH", OracleDbType.Varchar2, pInfo.SO_DK_QTNH.Trim(), ParameterDirection.Input),
                    new OracleParameter("P_NGAYNOPDON_DKQTNH", OracleDbType.Date, pInfo.NGAYNOPDON_DKQTNH, ParameterDirection.Input),
                    new OracleParameter("P_LOAI_DK", OracleDbType.Varchar2, pInfo.LOAI_DK, ParameterDirection.Input),
                     paramReturn 
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


        public decimal UpDate(C08_Info pInfo)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_C08.PROC_APP_DETAIL_C08_UPDATE",
                    new OracleParameter("P_ID", OracleDbType.Varchar2, pInfo.C08_Id, ParameterDirection.Input),
                    new OracleParameter("P_CASE_CODE", OracleDbType.Varchar2, pInfo.Case_Code, ParameterDirection.Input),
                    new OracleParameter("P_APP_HEADER_ID", OracleDbType.Decimal, pInfo.App_Header_Id, ParameterDirection.Input),
                    new OracleParameter("P_LANGUAGE_CODE", OracleDbType.Varchar2, pInfo.Language_Code, ParameterDirection.Input),
                    new OracleParameter("P_SO_DON_DK_QTNH", OracleDbType.Varchar2, pInfo.SO_DON_DK_QTNH, ParameterDirection.Input),
                    new OracleParameter("P_SO_DK_QTNH", OracleDbType.Varchar2, pInfo.SO_DK_QTNH.Trim(), ParameterDirection.Input),
                    new OracleParameter("P_NGAYNOPDON_DKQTNH", OracleDbType.Date, pInfo.NGAYNOPDON_DKQTNH, ParameterDirection.Input),
                    new OracleParameter("P_LOAI_DK", OracleDbType.Varchar2, pInfo.LOAI_DK, ParameterDirection.Input),
                    paramReturn
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
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_C08.PROC_APP_DETAIL_C08_DELETE",
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
                DataSet _ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_C08.PROC_GETBY_ID",
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

using Common;
using ObjectInfos;
using ObjectInfos.ModuleTrademark;
using Oracle.DataAccess.Client;
using System;
using System.Data;
namespace DataAccess
{
    public class App_Detail_C01_DA
    {

        public decimal Insert(App_Detail_C01_Info pInfo)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_detail_c01.Proc_App_Detail_C01_Insert",
                    new OracleParameter("p_app_header_id", OracleDbType.Decimal, pInfo.App_Header_Id, ParameterDirection.Input),
                    new OracleParameter("p_appcode", OracleDbType.Varchar2, pInfo.Appcode, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, pInfo.Language_Code, ParameterDirection.Input),
                    new OracleParameter("p_request_change_type", OracleDbType.Decimal, pInfo.Request_Change_Type, ParameterDirection.Input),
                    new OracleParameter("p_app_no_change", OracleDbType.Varchar2, pInfo.App_No_Change, ParameterDirection.Input),
                    new OracleParameter("p_request_master_to", OracleDbType.Varchar2, pInfo.Request_Master_To, ParameterDirection.Input),
                    new OracleParameter("p_request_address_to", OracleDbType.Varchar2, pInfo.Request_Address_To, ParameterDirection.Input),
                    new OracleParameter("p_request_others_to", OracleDbType.Varchar2, pInfo.Request_Others_To, ParameterDirection.Input),
                    new OracleParameter("p_request_to_content", OracleDbType.Varchar2, pInfo.Request_To_Content, ParameterDirection.Input),
                    paramReturn);
                var result = Convert.ToDecimal(paramReturn.Value.ToString());
                return result;


            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }


        public int UpDate(App_Detail_C01_Info pInfo)
        {
            try
            {
                var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_detail_c01.proc_app_detail_c01_update",
                    new OracleParameter("p_id", OracleDbType.Decimal, pInfo.Detail_Id, ParameterDirection.Input),
                    new OracleParameter("p_app_header_id", OracleDbType.Decimal, pInfo.App_Header_Id, ParameterDirection.Input),
                    new OracleParameter("p_appcode", OracleDbType.Varchar2, pInfo.Appcode, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, pInfo.Language_Code, ParameterDirection.Input),
                    new OracleParameter("p_request_change_type", OracleDbType.Decimal, pInfo.Request_Change_Type, ParameterDirection.Input),
                    new OracleParameter("p_app_no_change", OracleDbType.Varchar2, pInfo.App_No_Change, ParameterDirection.Input),
                    new OracleParameter("p_request_master_to", OracleDbType.Varchar2, pInfo.Request_Master_To, ParameterDirection.Input),
                    new OracleParameter("p_request_address_to", OracleDbType.Varchar2, pInfo.Request_Address_To, ParameterDirection.Input),
                    new OracleParameter("p_request_others_to", OracleDbType.Varchar2, pInfo.Request_Others_To, ParameterDirection.Input),
                    new OracleParameter("p_request_to_content", OracleDbType.Varchar2, pInfo.Request_To_Content, ParameterDirection.Input),
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

        public int Deleted(decimal p_app_header_id, string pAppCode, string pLanguage)
        {
            try
            {
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_detail_c01.Proc_C01_Delete",
                    new OracleParameter("p_app_header_id", OracleDbType.Decimal, p_app_header_id, ParameterDirection.Input),
                    new OracleParameter("p_appcode", OracleDbType.Varchar2, pAppCode, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, pLanguage, ParameterDirection.Input),
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
                DataSet _ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_detail_c01.Proc_GetBy_ID",
                    new OracleParameter("p_app_header_id", OracleDbType.Decimal, p_app_header_id, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, p_language_code, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursorHeader", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursor_doc", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursor_fee", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursor_other_doc", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursor_image_public", OracleDbType.RefCursor, ParameterDirection.Output));
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

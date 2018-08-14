using Common;
using ObjectInfos;
using ObjectInfos.ModuleTrademark;
using Oracle.DataAccess.Client;
using System;
using System.Data;
namespace DataAccess
{
    public class App_Detail_PLC05_KN_DA
    {

        public decimal Insert(App_Detail_PLC05_KN_Info pInfo)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_detail_plc05_kn.Proc_Insert",
                    new OracleParameter("p_app_header_id", OracleDbType.Decimal, pInfo.App_Header_Id, ParameterDirection.Input),
                    new OracleParameter("p_appcode", OracleDbType.Varchar2, pInfo.Appcode, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, pInfo.Language_Code, ParameterDirection.Input),
                    new OracleParameter("p_customer_code", OracleDbType.Varchar2, pInfo.Customer_Code, ParameterDirection.Input),

                    new OracleParameter("p_times", OracleDbType.Decimal, pInfo.Times, ParameterDirection.Input),
                    new OracleParameter("p_appeal_type", OracleDbType.Decimal, pInfo.Appeal_Type, ParameterDirection.Input),
                    new OracleParameter("p_appeal_number", OracleDbType.Varchar2, pInfo.Appeal_Number, ParameterDirection.Input),
                    new OracleParameter("p_appeal_date", OracleDbType.Date, pInfo.Appeal_Date, ParameterDirection.Input),
                    new OracleParameter("p_appeal_appno", OracleDbType.Varchar2, pInfo.Appeal_Appno, ParameterDirection.Input),
                    new OracleParameter("p_appeal_degree", OracleDbType.Varchar2, pInfo.Appeal_Degree, ParameterDirection.Input),
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


        public int UpDate(App_Detail_PLC05_KN_Info pInfo)
        {
            try
            {
                var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_detail_plc05_kn.Proc_Update",
                    new OracleParameter("p_app_header_id", OracleDbType.Decimal, pInfo.App_Header_Id, ParameterDirection.Input),
                    new OracleParameter("p_appcode", OracleDbType.Varchar2, pInfo.Appcode, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, pInfo.Language_Code, ParameterDirection.Input),
                    new OracleParameter("p_customer_code", OracleDbType.Varchar2, pInfo.Customer_Code, ParameterDirection.Input),

                    new OracleParameter("p_times", OracleDbType.Decimal, pInfo.Times, ParameterDirection.Input),
                    new OracleParameter("p_appeal_type", OracleDbType.Decimal, pInfo.Appeal_Type, ParameterDirection.Input),
                    new OracleParameter("p_appeal_number", OracleDbType.Varchar2, pInfo.Appeal_Number, ParameterDirection.Input),
                    new OracleParameter("p_appeal_date", OracleDbType.Date, pInfo.Appeal_Date, ParameterDirection.Input),
                    new OracleParameter("p_appeal_appno", OracleDbType.Varchar2, pInfo.Appeal_Appno, ParameterDirection.Input),
                    new OracleParameter("p_appeal_degree", OracleDbType.Varchar2, pInfo.Appeal_Degree, ParameterDirection.Input),
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

        public int Deleted(decimal pAppHeaderID, string pAppCode, string pLanguage)
        {
            try
            {
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_detail_plc05_kn.Proc_Delete",
                    new OracleParameter("p_app_header_id", OracleDbType.Decimal, pAppHeaderID, ParameterDirection.Input),
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
                DataSet _ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_detail_plc05_kn.Proc_GetById",
                    new OracleParameter("p_app_header_id", OracleDbType.Decimal, p_app_header_id, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, p_language_code, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursorHeader", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursor_doc", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursor_fee", OracleDbType.RefCursor, ParameterDirection.Output));
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

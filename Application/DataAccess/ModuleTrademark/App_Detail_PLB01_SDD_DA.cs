using Common;
using ObjectInfos;
using ObjectInfos.ModuleTrademark;
using Oracle.DataAccess.Client;
using System;
using System.Data;
namespace DataAccess
{
    public class App_Detail_PLB01_SDD_DA
    {

        public int Insert(App_Detail_PLB01_SDD_Info pInfo)
        {
            try
            {
                var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_detail_plb01_sdd.Proc_Plb01_Sdd_Insert",
                    new OracleParameter("p_app_header_id", OracleDbType.Decimal, pInfo.App_Header_Id, ParameterDirection.Input),
                    new OracleParameter("p_appcode", OracleDbType.Varchar2, pInfo.Appcode, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, pInfo.Language_Code, ParameterDirection.Input),
                    new OracleParameter("p_request_change_type", OracleDbType.Decimal, pInfo.Request_Change_Type, ParameterDirection.Input),
                    new OracleParameter("p_app_no_change", OracleDbType.Varchar2, pInfo.App_No_Change, ParameterDirection.Input),
                    new OracleParameter("p_request_to_type", OracleDbType.Decimal, pInfo.Request_To_Type, ParameterDirection.Input),
                    new OracleParameter("p_request_to_content", OracleDbType.Varchar2, pInfo.Request_To_Content, ParameterDirection.Input),
                    new OracleParameter("p_number_pic", OracleDbType.Varchar2, pInfo.Number_Pic, ParameterDirection.Input),
                    new OracleParameter("p_number_page", OracleDbType.Varchar2, pInfo.Number_Page, ParameterDirection.Input),
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


        public int UpDate(App_Detail_PLB01_SDD_Info pInfo)
        {
            try
            {
                var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_detail_plb01_sdd.Proc_Plb01_Sdd_Update",
                    new OracleParameter("p_id", OracleDbType.Decimal, pInfo.Id, ParameterDirection.Input),
                    new OracleParameter("p_app_header_id", OracleDbType.Decimal, pInfo.App_Header_Id, ParameterDirection.Input),
                    new OracleParameter("p_appcode", OracleDbType.Varchar2, pInfo.Appcode, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, pInfo.Language_Code, ParameterDirection.Input),
                    new OracleParameter("p_request_change_type", OracleDbType.Decimal, pInfo.Request_Change_Type, ParameterDirection.Input),
                    new OracleParameter("p_app_no_change", OracleDbType.Varchar2, pInfo.App_No_Change, ParameterDirection.Input),
                    new OracleParameter("p_request_to_type", OracleDbType.Decimal, pInfo.Request_To_Type, ParameterDirection.Input),
                    new OracleParameter("p_request_to_content", OracleDbType.Varchar2, pInfo.Request_To_Content, ParameterDirection.Input),
                    new OracleParameter("p_number_pic", OracleDbType.Varchar2, pInfo.Number_Pic, ParameterDirection.Input),
                    new OracleParameter("p_number_page", OracleDbType.Varchar2, pInfo.Number_Page, ParameterDirection.Input),
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
                OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_detail_plb01_sdd.Proc_Plb01_Sdd_Delete",
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

        public DataSet GetByID(decimal p_id, decimal p_app_header_id, string p_language_code)
        {
            try
            {
                DataSet _ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_detail_plb01_sdd.Proc_GetById",
                    new OracleParameter("p_id", OracleDbType.Decimal, p_id, ParameterDirection.Input),
                    new OracleParameter("p_app_header_id", OracleDbType.Decimal, p_app_header_id, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, p_language_code, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
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

using Common;
using ObjectInfos;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class B03_DA
    {
        public decimal Insert(B03_Info pInfo)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_b03.Proc_App_Detail_B03_Insert",
                    new OracleParameter("p_id", OracleDbType.Decimal, pInfo.ID, ParameterDirection.Input),
                    new OracleParameter("p_appcode", OracleDbType.Varchar2, pInfo.AppCode, ParameterDirection.Input),
                    new OracleParameter("p_case_code", OracleDbType.Varchar2, pInfo.Case_Code, ParameterDirection.Input),
                    new OracleParameter("p_app_header_id", OracleDbType.Decimal, pInfo.App_Header_Id, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, pInfo.Language_Code, ParameterDirection.Input),
                    new OracleParameter("p_app_detail_number", OracleDbType.Varchar2, pInfo.App_Detail_Number, ParameterDirection.Input),
                    new OracleParameter("p_name_evaluator", OracleDbType.Varchar2, pInfo.Name_Evaluator, ParameterDirection.Input),
                    new OracleParameter("p_address_evaluator", OracleDbType.Varchar2, pInfo.Address_Evaluator, ParameterDirection.Input),
                    new OracleParameter("p_phone_evaluator", OracleDbType.Varchar2, pInfo.Phone_Evaluator, ParameterDirection.Input),
                    new OracleParameter("p_fax_evaluator", OracleDbType.Varchar2, pInfo.Fax_Evaluator, ParameterDirection.Input),
                    new OracleParameter("p_email_evaluator", OracleDbType.Varchar2, pInfo.Email_Evaluator, ParameterDirection.Input),
                    new OracleParameter("p_type_evaluator", OracleDbType.Varchar2, pInfo.Type_Evaluator, ParameterDirection.Input),
                    new OracleParameter("p_point", OracleDbType.Decimal, pInfo.Point, ParameterDirection.Input),
                    new OracleParameter("p_thamdinhnoidung", OracleDbType.Varchar2, pInfo.Thamdinhnoidung, ParameterDirection.Input),
                   
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


        public decimal UpDate(B03_Info pInfo)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_b03.Proc_App_Detail_B03_Update",
                    new OracleParameter("p_id", OracleDbType.Decimal, pInfo.ID, ParameterDirection.Input),
                    new OracleParameter("p_appcode", OracleDbType.Decimal, pInfo.AppCode, ParameterDirection.Input),
                    new OracleParameter("p_case_code", OracleDbType.Varchar2, pInfo.Case_Code, ParameterDirection.Input),
                    new OracleParameter("p_app_header_id", OracleDbType.Decimal, pInfo.App_Header_Id, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, pInfo.Language_Code, ParameterDirection.Input),
                    new OracleParameter("p_app_detail_number", OracleDbType.Varchar2, pInfo.App_Detail_Number, ParameterDirection.Input),
                    new OracleParameter("p_name_evaluator", OracleDbType.Varchar2, pInfo.Name_Evaluator, ParameterDirection.Input),
                    new OracleParameter("p_address_evaluator", OracleDbType.Varchar2, pInfo.Address_Evaluator, ParameterDirection.Input),
                    new OracleParameter("p_phone_evaluator", OracleDbType.Varchar2, pInfo.Phone_Evaluator, ParameterDirection.Input),
                    new OracleParameter("p_fax_evaluator", OracleDbType.Varchar2, pInfo.Fax_Evaluator, ParameterDirection.Input),
                    new OracleParameter("p_email_evaluator", OracleDbType.Varchar2, pInfo.Email_Evaluator, ParameterDirection.Input),
                    new OracleParameter("p_type_evaluator", OracleDbType.Varchar2, pInfo.Type_Evaluator, ParameterDirection.Input),
                    new OracleParameter("p_point", OracleDbType.Decimal, pInfo.Point, ParameterDirection.Input),
                    new OracleParameter("p_thamdinhnoidung", OracleDbType.Varchar2, pInfo.Thamdinhnoidung, ParameterDirection.Input),
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

        public int Deleted(decimal p_app_header_id)
        {
            try
            {
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_b03.proc_app_detail_B03_delete",
                    new OracleParameter("p_app_header_id", OracleDbType.Decimal, p_app_header_id, ParameterDirection.Input),
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

    }
}

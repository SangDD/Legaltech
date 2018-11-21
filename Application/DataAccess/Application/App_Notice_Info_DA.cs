using Common;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class App_Notice_Info_DA
    {
        public DataSet App_Notice_Search(string p_key_search, string p_from, string p_to, string p_sort_type, ref decimal p_total_record)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_total_record", OracleDbType.Decimal, ParameterDirection.Output);
                DataSet _ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_notice_info.proc_search",
                    new OracleParameter("p_key_search", OracleDbType.Varchar2, p_key_search, ParameterDirection.Input),
                    new OracleParameter("p_from", OracleDbType.Varchar2, p_from, ParameterDirection.Input),
                    new OracleParameter("p_to", OracleDbType.Varchar2, p_to, ParameterDirection.Input),
                    new OracleParameter("p_sort_type", OracleDbType.Varchar2, p_sort_type, ParameterDirection.Input),
                    paramReturn,
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));

                p_total_record = Convert.ToDecimal(paramReturn.Value.ToString());
                return _ds;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public DataSet App_Notice_GetBy_CaseCode(string p_case_code, decimal p_notice_type)
        {
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_notice_info.proc_getBy_CaseCode",
                    new OracleParameter("p_case_code", OracleDbType.Varchar2, p_case_code, ParameterDirection.Input),
                    new OracleParameter("p_notice_type", OracleDbType.Decimal, p_notice_type, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public DataSet Get_Number_Notice(string p_case_code, decimal p_notice_type)
        {
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_notice_info.proc_get_number",
                    new OracleParameter("p_case_code", OracleDbType.Varchar2, p_case_code, ParameterDirection.Input),
                    new OracleParameter("p_notice_type", OracleDbType.Decimal, p_notice_type, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public int App_Notice_Update_Status(string p_case_code, decimal p_notice_type, decimal p_status, decimal p_result,
            DateTime p_accept_date, string p_note, string p_modify_by, string p_language_code)
        {
            try
            {
                var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_notice_info.proc_update_status",
                    new OracleParameter("p_case_code", OracleDbType.Varchar2, p_case_code, ParameterDirection.Input),
                    new OracleParameter("p_notice_type", OracleDbType.Decimal, p_notice_type, ParameterDirection.Input),
                    new OracleParameter("p_status", OracleDbType.Decimal, p_status, ParameterDirection.Input),
                    new OracleParameter("p_result", OracleDbType.Decimal, p_result, ParameterDirection.Input),
                    new OracleParameter("p_accept_date", OracleDbType.Date, p_accept_date, ParameterDirection.Input),
                    new OracleParameter("p_note", OracleDbType.Varchar2, p_note, ParameterDirection.Input),
                    new OracleParameter("p_modify_by", OracleDbType.Varchar2, p_modify_by, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, p_language_code, ParameterDirection.Input),
                    paramReturn);
                var result = Convert.ToInt32(paramReturn.Value.ToString());
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal App_Notice_Insert(string p_case_code, string p_notice_number, DateTime p_notice_date, decimal p_notice_type, string p_notice_url, string p_notice_trans_url,
            decimal p_result, DateTime p_accept_date, DateTime p_exp_date, string p_accept_url, string p_reject_reason, decimal p_status, string p_advise_replies,
            decimal p_billing_id, string p_billing_url, string p_created_by, string p_note, DateTime p_replies_deadline)
        {
            {
                try
                {
                    var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                    OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_notice_info.Proc_App_Notice_Info_Insert",
                        new OracleParameter("p_case_code", OracleDbType.Varchar2, p_case_code, ParameterDirection.Input),
                        new OracleParameter("p_notice_number", OracleDbType.Varchar2, p_notice_number, ParameterDirection.Input),
                        new OracleParameter("p_notice_date", OracleDbType.Date, p_notice_date, ParameterDirection.Input),
                        new OracleParameter("p_notice_type", OracleDbType.Decimal, p_notice_type, ParameterDirection.Input),
                        new OracleParameter("p_notice_url", OracleDbType.Varchar2, p_notice_url, ParameterDirection.Input),
                        new OracleParameter("p_notice_trans_url", OracleDbType.Varchar2, p_notice_trans_url, ParameterDirection.Input),
                        new OracleParameter("p_result", OracleDbType.Decimal, p_result, ParameterDirection.Input),
                        new OracleParameter("p_accept_date", OracleDbType.Date, p_accept_date, ParameterDirection.Input),
                        new OracleParameter("p_exp_date", OracleDbType.Date, p_exp_date, ParameterDirection.Input),
                        new OracleParameter("p_accept_url", OracleDbType.Varchar2, p_accept_url, ParameterDirection.Input),
                        new OracleParameter("p_reject_reason", OracleDbType.Varchar2, p_reject_reason, ParameterDirection.Input),
                        new OracleParameter("p_status", OracleDbType.Decimal, p_status, ParameterDirection.Input),
                        new OracleParameter("p_advise_replies", OracleDbType.Varchar2, p_advise_replies, ParameterDirection.Input),
                        new OracleParameter("p_billing_id", OracleDbType.Decimal, p_billing_id, ParameterDirection.Input),
                        new OracleParameter("p_biling_url", OracleDbType.Varchar2, p_billing_url, ParameterDirection.Input),
                        new OracleParameter("p_replies_deadline", OracleDbType.Date, p_replies_deadline, ParameterDirection.Input),
                        new OracleParameter("p_created_by", OracleDbType.Varchar2, p_created_by, ParameterDirection.Input),
                        new OracleParameter("p_note", OracleDbType.Varchar2, p_note, ParameterDirection.Input),
                        paramReturn);

                    return Convert.ToDecimal(paramReturn.Value.ToString());
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                    return -1;
                }
            }
        }

        public decimal App_Notice_Review_Accept(string p_case_code, decimal p_notice_type, decimal p_status,
            string p_note, string p_modify_by, string p_language_code)
        {
            try
            {
                var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_notice_info.proc_review_accept",
                    new OracleParameter("p_case_code", OracleDbType.Varchar2, p_case_code, ParameterDirection.Input),
                    new OracleParameter("p_notice_type", OracleDbType.Decimal, p_notice_type, ParameterDirection.Input),
                    new OracleParameter("p_status", OracleDbType.Decimal, p_status, ParameterDirection.Input),
                    new OracleParameter("p_note", OracleDbType.Varchar2, p_note, ParameterDirection.Input),
                    new OracleParameter("p_modify_by", OracleDbType.Varchar2, p_modify_by, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, p_language_code, ParameterDirection.Input),
                    paramReturn);

                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal App_Notice_Review_Reject(string p_case_code, decimal p_notice_type, decimal p_status,
            string p_advise_replies, string p_advise_replies_trans, string p_note, string p_modify_by, string p_language_code, DateTime p_replies_date)
        {
            try
            {
                var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_notice_info.proc_review_reject",
                    new OracleParameter("p_case_code", OracleDbType.Varchar2, p_case_code, ParameterDirection.Input),
                    new OracleParameter("p_notice_type", OracleDbType.Decimal, p_notice_type, ParameterDirection.Input),
                    new OracleParameter("p_status", OracleDbType.Decimal, p_status, ParameterDirection.Input),
                    new OracleParameter("p_advise_replies", OracleDbType.Varchar2, p_advise_replies, ParameterDirection.Input),
                    new OracleParameter("p_advise_replies_trans", OracleDbType.Varchar2, p_advise_replies_trans, ParameterDirection.Input),
                    new OracleParameter("p_replies_date", OracleDbType.Date, p_replies_date, ParameterDirection.Input),

                    
                    new OracleParameter("p_note", OracleDbType.Varchar2, p_note, ParameterDirection.Input),
                    new OracleParameter("p_modify_by", OracleDbType.Varchar2, p_modify_by, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, p_language_code, ParameterDirection.Input),
                    paramReturn);

                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal Reject_Lawer_Update_Deadline(string p_case_code, decimal p_notice_type, DateTime p_replies_date, DateTime p_next_deadline, string p_replies_url,
            decimal p_status, string p_note, string p_modify_by, string p_language_code)
        {
            try
            {
                var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_notice_info.proc_ls_update_deadline",
                    new OracleParameter("p_case_code", OracleDbType.Varchar2, p_case_code, ParameterDirection.Input),
                    new OracleParameter("p_notice_type", OracleDbType.Decimal, p_notice_type, ParameterDirection.Input),
                    new OracleParameter("p_status", OracleDbType.Decimal, p_status, ParameterDirection.Input),
                    new OracleParameter("p_replies_date", OracleDbType.Date, p_replies_date, ParameterDirection.Input),
                    new OracleParameter("p_next_deadline", OracleDbType.Date, p_next_deadline, ParameterDirection.Input),
                    new OracleParameter("p_replies_url", OracleDbType.Varchar2, p_replies_url, ParameterDirection.Input),

                    new OracleParameter("p_note", OracleDbType.Varchar2, p_note, ParameterDirection.Input),
                    new OracleParameter("p_modify_by", OracleDbType.Varchar2, p_modify_by, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, p_language_code, ParameterDirection.Input),
                    paramReturn);

                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal Update_CV_Auto(string p_case_code, decimal p_notice_type, string p_cv_answer_url, string p_modify_by, string p_language_code)
        {
            try
            {
                var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_notice_info.proc_ls_update_cv_auto",
                    new OracleParameter("p_case_code", OracleDbType.Varchar2, p_case_code, ParameterDirection.Input),
                    new OracleParameter("p_notice_type", OracleDbType.Decimal, p_notice_type, ParameterDirection.Input),
                    new OracleParameter("p_cv_answer_url", OracleDbType.Varchar2, p_cv_answer_url, ParameterDirection.Input),
                    new OracleParameter("p_modify_by", OracleDbType.Varchar2, p_modify_by, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, p_language_code, ParameterDirection.Input),
                    paramReturn);

                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }
    }
}

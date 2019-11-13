namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using Common;
    using Oracle.DataAccess.Client;

    public class EmailDA
    {
        public decimal Send_Email_Insert(string p_email_from, string p_email_to, string p_email_cc, string p_display_name, string p_subject,
            string p_content, List<string> p_lst_attachment, string p_status, DateTime p_send_time, string p_created_by)
        {
            try
            {
                string _attachment = "";
                if (p_lst_attachment != null && p_lst_attachment.Count > 0)
                {
                    _attachment = string.Join(",", p_lst_attachment);
                }

                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_send_email.Proc_Send_Email_Insert_new",
                    new OracleParameter("p_email_from", OracleDbType.Varchar2, p_email_from, ParameterDirection.Input),
                    new OracleParameter("p_email_to", OracleDbType.Varchar2, p_email_to, ParameterDirection.Input),
                    new OracleParameter("p_email_cc", OracleDbType.Varchar2, p_email_cc, ParameterDirection.Input),
                    new OracleParameter("p_display_name", OracleDbType.Varchar2, p_display_name, ParameterDirection.Input),
                    new OracleParameter("p_subject", OracleDbType.Varchar2, p_subject, ParameterDirection.Input),
                    new OracleParameter("p_content", OracleDbType.Clob, p_content, ParameterDirection.Input),
                    new OracleParameter("p_lst_attachment", OracleDbType.Clob, _attachment, ParameterDirection.Input),
                    new OracleParameter("p_status", OracleDbType.Varchar2, p_status, ParameterDirection.Input),
                    new OracleParameter("p_send_time", OracleDbType.Date, p_send_time, ParameterDirection.Input),
                    new OracleParameter("p_created_by", OracleDbType.Varchar2, p_created_by, ParameterDirection.Input));
                return 0;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public DataSet Email_Search(string p_user_name, string p_key_search, string p_from, string p_to, string p_sort_type, ref decimal p_total_record)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_total_record", OracleDbType.Decimal, ParameterDirection.Output);
                DataSet _ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_send_email.proc_search",
                    new OracleParameter("p_user_name", OracleDbType.Varchar2, p_user_name, ParameterDirection.Input),
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

        public DataSet Email_GetBy_Id(decimal p_id, string p_case_code, string p_language_code)
        {
            try
            {
                DataSet _ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_send_email.proc_getby_id",
                    new OracleParameter("p_id", OracleDbType.Decimal, p_id, ParameterDirection.Input),
                    new OracleParameter("p_case_code", OracleDbType.Varchar2, p_case_code, ParameterDirection.Input),
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

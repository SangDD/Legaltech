namespace DataAccess.ModuleMemoryData
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using Common;
    using Oracle.DataAccess.Client;

    public class AllCodeDA
    {
        public static DataSet GetAllInAllCode()
        {
            var ds = new DataSet();
            try
            {
                ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_allcode.proc_AllCode_GetAll",
                new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return ds;
        }

        public static DataSet Get_All_Injection()
        {
            var ds = new DataSet();
            try
            {
                ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_allcode.proc_injection",
                new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return ds;
        }

        public static DataSet Country_GetAll()
        {
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_allcode.proc_Country_GetAll",
                 new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public static DataSet Nation_Represent_GetAll()
        {
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_allcode.proc_nation_represent_getall",
                 new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public decimal Send_Email_Insert(string p_email_from, string p_email_to, string p_email_cc, string p_display_name, string p_subject, 
            string p_content, List<string> p_lst_attachment, string p_status, DateTime p_send_time)
        {
            try
            {
                string _attachment = "";
                if (p_lst_attachment != null && p_lst_attachment.Count > 0)
                {
                    _attachment = string.Join(",", p_lst_attachment);
                }

                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_send_email.Proc_Send_Email_Insert",
                    new OracleParameter("p_email_from", OracleDbType.Varchar2, p_email_from, ParameterDirection.Input),
                    new OracleParameter("p_email_to", OracleDbType.Varchar2, p_email_to, ParameterDirection.Input),
                    new OracleParameter("p_email_cc", OracleDbType.Varchar2, p_email_cc, ParameterDirection.Input),
                    new OracleParameter("p_display_name", OracleDbType.Varchar2, p_display_name, ParameterDirection.Input),
                    new OracleParameter("p_subject", OracleDbType.Varchar2, p_subject, ParameterDirection.Input),
                    new OracleParameter("p_content", OracleDbType.Clob, p_content, ParameterDirection.Input),
                    new OracleParameter("p_lst_attachment", OracleDbType.Clob, _attachment, ParameterDirection.Input),
                    new OracleParameter("p_status", OracleDbType.Varchar2, p_status, ParameterDirection.Input),
                    new OracleParameter("p_send_time", OracleDbType.Date, p_send_time, ParameterDirection.Input));
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
    }
}

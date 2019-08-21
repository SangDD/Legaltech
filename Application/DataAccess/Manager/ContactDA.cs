using Common;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Manager
{
    public class ContactDA
    {
        public DataSet Contact_Search(string p_key_search, string p_language, string p_status,string p_from, string p_to, ref decimal p_total_record)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_total_record", OracleDbType.Decimal, ParameterDirection.Output);
                DataSet _ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_contact.proc_search_contact",
                    new OracleParameter("p_key_search", OracleDbType.Varchar2, p_key_search, ParameterDirection.Input),
                    new OracleParameter("p_language", OracleDbType.Varchar2, p_language, ParameterDirection.Input),
                    new OracleParameter("p_status", OracleDbType.Varchar2, p_status, ParameterDirection.Input),
                    new OracleParameter("p_from", OracleDbType.Varchar2, p_from, ParameterDirection.Input),
                    new OracleParameter("p_to", OracleDbType.Varchar2, p_to, ParameterDirection.Input),
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
        public DataSet Contact_GetByID(decimal p_id)
        {
            try
            {
                DataSet ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_contact.proc_getbyid_contact",
                 new OracleParameter("p_id", OracleDbType.Decimal, p_id, ParameterDirection.Input),
                 new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
                return ds;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }
        public decimal Contact_UpdateStatus(decimal p_id, decimal p_status, string p_replycontent, string p_replysubject, string p_replyby)
        {
            try
            {
                var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_contact.proc_update_status",
                    new OracleParameter("p_id", OracleDbType.Varchar2, p_id, ParameterDirection.Input),
                    new OracleParameter("p_status", OracleDbType.Decimal, p_status, ParameterDirection.Input),
                    new OracleParameter("p_replycontent", OracleDbType.Varchar2, p_replycontent, ParameterDirection.Input),
                    new OracleParameter("p_replysubject", OracleDbType.Varchar2, p_replysubject, ParameterDirection.Input),
                    new OracleParameter("p_replyby", OracleDbType.Varchar2, p_replyby, ParameterDirection.Input),
                    paramReturn);
                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }
        public decimal Contact_Insert(string p_contactname, string p_subject, string p_email, string p_content, string p_language)
        {
            try
            {
                var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_contact.proc_insert_contact",
                    new OracleParameter("p_contactname", OracleDbType.Varchar2, p_contactname, ParameterDirection.Input),
                    new OracleParameter("p_subject", OracleDbType.Varchar2, p_subject, ParameterDirection.Input),
                    new OracleParameter("p_email", OracleDbType.Varchar2, p_email, ParameterDirection.Input),
                    new OracleParameter("p_content", OracleDbType.Varchar2, p_content, ParameterDirection.Input),
                    new OracleParameter("p_language", OracleDbType.Varchar2, p_language, ParameterDirection.Input),
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

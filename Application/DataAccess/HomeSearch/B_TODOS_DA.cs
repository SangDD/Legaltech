using Common;
using Common.CommonData;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class B_TODOS_DA
    {
        public DataSet Notify_Search(string p_key_search, string p_user_name, string p_from, string p_to, string p_sort_type, ref decimal p_total_record)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_total_record", OracleDbType.Decimal, ParameterDirection.Output);
                DataSet _ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_todos.proc_todo_search",
                    new OracleParameter("p_key_search", OracleDbType.Varchar2, p_key_search, ParameterDirection.Input),
                    new OracleParameter("p_user_name", OracleDbType.Varchar2, p_user_name, ParameterDirection.Input),
                    new OracleParameter("p_from", OracleDbType.Varchar2, p_from, ParameterDirection.Input),
                    new OracleParameter("p_to", OracleDbType.Varchar2, p_to, ParameterDirection.Input),
                    new OracleParameter("p_sort_type", OracleDbType.Varchar2, p_sort_type, ParameterDirection.Input),
                    paramReturn,
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output)
                   );

                p_total_record = Convert.ToDecimal(paramReturn.Value.ToString());
                return _ds;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public DataSet B_Todos_Search(string p_key_search, string p_from, string p_to, string p_sort_type, ref decimal p_total_record)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_total_record", OracleDbType.Decimal, ParameterDirection.Output);
                DataSet _ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_todos.proc_todo_home_search",
                    new OracleParameter("p_key_search", OracleDbType.Varchar2, p_key_search, ParameterDirection.Input),
                    new OracleParameter("p_from", OracleDbType.Varchar2, p_from, ParameterDirection.Input),
                    new OracleParameter("p_to", OracleDbType.Varchar2, p_to, ParameterDirection.Input),
                    new OracleParameter("p_sort_type", OracleDbType.Varchar2, p_sort_type, ParameterDirection.Input),
                    paramReturn,
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output)
                   );

                p_total_record = Convert.ToDecimal(paramReturn.Value.ToString());
                return _ds;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public DataSet B_Remind_Search(string p_key_search, string p_from, string p_to, string p_sort_type, ref decimal p_total_record)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_total_record", OracleDbType.Decimal, ParameterDirection.Output);
                DataSet _ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_remind.proc_remind_home_search",
                    new OracleParameter("p_key_search", OracleDbType.Varchar2, p_key_search, ParameterDirection.Input),
                    new OracleParameter("p_from", OracleDbType.Varchar2, p_from, ParameterDirection.Input),
                    new OracleParameter("p_to", OracleDbType.Varchar2, p_to, ParameterDirection.Input),
                    new OracleParameter("p_sort_type", OracleDbType.Varchar2, p_sort_type, ParameterDirection.Input),
                    paramReturn,
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output)
                   );
                p_total_record = Convert.ToDecimal(paramReturn.Value.ToString());
                return _ds;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }


        public DataSet NotifiGetByCasecode(string p_key_search)
        {
            try
            {
                DataSet _ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_TODOS.PROC_NOTIFI_GETBY_APPS",
                    new OracleParameter("P_KEY_SEARCH", OracleDbType.Varchar2, p_key_search, ParameterDirection.Input),
                    new OracleParameter("P_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("P_CURSOR_RIMIND", OracleDbType.RefCursor, ParameterDirection.Output)
                   );
                return _ds;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public DataSet Todo_GetByCaseCode(decimal p_app_id, string p_processor_by)
        {
            try
            {
                DataSet _ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_TODOS.proc_Todo_GetByCaseCode",
                    new OracleParameter("p_app_id", OracleDbType.Decimal, p_app_id, ParameterDirection.Input),
                    new OracleParameter("p_processor_by", OracleDbType.Varchar2, p_processor_by, ParameterDirection.Input),
                    new OracleParameter("P_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output)
                   );
                return _ds;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public bool UpdateTodo_ByCaseCode(decimal p_app_id, string p_processor_by)
        {
            try
            {
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_TODOS.proc_UpdateTodo_ByCaseCode",
                   new OracleParameter("p_app_id", OracleDbType.Decimal, p_app_id, ParameterDirection.Input),
                   new OracleParameter("p_processor_by", OracleDbType.Varchar2, p_processor_by, ParameterDirection.Input)
                  );
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return false;
            }
        }


        public DataSet RemindGetByCasecode(string p_key_search)
        {
            try
            {
                DataSet _ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_TODOS.PROC_TODO_GETBY_APPS",
                    new OracleParameter("P_KEY_SEARCH", OracleDbType.Varchar2, p_key_search, ParameterDirection.Input),
                    new OracleParameter("P_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output)
                   );
                return _ds;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public DataSet GET_NOTIFY(string P_USERNAME)
        {
            try
            {
                DataSet _ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_TODOS.proc_get_notify",
                    new OracleParameter("P_USERNAME", OracleDbType.Varchar2, P_USERNAME, ParameterDirection.Input),
                    new OracleParameter("P_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output)
                   );
                return _ds;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public bool Remind_Insert_Common(decimal p_type, string p_case_code, decimal p_ref_id, string p_user_name, string p_language_code)
        {
            try
            {
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_remind.proc_do_insert",
                   new OracleParameter("p_type", OracleDbType.Decimal, p_type, ParameterDirection.Input),
                   new OracleParameter("p_case_code", OracleDbType.Varchar2, p_case_code, ParameterDirection.Input),
                   new OracleParameter("p_ref_id", OracleDbType.Decimal, p_ref_id, ParameterDirection.Input),
                   new OracleParameter("p_user_name", OracleDbType.Varchar2, p_user_name, ParameterDirection.Input),
                   new OracleParameter("p_language_code", OracleDbType.Varchar2, p_language_code, ParameterDirection.Input));
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return false;
            }
        }

        public bool Remind_Insert_ByTodo(decimal p_type, string p_case_code, decimal p_ref_id, string p_request_by,
            string p_content, string p_processor_by, string p_language_code)
        {
            try
            {
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_remind.proc_insert_by_todo",
                   new OracleParameter("p_type", OracleDbType.Decimal, p_type, ParameterDirection.Input),
                   new OracleParameter("p_case_code", OracleDbType.Varchar2, p_case_code, ParameterDirection.Input),

                   new OracleParameter("p_content", OracleDbType.Varchar2, p_content, ParameterDirection.Input),
                   new OracleParameter("p_request_by", OracleDbType.Varchar2, p_request_by, ParameterDirection.Input),
                   new OracleParameter("p_request_date", OracleDbType.Date, DateTime.Now, ParameterDirection.Input),
                   new OracleParameter("p_processor_by", OracleDbType.Varchar2, p_processor_by, ParameterDirection.Input),
                   new OracleParameter("p_status", OracleDbType.Decimal, (decimal)Remind_Status_Enum.Active, ParameterDirection.Input),
                   new OracleParameter("p_language_code", OracleDbType.Varchar2, p_language_code, ParameterDirection.Input),
                   new OracleParameter("p_active_date", OracleDbType.Date, DateTime.Now, ParameterDirection.Input),
                   new OracleParameter("p_ref_id", OracleDbType.Decimal, p_ref_id, ParameterDirection.Input));
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return false;
            }
        }
    }
}

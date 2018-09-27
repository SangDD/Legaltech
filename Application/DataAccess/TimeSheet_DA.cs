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
    public class TimeSheet_DA
    {
        public DataSet Timesheet_GetAll()
        {
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_timesheet.proc_timesheet_getall",
                new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public DataSet Timesheet_Search(string p_key_search, string p_from, string p_to, string p_column , string p_sort_type, ref decimal p_total_record)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_total_record", OracleDbType.Decimal, ParameterDirection.Output);
                DataSet _ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_timesheet.proc_timesheet_search",
                    new OracleParameter("p_key_search", OracleDbType.Varchar2, p_key_search, ParameterDirection.Input),
                    new OracleParameter("p_from", OracleDbType.Varchar2, p_from, ParameterDirection.Input),
                    new OracleParameter("p_to", OracleDbType.Varchar2, p_to, ParameterDirection.Input),

                    new OracleParameter("p_column", OracleDbType.Varchar2, p_column, ParameterDirection.Input),
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

        public DataSet Timesheet_GetBy_Id(decimal p_id)
        {
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_timesheet.proc_timesheet_getbyid",
                    new OracleParameter("p_id", OracleDbType.Decimal, p_id, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public DataSet Timesheet_GetBy_Casecode(string p_case_code)
        {
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_timesheet.proc_getbycasecode",
                    new OracleParameter("p_case_code", OracleDbType.Varchar2, p_case_code, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        

        public DataSet Timesheet_GetBy_Lawer(decimal p_lawer_id)
        {
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_timesheet.proc_timesheet_getby_lawer",
                    new OracleParameter("p_lawer_id", OracleDbType.Decimal, p_lawer_id, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public decimal Timesheet_Insert(string p_name, string p_app_case_code, decimal p_lawer_id, DateTime p_time_date, 
            string p_from_time, string p_to_time, decimal p_hours,decimal p_hours_adjust, string p_notes, 
            decimal p_status, string p_created_by, DateTime p_created_date, string p_language_code)
        {
            try
            {
                var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_timesheet.Proc_Timesheet_Insert",
                    new OracleParameter("p_name", OracleDbType.Varchar2, p_name, ParameterDirection.Input),
                    new OracleParameter("p_app_case_code", OracleDbType.Varchar2, p_app_case_code, ParameterDirection.Input),
                    new OracleParameter("p_lawer_id", OracleDbType.Decimal, p_lawer_id, ParameterDirection.Input),
                    new OracleParameter("p_time_date", OracleDbType.Date, p_time_date, ParameterDirection.Input),
                    new OracleParameter("p_from_time", OracleDbType.Varchar2, p_from_time, ParameterDirection.Input),
                    new OracleParameter("p_to_time", OracleDbType.Varchar2, p_to_time, ParameterDirection.Input),

                    new OracleParameter("p_hours", OracleDbType.Decimal, p_hours, ParameterDirection.Input),
                    new OracleParameter("p_hours_adjust", OracleDbType.Decimal, p_hours_adjust, ParameterDirection.Input),

                    new OracleParameter("p_notes", OracleDbType.Varchar2, p_notes, ParameterDirection.Input),
                    new OracleParameter("p_status", OracleDbType.Decimal, p_status, ParameterDirection.Input),
                    new OracleParameter("p_created_by", OracleDbType.Varchar2, p_created_by, ParameterDirection.Input),
                    new OracleParameter("p_created_date", OracleDbType.Date, p_created_date, ParameterDirection.Input),
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

        public decimal Timesheet_Update(decimal p_id, string p_name, DateTime p_time_date, string p_from_time, string p_to_time, decimal p_hours, decimal p_hours_adjust, 
            string p_notes,  string p_modify_by, DateTime p_modify_date)
        {
            try
            {
                var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_timesheet.proc_timesheet_update",
                    new OracleParameter("p_id", OracleDbType.Decimal, p_id, ParameterDirection.Input),
                    new OracleParameter("p_name", OracleDbType.Varchar2, p_name, ParameterDirection.Input),
                    new OracleParameter("p_time_date", OracleDbType.Date, p_time_date, ParameterDirection.Input),
                    new OracleParameter("p_from_time", OracleDbType.Varchar2, p_from_time, ParameterDirection.Input),
                    new OracleParameter("p_to_time", OracleDbType.Varchar2, p_to_time, ParameterDirection.Input),

                    new OracleParameter("p_hours", OracleDbType.Decimal, p_hours, ParameterDirection.Input),
                    new OracleParameter("p_hours_adjust", OracleDbType.Decimal, p_hours_adjust, ParameterDirection.Input),
                    new OracleParameter("p_notes", OracleDbType.Varchar2, p_notes, ParameterDirection.Input),
                    new OracleParameter("p_modify_by", OracleDbType.Varchar2, p_modify_by, ParameterDirection.Input),
                    new OracleParameter("p_modify_date", OracleDbType.Date, p_modify_date, ParameterDirection.Input),
                    paramReturn);

                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal Timesheet_Delete(decimal p_id, string p_modify_by, DateTime p_modify_date)
        {
            try
            {
                var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_timesheet.proc_timesheet_delete",
                    new OracleParameter("p_id", OracleDbType.Decimal, p_id, ParameterDirection.Input),
                    new OracleParameter("p_modify_by", OracleDbType.Varchar2, p_modify_by, ParameterDirection.Input),
                    new OracleParameter("p_modify_date", OracleDbType.Date, p_modify_date, ParameterDirection.Input),
                    paramReturn);

                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal Timesheet_Approve(decimal p_id, int p_status, string p_reject_reason, string p_modify_by, DateTime p_modify_date)
        {
            try
            {
                var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_timesheet.proc_timesheet_approve",
                    new OracleParameter("p_id", OracleDbType.Decimal, p_id, ParameterDirection.Input),
                    //new OracleParameter("p_hours_adjust", OracleDbType.Decimal, p_hours_adjust, ParameterDirection.Input),
                    new OracleParameter("p_status", OracleDbType.Int16, p_status, ParameterDirection.Input),
                    new OracleParameter("p_reject_reason", OracleDbType.Varchar2, p_reject_reason, ParameterDirection.Input),
                    new OracleParameter("p_modify_by", OracleDbType.Varchar2, p_modify_by, ParameterDirection.Input),
                    new OracleParameter("p_modify_date", OracleDbType.Date, p_modify_date, ParameterDirection.Input),
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

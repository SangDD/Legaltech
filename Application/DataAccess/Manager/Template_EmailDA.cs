namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using Common;
    using ObjectInfos;
    using Oracle.DataAccess.Client;

    public class Template_EmailDA
    {
        public DataSet Template_Email_Search(string p_user_name, string p_key_search, string p_from, string p_to, string p_sort_type, ref decimal p_total_record)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_total_record", OracleDbType.Decimal, ParameterDirection.Output);
                DataSet _ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_template_email.proc_search",
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

        public DataSet Template_email_GetAll()
        {
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_template_email.proc_template_email_getall",
                new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public decimal Template_Email_Insert(Template_Email_Info info)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_template_email.Proc_Template_Email_Insert",
                    new OracleParameter("p_name", OracleDbType.Varchar2, info.Name, ParameterDirection.Input),
                    new OracleParameter("p_type", OracleDbType.Varchar2, info.Type, ParameterDirection.Input),
                    new OracleParameter("p_content", OracleDbType.Clob, info.Content, ParameterDirection.Input),
                    new OracleParameter("p_created_by", OracleDbType.Varchar2, info.Created_By, ParameterDirection.Input),
                    paramReturn
                );
                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal Template_Email_Update(Template_Email_Info info)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_template_email.proc_template_email_update",
                    new OracleParameter("p_id", OracleDbType.Decimal, info.Id, ParameterDirection.Input),
                    new OracleParameter("p_name", OracleDbType.Varchar2, info.Name, ParameterDirection.Input),
                    new OracleParameter("p_type", OracleDbType.Varchar2, info.Type, ParameterDirection.Input),
                    new OracleParameter("p_content", OracleDbType.Clob, info.Content, ParameterDirection.Input),
                    new OracleParameter("p_modify_by", OracleDbType.Varchar2, info.Modify_By, ParameterDirection.Input),
                    paramReturn
                );
                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal Template_Email_Delete(decimal p_id)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_template_email.proc_template_email_delete",
                    new OracleParameter("p_id", OracleDbType.Decimal, p_id, ParameterDirection.Input),
                    paramReturn
                );
                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public DataSet Template_Email_GetBy_Id(decimal p_id, string p_language_code)
        {
            try
            {
                DataSet _ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_template_email.proc_getby_id",
                    new OracleParameter("p_id", OracleDbType.Decimal, p_id, ParameterDirection.Input),
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

using Common;
using Oracle.DataAccess.Client;
using System;
using System.Data;


namespace DataAccess
{
    public class App_Lawer_DA
    {
        public DataSet GetApp_Grant4Lawer(decimal p_lawer_id, decimal p_user_type, string p_language_code)
        {
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_lawer.proc_get_appgrant4Lawer",
                    new OracleParameter("p_lawer_id", OracleDbType.Decimal, p_lawer_id, ParameterDirection.Input),
                    new OracleParameter("p_user_type", OracleDbType.Decimal, p_user_type, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, p_language_code, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }


        public decimal App_Lawer_Insert(decimal p_application_header_id, decimal p_lawer_id, string p_notes, string p_language_code,
            string p_created_by, DateTime p_created_date)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_lawer.proc_grant_app2_lawer",
                    new OracleParameter("p_application_header_id", OracleDbType.Decimal, p_application_header_id, ParameterDirection.Input),
                    new OracleParameter("p_lawer_id", OracleDbType.Decimal, p_lawer_id, ParameterDirection.Input),
                    new OracleParameter("p_notes", OracleDbType.Varchar2, p_notes, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, p_language_code, ParameterDirection.Input),
                    new OracleParameter("p_created_by", OracleDbType.Varchar2, p_created_by, ParameterDirection.Input),
                    new OracleParameter("p_created_date", OracleDbType.Date, p_created_date, ParameterDirection.Input),
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

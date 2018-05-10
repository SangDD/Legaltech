﻿using Common;
using Oracle.DataAccess.Client;
using System;
using System.Data;


namespace DataAccess
{
    public class App_Lawer_DA
    {
        public decimal App_Lawer_Insert(decimal p_application_header_id, decimal p_lawer_id, string p_notes, string p_language_code)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_lawer.proc_grant_app2_lawer",
                    new OracleParameter("p_application_header_id", OracleDbType.Decimal, p_application_header_id, ParameterDirection.Input),
                    new OracleParameter("p_lawer_id", OracleDbType.Decimal, p_lawer_id, ParameterDirection.Input),
                    new OracleParameter("p_notes", OracleDbType.Varchar2, p_notes, ParameterDirection.Input),
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

using Common;
using Oracle.DataAccess.Client;
using System;
using System.Data;


namespace DataAccess.ModuleTrademark
{
    public class SysApplicationDA
    {

        public static DataSet SysApplicationGetAll()
        {
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_SYS_APPLICATION.PROC_SYS_APPLICATION_GETALL",
                new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public DataSet Sys_App_Fix_Charge_GetAll()
        {
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_SYS_APPLICATION.proc_sys_app_fix_charge_getall",
                new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public DataSet SysAppFeeFixGetById(decimal pID, string pAppCode)
        {
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_SYS_APPLICATION.PROC_SYS_APP_FIX_GET_BY_ID",
                 new OracleParameter("P_ID", OracleDbType.Decimal, pID, ParameterDirection.Input),
                 new OracleParameter("P_APPCODE", OracleDbType.Varchar2, pAppCode, ParameterDirection.Input),
                 new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public decimal SysAppFeeFixUpdate(decimal p_id, string p_appcode, decimal p_amount, decimal p_amount_usd, decimal p_amount_represent, decimal p_amount_represent_usd, string p_char01, string p_description)
        {
            try
            {
                var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_SYS_APPLICATION.PROC_SYS_APP_FIX_UPDATE",
                   new OracleParameter("p_id", OracleDbType.Decimal, p_id, ParameterDirection.Input),
                   new OracleParameter("p_appcode", OracleDbType.Varchar2, p_appcode, ParameterDirection.Input),
                   new OracleParameter("p_amount", OracleDbType.Decimal, p_amount, ParameterDirection.Input),
                   new OracleParameter("p_amount_usd", OracleDbType.Decimal, p_amount_usd, ParameterDirection.Input),
                   new OracleParameter("p_amount_represent", OracleDbType.Decimal, p_amount_represent, ParameterDirection.Input),
                   new OracleParameter("p_amount_represent_usd", OracleDbType.Decimal, p_amount_represent_usd, ParameterDirection.Input),

                   new OracleParameter("p_char01", OracleDbType.Varchar2, p_char01, ParameterDirection.Input),
                   new OracleParameter("p_description", OracleDbType.Varchar2, p_description, ParameterDirection.Input),
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

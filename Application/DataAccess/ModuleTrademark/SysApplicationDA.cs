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

        public decimal SysAppFeeFixUpdate(decimal pID, string pAppCode, decimal pAmount, string pChar01, string pDescription)
        {
            try
            {
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                decimal preturn = OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_SYS_APPLICATION.PROC_SYS_APP_FIX_GET_BY_ID",
                   new OracleParameter("P_ID", OracleDbType.Decimal, pID, ParameterDirection.Input),
                   new OracleParameter("P_APPCODE", OracleDbType.Varchar2, pAppCode, ParameterDirection.Input),
                   new OracleParameter("P_AMOUNT", OracleDbType.Varchar2, pAppCode, ParameterDirection.Input),
                   new OracleParameter("P_CHAR01", OracleDbType.Varchar2, pAppCode, ParameterDirection.Input),
                   new OracleParameter("P_DESCRIPTION", OracleDbType.Varchar2, pAppCode, ParameterDirection.Input),
                    paramReturn);

                return preturn;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

    }
}

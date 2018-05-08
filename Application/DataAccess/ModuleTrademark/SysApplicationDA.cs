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
    }
}

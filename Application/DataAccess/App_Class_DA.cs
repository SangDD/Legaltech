using Common;
using Common.SearchingAndFiltering;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class App_Class_DA
    {

        public DataSet AppClassGetByID(decimal P_ID)
        {
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_APP_CLASS.PROC_APP_CLASS_GET_BY_ID",
                    new OracleParameter("P_ID", OracleDbType.Decimal, P_ID, ParameterDirection.Input),
                    new OracleParameter("P_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public DataSet SearchAppClass(string keysSearch, OptionFilter options, ref int totalRecord)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("P_TOTAL_RECORD", OracleDbType.Decimal, ParameterDirection.Output);
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_APP_CLASS.PROC_APP_CLASS_SEARCH",
                 new OracleParameter("P_KEY_SEARCH", OracleDbType.Varchar2, keysSearch, ParameterDirection.Input),
                 new OracleParameter("P_FROM", OracleDbType.Varchar2, options.StartAt.ToString(), ParameterDirection.Input),
                 new OracleParameter("P_TO", OracleDbType.Varchar2, options.EndAt.ToString(), ParameterDirection.Input),
                 new OracleParameter("P_SORT_TYPE", OracleDbType.Varchar2, options.OrderBy, ParameterDirection.Input),
                 paramReturn,
                 new OracleParameter("P_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }


        public decimal App_Class_Insert(string P_CODE, string P_NAME_VI, string P_NAME_EN, string P_NAME_CN,
          string P_CREATED_BY, DateTime P_CREATED_DATE)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_APP_CLASS.PROC_CLASS_ADD",
                    new OracleParameter("P_CODE", OracleDbType.Varchar2, P_CODE, ParameterDirection.Input),
                    new OracleParameter("P_NAME_VI", OracleDbType.Varchar2, P_NAME_VI, ParameterDirection.Input),
                    new OracleParameter("P_NAME_EN", OracleDbType.Varchar2, P_NAME_EN, ParameterDirection.Input),
                      new OracleParameter("P_NAME_CN", OracleDbType.Varchar2, P_NAME_CN, ParameterDirection.Input),
                    new OracleParameter("P_CREATED_BY", OracleDbType.Varchar2, P_CREATED_BY, ParameterDirection.Input),
                    new OracleParameter("P_CREATED_DATE", OracleDbType.Date, P_CREATED_DATE, ParameterDirection.Input),
                    paramReturn);

                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal App_Class_Update(decimal P_ID, string P_CODE, string P_NAME_VI, string P_NAME_EN, string P_NAME_CN,
        string P_MODIFIED_BY, DateTime P_MODIFIED_DATE)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_APP_CLASS.PROC_CLASS_UPDATE",
                    new OracleParameter("P_ID", OracleDbType.Decimal, P_ID, ParameterDirection.Input),
                    new OracleParameter("P_CODE", OracleDbType.Varchar2, P_CODE, ParameterDirection.Input),
                    new OracleParameter("P_NAME_VI", OracleDbType.Varchar2, P_NAME_VI, ParameterDirection.Input),
                    new OracleParameter("P_NAME_EN", OracleDbType.Varchar2, P_NAME_EN, ParameterDirection.Input),
                     new OracleParameter("P_NAME_CN", OracleDbType.Varchar2, P_NAME_CN, ParameterDirection.Input),
                    new OracleParameter("P_MODIFIED_BY", OracleDbType.Varchar2, P_MODIFIED_BY, ParameterDirection.Input),
                    new OracleParameter("P_MODIFIED_DATE", OracleDbType.Date, P_MODIFIED_DATE, ParameterDirection.Input),
                    paramReturn);

                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal App_Class_Delete(decimal P_ID, string P_MODIFIED_BY, DateTime P_MODIFIED_DATE)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("P_RETURN", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_APP_CLASS.PROC_CLASS_DELETE",
                    new OracleParameter("P_ID", OracleDbType.Decimal, P_ID, ParameterDirection.Input),
                    new OracleParameter("P_MODIFIED_BY", OracleDbType.Varchar2, P_MODIFIED_BY, ParameterDirection.Input),
                    new OracleParameter("P_MODIFIED_DATE", OracleDbType.Date, P_MODIFIED_DATE, ParameterDirection.Input),
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

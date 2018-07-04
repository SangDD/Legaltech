using Common;
using Common.Converters;
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
   public class WikiCatalogue_DA
    {
        public DataSet WikiCatalogue_GetAll(decimal P_ID)
        {
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_WIKI_CATALOUGE.PROC_WIKI_CATALOGUES_GETALL",
                    new OracleParameter("P_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }


        public decimal WikiCatalogue_Insert(string P_NAME, string P_NAME_ENG, Decimal P_CATA_LEVEL,  
          string P_CREATED_BY, DateTime P_CREATED_DATE)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_WIKI_CATALOUGE.PROC_WIKI_CATALOGUES_INSERT",
                    new OracleParameter("P_NAME", OracleDbType.Varchar2, P_NAME, ParameterDirection.Input),
                    new OracleParameter("P_NAME_ENG", OracleDbType.Varchar2, P_NAME_ENG, ParameterDirection.Input),
                    new OracleParameter("P_CATA_LEVEL", OracleDbType.Varchar2, P_CATA_LEVEL, ParameterDirection.Input),
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

        public decimal WikiCatalogue_Update(decimal P_ID, string P_NAME, string P_NAME_ENG, Decimal P_CATA_LEVEL,
         string P_MODIFIED_BY, DateTime P_MODIFIED_DATE)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_WIKI_CATALOUGE.PROC_WIKI_CATALOGUES_UPDATE",
                    new OracleParameter("P_ID", OracleDbType.Decimal, P_ID, ParameterDirection.Input),
                    new OracleParameter("P_NAME", OracleDbType.Varchar2, P_NAME, ParameterDirection.Input),
                    new OracleParameter("P_NAME_ENG", OracleDbType.Varchar2, P_NAME_ENG, ParameterDirection.Input),
                    new OracleParameter("P_CATA_LEVEL", OracleDbType.Varchar2, P_CATA_LEVEL, ParameterDirection.Input),
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

        public decimal WikiCatalogue_Delete(decimal P_ID)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_WIKI_CATALOUGE.PROC_WIKI_CATALOGUES_DELETE",
                    new OracleParameter("P_ID", OracleDbType.Decimal, P_ID, ParameterDirection.Input),
                    paramReturn);
                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public DataSet WikiCatalogue_GetByID(decimal P_ID)
        {
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_WIKI_CATALOUGE.PROC_WIKI_CATALOGUES_GETBYID",
                    new OracleParameter("P_ID", OracleDbType.Decimal, P_ID, ParameterDirection.Input),
                    new OracleParameter("P_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

    }
}

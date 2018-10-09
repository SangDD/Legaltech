using Common;
using ObjectInfos;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class Sys_Search_Fix_DA
    {
        public DataSet Sys_Search_Fix_GetAll()
        {
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_sys_search_fix.proc_getall",
                new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public DataSet Sys_Search_Fix_GetById(decimal p_id)
        {
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_sys_search_fix.proc_get_by_id",
                    new OracleParameter("p_id", OracleDbType.Decimal, p_id, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public decimal Sys_Search_Fee_Insert(Sys_Search_Fix_Info p_obj)
        {
            try
            {
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_sys_search_fix.Proc_Sys_Search_Fee_Insert",
                    new OracleParameter("p_country_id", OracleDbType.Decimal, p_obj.Country_Id, ParameterDirection.Input),
                    new OracleParameter("p_search_object", OracleDbType.Decimal, p_obj.Search_Object, ParameterDirection.Input),
                    new OracleParameter("p_search_type", OracleDbType.Decimal, p_obj.Search_Type, ParameterDirection.Input),
                    new OracleParameter("p_amount", OracleDbType.Decimal, p_obj.Amount, ParameterDirection.Input),
                    new OracleParameter("p_amount_usd", OracleDbType.Decimal, p_obj.Amount_usd, ParameterDirection.Input),
                    new OracleParameter("p_created_by", OracleDbType.Varchar2, p_obj.Created_By, ParameterDirection.Input)
                );
                return 0;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal Sys_Search_Fee_Update(Sys_Search_Fix_Info p_obj)
        {
            try
            {
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_sys_search_fix.proc_sys_search_fee_update",
                    new OracleParameter("p_id", OracleDbType.Decimal, p_obj.Id, ParameterDirection.Input),
                    new OracleParameter("p_country_id", OracleDbType.Decimal, p_obj.Country_Id, ParameterDirection.Input),
                    new OracleParameter("p_search_object", OracleDbType.Decimal, p_obj.Search_Object, ParameterDirection.Input),
                    new OracleParameter("p_search_type", OracleDbType.Decimal, p_obj.Search_Type, ParameterDirection.Input),
                    new OracleParameter("p_amount", OracleDbType.Decimal, p_obj.Amount, ParameterDirection.Input),
                    new OracleParameter("p_amount_usd", OracleDbType.Decimal, p_obj.Amount_usd, ParameterDirection.Input),
                    new OracleParameter("p_modified_by", OracleDbType.Varchar2, p_obj.Modified_By, ParameterDirection.Input)
                );
                return 0;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal Sys_Search_Fee_Delete(decimal p_id, string p_modified_by)
        {
            try
            {
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_sys_search_fix.proc_delete",
                    new OracleParameter("p_id", OracleDbType.Decimal, p_id, ParameterDirection.Input),
                    new OracleParameter("p_modified_by", OracleDbType.Varchar2, p_modified_by, ParameterDirection.Input)
                );
                return 0;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

    }
}

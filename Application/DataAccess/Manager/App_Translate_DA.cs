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
    public class App_Translate_DA
    {

        public DataSet App_Translate_GetBy_AppId(decimal p_app_header_id)
        {
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_translate.proc_getby_app_id",
                    new OracleParameter("p_app_header_id", OracleDbType.Decimal, p_app_header_id, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public DataSet App_Translate_GetBy_Casecode(string p_case_code)
        {
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_translate.proc_getby_app_case_code",
                    new OracleParameter("p_case_code", OracleDbType.Varchar2, p_case_code, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public DataSet Sys_App_Translate_GetBy_Casecode(string p_appcode)
        {
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_translate.proc_sys_get_Appcode",
                    new OracleParameter("p_appcode", OracleDbType.Varchar2, p_appcode, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }


        public DataSet AppDetail_GetBy_Id(string p_appcode, decimal p_app_header_id)
        {
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_translate.proc_app_getby_app_id",
                    new OracleParameter("p_appcode", OracleDbType.Varchar2, p_appcode, ParameterDirection.Input),
                    new OracleParameter("p_app_header_id", OracleDbType.Decimal, p_app_header_id, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursor_detail", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursor_class", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }


        public decimal App_Translate_Insert(List<App_Translate_Info> pInfo)
        {
            try
            {
                OracleHelper.ExcuteBatchNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_translate.Proc_App_Translate_Insert",pInfo.Count,
                    new OracleParameter("p_app_header_id", OracleDbType.Decimal, pInfo.Select(o => o.App_Header_Id).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_case_code", OracleDbType.Varchar2, pInfo.Select(o => o.Case_Code).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_object_name", OracleDbType.Varchar2, pInfo.Select(o => o.Object_Name).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_value_old", OracleDbType.Varchar2, pInfo.Select(o => o.Value_Old).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_value_translate", OracleDbType.Varchar2, pInfo.Select(o => o.Value_Translate).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_type", OracleDbType.Varchar2, pInfo.Select(o => o.Type).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_language", OracleDbType.Varchar2, pInfo.Select(o => o.Language).ToArray(), ParameterDirection.Input));

                return 0;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal App_Translate_Delete_ByAppId(decimal p_app_header_id)
        {
            try
            {
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_translate.proc_delete_by_id",
                    new OracleParameter("p_app_header_id", OracleDbType.Decimal, p_app_header_id, ParameterDirection.Input));

                return 0;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal App_Translate_Delete_ByCaseCode(string p_case_code)
        {
            try
            {
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_translate.proc_delete_by_case_code",
                    new OracleParameter("p_case_code", OracleDbType.Varchar2, p_case_code, ParameterDirection.Input));

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

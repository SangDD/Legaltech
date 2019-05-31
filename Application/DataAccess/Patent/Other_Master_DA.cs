using Common;
using ObjectInfos;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DataAccess
{
    public class Other_Master_DA
    {
        public decimal Insert(List<Other_MasterInfo> pInfo)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExcuteBatchNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_other_master.Proc_Other_Master_Insert", pInfo.Count,
                     new OracleParameter("p_id", OracleDbType.Decimal, pInfo.Select(o => o.Id).ToArray(), ParameterDirection.Input),
                     new OracleParameter("p_app_header_id", OracleDbType.Decimal, pInfo.Select(o => o.App_Header_Id).ToArray(), ParameterDirection.Input),
                     new OracleParameter("p_case_code", OracleDbType.Varchar2, pInfo.Select(o => o.Case_Code).ToArray(), ParameterDirection.Input),
                     new OracleParameter("p_master_name", OracleDbType.Varchar2, pInfo.Select(o => o.Master_Name).ToArray(), ParameterDirection.Input),
                     new OracleParameter("p_master_address", OracleDbType.Varchar2, pInfo.Select(o => o.Master_Address).ToArray(), ParameterDirection.Input),
                     new OracleParameter("p_master_phone", OracleDbType.Varchar2, pInfo.Select(o => o.Master_Phone).ToArray(), ParameterDirection.Input),
                     new OracleParameter("p_master_fax", OracleDbType.Varchar2, pInfo.Select(o => o.Master_Fax).ToArray(), ParameterDirection.Input),
                     new OracleParameter("p_master_email", OracleDbType.Varchar2, pInfo.Select(o => o.Master_Email).ToArray(), ParameterDirection.Input),
                     new OracleParameter("p_tacgiadongthoi", OracleDbType.Varchar2, pInfo.Select(o => o.TacGiaDongThoi).ToArray(), ParameterDirection.Input),
                     new OracleParameter("p_phoban", OracleDbType.Varchar2, pInfo.Select(o => o.PhoBan).ToArray(), ParameterDirection.Input),
                    paramReturn);
                Oracle.DataAccess.Types.OracleDecimal[] totalReturn = (Oracle.DataAccess.Types.OracleDecimal[])paramReturn.Value;
                foreach (Oracle.DataAccess.Types.OracleDecimal _item in totalReturn)
                {
                    if (Convert.ToDecimal(_item.Value.ToString()) < 0)
                    {
                        return Convert.ToDecimal(_item.Value.ToString());
                    }
                }

                return 1;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public decimal Update(Other_MasterInfo pInfo)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_other_master.proc_other_master_update",
                     new OracleParameter("p_id", OracleDbType.Decimal, pInfo.Id, ParameterDirection.Input),
                     new OracleParameter("p_app_header_id", OracleDbType.Decimal, pInfo.App_Header_Id, ParameterDirection.Input),
                     new OracleParameter("p_case_code", OracleDbType.Varchar2, pInfo.Case_Code, ParameterDirection.Input),
                     new OracleParameter("p_master_name", OracleDbType.Varchar2, pInfo.Master_Name, ParameterDirection.Input),
                     new OracleParameter("p_master_address", OracleDbType.Varchar2, pInfo.Master_Address, ParameterDirection.Input),
                     new OracleParameter("p_master_phone", OracleDbType.Varchar2, pInfo.PhoBan, ParameterDirection.Input),
                     new OracleParameter("p_master_fax", OracleDbType.Varchar2, pInfo.Master_Fax, ParameterDirection.Input),
                     new OracleParameter("p_master_email", OracleDbType.Varchar2, pInfo.Master_Email, ParameterDirection.Input),
                     new OracleParameter("p_tacgiadongthoi", OracleDbType.Varchar2, pInfo.TacGiaDongThoi, ParameterDirection.Input),
                     new OracleParameter("p_phoban", OracleDbType.Varchar2, pInfo.PhoBan, ParameterDirection.Input),

                    paramReturn);
                var result = Convert.ToDecimal(paramReturn.Value.ToString());
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public int Deleted(string p_case_code, string pLanguage)
        {
            try
            {
                var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_other_master.Proc_Other_Master_delete",
                    new OracleParameter("p_case_code", OracleDbType.Varchar2, p_case_code, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, pLanguage, ParameterDirection.Input),
                    paramReturn);

                var result = Convert.ToInt32(paramReturn.Value.ToString());
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

    }
}

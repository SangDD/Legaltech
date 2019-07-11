using Common;
using ObjectInfos;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DataAccess
{
    public class UuTien_DA
    {
        public decimal Insert(List<UTienInfo> pInfo)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExcuteBatchNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_don_uu_tien.Proc_App_Don_Uu_Tien_Insert", pInfo.Count,
                    new OracleParameter("p_id", OracleDbType.Decimal, pInfo.Select(o => o.Id).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_app_header_id", OracleDbType.Decimal, pInfo.Select(o => o.App_Header_Id).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_case_code", OracleDbType.Varchar2, pInfo.Select(o => o.Case_Code).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_ut_sodon", OracleDbType.Varchar2, pInfo.Select(o => o.UT_SoDon).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_ut_ngaynopdon", OracleDbType.Date, pInfo.Select(o => o.UT_NgayNopDon).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_ut_quocgia", OracleDbType.Decimal, pInfo.Select(o => o.UT_QuocGia).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_ut_type", OracleDbType.Varchar2, pInfo.Select(o => o.UT_Type).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_ut_thoathuankhac", OracleDbType.Varchar2, pInfo.Select(o => o.UT_ThoaThuanKhac).ToArray(), ParameterDirection.Input),
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

        public decimal Update(UTienInfo pInfo)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_don_uu_tien.proc_app_don_uu_tien_update",
                    new OracleParameter("p_id", OracleDbType.Decimal, pInfo.Id, ParameterDirection.Input),
                    new OracleParameter("p_case_code", OracleDbType.Decimal, pInfo.Case_Code, ParameterDirection.Input),
                    new OracleParameter("p_ut_sodon", OracleDbType.Varchar2, pInfo.UT_SoDon, ParameterDirection.Input),
                    new OracleParameter("p_ut_ngaynopdon", OracleDbType.Date, pInfo.UT_NgayNopDon, ParameterDirection.Input),
                    new OracleParameter("p_ut_quocgia", OracleDbType.Decimal, pInfo.UT_QuocGia, ParameterDirection.Input),
                    new OracleParameter("p_ut_type", OracleDbType.Varchar2, pInfo.UT_Type, ParameterDirection.Input),
                    new OracleParameter("p_ut_thoathuankhac", OracleDbType.Varchar2, pInfo.UT_ThoaThuanKhac, ParameterDirection.Input),
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
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_don_uu_tien.Proc_delete",
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

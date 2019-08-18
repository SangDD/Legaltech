using Common;
using ObjectInfos;
using ObjectInfos.ModuleTrademark;
using Oracle.DataAccess.Client;
using System;
using System.Data;
namespace DataAccess
{
    public class A02_DA
    {
        public decimal Insert(A02_Info pInfo)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_A02.PROC_APP_DETAIL_A02_INSERT",
                    paramReturn,
                    new OracleParameter("P_CASE_CODE", OracleDbType.Varchar2, pInfo.Case_Code, ParameterDirection.Input),
                    new OracleParameter("P_APP_HEADER_ID", OracleDbType.Decimal, pInfo.App_Header_Id, ParameterDirection.Input),
                    new OracleParameter("P_LANGUAGE_CODE", OracleDbType.Varchar2, pInfo.Language_Code, ParameterDirection.Input),
                    new OracleParameter("P_TENTHIETKE", OracleDbType.Varchar2, pInfo.TenThietKe, ParameterDirection.Input),
                    new OracleParameter("P_NGAYTAOTHIETKE", OracleDbType.Date, pInfo.NgayTaoThietKe, ParameterDirection.Input),
                    new OracleParameter("P_KHAITHACTM", OracleDbType.Varchar2, pInfo.KhaiThactm, ParameterDirection.Input),
                    new OracleParameter("P_KHAITHACTM_TAINUOC", OracleDbType.Varchar2, pInfo.KhaiThactm_TaiNuoc, ParameterDirection.Input),
                    new OracleParameter("P_KHAITHACTM_NGAY", OracleDbType.Date, pInfo.KhaiThactm_Ngay, ParameterDirection.Input),
                    new OracleParameter("P_CHUCNANG", OracleDbType.Varchar2, pInfo.ChucNang, ParameterDirection.Input),
                    new OracleParameter("P_CHUCNANG_OTHER", OracleDbType.Varchar2, pInfo.ChucNang_Other, ParameterDirection.Input),
                    new OracleParameter("P_CAUTRUC", OracleDbType.Varchar2, pInfo.CauTruc, ParameterDirection.Input),
                    new OracleParameter("P_CAUTRUC_OTHER", OracleDbType.Varchar2, pInfo.CauTruc_OTHER, ParameterDirection.Input),
                    new OracleParameter("P_CONGNGHE", OracleDbType.Varchar2, pInfo.CongNghe, ParameterDirection.Input),
                    new OracleParameter("P_CONGNGHE_OTHER", OracleDbType.Varchar2, pInfo.CongNghe_OTHER, ParameterDirection.Input),
                    new OracleParameter("P_MOTATOMTAT", OracleDbType.Varchar2, pInfo.MoTaTomTat, ParameterDirection.Input),
                    new OracleParameter("P_SOANH", OracleDbType.Decimal, pInfo.SoAnh, ParameterDirection.Input)
                    
                    );
                var result = Convert.ToDecimal(paramReturn.Value.ToString());
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }


        public decimal UpDate(A02_Info pInfo)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_A02.PROC_APP_DETAIL_A02_UPDATE",
                    paramReturn,
                    new OracleParameter("P_ID", OracleDbType.Decimal, pInfo.Id, ParameterDirection.Input),
                    new OracleParameter("P_CASE_CODE", OracleDbType.Varchar2, pInfo.Case_Code, ParameterDirection.Input),
                    new OracleParameter("P_APP_HEADER_ID", OracleDbType.Decimal, pInfo.App_Header_Id, ParameterDirection.Input),
                    new OracleParameter("P_LANGUAGE_CODE", OracleDbType.Varchar2, pInfo.Language_Code, ParameterDirection.Input),
                    new OracleParameter("P_TENTHIETKE", OracleDbType.Varchar2, pInfo.TenThietKe, ParameterDirection.Input),
                    new OracleParameter("P_NGAYTAOTHIETKE", OracleDbType.Date, pInfo.NgayTaoThietKe, ParameterDirection.Input),
                    new OracleParameter("P_KHAITHACTM", OracleDbType.Varchar2, pInfo.KhaiThactm, ParameterDirection.Input),
                    new OracleParameter("P_KHAITHACTM_TAINUOC", OracleDbType.Varchar2, pInfo.KhaiThactm_TaiNuoc, ParameterDirection.Input),
                    new OracleParameter("P_KHAITHACTM_NGAY", OracleDbType.Date, pInfo.KhaiThactm_Ngay, ParameterDirection.Input),
                    new OracleParameter("P_CHUCNANG", OracleDbType.Varchar2, pInfo.ChucNang, ParameterDirection.Input),
                    new OracleParameter("P_CHUCNANG_OTHER", OracleDbType.Varchar2, pInfo.ChucNang_Other, ParameterDirection.Input),
                    new OracleParameter("P_CAUTRUC", OracleDbType.Varchar2, pInfo.CauTruc, ParameterDirection.Input),
                    new OracleParameter("P_CAUTRUC_OTHER", OracleDbType.Varchar2, pInfo.CauTruc_OTHER, ParameterDirection.Input),
                    new OracleParameter("P_CONGNGHE", OracleDbType.Varchar2, pInfo.CongNghe, ParameterDirection.Input),
                    new OracleParameter("P_CONGNGHE_OTHER", OracleDbType.Varchar2, pInfo.CongNghe_OTHER, ParameterDirection.Input),
                    new OracleParameter("P_MOTATOMTAT", OracleDbType.Varchar2, pInfo.MoTaTomTat, ParameterDirection.Input),
                    new OracleParameter("P_SOANH", OracleDbType.Decimal, pInfo.SoAnh, ParameterDirection.Input)

                    );
                var result = Convert.ToDecimal(paramReturn.Value.ToString());
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public int Deleted(decimal p_app_header_id, string pAppCode, string pLanguage)
        {
            try
            {
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_A02.PROC_APP_DETAIL_A02_DELETE",
                    new OracleParameter("P_APP_HEADER_ID", OracleDbType.Decimal, p_app_header_id, ParameterDirection.Input),
                    new OracleParameter("P_APPCODE", OracleDbType.Varchar2, pAppCode, ParameterDirection.Input),
                    new OracleParameter("P_LANGUAGE_CODE", OracleDbType.Varchar2, pLanguage, ParameterDirection.Input),
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

        public DataSet GetByID(decimal p_app_header_id, string p_language_code)
        {
            try
            {
                DataSet _ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_A02.PROC_GETBY_ID",
                    new OracleParameter("P_APP_HEADER_ID", OracleDbType.Decimal, p_app_header_id, ParameterDirection.Input),
                    new OracleParameter("P_LANGUAGE_CODE", OracleDbType.Varchar2, p_language_code, ParameterDirection.Input),
                    new OracleParameter("P_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("P_CURSORHEADER", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("P_CURSOR_DOC", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("P_CURSOR_FEE", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("P_CURSOR_OTHER_MASTER", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("P_CURSOR_AUTHOR", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("P_CURSOR_OTHER_DOC", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("P_CURSOR_DOC_DESIGN", OracleDbType.RefCursor, ParameterDirection.Output)
                    );
                return _ds;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

    }
}

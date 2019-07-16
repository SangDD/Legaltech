using Common;
using ObjectInfos;
using ObjectInfos.ModuleTrademark;
using Oracle.DataAccess.Client;
using System;
using System.Data;
namespace DataAccess
{
    public class A03_DA
    {
        public decimal Insert(A03_Info pInfo)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_A03.PROC_APP_DETAIL_A03_INSERT",
                    paramReturn,
                    new OracleParameter("P_ID", OracleDbType.Decimal, pInfo.Id, ParameterDirection.Input),
                    new OracleParameter("P_CASE_CODE", OracleDbType.Varchar2, pInfo.Case_Code, ParameterDirection.Input),
                    new OracleParameter("P_APP_HEADER_ID", OracleDbType.Decimal, pInfo.App_Header_Id, ParameterDirection.Input),
                    new OracleParameter("P_LANGUAGE_CODE", OracleDbType.Varchar2, pInfo.Language_Code, ParameterDirection.Input),
                    new OracleParameter("P_REFAPPNO", OracleDbType.Varchar2, pInfo.Refappno, ParameterDirection.Input),
                    new OracleParameter("P_APP_SENTDATE", OracleDbType.Date, pInfo.App_Sentdate, ParameterDirection.Input),
                    new OracleParameter("P_INDUSTRY_DESIGN_NAME", OracleDbType.Varchar2, pInfo.Industry_Design_Name, ParameterDirection.Input),
                    new OracleParameter("P_KQTD_TYPE", OracleDbType.Decimal, pInfo.Kqtd_Type, ParameterDirection.Input),
                    new OracleParameter("P_KQTD_VALUE", OracleDbType.Decimal, pInfo.Kqtd_Value, ParameterDirection.Input),
                    new OracleParameter("P_PHANLOAI_TYPE", OracleDbType.Decimal, pInfo.Phanloai_Type, ParameterDirection.Input),
                    new OracleParameter("P_USED_SPECIAL", OracleDbType.Decimal, pInfo.USED_SPECIAL, ParameterDirection.Input),
                    new OracleParameter("P_CLASS_CONTENT", OracleDbType.Varchar2, pInfo.Class_Content, ParameterDirection.Input),
                    new OracleParameter("P_SOHINHPHATSINH", OracleDbType.Decimal, pInfo.SoHinhPhatSinh, ParameterDirection.Input),
                    new OracleParameter("P_SO_PA", OracleDbType.Decimal, pInfo.SO_PA, ParameterDirection.Input),
                    new OracleParameter("P_SO_HA", OracleDbType.Decimal, pInfo.SO_HA, ParameterDirection.Input)
                    
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


        public decimal UpDate(A03_Info pInfo)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_A03.PROC_APP_DETAIL_A03_UPDATE",
                    paramReturn,
                    new OracleParameter("P_ID", OracleDbType.Decimal, pInfo.Id, ParameterDirection.Input),
                    new OracleParameter("P_CASE_CODE", OracleDbType.Varchar2, pInfo.Case_Code, ParameterDirection.Input),
                    new OracleParameter("P_APP_HEADER_ID", OracleDbType.Decimal, pInfo.App_Header_Id, ParameterDirection.Input),
                    new OracleParameter("P_LANGUAGE_CODE", OracleDbType.Varchar2, pInfo.Language_Code, ParameterDirection.Input),
                    new OracleParameter("P_REFAPPNO", OracleDbType.Varchar2, pInfo.Refappno, ParameterDirection.Input),
                    new OracleParameter("P_APP_SENTDATE", OracleDbType.Date, pInfo.App_Sentdate, ParameterDirection.Input),
                    new OracleParameter("P_INDUSTRY_DESIGN_NAME", OracleDbType.Varchar2, pInfo.Industry_Design_Name, ParameterDirection.Input),
                    new OracleParameter("P_KQTD_TYPE", OracleDbType.Decimal, pInfo.Kqtd_Type, ParameterDirection.Input),
                    new OracleParameter("P_KQTD_VALUE", OracleDbType.Decimal, pInfo.Kqtd_Value, ParameterDirection.Input),
                    new OracleParameter("P_PHANLOAI_TYPE", OracleDbType.Decimal, pInfo.Phanloai_Type, ParameterDirection.Input),
                    new OracleParameter("P_USED_SPECIAL", OracleDbType.Decimal, pInfo.USED_SPECIAL, ParameterDirection.Input),
                    new OracleParameter("P_CLASS_CONTENT", OracleDbType.Varchar2, pInfo.Class_Content, ParameterDirection.Input),
                    new OracleParameter("P_SOHINHPHATSINH", OracleDbType.Decimal, pInfo.SoHinhPhatSinh, ParameterDirection.Input),
                    new OracleParameter("P_SO_PA", OracleDbType.Decimal, pInfo.SO_PA, ParameterDirection.Input),
                    new OracleParameter("P_SO_HA", OracleDbType.Decimal, pInfo.SO_HA, ParameterDirection.Input)
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
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_A03.PROC_APP_DETAIL_A03_DELETE",
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
                DataSet _ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_A03.PROC_GETBY_ID",
                    new OracleParameter("P_APP_HEADER_ID", OracleDbType.Decimal, p_app_header_id, ParameterDirection.Input),
                    new OracleParameter("P_LANGUAGE_CODE", OracleDbType.Varchar2, p_language_code, ParameterDirection.Input),
                    new OracleParameter("P_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("P_CURSORHEADER", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("P_CURSOR_DOC", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("P_CURSOR_FEE", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("P_CURSOR_OTHER_MASTER", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("P_CURSOR_AUTHOR", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("P_CURSOR_CLASS", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("P_CURSOR_OTHER_DOC", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("P_CURSOR_UU_TIEN", OracleDbType.RefCursor, ParameterDirection.Output),
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

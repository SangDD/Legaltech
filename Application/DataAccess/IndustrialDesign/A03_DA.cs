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
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_A03.Proc_App_Detail_A03_Insert",
                    new OracleParameter("p_id", OracleDbType.Decimal, pInfo.Id, ParameterDirection.Input),
                    new OracleParameter("p_case_code", OracleDbType.Varchar2, pInfo.Case_Code, ParameterDirection.Input),
                    new OracleParameter("p_app_header_id", OracleDbType.Decimal, pInfo.App_Header_Id, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, pInfo.Language_Code, ParameterDirection.Input),
                    new OracleParameter("p_appno", OracleDbType.Varchar2, pInfo.Appno, ParameterDirection.Input),
                    new OracleParameter("p_patent_type", OracleDbType.Varchar2, pInfo.Patent_Type, ParameterDirection.Input),
                    new OracleParameter("p_patent_name", OracleDbType.Varchar2, pInfo.Patent_Name, ParameterDirection.Input),
                    new OracleParameter("p_source_pct", OracleDbType.Varchar2, pInfo.Source_PCT, ParameterDirection.Input),
                    new OracleParameter("p_pct_number", OracleDbType.Varchar2, pInfo.PCT_Number, ParameterDirection.Input),
                    new OracleParameter("p_pct_filling_date_qt", OracleDbType.Date, pInfo.PCT_Filling_Date_Qt, ParameterDirection.Input),
                    new OracleParameter("p_pct_number_qt", OracleDbType.Varchar2, pInfo.PCT_Number_Qt, ParameterDirection.Input),
                    new OracleParameter("p_pct_date", OracleDbType.Date, pInfo.PCT_Date, ParameterDirection.Input),
                    new OracleParameter("p_pct_vn_date", OracleDbType.Date, pInfo.PCT_VN_Date, ParameterDirection.Input),
                    new OracleParameter("p_pct_suadoi", OracleDbType.Decimal, pInfo.PCT_Suadoi, ParameterDirection.Input),
                    new OracleParameter("p_pct_suadoi_type", OracleDbType.Varchar2, pInfo.PCT_Suadoi_Type, ParameterDirection.Input),
                    new OracleParameter("p_pct_suadoi_content", OracleDbType.Varchar2, pInfo.PCT_Suadoi_Content, ParameterDirection.Input),
                    new OracleParameter("p_source_dqsc", OracleDbType.Varchar2, pInfo.Source_DQSC, ParameterDirection.Input),
                    new OracleParameter("p_dqsc_origin_app_no", OracleDbType.Varchar2, pInfo.DQSC_Origin_App_No, ParameterDirection.Input),
                    new OracleParameter("p_dqsc_filling_date", OracleDbType.Date, pInfo.DQSC_Filling_Date, ParameterDirection.Input),
                    new OracleParameter("p_dqsc_valid_before", OracleDbType.Decimal, pInfo.DQSC_Valid_Before, ParameterDirection.Input),
                    new OracleParameter("p_dqsc_valid_after", OracleDbType.Decimal, pInfo.DQSC_Valid_After, ParameterDirection.Input),
                    new OracleParameter("p_source_gphi", OracleDbType.Varchar2, pInfo.Source_GPHI, ParameterDirection.Input),
                    new OracleParameter("p_gphi_origin_app_no", OracleDbType.Varchar2, pInfo.GPHI_Origin_App_No, ParameterDirection.Input),
                    new OracleParameter("p_gphi_filling_date", OracleDbType.Date, pInfo.GPHI_Filling_Date, ParameterDirection.Input),
                    new OracleParameter("p_gphi_valid_before", OracleDbType.Decimal, pInfo.GPHI_Valid_Before, ParameterDirection.Input),
                    new OracleParameter("p_gphi_valid_after", OracleDbType.Decimal, pInfo.GPHI_Valid_After, ParameterDirection.Input),

                    new OracleParameter("p_Point", OracleDbType.Decimal, pInfo.Point, ParameterDirection.Input),
                    new OracleParameter("p_ThamDinhNoiDung", OracleDbType.Varchar2, pInfo.ThamDinhNoiDung, ParameterDirection.Input),
                    new OracleParameter("p_ChuyenDoiDon", OracleDbType.Varchar2, pInfo.ChuyenDoiDon, ParameterDirection.Input),

                    new OracleParameter("p_class_type", OracleDbType.Varchar2, pInfo.Class_Type, ParameterDirection.Input),
                    new OracleParameter("p_class_content", OracleDbType.Varchar2, pInfo.Class_Content, ParameterDirection.Input),
                    new OracleParameter("p_Used_Special", OracleDbType.Decimal, pInfo.Used_Special, ParameterDirection.Input),
                    
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


        public decimal UpDate(A03_Info pInfo)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_A03.proc_app_detail_A03_update",
                    new OracleParameter("p_id", OracleDbType.Decimal, pInfo.Id, ParameterDirection.Input),
                    new OracleParameter("p_case_code", OracleDbType.Varchar2, pInfo.Case_Code, ParameterDirection.Input),
                    new OracleParameter("p_app_header_id", OracleDbType.Decimal, pInfo.App_Header_Id, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, pInfo.Language_Code, ParameterDirection.Input),
                    new OracleParameter("p_appno", OracleDbType.Varchar2, pInfo.Appno, ParameterDirection.Input),
                    new OracleParameter("p_patent_type", OracleDbType.Varchar2, pInfo.Patent_Type, ParameterDirection.Input),
                    new OracleParameter("p_patent_name", OracleDbType.Varchar2, pInfo.Patent_Name, ParameterDirection.Input),
                    new OracleParameter("p_source_pct", OracleDbType.Varchar2, pInfo.Source_PCT, ParameterDirection.Input),
                    new OracleParameter("p_pct_number", OracleDbType.Varchar2, pInfo.PCT_Number, ParameterDirection.Input),
                    new OracleParameter("p_pct_filling_date_qt", OracleDbType.Date, pInfo.PCT_Filling_Date_Qt, ParameterDirection.Input),
                    new OracleParameter("p_pct_number_qt", OracleDbType.Varchar2, pInfo.PCT_Number_Qt, ParameterDirection.Input),
                    new OracleParameter("p_pct_date", OracleDbType.Date, pInfo.PCT_Date, ParameterDirection.Input),
                    new OracleParameter("p_pct_vn_date", OracleDbType.Date, pInfo.PCT_VN_Date, ParameterDirection.Input),
                    new OracleParameter("p_pct_suadoi", OracleDbType.Decimal, pInfo.PCT_Suadoi, ParameterDirection.Input),
                    new OracleParameter("p_pct_suadoi_type", OracleDbType.Varchar2, pInfo.PCT_Suadoi_Type, ParameterDirection.Input),
                    new OracleParameter("p_pct_suadoi_content", OracleDbType.Varchar2, pInfo.PCT_Suadoi_Content, ParameterDirection.Input),
                    new OracleParameter("p_source_dqsc", OracleDbType.Varchar2, pInfo.Source_DQSC, ParameterDirection.Input),
                    new OracleParameter("p_dqsc_origin_app_no", OracleDbType.Varchar2, pInfo.DQSC_Origin_App_No, ParameterDirection.Input),
                    new OracleParameter("p_dqsc_filling_date", OracleDbType.Date, pInfo.DQSC_Filling_Date, ParameterDirection.Input),
                    new OracleParameter("p_dqsc_valid_before", OracleDbType.Decimal, pInfo.DQSC_Valid_Before, ParameterDirection.Input),
                    new OracleParameter("p_dqsc_valid_after", OracleDbType.Decimal, pInfo.DQSC_Valid_After, ParameterDirection.Input),
                    new OracleParameter("p_source_gphi", OracleDbType.Varchar2, pInfo.Source_GPHI, ParameterDirection.Input),
                    new OracleParameter("p_gphi_origin_app_no", OracleDbType.Varchar2, pInfo.GPHI_Origin_App_No, ParameterDirection.Input),
                    new OracleParameter("p_gphi_filling_date", OracleDbType.Date, pInfo.GPHI_Filling_Date, ParameterDirection.Input),
                    new OracleParameter("p_gphi_valid_before", OracleDbType.Decimal, pInfo.GPHI_Valid_Before, ParameterDirection.Input),
                    new OracleParameter("p_gphi_valid_after", OracleDbType.Decimal, pInfo.GPHI_Valid_After, ParameterDirection.Input),

                    new OracleParameter("p_Point", OracleDbType.Decimal, pInfo.Point, ParameterDirection.Input),
                    new OracleParameter("p_ThamDinhNoiDung", OracleDbType.Varchar2, pInfo.ThamDinhNoiDung, ParameterDirection.Input),
                    new OracleParameter("p_ChuyenDoiDon", OracleDbType.Varchar2, pInfo.ChuyenDoiDon, ParameterDirection.Input),

                    new OracleParameter("p_class_type", OracleDbType.Varchar2, pInfo.Class_Type, ParameterDirection.Input),
                    new OracleParameter("p_class_content", OracleDbType.Varchar2, pInfo.Class_Content, ParameterDirection.Input),
                    new OracleParameter("p_Used_Special", OracleDbType.Decimal, pInfo.Used_Special, ParameterDirection.Input),
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

        public int Deleted(decimal p_app_header_id, string pAppCode, string pLanguage)
        {
            try
            {
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_A03.proc_app_detail_A03_delete",
                    new OracleParameter("p_app_header_id", OracleDbType.Decimal, p_app_header_id, ParameterDirection.Input),
                    new OracleParameter("p_appcode", OracleDbType.Varchar2, pAppCode, ParameterDirection.Input),
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

        public DataSet GetByID(decimal p_app_header_id, string p_language_code)
        {
            try
            {
                DataSet _ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_A03.Proc_GetBy_ID",
                    new OracleParameter("p_app_header_id", OracleDbType.Decimal, p_app_header_id, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, p_language_code, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursorHeader", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursor_doc", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursor_fee", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursor_other_master", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursor_author", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursor_class", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursor_other_doc", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursor_uu_tien", OracleDbType.RefCursor, ParameterDirection.Output));
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

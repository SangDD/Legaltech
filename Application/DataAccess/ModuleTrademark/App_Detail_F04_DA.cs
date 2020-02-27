using Common;
using ObjectInfos;
using ObjectInfos.ModuleTrademark;
using Oracle.DataAccess.Client;
using System;
using System.Data;
namespace DataAccess
{
    public class App_Detail_F04_DA
    {

        public decimal Insert(App_Detail_F04_Info pInfo)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_detail_F04.Proc_App_Detail_F04_Insert",
                    new OracleParameter("p_app_header_id", OracleDbType.Decimal, pInfo.App_Header_Id, ParameterDirection.Input),
                    new OracleParameter("p_appcode", OracleDbType.Varchar2, pInfo.Appcode, ParameterDirection.Input),
                    new OracleParameter("p_case_code", OracleDbType.Varchar2, pInfo.Case_Code, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, pInfo.Language_Code, ParameterDirection.Input),
                    new OracleParameter("p_applicant_type", OracleDbType.Decimal, pInfo.Applicant_Type, ParameterDirection.Input),
                    new OracleParameter("p_business_line", OracleDbType.Varchar2, pInfo.Business_Line, ParameterDirection.Input),
                    new OracleParameter("p_description", OracleDbType.Varchar2, pInfo.Description, ParameterDirection.Input),
                    new OracleParameter("p_codelogo", OracleDbType.Varchar2, pInfo.Codelogo, ParameterDirection.Input),
                    new OracleParameter("p_loainhanhieu", OracleDbType.Varchar2, pInfo.Loainhanhieu, ParameterDirection.Input),
                    new OracleParameter("p_sodon_ut", OracleDbType.Varchar2, pInfo.Sodon_ut, ParameterDirection.Input),
                    new OracleParameter("p_ngaynopdon_ut", OracleDbType.Date, pInfo.Ngaynopdon_ut, ParameterDirection.Input),
                    new OracleParameter("p_nuocnopdon_ut", OracleDbType.Decimal, pInfo.Nuocnopdon_ut, ParameterDirection.Input),
                    new OracleParameter("p_color", OracleDbType.Varchar2, pInfo.Color, ParameterDirection.Input),
                    new OracleParameter("p_translation_of_word", OracleDbType.Varchar2, pInfo.Translation_Of_Word, ParameterDirection.Input),
                    new OracleParameter("p_logourl", OracleDbType.Varchar2, pInfo.Logourl, ParameterDirection.Input),

                    new OracleParameter("p_classno", OracleDbType.Varchar2, pInfo.ClassNo, ParameterDirection.Input),
                    new OracleParameter("p_duadate", OracleDbType.Date, pInfo.Duadate, ParameterDirection.Input),
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


        public int UpDate(App_Detail_F04_Info pInfo)
        {
            try
            {
                var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_detail_F04.proc_app_detail_F04_update",
                    new OracleParameter("p_id", OracleDbType.Decimal, pInfo.Id, ParameterDirection.Input),
                    new OracleParameter("p_app_header_id", OracleDbType.Decimal, pInfo.App_Header_Id, ParameterDirection.Input),
                    new OracleParameter("p_appno", OracleDbType.Varchar2, pInfo.Appno, ParameterDirection.Input),
                     new OracleParameter("p_case_code", OracleDbType.Varchar2, pInfo.Case_Code, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, pInfo.Language_Code, ParameterDirection.Input),
                    new OracleParameter("p_applicant_type", OracleDbType.Decimal, pInfo.Applicant_Type, ParameterDirection.Input),
                    new OracleParameter("p_business_line", OracleDbType.Varchar2, pInfo.Business_Line, ParameterDirection.Input),
                    new OracleParameter("p_description", OracleDbType.Varchar2, pInfo.Description, ParameterDirection.Input),
                    new OracleParameter("p_codelogo", OracleDbType.Varchar2, pInfo.Codelogo, ParameterDirection.Input),
                    new OracleParameter("p_loainhanhieu", OracleDbType.Varchar2, pInfo.Loainhanhieu, ParameterDirection.Input),
                    new OracleParameter("p_sodon_ut", OracleDbType.Varchar2, pInfo.Sodon_ut, ParameterDirection.Input),
                    new OracleParameter("p_ngaynopdon_ut", OracleDbType.Date, pInfo.Ngaynopdon_ut, ParameterDirection.Input),
                    new OracleParameter("p_nuocnopdon_ut", OracleDbType.Decimal, pInfo.Nuocnopdon_ut, ParameterDirection.Input),
                    new OracleParameter("p_color", OracleDbType.Varchar2, pInfo.Color, ParameterDirection.Input),
                    new OracleParameter("p_translation_of_word", OracleDbType.Varchar2, pInfo.Translation_Of_Word, ParameterDirection.Input),
                    new OracleParameter("p_logourl", OracleDbType.Varchar2, pInfo.Logourl, ParameterDirection.Input),

                    new OracleParameter("p_classno", OracleDbType.Varchar2, pInfo.ClassNo, ParameterDirection.Input),
                    new OracleParameter("p_duadate", OracleDbType.Date, pInfo.Duadate, ParameterDirection.Input),
                   
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

        public int Deleted(decimal p_app_header_id, string pAppNo, string pLanguage)
        {
            try
            {
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_detail_F04.Proc_f04_Delete",
                    new OracleParameter("p_app_header_id", OracleDbType.Decimal, p_app_header_id, ParameterDirection.Input),
                    new OracleParameter("p_appno", OracleDbType.Varchar2, pAppNo, ParameterDirection.Input),
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
                DataSet _ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_detail_F04.Proc_GetBy_ID",
                    new OracleParameter("p_app_header_id", OracleDbType.Decimal, p_app_header_id, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, p_language_code, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursorHeader", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursor_doc", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursor_other_doc", OracleDbType.RefCursor, ParameterDirection.Output));
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

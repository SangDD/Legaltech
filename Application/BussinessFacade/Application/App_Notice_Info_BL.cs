using Common;
using Common.SearchingAndFiltering;
using DataAccess;
using ObjectInfos;
using System;
using System.Collections.Generic;
using System.Data;
using ZetaCompressionLibrary;


namespace BussinessFacade
{
    public class App_Notice_Info_BL
    {
        public App_Notice_Info App_Notice_GetBy_CaseCode(string p_case_code, decimal p_notice_type = -1)
        {
            try
            {
                App_Notice_Info_DA _da = new App_Notice_Info_DA();
                DataSet _ds = _da.App_Notice_GetBy_CaseCode(p_case_code, p_notice_type);
                return CBO<App_Notice_Info>.FillObjectFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new App_Notice_Info();
            }
        }

        public int Get_Number_Notice(string p_case_code, decimal p_notice_type)
        {
            try
            {
                App_Notice_Info_DA _da = new App_Notice_Info_DA();
                DataSet _ds = _da.Get_Number_Notice(p_case_code, p_notice_type);
                if (_ds != null && _ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToInt16(_ds.Tables[0].Rows[0][0]);
                }
                return 0;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return 0;
            }
        }


        public List<App_Notice_Info> App_Notice_Search(string p_key_search, ref decimal p_total_record,
            string p_from = "1", string p_to = "10", string p_column = "ALL", string p_sort_type = "ALL")
        {
            try
            {
                App_Notice_Info_DA _da = new App_Notice_Info_DA();
                DataSet _ds = _da.App_Notice_Search(p_key_search, p_from, p_to, p_sort_type, ref p_total_record);
                return CBO<App_Notice_Info>.FillCollectionFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<App_Notice_Info>();
            }
        }

        public decimal App_Notice_Insert(App_Notice_Info p_obj, string p_language_code)
        {
            try
            {
                App_Notice_Info_DA _da = new App_Notice_Info_DA();
                return _da.App_Notice_Insert(p_obj.Case_Code, p_obj.Notice_Number, p_obj.Notice_Date, p_obj.Notice_Type,
                    p_obj.Notice_Url, p_obj.Notice_Trans_Url, p_obj.Result, p_obj.Accept_Date, p_obj.Accept_Url, p_obj.Reject_Reason, p_obj.Status, p_obj.Advise_Replies,
                    p_obj.Billing_Url, p_obj.Created_By, p_obj.Note);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal App_Notice_Review_Accept(string p_Case_Code, decimal p_Notice_Type, decimal p_status,
            string p_notes, string p_modify_by, string p_language_code)
        {
            try
            {
                App_Notice_Info_DA _da = new App_Notice_Info_DA();
                return _da.App_Notice_Review_Accept(p_Case_Code, p_Notice_Type, p_status, p_notes, p_modify_by, p_language_code);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal App_Notice_Review_Reject(string p_Case_Code, decimal p_Notice_Type, decimal p_status, string Advise_Replies, string Advise_Replies_Trans,
           string p_notes, string p_modify_by, string p_language_code)
        {
            try
            {
                App_Notice_Info_DA _da = new App_Notice_Info_DA();
                return _da.App_Notice_Review_Reject(p_Case_Code, p_Notice_Type, p_status, Advise_Replies, Advise_Replies_Trans, p_notes, p_modify_by, p_language_code);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal App_Notice_Update_Status(string p_case_code, decimal p_notice_type, decimal p_status, decimal p_result,
            DateTime p_accept_date, string p_note, string p_modify_by, string p_language_code)
        {
            try
            {
                App_Notice_Info_DA _da = new App_Notice_Info_DA();
                return _da.App_Notice_Update_Status(p_case_code, p_notice_type, p_status, p_result, p_accept_date, p_note, p_modify_by, p_language_code);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

    }
}

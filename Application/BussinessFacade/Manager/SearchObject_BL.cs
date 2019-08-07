using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Common;
using ObjectInfos;
using System.Data;
using Common.SearchingAndFiltering;

namespace BussinessFacade
{
    public class SearchObject_BL : RepositoriesBL
    {
        public decimal SEARCH_HEADER_INSERT(SearchObject_Header_Info p_SearchObject_Header_Info)

        {
            try
            {
                SearchObject_DA _objDA = new SearchObject_DA();
                return _objDA.SEARCH_HEADER_INSERT(p_SearchObject_Header_Info);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal SEARCH_HEADER_UPDATE(SearchObject_Header_Info p_SearchObject_Header_Info)
        {
            try
            {
                SearchObject_DA _objDA = new SearchObject_DA();
                return _objDA.SEARCH_HEADER_UPDATE(p_SearchObject_Header_Info);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public SearchObject_Header_Info SEARCH_HEADER_GETBYID(decimal P_ID, ref List<SearchObject_Detail_Info> p_searchListdetail,
            ref SearchObject_Question_Info p_question_info, ref List<Search_Class_Info> search_Class_Infos)
        {
            try
            {
                SearchObject_DA _objDA = new SearchObject_DA();
                DataSet _ds = _objDA.SEARCH_HEADER_GETBYID(P_ID);

                p_searchListdetail = CBO<SearchObject_Detail_Info>.FillCollectionFromDataTable(_ds.Tables[1]);
                p_question_info = CBO<SearchObject_Question_Info>.FillObjectFromDataTable(_ds.Tables[2]);
                search_Class_Infos = CBO<Search_Class_Info>.FillCollectionFromDataTable(_ds.Tables[3]);
                return CBO<SearchObject_Header_Info>.FillObjectFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new SearchObject_Header_Info();
            }
        }

        public SearchObject_Header_Info SEARCH_HEADER_GETBY_CASECODE(string p_casecode, ref List<SearchObject_Detail_Info> p_searchListdetail, 
            ref SearchObject_Question_Info p_question_info, ref List<Search_Class_Info> search_Class_Infos)
        {
            try
            {
                SearchObject_DA _objDA = new SearchObject_DA();
                DataSet _ds = _objDA.SEARCH_HEADER_GETBY_CASECODE(p_casecode);
                p_searchListdetail = CBO<SearchObject_Detail_Info>.FillCollectionFromDataTable(_ds.Tables[1]);
                p_question_info = CBO<SearchObject_Question_Info>.FillObjectFromDataTable(_ds.Tables[2]);
                search_Class_Infos = CBO<Search_Class_Info>.FillCollectionFromDataTable(_ds.Tables[3]);
                return CBO<SearchObject_Header_Info>.FillObjectFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new SearchObject_Header_Info();
            }
        }

        public decimal SEARCH_HEADER_DELETE(decimal P_SEARCH_ID, string p_user_name, string p_language)
        {
            try
            {
                SearchObject_DA _objDA = new SearchObject_DA();
                return _objDA.SEARCH_HEADER_DELETE(P_SEARCH_ID, p_user_name, p_language);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public List<SearchObject_Header_Info> SEARCH_OBJECT_SEARCH(string P_KEY_SEARCH = "", string OPTIONS = "")
        {
            try
            {
                SearchObject_DA _da = new SearchObject_DA();
                var optionFilter = new OptionFilter(OPTIONS);
                decimal totalRecordFindResult = 0;
                var ds = _da.SEARCH_OBJECT_SEARCH(P_KEY_SEARCH, optionFilter, ref totalRecordFindResult);
                this.SetupPagingHtml(optionFilter, Convert.ToInt32(totalRecordFindResult), "pageListOfObjects", "divNumberRecordOnPageListObjects");
                return CBO<SearchObject_Header_Info>.FillCollectionFromDataSet(ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<SearchObject_Header_Info>();
            }
        }

        public decimal SEARCH_DETAIL_INSERT(List<SearchObject_Detail_Info> P_SearchDetails)
        {
            try
            {
                SearchObject_DA _objDA = new SearchObject_DA();
                return _objDA.SEARCH_DETAIL_INSERT(P_SearchDetails);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal SEARCH_DETAIL_DELETE(decimal P_SEARCH_ID)
        {
            try
            {
                SearchObject_DA _objDA = new SearchObject_DA();
                return _objDA.SEARCH_DETAIL_DELETE(P_SEARCH_ID);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal SEARCH_QUESTION_INSERT(SearchObject_Question_Info p_question_info)
        {
            try
            {

                SearchObject_DA _objDA = new SearchObject_DA();
                return _objDA.SEARCH_QUESTION_INSERT(p_question_info);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal SEARCH_QUESTION_UPDATE(SearchObject_Question_Info p_question_info)
        {
            try
            {

                SearchObject_DA _objDA = new SearchObject_DA();
                return _objDA.SEARCH_QUESTION_UPDATE(p_question_info);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal SEARCH_QUESTION_DELETE(decimal P_SEARCH_ID)
        {
            try
            {
                SearchObject_DA _objDA = new SearchObject_DA();
                return _objDA.SEARCH_QUESTION_DELETE(P_SEARCH_ID);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal Update_Lawer(string p_case_code, decimal p_lawer_id, string p_note,string p_language_code, string p_user_name)
        {
            try
            {
                SearchObject_DA _objDA = new SearchObject_DA();
                return _objDA.Update_Lawer(p_case_code, p_lawer_id, p_note, p_language_code, p_user_name);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal Admin_Update(string p_case_code, decimal p_status, string p_notes, string p_language_code, string p_modified_by)
        {
            try
            {
                SearchObject_DA _objDA = new SearchObject_DA();
                return _objDA.Admin_Update(p_case_code, p_status, p_notes, p_language_code, p_modified_by);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal Customer_Review(string p_case_code, decimal p_status, string p_notes, string p_language_code, string p_modified_by)
        {
            try
            {
                SearchObject_DA _objDA = new SearchObject_DA();
                return _objDA.Customer_Review(p_case_code, p_status, p_notes, p_language_code, p_modified_by);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        

        public decimal SEARCH_RESULT_SEARCH(SearchObject_Question_Info p_obj)
        {
            try
            {
                SearchObject_DA _objDA = new SearchObject_DA();
                return _objDA.SEARCH_RESULT_SEARCH(p_obj.CASE_CODE, p_obj.LAWER_ID, p_obj.NOTE, p_obj.CONTENT, p_obj.LANGUAGE_CODE, p_obj.MODIFIED_BY, p_obj.FILE_URL, p_obj.FILE_URL02);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public int Search_Class_InsertBatch(List<Search_Class_Info> pInfo, decimal p_search_header, string pLanguage)
        {
            try
            {
                SearchObject_DA _objDA = new SearchObject_DA();
                return _objDA.SearchClass_InsertBatch(pInfo, p_search_header, pLanguage);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public int Search_Class_Delete(decimal p_search_header, string pLanguage)
        {
            try
            {
                SearchObject_DA _objDA = new SearchObject_DA();
                return _objDA.SearchClass_Delete(p_search_header, pLanguage);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public int Search_Fee_InsertBatch(List<Search_Fix_Info> pInfo, decimal p_search_header, string pLanguage)
        {
            try
            {
                SearchObject_DA _objDA = new SearchObject_DA();
                return _objDA.Search_Fee_InsertBatch(pInfo, p_search_header, pLanguage);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public int Search_Fee_Delete(decimal p_search_header, string pLanguage)
        {
            try
            {
                SearchObject_DA _objDA = new SearchObject_DA();
                return _objDA.Search_Fee_Delete(p_search_header, pLanguage);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }


        /////
        public SearchObject_Header_Info GetBilling_By_Case_Code(string p_case_code, string p_user_name, string p_language_code, 
            ref List<Billing_Detail_Info> p_lst_billing_detail)
        {
            try
            {
                SearchObject_DA _da = new SearchObject_DA();
                DataSet _ds = _da.GetBilling_By_Case_Code(p_case_code, p_user_name, p_language_code);
                if (_ds != null && _ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0)
                {
                    p_lst_billing_detail = CBO<Billing_Detail_Info>.FillCollectionFromDataTable(_ds.Tables[1]);
                    return CBO<SearchObject_Header_Info>.FillObjectFromDataTable(_ds.Tables[0]);
                }
                else return null;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }

        }

        public int Update_Url_Billing(string p_case_code,decimal p_billing_id, string p_url_billing)
        {
            try
            {
                SearchObject_DA objData = new SearchObject_DA();

                return objData.Update_Url_Billing(p_case_code, p_billing_id, p_url_billing);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }
    }
}

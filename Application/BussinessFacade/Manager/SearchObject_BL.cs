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
   public class SearchObject_BL: RepositoriesBL
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

        public SearchObject_Header_Info SEARCH_HEADER_GETBYID(decimal P_ID, ref List<SearchObject_Detail_Info> p_searchListdetail, ref SearchObject_Question_Info p_question_info)
        {
            try
            {
                SearchObject_DA _objDA = new SearchObject_DA();
                DataSet _ds = _objDA.SEARCH_HEADER_GETBYID(P_ID);

                p_searchListdetail = CBO<SearchObject_Detail_Info>.FillCollectionFromDataTable(_ds.Tables[1]);
                p_question_info = CBO<SearchObject_Question_Info>.FillObjectFromDataTable(_ds.Tables[2]);
                return CBO<SearchObject_Header_Info>.FillObjectFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new SearchObject_Header_Info();
            }
        }

        public SearchObject_Header_Info SEARCH_HEADER_GETBY_CASECODE(string p_casecode, ref List<SearchObject_Detail_Info> p_searchListdetail, ref SearchObject_Question_Info p_question_info)
        {
            try
            {
                SearchObject_DA _objDA = new SearchObject_DA();
                DataSet _ds = _objDA.SEARCH_HEADER_GETBY_CASECODE(p_casecode);
                p_searchListdetail = CBO<SearchObject_Detail_Info>.FillCollectionFromDataTable(_ds.Tables[1]);
                p_question_info = CBO<SearchObject_Question_Info>.FillObjectFromDataTable(_ds.Tables[2]);
                return CBO<SearchObject_Header_Info>.FillObjectFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new SearchObject_Header_Info();
            }
        }



        public decimal SEARCH_HEADER_DELETE(decimal P_SEARCH_ID)
        {
            try
            {
                 SearchObject_DA _objDA = new SearchObject_DA();
                   return _objDA.SEARCH_HEADER_DELETE(P_SEARCH_ID); 
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

       public decimal  SEARCH_LAWER_INSERT(App_Lawer_Info p_obj)
        {
            try
            {
                SearchObject_DA _objDA = new SearchObject_DA();
                return _objDA.SEARCH_LAWER_INSERT(p_obj.Case_Code, p_obj.Lawer_Id, p_obj.Notes, p_obj.Language_Code, p_obj.Created_By, p_obj.Created_Date);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

    }


}

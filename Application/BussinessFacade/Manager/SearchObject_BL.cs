using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Common;
using ObjectInfos;
using System.Data;

namespace BussinessFacade
{
   public class SearchObject_BL
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

        public SearchObject_Header_Info SEARCH_HEADER_GETBYID(decimal P_ID)
        {
            try
            {
                SearchObject_DA _objDA = new SearchObject_DA();
                DataSet _ds = _objDA.SEARCH_HEADER_GETBYID(P_ID);
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

        public List<SearchObject_Header_Info> SEARCH_OBJECT_SEARCH(string p_key_search, ref decimal p_total_record,
                string p_from = "1", string p_to = "10", string p_sort_type = "ALL")
        {
            try
            {
                SearchObject_DA _objDA = new SearchObject_DA();
                DataSet _ds = _objDA.SEARCH_OBJECT_SEARCH(p_key_search, p_from, p_to, p_sort_type, ref p_total_record);
                return CBO<SearchObject_Header_Info>.FillCollectionFromDataSet(_ds);
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

    }


}

using Common;
using DataAccess.ModuleArticles;
using ObjectInfos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessFacade
{
    public class NewsBL
    {
        public List<NewsInfo> ArticlesGetByPage(string pLanguage, string pTitle, DateTime pNgayCongBo, int pStart, int pEnd, ref decimal pTotalRecord)
        {
            try
            {
                NewsDA _da = new NewsDA();
                DataSet _ds = _da.ArticlesGetByPage(pLanguage,   pTitle,   pNgayCongBo,   pStart,   pEnd,ref pTotalRecord);
                return CBO<NewsInfo>.FillCollectionFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<NewsInfo>();
            }
        }

        public List<NewsInfo> ArticleHomeSearch(string p_key_search, ref decimal p_total_record,
               string p_from = "1", string p_to = "10", string p_sort_type = "ALL")
        {
            try
            {
                NewsDA _da = new NewsDA();
                DataSet _ds = _da.NewsHomeSearch(p_key_search, p_from, p_to, p_sort_type, ref p_total_record);
                return CBO<NewsInfo>.FillCollectionFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<NewsInfo>();
            }
        }

        public List<NewsInfo> NewsStatic(string language)
        {
            try
            {
                NewsDA _da = new NewsDA();
                DataSet _ds = _da.NewsStatic(language);
                return CBO<NewsInfo>.FillCollectionFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<NewsInfo>();
            }
        }


        public NewsInfo ArticlesGetById(decimal pID, string pLanguage)
        {
            try
            {
                NewsDA objDA = new NewsDA();
                DataSet ResultData = objDA.ArticlesGetById(pID, pLanguage);
                return CBO<NewsInfo>.FillObjectFromDataSet(ResultData);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new NewsInfo();
            }
        }

        public NewsInfo ArticlesGetByCaseCode(string p_Case_Code, string pLanguage)
        {
            try
            {
                NewsDA objDA = new NewsDA();
                DataSet ResultData = objDA.ArticlesGetByCaseCode(p_Case_Code, pLanguage);
                return CBO<NewsInfo>.FillObjectFromDataSet(ResultData);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new NewsInfo();
            }
        }

        public decimal ArticlesInsert(NewsInfo pInfo)
        {
            try
            {
                NewsDA objDA = new NewsDA();
                return objDA.ArticlesInsert(pInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public decimal ArticlesUpdate(NewsInfo pInfo)
        {
            try
            {
                NewsDA objDA = new NewsDA();
                return objDA.ArticlesUpdate(pInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public decimal ArticlesDeleted(NewsInfo pInfo)
        {
            try
            {
                NewsDA objDA = new NewsDA();
                return objDA.ArticlesDeleted(pInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

    }
}

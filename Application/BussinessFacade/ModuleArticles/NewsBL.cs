using Common;
using DataAccess.ModuleArticles;
using ObjectInfos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessFacade.ModuleArticles
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
    }
}

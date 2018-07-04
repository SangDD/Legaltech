using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using ObjectInfos;
using Common;

namespace BussinessFacade
{
  public  class WikiCatalogue_BL
    {
        public List<WikiCatalogues_Info> WikiCatalogueGetAll()
        {
            try
            {
                WikiCatalogue_DA _objDa = new WikiCatalogue_DA();
                var ds = _objDa.WikiCatalogue_GetAll();
                return CBO<WikiCatalogues_Info>.FillCollectionFromDataSet(ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<WikiCatalogues_Info>();
            }
        }
    }
}

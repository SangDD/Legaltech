using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using ObjectInfos;
using Common;
using Common.SearchingAndFiltering;

namespace BussinessFacade
{
  public  class WikiCatalogue_BL: RepositoriesBL
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

        public decimal WikiCatalogue_Insert(WikiCatalogues_Info p_ObjInffo)
        {
            try
            {
                WikiCatalogue_DA _da = new WikiCatalogue_DA();
                return _da.WikiCatalogue_Insert(p_ObjInffo.NAME, p_ObjInffo.NAME_ENG, p_ObjInffo.CATA_LEVEL, p_ObjInffo.CREATED_BY, p_ObjInffo.CREATED_DATE, p_ObjInffo.PARENT_ID);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal WikiCatalogue_Update(WikiCatalogues_Info p_ObjInffo)
        {
            try
            {
                WikiCatalogue_DA _da = new WikiCatalogue_DA();
                return _da.WikiCatalogue_Update(p_ObjInffo.ID, p_ObjInffo.NAME, p_ObjInffo.NAME_ENG, p_ObjInffo.CATA_LEVEL, p_ObjInffo.MODIFIED_BY, p_ObjInffo.MODIFIED_DATE, p_ObjInffo.PARENT_ID);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal WikiCatalogue_Delete(decimal p_id)
        {
            try
            {
                WikiCatalogue_DA _da = new WikiCatalogue_DA();
                return _da.WikiCatalogue_Delete(p_id);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public WikiCatalogues_Info WikiCatalogue_GetByID(decimal P_ID)
        {
            try
            {
                WikiCatalogue_DA _da = new WikiCatalogue_DA();
                var ds = _da.WikiCatalogue_GetByID(P_ID);
                return CBO<WikiCatalogues_Info>.FillObjectFromDataSet(ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new WikiCatalogues_Info();
            }
        }

        public List<WikiCatalogues_Info> WikiCata_Search(string P_KEY_SEARCH = "", string OPTIONS = "")
        {
            try
            {
                WikiCatalogue_DA _da = new WikiCatalogue_DA();
                var optionFilter = new OptionFilter(OPTIONS);
                var totalRecordFindResult = 0;
                var ds = _da.WikiCata_Search(P_KEY_SEARCH, optionFilter, ref totalRecordFindResult);
                this.SetupPagingHtml(optionFilter, totalRecordFindResult, "pageListOfObjects", "divNumberRecordOnPageListObjects");
                return CBO<WikiCatalogues_Info>.FillCollectionFromDataSet(ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<WikiCatalogues_Info>();
            }
        }

        public List<WikiCatalogues_Info> Portal_CataGetAll()
        {
            try
            {
                WikiCatalogue_DA _da = new WikiCatalogue_DA();
                var ds = _da.Portal_CataGetAll();
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

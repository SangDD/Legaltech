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
  public  class App_Class_BL: RepositoriesBL
    {
        public App_Class_Info AppClassGetByID(decimal P_ID)
        {
            try
            {
                App_Class_DA _da = new App_Class_DA();
                DataSet _ds = _da.AppClassGetByID(P_ID);
                return CBO<App_Class_Info>.FillObjectFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new App_Class_Info();
            }
        }


        public List<App_Class_Info>  AppClassGetAll()
        {
            try
            {
              
                App_Class_DA _da = new App_Class_DA();
                var optionFilter = new OptionFilter();
                optionFilter.EndAt = 0;
                optionFilter.StartAt = 0;
                var totalRecordFindResult = 0;
                var ds = _da.SearchAppClass("", optionFilter, ref totalRecordFindResult);
                return CBO<App_Class_Info>.FillCollectionFromDataSet(ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<App_Class_Info>();
            }
        }

        public List<App_Class_Info> SearchAppClass(string P_KEY_SEARCH = "",  string OPTIONS = "")
        {
            try
            {
                App_Class_DA _da = new App_Class_DA();
                var optionFilter = new OptionFilter(OPTIONS);
                var totalRecordFindResult = 0;
                var ds = _da.SearchAppClass(P_KEY_SEARCH, optionFilter, ref totalRecordFindResult);
                this.SetupPagingHtml(optionFilter, totalRecordFindResult, "pageListOfObjects", "divNumberRecordOnPageListObjects");
                return CBO<App_Class_Info>.FillCollectionFromDataSet(ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<App_Class_Info>();
            }
        }

        public decimal App_Class_Insert(App_Class_Info p_ObjInffo)
        {
            try
            {
                App_Class_DA _da = new App_Class_DA();
                return _da.App_Class_Insert(p_ObjInffo.Code, p_ObjInffo.Name_Vi, p_ObjInffo.Name_En, p_ObjInffo.Name_Cn, p_ObjInffo.Created_By, p_ObjInffo.Created_Date);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal App_Class_Update(App_Class_Info p_ObjInffo)
        {
            try
            {
                App_Class_DA _da = new App_Class_DA();
                return _da.App_Class_Update(p_ObjInffo.Id, p_ObjInffo.Code, p_ObjInffo.Name_Vi, p_ObjInffo.Name_En, p_ObjInffo.Name_Cn, p_ObjInffo.Modified_By, p_ObjInffo.Modified_Date);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal App_Class_Delete(decimal p_id, string p_username, DateTime p_date)
        {
            try
            {
                App_Class_DA _da = new App_Class_DA();
                return _da.App_Class_Delete(p_id, p_username, p_date);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }


    }
}

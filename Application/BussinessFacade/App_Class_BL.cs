using Common;
using DataAccess;
using ObjectInfos;
using System;
using System.Collections.Generic;
using System.Data;
using ZetaCompressionLibrary;


namespace BussinessFacade
{
  public  class App_Class_BL
    {
        public List<App_Class_Info> AppClassGetByID(decimal P_ID)
        {
            try
            {
                App_Class_DA _da = new App_Class_DA();
                DataSet _ds = _da.AppClassGetByID(P_ID);
                return CBO<App_Class_Info>.FillCollectionFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<App_Class_Info>();
            }
        }


        public List<App_Class_Info> SearchAppClass(string P_KEY_SEARCH, string P_FROM, string P_TO, string P_SORT_TYPE, ref decimal P_TOTAL_RECORD)
        {
            try
            {
                App_Class_DA _da = new App_Class_DA();
                DataSet _ds = _da.SearchAppClass(P_KEY_SEARCH, P_FROM, P_TO, P_SORT_TYPE, ref P_TOTAL_RECORD);
                return CBO<App_Class_Info>.FillCollectionFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<App_Class_Info>();
            }
        }

        public decimal App_Class_Insert(App_Class_Info p_ObjInffo, string p_username, DateTime p_date)
        {
            try
            {
                App_Class_DA _da = new App_Class_DA();
                return _da.App_Class_Insert(p_ObjInffo.Code, p_ObjInffo.Name_Vi, p_ObjInffo.Name_En, p_ObjInffo.Name_Cn, p_username, p_date);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal App_Class_Update(App_Class_Info p_ObjInffo, string p_username, DateTime p_date)
        {
            try
            {
                App_Class_DA _da = new App_Class_DA();
                return _da.App_Class_Update(p_ObjInffo.Id, p_ObjInffo.Code, p_ObjInffo.Name_Vi, p_ObjInffo.Name_En, p_ObjInffo.Name_Cn, p_username, p_date);
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

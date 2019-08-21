using Common;
using DataAccess;
using ObjectInfos;
using System;
using System.Collections.Generic;
using System.Data;
using ZetaCompressionLibrary;

namespace BussinessFacade
{
    public class App_Translate_BL
    {
        public List<App_Translate_Info> App_Translate_GetBy_AppId(decimal p_app_header_id)
        {
            try
            {
                App_Translate_DA _da = new App_Translate_DA();
                DataSet _ds = _da.App_Translate_GetBy_AppId(p_app_header_id);
                return CBO<App_Translate_Info>.FillCollectionFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<App_Translate_Info>();
            }
        }

        public List<App_Translate_Info> App_Translate_GetBy_Casecode(string p_case_code)
        {
            try
            {
                App_Translate_DA _da = new App_Translate_DA();
                DataSet _ds = _da.App_Translate_GetBy_Casecode(p_case_code);
                return CBO<App_Translate_Info>.FillCollectionFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<App_Translate_Info>();
            }
        }

        public List<Sys_App_Translate_Info> Sys_App_Translate_GetBy_Casecode(string p_appcode)
        {
            try
            {
                App_Translate_DA _da = new App_Translate_DA();
                DataSet _ds = _da.Sys_App_Translate_GetBy_Casecode(p_appcode);
                return CBO<Sys_App_Translate_Info>.FillCollectionFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<Sys_App_Translate_Info>();
            }
        }

        public DataSet AppDetail_GetBy_Id(string p_appcode, decimal p_app_header_id)
        {
            try
            {
                App_Translate_DA _da = new App_Translate_DA();
                return _da.AppDetail_GetBy_Id(p_appcode, p_app_header_id);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public decimal App_Translate_Insert(List<App_Translate_Info> pInfo)
        {
            try
            {
                App_Translate_DA _da = new App_Translate_DA();
                return _da.App_Translate_Insert(pInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal App_Translate_Delete_ByAppId(decimal p_app_header_id)
        {
            try
            {
                App_Translate_DA _da = new App_Translate_DA();
                return _da.App_Translate_Delete_ByAppId(p_app_header_id);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal App_Translate_Delete_ByCaseCode(string p_case_code)
        {
            try
            {
                App_Translate_DA _da = new App_Translate_DA();
                return _da.App_Translate_Delete_ByCaseCode(p_case_code);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

    }
}

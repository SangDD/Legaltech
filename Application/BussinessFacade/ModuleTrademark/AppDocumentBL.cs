using Common;
using DataAccess.ModuleTrademark;
using System;
using System.Collections.Generic;
using ObjectInfos.ModuleTrademark;
using ObjectInfos;
using System.Data;
using DataAccess;

namespace BussinessFacade.ModuleTrademark
{
    public class AppDocumentBL
    {
        public List<Sys_Document_Info> Sys_Document_GetBy_Casecode(string p_appcode)
        {
            try
            {
                App_Translate_DA _da = new App_Translate_DA();
                DataSet _ds = _da.Sys_Document_GetBy_Casecode(p_appcode);
                return CBO<Sys_Document_Info>.FillCollectionFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<Sys_Document_Info>();
            }
        }

        public List<Sys_Document_Info> Sys_Document_GetAll()
        {
            try
            {
                App_Translate_DA _da = new App_Translate_DA();
                DataSet _ds = _da.Sys_Document_GetAll();
                return CBO<Sys_Document_Info>.FillCollectionFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<Sys_Document_Info>();
            }
        }

        public int AppDocumentInsertBath(List<AppDocumentInfo> pInfo, decimal pAppHeaderid)
        {
            try
            {
                AppDocumentDA objData = new AppDocumentDA();
                return objData.AppDocumentInsertBatch(pInfo, pAppHeaderid);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public int AppDocumentDelByID(decimal pID, string pLanguage, decimal pAppHeaderID)
        {
            try
            {
                AppDocumentDA objData = new AppDocumentDA();
                return objData.AppDocumentDeletedByID(pID, pLanguage, pAppHeaderID);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public int AppDocumentDelByApp(decimal pAppHeaderID, string pLanguage)
        {
            try
            {
                AppDocumentDA objData = new AppDocumentDA();
                return objData.AppDocumentDeletedByApp(pAppHeaderID, pLanguage);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public int AppDocumentTranslate(string pLanguage_new, decimal p_app_old_id, decimal p_app_new_id)
        {
            try
            {
                AppDocumentDA objData = new AppDocumentDA();
                return objData.AppDocumentTranslate(pLanguage_new, p_app_old_id, p_app_new_id);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public List<AppDocumentInfo> AppDocument_Getby_AppHeader(decimal p_app_header_id, string p_language_code)
        {
            try
            {
                AppDocumentDA objData = new AppDocumentDA();
                DataSet ds = objData.AppDocument_Getby_AppHeader(p_app_header_id, p_language_code);
                return CBO<AppDocumentInfo>.FillCollectionFromDataSet(ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<AppDocumentInfo>();
            }
        }

        // document others

        public int AppDocumentOtherInsertBatch(List<AppDocumentOthersInfo> pInfo)
        {
            try
            {
                AppDocumentDA objData = new AppDocumentDA();
                return objData.AppDocumentOtherInsertBatch(pInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public int AppDocumentOtherDeletedByApp(decimal pAppHeaderID, string pLanguage)
        {
            try
            {
                AppDocumentDA objData = new AppDocumentDA();
                return objData.AppDocumentOtherDeletedByApp(pAppHeaderID, pLanguage);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public int AppDocumentOtherDeletedByApp_Type(decimal pAppHeaderID, string pLanguage, decimal p_fileType)
        {
            try
            {
                AppDocumentDA objData = new AppDocumentDA();
                return objData.AppDocumentOtherDeletedByApp_Type(pAppHeaderID, pLanguage, p_fileType);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }


        public int AppDocOther_Del_ByID(decimal pID, string pLanguage)
        {
            try
            {
                AppDocumentDA objData = new AppDocumentDA();
                return objData.AppDocOther_Del_ByID(pID, pLanguage);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public List<AppDocumentOthersInfo> DocumentOthers_GetByAppHeader(decimal p_app_header_id, string p_language_code)
        {
            try
            {
                AppDocumentDA objData = new AppDocumentDA();
                DataSet ds = objData.DocumentOthers_GetByAppHeader(p_app_header_id, p_language_code);
                return CBO<AppDocumentOthersInfo>.FillCollectionFromDataSet(ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<AppDocumentOthersInfo>();
            }
        }
    }
}

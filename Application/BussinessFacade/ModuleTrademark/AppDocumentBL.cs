using Common;
using DataAccess.ModuleTrademark;
using System;
using System.Collections.Generic;
using ObjectInfos.ModuleTrademark;
using ObjectInfos;
using System.Data;

namespace BussinessFacade.ModuleTrademark
{
    public class AppDocumentBL
    {
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

        public int AppDocOtherByID(decimal pID, string pLanguage)
        {
            try
            {
                AppDocumentDA objData = new AppDocumentDA();
                return objData.AppDocOtherByID(pID, pLanguage);
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

    }
}

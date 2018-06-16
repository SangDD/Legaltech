using Common;
using DataAccess.ModuleTrademark;
using System;
using System.Collections.Generic;
using ObjectInfos.ModuleTrademark;
using ObjectInfos;

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

        public int AppDocumentDelByID(decimal pID, string pLanguage, decimal pAppHeaderID, decimal pDocumentID)
        {
            try
            {
                AppDocumentDA objData = new AppDocumentDA();
                return objData.AppDocumentDeletedByID(pID, pLanguage, pAppHeaderID, pDocumentID);
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

    }
}

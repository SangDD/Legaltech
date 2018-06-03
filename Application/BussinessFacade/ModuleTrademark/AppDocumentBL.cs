using Common;
using DataAccess.ModuleTrademark;
using System;
using System.Collections.Generic;
using ObjectInfos.ModuleTrademark;

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
                return objData.AppHeaderDeletedByApp(pAppHeaderID, pLanguage);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

    }
}

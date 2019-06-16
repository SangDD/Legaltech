using Common;
using DataAccess.ModuleTrademark;
using System;
using System.Collections.Generic;
using ObjectInfos.ModuleTrademark;
using ObjectInfos;
using System.Data;

namespace BussinessFacade.ModuleTrademark
{
    public class AppImageBL
    {
        public int AppImageInsertBatch(List<AppDocumentOthersInfo> pInfo)
        {
            try
            {
                AppImageDA objData = new AppImageDA();
                return objData.AppImageInsertBatch(pInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public int AppImageDeletedByApp(decimal pAppHeaderID, string pLanguage)
        {
            try
            {
                AppImageDA objData = new AppImageDA();
                return objData.AppImageDeletedByApp(pAppHeaderID, pLanguage);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public int AppImageDelByID(decimal pID, string pLanguage)
        {
            try
            {
                AppImageDA objData = new AppImageDA();
                return objData.AppImageDelByID(pID, pLanguage);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }
    }
}

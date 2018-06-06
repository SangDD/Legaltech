using Common;
using DataAccess.ModuleTrademark;
using ObjectInfos;
using System;
using System.Collections.Generic;

namespace BussinessFacade.ModuleTrademark
{
    public class AppClassDetailBL
    {
        public int AppClassDetailInsertBatch(List<AppClassDetailInfo> pInfo, decimal pAppHeaderid,string pLanguage)
        {
            try
            {
                var objData = new AppClassDetail_DA();
                return objData.AppClassDetailInsertBatch(pInfo, pAppHeaderid, pLanguage);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }


        public int AppClassDetailDeleted(decimal pAppHeaderID,string pLanguage)
        {
            try
            {
                var objData = new AppClassDetail_DA();
                return objData.AppClassDetailDeleted(pAppHeaderID, pLanguage);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }
    }
}

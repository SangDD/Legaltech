using Common;
using DataAccess.ModuleTrademark;
using ObjectInfos;
using System;
using System.Collections.Generic;

namespace BussinessFacade.ModuleTrademark
{
    public class AppClassDetailBL
    {
        public int AppClassDetailInsertBatch(List<AppClassDetailInfo> pInfo, decimal pAppHeaderid)
        {
            try
            {
                var objData = new AppClassDetail_DA();
                return objData.AppClassDetailInsertBatch(pInfo, pAppHeaderid);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }


        public int AppClassDetailDeleted(decimal pAppHeaderID)
        {
            try
            {
                var objData = new AppClassDetail_DA();
                return objData.AppClassDetailDeleted(pAppHeaderID);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }
    }
}

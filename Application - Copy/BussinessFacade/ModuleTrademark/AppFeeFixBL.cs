using Common;
using DataAccess.ModuleTrademark;
using ObjectInfos.ModuleTrademark;
using System;
using System.Collections.Generic;

namespace BussinessFacade.ModuleTrademark
{
    public class AppFeeFixBL
    {
        public int AppFeeFixInsertBath(List<AppFeeFixInfo> pInfo, decimal pAppHeaderid)
        {
            try
            {
                AppFeeFixDA objData = new AppFeeFixDA();
                return objData.AppFeeFixInsertBatch(pInfo, pAppHeaderid);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public int AppFeeFixlUpdate(AppFeeFixInfo pInfo)
        {
            try
            {
                AppFeeFixDA objData = new AppFeeFixDA();
                return objData.AppFeeFixUpdate(pInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

    }
}

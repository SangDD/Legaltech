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

        public int AppFeeFixDelete(decimal p_app_header_id, string p_language_code)
        {
            try
            {
                AppFeeFixDA objData = new AppFeeFixDA();
                return objData.AppFeeFixDelete(p_app_header_id, p_language_code);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }
    }
}

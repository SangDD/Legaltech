using Common;
using DataAccess.ModuleTrademark;
using ObjectInfos.ModuleTrademark;
using System;

namespace BussinessFacade.ModuleTrademark
{
    public class AppDetail01BL
    {

        public int AppDetailInsert(AppDetail01Info pInfo)
        {
            try
            {
                AppDetail01_DA objData = new AppDetail01_DA();
                return objData.AppDetailInsert(pInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public int AppDetailUpdate(AppDetail01Info pInfo)
        {
            try
            {
                AppDetail01_DA objData = new AppDetail01_DA();
                return objData.AppDetailUpDate(pInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }


        public int AppDetailDeleted(decimal pID, string pLanguage)
        {
            try
            {
                AppDetail01_DA objData = new AppDetail01_DA();
                return objData.AppDetailDeleted(pID, pLanguage);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }
    }
}

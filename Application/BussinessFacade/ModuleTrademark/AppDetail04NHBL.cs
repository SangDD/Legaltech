using Common;
using DataAccess.ModuleTrademark;
using ObjectInfos;
using System;

namespace BussinessFacade.ModuleTrademark
{
    public class AppDetail04NHBL
    {

        public int App_Detail_04NH_Insert(AppDetail04NHInfo pInfo)
        {
            try
            {
                AppDetail04NH_DA objData = new AppDetail04NH_DA();
                return objData.App_Detail_04NH_Insert(pInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public int App_Detail_04NH_Update(AppDetail04NHInfo pInfo)
        {
            try
            {
                AppDetail04NH_DA objData = new AppDetail04NH_DA();
                return objData.App_Detail_04NH_Update(pInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public int App_Detail_04NH_Deleted(decimal pAppHeaderID, string pAppCode, string pLanguage)
        {
            try
            {
                AppDetail04NH_DA objData = new AppDetail04NH_DA();
                return objData.App_Detail_04NH_Deleted(pAppHeaderID, pAppCode, pLanguage);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }
    }
}

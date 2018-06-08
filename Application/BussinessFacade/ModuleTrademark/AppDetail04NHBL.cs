using Common;
using DataAccess.ModuleTrademark;
using ObjectInfos;
using System;
using System.Data;

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

        public DataSet AppTM04NHGetByID(decimal pAppHeaderId, string pLanguage, int pStatus)
        {
            try
            {
                var objData = new AppDetail04NH_DA();
                return objData.AppTM04NHGetByID(pAppHeaderId, pLanguage, pStatus);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
                     
            }
        }
    }
}

using Common;
using DataAccess.ModuleTrademark;
using ObjectInfos;
using ObjectInfos.ModuleTrademark;
using System;
using System.Collections.Generic;
using System.Data;

namespace BussinessFacade.ModuleTrademark
{
    public class SysApplicationBL
    {
        private static readonly object objlockSynchronize = new object();
        private static List<SysApplicationInfo> lstMemSysApplication = new List<SysApplicationInfo>();


        public static void SysApplicationAllOnMem()
        {
            lock (objlockSynchronize)
            {
                var ds = SysApplicationDA.SysApplicationGetAll();
                lstMemSysApplication = CBO<SysApplicationInfo>.FillCollectionFromDataSet(ds);
            }
        }


        public static List<SysApplicationInfo> GetSysAppByLanguage(string pLanguage)
        {
            try
            {
                List<SysApplicationInfo> lstDataByLanguage = new List<SysApplicationInfo>();
                lock (objlockSynchronize)
                {
                    foreach (SysApplicationInfo item in lstMemSysApplication)
                    {
                        if (item.Languagecode == pLanguage)
                        {
                            lstDataByLanguage.Add(item);
                        }
                    }
                }
                return lstDataByLanguage;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<SysApplicationInfo>();
            }
        }

        public List<SysAppFixChargeInfo> Sys_App_Fix_Charge_GetAll()
        {
            try
            {
                SysApplicationDA _da = new SysApplicationDA();
                DataSet ds = _da.Sys_App_Fix_Charge_GetAll();
                return CBO<SysAppFixChargeInfo>.FillCollectionFromDataSet(ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<SysAppFixChargeInfo>();
            }
        }
        public SysAppFixChargeInfo SysAppFixChargeById(decimal pID, string pAppCode)
        {
            try
            {
                var _da = new SysApplicationDA();
                DataSet ds = _da.SysAppFeeFixGetById(pID, pAppCode);
                return CBO<SysAppFixChargeInfo>.FillObjectFromDataSet(ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new SysAppFixChargeInfo();
            }
        }

        public decimal SysAppFixChargeUpdate(SysAppFixChargeInfo pInfo)
        {
            try
            {
                var _da = new SysApplicationDA();
                var preturn = _da.SysAppFeeFixUpdate(pInfo.Id, pInfo.Appcode, pInfo.Amount, pInfo.Amount_Usd, pInfo.Amount_Represent, pInfo.Amount_Represent_Usd, pInfo.Char01, pInfo.Description);
                return preturn;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -3;
            }
        }
    }
}

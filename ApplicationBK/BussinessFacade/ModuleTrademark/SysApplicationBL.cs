﻿using Common;
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
        public SysAppFixChargeInfo  SysAppFixChargeById(decimal pID, string pAppCode)
        {
            try
            {
                var _da = new SysApplicationDA();
                DataSet ds = _da.SysAppFeeFixGetById(pID,pAppCode);
               return CBO<SysAppFixChargeInfo>.FillObjectFromDataSet(ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new SysAppFixChargeInfo();
            }
        }

        public decimal SysAppFixChargeUpdate(decimal pID, string pAppCode, decimal pAmount, string pChar01, string pDescription)
        {
            try
            {
                var _da = new SysApplicationDA();
                var preturn = _da.SysAppFeeFixUpdate(pID, pAppCode, pAmount, pChar01, pDescription);
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
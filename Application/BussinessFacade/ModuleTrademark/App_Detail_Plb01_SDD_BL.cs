using Common;
using DataAccess;
using DataAccess.ModuleTrademark;
using ObjectInfos;
using ObjectInfos.ModuleTrademark;
using System;
using System.Collections.Generic;
using System.Data;

namespace BussinessFacade.ModuleTrademark
{
    public class App_Detail_PLB01_SDD_BL
    {

        public decimal Insert(App_Detail_PLB01_SDD_Info pInfo)
        {
            try
            {
                App_Detail_PLB01_SDD_DA objData = new App_Detail_PLB01_SDD_DA();
                return objData.Insert(pInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public int Update(App_Detail_PLB01_SDD_Info pInfo)
        {
            try
            {
                App_Detail_PLB01_SDD_DA objData = new App_Detail_PLB01_SDD_DA();
                return objData.UpDate(pInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public int Deleted(decimal pAppHeaderID, string pAppCode, string pLanguage)
        {
            try
            {
                App_Detail_PLB01_SDD_DA objData = new App_Detail_PLB01_SDD_DA();
                return objData.Deleted(pAppHeaderID, pAppCode, pLanguage);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public App_Detail_PLB01_SDD_Info GetByID(decimal p_app_header_id, string p_language_code, 
            ref ApplicationHeaderInfo applicationHeaderInfo,
            ref List<AppDocumentInfo> appDocumentInfos, ref List<AppFeeFixInfo> appFeeFixInfos)
        {
            try
            {
                var objData = new App_Detail_PLB01_SDD_DA();
                DataSet dataSet = objData.GetByID(p_app_header_id, p_language_code);
                App_Detail_PLB01_SDD_Info _App_Detail_PLB01_SDD_Info = CBO<App_Detail_PLB01_SDD_Info>.FillObjectFromDataSet(dataSet);
                if (dataSet != null && dataSet.Tables.Count == 4)
                {
                    applicationHeaderInfo = CBO<ApplicationHeaderInfo>.FillObjectFromDataTable(dataSet.Tables[1]);
                    appDocumentInfos = CBO<AppDocumentInfo>.FillCollectionFromDataTable(dataSet.Tables[2]);
                    appFeeFixInfos = CBO<AppFeeFixInfo>.FillCollectionFromDataTable(dataSet.Tables[3]);
                }

                return _App_Detail_PLB01_SDD_Info;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new App_Detail_PLB01_SDD_Info();
            }
        }
    }
}

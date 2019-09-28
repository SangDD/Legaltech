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
    public class App_Detail_C02_BL
    {

        public decimal Insert(App_Detail_C02_Info pInfo)
        {
            try
            {
                App_Detail_C02_DA objData = new App_Detail_C02_DA();
                return objData.Insert(pInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public int Update(App_Detail_C02_Info pInfo)
        {
            try
            {
                App_Detail_C02_DA objData = new App_Detail_C02_DA();
                return objData.UpDate(pInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public int Deleted(decimal p_app_header_id, string pAppCode, string pLanguage)
        {
            try
            {
                App_Detail_C02_DA objData = new App_Detail_C02_DA();
                return objData.Deleted(p_app_header_id, pAppCode, pLanguage);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public App_Detail_C02_Info GetByID(decimal p_app_header_id, string p_language_code,
            ref ApplicationHeaderInfo applicationHeaderInfo,
            ref List<AppDocumentInfo> appDocumentInfos, ref List<AppFeeFixInfo> appFeeFixInfos, 
            ref List<AppDocumentOthersInfo> _LstDocumentOthersInfo)
        {
            try
            {
                var objData = new App_Detail_C02_DA();
                DataSet dataSet = objData.GetByID(p_app_header_id, p_language_code);
                App_Detail_C02_Info app_Detail_C02 = CBO<App_Detail_C02_Info>.FillObjectFromDataSet(dataSet);
                if (dataSet != null && dataSet.Tables.Count == 5)
                {
                    applicationHeaderInfo = CBO<ApplicationHeaderInfo>.FillObjectFromDataTable(dataSet.Tables[1]);
                    appDocumentInfos = CBO<AppDocumentInfo>.FillCollectionFromDataTable(dataSet.Tables[2]);
                    appFeeFixInfos = CBO<AppFeeFixInfo>.FillCollectionFromDataTable(dataSet.Tables[3]);
                    _LstDocumentOthersInfo = CBO<AppDocumentOthersInfo>.FillCollectionFromDataTable(dataSet.Tables[4]);
                    
                }

                return app_Detail_C02;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new App_Detail_C02_Info();
            }
        }
    }
}

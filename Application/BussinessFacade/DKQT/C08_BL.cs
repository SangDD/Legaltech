using Common;
using ObjectInfos;
using System;
using DataAccess;
using ObjectInfos.ModuleTrademark;
using System.Collections.Generic;
using System.Data;

namespace BussinessFacade
{
    public class C08_BL
    {
        public decimal Insert(C08_Info pInfo)
        {
            try
            {
                C08_DA _obj_da = new C08_DA();
                return _obj_da.Insert(pInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }


        public decimal UpDate(C08_Info pInfo)
        {
            try
            {
                C08_DA _obj_da = new C08_DA();
                return _obj_da.UpDate(pInfo);
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
                C08_DA _obj_da = new C08_DA();
                return _obj_da.Deleted(p_app_header_id, pAppCode, pLanguage);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public C08_Info GetByID(decimal p_app_header_id, string p_language_code,
            ref ApplicationHeaderInfo applicationHeaderInfo,
            ref List<AppDocumentInfo> appDocumentInfos,
            ref List<AppFeeFixInfo> appFeeFixInfos,
             ref List<Other_MasterInfo> pOther_MasterInfo,
            ref List<AppDocumentOthersInfo> pAppDocOtherInfo              
             )
        {
            try
            {
                C08_DA _obj_da = new C08_DA();
                DataSet dataSet = _obj_da.GetByID(p_app_header_id, p_language_code);
                C08_Info _C08_Info = CBO<C08_Info>.FillObjectFromDataSet(dataSet);
                if (dataSet != null && dataSet.Tables.Count == 6)
                {
                    applicationHeaderInfo = CBO<ApplicationHeaderInfo>.FillObjectFromDataTable(dataSet.Tables[1]);
                    appDocumentInfos = CBO<AppDocumentInfo>.FillCollectionFromDataTable(dataSet.Tables[2]);
                    appFeeFixInfos = CBO<AppFeeFixInfo>.FillCollectionFromDataTable(dataSet.Tables[3]);
                    pOther_MasterInfo = CBO<Other_MasterInfo>.FillCollectionFromDataTable(dataSet.Tables[4]);
                    pAppDocOtherInfo = CBO<AppDocumentOthersInfo>.FillCollectionFromDataTable(dataSet.Tables[5]);
                }

                return _C08_Info;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new C08_Info();
            }
        }

        public C08_Info_Export GetByID_Exp(decimal p_app_header_id, string p_language_code,
          ref ApplicationHeaderInfo applicationHeaderInfo,
          ref List<AppDocumentInfo> appDocumentInfos, 
          ref List<AppFeeFixInfo> appFeeFixInfos,
          ref List<Other_MasterInfo> pOther_MasterInfo,
          ref List<AppDocumentOthersInfo> pAppDocOtherInfo)
        {
            try
            {
                C08_DA _obj_da = new C08_DA();
                DataSet dataSet = _obj_da.GetByID(p_app_header_id, p_language_code);
                C08_Info_Export _C08_Info = CBO<C08_Info_Export>.FillObjectFromDataSet(dataSet);
                if (dataSet != null && dataSet.Tables.Count == 5)
                {
                    applicationHeaderInfo = CBO<ApplicationHeaderInfo>.FillObjectFromDataTable(dataSet.Tables[1]);
                    appDocumentInfos = CBO<AppDocumentInfo>.FillCollectionFromDataTable(dataSet.Tables[2]);
                    appFeeFixInfos = CBO<AppFeeFixInfo>.FillCollectionFromDataTable(dataSet.Tables[3]);
                    pOther_MasterInfo = CBO<Other_MasterInfo>.FillCollectionFromDataTable(dataSet.Tables[4]);
                    pAppDocOtherInfo = CBO<AppDocumentOthersInfo>.FillCollectionFromDataTable(dataSet.Tables[5]);
                }
                return _C08_Info;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new C08_Info_Export();
            }
        }
    }
}

using Common;
using ObjectInfos;
using System;
using DataAccess;
using ObjectInfos.ModuleTrademark;
using System.Collections.Generic;
using System.Data;

namespace BussinessFacade
{
    public class C07_BL
    {
        public decimal Insert(C07_Info pInfo)
        {
            try
            {
                C07_DA _obj_da = new C07_DA();
                return _obj_da.Insert(pInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }


        public decimal UpDate(C07_Info pInfo)
        {
            try
            {
                C07_DA _obj_da = new C07_DA();
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
                C07_DA _obj_da = new C07_DA();
                return _obj_da.Deleted(p_app_header_id, pAppCode, pLanguage);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public C07_Info GetByID(decimal p_app_header_id, string p_language_code,
            ref ApplicationHeaderInfo applicationHeaderInfo,
            ref List<AppDocumentInfo> appDocumentInfos,
            ref List<AppFeeFixInfo> appFeeFixInfos,
             ref List<Other_MasterInfo> pOther_MasterInfo,
            ref List<AppDocumentOthersInfo> pAppDocOtherInfo,
            ref List<AppClassDetailInfo> appClassDetailInfos 
             )
        {
            try
            {
                C07_DA _obj_da = new C07_DA();
                DataSet dataSet = _obj_da.GetByID(p_app_header_id, p_language_code);
                C07_Info _C07_Info = CBO<C07_Info>.FillObjectFromDataSet(dataSet);
                if (dataSet != null && dataSet.Tables.Count == 7)
                {
                    applicationHeaderInfo = CBO<ApplicationHeaderInfo>.FillObjectFromDataTable(dataSet.Tables[1]);
                    appDocumentInfos = CBO<AppDocumentInfo>.FillCollectionFromDataTable(dataSet.Tables[2]);
                    appFeeFixInfos = CBO<AppFeeFixInfo>.FillCollectionFromDataTable(dataSet.Tables[3]);
                    pOther_MasterInfo = CBO<Other_MasterInfo>.FillCollectionFromDataTable(dataSet.Tables[4]);
                    appClassDetailInfos = CBO<AppClassDetailInfo>.FillCollectionFromDataTable(dataSet.Tables[5]);
                    pAppDocOtherInfo = CBO<AppDocumentOthersInfo>.FillCollectionFromDataTable(dataSet.Tables[6]);
                }

                return _C07_Info;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new C07_Info();
            }
        }

        public C07_Info_Export GetByID_Exp(decimal p_app_header_id, string p_language_code,
          ref ApplicationHeaderInfo applicationHeaderInfo,
          ref List<AppDocumentInfo> appDocumentInfos, 
          ref List<AppFeeFixInfo> appFeeFixInfos,
          ref List<Other_MasterInfo> pOther_MasterInfo,
          ref List<AppDocumentOthersInfo> pAppDocOtherInfo )
        {
            try
            {
                C07_DA _obj_da = new C07_DA();
                DataSet dataSet = _obj_da.GetByID(p_app_header_id, p_language_code);
                C07_Info_Export _C07_Info = CBO<C07_Info_Export>.FillObjectFromDataSet(dataSet);
                if (dataSet != null && dataSet.Tables.Count == 6)
                {
                    applicationHeaderInfo = CBO<ApplicationHeaderInfo>.FillObjectFromDataTable(dataSet.Tables[1]);
                    appDocumentInfos = CBO<AppDocumentInfo>.FillCollectionFromDataTable(dataSet.Tables[2]);
                    appFeeFixInfos = CBO<AppFeeFixInfo>.FillCollectionFromDataTable(dataSet.Tables[3]);
                    pOther_MasterInfo = CBO<Other_MasterInfo>.FillCollectionFromDataTable(dataSet.Tables[4]);
                    pAppDocOtherInfo = CBO<AppDocumentOthersInfo>.FillCollectionFromDataTable(dataSet.Tables[5]);
                }
                return _C07_Info;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new C07_Info_Export();
            }
        }

        public App_Detail_TM06DKQT_Info AppTM06DKQTGetByID(string pAppHeaderID, 
            string pLanguage, int pStatus, ref List<AppClassDetailInfo> pListAppClass)
        {
            List<AppClassDetailInfo> _listAppClass = new List<AppClassDetailInfo>();
            App_Detail_TM06DKQT_Info _Tm06DKQT = new App_Detail_TM06DKQT_Info();
            DataSet _DS = new DataSet();
            try
            {
                var objData = new C07_DA();
                _DS = objData.AppTM06DKQTGetByID(pAppHeaderID, pLanguage, pStatus);
                if (_DS != null && _DS.Tables.Count == 3)
                {
                    _Tm06DKQT = CBO<App_Detail_TM06DKQT_Info>.FillObjectFromDataSet(_DS);
                    _listAppClass = CBO<AppClassDetailInfo>.FillCollectionFromDataTable(_DS.Tables[2]);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return _Tm06DKQT;
            }
            pListAppClass = _listAppClass;
            return _Tm06DKQT;
        }
    }
}

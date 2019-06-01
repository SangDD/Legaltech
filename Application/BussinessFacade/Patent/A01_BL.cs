using Common;
using ObjectInfos;
using System;
using DataAccess;
using ObjectInfos.ModuleTrademark;
using System.Collections.Generic;
using System.Data;

namespace BussinessFacade
{
    public class A01_BL
    {
        public decimal Insert(A01_Info pInfo)
        {
            try
            {
                A01_DA _obj_da = new A01_DA();
                return _obj_da.Insert(pInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }


        public decimal UpDate(A01_Info pInfo)
        {
            try
            {
                A01_DA _obj_da = new A01_DA();
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
                A01_DA _obj_da = new A01_DA();
                return _obj_da.Deleted(p_app_header_id, pAppCode, pLanguage);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public A01_Info GetByID(decimal p_app_header_id, string p_language_code,
            ref ApplicationHeaderInfo applicationHeaderInfo,
            ref List<AppDocumentInfo> appDocumentInfos, ref List<AppFeeFixInfo> appFeeFixInfos,
            ref List<AuthorsInfo> pAppAuthorsInfo, ref List<Other_MasterInfo> pOther_MasterInfo,
            ref List<AppClassDetailInfo> appClassDetailInfos, ref List<AppDocumentOthersInfo> pAppDocOtherInfo)
        {
            try
            {
                A01_DA _obj_da = new A01_DA();
                DataSet dataSet = _obj_da.GetByID(p_app_header_id, p_language_code);
                A01_Info _A01_Info = CBO<A01_Info>.FillObjectFromDataSet(dataSet);
                if (dataSet != null && dataSet.Tables.Count == 8)
                {
                    applicationHeaderInfo = CBO<ApplicationHeaderInfo>.FillObjectFromDataTable(dataSet.Tables[1]);
                    appDocumentInfos = CBO<AppDocumentInfo>.FillCollectionFromDataTable(dataSet.Tables[2]);
                    appFeeFixInfos = CBO<AppFeeFixInfo>.FillCollectionFromDataTable(dataSet.Tables[3]);
                    pOther_MasterInfo = CBO<Other_MasterInfo>.FillCollectionFromDataTable(dataSet.Tables[4]);
                    pAppAuthorsInfo = CBO<AuthorsInfo>.FillCollectionFromDataTable(dataSet.Tables[5]);

                    appClassDetailInfos = CBO<AppClassDetailInfo>.FillCollectionFromDataTable(dataSet.Tables[6]);
                    pAppDocOtherInfo = CBO<AppDocumentOthersInfo>.FillCollectionFromDataTable(dataSet.Tables[7]);
                }

                return _A01_Info;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new A01_Info();
            }
        }

    }
}

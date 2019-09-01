using Common;
using DataAccess;
using ObjectInfos;
using ObjectInfos.ModuleTrademark;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessFacade.Patent
{
    public class B03_BL
    {
        public decimal Insert(B03_Info pInfo)
        {
            try
            {
                B03_DA _obj_da = new B03_DA();
                return _obj_da.Insert(pInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }


        public decimal UpDate(B03_Info pInfo)
        {
            try
            {
                B03_DA _obj_da = new B03_DA();
                return _obj_da.UpDate(pInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public int Deleted(decimal p_app_header_id)
        {
            try
            {
                B03_DA _obj_da = new B03_DA();
                return _obj_da.Deleted(p_app_header_id);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public B03_Info GetByID(decimal p_app_header_id, string p_language_code,
           ref ApplicationHeaderInfo applicationHeaderInfo,
           ref List<AppDocumentInfo> appDocumentInfos, ref List<AppFeeFixInfo> appFeeFixInfos)
        {
            try
            {
                B03_DA _obj_da = new B03_DA();
                DataSet dataSet = _obj_da.GetByID(p_app_header_id, p_language_code);
                B03_Info _B03_Info = CBO<B03_Info>.FillObjectFromDataSet(dataSet);
                if (dataSet != null && dataSet.Tables.Count == 4)
                {
                    applicationHeaderInfo = CBO<ApplicationHeaderInfo>.FillObjectFromDataTable(dataSet.Tables[1]);
                    appDocumentInfos = CBO<AppDocumentInfo>.FillCollectionFromDataTable(dataSet.Tables[2]);
                    appFeeFixInfos = CBO<AppFeeFixInfo>.FillCollectionFromDataTable(dataSet.Tables[3]);
                }

                return _B03_Info;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new B03_Info();
            }
        }


    }
}

using Common;
using DataAccess.ModuleTrademark;
using ObjectInfos.ModuleTrademark;
using System;
using System.Collections.Generic;
using System.Data;

namespace BussinessFacade.ModuleTrademark
{
    public class AppFeeFixBL
    {
        public int AppFeeFixInsertBath(List<AppFeeFixInfo> pInfo, string p_case_code)
        {
            try
            {
                AppFeeFixDA objData = new AppFeeFixDA();
                return objData.AppFeeFixInsertBatch(pInfo, p_case_code);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public int AppFeeFixDelete(string p_case_code, string p_language_code)
        {
            try
            {
                AppFeeFixDA objData = new AppFeeFixDA();
                return objData.AppFeeFixDelete(p_case_code, p_language_code);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public List<AppFeeFixInfo> GetByCaseCode(string p_case_code)
        {
            try
            {
                AppFeeFixDA objData = new AppFeeFixDA();
                DataSet _ds = objData.GetByCaseCode(p_case_code);
                return CBO<AppFeeFixInfo>.FillCollectionFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<AppFeeFixInfo>();
            }
        }

        public List<AppFeeFixInfo> AppFee_Search(string p_key_search, ref decimal p_total_record,
           string p_from = "1", string p_to = "10", string p_sort_type = "ALL")
        {
            try
            {
                AppFeeFixDA objData = new AppFeeFixDA();
                DataSet _ds = objData.AppFee_Search(p_key_search, p_from, p_to, p_sort_type, ref p_total_record);
                return CBO<AppFeeFixInfo>.FillCollectionFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<AppFeeFixInfo>();
            }
        }

        public AppFeeFixInfo AppFee_GetById(decimal p_id)
        {
            try
            {
                AppFeeFixDA objData = new AppFeeFixDA();
                DataSet _ds = objData.AppFee_GetById(p_id);
                return CBO<AppFeeFixInfo>.FillObjectFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new AppFeeFixInfo();
            }
        }

        public decimal AppFee_UpdateById(AppFeeFixInfo appFeeFixInfo)
        {
            try
            {
                AppFeeFixDA objData = new AppFeeFixDA();
                return objData.AppFee_UpdateById(appFeeFixInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }
    }
}

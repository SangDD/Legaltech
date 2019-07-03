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
    }
}

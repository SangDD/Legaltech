using Common;
using ObjectInfos;
using System;
using System.Data;
using DataAccess;
using System.Collections.Generic;

namespace BussinessFacade
{
    public class Other_Master_BL
    {
        public decimal Insert(List<Other_MasterInfo> pInfo)
        {
            try
            {
                Other_Master_DA _obj_da = new Other_Master_DA();
                return _obj_da.Insert(pInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public decimal Update(Other_MasterInfo pInfo)
        {
            try
            {
                Other_Master_DA _obj_da = new Other_Master_DA();
                return _obj_da.Update(pInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public int Deleted(string p_case_code, string pLanguage)
        {
            try
            {
                Other_Master_DA _obj_da = new Other_Master_DA();
                return _obj_da.Deleted(p_case_code, pLanguage);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

    }
}

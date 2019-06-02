using Common;
using ObjectInfos;
using System;
using DataAccess;
using System.Collections.Generic;

namespace BussinessFacade
{
    public class Uu_Tien_BL
    {
        public decimal Insert(List<UTienInfo> pInfo)
        {
            try
            {
                UuTien_DA _obj_da = new UuTien_DA();
                return _obj_da.Insert(pInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public decimal Update(UTienInfo pInfo)
        {
            try
            {
                UuTien_DA _obj_da = new UuTien_DA();
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
                UuTien_DA _obj_da = new UuTien_DA();
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

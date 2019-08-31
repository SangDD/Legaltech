using Common;
using DataAccess;
using ObjectInfos;
using System;
using System.Collections.Generic;
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
    }
}

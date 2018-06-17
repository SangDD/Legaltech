using Common;
using DataAccess;
using ObjectInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessFacade
{
  public  class AppDetail06DKQT_BL
    {
        public int App_Detail_06TMDKQT_Insert(App_Detail_TM06DKQT_Info pInfo)
        {
            try
            {
                App_Detail_TM06DKQT_DA _Da = new App_Detail_TM06DKQT_DA();
                return _Da.App_Detail_06TMDKQT_Insert(pInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public int App_Detail_06TMDKQT_Update(App_Detail_TM06DKQT_Info pInfo)
        {
            try
            {
                App_Detail_TM06DKQT_DA _Da = new App_Detail_TM06DKQT_DA();
                return _Da.App_Detail_06TMDKQT_Update(pInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }
    }
}

using Common;
using DataAccess;
using ObjectInfos;
using System;
using System.Collections.Generic;
using System.Data;
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

        public DataSet AppTM06DKQTGetByID(decimal pAppHeaderId, string pLanguage, int pStatus)
        {
            try
            {
                var objData = new App_Detail_TM06DKQT_DA();
                return objData.AppTM06DKQTGetByID(pAppHeaderId, pLanguage, pStatus);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();

            }
        }

        public List<App_Detail_TM06DKQT_Info> AppTM06SearchByStatus(int p_status, string p_languagecode)
        {
            try
            {
                App_Detail_TM06DKQT_DA objData = new App_Detail_TM06DKQT_DA();
                DataSet ds = objData.AppTM06SearchByStatus(p_status, p_languagecode);
                return CBO<App_Detail_TM06DKQT_Info>.FillCollectionFromDataSet(ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<App_Detail_TM06DKQT_Info>();
            }
        }
    }
}

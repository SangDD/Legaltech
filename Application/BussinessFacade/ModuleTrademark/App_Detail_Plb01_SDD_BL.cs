using Common;
using DataAccess;
using DataAccess.ModuleTrademark;
using ObjectInfos;
using System;
using System.Data;

namespace BussinessFacade.ModuleTrademark
{
    public class App_Detail_PLB01_SDD_BL
    {

        public int Insert(App_Detail_PLB01_SDD_Info pInfo)
        {
            try
            {
                App_Detail_PLB01_SDD_DA objData = new App_Detail_PLB01_SDD_DA();
                return objData.Insert(pInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public int Update(App_Detail_PLB01_SDD_Info pInfo)
        {
            try
            {
                App_Detail_PLB01_SDD_DA objData = new App_Detail_PLB01_SDD_DA();
                return objData.UpDate(pInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public int Deleted(decimal pAppHeaderID, string pAppCode, string pLanguage)
        {
            try
            {
                App_Detail_PLB01_SDD_DA objData = new App_Detail_PLB01_SDD_DA();
                return objData.Deleted(pAppHeaderID, pAppCode, pLanguage);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public DataSet GetByID(decimal p_id, decimal p_app_header_id, string p_language_code)
        {
            try
            {
                var objData = new App_Detail_PLB01_SDD_DA();
                return objData.GetByID(p_id, p_app_header_id, p_language_code);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();

            }
        }
    }
}

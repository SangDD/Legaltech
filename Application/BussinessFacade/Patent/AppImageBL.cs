using Common;
using DataAccess.ModuleTrademark;
using System;
using System.Collections.Generic;
using ObjectInfos.ModuleTrademark;
using ObjectInfos;
using System.Data;

namespace BussinessFacade.ModuleTrademark
{
    public class AppImageBL
    {

        public List<AppDocumentOthersInfo> GetByAppHeader(decimal p_app_header_id, string p_language_code)
        {
            try
            {
                AppImageDA objData = new AppImageDA();
                DataSet ds = objData.GetByAppHeader(p_app_header_id, p_language_code);
                return CBO<AppDocumentOthersInfo>.FillCollectionFromDataSet(ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<AppDocumentOthersInfo>();
            }
        }


        public int AppImageInsertBatch(List<AppDocumentOthersInfo> pInfo)
        {
            try
            {
                AppImageDA objData = new AppImageDA();
                return objData.AppImageInsertBatch(pInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public int AppImageDeletedByApp(decimal pAppHeaderID, string pLanguage)
        {
            try
            {
                AppImageDA objData = new AppImageDA();
                return objData.AppImageDeletedByApp(pAppHeaderID, pLanguage);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public int AppImageDelByID(decimal pID, string pLanguage)
        {
            try
            {
                AppImageDA objData = new AppImageDA();
                return objData.AppImageDelByID(pID, pLanguage);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }
    }
}

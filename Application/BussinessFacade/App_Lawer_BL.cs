using Common;
using DataAccess;
using ObjectInfos;
using System;
using System.Collections.Generic;
using System.Data;
using ZetaCompressionLibrary;

namespace BussinessFacade
{
    public class App_Lawer_BL
    {

        public List<App_Lawer_Info> GetApp_Grant4Lawer(decimal p_lawer_id)
        {
            try
            {
                App_Lawer_DA _da = new App_Lawer_DA();
                DataSet _ds = _da.GetApp_Grant4Lawer(p_lawer_id);
                return CBO<App_Lawer_Info>.FillCollectionFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<App_Lawer_Info>();
            }
        }

        public decimal App_Lawer_Insert(App_Lawer_Info p_obj)
        {
            try
            {
                App_Lawer_DA _da = new App_Lawer_DA();
                return _da.App_Lawer_Insert(p_obj.Application_Header_Id, p_obj.Lawer_Id, p_obj.Notes, p_obj.Language_Code, p_obj.Created_By,p_obj.Created_Date);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }
    }
}

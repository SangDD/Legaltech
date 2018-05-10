using Common;
using DataAccess.ModuleTrademark;
using ObjectInfos;
using ObjectInfos.ModuleTrademark;
using System;
using System.Collections.Generic;
using System.Data;
using ZetaCompressionLibrary;

namespace BussinessFacade.ModuleTrademark
{
    public class Application_Header_BL
    {
        public List<ApplicationHeaderInfo> ApplicationHeader_Search(string p_key_search, ref decimal p_total_record,
            string p_from = "1", string p_to = "10", string p_sort_type = "ALL")
        {
            try
            {
                Application_Header_DA _da = new Application_Header_DA();
                DataSet _ds = _da.ApplicationHeader_Search(p_key_search, p_from, p_to, p_sort_type, ref p_total_record);
                return CBO<ApplicationHeaderInfo>.FillCollectionFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<ApplicationHeaderInfo>();
            }
        }
    }
}

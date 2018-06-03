using Common;
using DataAccess;
using ObjectInfos;
using System;
using System.Collections.Generic;
using System.Data;
using ZetaCompressionLibrary;

namespace BussinessFacade
{
    public class Lawer_Info_BL
    {
        public List<Lawer_Info> Lawer_Info_GetAll()
        {
            try
            {
                Lawer_Info_DA _da = new Lawer_Info_DA();
                DataSet _ds = _da.Lawer_Info_GetAll();
                return CBO<Lawer_Info>.FillCollectionFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<Lawer_Info>();
            }
        }

        public List<Lawer_Info> Lawer_Info_Search(string p_key_search, ref decimal p_total_record,
            string p_from = "1", string p_to = "10", string p_sort_type = "ALL")
        {
            try
            {
                Lawer_Info_DA _da = new Lawer_Info_DA();
                DataSet _ds = _da.Lawer_Info_Search(p_key_search, p_from, p_to, p_sort_type, ref p_total_record);
                return CBO<Lawer_Info>.FillCollectionFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<Lawer_Info>();
            }
        }


    }
}

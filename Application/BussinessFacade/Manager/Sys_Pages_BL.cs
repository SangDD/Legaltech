using Common;
using DataAccess;
using DataAccess.ModuleArticles;
using ObjectInfos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessFacade
{
    public class Sys_Pages_BL
    {
        public List<Sys_Pages_Info> Sys_Pages_Search(string p_key_search, ref decimal p_total_record,
               string p_from = "1", string p_to = "10", string p_orderby = "ALL")
        {
            try
            {
                Sys_Pages_DA _da = new Sys_Pages_DA();
                DataSet _ds = _da.Sys_Pages_Search(p_key_search, p_from, p_to, p_orderby, ref p_total_record);
                return CBO<Sys_Pages_Info>.FillCollectionFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<Sys_Pages_Info>();
            }
        }

        public Sys_Pages_Info Sys_Pages_GetBy_Code(string p_code)
        {
            try
            {
                Sys_Pages_DA objDA = new Sys_Pages_DA();
                DataSet ResultData = objDA.Sys_Pages_GetBy_Code(p_code);
                return CBO<Sys_Pages_Info>.FillObjectFromDataSet(ResultData);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new Sys_Pages_Info();
            }
        }

        public Sys_Pages_Info Sys_Pages_GetById(decimal p_id)
        {
            try
            {
                Sys_Pages_DA objDA = new Sys_Pages_DA();
                DataSet ResultData = objDA.Sys_Pages_GetById(p_id);
                return CBO<Sys_Pages_Info>.FillObjectFromDataSet(ResultData);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new Sys_Pages_Info();
            }
        }

        public decimal Sys_Pages_Insert(Sys_Pages_Info pInfo)
        {
            try
            {
                Sys_Pages_DA objDA = new Sys_Pages_DA();
                return objDA.Sys_Pages_Insert(pInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public decimal Sys_Pages_Update(Sys_Pages_Info pInfo)
        {
            try
            {
                Sys_Pages_DA objDA = new Sys_Pages_DA();
                return objDA.Sys_Pages_Update(pInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public decimal Sys_Pages_Deleted(Sys_Pages_Info pInfo)
        {
            try
            {
                Sys_Pages_DA objDA = new Sys_Pages_DA();
                return objDA.Sys_Pages_Deleted(pInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

    }
}

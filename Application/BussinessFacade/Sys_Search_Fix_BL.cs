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
    public class Sys_Search_Fix_BL
    {
        public List<Sys_Search_Fix_Info> Sys_Search_Fix_GetAll()
        {
            try
            {
                Sys_Search_Fix_DA _da = new Sys_Search_Fix_DA();
                DataSet _ds = _da.Sys_Search_Fix_GetAll();
                return CBO<Sys_Search_Fix_Info>.FillCollectionFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<Sys_Search_Fix_Info>();
            }
        }

        public Sys_Search_Fix_Info Sys_Search_Fix_GetById(decimal p_id)
        {
            try
            {
                Sys_Search_Fix_DA _da = new Sys_Search_Fix_DA();
                DataSet _ds = _da.Sys_Search_Fix_GetById(p_id);
                return CBO<Sys_Search_Fix_Info>.FillObjectFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new Sys_Search_Fix_Info();
            }
        }

        public decimal Sys_Search_Fee_Insert(Sys_Search_Fix_Info p_obj)
        {
            try
            {
                Sys_Search_Fix_DA _da = new Sys_Search_Fix_DA();
                return _da.Sys_Search_Fee_Insert(p_obj);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal Sys_Search_Fee_Update(Sys_Search_Fix_Info p_obj)
        {
            try
            {
                Sys_Search_Fix_DA _da = new Sys_Search_Fix_DA();
                return _da.Sys_Search_Fee_Update(p_obj);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal Sys_Search_Fee_Delete(decimal p_id, string p_modified_by)
        {
            try
            {
                Sys_Search_Fix_DA _da = new Sys_Search_Fix_DA();
                return _da.Sys_Search_Fee_Delete(p_id, p_modified_by);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }
    }
}

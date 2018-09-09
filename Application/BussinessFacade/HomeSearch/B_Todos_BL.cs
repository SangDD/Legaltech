using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObjectInfos;
using DataAccess;
using System.Data;
using Common;

namespace BussinessFacade
{
   public class B_Todos_BL
    {
        public List<B_Todos_Info> B_Todos_Search(string p_key_search, ref decimal p_total_record,
                string p_from = "1", string p_to = "10", string p_sort_type = "ALL")
        {
            try
            {
                B_TODOS_DA _da = new B_TODOS_DA();
                DataSet _ds = _da.B_Todos_Search(p_key_search, p_from, p_to, p_sort_type, ref p_total_record);
                return CBO<B_Todos_Info>.FillCollectionFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<B_Todos_Info>();
            }
        }
    }
}

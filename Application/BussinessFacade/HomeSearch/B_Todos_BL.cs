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
        public List<B_Todos_Info> Notify_Search(string p_key_search,string p_user_name, ref decimal p_total_record,
                string p_from = "1", string p_to = "10", string p_sort_type = "ALL")
        {
            try
            {
                B_TODOS_DA _da = new B_TODOS_DA();
                DataSet _ds = _da.Notify_Search(p_key_search,p_user_name, p_from, p_to, p_sort_type, ref p_total_record);
                return CBO<B_Todos_Info>.FillCollectionFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<B_Todos_Info>();
            }
        }

        public List<B_Remind_Info> Notify_R_Search(string p_key_search, string p_user_name, ref decimal p_total_record,
               string p_from = "1", string p_to = "10", string p_sort_type = "ALL")
        {
            try
            {
                B_TODOS_DA _da = new B_TODOS_DA();
                DataSet _ds = _da.Notify_Search(p_key_search, p_user_name, p_from, p_to, p_sort_type, ref p_total_record);
                return CBO<B_Remind_Info>.FillCollectionFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<B_Remind_Info>();
            }
        }


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

        public List<B_Remind_Info> B_Remind_Search(string p_key_search, ref decimal p_total_record,
                string p_from = "1", string p_to = "10", string p_sort_type = "ALL")
        {
            try
            {
                B_TODOS_DA _da = new B_TODOS_DA();
                DataSet _ds = _da.B_Remind_Search(p_key_search, p_from, p_to, p_sort_type, ref p_total_record);
                return CBO<B_Remind_Info>.FillCollectionFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<B_Remind_Info>();
            }
        }

        public List<B_Todos_Info> NotifiGetByCasecode(string p_key_search, ref List<B_Remind_Info> p_remind_list)
        {
            try
            {
                B_TODOS_DA _da = new B_TODOS_DA();
                DataSet _ds = _da.NotifiGetByCasecode(p_key_search);
                p_remind_list = CBO<B_Remind_Info>.FillCollectionFromDataTable(_ds.Tables[1]);
                return CBO<B_Todos_Info>.FillCollectionFromDataTable(_ds.Tables[0]);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<B_Todos_Info>();
            }
        }
        public B_Todos_Info Todo_GetByCaseCode(decimal p_app_id, string p_processor_by)
        {
            try
            {
                B_TODOS_DA _da = new B_TODOS_DA();
                DataSet _ds = _da.Todo_GetByCaseCode(p_app_id, p_processor_by);
                return CBO<B_Todos_Info>.FillObjectFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }
        }

        public bool UpdateTodo_ByCaseCode(decimal p_app_id, string p_processor_by)
        {
            try
            {
                B_TODOS_DA _da = new B_TODOS_DA();
                return _da.UpdateTodo_ByCaseCode(p_app_id, p_processor_by);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return false;
            }
        }

        public B_TodoNotify_Info GET_NOTIFY(string p_username)
        {
            try
            {
                B_TODOS_DA _da = new B_TODOS_DA();
                DataSet _ds = _da.GET_NOTIFY(p_username);
                return CBO<B_TodoNotify_Info>.FillObjectFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new B_TodoNotify_Info();
            }
        }

    }
}

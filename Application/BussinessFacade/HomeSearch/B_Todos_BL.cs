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
        public List<B_Todos_Info> Notify_Search(string p_key_search, string p_user_name, ref decimal p_total_record,
                string p_from = "1", string p_to = "10", string p_sort_type = "ALL")
        {
            try
            {
                B_TODOS_DA _da = new B_TODOS_DA();
                DataSet _ds = _da.Notify_Search(p_key_search, p_user_name, p_from, p_to, p_sort_type, ref p_total_record);
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

        public B_Todos_Info Todo_GetByCaseCode(string p_case_code, string p_processor_by)
        {
            try
            {
                B_TODOS_DA _da = new B_TODOS_DA();
                DataSet _ds = _da.Todo_GetByCaseCode(p_case_code, p_processor_by);
                return CBO<B_Todos_Info>.FillObjectFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }
        }

        public B_Todos_Info Todo_GetByAppId(decimal p_app_id, string p_processor_by)
        {
            try
            {
                B_TODOS_DA _da = new B_TODOS_DA();
                DataSet _ds = _da.Todo_GetByAppId(p_app_id, p_processor_by);
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

        public bool Remind_Insert_Common(decimal p_type, string p_case_code, decimal p_ref_id, string p_user_name, string p_language_code)
        {
            try
            {
                B_TODOS_DA _da = new B_TODOS_DA();
                return _da.Remind_Insert_Common(p_type, p_case_code, p_ref_id, p_user_name, p_language_code);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return false;
            }
        }

        public bool Remind_Insert_ByTodo(decimal p_type, string p_case_code, decimal p_ref_id, 
            string p_request_by, string p_language_code,
            string p_content = "NA", string p_processor_by = "NA")
        {
            try
            {
                B_TODOS_DA _da = new B_TODOS_DA();
                return _da.Remind_Insert_ByTodo(p_type, p_case_code, p_ref_id, p_request_by, p_content, p_processor_by, p_language_code);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return false;
            }
        }

        public List<B_Todos_Info> GetSend_Email()
        {
            try
            {
                B_TODOS_DA _da = new B_TODOS_DA();
                DataSet _ds = _da.GetSend_Email();
                return CBO<B_Todos_Info>.FillCollectionFromDataTable(_ds.Tables[0]);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<B_Todos_Info>();
            }
        }

        public bool Update_Todo_Email(List<B_Todos_Info> p_lst)
        {
            try
            {
                B_TODOS_DA _da = new B_TODOS_DA();
                return _da.Update_Todo_Email(p_lst);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return false;
            }
        }
    }
}

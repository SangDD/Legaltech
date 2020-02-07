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
    public class Email_BL
    {
        public List<Email_Info> Email_Search(string p_user_name, string p_key_search, ref decimal p_total_record,
           string p_from = "1", string p_to = "10", string p_sort_type = "ALL")
        {
            try
            {
                EmailDA _da = new EmailDA();
                DataSet _ds = _da.Email_Search(p_user_name, p_key_search, p_from, p_to, p_sort_type, ref p_total_record);
                return CBO<Email_Info>.FillCollectionFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<Email_Info>();
            }
        }


        public Email_Info Email_GetBy_Id(decimal p_id, string p_case_code, string p_language_code)
        {
            try
            {
                EmailDA _da = new EmailDA();
                DataSet _ds = _da.Email_GetBy_Id(p_id, p_case_code, p_language_code);
                return CBO<Email_Info>.FillObjectFromDataTable(_ds.Tables[0]);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new Email_Info();
            }
        }

        public decimal Send_Email_Insert(Email_Info info)
        {
            try
            {
                EmailDA emailDA = new EmailDA();
                return emailDA.Send_Email_Insert(info.EmailFrom, info.EmailTo, info.EmailCC, info.Display_Name,
                    info.Subject, info.Content, info.LstAttachment, info.Status, info.Send_Time, info.Created_by);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }


        public List<Template_Email_Info> Template_Email_Search(string p_user_name, string p_key_search, ref decimal p_total_record,
           string p_from = "1", string p_to = "10", string p_sort_type = "ALL")
        {
            try
            {
                Template_EmailDA _da = new Template_EmailDA();
                DataSet _ds = _da.Template_Email_Search(p_user_name, p_key_search, p_from, p_to, p_sort_type, ref p_total_record);
                return CBO<Template_Email_Info>.FillCollectionFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<Template_Email_Info>();
            }
        }

        public Template_Email_Info Template_Email_GetBy_Id(decimal p_id, string p_language_code)
        {
            try
            {
                Template_EmailDA _da = new Template_EmailDA();
                DataSet _ds = _da.Template_Email_GetBy_Id(p_id, p_language_code);
                return CBO<Template_Email_Info>.FillObjectFromDataTable(_ds.Tables[0]);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new Template_Email_Info();
            }
        }


        public List<Template_Email_Info> Template_email_GetAll()
        {
            try
            {
                Template_EmailDA _da = new Template_EmailDA();
                DataSet _ds = _da.Template_email_GetAll();
                return CBO<Template_Email_Info>.FillCollectionFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<Template_Email_Info>();
            }
        }

        public decimal Template_Email_Insert(Template_Email_Info info)
        {
            try
            {
                Template_EmailDA emailDA = new Template_EmailDA();
                return emailDA.Template_Email_Insert(info);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal Template_Email_Update(Template_Email_Info info)
        {
            try
            {
                Template_EmailDA emailDA = new Template_EmailDA();
                return emailDA.Template_Email_Update(info);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal Template_Email_Delete(decimal p_id)
        {
            try
            {
                Template_EmailDA emailDA = new Template_EmailDA();
                return emailDA.Template_Email_Delete(p_id);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }
    }
}

using Common;
using DataAccess.Manager;
using ObjectInfos.Manager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessFacade.Manager
{
    public class Contact_BL
    {
        public List<ContactInfo> Contact_Search(string p_key_search, string p_language, string p_status,ref decimal p_total_record,
            string p_from = "1", string p_to = "10")
        {
            try
            {
                ContactDA _da = new ContactDA();
                DataSet _ds = _da.Contact_Search(p_key_search, p_language, p_status, p_from, p_to, ref p_total_record);
                return CBO<ContactInfo>.FillCollectionFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<ContactInfo>();
            }
        }
        public ContactInfo Contact_GetByID(string p_id)
        {
            try
            {
                ContactDA _da = new ContactDA();
                DataSet _ds = _da.Contact_GetByID(p_id);
                return CBO<ContactInfo>.FillObjectFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new ContactInfo();
            }
        }
        public decimal Contact_Insert(ContactInfo _contact)
        {
            try
            {
                ContactDA _da = new ContactDA();
                decimal _ck = _da.Contact_Insert(_contact.ContactName, _contact.Subject, _contact.Email, _contact.Content, _contact.Language);
                return _ck;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal Contact_UpdateStatus(decimal p_id, decimal p_status, string p_replycontent, string p_replysubject, string p_replyby, string url, string url02)
        {
            try
            {
                ContactDA _da = new ContactDA();
                decimal _ck = _da.Contact_UpdateStatus(p_id, p_status, p_replycontent, p_replysubject, p_replyby, url, url02);
                return _ck;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

    }
}

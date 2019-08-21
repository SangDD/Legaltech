using BussinessFacade;
using BussinessFacade.Manager;
using Common;
using ObjectInfos;
using ObjectInfos.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApps.AppStart;
using WebApps.CommonFunction;

namespace WebApps.Areas.Contact.Controllers
{
    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("contact", AreaPrefix = "contact-send")]
    [Route("{action}")]
    public class ContactController : Controller
    {
        [HttpGet]
        [Route("contact")]
        public ActionResult ContactDisplay()
        {
            var objBL = new Sys_Pages_BL();
            string language = AppsCommon.GetCurrentLang();
            Sys_Pages_Info _pageInfo = objBL.Sys_Pages_GetBy_Code("abot");
            ViewBag.objNewInfo = _pageInfo;
            return View("~/Areas/Contact/Views/Contact/ContactDisplay.cshtml");
        }

        [HttpPost]
        [Route("send-mail")]
        public ActionResult SendEmail(string name, string mail, string sub, string content)
        {
            try
            {
                // insert
                Contact_BL _bl = new Contact_BL();
                string _LanguageCode = AppsCommon.GetCurrentLang();
                ContactInfo _contact = new ContactInfo();
                _contact.ContactName = name;
                _contact.Email = mail;
                _contact.Subject = sub;
                _contact.Content = content;
                _contact.Language = _LanguageCode;
                decimal _ck = _bl.Contact_Insert(_contact);
                return Json(new { status = _ck });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { status = -1 });
            }

        }

        [HttpGet]
        [Route("danh-sach-contact")]
        public ActionResult ListMailContact()
        {
            return PartialView("~/Areas/Contact/Views/Contact/ListMailContact.cshtml");
        }

        [HttpPost]
        [Route("searchcontact")]
        public ActionResult FindUser(string p_keysearch,  string p_language, string p_status,  int p_CurrentPage)
        {
            try
            {
                decimal _total_record = 0;

                string p_to = "";
                string p_from = CommonFuc.Get_From_To_Page(p_CurrentPage, ref p_to);

                var contact_bl = new Contact_BL();
                List<ContactInfo> _lst = contact_bl.Contact_Search(p_keysearch, p_language, p_status, ref _total_record, p_from, p_to);
                string htmlPaging = CommonFuc.Get_HtmlPaging<Contact_BL>((int)_total_record, p_CurrentPage, "Luật sư");

                ViewBag.Paging = htmlPaging;
                ViewBag.Obj = _lst;
                ViewBag.SumRecord = _total_record;

                return PartialView("~/Areas/Contact/Views/Contact/_Partial_List_Contact.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/Contact/Views/Contact/_Partial_List_Contact.cshtml");
            }
        }

       
        [HttpGet]
        [Route("xem-chi-tiet/{id}")]
        public ActionResult ViewDetaillawer()
        {
          
            try
            {
                if (RouteData.Values["id"] != null && RouteData.Values["id"].ToString() != "")
                {
                    int _Id = Convert.ToInt32(RouteData.Values["id"].ToString());
                    var contact_bl = new Contact_BL();
                    ContactInfo _contract = contact_bl.Contact_GetByID(_Id);
                    ViewBag.Contract = _contract;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return View("~/Areas/Contact/Views/Contact/_View_Detail_Contact.cshtml");
        }

    }
}
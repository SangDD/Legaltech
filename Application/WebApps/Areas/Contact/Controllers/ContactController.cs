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
    //[Route("{action}")]
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
            }catch(Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { status = -1 });
            }
           
        }
    }
}
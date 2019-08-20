using Common;
using ObjectInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApps.AppStart;

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
            return View("~/Areas/Contact/Views/Contact/ContactDisplay.cshtml");
        }

        [HttpPost]
        [Route("send-mail")]
        public ActionResult SendEmail(string name, string mail, string sub, string content)
        {
            try
            {
                Email_Info _Email_Info = new Email_Info
                {
                    EmailFrom = EmailHelper.EmailOriginal.EMailFrom,
                    Pass = EmailHelper.EmailOriginal.PassWord,
                    Display_Name = EmailHelper.EmailOriginal.DisplayName,
                    EmailTo = mail,
                    EmailCC = "",
                    Subject = sub,
                    Content = content,
                };
                CommonFunction.AppsCommon.EnqueueSendEmail(_Email_Info);
                return Json(new { status = 1 });
            }catch(Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { status = -1 });
            }
           
        }
    }
}
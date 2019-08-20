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
    }
}
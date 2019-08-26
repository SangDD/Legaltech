using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApps.AppStart;

namespace WebApps.Areas.Patent.Controllers
{
    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("PatentRegistration", AreaPrefix = "lg-patentB03")]
    [Route("{action}")]
    public class B03Controller : Controller
    {
        [HttpGet]
        [Route("register/{id}")]
        public ActionResult B03Display()
        {
            return View();
        }
    }
}
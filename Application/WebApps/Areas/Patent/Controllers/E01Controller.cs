using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApps.AppStart;

namespace WebApps.Areas.Patent.Controllers
{
    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("PatentRegistration", AreaPrefix = "lg-patentE01")]
    [Route("{action}")]
    public class E01Controller : Controller
    {
        [HttpGet]
        [Route("register/{id}")]
        public ActionResult Index()
        {
          return PartialView("~/Areas/Patent/Views/E01/_Partial_E01.cshtml");
        }
    }
}
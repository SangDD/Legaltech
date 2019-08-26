using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApps.AppStart;
using WebApps.Session;

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
            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("/");

                SessionData.CurrentUser.chashFile.Clear();
                string AppCode = "";
                if (RouteData.Values.ContainsKey("id"))
                {
                    AppCode = RouteData.Values["id"].ToString().ToUpper();
                }
                ViewBag.AppCode = AppCode;
                return PartialView("~/Areas/Patent/Views/B03/B03Display.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/Patent/Views/B03/B03Display.cshtml");
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApps.AppStart;

using ObjectInfos;
using BussinessFacade;
using Common;
using WebApps.CommonFunction;
using Common.CommonData;
using WebApps.AppStart;
using WebApps.Session;

namespace WebApps.Areas.Manager.Controllers
{

    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("Manager", AreaPrefix = "app-translate")]
    [Route("{action}")]
    public class TranslateController : Controller
    {

        [HttpGet]
        [Route("translate-app/{id}")]
        public ActionResult Translate_App()
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
                return PartialView("~/Areas/Manager/Views/Translate/Translate_App.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return View();
            }
        }
    }
}
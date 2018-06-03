using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BussinessFacade;
using Common;
using ObjectInfos;
using WebApps.AppStart;

namespace WebApps.Areas.AppClass.Controllers
{
    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("AppClass", AreaPrefix = "quan-tri-he-thong")]
    [Route("{action}")]
    public class AppClassController : Controller
    {
        // GET: AppClass/AppClass
        public ActionResult AppClassList()
        {
            try
            {
                var ObjBL = new App_Class_BL();
               var lstObj = ObjBL.SearchAppClass();
                ViewBag.Paging = ObjBL.GetPagingHtml();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }


            return View();
        }
    }
}
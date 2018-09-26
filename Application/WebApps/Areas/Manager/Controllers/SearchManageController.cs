using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApps.AppStart;
using WebApps.Session;

namespace WebApps.Areas.Manager.Controllers
{
    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("Manager", AreaPrefix = "quan-ly-search")]
    [Route("{action}")]
    public class SearchManageController : Controller
    {
        // GET: Manager/SearchManage

        [HttpGet]
        [Route("danh-sach-search/{id}")]
        public ActionResult Index()
        {
            if (SessionData.CurrentUser == null)
            {
                return Redirect("/dang-xuat");
            }
            try
            {
                int _Status = 0;
                if (RouteData.Values.ContainsKey("id"))
                {
                    _Status = Convert.ToInt32(RouteData.Values["id"]);
                    ViewBag.CurrStatus = _Status;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return View(@"~\Areas\Manager\Views\SearchManage\ListSearch.cshtml");
        }

        [HttpGet]
        [Route("them-moi")]
        public ActionResult SearchAdd()
        {
            if (SessionData.CurrentUser == null)
            {
                return Redirect("/dang-xuat");
            }
            try
            {
               
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return View(@"~\Areas\Manager\Views\SearchManage\SearchAdd.cshtml");
        }
    }
}
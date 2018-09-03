using Common;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WebApps.AppStart;
using ObjectInfos;
using BussinessFacade.ModuleTrademark;
using WebApps.Session;

namespace WebApps.Areas.Articles.Controllers
{
    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("tintuc", AreaPrefix = "quan-ly-tin")]
    [Route("{action}")]
    public class ArticlesNewsController : Controller
    {
        // GET: Articles/ArticlesNews
        [HttpGet]
        [Route("danh-sach-tin")]
        public ActionResult GetListArticles()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                Logger.LogException(ex);
                return View();
            }
        }
    }
}
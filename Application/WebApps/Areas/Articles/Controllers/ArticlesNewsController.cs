using Common;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WebApps.AppStart;
using ObjectInfos;
using BussinessFacade.ModuleTrademark;
using WebApps.Session;
using BussinessFacade;
using BussinessFacade.ModuleMemoryData;

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
                if (SessionData.CurrentUser == null)
                    return Redirect("/");
                decimal _total_record = 0;
                NewsBL objBL = new NewsBL();
                string _status = "ALL";
                ViewBag.Status = _status;
                string _keySearch = "ALL|" + _status;
                List<NewsInfo> _lst = objBL.ArticleHomeSearch(_keySearch, ref _total_record);
                string htmlPaging = CommonFuc.Get_HtmlPaging<NewsInfo>((int)_total_record, 1, "Tin");
                ViewBag.listArticles = _lst;
                ViewBag.Paging = htmlPaging;
                ViewBag.SumRecord = _total_record;
                ViewBag.lstCategory = MemoryData.AllCode_GetBy_CdTypeCdName("ARTICLES", "CATEGORIES");
                return View("~/Areas/Articles/Views/ArticlesNews/GetListArticles.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return View();
            }
        }

        [HttpGet]
        [Route("soan-bai-viet")]
        public ActionResult AddNewsArticles()
        {
            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("/");
                ViewBag.lstCategory = MemoryData.AllCode_GetBy_CdTypeCdName("ARTICLES", "CATEGORIES");

                return View("~/Areas/Articles/Views/ArticlesNews/_PartialViewAdd.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return View();
            }
        }

        [HttpGet]
        [Route("sua-bai-viet")]
        public ActionResult EdiNewsArticles(decimal pIDArticles,decimal pCategory, string pLanguage)
        {
            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("/");
                ViewBag.lstCategory = MemoryData.AllCode_GetBy_CdTypeCdName("ARTICLES", "CATEGORIES");
                return View("~/Areas/Articles/Views/ArticlesNews/_PartialviewEdit.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return View();
            }
        }
    }
}
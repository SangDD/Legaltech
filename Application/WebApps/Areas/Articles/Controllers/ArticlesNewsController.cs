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
using System.Web;
using WebApps.CommonFunction;

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
        public ActionResult AddNewsArticles(NewsInfo pNewsInfo)
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

        [HttpPost]
        [Route("them-moi-bai-viet")]
        public ActionResult SaveNewsArticles(NewsInfo pNewsInfo  )
        {
            try
            {
                decimal preturn = 0;
                if (pNewsInfo ==null)
                {
                       return Json(new { status = -99 });
                }
                string language = AppsCommon.GetCurrentLang();
                var CreatedBy = SessionData.CurrentUser.Username;
                var CreatedDate = SessionData.CurrentUser.CurrentDate;
                if (pNewsInfo.pfileLogo != null)
                {
                    pNewsInfo.Imageheader = AppLoadHelpers.PushFileToServer(pNewsInfo.pfileLogo, AppUpload.Logo);
                }
                pNewsInfo.Languagecode = language;
                pNewsInfo.Createdby = CreatedBy;
                pNewsInfo.Createddate = CreatedDate;
                var objNewsBL = new NewsBL();
                preturn = objNewsBL.ArticlesInsert(pNewsInfo);

                return Json(new { status = preturn });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return View();
            }
        }

        [HttpGet]
        [Route("sua-bai-viet/{id}")]
        public ActionResult EditNewsArticles()
        {
            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("/");
                decimal pIDArticles = 0;
                if (RouteData.Values.ContainsKey("id"))
                {
                    pIDArticles = CommonFuc.ConvertToDecimal(RouteData.Values["id"]);
                }

                var objNewsBL = new NewsBL();
                ViewBag.lstCategory = MemoryData.AllCode_GetBy_CdTypeCdName("ARTICLES", "CATEGORIES");
                string language = AppsCommon.GetCurrentLang();
                var objNewInfo = objNewsBL.ArticlesGetById(pIDArticles, language);
                return View("~/Areas/Articles/Views/ArticlesNews/_PartialviewEdit.cshtml",objNewInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return View();
            }
        }
        [HttpGet]
        [Route("xoa-bai-viet")]
        public ActionResult DelNewsArticles(decimal pIDArticles)
        {
            try
            {
                decimal preturn = 0;
                if (SessionData.CurrentUser == null)
                    return Redirect("/");
                var objNewsBL = new NewsBL();
                ViewBag.lstCategory = MemoryData.AllCode_GetBy_CdTypeCdName("ARTICLES", "CATEGORIES");
                string language = AppsCommon.GetCurrentLang();
                NewsInfo pNewsInfo = new NewsInfo();
                var ModifiedBy = SessionData.CurrentUser.Username;
                var ModifiedDate = SessionData.CurrentUser.CurrentDate;
                pNewsInfo.Id = pIDArticles;
                pNewsInfo.Languagecode = language;
                pNewsInfo.Modifiedby = ModifiedBy;
                pNewsInfo.Modifieddate = ModifiedDate;

                preturn = objNewsBL.ArticlesDeleted(pNewsInfo);
                return Json(new { status = preturn });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return View();
            }
        }

        [HttpPost]
        [Route("luu-sua-bai-viet")]
        public ActionResult ExecuteNewsArticles(NewsInfo pNewsInfo )
        {
            try
            {
                decimal preturn = 0;
                if (pNewsInfo == null)
                {
                    return Json(new { status = -99 });
                }
                string language = AppsCommon.GetCurrentLang();
                var ModifiedBy = SessionData.CurrentUser.Username;
                var ModifiedDate = SessionData.CurrentUser.CurrentDate;
                if (pNewsInfo.pfileLogo != null)
                {
                    pNewsInfo.Imageheader = AppLoadHelpers.PushFileToServer(pNewsInfo.pfileLogo, AppUpload.Logo);
                }
                pNewsInfo.Languagecode = language;
                pNewsInfo.Modifiedby = ModifiedBy;
                pNewsInfo.Modifieddate = ModifiedDate;
                var objNewsBL = new NewsBL();
                preturn = objNewsBL.ArticlesUpdate(pNewsInfo);

                return Json(new { status = preturn });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return View();
            }
        }
    }
}
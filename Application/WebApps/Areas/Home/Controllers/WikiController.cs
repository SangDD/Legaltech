using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BussinessFacade;
using ObjectInfos;
using WebApps.AppStart;

namespace WebApps.Areas.Home.Controllers
{
    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("HomeAreaRegistration", AreaPrefix = "wiki-view")]
    [Route("{action}")]
    public class WikiController : Controller
    {
        // GET: Home/Wiki

        [HttpGet]
        [Route("doc-list/{id}/{id1}")]
        public ActionResult List()
        {
            try
            {
                WikiCatalogue_BL _CatalogueBL = new WikiCatalogue_BL();
                WikiDoc_BL _WikiBL = new WikiDoc_BL();
                decimal _Cataid = 0, _Docid = 0;
                if(RouteData.Values["id"] != null)
                {
                    _Cataid = Convert.ToDecimal(RouteData.Values["id"]);
                }
                if (RouteData.Values["id1"] != null)
                {
                    _Docid = Convert.ToDecimal(RouteData.Values["id1"]);
                }
                List<WikiCatalogues_Info> _ListCata = new List<WikiCatalogues_Info>();
                _ListCata = _CatalogueBL.Portal_CataGetAll();
                List<WikiDoc_Info> _ListDocSearch = new List<WikiDoc_Info>();
                if(_Cataid > 0)
                {
                    // lấy ds tin theo danh mục
                    _ListDocSearch = _WikiBL.WikiDoc_GetBy_CataID(_Cataid);
                    ViewBag.ListDocSearch = _ListDocSearch;
                }
                WikiDoc_Info _DocInfo = new WikiDoc_Info();
                if(_Docid > 0)
                {
                    // lấy chi tiết tin
                    _DocInfo = _WikiBL.WikiDoc_GetById(_Docid);
                    ViewBag.DocdetailInfo = _DocInfo;
                }
                ViewBag.ListCatalogue = _ListCata;

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return View("/Areas/Home/Views/Wiki/List.cshtml");
        }
    }
}
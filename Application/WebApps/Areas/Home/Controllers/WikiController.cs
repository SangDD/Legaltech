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
                if (RouteData.Values["id"] != null)
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
                if (_Cataid > 0)
                {
                    // lấy ds tin theo danh mục
                    //  _ListDocSearch = _WikiBL.WikiDoc_GetBy_CataID(_Cataid);
                    //ViewBag.ListDocSearch = _ListDocSearch;
                  
                    _ListDocSearch = _WikiBL.PortalWikiDoc_Search("3|" + _Cataid.ToString() + "|ALL");
                    ViewBag.Paging = _WikiBL.GetPagingHtml();
                    ViewBag.ListDocSearch = _ListDocSearch;
                    WikiCatalogue_BL _Catabl = new WikiCatalogue_BL();
                    WikiCatalogues_Info _Catainfo = new WikiCatalogues_Info();
                    _Catainfo = _Catabl.WikiCatalogue_GetByID(_Cataid);
                    ViewBag.CatalogueInfo = _Catainfo;
                }
                else
                {
                    List<WikiDoc_Info> lstOjects = _WikiBL.PortalWikiDoc_Search("-1|-1|-1");
                    ViewBag.Paging = _WikiBL.GetPagingHtml();
                    ViewBag.ListDocSearch = _ListDocSearch;
                }
                WikiDoc_Info _DocInfo = new WikiDoc_Info();
                if (_Docid > 0)
                {
                    // lấy chi tiết tin
                    _DocInfo = _WikiBL.PortalWikiDoc_GetById(_Docid);
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

        [HttpPost]
        [Route("GetDocByCataid")]
        public ActionResult GetListdocByCataid(decimal p_id)
        {
            try
            {
                WikiCatalogue_BL _CatalogueBL = new WikiCatalogue_BL();
                WikiDoc_BL _WikiBL = new WikiDoc_BL();
                List<WikiDoc_Info> _ListDocSearch = new List<WikiDoc_Info>();

                // lấy ds tin theo danh mục
                _ListDocSearch = _WikiBL.WikiDoc_GetBy_CataID(p_id);
                ViewBag.ListDocSearch = _ListDocSearch;
                WikiCatalogue_BL _Catabl = new WikiCatalogue_BL();
                WikiCatalogues_Info _Catainfo = new WikiCatalogues_Info();
                _Catainfo = _Catabl.WikiCatalogue_GetByID(p_id);
                ViewBag.CatalogueInfo = _Catainfo;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return PartialView("/Areas/Home/Views/Wiki/_PartialListDocByCata.cshtml");
        }

        [HttpPost]
        [Route("WikiDocSearch")]
        public ActionResult WikiDocSearch(string keysSearch, string options)
        {
            var lstOjects = new List<WikiDoc_Info>();
            try
            {
                var _WikiDoc_BL = new WikiDoc_BL();
                lstOjects = _WikiDoc_BL.PortalWikiDoc_Search(keysSearch, options);
                ViewBag.Paging = _WikiDoc_BL.GetPagingHtml();
                if (keysSearch.Split('|').Length >2 && keysSearch.Split('|')[1] != "ALL" && keysSearch.Split('|')[1] != "")
                {
                    WikiCatalogue_BL _Catabl = new WikiCatalogue_BL();
                    WikiCatalogues_Info _Catainfo = new WikiCatalogues_Info();
                    _Catainfo = _Catabl.WikiCatalogue_GetByID(Convert.ToDecimal((keysSearch.Split('|')[1])));
                    ViewBag.CatalogueInfo = _Catainfo;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            ViewBag.ListDocSearch = lstOjects;
            return PartialView("/Areas/Home/Views/Wiki/_PartialListDocByCata.cshtml");
        }

        [HttpPost]
        [Route("GetDocDetail")]
        public ActionResult GetListdocDetail(decimal p_id)
        {
            try
            {
                WikiDoc_BL _WikiBL = new WikiDoc_BL();
                WikiDoc_Info _DocInfo = new WikiDoc_Info();
                    // lấy chi tiết tin
               _DocInfo = _WikiBL.PortalWikiDoc_GetById(p_id);
                ViewBag.DocdetailInfo = _DocInfo;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return PartialView("/Areas/Home/Views/Wiki/_PartialDocViewDetail.cshtml");
        }

    }
}
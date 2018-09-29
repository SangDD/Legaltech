using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApps.AppStart;
using WebApps.Session;
using ObjectInfos;
using BussinessFacade;
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
        public ActionResult ListByStatus()
        {
            if (SessionData.CurrentUser == null)
            {
                return Redirect("/dang-xuat");
            }
            List<SearchObject_Header_Info> lstOjects = new List<SearchObject_Header_Info>();
            try
            {
                int _Status = 0;
                if (RouteData.Values.ContainsKey("id"))
                {
                    _Status = Convert.ToInt32(RouteData.Values["id"]);
                    ViewBag.CurrStatus = _Status;
                }
                var _SearchObject_BL = new SearchObject_BL();
                lstOjects = _SearchObject_BL.SEARCH_OBJECT_SEARCH(_Status.ToString() + "|ALL|ALL|ALL|ALL");
                ViewBag.Paging = _SearchObject_BL.GetPagingHtml();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return View(@"~\Areas\Manager\Views\SearchManage\ListSearch.cshtml", lstOjects);
        }


        [HttpPost]
        [Route("findobject-search")]
        public ActionResult FindsOnjectSearch(string keysSearch, string options)
        {
            List<SearchObject_Header_Info> lstOjects = new List<SearchObject_Header_Info>();
            try
            {

                var _SearchObject_BL = new SearchObject_BL();
                lstOjects = _SearchObject_BL.SEARCH_OBJECT_SEARCH(keysSearch, options);
                ViewBag.Paging = _SearchObject_BL.GetPagingHtml();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }
            return PartialView(@"~\Areas\Manager\Views\SearchManage\_SearchData.cshtml", lstOjects);
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

        [HttpPost]
        [Route("them-dieu-kien")]
        public ActionResult AddNewDetail(decimal _idSearch)
        {
          
            try
            {

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return PartialView(@"~\Areas\Manager\Views\SearchManage\_PartialSearchCondition.cshtml", _idSearch.ToString() + "|0");
        }

        [HttpPost]
        [Route("SearchInsert")]
        public ActionResult SearchInsert(SearchObject_Header_Info p_searchHeaderInfo, List<SearchObject_Detail_Info> p_SearchObject_Detail_Info,
            SearchObject_Question_Info p_questionInfo
            )
        {
            decimal _rel = 0;
            try
            {
                SearchObject_BL _searchBL = new SearchObject_BL();
                p_searchHeaderInfo.CREATED_BY = SessionData.CurrentUser.Username;
                p_searchHeaderInfo.CREATED_DATE = DateTime.Now;
                p_searchHeaderInfo.REQUEST_DATE = DateTime.Now;
                _rel = _searchBL.SEARCH_HEADER_INSERT(p_searchHeaderInfo);
                if(_rel < 0)
                {
                    // lỗi thì xóa
                   // _searchBL.SEARCH_HEADER_DELETE()
                    return Json(new { success = _rel });
                }
                p_searchHeaderInfo.SEARCH_ID = _rel;
                p_questionInfo.SEARCH_ID = p_searchHeaderInfo.SEARCH_ID;
                _rel = _searchBL.SEARCH_QUESTION_INSERT(p_questionInfo);
                if (_rel < 0)
                {
                    // lỗi thì xóa
                    _searchBL.SEARCH_HEADER_DELETE(p_searchHeaderInfo.SEARCH_ID);
                    return Json(new { success = _rel });
                }
                foreach (SearchObject_Detail_Info item in p_SearchObject_Detail_Info)
                {
                    item.SEARCH_ID = p_searchHeaderInfo.SEARCH_ID;
                }
                _rel = _searchBL.SEARCH_DETAIL_INSERT(p_SearchObject_Detail_Info);
                if (_rel < 0)
                {
                    // lỗi thì xóa
                    _searchBL.SEARCH_HEADER_DELETE(p_searchHeaderInfo.SEARCH_ID);
                    return Json(new { success = _rel });
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return Json(new { success = _rel  });
        }
    }
}
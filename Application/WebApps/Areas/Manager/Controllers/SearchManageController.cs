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
using WebApps.CommonFunction;
using ObjectInfos.ModuleTrademark;

namespace WebApps.Areas.Manager.Controllers
{
    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("Manager", AreaPrefix = "quan-ly-search")]
    [Route("{action}")]
    public class SearchManageController : Controller
    {
        // GET: Manager/SearchManage

        [HttpPost]
        [Route("push-file-to-server")]
        public ActionResult PushFileToServer(AppDocumentInfo pInfo)
        {
            try
            {
                if (pInfo.pfiles != null)
                {
                    var url = AppLoadHelpers.PushFileToServer(pInfo.pfiles, AppUpload.Search);
                    SessionData.CurrentUser.chashFile[pInfo.keyFileUpload] = pInfo.pfiles;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = -1 });
            }
            return Json(new { success = 0 });
        }

        [HttpGet]
        [Route("danh-sach-search")]
        public ActionResult ListByStatus()
        {
            if (SessionData.CurrentUser == null)
            {
                return Redirect("/dang-xuat");
            }
            List<SearchObject_Header_Info> lstOjects = new List<SearchObject_Header_Info>();
            try
            {

                var _SearchObject_BL = new SearchObject_BL();
                lstOjects = _SearchObject_BL.SEARCH_OBJECT_SEARCH("||||");
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

            ViewBag.Data_Object = BussinessFacade.ModuleMemoryData.MemoryData.clstAppClassSuggest;
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
            SearchObject_Question_Info p_questionInfo)
        {
            decimal _rel = 0;
            try
            {
                SearchObject_BL _searchBL = new SearchObject_BL();
                p_searchHeaderInfo.CREATED_BY = SessionData.CurrentUser.Username;
                p_searchHeaderInfo.CREATED_DATE = DateTime.Now;
                p_searchHeaderInfo.REQUEST_DATE = DateTime.Now;
                p_searchHeaderInfo.LANGUAGE_CODE = AppsCommon.GetCurrentLang();
                _rel = _searchBL.SEARCH_HEADER_INSERT(p_searchHeaderInfo);
                if (_rel < 0)
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
            return Json(new { success = _rel });
        }


        [HttpGet]
        [Route("search-edit/{id}/{id2}")]
        public ActionResult SearchEdit()
        {
            if (SessionData.CurrentUser == null)
            {
                return Redirect("/dang-xuat");
            }
            try
            {
                SearchObject_BL _searchBL = new SearchObject_BL();
                SearchObject_Header_Info _HeaderInfo = new SearchObject_Header_Info();
                List<SearchObject_Detail_Info> _ListDetail = new List<SearchObject_Detail_Info>();
                SearchObject_Question_Info _QuestionInfo = new SearchObject_Question_Info();
                decimal _Searchid = 0;
                if (RouteData.Values.ContainsKey("id"))
                {
                    _Searchid = Convert.ToDecimal(RouteData.Values["id"]);
                    _HeaderInfo = _searchBL.SEARCH_HEADER_GETBYID(_Searchid, ref _ListDetail, ref _QuestionInfo);
                    ViewBag.SearchHeader = _HeaderInfo;
                    ViewBag.SearchListDetail = _ListDetail;
                    ViewBag.QuestionInfo = _QuestionInfo;
                }
                int _Status = 0;
                if (RouteData.Values.ContainsKey("id2"))
                {
                    _Status = Convert.ToInt32(RouteData.Values["id2"]);
                    ViewBag.CurrStatus = _Status;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return View(@"~\Areas\Manager\Views\SearchManage\SearchEdit.cshtml");
        }

        [HttpPost]
        [Route("SearchEdit")]
        public ActionResult SearchEdit(SearchObject_Header_Info p_searchHeaderInfo, List<SearchObject_Detail_Info> p_SearchObject_Detail_Info,
          SearchObject_Question_Info p_questionInfo
          )
        {
            decimal _rel = 0;
            try
            {
                SearchObject_BL _searchBL = new SearchObject_BL();
                p_searchHeaderInfo.MODIFIED_BY = SessionData.CurrentUser.Username;
                p_searchHeaderInfo.MODIFIED_DATE = DateTime.Now;
                p_searchHeaderInfo.REQUEST_DATE = DateTime.Now;
                _rel = _searchBL.SEARCH_HEADER_UPDATE(p_searchHeaderInfo);
                if (_rel < 0)
                {
                    return Json(new { success = _rel });
                }

                p_questionInfo.SEARCH_ID = p_searchHeaderInfo.SEARCH_ID;
                _rel = _searchBL.SEARCH_QUESTION_UPDATE(p_questionInfo);

                foreach (SearchObject_Detail_Info item in p_SearchObject_Detail_Info)
                {
                    item.SEARCH_ID = p_searchHeaderInfo.SEARCH_ID;
                }
                //xóa detail
                _searchBL.SEARCH_DETAIL_DELETE(p_searchHeaderInfo.SEARCH_ID);
                _rel = _searchBL.SEARCH_DETAIL_INSERT(p_SearchObject_Detail_Info);
                return Json(new { success = _rel });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return Json(new { success = _rel });
        }

        [HttpPost]
        [Route("SearchEdit4Lawer")]
        public ActionResult SearchEdit4Lawer(SearchObject_Header_Info p_searchHeaderInfo, List<SearchObject_Detail_Info> p_SearchObject_Detail_Info,
         SearchObject_Question_Info p_questionInfo
         )
        {
            decimal _rel = 0;
            try
            {
                SearchObject_BL _searchBL = new SearchObject_BL();
                p_searchHeaderInfo.MODIFIED_BY = SessionData.CurrentUser.Username;
                p_searchHeaderInfo.MODIFIED_DATE = DateTime.Now;
                p_searchHeaderInfo.REQUEST_DATE = DateTime.Now;
                _rel = _searchBL.SEARCH_HEADER_UPDATE(p_searchHeaderInfo);
                if (_rel < 0)
                {
                    return Json(new { success = _rel });
                }

                p_questionInfo.SEARCH_ID = p_searchHeaderInfo.SEARCH_ID;
                _rel = _searchBL.SEARCH_QUESTION_UPDATE(p_questionInfo);

                foreach (SearchObject_Detail_Info item in p_SearchObject_Detail_Info)
                {
                    item.SEARCH_ID = p_searchHeaderInfo.SEARCH_ID;
                }
                //xóa detail
                _searchBL.SEARCH_DETAIL_DELETE(p_searchHeaderInfo.SEARCH_ID);
                _rel = _searchBL.SEARCH_DETAIL_INSERT(p_SearchObject_Detail_Info);
                return Json(new { success = _rel });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return Json(new { success = _rel });
        }


        [HttpGet]
        [Route("search-todo-detail/{id}")]
        public ActionResult SearchShowTodo()
        {
            if (SessionData.CurrentUser == null)
            {
                return Redirect("/dang-xuat");
            }
            try
            {
                SearchObject_BL _searchBL = new SearchObject_BL();
                SearchObject_Header_Info _HeaderInfo = new SearchObject_Header_Info();
                List<SearchObject_Detail_Info> _ListDetail = new List<SearchObject_Detail_Info>();
                SearchObject_Question_Info _QuestionInfo = new SearchObject_Question_Info();
                string _casecode = "";
                if (RouteData.Values.ContainsKey("id"))
                {
                    _casecode = RouteData.Values["id"].ToString();
                    _HeaderInfo = _searchBL.SEARCH_HEADER_GETBY_CASECODE(_casecode, ref _ListDetail, ref _QuestionInfo);
                    ViewBag.SearchHeader = _HeaderInfo;
                    ViewBag.SearchListDetail = _ListDetail;
                    ViewBag.QuestionInfo = _QuestionInfo;

                    //  lấy dữ liệu lịch sử giao dịch
                    B_Todos_BL _B_Todos_BL = new B_Todos_BL();
                    List<B_Remind_Info> _ListRemind = new List<B_Remind_Info>();
                    List<B_Todos_Info> _Listtodo = _B_Todos_BL.NotifiGetByCasecode(_casecode, ref _ListRemind);
                    ViewBag.ListTodo = _Listtodo;
                    ViewBag.ListRemind = _ListRemind;
                    ViewBag.Currstatus = _HeaderInfo.STATUS;

                    // action là view hay sửa
                    //decimal _operator_type = Convert.ToDecimal(Common.CommonData.CommonEnums.Operator_Type.Update);
                    //if (RouteData.Values.ContainsKey("id1"))
                    //{
                    //    _operator_type = Convert.ToDecimal(RouteData.Values["id1"].ToString());
                    //}
                    //ViewBag.Operator_Type = _operator_type;
                }


            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return View(@"~\Areas\Manager\Views\SearchManage\ToDo4Lawer.cshtml");
        }

        [HttpPost]
        [Route("phan-loai-4lawer")]
        public ActionResult DoSearch4Lawer(App_Lawer_Info p_App_Lawer_Info)
        {
            try
            {
                p_App_Lawer_Info.Language_Code = AppsCommon.GetCurrentLang();
                p_App_Lawer_Info.Created_By = SessionData.CurrentUser.Username;
                p_App_Lawer_Info.Created_Date = DateTime.Now;
                SearchObject_BL _con = new SearchObject_BL();
                decimal _ck = _con.SEARCH_LAWER_INSERT(p_App_Lawer_Info);
                return Json(new { success = _ck });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = "-1" });
            }
        }

        [HttpPost]
        [Route("tra-loi-search")]
        public ActionResult DoSearchResult(SearchObject_Question_Info p_SearchObject_Header_Info,
            List<AppDocumentInfo> pAppDocumentInfo)
        {
            try
            {
                p_SearchObject_Header_Info.LANGUAGE_CODE = AppsCommon.GetCurrentLang();
                p_SearchObject_Header_Info.CREATED_BY = SessionData.CurrentUser.Username;
                p_SearchObject_Header_Info.CREATED_DATE = DateTime.Now;
                SearchObject_BL _con = new SearchObject_BL();

                if (pAppDocumentInfo != null)
                {
                    if (pAppDocumentInfo.Count > 0)
                    {

                        foreach (var info in pAppDocumentInfo)
                        {
                            if (SessionData.CurrentUser.chashFile.ContainsKey(info.keyFileUpload))
                            {
                                HttpPostedFileBase pfiles = (HttpPostedFileBase)SessionData.CurrentUser.chashFile[info.keyFileUpload];
                                info.Filename = pfiles.FileName;
                                info.Url_Hardcopy = "/Content/Archive/" + AppUpload.Search + "/" + pfiles.FileName;
                                if (info.keyFileUpload == "ADD_FILE_01")
                                {
                                    p_SearchObject_Header_Info.FILE_URL = info.Url_Hardcopy;
                                }
                                if (info.keyFileUpload == "ADD_FILE_02")
                                {
                                    p_SearchObject_Header_Info.FILE_URL02 = info.Url_Hardcopy;
                                }

                                // lấy xong thì xóa
                                SessionData.CurrentUser.chashFile.Remove(info.keyFileUpload);
                            }
                        }
                    }
                }

                decimal _ck = _con.SEARCH_RESULT_SEARCH(p_SearchObject_Header_Info);
                return Json(new { success = _ck });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = "-1" });
            }
        }

    }
}
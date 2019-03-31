namespace WebApps.Areas.Home.Controllers
{
    using System;
    using System.Web.Mvc;
    using AppStart;
    using Common;
    using Session;
    using WebApps.CommonFunction;
    using BussinessFacade.ModuleUsersAndRoles;
    using ObjectInfos;
    using BussinessFacade;
    using System.Collections.Generic;
    using Common.CommonData;
    using BussinessFacade.ModuleTrademark;
    using System.IO;


    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("home", AreaPrefix = "")]
    public class HomeController : Controller
    {
        // GET: Home/index


        // GET: Home/Home
        [HttpGet]
        [Route("home")]
        public ActionResult KnHome()
        {
            if (SessionData.CurrentUser == null)
            {
                return this.Redirect("/");
            }
            var userBL = new UserBL(SessionData.CurrentUser);
            string language = AppsCommon.GetCurrentLang();
            string sessionLanguage = SessionData.CurrentUser.Language;
            if (language != sessionLanguage)
            {
                SessionData.CurrentUser.Language = language;
                SessionData.CurrentUser.HtmlMenu = userBL.GetUserHtmlMenu(language);
            }
            ViewBag.LanguageCode = language;

            B_Todos_BL _obj_bl = new B_Todos_BL();
            B_TodoNotify_Info p_todonotify = new B_TodoNotify_Info();
            p_todonotify = _obj_bl.GET_NOTIFY(SessionData.CurrentUser.Username);
            ViewBag.NotifyInfo = p_todonotify;

            return View("~/Areas/Home/Views/Home/LegalHome.cshtml");
        }

        [HttpGet]
        [Route("filter-request-not-identity")]
        public ActionResult FilterRequestNotIdentity(string requestMethod, string isRequestTypeAjax, string urlRedirect, string returnUrl = "")
        {
            try
            {
                var fullPathResponse = urlRedirect;

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    fullPathResponse += "?returnUrl=" + returnUrl;
                }

                if (requestMethod.ToUpper() == "POST" || isRequestTypeAjax?.ToUpper() == "TRUE")
                {
                    if (urlRedirect == RouteConfig.KnAccessDenied)
                    {
                        return this.Json(new { redirectTo = RouteConfig.KnAccessDeniedShortern, dataInTab = true }, JsonRequestBehavior.AllowGet);
                    }

                    return this.Json(new { redirectTo = fullPathResponse }, JsonRequestBehavior.AllowGet);
                }

                if (requestMethod.ToUpper() == "GET")
                {
                    return Redirect(fullPathResponse);
                }
            }
            catch (Exception)
            {
                // Ignored
            }

            return Json(new { redirectTo = RouteConfig.KnHttpNotFound });
        }

        [HttpGet]
        [Route("about-us")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View("~/Areas/Home/Views/Home/About.cshtml");
        }

        [HttpGet]
        [Route("contact")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View("~/Areas/Home/Views/Home/Contact.cshtml");
        }

        [HttpGet]
        [Route("page-not-found")]
        public ActionResult PageNotFound()
        {
            return View("~/Areas/Home/Views/Home/PageNotFound.cshtml");
        }

        [HttpGet]
        [Route("access-denied")]
        public ActionResult AccessDenied()
        {
            return View("~/Areas/Home/Views/Home/AccessDenied.cshtml");
        }

        [HttpGet]
        [Route("re-login")]
        public ActionResult ForceRelogin()
        {
            SessionData.CurrentUser = null;
            Session.Abandon();
            return View("~/Areas/Home/Views/Home/ForceRelogin.cshtml");
        }

        [HttpGet]
        [Route("account-session-invalid")]
        public ActionResult AccountSessionInvalid()
        {
            SessionData.CurrentUser = null;
            Session.Abandon();
            return View("~/Areas/Home/Views/Home/AccountSessionInvalid.cshtml");
        }

        [HttpGet]
        [Route("Language")]
        public ActionResult Language(string culture, string returnUrl)
        {
            try
            {
                if (!string.IsNullOrEmpty(culture))
                {
                    var httpCookie = Request.Cookies["language"];
                    if (httpCookie != null)
                    {
                        var cookie = Response.Cookies["language"];
                        if (cookie != null)
                        {
                            cookie.Value = culture;
                            Response.SetCookie(cookie);
                        }
                        httpCookie.Value = culture;
                    }

                    if (culture.ToUpper() != "EN-GB" && Request.UrlReferrer.ToString().ToLower().Contains("/en-gb/"))
                    {
                        return Redirect(Request.UrlReferrer.ToString().ToLower().Replace("/en-gb/", "/"));
                    }
                    else if (culture.ToUpper() != "VI-VN" && Request.UrlReferrer.ToString().ToLower().Contains("/vi-vn/"))
                    {
                        return Redirect(Request.UrlReferrer.ToString().ToLower().Replace("/vi-vn/", "/"));
                    }
                }

                return Redirect(Request.UrlReferrer.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Redirect("/home.html");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        [Route("CheckSessionTimeOut")]
        public JsonResult CheckSessionTimeOut()
        {
            try
            {
                var msg = new MsgReportServerInfo();
                if (SessionData.CurrentUser == null)
                {
                    msg.Code = "-1";
                    msg.Msg = "Hệ thống đã hết thời gian kết nối, bạn hãy đăng nhập lại.";
                }
                else
                {
                    msg.Code = "0";
                }
                return Json(msg);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                var msg = new MsgReportServerInfo();
                msg.Code = "-1";
                msg.Msg = "Không kết nối được tới máy chủ.";
                return Json(msg);
            }
        }

        #region box search 


        [HttpPost]
        [Route("search-dashboard")]
        public ActionResult FindOject(int searchtype, string keysSearch, string options)
        {

            int p_CurrentPage = 1;
            int _reconpage = 5;
            p_CurrentPage = Convert.ToInt32(options.Split('|')[3]);
            _reconpage = Convert.ToInt32(options.Split('|')[4]);
            decimal _total_record = 0;
            string p_to = "";
            string p_from = CommonFuc.Get_From_To_Page(p_CurrentPage, ref p_to, _reconpage);
            string _sortype = "ALL";
            _sortype = " ORDER BY " + options.Split('|')[0] + " " + options.Split('|')[1];
            if (string.IsNullOrEmpty(_sortype) || _sortype.Trim() == "ORDER BY")
            {
                _sortype = "ALL";
            }
            string htmlPaging = "";
            try
            {
                ViewBag.SearchType = searchtype.ToString();
                if (searchtype == 1)
                {
                    // đơn
                    Application_Header_BL _obj_bl = new Application_Header_BL();
                    List<ApplicationHeaderInfo> _lst = _obj_bl.ApplicationHeader_Search(SessionData.CurrentUser.Username, keysSearch, ref _total_record, p_from, p_to, _sortype, 1);
                    htmlPaging = CommonFuc.Get_HtmlPaging<ApplicationHeaderInfo>((int)_total_record, p_CurrentPage, "Đơn", _reconpage);

                    ViewBag.Paging = htmlPaging;
                    ViewBag.Obj = _lst;
                    ViewBag.SumRecord = _total_record;
                    return PartialView("~/Areas/Home/Views/Shared/_SearchDataAppStatus.cshtml");
                }
                if (searchtype == 2)
                {
                    // luật sư
                    var userBL = new UserBL();
                    var lstUsers = new List<UserInfo>();
                    keysSearch = "|" + keysSearch + "|" + Convert.ToInt16(CommonEnums.UserType.Lawer) + "|";
                    lstUsers = userBL.HomeFindUser(ref _total_record, keysSearch, options);
                    htmlPaging = CommonFuc.Get_HtmlPaging<UserInfo>((int)_total_record, p_CurrentPage, "Luật sư", _reconpage);
                    ViewBag.Paging = htmlPaging;
                    ViewBag.SumRecord = _total_record;

                    return PartialView("~/Areas/Home/Views/Shared/_SearchDataLawyer.cshtml", lstUsers);
                }
                if (searchtype == 3)
                {
                    // khách hàng
                    var userBL = new UserBL();
                    var lstUsers = new List<UserInfo>();
                    keysSearch = "|" + keysSearch + "|" + Convert.ToInt16(CommonEnums.UserType.Customer) + "|";
                    lstUsers = userBL.HomeFindUser(ref _total_record, keysSearch, options);
                    htmlPaging = CommonFuc.Get_HtmlPaging<UserInfo>((int)_total_record, p_CurrentPage, "Khách hàng", _reconpage);
                    ViewBag.Paging = htmlPaging;
                    ViewBag.SumRecord = _total_record;

                    return PartialView("~/Areas/Home/Views/Shared/_SearchDataCustomer.cshtml", lstUsers);
                }
                if (searchtype == 4)
                {
                    //wiki
                    var lstOjects = new List<WikiDoc_Info>();
                    keysSearch = CommonWiki.Stt_daduyet.ToString() + "||" + keysSearch;
                    var _WikiDoc_BL = new WikiDoc_BL();
                    lstOjects = _WikiDoc_BL.WikiDoc_DashboardSearch(ref _total_record, keysSearch, options);
                    htmlPaging = CommonFuc.Get_HtmlPaging<WikiDoc_Info>((int)_total_record, p_CurrentPage, "Bài viết", _reconpage);
                    ViewBag.Paging = htmlPaging;
                    ViewBag.SumRecord = _total_record;

                    return PartialView("~/Areas/Home/Views/Shared/_SearchDataWiki.cshtml", lstOjects);
                }

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return null;
        }

        #endregion

        #region Box Todo
        [HttpPost]
        [Route("search-todos")]
        public ActionResult Findtodos(string keysSearch, string _sortype, int _reconpage, int p_CurrentPage)
        {
            decimal _total_record = 0;
            string p_to = "";
            string p_from = CommonFuc.Get_From_To_Page(p_CurrentPage, ref p_to, _reconpage);
            _sortype = " ORDER BY " + _sortype;
            if (string.IsNullOrEmpty(_sortype) || _sortype.Trim() == "ORDER BY")
            {
                _sortype = "ALL";
            }
            string htmlPaging = "";
            try
            {
                B_Todos_BL _obj_bl = new B_Todos_BL();
                keysSearch = B_Todo.TypeProcess + "|" + SessionData.CurrentUser.Username;
                List<B_Todos_Info> _lst = _obj_bl.B_Todos_Search(keysSearch, ref _total_record, p_from, p_to, _sortype);
                htmlPaging = CommonFuc.Get_HtmlPaging<B_Todos_Info>((int)_total_record, p_CurrentPage, "Nội dung", _reconpage, "TodojsPaging");
                ViewBag.Paging = htmlPaging;
                ViewBag.Obj = _lst;
                ViewBag.SumRecord = _total_record;
                //return Json(new { TodoData = RenderPartialToString("~/Areas/Home/Views/Shared/_TodoData.cshtml", null), TodoNotify = p_todonotify });

                var _TodoData = AppsCommon.RenderRazorViewToString(this.ControllerContext, "~/Areas/Home/Views/Shared/_TodoData.cshtml");
                return Json(new { TodoData = _TodoData });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return null;
        }

        [HttpPost]
        [Route("do-remind")]
        public ActionResult do_Remind(decimal p_type, string p_case_code, decimal p_ref_id)
        {
            try
            {
                B_Todos_BL _obj_bl = new B_Todos_BL();
                var modifiedBy = SessionData.CurrentUser.Username;
                bool _result = _obj_bl.Remind_Insert_ByTodo(p_type, p_case_code, p_ref_id, SessionData.CurrentUser.Username, AppsCommon.GetCurrentLang());
                if (_result)
                {
                    return Json(new { success = 1 });
                }
                else
                    return Json(new { success = -1 });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = -1 });
            }
        }

        #endregion

        #region Box order
        [HttpPost]
        [Route("search-orders")]
        public ActionResult Findorders(string keysSearch, string _sortype, int _reconpage, int p_CurrentPage)
        {
            decimal _total_record = 0;
            string p_to = "";
            string p_from = CommonFuc.Get_From_To_Page(p_CurrentPage, ref p_to, _reconpage);
            _sortype = " ORDER BY " + _sortype;
            if (string.IsNullOrEmpty(_sortype) || _sortype.Trim() == "ORDER BY")
            {
                _sortype = "ALL";
            }
            string htmlPaging = "";
            try
            {
                B_Todos_BL _obj_bl = new B_Todos_BL();
                keysSearch = B_Todo.TypeRequest + "|" + SessionData.CurrentUser.Username;
                List<B_Todos_Info> _lst = _obj_bl.B_Todos_Search(keysSearch, ref _total_record, p_from, p_to, _sortype);
                htmlPaging = CommonFuc.Get_HtmlPaging<B_Todos_Info>((int)_total_record, p_CurrentPage, "Nội dung", _reconpage, "OrderjsPaging");
                ViewBag.Paging = htmlPaging;
                ViewBag.Obj = _lst;
                ViewBag.SumRecord = _total_record;

                var _TodoData = AppsCommon.RenderRazorViewToString(this.ControllerContext, "~/Areas/Home/Views/Shared/_OrderData.cshtml");
                return Json(new { TodoData = _TodoData });

                //return Json(new { TodoData = RenderPartialToString("~/Areas/Home/Views/Shared/_OrderData.cshtml", null), TodoNotify = p_todonotify });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return null;
        }

        #endregion

        #region Box remind
        [HttpPost]
        [Route("search-remind")]
        public ActionResult FindReminds(string keysSearch, string _sortype, int _reconpage, int p_CurrentPage)
        {
            decimal _total_record = 0;
            string p_to = "";
            string p_from = CommonFuc.Get_From_To_Page(p_CurrentPage, ref p_to, _reconpage);
            _sortype = " ORDER BY " + _sortype;
            if (string.IsNullOrEmpty(_sortype) || _sortype.Trim() == "ORDER BY")
            {
                _sortype = "ALL";
            }
            string htmlPaging = "";
            try
            {
                B_Todos_BL _obj_bl = new B_Todos_BL();
                keysSearch = SessionData.CurrentUser.Username;
                List<B_Remind_Info> _lst = _obj_bl.B_Remind_Search(keysSearch, ref _total_record, p_from, p_to, _sortype);
                htmlPaging = CommonFuc.Get_HtmlPaging<B_Remind_Info>((int)_total_record, p_CurrentPage, "Nội dung", _reconpage, "RemindjsPaging");
                ViewBag.Paging = htmlPaging;
                ViewBag.Obj = _lst;
                ViewBag.SumRecord = _total_record;

                var RemindData = AppsCommon.RenderRazorViewToString(this.ControllerContext, "~/Areas/Home/Views/Shared/_RemindData.cshtml");
                return Json(new { RemindData, Total = _total_record });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return null;
        }
        #endregion

        #region Box tin 
        [HttpPost]
        [Route("search-bulletin")]
        public ActionResult Findbulletin(string keysSearch, string _sortype, int _reconpage, int p_CurrentPage)
        {
            decimal _total_record = 0;
            string p_to = "";
            string p_from = CommonFuc.Get_From_To_Page(p_CurrentPage, ref p_to, _reconpage);
            _sortype = " ORDER BY " + _sortype;
            if (string.IsNullOrEmpty(_sortype) || _sortype.Trim() == "ORDER BY")
            {
                _sortype = "ALL";
            }
            string htmlPaging = "";
            try
            {
                NewsBL _obj_bl = new NewsBL();
                keysSearch = "7|" + "ALL|" + SessionData.CurrentUser.Language + "|THONGBAO";
                //string pLanguage, string pTitle, DateTime pNgayCongBo, int pStart, int pEnd, ref decimal pTotalRecord)

                List<NewsInfo> _lst = _obj_bl.ArticleHomeSearch(keysSearch, ref _total_record, p_from, p_to, _sortype);
                htmlPaging = CommonFuc.Get_HtmlPaging<B_Remind_Info>((int)_total_record, p_CurrentPage, "Tin", _reconpage, "BulletinjsPaging");
                ViewBag.Paging = htmlPaging;
                ViewBag.Obj = _lst;
                ViewBag.SumRecord = _total_record;
                return PartialView("~/Areas/Home/Views/Shared/_BulletinData.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return null;
        }

        [HttpPost]
        [Route("search-HotNews")]
        public ActionResult FindHotNews(string keysSearch, string _sortype, int _reconpage, int p_CurrentPage)
        {
            decimal _total_record = 0;
            string p_to = "";
            string p_from = CommonFuc.Get_From_To_Page(p_CurrentPage, ref p_to, _reconpage);
            _sortype = " ORDER BY " + _sortype;
            if (string.IsNullOrEmpty(_sortype) || _sortype.Trim() == "ORDER BY")
            {
                _sortype = "ALL";
            }
            string htmlPaging = "";
            try
            {
                NewsBL _obj_bl = new NewsBL();
                // keysSearch = "7|" + "1|" + SessionData.CurrentUser.Language;
                keysSearch = "7|" + "ALL|" + SessionData.CurrentUser.Language + "|ALL";

                List<NewsInfo> _lst = _obj_bl.ArticleHomeSearch(keysSearch, ref _total_record, p_from, p_to, _sortype);
                htmlPaging = CommonFuc.Get_HtmlPaging<B_Remind_Info>((int)_total_record, p_CurrentPage, "Tin", _reconpage, "HotnewsjsPaging");
                ViewBag.Paging = htmlPaging;
                ViewBag.Obj = _lst;
                ViewBag.SumRecord = _total_record;
                return PartialView("~/Areas/Home/Views/Shared/_HotnewsData.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return null;
        }


        [HttpPost]
        [Route("search-Updatenews")]
        public ActionResult FindUpdateNews(string keysSearch, string _sortype, int _reconpage, int p_CurrentPage)
        {
            decimal _total_record = 0;
            string p_to = "";
            string p_from = CommonFuc.Get_From_To_Page(p_CurrentPage, ref p_to, _reconpage);
            _sortype = " ORDER BY " + _sortype;
            if (string.IsNullOrEmpty(_sortype) || _sortype.Trim() == "ORDER BY")
            {
                _sortype = "ALL";
            }
            string htmlPaging = "";
            try
            {
                NewsBL _obj_bl = new NewsBL();
                // keysSearch = "7|" + "1|" + SessionData.CurrentUser.Language;
                keysSearch = "7|" + "ALL|" + SessionData.CurrentUser.Language + "|TIN_TUC";

                List<NewsInfo> _lst = _obj_bl.ArticleHomeSearch(keysSearch, ref _total_record, p_from, p_to, _sortype);
                htmlPaging = CommonFuc.Get_HtmlPaging<B_Remind_Info>((int)_total_record, p_CurrentPage, "Tin", _reconpage, "UpdatenewsjsPaging");
                ViewBag.Paging = htmlPaging;
                ViewBag.Obj = _lst;
                ViewBag.SumRecord = _total_record;
                return PartialView("~/Areas/Home/Views/Shared/_UpdatenewsData.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return null;
        }

        [HttpPost]
        [Route("search-Wikihome")]
        public ActionResult WikihomefindObjects(string keysSearch, string _sortype, int _reconpage, int p_CurrentPage)
        {
            decimal _total_record = 0;
            string p_to = "";
            string p_from = CommonFuc.Get_From_To_Page(p_CurrentPage, ref p_to, _reconpage);
            _sortype = " ORDER BY " + _sortype;
            if (string.IsNullOrEmpty(_sortype) || _sortype.Trim() == "ORDER BY")
            {
                _sortype = "ALL";
            }
            string htmlPaging = "";
            try
            {
                var _WikiDoc_BL = new WikiDoc_BL();
                keysSearch = "3|" + "ALL|ALL|" + SessionData.CurrentUser.Language;
                List<WikiDoc_Info> _lst = _WikiDoc_BL.HomeWikiDoc_Search(keysSearch, ref _total_record, p_from, p_to, _sortype);
                ViewBag.Paging = _WikiDoc_BL.GetPagingHtml();
                htmlPaging = CommonFuc.Get_HtmlPaging<WikiDoc_Info>((int)_total_record, p_CurrentPage, "Bài viết", _reconpage, "WikihomejsPaging");
                ViewBag.Paging = htmlPaging;
                ViewBag.Obj = _lst;
                ViewBag.SumRecord = _total_record;
                return PartialView("~/Areas/Home/Views/Shared/_WikiHomeData.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return null;
        }

        #endregion
    }

    public class MsgReportServerInfo
    {
        public string Code { get; set; }
        public string Msg { get; set; }
    }
}
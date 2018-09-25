using System;
using System.Web.Mvc;
using Common;
using WebApps.CommonFunction;
using BussinessFacade.ModuleUsersAndRoles;
using ObjectInfos;
using BussinessFacade;
using System.Collections.Generic;
using Common.CommonData;
using BussinessFacade.ModuleTrademark;
using System.IO;
using WebApps.Session;
using WebApps.AppStart;

namespace WebApps.Areas.Home.Controllers
{
    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("home", AreaPrefix = "search")]
    [Route("{action}")]
    public class SearchController : Controller
    {
        [HttpGet]
        [Route("search-dashboard")]
        public ActionResult HomeSearch(string searchtype, string searchContent, string options)
        {
            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("/");
                ViewBag.Searchtype = searchtype;
                ViewBag.SearchContent = searchContent;
                ViewBag.Options = options;

                return View("~/Areas/Home/Views/Search/HomeSearch.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return View();
            }
        }

        [HttpPost]
        [Route("search-dashboard/search")]
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
                    List<ApplicationHeaderInfo> _lst = _obj_bl.ApplicationHeader_Search(keysSearch, ref _total_record, p_from, p_to, _sortype, 1);
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
    }
}
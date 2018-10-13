using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApps.AppStart;
using WebApps.Session;
using System.Linq;

using ObjectInfos;
using BussinessFacade;
using Common;
using WebApps.CommonFunction;
using Common.CommonData;
using BussinessFacade.ModuleTrademark;
using System.Transactions;
using GemBox.Document;
using BussinessFacade.ModuleUsersAndRoles;
using System.IO;
using System.Data;

namespace WebApps.Areas.Manager.Controllers
{
    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("Manager", AreaPrefix = "quan-ly-notify")]
    [Route("{action}")]
    public class NotifyController : Controller
    {
        [HttpGet]
        [Route("danh-sach-notify/{id}")]
        public ActionResult Notify_Display()
        {
            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("/");

                string _type = B_Todo.TypeProcess;
                if (RouteData.Values["id"] != null && RouteData.Values["id"].ToString() != "")
                {
                    _type = RouteData.Values["id"].ToString();
                }

                decimal _total_record = 0;
                B_Todos_BL _obj_bl = new B_Todos_BL();
                string keysSearch = _type + "|" + SessionData.CurrentUser.Username;
                List<B_Todos_Info> _lst = _obj_bl.B_Todos_Search(keysSearch, ref _total_record);
                string htmlPaging = CommonFuc.Get_HtmlPaging<B_Todos_Info>((int)_total_record, 1, "Nội dung", 10, "TodojsPaging");

                ViewBag.Obj = _lst;
                ViewBag.Paging = htmlPaging;
                ViewBag.SumRecord = _total_record;
                ViewBag.Type = _type;

                return View("~/Areas/Manager/Views/Notify/Notify_Display.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return View();
            }
        }

        [HttpPost]
        [Route("danh-sach-notify/search")]
        public ActionResult Notify_Billing(string p_keysearch, int p_CurrentPage, string p_column, string p_type_sort)
        {
            try
            {
                decimal _total_record = 0;
                string p_to = "";
                string p_from = CommonFuc.Get_From_To_Page(p_CurrentPage, ref p_to);
                B_Todos_BL _obj_bl = new B_Todos_BL();
                List<B_Todos_Info> _lst = _obj_bl.Notify_Search(p_keysearch, ref _total_record, p_from, p_to);
                string htmlPaging = CommonFuc.Get_HtmlPaging<B_Todos_Info>((int)_total_record, 1, "Nội dung");

                ViewBag.Paging = htmlPaging;
                ViewBag.Obj = _lst;
                ViewBag.SumRecord = _total_record;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            string[] _arr = p_keysearch.Split('|');
            if (_arr.Length > 0)
            {
                if (_arr[0] == "TODO")
                {
                    return PartialView("~/Areas/Home/Views/Shared/_TodoData.cshtml");
                }
                else if (_arr[0] == "ORDER")
                {
                    return PartialView("~/Areas/Home/Views/Shared/_OrderData.cshtml");
                }
                else if (_arr[0] == "REMIND")
                {
                    return PartialView("~/Areas/Home/Views/Shared/_RemindData.cshtml");
                }
            }
            return PartialView("~/Areas/Home/Views/Shared/_TodoData.cshtml");
        }

        [HttpPost]
        [Route("danh-sach-notify/search-todos")]
        public ActionResult Find(string keysSearch, string _sortype, int _reconpage, int p_CurrentPage)
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

                var _TodoData = AppsCommon.RenderRazorViewToString(this.ControllerContext, "~/Areas/Home/Views/Shared/_TodoData.cshtml");
                return Json(new { TodoData = _TodoData });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return null;
        }
    }
}
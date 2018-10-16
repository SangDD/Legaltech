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
    [RouteArea("Manager", AreaPrefix = "quan-ly-phi-tim-kiem")]
    [Route("{action}")]
    public class Search_FeeController : Controller
    {
        [HttpGet]
        [Route("danh-sach-phi-tim-kiem")]
        public ActionResult Search_Fee_Display()
        {
            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("/");

                decimal _total_record = 0;
                Sys_Search_Fix_BL _obj_bl = new Sys_Search_Fix_BL();
                string _keySearch = "ALL" + "|" + "ALL" + "|" + "ALL";
                List<Sys_Search_Fix_Info> _lst = _obj_bl.Sys_Search_Fix_Search(_keySearch, ref _total_record);
                string htmlPaging = CommonFuc.Get_HtmlPaging<Sys_Search_Fix_Info>((int)_total_record, 1, "phí");

                ViewBag.Obj = _lst;
                ViewBag.Paging = htmlPaging;
                ViewBag.SumRecord = _total_record;

                return View("~/Areas/Manager/Views/Search_Fee/Search_Fee_Display.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return View();
            }
        }

        [HttpPost]
        [Route("search")]
        public ActionResult Fee_Billing(string p_keysearch, int p_CurrentPage, string p_column, string p_type_sort)
        {
            try
            {
                decimal _total_record = 0;
                string p_to = "";
                string p_from = CommonFuc.Get_From_To_Page(p_CurrentPage, ref p_to);
                Sys_Search_Fix_BL _obj_bl = new Sys_Search_Fix_BL();
                List<Sys_Search_Fix_Info> _lst = _obj_bl.Sys_Search_Fix_Search(p_keysearch, ref _total_record, p_from, p_to);
                string htmlPaging = CommonFuc.Get_HtmlPaging<Sys_Search_Fix_Info>((int)_total_record, 1, "phí");

                ViewBag.Paging = htmlPaging;
                ViewBag.Obj = _lst;
                ViewBag.SumRecord = _total_record;
                return PartialView("~/Areas/Manager/Views/Search_Fee/_PartialTableSearch_Fee.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/Manager/Views/Search_Fee/_PartialTableSearch_Fee.cshtml");
            }
        }

        [Route("show-view")]
        public ActionResult GetView2View(decimal id)
        {
            try
            {
                Sys_Search_Fix_BL _obj_bl = new Sys_Search_Fix_BL();
                Sys_Search_Fix_Info _Sys_Search_Fix_Info = _obj_bl.Sys_Search_Fix_GetById(id);
                return PartialView("~/Areas/Manager/Views/Search_Fee/_PartialView.cshtml", _Sys_Search_Fix_Info);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/Manager/Views/Search_Fee/_PartialView.cshtml", new Sys_Search_Fix_Info());
            }
        }


        [Route("show-edit")]
        public ActionResult GetView2Edit(decimal id)
        {
            try
            {
                Sys_Search_Fix_BL _obj_bl = new Sys_Search_Fix_BL();
                Sys_Search_Fix_Info _Sys_Search_Fix_Info = _obj_bl.Sys_Search_Fix_GetById(id);
                return PartialView("~/Areas/Manager/Views/Search_Fee/_PartialEdit.cshtml", _Sys_Search_Fix_Info);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/Manager/Views/Search_Fee/_PartialEdit.cshtml", new Sys_Search_Fix_Info());
            }
        }

        [HttpGet]
        [Route("show-insert")]
        public ActionResult GetView2Insert()
        {
            return View("~/Areas/Manager/Views/Search_Fee/_PartialInsert.cshtml", new Sys_Search_Fix_Info());
        }

        [HttpPost]
        [Route("do-insert")]
        public ActionResult DoInsert(Sys_Search_Fix_Info p_obj)
        {
            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("/");

                Sys_Search_Fix_BL _obj_bl = new Sys_Search_Fix_BL();
                p_obj.Created_By = SessionData.CurrentUser.Username;
                p_obj.Created_Date = DateTime.Now;
                decimal _ck = _obj_bl.Sys_Search_Fee_Insert(p_obj);
                BussinessFacade.ModuleMemoryData.MemoryData.LoadSys_Search_Fee_Fix();
                return Json(new { success = _ck });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = "-1" });
            }
        }

        [HttpPost]
        [Route("do-edit")]
        public ActionResult DoEdit(Sys_Search_Fix_Info p_obj)
        {
            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("/");

                Sys_Search_Fix_BL _obj_bl = new Sys_Search_Fix_BL();
                p_obj.Modified_By = SessionData.CurrentUser.Username;
                decimal _ck = _obj_bl.Sys_Search_Fee_Update(p_obj);
                BussinessFacade.ModuleMemoryData.MemoryData.LoadSys_Search_Fee_Fix();
                return Json(new { success = _ck });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = "-1" });
            }
        }

        [HttpPost]
        [Route("do-delete-fee")]
        public ActionResult DoDelete(decimal p_id)
        {
            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("/");

                Sys_Search_Fix_BL _obj_bl = new Sys_Search_Fix_BL();
                decimal _ck = _obj_bl.Sys_Search_Fee_Delete(p_id, SessionData.CurrentUser.Username);
                BussinessFacade.ModuleMemoryData.MemoryData.LoadSys_Search_Fee_Fix();
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
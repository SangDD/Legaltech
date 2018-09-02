using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApps.AppStart;
using WebApps.Session;

using ObjectInfos;
using BussinessFacade;
using Common;
using WebApps.CommonFunction;

namespace WebApps.Areas.DockingManager.Controllers
{
    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("Docking", AreaPrefix = "quan-ly-docking")]
    [Route("{action}")]
    public class DockingController : Controller
    {

        [HttpGet]
        [Route("danh-sach-docking")]
        public ActionResult Docking_Display()
        {
            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("~/home/index");

                decimal _total_record = 0;
                Docking_BL _obj_bl = new Docking_BL();
                string _keySearch = "ALL" + "|" + "ALL" + "|" + "ALL" + "|" + "ALL" + "|" + "ALL";
                List<Docking_Info> _lst = _obj_bl.Docking_Search(_keySearch, ref _total_record);
                string htmlPaging = CommonFuc.Get_HtmlPaging<ApplicationHeaderInfo>((int)_total_record, 1, "Tài liệu");

                ViewBag.Obj = _lst;
                ViewBag.Paging = htmlPaging;
                ViewBag.SumRecord = _total_record;

                return View("~/Areas/DockingManager/Views/Docking/Docking_Display.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return View();
            }
        }

        [HttpPost]
        [Route("danh-sach-docking/search")]
        public ActionResult Search_Docking(string p_keysearch, int p_CurrentPage, string p_column, string p_type_sort)
        {
            try
            {
                decimal _total_record = 0;
                string p_to = "";
                string p_from = CommonFuc.Get_From_To_Page(p_CurrentPage, ref p_to);
                Docking_BL _obj_bl = new Docking_BL();
                List<Docking_Info> _lst = _obj_bl.Docking_Search(p_keysearch, ref _total_record, p_from, p_to);
                string htmlPaging = CommonFuc.Get_HtmlPaging<Docking_Info>((int)_total_record, 1, "Tài liệu");

                ViewBag.Paging = htmlPaging;
                ViewBag.Obj = _lst;
                ViewBag.SumRecord = _total_record;
                return PartialView("~/Areas/DockingManager/Views/Docking/_PartialTableDocking.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/DockingManager/Views/Docking/_PartialTableDocking.cshtml");
            }
        }

        [HttpPost]
        [Route("danh-sach-docking/do-delete-docking")]
        public ActionResult DoDelete(int p_id)
        {
            try
            {
                Docking_BL _obj_bl = new Docking_BL();
                var modifiedBy = SessionData.CurrentUser.Username;
                decimal _result = _obj_bl.Docking_Update_Delete(p_id, AppsCommon.GetCurrentLang(), SessionData.CurrentUser.Username, DateTime.Now);
                return Json(new { success = _result });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = -1 });
            }
        }

        [HttpPost]
        [Route("danh-sach-docking/show-view")]
        public ActionResult GetView2View(decimal p_id)
        {
            try
            {
                Docking_BL _obj_bl = new Docking_BL();
                Docking_Info _Docking_Info = _obj_bl.Docking_GetBy_Id(p_id);
                return PartialView("~/Areas/DockingManager/Views/Docking/_PartialView.cshtml", _Docking_Info);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/DockingManager/Views/Docking/_PartialView.cshtml", new Docking_Info());
            }
        }

        // Insert 
        [HttpPost]
        [Route("danh-sach-docking/show-insert")]
        public ActionResult GetView2Insert()
        {
            return PartialView("~/Areas/DockingManager/Views/Docking/_PartialInsert.cshtml", new Docking_Info());
        }

        [HttpPost]
        [Route("danh-sach-docking/do-insert-docking")]
        public ActionResult DoInsert(Docking_Info p_Docking_Info)
        {
            try
            {
                Docking_BL _obj_bl = new Docking_BL();
                p_Docking_Info.Created_By = SessionData.CurrentUser.Username;
                p_Docking_Info.Created_Date = DateTime.Now;
                decimal _ck = _obj_bl.Docking_Insert(p_Docking_Info);
                return Json(new { success = _ck });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = "-1" });
            }
        }

        // edit
        [HttpPost]
        [Route("danh-sach-docking/show-edit")]
        public ActionResult GetView2Edit(int p_id)
        {
            try
            {
                Docking_BL _obj_bl = new Docking_BL();
                Docking_Info _Docking_Info = _obj_bl.Docking_GetBy_Id(p_id);
                return PartialView("~/Areas/DockingManager/Views/Docking/_PartialEdit.cshtml", _Docking_Info);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/DockingManager/Views/Docking/_PartialEdit.cshtml");
            }
        }

        [HttpPost]
        [Route("danh-sach-docking/do-edit-docking")]
        public ActionResult DoEdit(Docking_Info p_Docking_Info)
        {
            try
            {
                Docking_BL _obj_bl = new Docking_BL();
                p_Docking_Info.Modify_By = SessionData.CurrentUser.Username;
                p_Docking_Info.Modify_Date = DateTime.Now;

                decimal _ck = _obj_bl.Docking_Update(p_Docking_Info);
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
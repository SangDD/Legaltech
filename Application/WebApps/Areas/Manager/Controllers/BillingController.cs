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
using Common.CommonData;

namespace WebApps.Areas.Manager.Controllers
{
    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("Manager", AreaPrefix = "quan-ly-billing")]
    [Route("{action}")]
    public class BillingController : Controller
    {
        [HttpGet]
        [Route("danh-sach-billing")]
        public ActionResult Billing_Display()
        {
            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("~/home/index");

                decimal _total_record = 0;
                Billing_BL _obj_bl = new Billing_BL();
                string _keySearch = "ALL" + "|" + "ALL" + "|" + "ALL" + "|" + "ALL" + "|" + "ALL";
                List<Billing_Header_Info> _lst = _obj_bl.Billing_Search(_keySearch, ref _total_record);
                string htmlPaging = CommonFuc.Get_HtmlPaging<Billing_Header_Info>((int)_total_record, 1, "Billing");

                ViewBag.Obj = _lst;
                ViewBag.Paging = htmlPaging;
                ViewBag.SumRecord = _total_record;

                return View("~/Areas/Manager/Views/Billing/Billing_Display.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return View();
            }
        }

        [HttpPost]
        [Route("danh-sach-billing/search")]
        public ActionResult Search_Billing(string p_keysearch, int p_CurrentPage, string p_column, string p_type_sort)
        {
            try
            {
                decimal _total_record = 0;
                string p_to = "";
                string p_from = CommonFuc.Get_From_To_Page(p_CurrentPage, ref p_to);
                Billing_BL _obj_bl = new Billing_BL();
                List<Billing_Header_Info> _lst = _obj_bl.Billing_Search(p_keysearch, ref _total_record, p_from, p_to);
                string htmlPaging = CommonFuc.Get_HtmlPaging<Billing_Header_Info>((int)_total_record, 1, "Billing");

                ViewBag.Paging = htmlPaging;
                ViewBag.Obj = _lst;
                ViewBag.SumRecord = _total_record;
                return PartialView("~/Areas/Manager/Views/Billing/_PartialTableBilling.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/Manager/Views/Billing/_PartialTableBilling.cshtml");
            }
        }

        [HttpPost]
        [Route("danh-sach-billing/do-delete-billing")]
        public ActionResult DoDelete(int p_id)
        {
            try
            {
                Billing_BL _obj_bl = new Billing_BL();
                var modifiedBy = SessionData.CurrentUser.Username;
                decimal _result = _obj_bl.Billing_Update_Delete(p_id, AppsCommon.GetCurrentLang(), SessionData.CurrentUser.Username, DateTime.Now);
                return Json(new { success = _result });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = -1 });
            }
        }

        [HttpPost]
        [Route("danh-sach-billing/show-view")]
        public ActionResult GetView2View(decimal p_id)
        {
            try
            {
                Billing_BL _obj_bl = new Billing_BL();
                Billing_Header_Info _Billing_Header_Info = _obj_bl.Billing_GetBy_Id(p_id);
                return PartialView("~/Areas/Manager/Views/Billing/_PartialView.cshtml", _Billing_Header_Info);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/Manager/Views/Billing/_PartialView.cshtml", new Billing_Header_Info());
            }
        }

        // Insert 
        [HttpPost]
        [Route("danh-sach-billing/show-insert")]
        public ActionResult GetView2Insert()
        {
            return PartialView("~/Areas/Manager/Views/Billing/_PartialInsert.cshtml", new Billing_Header_Info());
        }

        [HttpPost]
        [Route("danh-sach-billing/do-insert-billing")]
        public ActionResult DoInsert(Docking_Info p_Billing_Header_Info)
        {
            try
            {

                Billing_BL _obj_bl = new Billing_BL();
                p_Billing_Header_Info.Created_By = SessionData.CurrentUser.Username;
                p_Billing_Header_Info.Created_Date = DateTime.Now;
                p_Billing_Header_Info.Language_Code = AppsCommon.GetCurrentLang();
                p_Billing_Header_Info.Status = (decimal)CommonEnums.Billing_Status.New_Wait_Approve;
                
                decimal _ck = _obj_bl.Billing_Insert(p_Billing_Header_Info);
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
        [Route("danh-sach-billing/show-edit")]
        public ActionResult GetView2Edit(int p_id)
        {
            try
            {
                Billing_BL _obj_bl = new Billing_BL();
                Billing_Header_Info _Billing_Header_Info = _obj_bl.Billing_GetBy_Id(p_id);
                return PartialView("~/Areas/Manager/Views/Billing/_PartialEdit.cshtml", _Billing_Header_Info);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/Manager/Views/Billing/_PartialEdit.cshtml");
            }
        }

        [HttpPost]
        [Route("danh-sach-billing/do-edit-billing")]
        public ActionResult DoEdit(Docking_Info p_Docking_Info)
        {
            try
            {
                Billing_BL _obj_bl = new Billing_BL();
                p_Docking_Info.Modify_By = SessionData.CurrentUser.Username;
                p_Docking_Info.Modify_Date = DateTime.Now;
                p_Docking_Info.Language_Code = AppsCommon.GetCurrentLang();

                if (p_Docking_Info.File_Upload != null)
                {
                    var url_File_Upload = AppLoadHelpers.PushFileToServer(p_Docking_Info.File_Upload, AppUpload.Document);
                    p_Docking_Info.FileName = p_Docking_Info.File_Upload.FileName;
                    p_Docking_Info.Url = url_File_Upload;
                }
                else
                {
                    p_Docking_Info.FileName = "NA";
                    p_Docking_Info.Url = "NA";
                }

                decimal _ck = _obj_bl.Billing_Update(p_Docking_Info);
                return Json(new { success = _ck });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = "-1" });
            }
        }

        [HttpPost]
        [Route("danh-sach-billing/show-change-status")]
        public ActionResult GetView2UpdateStatus(int p_id)
        {
            try
            {
                Billing_BL _obj_bl = new Billing_BL();
                Billing_Header_Info _Billing_Header_Info = _obj_bl.Billing_GetBy_Id(p_id);
                return PartialView("~/Areas/Manager/Views/Billing/_PartialChangeStatus.cshtml", _Billing_Header_Info);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/Manager/Views/Billing/_PartialChangeStatus.cshtml");
            }

        }

        [HttpPost]
        [Route("danh-sach-billing/do-change-status")]
        public ActionResult DoUpdateStatus(int p_id)
        {
            try
            {
                Billing_BL _obj_bl = new Billing_BL();
                var modifiedBy = SessionData.CurrentUser.Username;
                decimal _status = (decimal)CommonEnums.Billing_Status.Approved;
                decimal _result = _obj_bl.Billing_Update_Status(p_id, AppsCommon.GetCurrentLang(), _status, SessionData.CurrentUser.Username, DateTime.Now);
                return Json(new { success = _result });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = -1 });
            }
        }
    }
}
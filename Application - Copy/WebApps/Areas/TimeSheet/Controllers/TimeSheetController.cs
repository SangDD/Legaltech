using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApps.AppStart;
using ObjectInfos;
using WebApps.Session;
using BussinessFacade;
using Common.CommonData;

namespace WebApps.Areas.TimeSheet.Controllers
{
    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("Timesheet", AreaPrefix = "quan-ly-timesheet")]
    [Route("{action}")]
    public class TimeSheetController : Controller
    {
        [HttpGet]
        [Route("danh-sach-timesheet")]
        public ActionResult TimeSheetList()
        {
            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("~/home/index");

                decimal _total_record = 0;
                TimeSheet_BL _obj_bl = new TimeSheet_BL();
                string _keySearch = "ALL" + "|" + ((int)CommonEnums.App_Status.Luu_tam).ToString();
                if (SessionData.CurrentUser.Type == (int)CommonEnums.UserType.Admin)
                {
                    _keySearch = "ALL|ALL|ALL";
                }
                else
                {
                    _keySearch = "ALL|ALL|" + SessionData.CurrentUser.Lawer_Id;
                }


                List<Timesheet_Info> _lst = _obj_bl.Timesheet_Search(_keySearch, ref _total_record);
                string htmlPaging = CommonFuc.Get_HtmlPaging<Timesheet_Info>((int)_total_record, 1, "Timesheet");

                ViewBag.Obj = _lst;
                ViewBag.Paging = htmlPaging;
                ViewBag.SumRecord = _total_record;

                return View("~/Areas/TimeSheet/Views/TimeSheet/TimeSheetList.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return View();
            }
        }

        [HttpPost]
        [Route("danh-sach-timesheet/search")]
        public ActionResult Search_TimeSheet(string p_keysearch, int p_CurrentPage, string p_column, string p_type_sort)
        {
            try
            {
                decimal _total_record = 0;

                string p_to = "";
                string p_from = CommonFuc.Get_From_To_Page(p_CurrentPage, ref p_to);

                TimeSheet_BL _obj_bl = new TimeSheet_BL();
                List<Timesheet_Info> _lst = _obj_bl.Timesheet_Search(p_keysearch, ref _total_record);
                string htmlPaging = CommonFuc.Get_HtmlPaging<Timesheet_Info>((int)_total_record, 1, "Timesheet");

                ViewBag.Paging = htmlPaging;
                ViewBag.Obj = _lst;
                ViewBag.SumRecord = _total_record;
                return PartialView("~/Areas/TimeSheet/Views/TimeSheet/_PartialTableTimeSheet.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/TimeSheet/Views/TimeSheet/_PartialTableTimeSheet.cshtml");
            }
        }


        [HttpPost]
        [Route("danh-sach-timesheet/do-delete-timesheet")]
        public ActionResult DoDeleteUser(int p_id)
        {
            try
            {
                TimeSheet_BL _obj_bl = new TimeSheet_BL();
                var modifiedBy = SessionData.CurrentUser.Username;
                decimal _result = _obj_bl.Timesheet_Delete(p_id, SessionData.CurrentUser.Username, DateTime.Now);
                return Json(new { success = _result });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = -1 });
            }
        }

        [HttpPost]
        [Route("danh-sach-timesheet/show-view")]
        public ActionResult GetView2View(decimal p_id)
        {
            try
            {
                TimeSheet_BL _obj_bl = new TimeSheet_BL();
                Timesheet_Info _Timesheet_Info = _obj_bl.Timesheet_GetBy_Id(p_id);
                ViewBag.Timesheet_Info = _Timesheet_Info;
                return PartialView("~/Areas/TimeSheet/Views/TimeSheet/_PartialViewTimeSheet.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/TimeSheet/Views/TimeSheet/_PartialViewTimeSheet.cshtml");
            }
        }

        // Insert 
        [HttpPost]
        [Route("danh-sach-timesheet/show-insert")]
        public ActionResult GetView2Insert()
        {
            return PartialView("~/Areas/TimeSheet/Views/TimeSheet/_PartialInsertTimeSheet.cshtml", new Timesheet_Info());
        }

        [HttpPost]
        [Route("danh-sach-timesheet/do-insert-timeshet")]
        public ActionResult DoInsertTimeSheet(Timesheet_Info p_Timesheet_Info)
        {
            try
            {
                TimeSheet_BL _obj_bl = new TimeSheet_BL();
                p_Timesheet_Info.Created_By = SessionData.CurrentUser.Username;
                p_Timesheet_Info.Lawer_Id = SessionData.CurrentUser.Lawer_Id;
                p_Timesheet_Info.Status = (decimal)CommonEnums.TimeSheet_Status.New;
                decimal _ck = _obj_bl.Timesheet_Insert(p_Timesheet_Info);
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
        [Route("danh-sach-timesheet/show-edit")]
        public ActionResult GetView2Edit(int p_id)
        {
            try
            {
                TimeSheet_BL _obj_bl = new TimeSheet_BL();
                Timesheet_Info _Timesheet_Info = _obj_bl.Timesheet_GetBy_Id(p_id);
                return PartialView("~/Areas/TimeSheet/Views/TimeSheet/_PartialEditTimeSheet.cshtml", _Timesheet_Info);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/TimeSheet/Views/TimeSheet/_PartialEditTimeSheet.cshtml");
            }

        }

        [HttpPost]
        [Route("danh-sach-timesheet/do-edit-timeshet")]
        public ActionResult DoEditTimeSheet(Timesheet_Info p_Timesheet_Info)
        {
            try
            {
                TimeSheet_BL _obj_bl = new TimeSheet_BL();
                p_Timesheet_Info.Modify_By = SessionData.CurrentUser.Username;
                decimal _ck = _obj_bl.Timesheet_Update(p_Timesheet_Info);
                return Json(new { success = _ck });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = "-1" });
            }
        }

        // Approve
        [HttpPost]
        [Route("danh-sach-timesheet/show-approve")]
        public ActionResult GetView2Approve(int p_id)
        {
            try
            {
                TimeSheet_BL _obj_bl = new TimeSheet_BL();
                Timesheet_Info _Timesheet_Info = _obj_bl.Timesheet_GetBy_Id(p_id);
                return PartialView("~/Areas/TimeSheet/Views/TimeSheet/_PartialApproveTimeSheet.cshtml", _Timesheet_Info);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/TimeSheet/Views/TimeSheet/_PartialApproveTimeSheet.cshtml");
            }

        }

        [HttpPost]
        [Route("danh-sach-timesheet/do-approve-timeshet")]
        public ActionResult DoApproveTimeSheet(decimal p_id, int p_status, string p_reject_reason)
        {
            try
            {
                TimeSheet_BL _obj_bl = new TimeSheet_BL();
                decimal _ck = _obj_bl.Timesheet_Approve(p_id, p_status, p_reject_reason, SessionData.CurrentUser.Username);
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
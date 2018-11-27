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
using WebApps.CommonFunction;

namespace WebApps.Areas.Manager.Controllers
{
    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("Manager", AreaPrefix = "quan-ly-timesheet")]
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
                    return Redirect("/");

                decimal _total_record = 0;
                TimeSheet_BL _obj_bl = new TimeSheet_BL();
                string _keySearch = "ALL" + "|" + ((int)CommonEnums.App_Status.Luu_tam).ToString();
                if (SessionData.CurrentUser.Type == (int)CommonEnums.UserType.Admin)
                {
                    _keySearch = "ALL|ALL|ALL";
                }
                else
                {
                    _keySearch = "ALL|ALL|" + SessionData.CurrentUser.Id;
                }


                List<Timesheet_Info> _lst = _obj_bl.Timesheet_Search(SessionData.CurrentUser.Username, _keySearch, ref _total_record);
                string htmlPaging = CommonFuc.Get_HtmlPaging<Timesheet_Info>((int)_total_record, 1, "Timesheet");

                ViewBag.Obj = _lst;
                ViewBag.Paging = htmlPaging;
                ViewBag.SumRecord = _total_record;

                return View("~/Areas/Manager/Views/TimeSheet/TimeSheetList.cshtml");
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
                List<Timesheet_Info> _lst = _obj_bl.Timesheet_Search(SessionData.CurrentUser.Username, p_keysearch, ref _total_record, p_from, p_to);
                string htmlPaging = CommonFuc.Get_HtmlPaging<Timesheet_Info>((int)_total_record, 1, "Timesheet");

                ViewBag.Paging = htmlPaging;
                ViewBag.Obj = _lst;
                ViewBag.SumRecord = _total_record;
                return PartialView("~/Areas/Manager/Views/TimeSheet/_PartialTableTimeSheet.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/Manager/Views/TimeSheet/_PartialTableTimeSheet.cshtml");
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

        [HttpGet]
        [Route("danh-sach-timesheet/show-view/{id}")]
        public ActionResult GetView2View()
        {
            try
            {
                if (RouteData.Values["id"] != null && RouteData.Values["id"].ToString() != "")
                {
                    decimal p_id = Convert.ToDecimal(RouteData.Values["id"].ToString());
                    TimeSheet_BL _obj_bl = new TimeSheet_BL();
                    Timesheet_Info _Timesheet_Info = _obj_bl.Timesheet_GetBy_Id(p_id);
                    return View("~/Areas/Manager/Views/TimeSheet/_PartialViewTimeSheet.cshtml", _Timesheet_Info);
                }
                else return View();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return View("~/Areas/Manager/Views/TimeSheet/_PartialViewTimeSheet.cshtml", new Timesheet_Info());
            }
        }

        [HttpGet]
        [Route("danh-sach-timesheet/show-view-by-case-code/{id}")]
        public ActionResult GetView2View_ByCaseCode()
        {
            try
            {
                if (RouteData.Values["id"] != null && RouteData.Values["id"].ToString() != "")
                {
                    string p_caseCode = RouteData.Values["id"].ToString();
                    TimeSheet_BL _obj_bl = new TimeSheet_BL();
                    Timesheet_Info _Timesheet_Info = _obj_bl.Timesheet_GetBy_Casecode(p_caseCode);
                    return View("~/Areas/Manager/Views/TimeSheet/_PartialViewTimeSheet.cshtml", _Timesheet_Info);
                }
                else return View();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return View("~/Areas/Manager/Views/TimeSheet/_PartialViewTimeSheet.cshtml", new Timesheet_Info());
            }
        }


        // Insert 
        [HttpGet]
        [Route("danh-sach-timesheet/show-insert")]
        public ActionResult GetView2Insert()
        {
            return View("~/Areas/Manager/Views/TimeSheet/_PartialInsertTimeSheet.cshtml", new Timesheet_Info());
        }

        [HttpPost]
        [Route("danh-sach-timesheet/do-insert-timeshet")]
        public ActionResult DoInsertTimeSheet(Timesheet_Info p_Timesheet_Info)
        {
            try
            {
                TimeSheet_BL _obj_bl = new TimeSheet_BL();
                p_Timesheet_Info.Created_By = SessionData.CurrentUser.Username;
                p_Timesheet_Info.Lawer_Id = SessionData.CurrentUser.Id;
                p_Timesheet_Info.Status = (decimal)CommonEnums.TimeSheet_Status.New;
                decimal _ck = _obj_bl.Timesheet_Insert(p_Timesheet_Info, AppsCommon.GetCurrentLang());
                return Json(new { success = _ck });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = "-1" });
            }
        }

        // edit
        [HttpGet]
        [Route("danh-sach-timesheet/show-edit/{id}")]
        public ActionResult GetView2Edit()
        {
            try
            {
                if (RouteData.Values["id"] != null && RouteData.Values["id"].ToString() != "")
                {
                    decimal p_id = Convert.ToDecimal(RouteData.Values["id"].ToString());
                    TimeSheet_BL _obj_bl = new TimeSheet_BL();
                    Timesheet_Info _Timesheet_Info = _obj_bl.Timesheet_GetBy_Id(p_id);
                    return View("~/Areas/Manager/Views/TimeSheet/_PartialEditTimeSheet.cshtml", _Timesheet_Info);
                }
                else return View();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return View("~/Areas/Manager/Views/TimeSheet/_PartialEditTimeSheet.cshtml");
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
                decimal _ck = _obj_bl.Timesheet_Update(p_Timesheet_Info, AppsCommon.GetCurrentLang());
                return Json(new { success = _ck });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = "-1" });
            }
        }

        // Approve
        [HttpGet]
        [Route("danh-sach-timesheet/show-approve/{id}")]
        public ActionResult GetView2Approve()
        {
            try
            {
                if (RouteData.Values["id"] != null && RouteData.Values["id"].ToString() != "")
                {
                    decimal p_id = Convert.ToDecimal(RouteData.Values["id"].ToString());
                    TimeSheet_BL _obj_bl = new TimeSheet_BL();
                    Timesheet_Info _Timesheet_Info = _obj_bl.Timesheet_GetBy_Id(p_id);
                    return View("~/Areas/Manager/Views/TimeSheet/_PartialApproveTimeSheet.cshtml", _Timesheet_Info);
                }
                else return View();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return View("~/Areas/Manager/Views/TimeSheet/_PartialApproveTimeSheet.cshtml");
            }

        }

        [HttpGet]
        [Route("danh-sach-timesheet/show-action-by-casecode/{id}")]
        public ActionResult GetView2Action_ByCasecode()
        {
            try
            {
                if (RouteData.Values["id"] != null && RouteData.Values["id"].ToString() != "")
                {
                    string p_caseCode = RouteData.Values["id"].ToString();
                    TimeSheet_BL _obj_bl = new TimeSheet_BL();
                    Timesheet_Info _Timesheet_Info = _obj_bl.Timesheet_GetBy_Casecode(p_caseCode);
                    if (_Timesheet_Info.Status == (decimal)Common.CommonData.CommonEnums.TimeSheet_Status.New)
                    {
                        return View("~/Areas/Manager/Views/TimeSheet/_PartialApproveTimeSheet.cshtml", _Timesheet_Info);
                    }
                    else if (_Timesheet_Info.Status == (decimal)Common.CommonData.CommonEnums.TimeSheet_Status.Reject)
                    {
                        return View("~/Areas/Manager/Views/TimeSheet/_PartialEditTimeSheet.cshtml", _Timesheet_Info);
                    }
                    else
                    {
                        return View("~/Areas/Manager/Views/TimeSheet/_PartialViewTimeSheet.cshtml", _Timesheet_Info);
                    }
                }
                else return View();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return View("~/Areas/Manager/Views/TimeSheet/_PartialApproveTimeSheet.cshtml");
            }

        }

        [HttpGet]
        [Route("danh-sach-timesheet/show-approve-by-casecode/{id}")]
        public ActionResult GetView2Approve_ByCasecode()
        {
            try
            {
                if (RouteData.Values["id"] != null && RouteData.Values["id"].ToString() != "")
                {
                    string p_caseCode = RouteData.Values["id"].ToString();
                    TimeSheet_BL _obj_bl = new TimeSheet_BL();
                    Timesheet_Info _Timesheet_Info = _obj_bl.Timesheet_GetBy_Casecode(p_caseCode);
                    return View("~/Areas/Manager/Views/TimeSheet/_PartialApproveTimeSheet.cshtml", _Timesheet_Info);
                }
                else return View();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return View("~/Areas/Manager/Views/TimeSheet/_PartialApproveTimeSheet.cshtml");
            }

        }

        [HttpPost]
        [Route("danh-sach-timesheet/do-approve-timeshet")]
        public ActionResult DoApproveTimeSheet(decimal p_id, int p_status, string p_reject_reason, string p_notes)
        {
            try
            {
                TimeSheet_BL _obj_bl = new TimeSheet_BL();
                decimal _ck = _obj_bl.Timesheet_Approve(p_id, p_status, p_reject_reason, p_notes, SessionData.CurrentUser.Username);
                return Json(new { success = _ck });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = "-1" });
            }
        }

        [HttpPost]
        [Route("danh-sach-timesheet/call-hours")]
        public ActionResult CallHours(string p_From_Time, string p_To_Time)
        {
            try
            {
                p_From_Time = p_From_Time + ":00";
                p_To_Time = p_To_Time + ":00";

                DateTime _FromDate = ConvertData.ConvertStringTime2Date(p_From_Time);
                DateTime _ToDate = ConvertData.ConvertStringTime2Date(p_To_Time);
                TimeSpan _ts = _ToDate - _FromDate;
                return Json(new { success = Math.Round(_ts.TotalHours, 2).ToString("") });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = DateTime.Now.AddMonths(1).ToString("dd/MM/yyyy") });
            }
        }
    }
}
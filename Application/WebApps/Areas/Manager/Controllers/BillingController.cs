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

        [Route("danh-sach-billing/show-view-billing")]
        public ActionResult GetView2View_Biling(decimal id, string case_code)
        {
            try
            {
                Billing_BL _obj_bl = new Billing_BL();
                ApplicationHeaderInfo objAppHeaderInfo = new ApplicationHeaderInfo();
                List<Billing_Detail_Info> _lst_billing_detail = new List<Billing_Detail_Info>();
                Billing_Header_Info _Billing_Header_Info = _obj_bl.Billing_GetBy_Id(id, case_code, AppsCommon.GetCurrentLang(), ref objAppHeaderInfo, ref _lst_billing_detail);

                ViewBag.objAppHeaderInfo = objAppHeaderInfo;
                ViewBag.List_Billing = _lst_billing_detail;
                return PartialView("~/Areas/Manager/Views/Billing/_PartialView.cshtml", _Billing_Header_Info);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/Manager/Views/Billing/_PartialView.cshtml", new Billing_Header_Info());
            }
        }


        // Insert 
        [HttpGet]
        [Route("danh-sach-billing/show-insert")]
        public ActionResult GetView2Insert()
        {
            try
            {
                Billing_BL _obj_bl = new Billing_BL();
                string _caseCode = _obj_bl.Billing_GenCaseCode();
                Billing_Header_Info _Billing_Header_Info = new Billing_Header_Info();
                _Billing_Header_Info.Case_Code = _caseCode;
                //return PartialView("~/Areas/Manager/Views/Billing/_PartialInsert.cshtml", _Billing_Header_Info);

                return View("~/Areas/Manager/Views/Billing/_PartialInsert.cshtml", _Billing_Header_Info);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);

                Billing_Header_Info _Billing_Header_Info = new Billing_Header_Info();
                return PartialView("~/Areas/Manager/Views/Billing/_PartialInsert.cshtml", new Billing_Header_Info());
            }
        }

        [HttpPost]
        [Route("danh-sach-billing/GetAppByCaseCode")]
        public ActionResult GetInfoByCaseCode(string p_case_code)
        {
            try
            {
                Application_Header_BL _bl = new Application_Header_BL();
                List<Billing_Detail_Info> _lst_billing_detail = new List<Billing_Detail_Info>();
                ApplicationHeaderInfo objAppHeaderInfo = _bl.GetApp_By_Case_Code(p_case_code, SessionData.CurrentUser.Username, AppsCommon.GetCurrentLang(), ref _lst_billing_detail);
                ViewBag.objAppHeaderInfo = objAppHeaderInfo;

                Billing_Detail_Info _ChiPhiKhac = new Billing_Detail_Info();
                _ChiPhiKhac.Nation_Fee = 0;
                _ChiPhiKhac.Represent_Fee = 0;
                _ChiPhiKhac.Service_Fee = 0;
                _ChiPhiKhac.Biling_Detail_Name = "Chi phí khác";
                _ChiPhiKhac.Type = Convert.ToDecimal(Common.CommonData.CommonEnums.Billing_Detail_Type.Service);
                _lst_billing_detail.Add(_ChiPhiKhac);

                foreach (Billing_Detail_Info item in _lst_billing_detail)
                {
                    item.Total_Fee = item.Nation_Fee + item.Represent_Fee + item.Service_Fee;
                }

                SessionData.SetDataSession(p_case_code, _lst_billing_detail);

                ViewBag.List_Billing = _lst_billing_detail;

                var Partial_AppInfo = AppsCommon.RenderRazorViewToString(this.ControllerContext, "~/Areas/Manager/Views/Billing/_Partial_AppInfo.cshtml");
                var PartialDetail_Insert_Billing = AppsCommon.RenderRazorViewToString(this.ControllerContext, "~/Areas/Manager/Views/Billing/_PartialDetail_Insert_Billing.cshtml");

                var json = Json(new { Partial_AppInfo, PartialDetail_Insert_Billing });
                return json;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/TradeMark/Views/Shared/_PartialThongTinChuDon.cshtml");
            }
        }

        [HttpPost]
        [Route("danh-sach-billing/do-delete-billing-detail")]
        public ActionResult doDeleteBillingDetail(string p_case_code, decimal p_Ref_Id, decimal p_Type)
        {
            try
            {
                List<Billing_Detail_Info> _lst_billing_detail = (List<Billing_Detail_Info>)SessionData.GetDataSession(p_case_code);
                if (_lst_billing_detail == null)
                {
                    _lst_billing_detail = new List<Billing_Detail_Info>();
                }

                if (_lst_billing_detail.Count > 0)
                    _lst_billing_detail.RemoveAll(x => x.Ref_Id == p_Ref_Id);

                SessionData.SetDataSession(p_case_code, _lst_billing_detail);

                ViewBag.List_Billing = _lst_billing_detail;
                return PartialView("~/Areas/Manager/Views/Billing/_PartialDetail_Insert_Billing.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/Manager/Views/Billing/_PartialTableBilling.cshtml");
            }
        }

        [HttpPost]
        [Route("danh-sach-billing/change-other-billing-detail")]
        public ActionResult ChangeOthersBillingDetail(string p_case_code, decimal p_amount)
        {
            try
            {
                List<Billing_Detail_Info> _lst_billing_detail = (List<Billing_Detail_Info>)SessionData.GetDataSession(p_case_code);
                if (_lst_billing_detail == null)
                {
                    _lst_billing_detail = new List<Billing_Detail_Info>();
                }

                foreach (Billing_Detail_Info item in _lst_billing_detail)
                {
                    if (item.Type != Convert.ToDecimal(Common.CommonData.CommonEnums.Billing_Detail_Type.Service)) continue;

                    item.Service_Fee = p_amount;
                    item.Total_Fee = item.Nation_Fee + item.Represent_Fee + item.Service_Fee;
                }

                SessionData.SetDataSession(p_case_code, _lst_billing_detail);

                ViewBag.List_Billing = _lst_billing_detail;
                return PartialView("~/Areas/Manager/Views/Billing/_PartialDetail_Insert_Billing.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/Manager/Views/Billing/_PartialTableBilling.cshtml");
            }
        }

        [HttpPost]
        [Route("danh-sach-billing/do-insert-billing")]
        public ActionResult DoInsert(Billing_Header_Info p_Billing_Header_Info)
        {
            try
            {
                List<Billing_Detail_Info> _lst_billing_detail = (List<Billing_Detail_Info>)SessionData.GetDataSession(p_Billing_Header_Info.App_Case_Code);
                if (_lst_billing_detail == null)
                {
                    _lst_billing_detail = new List<Billing_Detail_Info>();
                };

                if (p_Billing_Header_Info.Total_Vnd == 0)
                {
                    return Json(new { success = "-2" });
                }

                Billing_BL _obj_bl = new Billing_BL();
                p_Billing_Header_Info.Created_By = SessionData.CurrentUser.Username;
                p_Billing_Header_Info.Created_Date = DateTime.Now;
                p_Billing_Header_Info.Language_Code = AppsCommon.GetCurrentLang();
                p_Billing_Header_Info.Status = (decimal)CommonEnums.Billing_Status.New_Wait_Approve;

                decimal _ck = 0;
                using (var scope = new TransactionScope())
                {
                    _ck = _obj_bl.Billing_Insert(p_Billing_Header_Info);

                    if (_ck > 0 && _lst_billing_detail.Count > 0)
                    {
                        _ck = _obj_bl.Billing_Detail_InsertBatch(_lst_billing_detail, _ck);
                    }

                    //end
                    if (_ck < 0)
                    {
                        Transaction.Current.Rollback();
                    }
                    else
                    {
                        scope.Complete();
                    }
                }
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
        public ActionResult GetView2Edit(int p_id, string p_app_case_code)
        {
            try
            {
                Billing_BL _obj_bl = new Billing_BL();
                ApplicationHeaderInfo objAppHeaderInfo = new ApplicationHeaderInfo();
                List<Billing_Detail_Info> _lst_billing_detail = new List<Billing_Detail_Info>();
                Billing_Header_Info _Billing_Header_Info = _obj_bl.Billing_GetBy_Id(p_id, p_app_case_code, AppsCommon.GetCurrentLang(), ref objAppHeaderInfo, ref _lst_billing_detail);

                ViewBag.objAppHeaderInfo = objAppHeaderInfo;
                ViewBag.List_Billing = _lst_billing_detail;
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
        public ActionResult DoEdit(Billing_Header_Info p_Billing_Header_Info)
        {
            try
            {
                Billing_BL _obj_bl = new Billing_BL();
                p_Billing_Header_Info.Modify_By = SessionData.CurrentUser.Username;
                p_Billing_Header_Info.Modify_Date = DateTime.Now;
                p_Billing_Header_Info.Language_Code = AppsCommon.GetCurrentLang();

                decimal _ck = _obj_bl.Billing_Update(p_Billing_Header_Info);
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
        public ActionResult GetView2UpdateStatus(int p_id, string p_app_case_code)
        {
            try
            {
                Billing_BL _obj_bl = new Billing_BL();
                ApplicationHeaderInfo objAppHeaderInfo = new ApplicationHeaderInfo();
                List<Billing_Detail_Info> _lst_billing_detail = new List<Billing_Detail_Info>();
                Billing_Header_Info _Billing_Header_Info = _obj_bl.Billing_GetBy_Id(p_id, p_app_case_code, AppsCommon.GetCurrentLang(), ref objAppHeaderInfo, ref _lst_billing_detail);

                ViewBag.objAppHeaderInfo = objAppHeaderInfo;
                ViewBag.List_Billing = _lst_billing_detail;
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

        [HttpPost]
        [Route("danh-sach-billing/show-billing")]
        public ActionResult GetView2Approve(int p_id, string p_app_case_code)
        {
            try
            {
                Billing_BL _obj_bl = new Billing_BL();
                ApplicationHeaderInfo objAppHeaderInfo = new ApplicationHeaderInfo();
                List<Billing_Detail_Info> _lst_billing_detail = new List<Billing_Detail_Info>();
                Billing_Header_Info _Billing_Header_Info = _obj_bl.Billing_GetBy_Id(p_id, p_app_case_code, AppsCommon.GetCurrentLang(), ref objAppHeaderInfo, ref _lst_billing_detail);

                ViewBag.objAppHeaderInfo = objAppHeaderInfo;
                ViewBag.List_Billing = _lst_billing_detail;
                return PartialView("~/Areas/Manager/Views/Billing/_PartialView.cshtml", _Billing_Header_Info);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/TimeSheet/Views/TimeSheet/_PartialApproveTimeSheet.cshtml");
            }

        }
    }
}
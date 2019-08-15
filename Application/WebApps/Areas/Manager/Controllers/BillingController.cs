using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WebApps.AppStart;
using WebApps.Session;
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
                    return Redirect("/");

                decimal _total_record = 0;
                Billing_BL _obj_bl = new Billing_BL();
                string _keySearch = "ALL" + "|" + "ALL" + "|" + "ALL" + "|" + "ALL" + "|" + "ALL";
                List<Billing_Header_Info> _lst = _obj_bl.Billing_Search(SessionData.CurrentUser.Username, _keySearch, ref _total_record);
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
                List<Billing_Header_Info> _lst = _obj_bl.Billing_Search(SessionData.CurrentUser.Username, p_keysearch, ref _total_record, p_from, p_to);
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
                SearchObject_Header_Info SearchObject_Header_Info = new SearchObject_Header_Info();
                List<Billing_Detail_Info> _lst_billing_detail = new List<Billing_Detail_Info>();
                Billing_Header_Info _Billing_Header_Info = new Billing_Header_Info();
                if (case_code.Contains("SEARCH"))
                {
                    _Billing_Header_Info = _obj_bl.Billing_Search_GetBy_Id(id, case_code, AppsCommon.GetCurrentLang(), ref SearchObject_Header_Info, ref _lst_billing_detail);
                }
                else
                {
                    _Billing_Header_Info = _obj_bl.Billing_GetBy_Id(id, case_code, AppsCommon.GetCurrentLang(), ref objAppHeaderInfo, ref _lst_billing_detail);
                }

                foreach (Billing_Detail_Info item in _lst_billing_detail)
                {
                    item.Total_Fee = item.Nation_Fee + item.Represent_Fee + item.Service_Fee;
                }

                ViewBag.List_Billing = _lst_billing_detail;
                ViewBag.Operator_Type = Convert.ToDecimal(Common.CommonData.CommonEnums.Operator_Type.View);
                ViewBag.Billing_Header_Info = _Billing_Header_Info;

                ViewBag.objSearch_HeaderInfo = SearchObject_Header_Info;
                ViewBag.objAppHeaderInfo = objAppHeaderInfo;

                return PartialView("~/Areas/Manager/Views/Billing/_PartialView.cshtml", _Billing_Header_Info);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/Manager/Views/Billing/_PartialView.cshtml", new Billing_Header_Info());
            }
        }

        [Route("danh-sach-billing/show-view-billing-by-code")]
        public ActionResult GetView2View_Biling_ByCode(string case_code)
        {
            try
            {
                Billing_BL _obj_bl = new Billing_BL();
                ApplicationHeaderInfo objAppHeaderInfo = new ApplicationHeaderInfo();
                SearchObject_Header_Info SearchObject_Header_Info = new SearchObject_Header_Info();
                List<Billing_Detail_Info> _lst_billing_detail = new List<Billing_Detail_Info>();
                Billing_Header_Info _Billing_Header_Info = new Billing_Header_Info();
                if (case_code.Contains("SEARCH"))
                {
                    _Billing_Header_Info = _obj_bl.Billing_Search_GetBy_Code(case_code, AppsCommon.GetCurrentLang(), ref SearchObject_Header_Info, ref _lst_billing_detail);
                }
                else
                {
                    _Billing_Header_Info = _obj_bl.Billing_GetBy_Code(case_code, AppsCommon.GetCurrentLang(), ref objAppHeaderInfo, ref _lst_billing_detail);
                }

                foreach (Billing_Detail_Info item in _lst_billing_detail)
                {
                    item.Total_Fee = item.Nation_Fee + item.Represent_Fee + item.Service_Fee;
                }

                ViewBag.Billing_Header_Info = _Billing_Header_Info;
                ViewBag.List_Billing = _lst_billing_detail;
                ViewBag.Operator_Type = Convert.ToDecimal(Common.CommonData.CommonEnums.Operator_Type.View);


                ViewBag.objSearch_HeaderInfo = SearchObject_Header_Info;
                ViewBag.objAppHeaderInfo = objAppHeaderInfo;

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
                return View("~/Areas/Manager/Views/Billing/_PartialInsert.cshtml", _Billing_Header_Info);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);

                Billing_Header_Info _Billing_Header_Info = new Billing_Header_Info();
                return PartialView("~/Areas/Manager/Views/Billing/_PartialInsert.cshtml", new Billing_Header_Info());
            }
        }

        [HttpGet]
        [Route("danh-sach-billing/show-insert-by-casecode")]
        public ActionResult GetView2InsertByCaseCode(string p_case_code, decimal p_type)
        {
            try
            {
                Billing_BL _obj_bl = new Billing_BL();
                string _caseCode = _obj_bl.Billing_GenCaseCode();
                Billing_Header_Info _Billing_Header_Info = new Billing_Header_Info();
                _Billing_Header_Info.Case_Code = _caseCode;
                ViewBag.Case_Code = p_case_code;
                ViewBag.Insert_Type = p_type;
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
        [Route("danh-sach-billing/do-insert-billing")]
        public ActionResult DoInsert(Billing_Header_Info p_Billing_Header_Info)
        {
            try
            {
                List<Billing_Detail_Info> _lst_billing_detail = Get_LstFee_Detail(p_Billing_Header_Info.App_Case_Code);

                if (p_Billing_Header_Info.Total_Amount == 0)
                {
                    return Json(new { success = "-2" });
                }

                Billing_BL _obj_bl = new Billing_BL();
                p_Billing_Header_Info.Created_By = SessionData.CurrentUser.Username;
                p_Billing_Header_Info.Created_Date = DateTime.Now;
                p_Billing_Header_Info.Language_Code = AppsCommon.GetCurrentLang();
                p_Billing_Header_Info.Status = (decimal)CommonEnums.Billing_Status.New_Wait_Approve;
                p_Billing_Header_Info.Billing_Type = (decimal)CommonEnums.Billing_Type.App;
                p_Billing_Header_Info.Currency_Rate = AppsCommon.Get_Currentcy_VCB();
                if (p_Billing_Header_Info.App_Case_Code.Contains("SEARCH"))
                {
                    p_Billing_Header_Info.Billing_Type = (decimal)CommonEnums.Billing_Type.Search;
                }
                decimal _ck = 0;
                decimal _idBilling = 0;
                using (var scope = new TransactionScope())
                {
                    _idBilling = _obj_bl.Billing_Insert(p_Billing_Header_Info);

                    if (_idBilling > 0 && _lst_billing_detail.Count > 0)
                    {
                        _ck = _obj_bl.Billing_Detail_InsertBatch(_lst_billing_detail, _idBilling);
                    }

                    //if (_ck > 0 && p_Billing_Header_Info.Insert_Type != (decimal)Common.CommonData.CommonEnums.Billing_Insert_Type.App)
                    //{
                    //    string _fileExport = Export_Billing(p_Billing_Header_Info.Case_Code);
                    //    if (_fileExport == "") goto Commit_Transaction;

                    //    if (p_Billing_Header_Info.Insert_Type == (decimal)Common.CommonData.CommonEnums.Billing_Insert_Type.Advise_Filling)
                    //    {
                    //        Application_Header_BL _BL = new Application_Header_BL();
                    //        _ck = _BL.AppHeader_Update_Advise_Url_Billing(p_Billing_Header_Info.App_Case_Code, _idBilling, _fileExport);
                    //    }
                    //    else
                    //    {
                    //        string _key = "BILLING_APP_URL_" + p_Billing_Header_Info.App_Case_Code + "_" + p_Billing_Header_Info.Insert_Type.ToString();
                    //        SessionData.SetDataSession(_key, _fileExport);

                    //        _key = "BILLING_APP_ID_" + p_Billing_Header_Info.App_Case_Code + "_" + p_Billing_Header_Info.Insert_Type.ToString();
                    //        SessionData.SetDataSession(_key, _idBilling);
                    //    }

                    //    // nếu kết xuất file thành công thì insert vào docking
                    //    AppsCommon.Insert_Docketing(p_Billing_Header_Info.App_Case_Code, "Report Billing", _fileExport, true);
                    //}

                    //end
                    Commit_Transaction:
                    if (_ck < 0)
                    {
                        Transaction.Current.Rollback();
                    }
                    else
                    {
                        SessionData.RemoveDataSession(p_Billing_Header_Info.App_Case_Code);
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

        // GetAppByCaseCode
        [HttpPost]
        [Route("danh-sach-billing/GetAppByCaseCode")]
        public ActionResult GetInfoByCaseCode(string p_case_code)
        {
            try
            {
                Billing_BL _obj_bl = new Billing_BL();
                ApplicationHeaderInfo objAppHeaderInfo = new ApplicationHeaderInfo();
                SearchObject_Header_Info SearchObject_Header_Info = new SearchObject_Header_Info();
                List<Billing_Detail_Info> _lst_billing_detail = new List<Billing_Detail_Info>();
                Billing_Header_Info _Billing_Header_Info = new Billing_Header_Info();
                if (p_case_code.Contains("SEARCH"))
                {
                    SearchObject_BL _bl = new SearchObject_BL();
                    SearchObject_Header_Info = _bl.GetBilling_By_Case_Code(p_case_code, SessionData.CurrentUser.Username,
                      AppsCommon.GetCurrentLang(), ref _lst_billing_detail);

                    ViewBag.Currency_Type = SearchObject_Header_Info.Currency_Type;
                    SessionData.SetDataSession(p_case_code + "_CURRENCY_TYPE", SearchObject_Header_Info.Currency_Type);

                    ViewBag.objSearch_HeaderInfo = SearchObject_Header_Info;
                    if (SearchObject_Header_Info == null)
                    {
                        return Json(new { success = -1 });
                    }
                }
                else
                {
                    Application_Header_BL _bl = new Application_Header_BL();
                    objAppHeaderInfo = _bl.GetBilling_ByAppCase_Code(p_case_code, SessionData.CurrentUser.Username, AppsCommon.GetCurrentLang(), ref _lst_billing_detail);
                    ViewBag.objAppHeaderInfo = objAppHeaderInfo;
                    ViewBag.Currency_Type = objAppHeaderInfo.Currency_Type;
                    SessionData.SetDataSession(p_case_code + "_CURRENCY_TYPE", objAppHeaderInfo.Currency_Type);

                    if (objAppHeaderInfo == null)
                    {
                        return Json(new { success = -1 });
                    }


                    // chỉ lấy những thằng nào mà > đã nộp đơn lên cục
                    if (objAppHeaderInfo != null && objAppHeaderInfo.Status < (decimal)Common.CommonData.CommonEnums.App_Status.DaNopDon)
                    {
                        return Json(new { success = -2 });
                    }
                }

                // chi phí khác
                Billing_Detail_Info _ChiPhiKhac = new Billing_Detail_Info();
                _ChiPhiKhac.Nation_Fee = 0;
                _ChiPhiKhac.Represent_Fee = 0;
                _ChiPhiKhac.Service_Fee = 0;
                _ChiPhiKhac.Biling_Detail_Name = "Chi phí khác";
                _ChiPhiKhac.Type = Convert.ToDecimal(Common.CommonData.CommonEnums.Billing_Detail_Type.Others);
                _lst_billing_detail.Add(_ChiPhiKhac);

                foreach (Billing_Detail_Info item in _lst_billing_detail)
                {
                    item.Total_Fee = item.Nation_Fee + item.Represent_Fee + item.Service_Fee;
                }

                SessionData.SetDataSession(p_case_code, _lst_billing_detail);

                ViewBag.List_Billing = _lst_billing_detail;
                ViewBag.Operator_Type = Convert.ToDecimal(Common.CommonData.CommonEnums.Operator_Type.Insert);
                ViewBag.App_Case_Code = p_case_code;
                ViewBag.ShowPopUp = 0;

                if (p_case_code.Contains("SEARCH"))
                {
                    var Partial_AppInfo = AppsCommon.RenderRazorViewToString(this.ControllerContext, "~/Areas/Manager/Views/Billing_Search/_Partial_SearchInfo.cshtml");
                    var PartialDetail_Insert_Billing = AppsCommon.RenderRazorViewToString(this.ControllerContext, "~/Areas/Manager/Views/Billing_Search/_PartialDetail_Insert_Billing.cshtml");

                    var json = Json(new { success = 1, Partial_AppInfo, PartialDetail_Insert_Billing });
                    return json;
                }
                else
                {
                    var Partial_AppInfo = AppsCommon.RenderRazorViewToString(this.ControllerContext, "~/Areas/Manager/Views/Billing/_Partial_AppInfo.cshtml");
                    var PartialDetail_Insert_Billing = AppsCommon.RenderRazorViewToString(this.ControllerContext, "~/Areas/Manager/Views/Billing/_PartialDetail_Insert_Billing.cshtml");
                    var json = Json(new { success = 1, Partial_AppInfo, PartialDetail_Insert_Billing });
                    return json;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/TradeMark/Views/Shared/_PartialThongTinChuDon.cshtml");
            }
        }

        [HttpPost]
        [Route("danh-sach-billing/do-delete-billing-detail")]
        public ActionResult doDeleteBillingDetail(string p_case_code, decimal p_Ref_Id, decimal p_Type, decimal p_ShowPopUp)
        {
            try
            {
                List<Billing_Detail_Info> _lst_billing_detail = Get_LstFee_Detail(p_case_code);

                if (_lst_billing_detail.Count > 0)
                    _lst_billing_detail.RemoveAll(x => x.Ref_Id == p_Ref_Id);

                if (_lst_billing_detail.Count == 0)
                {
                    Billing_Detail_Info _ChiPhiKhac = new Billing_Detail_Info();
                    _ChiPhiKhac.Nation_Fee = 0;
                    _ChiPhiKhac.Represent_Fee = 0;
                    _ChiPhiKhac.Service_Fee = 0;
                    _ChiPhiKhac.Biling_Detail_Name = "Chi phí khác";
                    _ChiPhiKhac.Type = Convert.ToDecimal(Common.CommonData.CommonEnums.Billing_Detail_Type.Others);
                    _lst_billing_detail.Add(_ChiPhiKhac);
                }

                foreach (Billing_Detail_Info item in _lst_billing_detail)
                {
                    item.Total_Fee = item.Nation_Fee + item.Represent_Fee + item.Service_Fee;
                }

                SessionData.SetDataSession(p_case_code, _lst_billing_detail);
                ViewBag.List_Billing = _lst_billing_detail;

                ViewBag.App_Case_Code = p_case_code;
                ViewBag.ShowPopUp = p_ShowPopUp;
                string _Currency_Type = (string)SessionData.GetDataSession(p_case_code + "_CURRENCY_TYPE");
                if (_Currency_Type != null)
                {
                    ViewBag.Currency_Type = _Currency_Type;
                }

                if (p_case_code.Contains("SEARCH"))
                {
                    return PartialView("~/Areas/Manager/Views/Billing_Search/_PartialDetail_Insert_Billing.cshtml");
                }
                else
                {
                    return PartialView("~/Areas/Manager/Views/Billing/_PartialDetail_Insert_Billing.cshtml");
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/Manager/Views/Billing/_PartialDetail_Insert_Billing.cshtml");
            }
        }

        // Change
        [HttpPost]
        [Route("danh-sach-billing/change-other-fee")]
        public ActionResult Change_Others_Fee(string p_case_code, decimal p_amount, decimal p_ShowPopUp)
        {
            try
            {
                List<Billing_Detail_Info> _lst_billing_detail = Get_LstFee_Detail(p_case_code);

                foreach (Billing_Detail_Info item in _lst_billing_detail)
                {
                    if (item.Type != Convert.ToDecimal(Common.CommonData.CommonEnums.Billing_Detail_Type.Others)) continue;

                    item.Service_Fee = p_amount;
                    item.Total_Fee = item.Nation_Fee + item.Represent_Fee + item.Service_Fee;
                }

                SessionData.SetDataSession(p_case_code, _lst_billing_detail);
                ViewBag.List_Billing = _lst_billing_detail;

                ViewBag.App_Case_Code = p_case_code;
                ViewBag.ShowPopUp = p_ShowPopUp;
                string _Currency_Type = (string)SessionData.GetDataSession(p_case_code + "_CURRENCY_TYPE");
                if (_Currency_Type != null)
                {
                    ViewBag.Currency_Type = _Currency_Type;
                }

                if (p_case_code.Contains("SEARCH"))
                {
                    return PartialView("~/Areas/Manager/Views/Billing_Search/_PartialDetail_Insert_Billing.cshtml");
                }
                else
                {
                    return PartialView("~/Areas/Manager/Views/Billing/_PartialDetail_Insert_Billing.cshtml");
                }

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/Manager/Views/Billing/_PartialDetail_Insert_Billing.cshtml");
            }
        }

        // Change
        [HttpPost]
        [Route("danh-sach-billing/change-represent-fee")]
        public ActionResult Change_Represent_Fee(string p_case_code, decimal p_billing_detail_id, decimal p_amount, decimal p_ShowPopUp)
        {
            try
            {
                List<Billing_Detail_Info> _lst_billing_detail = Get_LstFee_Detail(p_case_code);

                foreach (Billing_Detail_Info item in _lst_billing_detail)
                {
                    if (item.Billing_Detail_Id != p_billing_detail_id) continue;

                    //if (item.Type != Convert.ToDecimal(Common.CommonData.CommonEnums.Billing_Detail_Type.Search)) continue;

                    item.Represent_Fee = p_amount;
                    item.Total_Fee = item.Nation_Fee + item.Represent_Fee + item.Service_Fee;
                }

                SessionData.SetDataSession(p_case_code, _lst_billing_detail);
                ViewBag.List_Billing = _lst_billing_detail;

                ViewBag.App_Case_Code = p_case_code;
                ViewBag.ShowPopUp = p_ShowPopUp;
                string _Currency_Type = (string)SessionData.GetDataSession(p_case_code + "_CURRENCY_TYPE");
                if (_Currency_Type != null)
                {
                    ViewBag.Currency_Type = _Currency_Type;
                }

                if (p_case_code.Contains("SEARCH"))
                {
                    return PartialView("~/Areas/Manager/Views/Billing_Search/_PartialDetail_Insert_Billing.cshtml");
                }
                else
                {
                    return PartialView("~/Areas/Manager/Views/Billing/_PartialDetail_Insert_Billing.cshtml");
                }

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/Manager/Views/Billing/_PartialDetail_Insert_Billing.cshtml");
            }
        }

        // edit
        [Route("danh-sach-billing/show-edit")]
        public ActionResult GetView2Edit(int p_id, string p_app_case_code)
        {
            try
            {
                Billing_BL _obj_bl = new Billing_BL();
                SearchObject_Header_Info SearchObject_Header_Info = new SearchObject_Header_Info();
                ApplicationHeaderInfo objAppHeaderInfo = new ApplicationHeaderInfo();
                List<Billing_Detail_Info> _lst_billing_detail = new List<Billing_Detail_Info>();

                Billing_Header_Info _Billing_Header_Info = new Billing_Header_Info();
                if (p_app_case_code.Contains("SEARCH"))
                {
                    _Billing_Header_Info = _obj_bl.Billing_Search_GetBy_Id(p_id, p_app_case_code, AppsCommon.GetCurrentLang(), ref SearchObject_Header_Info, ref _lst_billing_detail);

                    SessionData.SetDataSession(p_app_case_code + "_CURRENCY_TYPE", SearchObject_Header_Info.Currency_Type);
                    ViewBag.Currency_Type = SearchObject_Header_Info.Currency_Type;
                }
                else
                {
                    //_Billing_Header_Info = _obj_bl.Billing_GetBy_Code(case_code, AppsCommon.GetCurrentLang(), ref objAppHeaderInfo, ref _lst_billing_detail);
                    _Billing_Header_Info = _obj_bl.Billing_GetBy_Id(p_id, p_app_case_code, AppsCommon.GetCurrentLang(), ref objAppHeaderInfo, ref _lst_billing_detail);

                    SessionData.SetDataSession(p_app_case_code + "_CURRENCY_TYPE", objAppHeaderInfo.Currency_Type);
                    ViewBag.Currency_Type = objAppHeaderInfo.Currency_Type;
                }

                foreach (Billing_Detail_Info item in _lst_billing_detail)
                {
                    item.Total_Fee = item.Nation_Fee + item.Represent_Fee + item.Service_Fee;
                }

                ViewBag.List_Billing = _lst_billing_detail;
                SessionData.SetDataSession(p_app_case_code, _lst_billing_detail);
                ViewBag.Operator_Type = Convert.ToDecimal(Common.CommonData.CommonEnums.Operator_Type.Insert);
                ViewBag.Billing_Header_Info = _Billing_Header_Info;

                ViewBag.App_Case_Code = p_app_case_code;


                ViewBag.ShowPopUp = 0;

                if (p_app_case_code.Contains("SEARCH"))
                {
                    ViewBag.objSearch_HeaderInfo = SearchObject_Header_Info;
                    //return PartialView("~/Areas/Manager/Views/Billing_Search/_PartialEdit.cshtml", _Billing_Header_Info);
                    return PartialView("~/Areas/Manager/Views/Billing/_PartialEdit.cshtml", _Billing_Header_Info);
                }
                else
                {
                    ViewBag.objAppHeaderInfo = objAppHeaderInfo;
                    return PartialView("~/Areas/Manager/Views/Billing/_PartialEdit.cshtml", _Billing_Header_Info);
                }
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
                List<Billing_Detail_Info> _lst_billing_detail = Get_LstFee_Detail(p_Billing_Header_Info.App_Case_Code);
                if (p_Billing_Header_Info.Total_Amount == 0)
                {
                    return Json(new { success = "-2" });
                }

                Billing_BL _obj_bl = new Billing_BL();
                p_Billing_Header_Info.Modify_By = SessionData.CurrentUser.Username;
                p_Billing_Header_Info.Modify_Date = DateTime.Now;
                p_Billing_Header_Info.Currency_Rate = AppsCommon.Get_Currentcy_VCB();

                decimal _ck = 0;
                using (var scope = new TransactionScope())
                {
                    _ck = _obj_bl.Billing_Update(p_Billing_Header_Info);
                    if (_ck <= 0)
                    {
                        goto Commit_Transaction;
                    }

                    // xóa đi trước
                    if (_ck > 0)
                    {
                        _ck = _obj_bl.Billing_Delete_Detail(p_Billing_Header_Info.Billing_Id);
                        if (_ck <= 0)
                        {
                            goto Commit_Transaction;
                        }
                    }

                    // và insert lại sau
                    if (_lst_billing_detail.Count > 0)
                    {
                        _ck = _obj_bl.Billing_Detail_InsertBatch(_lst_billing_detail, p_Billing_Header_Info.Billing_Id);
                    }

                    //end
                    Commit_Transaction:
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

        // approve
        [Route("danh-sach-billing/show-approve-billing")]
        public ActionResult GetView2Approve(int id, string case_code)
        {
            try
            {
                Billing_BL _obj_bl = new Billing_BL();
                ApplicationHeaderInfo objAppHeaderInfo = new ApplicationHeaderInfo();
                SearchObject_Header_Info SearchObject_Header_Info = new SearchObject_Header_Info();
                List<Billing_Detail_Info> _lst_billing_detail = new List<Billing_Detail_Info>();
                Billing_Header_Info _Billing_Header_Info = new Billing_Header_Info();
                if (case_code.Contains("SEARCH"))
                {
                    _Billing_Header_Info = _obj_bl.Billing_Search_GetBy_Id(id, case_code, AppsCommon.GetCurrentLang(), ref SearchObject_Header_Info, ref _lst_billing_detail);
                }
                else
                {
                    _Billing_Header_Info = _obj_bl.Billing_GetBy_Id(id, case_code, AppsCommon.GetCurrentLang(), ref objAppHeaderInfo, ref _lst_billing_detail);
                }

                foreach (Billing_Detail_Info item in _lst_billing_detail)
                {
                    item.Total_Fee = item.Nation_Fee + item.Represent_Fee + item.Service_Fee;
                }

                ViewBag.Billing_Header_Info = _Billing_Header_Info;
                ViewBag.List_Billing = _lst_billing_detail;
                ViewBag.Operator_Type = Convert.ToDecimal(Common.CommonData.CommonEnums.Operator_Type.Approve);

                ViewBag.objSearch_HeaderInfo = SearchObject_Header_Info;
                ViewBag.objAppHeaderInfo = objAppHeaderInfo;
                return PartialView("~/Areas/Manager/Views/Billing/_PartialApprove.cshtml", _Billing_Header_Info);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/TimeSheet/Views/TimeSheet/_PartialApprove.cshtml");
            }

        }

        [HttpPost]
        [Route("danh-sach-billing/do-approve")]
        public ActionResult DoUpdateStatus(int p_id)
        {
            try
            {
                Billing_BL _obj_bl = new Billing_BL();
                var modifiedBy = SessionData.CurrentUser.Username;
                decimal _status = (decimal)CommonEnums.Billing_Status.Approved;
                decimal _result = _obj_bl.Billing_Update_Status(p_id, "", AppsCommon.GetCurrentLang(), _status, SessionData.CurrentUser.Username, DateTime.Now);
                return Json(new { success = _result });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = -1 });
            }
        }

        [HttpPost]
        [Route("danh-sach-billing/do-reject")]
        public ActionResult DoReject(int p_id, string p_reject_reason)
        {
            try
            {
                Billing_BL _obj_bl = new Billing_BL();
                var modifiedBy = SessionData.CurrentUser.Username;
                decimal _status = (decimal)CommonEnums.Billing_Status.Reject;
                decimal _result = _obj_bl.Billing_Update_Status(p_id, p_reject_reason, AppsCommon.GetCurrentLang(), _status, SessionData.CurrentUser.Username, DateTime.Now);
                return Json(new { success = _result });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = -1 });
            }
        }

        [HttpPost]
        [Route("danh-sach-billing/change-date")]
        public ActionResult ChangeDate(string p_date)
        {
            try
            {
                DateTime _dt = ConvertData.ConvertString2Date(p_date);
                return Json(new { success = _dt.AddMonths(1).ToString("dd/MM/yyyy") });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = DateTime.Now.AddMonths(1).ToString("dd/MM/yyyy") });
            }
        }

        List<Billing_Detail_Info> Get_LstFee_Detail(string p_case_code)
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
                    item.Total_Fee = item.Nation_Fee + item.Represent_Fee + item.Service_Fee;
                }

                return _lst_billing_detail;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<Billing_Detail_Info>();
            }
        }

        // change pay status
        [Route("danh-sach-billing/show-change-status-pay")]
        public ActionResult GetView2ChangePayStatus(int id, string case_code)
        {
            try
            {
                Billing_BL _obj_bl = new Billing_BL();
                ApplicationHeaderInfo objAppHeaderInfo = new ApplicationHeaderInfo();
                SearchObject_Header_Info SearchObject_Header_Info = new SearchObject_Header_Info();
                List<Billing_Detail_Info> _lst_billing_detail = new List<Billing_Detail_Info>();
                Billing_Header_Info _Billing_Header_Info = new Billing_Header_Info();
                if (case_code.Contains("SEARCH"))
                {
                    _Billing_Header_Info = _obj_bl.Billing_Search_GetBy_Id(id, case_code, AppsCommon.GetCurrentLang(), ref SearchObject_Header_Info, ref _lst_billing_detail);
                }
                else
                {
                    _Billing_Header_Info = _obj_bl.Billing_GetBy_Id(id, case_code, AppsCommon.GetCurrentLang(), ref objAppHeaderInfo, ref _lst_billing_detail);
                }
                foreach (Billing_Detail_Info item in _lst_billing_detail)
                {
                    item.Total_Fee = item.Nation_Fee + item.Represent_Fee + item.Service_Fee;
                }

                ViewBag.Billing_Header_Info = _Billing_Header_Info;
                ViewBag.List_Billing = _lst_billing_detail;
                ViewBag.Operator_Type = Convert.ToDecimal(Common.CommonData.CommonEnums.Operator_Type.Approve);

                ViewBag.objSearch_HeaderInfo = SearchObject_Header_Info;
                ViewBag.objAppHeaderInfo = objAppHeaderInfo;

                return PartialView("~/Areas/Manager/Views/Billing/_PartialChangePayStatus.cshtml", _Billing_Header_Info);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/TimeSheet/Views/TimeSheet/_PartialChangePayStatus.cshtml");
            }

        }

        [HttpPost]
        [Route("danh-sach-billing/do-change-pay-status")]
        public ActionResult DoUpdatePayStatus(int p_id)
        {
            try
            {
                Billing_BL _obj_bl = new Billing_BL();
                var modifiedBy = SessionData.CurrentUser.Username;
                decimal _status = (decimal)CommonEnums.Billing_Pay_Status.Paid;
                decimal _result = _obj_bl.Billing_Update_Pay_Status(p_id, AppsCommon.GetCurrentLang(), _status, SessionData.CurrentUser.Username, DateTime.Now);
                return Json(new { success = _result });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = -1 });
            }
        }


        // action todo
        [Route("danh-sach-billing/show-action-billing-by-code")]
        public ActionResult GetView2Actiom_Biling_ByCode(string case_code)
        {
            try
            {
                Billing_BL _obj_bl = new Billing_BL();
                ApplicationHeaderInfo objAppHeaderInfo = new ApplicationHeaderInfo();
                SearchObject_Header_Info SearchObject_Header_Info = new SearchObject_Header_Info();
                List<Billing_Detail_Info> _lst_billing_detail = new List<Billing_Detail_Info>();
                Billing_Header_Info _Billing_Header_Info = new Billing_Header_Info();
                if (case_code.Contains("SEARCH"))
                {
                    _Billing_Header_Info = _obj_bl.Billing_Search_GetBy_Code(case_code, AppsCommon.GetCurrentLang(), ref SearchObject_Header_Info, ref _lst_billing_detail);
                }
                else
                {
                    _Billing_Header_Info = _obj_bl.Billing_GetBy_Code(case_code, AppsCommon.GetCurrentLang(), ref objAppHeaderInfo, ref _lst_billing_detail);
                    if (_Billing_Header_Info.Billing_Type == (decimal)CommonEnums.Billing_Type.Search)
                    {
                        _Billing_Header_Info = _obj_bl.Billing_Search_GetBy_Code(case_code, AppsCommon.GetCurrentLang(), ref SearchObject_Header_Info, ref _lst_billing_detail);
                    }
                }

                ViewBag.Currency_Type = objAppHeaderInfo.Currency_Type;
                SessionData.SetDataSession(case_code + "_CURRENCY_TYPE", objAppHeaderInfo.Currency_Type);

                foreach (Billing_Detail_Info item in _lst_billing_detail)
                {
                    item.Total_Fee = item.Nation_Fee + item.Represent_Fee + item.Service_Fee;
                }

                if (_Billing_Header_Info.Status == (decimal)CommonEnums.Billing_Status.New_Wait_Approve)
                {

                    ViewBag.Billing_Header_Info = _Billing_Header_Info;
                    ViewBag.List_Billing = _lst_billing_detail;
                    ViewBag.Operator_Type = Convert.ToDecimal(Common.CommonData.CommonEnums.Operator_Type.Approve);

                    ViewBag.objSearch_HeaderInfo = SearchObject_Header_Info;
                    ViewBag.objAppHeaderInfo = objAppHeaderInfo;

                    ViewBag.App_Case_Code = _Billing_Header_Info.App_Case_Code;

                    return PartialView("~/Areas/Manager/Views/Billing/_PartialApprove.cshtml", _Billing_Header_Info);
                }
                else if (_Billing_Header_Info.Status == (decimal)CommonEnums.Billing_Status.Approved)
                {
                    ViewBag.Billing_Header_Info = _Billing_Header_Info;
                    ViewBag.List_Billing = _lst_billing_detail;
                    ViewBag.Operator_Type = Convert.ToDecimal(Common.CommonData.CommonEnums.Operator_Type.Approve);

                    ViewBag.objSearch_HeaderInfo = SearchObject_Header_Info;
                    ViewBag.objAppHeaderInfo = objAppHeaderInfo;

                    return PartialView("~/Areas/Manager/Views/Billing/_PartialChangePayStatus.cshtml", _Billing_Header_Info);
                }
                else
                {
                    ViewBag.Billing_Header_Info = _Billing_Header_Info;
                    ViewBag.List_Billing = _lst_billing_detail;
                    ViewBag.Operator_Type = Convert.ToDecimal(Common.CommonData.CommonEnums.Operator_Type.View);

                    ViewBag.objSearch_HeaderInfo = SearchObject_Header_Info;
                    ViewBag.objAppHeaderInfo = objAppHeaderInfo;

                    return PartialView("~/Areas/Manager/Views/Billing/_PartialView.cshtml", _Billing_Header_Info);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/Manager/Views/Billing/_PartialView.cshtml", new Billing_Header_Info());
            }
        }

        [HttpPost]
        [Route("danh-sach-billing/do-export-billing")]
        public ActionResult do_export_billing(string p_case_code)
        {
            try
            {
                string _fileName = AppsCommon.Export_Billing(p_case_code);
                return Json(new { success = _fileName });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = "-1" });
            }
        }

        //string Export_Billing(string p_case_code)
        //{
        //    try
        //    {
        //        Billing_BL _obj_bl = new Billing_BL();
        //        ApplicationHeaderInfo _ApplicationHeaderInfo = new ApplicationHeaderInfo();
        //        List<Billing_Detail_Info> _lst_billing_detail = new List<Billing_Detail_Info>();
        //        Billing_Header_Info _Billing_Header_Info = _obj_bl.Billing_GetBy_Code(p_case_code, AppsCommon.GetCurrentLang(), ref _ApplicationHeaderInfo, ref _lst_billing_detail);
        //        foreach (Billing_Detail_Info item in _lst_billing_detail)
        //        {
        //            item.Total_Fee = item.Nation_Fee + item.Represent_Fee + item.Service_Fee;
        //        }

        //        string _fileTemp = System.Web.HttpContext.Current.Server.MapPath("/Content/Report/Biling_Report.doc");
        //        //if (_ApplicationHeaderInfo.Customer_Country != Common.Common.Country_VietNam_Id)
        //        //    _fileTemp = System.Web.HttpContext.Current.Server.MapPath("/Content/Report/Biling_Report_EN.doc");
        //        DocumentModel document = DocumentModel.Load(_fileTemp);

        //        // Fill export_header
        //        string fileName_exp = "/Content/Export/" + "Biling_Report" + p_case_code + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
        //        string fileName_exp_doc = "/Content/Export/" + "Biling_Report" + p_case_code + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";

        //        string fileName = System.Web.HttpContext.Current.Server.MapPath(fileName_exp);
        //        string fileName_doc = System.Web.HttpContext.Current.Server.MapPath(fileName_exp_doc);

        //        document.MailMerge.FieldMerging += (sender, e) =>
        //        {
        //            if (e.IsValueFound)
        //            {
        //                if (e.FieldName == "Text")
        //                    ((Run)e.Inline).Text = e.Value.ToString();
        //            }
        //        };

        //        document.MailMerge.Execute(new { DateNo = DateTime.Now.ToString("dd-MM-yyyy") });
        //        document.MailMerge.Execute(new { Case_Name = _ApplicationHeaderInfo.Case_Name });
        //        document.MailMerge.Execute(new { Client_Reference = _ApplicationHeaderInfo.Client_Reference });
        //        document.MailMerge.Execute(new { Case_Code = _ApplicationHeaderInfo.Case_Code });
        //        document.MailMerge.Execute(new { Master_Name = _ApplicationHeaderInfo.Master_Name });
        //        document.MailMerge.Execute(new { App_No = _ApplicationHeaderInfo.App_No });
        //        document.MailMerge.Execute(new { Customer_Country_Name = _ApplicationHeaderInfo.Customer_Country_Name });
        //        document.MailMerge.Execute(new { Bill_Code = _Billing_Header_Info.Case_Code });

        //        document.MailMerge.Execute(new { Total_Amount = _Billing_Header_Info.Total_Amount.ToString("#,##0.##") });
        //        document.MailMerge.Execute(new { Total_Pre_Tex = _Billing_Header_Info.Total_Pre_Tex.ToString("#,##0.##") });
        //        document.MailMerge.Execute(new { Tex_Fee = _Billing_Header_Info.Tex_Fee.ToString("#,##0.##") });
        //        document.MailMerge.Execute(new { Currency = _Billing_Header_Info.Currency });

        //        document.MailMerge.Execute(new { Deadline = _Billing_Header_Info.Deadline.ToString("dd/MM/yyyy") });
        //        document.MailMerge.Execute(new { Billing_Date = _Billing_Header_Info.Billing_Date.ToString("dd/MM/yyyy") });

        //        // lấy thông tin người dùng
        //        UserBL _UserBL = new UserBL();
        //        UserInfo userInfo = _UserBL.GetUserByUsername(_ApplicationHeaderInfo.Created_By);
        //        if (userInfo != null)
        //        {
        //            document.MailMerge.Execute(new { Contact_Person = userInfo.Contact_Person + " " + userInfo.FullName });
        //            document.MailMerge.Execute(new { Address = userInfo.Address });
        //            document.MailMerge.Execute(new { FullName = userInfo.FullName });
        //        }
        //        else
        //        {
        //            document.MailMerge.Execute(new { Contact_Person = "" });
        //            document.MailMerge.Execute(new { Address = "" });
        //            document.MailMerge.Execute(new { FullName = "" });
        //        }

        //        DataTable dtDetail = new DataTable();
        //        dtDetail = ConvertData.ConvertToDatatable<Billing_Detail_Info>(_lst_billing_detail);
        //        document.MailMerge.Execute(dtDetail, "TEMP");

        //        document.Save(fileName, SaveOptions.PdfDefault);
        //        //document.Save(fileName_doc, SaveOptions.DocxDefault);

        //        byte[] fileContents;
        //        var options = SaveOptions.PdfDefault;
        //        // Save document to DOCX format in byte array.
        //        using (var stream = new MemoryStream())
        //        {
        //            document.Save(stream, options);
        //            fileContents = stream.ToArray();
        //        }
        //        Convert.ToBase64String(fileContents);

        //        return fileName_exp;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.LogException(ex);
        //        return "";
        //    }
        //}

        [Route("Pre-View")]
        public ActionResult PreViewApplication(string p_filename)
        {
            try
            {
                ViewBag.FileName = p_filename;
                return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistration/_PartialContentPreview.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistration/_PartialContentPreview.cshtml");
            }
        }

        //public static void Insert_Docketing(string p_app_case_code, string p_doc_name, string p_url_File_Atachment)
        //{
        //    try
        //    {
        //        // insert vào docking để lưu trữ
        //        Docking_BL _obj_docBL = new Docking_BL();
        //        Docking_Info p_Docking_Info = new Docking_Info();
        //        p_Docking_Info.Created_By = SessionData.CurrentUser.Username;
        //        p_Docking_Info.Created_Date = DateTime.Now;
        //        p_Docking_Info.Language_Code = AppsCommon.GetCurrentLang();
        //        p_Docking_Info.Status = (decimal)CommonEnums.Docking_Status.Completed;
        //        p_Docking_Info.Docking_Type = (decimal)CommonEnums.Docking_Type_Enum.In_Book;
        //        p_Docking_Info.Document_Type = (decimal)CommonEnums.Document_Type_Enum.Khac;
        //        p_Docking_Info.Document_Name = p_doc_name;
        //        p_Docking_Info.In_Out_Date = DateTime.Now;
        //        p_Docking_Info.Isshowcustomer = 1;
        //        p_Docking_Info.App_Case_Code = p_app_case_code;

        //        string[] _arr = p_url_File_Atachment.Split('/');
        //        if (_arr.Length > 0)
        //        {
        //            p_Docking_Info.FileName = _arr[_arr.Length - 1];
        //        }
        //        p_Docking_Info.Url = p_url_File_Atachment;

        //        _obj_docBL.Docking_Insert(p_Docking_Info);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.LogException(ex);
        //    }
        //}

        [Route("danh-sach-billing/check-exits-billing")]
        public ActionResult Check_Exits_Billing(string p_app_case_code, decimal p_type)
        {
            try
            {
                Application_Header_BL _bl_app = new Application_Header_BL();
                ApplicationHeaderInfo _ApplicationHeaderInfo = _bl_app.GetApp_By_Case_Code(p_app_case_code);

                // 0 advise filling
                if (p_type == 0)
                {
                    if (_ApplicationHeaderInfo != null && _ApplicationHeaderInfo.Billing_Id_Advise > 0)
                    {
                        return Json(new { success = _ApplicationHeaderInfo.Billing_Id_Advise });
                    }
                }
                else
                {
                    // lấy ở app notice ra
                    App_Notice_Info_BL _bl = new App_Notice_Info_BL();
                    App_Notice_Info _App_Notice_Info = _bl.App_Notice_GetBy_CaseCode(p_app_case_code, p_type);
                    if (_App_Notice_Info != null && _App_Notice_Info.Billing_Id > 0)
                    {
                        return Json(new { success = _App_Notice_Info.Billing_Id });
                    }
                }

                return Json(new { success = 0 });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = 0 });
            }
        }

        [Route("danh-sach-billing/get-view-to-popup-insert")]
        public ActionResult Get_View_To_Popup_Insert(string p_case_code, decimal p_type)
        {
            try
            {
                List<Billing_Detail_Info> _lst_billing_detail = AppsCommon.Get_LstFee_Detail(p_case_code);
                if (_lst_billing_detail.Count > 0)
                {
                    foreach (Billing_Detail_Info item in _lst_billing_detail)
                    {
                        item.Total_Fee = item.Nation_Fee + item.Represent_Fee + item.Service_Fee;
                    }

                    string _Currency_Type = (string)SessionData.GetDataSession(p_case_code + "_CURRENCY_TYPE");
                    if (_Currency_Type != null)
                    {
                        ViewBag.Currency_Type = _Currency_Type;
                    }
                }
                else
                {
                    // nếu chưa có phí
                    Billing_BL _obj_bl = new Billing_BL();
                    ApplicationHeaderInfo objAppHeaderInfo = new ApplicationHeaderInfo();
                    SearchObject_Header_Info SearchObject_Header_Info = new SearchObject_Header_Info();
                    Billing_Header_Info _Billing_Header_Info = new Billing_Header_Info();
                    if (p_case_code.Contains("SEARCH"))
                    {
                        SearchObject_BL _bl = new SearchObject_BL();
                        SearchObject_Header_Info = _bl.GetBilling_By_Case_Code(p_case_code, SessionData.CurrentUser.Username,
                          AppsCommon.GetCurrentLang(), ref _lst_billing_detail);

                        ViewBag.objSearch_HeaderInfo = SearchObject_Header_Info;
                        if (SearchObject_Header_Info == null)
                        {
                            return Json(new { success = -1 });
                        }

                        SessionData.SetDataSession(p_case_code + "_CURRENCY_TYPE", SearchObject_Header_Info.Currency_Type);
                        ViewBag.Currency_Type = SearchObject_Header_Info.Currency_Type;
                    }
                    else
                    {
                        Application_Header_BL _bl = new Application_Header_BL();
                        objAppHeaderInfo = _bl.GetBilling_ByAppCase_Code(p_case_code, SessionData.CurrentUser.Username, AppsCommon.GetCurrentLang(), ref _lst_billing_detail);
                        ViewBag.objAppHeaderInfo = objAppHeaderInfo;

                        if (objAppHeaderInfo == null)
                        {
                            return Json(new { success = -1 });
                        }

                        // chỉ lấy những thằng nào mà > đã nộp đơn lên cục
                        if (objAppHeaderInfo != null && objAppHeaderInfo.Status < (decimal)Common.CommonData.CommonEnums.App_Status.DaNopDon)
                        {
                            return Json(new { success = -2 });
                        }

                        SessionData.SetDataSession(p_case_code + "_CURRENCY_TYPE", objAppHeaderInfo.Currency_Type);
                        ViewBag.Currency_Type = objAppHeaderInfo.Currency_Type;
                    }

                    // chi phí khác
                    Billing_Detail_Info _ChiPhiKhac = new Billing_Detail_Info();
                    _ChiPhiKhac.Nation_Fee = 0;
                    _ChiPhiKhac.Represent_Fee = 0;
                    _ChiPhiKhac.Service_Fee = 0;
                    _ChiPhiKhac.Biling_Detail_Name = "Chi phí khác";
                    _ChiPhiKhac.Type = Convert.ToDecimal(Common.CommonData.CommonEnums.Billing_Detail_Type.Others);
                    _lst_billing_detail.Add(_ChiPhiKhac);

                    foreach (Billing_Detail_Info item in _lst_billing_detail)
                    {
                        item.Total_Fee = item.Nation_Fee + item.Represent_Fee + item.Service_Fee;
                    }

                    SessionData.SetDataSession(p_case_code, _lst_billing_detail);
                }

                ViewBag.List_Billing = _lst_billing_detail;
                ViewBag.Operator_Type = Convert.ToDecimal(Common.CommonData.CommonEnums.Operator_Type.Insert);
                ViewBag.ShowPopUp = 1;
                ViewBag.App_Case_Code = p_case_code;

                if (p_case_code.Contains("SEARCH"))
                {
                    return PartialView("~/Areas/Manager/Views/Billing_Search/_PartialDetail_Insert_Billing.cshtml");

                    //var Partial_AppInfo = AppsCommon.RenderRazorViewToString(this.ControllerContext, "~/Areas/Manager/Views/Billing_Search/_Partial_SearchInfo.cshtml");
                    //var PartialDetail_Insert_Billing = AppsCommon.RenderRazorViewToString(this.ControllerContext, "~/Areas/Manager/Views/Billing_Search/_PartialDetail_Insert_Billing.cshtml");
                    //var json = Json(new { success = 1, PartialDetail_Insert_Billing });
                    //return json;
                }
                else
                {
                    return PartialView("~/Areas/Manager/Views/Billing/_PartialDetail_Insert_Billing.cshtml");

                    //var Partial_AppInfo = AppsCommon.RenderRazorViewToString(this.ControllerContext, "~/Areas/Manager/Views/Billing/_Partial_AppInfo.cshtml");
                    //var PartialDetail_Insert_Billing = AppsCommon.RenderRazorViewToString(this.ControllerContext, "~/Areas/Manager/Views/Billing/_PartialDetail_Insert_Billing.cshtml");
                    //var json = Json(new { success = 1, PartialDetail_Insert_Billing });
                    //return json;
                }

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = 0 });
            }
        }
    }
}
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
    [RouteArea("Manager", AreaPrefix = "quan-ly-billing-search")]
    [Route("{action}")]
    public class Billing_SearchController : Controller
    {
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
                return View("~/Areas/Manager/Views/Billing_Search/_PartialInsert.cshtml", _Billing_Header_Info);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);

                Billing_Header_Info _Billing_Header_Info = new Billing_Header_Info();
                return PartialView("~/Areas/Manager/Views/Billing_Search/_PartialInsert.cshtml", new Billing_Header_Info());
            }
        }

        [HttpGet]
        [Route("danh-sach-billing/show-insert-by-casecode")]
        public ActionResult GetView2InsertByCaseCode(string p_case_code)
        {
            try
            {
                Billing_BL _obj_bl = new Billing_BL();
                string _caseCode = _obj_bl.Billing_GenCaseCode();
                Billing_Header_Info _Billing_Header_Info = new Billing_Header_Info();
                _Billing_Header_Info.Case_Code = _caseCode;
                ViewBag.Case_Code = p_case_code;
                return View("~/Areas/Manager/Views/Billing_Search/_PartialInsert.cshtml", _Billing_Header_Info);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);

                Billing_Header_Info _Billing_Header_Info = new Billing_Header_Info();
                return PartialView("~/Areas/Manager/Views/Billing_Search/_PartialInsert.cshtml", new Billing_Header_Info());
            }
        }

        [HttpPost]
        [Route("GetAppByCaseCode")]
        public ActionResult GetInfoByCaseCode(string p_case_code)
        {
            try
            {
                SearchObject_BL _bl = new SearchObject_BL();
                List<Billing_Detail_Info> _lst_billing_detail = new List<Billing_Detail_Info>();
                SearchObject_Header_Info objSearch_HeaderInfo = _bl.GetBilling_By_Case_Code(p_case_code, SessionData.CurrentUser.Username,
                    AppsCommon.GetCurrentLang(), ref _lst_billing_detail);
                ViewBag.objSearch_HeaderInfo = objSearch_HeaderInfo;

                if (objSearch_HeaderInfo == null)
                {
                    return Json(new { success = -1 });
                }

                // chỉ lấy những thằng nào mà > đã nộp đơn lên cục
                //if (objSearch_HeaderInfo != null && objSearch_HeaderInfo.Status < (decimal)Common.CommonData.CommonEnums.App_Status.DaNopDon)
                //{
                //    return Json(new { success = -2 });
                //}

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
                SessionData.SetDataSession(p_case_code + "_CURRENCY_TYPE", objSearch_HeaderInfo.Currency_Type);

                ViewBag.List_Billing = _lst_billing_detail;
                ViewBag.Operator_Type = Convert.ToDecimal(Common.CommonData.CommonEnums.Operator_Type.Insert);
                ViewBag.App_Case_Code = p_case_code;
                ViewBag.Currency_Type = objSearch_HeaderInfo.Currency_Type;
                ViewBag.ShowPopUp = 0;

                var Partial_AppInfo = AppsCommon.RenderRazorViewToString(this.ControllerContext, "~/Areas/Manager/Views/Billing_Search/_Partial_SearchInfo.cshtml");
                var PartialDetail_Insert_Billing = AppsCommon.RenderRazorViewToString(this.ControllerContext, "~/Areas/Manager/Views/Billing_Search/_PartialDetail_Insert_Billing.cshtml");

                var json = Json(new { success = 1, Partial_AppInfo, PartialDetail_Insert_Billing });
                return json;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/TradeMark/Views/Shared/_PartialThongTinChuDon.cshtml");
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
                return new List<Billing_Detail_Info>();
            }
        }

        [HttpPost]
        [Route("do-insert-billing")]
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
                p_Billing_Header_Info.Billing_Type = (decimal)CommonEnums.Billing_Type.Search;
                p_Billing_Header_Info.Currency_Rate = AppsCommon.Get_Currentcy_VCB();
                decimal _ck = 0;
                decimal _billing_id = 0;
                using (var scope = new TransactionScope())
                {
                    _billing_id = _obj_bl.Billing_Insert(p_Billing_Header_Info);

                    if (_billing_id > 0 && _lst_billing_detail.Count > 0)
                    {
                        _ck = _obj_bl.Billing_Detail_InsertBatch(_lst_billing_detail, _billing_id);
                    }

                    if (_ck > 0 && p_Billing_Header_Info.Insert_Type == (decimal)Common.CommonData.CommonEnums.Billing_Insert_Type.Search)
                    {
                        string _fileExport = Export_Billing(p_Billing_Header_Info.Case_Code);
                        if (_fileExport == "") goto Commit_Transaction;

                        SearchObject_BL _bl = new SearchObject_BL();
                        _ck = _bl.Update_Url_Billing(p_Billing_Header_Info.App_Case_Code, _billing_id, _fileExport);

                        // insert vào docking
                        AppsCommon.Insert_Docketing(p_Billing_Header_Info.Case_Code, "Report Billing", _fileExport, true);
                    }

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

        [HttpPost]
        [Route("do-delete-billing-detail")]
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

                ViewBag.App_Case_Code = p_case_code;
                ViewBag.List_Billing = _lst_billing_detail;
                ViewBag.ShowPopUp = p_ShowPopUp;
                string _Currency_Type = (string)SessionData.GetDataSession(p_case_code + "_CURRENCY_TYPE");
                if (_Currency_Type != null)
                {
                    ViewBag.Currency_Type = _Currency_Type;
                }

                return PartialView("~/Areas/Manager/Views/Billing_Search/_PartialDetail_Insert_Billing.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/Manager/Views/Billing_Search/_PartialDetail_Insert_Billing.cshtml");
            }
        }

        // Change
        [HttpPost]
        [Route("change-other-fee")]
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

                return PartialView("~/Areas/Manager/Views/Billing_Search/_PartialDetail_Insert_Billing.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/Manager/Views/Billing_Search/_PartialDetail_Insert_Billing.cshtml");
            }
        }

        [HttpPost]
        [Route("danh-sach-billing/do-export-billing")]
        public ActionResult do_export_billing(string p_case_code)
        {
            try
            {
                string _fileName = Export_Billing(p_case_code);
                return Json(new { success = _fileName });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = "-1" });
            }
        }

        string Export_Billing(string p_case_code)
        {
            try
            {
                Billing_BL _obj_bl = new Billing_BL();
                SearchObject_Header_Info SearchObject_Header_Info = new SearchObject_Header_Info();
                List<Billing_Detail_Info> _lst_billing_detail = new List<Billing_Detail_Info>();
                Billing_Header_Info _Billing_Header_Info = _obj_bl.Billing_Search_GetBy_Code(p_case_code, AppsCommon.GetCurrentLang(), ref SearchObject_Header_Info, ref _lst_billing_detail);
                foreach (Billing_Detail_Info item in _lst_billing_detail)
                {
                    item.Total_Fee = item.Nation_Fee + item.Represent_Fee + item.Service_Fee;
                }

                string _fileTemp = System.Web.HttpContext.Current.Server.MapPath("/Content/Report/Biling_Search_Report.doc");
                //if (_ApplicationHeaderInfo.Customer_Country != Common.Common.Country_VietNam_Id)
                //    _fileTemp = System.Web.HttpContext.Current.Server.MapPath("/Content/Report/Biling_Report_EN.doc");
                DocumentModel document = DocumentModel.Load(_fileTemp);

                // Fill export_header
                string fileName_exp = "/Content/Export/" + "Biling_Search_Report" + p_case_code + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
                string fileName_exp_doc = "/Content/Export/" + "Biling_Search_Report" + p_case_code + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";

                string fileName = System.Web.HttpContext.Current.Server.MapPath(fileName_exp);
                string fileName_doc = System.Web.HttpContext.Current.Server.MapPath(fileName_exp_doc);

                document.MailMerge.FieldMerging += (sender, e) =>
                {
                    if (e.IsValueFound)
                    {
                        if (e.FieldName == "Text")
                            ((Run)e.Inline).Text = e.Value.ToString();
                    }
                };

                document.MailMerge.Execute(new { DateNo = DateTime.Now.ToString("dd-MM-yyyy") });
                document.MailMerge.Execute(new { Case_Name = SearchObject_Header_Info.CASE_NAME });
                document.MailMerge.Execute(new { Client_Reference = SearchObject_Header_Info.CLIENT_REFERENCE });
                document.MailMerge.Execute(new { Case_Code = SearchObject_Header_Info.CASE_CODE });
                document.MailMerge.Execute(new { Master_Name = SearchObject_Header_Info.Customer_Name });
                //document.MailMerge.Execute(new { App_No = SearchObject_Header_Info.App_No });
                document.MailMerge.Execute(new { Customer_Country_Name = SearchObject_Header_Info.Customer_Country_Name });
                document.MailMerge.Execute(new { Bill_Code = _Billing_Header_Info.Case_Code });

                document.MailMerge.Execute(new { Total_Amount = _Billing_Header_Info.Total_Amount.ToString("#,##0.##") });
                document.MailMerge.Execute(new { Total_Pre_Tex = _Billing_Header_Info.Total_Pre_Tex.ToString("#,##0.##") });
                document.MailMerge.Execute(new { Tex_Fee = _Billing_Header_Info.Tex_Fee.ToString("#,##0.##") });
                document.MailMerge.Execute(new { Currency = _Billing_Header_Info.Currency });

                document.MailMerge.Execute(new { Deadline = _Billing_Header_Info.Deadline.ToString("dd/MM/yyyy") });
                document.MailMerge.Execute(new { Billing_Date = _Billing_Header_Info.Billing_Date.ToString("dd/MM/yyyy") });

                // lấy thông tin người dùng
                UserBL _UserBL = new UserBL();
                UserInfo userInfo = _UserBL.GetUserByUsername(SearchObject_Header_Info.CREATED_BY);
                if (userInfo != null)
                {
                    document.MailMerge.Execute(new { Contact_Person = userInfo.Contact_Person + " " + userInfo.FullName });
                    document.MailMerge.Execute(new { Address = userInfo.Address });
                    document.MailMerge.Execute(new { FullName = userInfo.FullName });
                }
                else
                {
                    document.MailMerge.Execute(new { Contact_Person = "" });
                    document.MailMerge.Execute(new { Address = "" });
                    document.MailMerge.Execute(new { FullName = "" });
                }

                DataTable dtDetail = new DataTable();
                dtDetail = ConvertData.ConvertToDatatable<Billing_Detail_Info>(_lst_billing_detail);
                document.MailMerge.Execute(dtDetail, "TEMP");

                document.Save(fileName, SaveOptions.PdfDefault);
                //document.Save(fileName_doc, SaveOptions.DocxDefault);

                byte[] fileContents;
                var options = SaveOptions.PdfDefault;
                // Save document to DOCX format in byte array.
                using (var stream = new MemoryStream())
                {
                    document.Save(stream, options);
                    fileContents = stream.ToArray();
                }
                Convert.ToBase64String(fileContents);

                return fileName_exp;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return "";
            }
        }
    }
}
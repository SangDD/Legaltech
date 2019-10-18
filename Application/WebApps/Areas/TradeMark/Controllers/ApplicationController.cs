using System;
using System.Web.Mvc;
using WebApps.AppStart;
using BussinessFacade.ModuleTrademark;
using ObjectInfos.ModuleTrademark;
using System.Collections.Generic;
using WebApps.CommonFunction;
using WebApps.Session;
using Common;
using ObjectInfos;
using Common.CommonData;
using BussinessFacade;
using System.Web;
using GemBox.Document;
using BussinessFacade.ModuleUsersAndRoles;
using System.IO;
using System.Transactions;
using System.Linq;

namespace WebApps.Areas.TradeMark.Controllers
{
    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("Application", AreaPrefix = "trade-mark-mana")]
    [Route("{action}")]
    public class ApplicationController : Controller
    {
        #region Quản lý đơn lưu tạm
        [HttpGet]
        [Route("quan-ly-don")]
        public ActionResult Application_Display()
        {
            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("/");
                string language = AppsCommon.GetCurrentLang();
                decimal _total_record = 0;
                Application_Header_BL _obj_bl = new Application_Header_BL();
                string _status = "ALL";
                ViewBag.Status = _status;
                string _keySearch = "ALL|" + _status + "|ALL|ALL|" + language;
                List<ApplicationHeaderInfo> _lst = _obj_bl.ApplicationHeader_Search(SessionData.CurrentUser.Username, _keySearch, ref _total_record);
                string htmlPaging = WebApps.CommonFunction.AppsCommon.Get_HtmlPaging<ApplicationHeaderInfo>((int)_total_record, 1, "Đơn");
                ViewBag.Obj = _lst;
                ViewBag.Paging = htmlPaging;
                ViewBag.SumRecord = _total_record;
                return View("~/Areas/TradeMark/Views/Application/DanhSach_Don.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return View();
            }
        }

        [HttpPost]
        [Route("quan-ly-don/search")]
        public ActionResult Search_Application(string p_keysearch, int p_CurrentPage, string p_column, string p_type_sort)
        {
            try
            {
                decimal _total_record = 0;

                string p_to = "";
                string p_from = CommonFuc.Get_From_To_Page(p_CurrentPage, ref p_to);
                string language = AppsCommon.GetCurrentLang();
                Application_Header_BL _obj_bl = new Application_Header_BL();
                List<ApplicationHeaderInfo> _lst = _obj_bl.ApplicationHeader_Search(SessionData.CurrentUser.Username, p_keysearch + "|" + language, ref _total_record, p_from, p_to);
                string htmlPaging = WebApps.CommonFunction.AppsCommon.Get_HtmlPaging<ApplicationHeaderInfo>((int)_total_record, p_CurrentPage, "Đơn");

                ViewBag.Paging = htmlPaging;
                ViewBag.Obj = _lst;
                ViewBag.SumRecord = _total_record;
                return PartialView("~/Areas/TradeMark/Views/Application/_PartialTableApplication.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/TradeMark/Views/Application/_PartialTableApplication.cshtml");
            }
        }

        [HttpPost]
        [Route("quan-ly-don/show-phan-loai")]
        public ActionResult GetViewToPhanLoai(decimal p_application_header_id)
        {
            ViewBag.Application_Header_Id = p_application_header_id;
            return PartialView("~/Areas/TradeMark/Views/Application/_PartialPhanLoai.cshtml");
        }

        [HttpPost]
        [Route("quan-ly-don/do-phan-loai")]
        public ActionResult DoAddAppLawer(App_Lawer_Info p_App_Lawer_Info)
        {
            try
            {
                p_App_Lawer_Info.Language_Code = AppsCommon.GetCurrentLang();
                p_App_Lawer_Info.Created_By = SessionData.CurrentUser.Username;
                p_App_Lawer_Info.Created_Date = DateTime.Now;
                p_App_Lawer_Info.ReGrant = 0;
                App_Lawer_BL _con = new App_Lawer_BL();
                decimal _ck = _con.App_Lawer_Insert(p_App_Lawer_Info, (decimal)CommonEnums.App_Status.DaPhanChoLuatSu);
                return Json(new { success = _ck });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = "-1" });
            }
        }

        [HttpPost]
        [Route("quan-ly-don/do-re-grant-advice")]
        public ActionResult DoReAddAppLawer(App_Lawer_Info p_App_Lawer_Info)
        {
            try
            {
                p_App_Lawer_Info.Language_Code = AppsCommon.GetCurrentLang();
                p_App_Lawer_Info.Created_By = SessionData.CurrentUser.Username;
                p_App_Lawer_Info.Created_Date = DateTime.Now;
                p_App_Lawer_Info.ReGrant = 1;
                App_Lawer_BL _con = new App_Lawer_BL();
                decimal _ck = _con.App_Lawer_Insert(p_App_Lawer_Info, (decimal)CommonEnums.App_Status.DaGuiLenCuc);
                return Json(new { success = _ck });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = "-1" });
            }
        }

        [HttpPost]
        [Route("quan-ly-don/do-phan-loai-employee")]
        public ActionResult AppHeader_Update_Employee(string p_case_code, decimal p_employee, string p_note)
        {
            try
            {
                Application_Header_BL _obj_bl = new Application_Header_BL();
                int _ck = _obj_bl.AppHeader_Update_Employee(p_case_code, p_employee, p_note, SessionData.CurrentUser.Username, DateTime.Now, AppsCommon.GetCurrentLang());
                return Json(new { success = _ck });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = "-1" });
            }
        }


        [HttpPost]
        [Route("quan-ly-don/do-phan-loai-admin")]
        public ActionResult AppHeader_Update_Admin(string p_case_code, decimal p_admin, string p_note)
        {
            try
            {
                Application_Header_BL _obj_bl = new Application_Header_BL();
                int _ck = _obj_bl.AppHeader_Update_Admin(p_case_code, p_admin, p_note, SessionData.CurrentUser.Username, DateTime.Now, AppsCommon.GetCurrentLang());
                return Json(new { success = _ck });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = "-1" });
            }
        }

        [HttpPost]
        [Route("quan-ly-don/show-kh-confirm")]
        public ActionResult GetViewToCustomerConfirm(decimal p_application_header_id)
        {
            ViewBag.Application_Header_Id = p_application_header_id;
            return PartialView("~/Areas/TradeMark/Views/Application/_PartialKH_Confirm.cshtml");
        }

        [HttpPost]
        [Route("quan-ly-don/do-kh-confirm")]
        public ActionResult DoCustomer_Confirm(string p_case_code, decimal p_status, string p_note)
        {
            try
            {
                Application_Header_BL _obj_bl = new Application_Header_BL();
                decimal _status = (decimal)CommonEnums.App_Status.KhacHangDaConfirm;
                if (p_status == 0)
                    _status = (decimal)CommonEnums.App_Status.KhacHangDaTuChoi;

                int _ck = _obj_bl.AppHeader_Update_Status(p_case_code, _status, p_note, SessionData.CurrentUser.Username, DateTime.Now, AppsCommon.GetCurrentLang());
                return Json(new { success = _ck });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = "-1" });
            }
        }

        [HttpPost]
        [Route("quan-ly-don/show-lw-confirm")]
        public ActionResult GetViewToLawerConfirm(decimal p_application_header_id)
        {
            ViewBag.Application_Header_Id = p_application_header_id;
            return PartialView("~/Areas/TradeMark/Views/Application/_PartialLawer_Confirm.cshtml");
        }

        [HttpPost]
        [Route("quan-ly-don/do-lw-confirm")]
        public ActionResult DoLawer_Confirm(string p_case_code, decimal p_status, string p_note)
        {
            try
            {
                Application_Header_BL _obj_bl = new Application_Header_BL();
                decimal _status = (decimal)CommonEnums.App_Status.LuatSuDaConfirm;

                int _ck = _obj_bl.AppHeader_Update_Status(p_case_code, _status, p_note, SessionData.CurrentUser.Username, DateTime.Now, AppsCommon.GetCurrentLang());
                return Json(new { success = _ck });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = "-1" });
            }
        }

        [HttpPost]
        [Route("quan-ly-don/do-admin-confirm-2-customer")]
        public ActionResult do_admin_confirm_2_customer(string p_case_code, string p_note)
        {
            try
            {
                Application_Header_BL _obj_bl = new Application_Header_BL();
                decimal _status = (decimal)CommonEnums.App_Status.ChoKHConfirm;

                int _ck = _obj_bl.AppHeader_Update_Status(p_case_code, _status, p_note,
                    SessionData.CurrentUser.Username, DateTime.Now, AppsCommon.GetCurrentLang());
                return Json(new { success = _ck });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = "-1" });
            }
        }

        [HttpPost]
        [Route("quan-ly-don/do-admin-reject-2-lawer")]
        public ActionResult do_admin_reject_2_lawer(string p_case_code, string p_note)
        {
            try
            {
                Application_Header_BL _obj_bl = new Application_Header_BL();
                decimal _status = (decimal)CommonEnums.App_Status.AdminReject;

                int _ck = _obj_bl.AppHeader_Update_Status(p_case_code, _status, p_note,
                    SessionData.CurrentUser.Username, DateTime.Now, AppsCommon.GetCurrentLang());
                return Json(new { success = _ck });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = "-1" });
            }
        }


        [HttpPost]
        [Route("quan-ly-don/do-employee-confirm")]
        public ActionResult DoEmployee_Confirm(ApplicationHeaderInfo pInfo)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    Application_Header_BL _obj_bl = new Application_Header_BL();
                    decimal _status = (decimal)CommonEnums.App_Status.DaNopDon;
                    var url_File_Copy_Filing = AppLoadHelpers.PushFileToServer(pInfo.File_Copy_Filing, AppUpload.App);

                    string url_File_Translate_Filing = "";
                    if (pInfo.File_Translate_Filing != null)
                    {
                        url_File_Translate_Filing = AppLoadHelpers.PushFileToServer(pInfo.File_Translate_Filing, AppUpload.App);
                    }

                    decimal _ck = _obj_bl.Employee_Update_Status(pInfo.Case_Code, _status, pInfo.App_No, url_File_Copy_Filing, url_File_Translate_Filing, pInfo.Note,
                        pInfo.Filing_Date, pInfo.Expected_Accept_Date,
                        SessionData.CurrentUser.Username, DateTime.Now, AppsCommon.GetCurrentLang());

                    if (_ck >= 0)
                    {
                        AppsCommon.Insert_Docketing(pInfo.Case_Code, "File Copy Filing", url_File_Copy_Filing);
                        AppsCommon.Insert_Docketing(pInfo.Case_Code, "File Translate Filing", url_File_Translate_Filing);
                    }
                    else
                    {
                        goto Commit_Transaction;
                    }

                    Commit_Transaction:
                    if (_ck < 0)
                    {
                        Transaction.Current.Rollback();
                    }
                    else
                    {
                        scope.Complete();
                    }
                    return Json(new { success = _ck });
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = "-1" });
            }
        }

        [HttpPost]
        [Route("quan-ly-don/do-send-result-to-customer")]
        public ActionResult DoSendRult2Customer(decimal p_app_id, string p_case_code, string p_note)
        {
            try
            {
                Application_Header_BL _obj_bl = new Application_Header_BL();
                decimal _status = (decimal)CommonEnums.App_Status.AdminGuiKetQuaNopDon;

                int _ck = _obj_bl.AppHeader_Update_Status(p_case_code, _status, p_note, SessionData.CurrentUser.Username, DateTime.Now, AppsCommon.GetCurrentLang());

                // nếu thành công thì gửi email cho khách hàng
                if (_ck != -1)
                {
                    // lấy thông tin đơn
                    Application_Header_BL _Application_Header_BL = new Application_Header_BL();
                    ApplicationHeaderInfo _ApplicationHeaderInfo = _Application_Header_BL.GetApplicationHeader_ById(p_app_id, AppsCommon.GetCurrentLang()); 

                    //// lấy thông tin người dùng
                    //UserBL _UserBL = new UserBL();
                    //UserInfo userInfo = _UserBL.GetUserByUsername(_ApplicationHeaderInfo.Created_By);

                    string _emailTo = _ApplicationHeaderInfo.Email_Customer;
                    string _emailCC = "";
                    List<string> _LstAttachment = new List<string>();
                    if (_ApplicationHeaderInfo.Url_copy_filing != null && _ApplicationHeaderInfo.Url_copy_filing != "")
                    {
                        _LstAttachment.Add(System.Web.HttpContext.Current.Server.MapPath(_ApplicationHeaderInfo.Url_copy_filing));
                    }

                    if (_ApplicationHeaderInfo.URL_TRANSLATE_FILING != null && _ApplicationHeaderInfo.URL_TRANSLATE_FILING != "")
                    {
                        _LstAttachment.Add(System.Web.HttpContext.Current.Server.MapPath(_ApplicationHeaderInfo.URL_TRANSLATE_FILING));
                    }

                    if (_ApplicationHeaderInfo.Billing_Id_Advise > 0)
                    {
                        // lấy thông tin billing
                        Billing_BL _Billing_BL = new Billing_BL();
                        Billing_Header_Info _Billing_Header_Info = _Billing_BL.Billing_GetBy_Id(_ApplicationHeaderInfo.Billing_Id_Advise, AppsCommon.GetCurrentLang());
                        if (_Billing_Header_Info.Billing_Id > 0 && _Billing_Header_Info.Status == (decimal)CommonEnums.Billing_Status.Approved)
                        {
                            // kết xuất thông tin
                            string _mapPath_Report = Server.MapPath("~/Report/");
                            string _mapPath = Server.MapPath("~/");

                            string _fileName = AppsCommon.Export_Billing_Crytal_View(_Billing_Header_Info.Case_Code, _mapPath_Report, _mapPath);
                            _LstAttachment.Add(System.Web.HttpContext.Current.Server.MapPath(_fileName));
                        }
                    }

                    string _content = "";
                    List<AllCodeInfo> _lstStatus = WebApps.CommonFunction.AppsCommon.AllCode_GetBy_CdTypeCdName("EMAIL", "CONTENT");
                    _lstStatus = _lstStatus.OrderBy(x => x.CdVal).ToList();
                    if (_lstStatus.Count > 1)
                    {
                        _content = _lstStatus[0].Content + _ApplicationHeaderInfo.Comment_Filling.Replace("\n", "<br>") + _lstStatus[1].Content;
                    }

                    Email_Info _Email_Info = new Email_Info
                    {
                        EmailFrom = EmailHelper.EmailOriginal.EMailFrom_Business,
                        Pass = EmailHelper.EmailOriginal.PassWord_Business,
                        Display_Name = EmailHelper.EmailOriginal.DisplayName_Business,

                        EmailTo = _emailTo,
                        EmailCC = _emailCC,
                        Subject = "Filing advice",
                        Content = _content,
                        LstAttachment = _LstAttachment,
                    };

                    CommonFunction.AppsCommon.EnqueueSendEmail(_Email_Info);
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
        [Route("quan-ly-don/do-reject-result-to-customer")]
        public ActionResult DoRejectRult2Customer(decimal p_app_id, string p_case_code, string p_note)
        {
            try
            {
                Application_Header_BL _obj_bl = new Application_Header_BL();
                decimal _status = (decimal)CommonEnums.App_Status.AdminTuChoiKetQuaNopDon;
                int _ck = _obj_bl.AppHeader_Update_Status(p_case_code, _status, p_note, SessionData.CurrentUser.Username, DateTime.Now, AppsCommon.GetCurrentLang());
                return Json(new { success = _ck });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = "-1" });
            }
        }


        [HttpPost]
        [Route("quan-ly-don/show-filing")]
        public ActionResult Show_Filing(decimal p_application_header_id)
        {
            ViewBag.Application_Header_Id = p_application_header_id;
            return PartialView("~/Areas/TradeMark/Views/Application/_Partial_Filing.cshtml");
        }

        [HttpPost]
        [Route("quan-ly-don/do-filing")]
        public ActionResult do_Filing(ApplicationHeaderInfo pInfo)
        {
            try
            {
                List<Billing_Detail_Info> _lst_billing_detail = AppsCommon.Get_LstFee_Detail(pInfo.Case_Code);

                decimal _ck = 0;
                using (var scope = new TransactionScope())
                {
                    Application_Header_BL _obj_bl = new Application_Header_BL();
                    decimal _status = (decimal)CommonEnums.App_Status.DaGuiLenCuc;
                    //var url_File_Copy_Filing = AppLoadHelpers.PushFileToServer(pInfo.File_Copy_Filing, AppUpload.App);
                    //var url_File_Translate_Filing = AppLoadHelpers.PushFileToServer(pInfo.File_Translate_Filing, AppUpload.App);

                    _ck = _obj_bl.AppHeader_Filing_Status(pInfo.Case_Code, _status, pInfo.App_No, pInfo.Filing_Date, pInfo.Expected_Accept_Date,
                      pInfo.Note, pInfo.Comment_Filling, SessionData.CurrentUser.Username, DateTime.Now, AppsCommon.GetCurrentLang());

                    if (_ck < 0)
                    {
                        goto Commit_Transaction;
                    }

                    // insert billing
                    if (_lst_billing_detail.Count == 0)
                    {
                        goto Commit_Transaction;
                    }

                    Billing_BL _Billing_BL = new Billing_BL();
                    Billing_Header_Info p_Billing_Header_Info = new Billing_Header_Info();
                    p_Billing_Header_Info.Created_By = SessionData.CurrentUser.Username;
                    p_Billing_Header_Info.Created_Date = DateTime.Now;
                    p_Billing_Header_Info.Language_Code = AppsCommon.GetCurrentLang();
                    p_Billing_Header_Info.Status = (decimal)CommonEnums.Billing_Status.New_Wait_Approve;
                    p_Billing_Header_Info.Billing_Type = (decimal)CommonEnums.Billing_Type.App;
                    p_Billing_Header_Info.Notes = "Billing for case code " + pInfo.Case_Code + " - " + pInfo.Note;

                    p_Billing_Header_Info.Case_Code = _Billing_BL.Billing_GenCaseCode();
                    p_Billing_Header_Info.App_Case_Code = pInfo.Case_Code;

                    p_Billing_Header_Info.Billing_Date = DateTime.Now;
                    p_Billing_Header_Info.Deadline = DateTime.Now.AddDays(30);

                    p_Billing_Header_Info.Request_By = SessionData.CurrentUser.Username;
                    p_Billing_Header_Info.Approve_By = "";

                    p_Billing_Header_Info.Insert_Type = (decimal)Common.CommonData.CommonEnums.Billing_Insert_Type.Advise_Filling;
                    p_Billing_Header_Info.Currency_Rate = AppsCommon.Get_Currentcy_VCB();
                    if (p_Billing_Header_Info.App_Case_Code.Contains("SEARCH"))
                        p_Billing_Header_Info.Billing_Type = (decimal)CommonEnums.Billing_Type.Search;

                    decimal _Total_Amount_Represent = 0;
                    decimal _Total_Amount_Temp = 0;
                    decimal _Percent_discount = 0;
                    List<AllCodeInfo> _lstDiscount = WebApps.CommonFunction.AppsCommon.AllCode_GetBy_CdTypeCdName("DISCOUNT", "SERVICE");
                    if (_lstDiscount.Count > 0)
                    {
                        _Percent_discount = Convert.ToDecimal(_lstDiscount[0].CdVal);
                    }

                    foreach (Billing_Detail_Info item in _lst_billing_detail)
                    {
                        _Total_Amount_Represent = _Total_Amount_Represent + item.Represent_Fee;
                        _Total_Amount_Temp = _Total_Amount_Temp + item.Total_Fee;
                    }

                    decimal _discount = Math.Round(_Total_Amount_Represent * _Percent_discount / 100);
                    p_Billing_Header_Info.Total_Pre_Tex = _Total_Amount_Temp - _discount;
                    p_Billing_Header_Info.Tex_Fee = Math.Round(p_Billing_Header_Info.Total_Pre_Tex / 100 * Common.Common.Tax);
                    p_Billing_Header_Info.Total_Amount = p_Billing_Header_Info.Total_Pre_Tex + p_Billing_Header_Info.Tex_Fee;
                    p_Billing_Header_Info.Discount_Fee_Service = _discount;
                    p_Billing_Header_Info.Percent_Discount = _Percent_discount;

                    decimal _idBilling = _Billing_BL.Billing_Insert(p_Billing_Header_Info);
                    if (_idBilling < 0)
                    {
                        _ck = -1;
                        goto Commit_Transaction;
                    }

                    if (_idBilling > 0 && _lst_billing_detail.Count > 0)
                    {
                        _ck = _Billing_BL.Billing_Detail_InsertBatch(_lst_billing_detail, _idBilling);

                        if (_ck < 0)
                        {
                            goto Commit_Transaction;
                        }
                    }

                    //string _fileExport = AppsCommon.Export_Billing(p_Billing_Header_Info.Case_Code);
                    string _mapPath_Report = Server.MapPath("~/Report/");
                    string _mapPath = Server.MapPath("~/");

                    string _fileExport = AppsCommon.Export_Billing_Crytal(p_Billing_Header_Info.Case_Code, _mapPath_Report, _mapPath, pInfo.Created_By, p_Billing_Header_Info, _lst_billing_detail);
                    if (_fileExport == "") goto Commit_Transaction;

                    Application_Header_BL _BL = new Application_Header_BL();
                    _ck = _BL.AppHeader_Update_Advise_Url_Billing(p_Billing_Header_Info.App_Case_Code, _idBilling, _fileExport);

                    // nếu kết xuất file thành công thì insert vào docking
                    AppsCommon.Insert_Docketing(p_Billing_Header_Info.App_Case_Code, "Report Billing", _fileExport, true);

                    //end
                    Commit_Transaction:
                    if (_ck < 0)
                    {
                        Transaction.Current.Rollback();
                    }
                    else
                    {
                        SessionData.RemoveDataSession(pInfo.Case_Code);
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
        [Route("quan-ly-don/do-customer-review-filing")]
        public ActionResult Do_Customer_Review(Customer_Review_Info pInfo)
        {
            try
            {
                Application_Header_BL _obj_bl = new Application_Header_BL();
                decimal _status = (decimal)CommonEnums.App_Status.Customer_Review;

                int _ck = _obj_bl.AppHeader_Update_Status(pInfo.Case_Code, _status, pInfo.Note,
                     SessionData.CurrentUser.Username, DateTime.Now, AppsCommon.GetCurrentLang());

                // insert file vào docketing
                if (_ck == 0)
                {
                    var url_File_Atachment = AppLoadHelpers.PushFileToServer(pInfo.File_Atachment, AppUpload.App);
                    AppsCommon.Insert_Docketing(pInfo.Case_Code, "Tài liệu đính kèm từ khách hàng", url_File_Atachment);
                }

                return Json(new { success = _ck });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = "-1" });
            }
        }


        void AddBilling2Notice(decimal p_insert_billing_type, ref App_Notice_Info pInfo)
        {
            try
            {
                string _url_billing = "";
                string _mapPath_Report = Server.MapPath("~/Report/");
                string _mapPath = Server.MapPath("~/");
                decimal _id_billing = AppsCommon.Insert_Billing_4Application(pInfo.Case_Code, pInfo.Note, p_insert_billing_type, _mapPath_Report, _mapPath, ref _url_billing);

                if (_id_billing > 0)
                {
                    pInfo.Billing_Url = _url_billing;
                    pInfo.Billing_Id = _id_billing;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        #endregion

        #region Thông báo hình thức

        [HttpPost]
        [Route("quan-ly-don/do-export-cv-auto")]
        public ActionResult do_export_cv_auto(string p_case_code, decimal p_Notice_Type, string p_advise_replies)
        {
            try
            {
                string _fileName = Export_CV_Auto(p_case_code, p_Notice_Type, p_advise_replies);
                return Json(new { success = _fileName });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = "-1" });
            }
        }

        string Export_CV_Auto(string p_case_code, decimal p_Notice_Type, string p_advise_replies)
        {
            try
            {
                App_Notice_Info_BL _bl = new App_Notice_Info_BL();
                App_Notice_Info _App_Notice_Info = _bl.App_Notice_GetBy_CaseCode(p_case_code, p_Notice_Type);

                Application_Header_BL _bl_app = new Application_Header_BL();
                ApplicationHeaderInfo _ApplicationHeaderInfo = _bl_app.GetApp_By_Case_Code(p_case_code);

                string _fileTemp = System.Web.HttpContext.Current.Server.MapPath("/Content/Report/CVForm.doc");
                DocumentModel document = DocumentModel.Load(_fileTemp);

                // Fill export_header
                string fileName_exp = "/Content/Export/" + "CVForm" + p_case_code + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
                string fileName_exp_doc = "/Content/Export/" + "CVForm" + p_case_code + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";

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

                // thông tin app
                document.MailMerge.Execute(new { Date_Now = "ngày " + DateTime.Now.Date.ToString() + " tháng " + DateTime.Now.Month.ToString() + " năm " + DateTime.Now.Year.ToString() });
                document.MailMerge.Execute(new { App_No = _ApplicationHeaderInfo.App_No });
                document.MailMerge.Execute(new { Filing_Date = _ApplicationHeaderInfo.Filing_Date.ToString("dd-MM-yyyy") });
                document.MailMerge.Execute(new { Master_Name = _ApplicationHeaderInfo.Master_Name });

                // thông tin trả lời
                document.MailMerge.Execute(new { Replies_Number = _App_Notice_Info.Replies_Number });
                document.MailMerge.Execute(new { Advise_Replies = _App_Notice_Info.Advise_Replies });


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

        [HttpPost]
        [Route("quan-ly-don/lawer-notice-form")]
        public ActionResult Do_Lawer_Notice_Form(App_Notice_Info pInfo)
        {
            try
            {
                pInfo.Notice_Type = (decimal)CommonEnums.Notice_Type.HinhThuc;
                pInfo.Status = (decimal)CommonEnums.Notice_Accept_Status.LuatSu_GuiChoAdminDuyet;

                decimal _status_app = (decimal)CommonEnums.App_Status.ChapNhan_ThongBaoHinhThuc;
                if (pInfo.Accept_Date == null || pInfo.Accept_Date.Date == DateTime.MinValue.Date)
                {
                    _status_app = (decimal)CommonEnums.App_Status.TuChoi_ThongBaoHinhThuc;
                    pInfo.Result = (decimal)CommonEnums.Notice_Result.TuChoi;
                }
                else
                {
                    pInfo.Result = (decimal)CommonEnums.Notice_Result.ChapNhan;
                }

                AddBilling2Notice((decimal)Common.CommonData.CommonEnums.Billing_Insert_Type.Accept_Form, ref pInfo);

                App_Notice_Info_BL _App_Notice_Info_BL = new App_Notice_Info_BL();
                decimal _ck = _App_Notice_Info_BL.App_Notice_Insert(pInfo, AppsCommon.GetCurrentLang());

                // update trạng thái đơn trước
                Application_Header_BL _obj_bl = new Application_Header_BL();
                _ck = _obj_bl.AppHeader_Update_Status(pInfo.Case_Code, _status_app, pInfo.Note,
                   SessionData.CurrentUser.Username, DateTime.Now, AppsCommon.GetCurrentLang());

                return Json(new { success = _ck });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = "-1" });
            }
        }

        [HttpPost]
        [Route("quan-ly-don/accept-admin-approve")]
        public ActionResult Accept_AdminApprove(string p_case_code, decimal p_Notice_Type, decimal p_status, string p_note)
        {
            try
            {
                App_Notice_Info_BL _obj_bl = new App_Notice_Info_BL();
                decimal _status = (decimal)CommonEnums.Notice_Accept_Status.Admin_DuyetGuiChoKhachHang;
                if (p_status != 1)
                {
                    _status = (decimal)CommonEnums.Notice_Accept_Status.Admin_TuchoiDuyet;
                }

                decimal _ck = _obj_bl.App_Notice_Review_Accept(p_case_code, p_Notice_Type, _status,
                    p_note, SessionData.CurrentUser.Username, AppsCommon.GetCurrentLang());

                // nếu thành công thì gửi email cho khách hàng
                if (_ck != -1)
                {
                    App_Notice_Info_BL _notice_BL = new App_Notice_Info_BL();
                    App_Notice_Info _App_Notice_Info = _notice_BL.App_Notice_GetBy_CaseCode(p_case_code);
                    if (_App_Notice_Info.Id > 0)
                    {
                        //// lấy thông tin người dùng
                        //UserBL _UserBL = new UserBL();
                        //UserInfo userInfo = _UserBL.GetUserByUsername(_App_Notice_Info.Customer);

                        string _emailTo = _App_Notice_Info.Email_Customer;
                        string _emailCC = "";
                        List<string> _LstAttachment = new List<string>();

                        if (_App_Notice_Info.Notice_Url != null && _App_Notice_Info.Notice_Url != "")
                        {
                            _LstAttachment.Add(System.Web.HttpContext.Current.Server.MapPath(_App_Notice_Info.Notice_Url));
                        }

                        if (_App_Notice_Info.Notice_Trans_Url != null && _App_Notice_Info.Notice_Trans_Url != "")
                        {
                            _LstAttachment.Add(System.Web.HttpContext.Current.Server.MapPath(_App_Notice_Info.Notice_Trans_Url));
                        }

                        if (_App_Notice_Info.Billing_Id > 0)
                        {
                            // lấy thông tin billing
                            Billing_BL _Billing_BL = new Billing_BL();
                            Billing_Header_Info _Billing_Header_Info = _Billing_BL.Billing_GetBy_Id(_App_Notice_Info.Billing_Id, AppsCommon.GetCurrentLang());
                            if (_Billing_Header_Info.Billing_Id > 0 && _Billing_Header_Info.Status == (decimal)CommonEnums.Billing_Status.Approved)
                            {
                                // kết xuất thông tin
                                string _mapPath_Report = Server.MapPath("~/Report/");
                                string _mapPath = Server.MapPath("~/");

                                string _fileName = AppsCommon.Export_Billing_Crytal_View(_Billing_Header_Info.Case_Code, _mapPath_Report, _mapPath);
                                _LstAttachment.Add(System.Web.HttpContext.Current.Server.MapPath(_fileName));
                            }
                        }

                        string _content = "";
                        List<AllCodeInfo> _lstStatus = WebApps.CommonFunction.AppsCommon.AllCode_GetBy_CdTypeCdName("EMAIL", "CONTENT");
                        _lstStatus = _lstStatus.OrderBy(x => x.CdVal).ToList();
                        if (_lstStatus.Count > 1)
                        {
                            _content = _lstStatus[0].Content + _App_Notice_Info.Advise_Replies.Replace("\n", "<br>") + _lstStatus[1].Content;
                        }

                        Email_Info _Email_Info = new Email_Info
                        {
                            EmailFrom = EmailHelper.EmailOriginal.EMailFrom_Business,
                            Pass = EmailHelper.EmailOriginal.PassWord_Business,
                            Display_Name = EmailHelper.EmailOriginal.DisplayName_Business,

                            EmailTo = _emailTo,
                            EmailCC = _emailCC,
                            Subject = "V.v " + p_case_code,
                            Content = _content,
                            LstAttachment = _LstAttachment,
                        };

                        CommonFunction.AppsCommon.EnqueueSendEmail(_Email_Info);
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
        [Route("quan-ly-don/accept-customer-approve")]
        public ActionResult Accept_Customer_Approve(string p_case_code, decimal p_Notice_Type, decimal p_status, string p_note)
        {
            try
            {
                App_Notice_Info_BL _obj_bl = new App_Notice_Info_BL();
                decimal _status = (decimal)CommonEnums.Notice_Accept_Status.KhachHang_Review_TraLoi;

                decimal _ck = _obj_bl.App_Notice_Review_Accept(p_case_code, p_Notice_Type, _status,
                    p_note, SessionData.CurrentUser.Username, AppsCommon.GetCurrentLang());

                return Json(new { success = _ck });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = "-1" });
            }
        }

        [HttpPost]
        [Route("quan-ly-don/reject-admin-approve")]
        public ActionResult Reject_AdminApprove(string p_case_code, decimal p_Notice_Type, string p_advise_replies, string p_advise_replies_trans, decimal p_status, string p_note)
        {
            try
            {
                App_Notice_Info_BL _obj_bl = new App_Notice_Info_BL();
                decimal _status = (decimal)CommonEnums.Notice_Reject_Status.Admin_DuyetGuiChoKhachHang;
                if (p_status != 1)
                {
                    _status = (decimal)CommonEnums.Notice_Reject_Status.Admin_DuyetGuiChoKhachHang;
                }

                decimal _ck = _obj_bl.App_Notice_Review_Reject(p_case_code, p_Notice_Type, _status, p_advise_replies, p_advise_replies_trans,
                    p_note, SessionData.CurrentUser.Username, AppsCommon.GetCurrentLang(), DateTime.MinValue);

                return Json(new { success = _ck });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = "-1" });
            }
        }

        [HttpPost]
        [Route("quan-ly-don/reject-customer-approve")]
        public ActionResult Reject_CustomerApprove(string p_case_code, decimal p_Notice_Type, string p_advise_replies, string p_advise_replies_trans, string p_note)
        {
            try
            {
                App_Notice_Info_BL _obj_bl = new App_Notice_Info_BL();
                decimal _status = (decimal)CommonEnums.Notice_Reject_Status.KhachHang_Review_TraLoi;

                decimal _ck = _obj_bl.App_Notice_Review_Reject(p_case_code, p_Notice_Type, _status, p_advise_replies, p_advise_replies_trans,
                    p_note, SessionData.CurrentUser.Username, AppsCommon.GetCurrentLang(), DateTime.MinValue);

                return Json(new { success = _ck });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = "-1" });
            }
        }

        [HttpPost]
        [Route("quan-ly-don/reject-lawer-translate")]
        public ActionResult Reject_Lawer_Translate(string p_case_code, decimal p_Notice_Type, string p_advise_replies, string p_advise_replies_trans, DateTime p_replies_date, string p_note)
        {
            try
            {
                App_Notice_Info_BL _obj_bl = new App_Notice_Info_BL();
                decimal _status = (decimal)CommonEnums.Notice_Reject_Status.LuatSu_DichTraLoiCuc;

                decimal _ck = _obj_bl.App_Notice_Review_Reject(p_case_code, p_Notice_Type, _status, p_advise_replies, p_advise_replies_trans,
                    p_note, SessionData.CurrentUser.Username, AppsCommon.GetCurrentLang(), p_replies_date);

                // file công văn trả lời cục tự động hệ thống kết xuất ra
                string _fileName = Export_CV_Auto(p_case_code, p_Notice_Type, p_advise_replies);
                if (_fileName != "")
                {
                    // update vào thằng notice
                    _ck = _obj_bl.Update_CV_Auto(p_case_code, p_Notice_Type, _fileName, SessionData.CurrentUser.Username, AppsCommon.GetCurrentLang());

                    // insert vào docking
                    AppsCommon.Insert_Docketing(p_case_code, "CV Answer", _fileName, false);
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
        [Route("quan-ly-don/reject-admin-approve-translate")]
        public ActionResult Reject_Admin_Approve_Translate(string p_case_code, decimal p_Notice_Type, string p_advise_replies, string p_advise_replies_trans,
            decimal p_status, string p_note)
        {
            try
            {
                App_Notice_Info_BL _obj_bl = new App_Notice_Info_BL();
                decimal _status = (decimal)CommonEnums.Notice_Reject_Status.Admin_Duyet_Dich;
                if (p_status != 1)
                {
                    _status = (decimal)CommonEnums.Notice_Reject_Status.Admin_TuChoi_Dich;
                }

                decimal _ck = _obj_bl.App_Notice_Review_Reject(p_case_code, p_Notice_Type, _status, p_advise_replies, p_advise_replies_trans,
                    p_note, SessionData.CurrentUser.Username, AppsCommon.GetCurrentLang(), DateTime.MinValue);

                return Json(new { success = _ck });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = "-1" });
            }
        }

        [HttpPost]
        [Route("quan-ly-don/reject-lawer-update-deadline")]
        public ActionResult Reject_Lawer_Update_Deadline(string p_case_code, decimal p_Notice_Type, DateTime p_replies_date, DateTime p_next_deadline,
            HttpPostedFileBase p_url_scan_copy_cv, string p_note, string p_replies_url)
        {
            try
            {
                App_Notice_Info_BL _obj_bl = new App_Notice_Info_BL();
                decimal _status = (decimal)CommonEnums.Notice_Reject_Status.LuatSu_Update_Deadline;

                // file copy scan cv trả lời có dấu đỏ của cục
                // chưa làm, làm sau
                string _Replies_Url = "";
                if (p_url_scan_copy_cv != null)
                {
                    _Replies_Url = AppLoadHelpers.PushFileToServer(p_url_scan_copy_cv, AppUpload.App);
                }

                // nếu lấy từ docketing
                if (p_replies_url != "")
                {
                    _Replies_Url = p_replies_url;
                }

                decimal _ck = _obj_bl.Reject_Lawer_Update_Deadline(p_case_code, p_Notice_Type, p_replies_date, p_next_deadline, _Replies_Url, _status,
                    p_note, SessionData.CurrentUser.Username, AppsCommon.GetCurrentLang());

                return Json(new { success = _ck });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = "-1" });
            }
        }

        [HttpPost]
        [Route("quan-ly-don/reject-lawer-send-replies")]
        public ActionResult Reject_Lawer_Send_Replise(string p_case_code, decimal p_Notice_Type, string p_advise_replies, string p_advise_replies_trans,
            decimal p_status, string p_note)
        {
            try
            {
                App_Notice_Info_BL _obj_bl = new App_Notice_Info_BL();
                decimal _status = (decimal)CommonEnums.Notice_Reject_Status.LuatSu_Update_Deadline;
                decimal _ck = _obj_bl.App_Notice_Review_Reject(p_case_code, p_Notice_Type, _status, p_advise_replies, p_advise_replies_trans,
                    p_note, SessionData.CurrentUser.Username, AppsCommon.GetCurrentLang(), DateTime.MinValue);

                return Json(new { success = _ck });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = "-1" });
            }
        }

        #endregion

        #region công bố đơn
        [HttpPost]
        [Route("quan-ly-don/do-lawer-public-application")]
        public ActionResult Do_Lawer_Public_App(App_Notice_Info pInfo)
        {
            try
            {
                pInfo.Notice_Type = (decimal)CommonEnums.Notice_Type.CongBo_Don;
                pInfo.Status = (decimal)CommonEnums.Notice_Accept_Status.LuatSu_GuiChoAdminDuyet;

                decimal _status_app = (decimal)CommonEnums.App_Status.CongBoDon;
                if (pInfo.Accept_Date == null || pInfo.Accept_Date.Date == DateTime.MinValue.Date)
                {
                    pInfo.Result = (decimal)CommonEnums.Notice_Result.TuChoi;
                }
                else
                {
                    pInfo.Result = (decimal)CommonEnums.Notice_Result.ChapNhan;
                }

                AddBilling2Notice((decimal)Common.CommonData.CommonEnums.Billing_Insert_Type.Public_Form, ref pInfo);

                App_Notice_Info_BL _App_Notice_Info_BL = new App_Notice_Info_BL();
                decimal _ck = _App_Notice_Info_BL.App_Notice_Insert(pInfo, AppsCommon.GetCurrentLang());

                // update trạng thái đơn trước
                Application_Header_BL _obj_bl = new Application_Header_BL();
                _ck = _obj_bl.AppHeader_Update_Status(pInfo.Case_Code, _status_app, pInfo.Note,
                   SessionData.CurrentUser.Username, DateTime.Now, AppsCommon.GetCurrentLang());

                return Json(new { success = _ck });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = "-1" });
            }
        }

        #endregion

        #region thông báo nội dung
        [HttpPost]
        [Route("quan-ly-don/lawer-notice-content")]
        public ActionResult Do_Lawer_Notice_Content(App_Notice_Info pInfo)
        {
            try
            {
                pInfo.Notice_Type = (decimal)CommonEnums.Notice_Type.NoiDung;
                pInfo.Status = (decimal)CommonEnums.Notice_Accept_Status.LuatSu_GuiChoAdminDuyet;

                decimal _status_app = (decimal)CommonEnums.App_Status.ChapNhan_ThongBaoNoiDung;
                if (pInfo.Accept_Date == null || pInfo.Accept_Date.Date == DateTime.MinValue.Date)
                {
                    _status_app = (decimal)CommonEnums.App_Status.TuChoi_ThongBaoNoiDung;
                    pInfo.Result = (decimal)CommonEnums.Notice_Result.TuChoi;
                }
                else
                {
                    pInfo.Result = (decimal)CommonEnums.Notice_Result.ChapNhan;
                }

                AddBilling2Notice((decimal)Common.CommonData.CommonEnums.Billing_Insert_Type.Accept_Content, ref pInfo);

                App_Notice_Info_BL _App_Notice_Info_BL = new App_Notice_Info_BL();
                decimal _ck = _App_Notice_Info_BL.App_Notice_Insert(pInfo, AppsCommon.GetCurrentLang());

                // update trạng thái đơn trước
                Application_Header_BL _obj_bl = new Application_Header_BL();
                _ck = _obj_bl.AppHeader_Update_Status(pInfo.Case_Code, _status_app, pInfo.Note,
                   SessionData.CurrentUser.Username, DateTime.Now, AppsCommon.GetCurrentLang());


                return Json(new { success = _ck });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = "-1" });
            }
        }
        #endregion

        #region thông báo cấp bằng
        [HttpPost]
        [Route("quan-ly-don/lawer-grant-accept")]
        public ActionResult Do_Lawer_Grant_Accept(App_Notice_Info pInfo)
        {
            try
            {
                pInfo.Notice_Type = (decimal)CommonEnums.Notice_Type.ThongBao_Cap_Bang;
                pInfo.Status = (decimal)CommonEnums.Notice_Accept_Status.LuatSu_GuiChoAdminDuyet;
                decimal _status_app = (decimal)CommonEnums.App_Status.ThongBaoCapBang;

                if (pInfo.Accept_Date == null || pInfo.Accept_Date.Date == DateTime.MinValue.Date)
                {
                    pInfo.Result = (decimal)CommonEnums.Notice_Result.TuChoi;
                }
                else
                {
                    pInfo.Result = (decimal)CommonEnums.Notice_Result.ChapNhan;
                }

                // url billing
                AddBilling2Notice((decimal)Common.CommonData.CommonEnums.Billing_Insert_Type.Grant_Accept, ref pInfo);

                App_Notice_Info_BL _App_Notice_Info_BL = new App_Notice_Info_BL();
                decimal _ck = _App_Notice_Info_BL.App_Notice_Insert(pInfo, AppsCommon.GetCurrentLang());

                // update trạng thái đơn trước
                Application_Header_BL _obj_bl = new Application_Header_BL();
                _ck = _obj_bl.AppHeader_Update_Status(pInfo.Case_Code, _status_app, pInfo.Note,
                    SessionData.CurrentUser.Username, DateTime.Now, AppsCommon.GetCurrentLang());

                return Json(new { success = _ck });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = "-1" });
            }
        }
        #endregion

        #region công bố bằng
        [HttpPost]
        [Route("quan-ly-don/lawer-grant-public")]
        public ActionResult Do_Lawer_Grant_Public(App_Notice_Info pInfo)
        {
            try
            {
                pInfo.Notice_Type = (decimal)CommonEnums.Notice_Type.CongBo_Bang;
                pInfo.Status = (decimal)CommonEnums.Notice_Accept_Status.LuatSu_GuiChoAdminDuyet;
                decimal _status_app = (decimal)CommonEnums.App_Status.ThongBaoCapBang;

                if (pInfo.Accept_Date == null || pInfo.Accept_Date.Date == DateTime.MinValue.Date)
                {
                    pInfo.Result = (decimal)CommonEnums.Notice_Result.TuChoi;
                }
                else
                {
                    pInfo.Result = (decimal)CommonEnums.Notice_Result.ChapNhan;
                }

                // url billing
                AddBilling2Notice((decimal)Common.CommonData.CommonEnums.Billing_Insert_Type.Grant_Public, ref pInfo);

                App_Notice_Info_BL _App_Notice_Info_BL = new App_Notice_Info_BL();
                decimal _ck = _App_Notice_Info_BL.App_Notice_Insert(pInfo, AppsCommon.GetCurrentLang());

                // update trạng thái đơn trước
                Application_Header_BL _obj_bl = new Application_Header_BL();
                _ck = _obj_bl.AppHeader_Update_Status(pInfo.Case_Code, _status_app, pInfo.Note,
                   SessionData.CurrentUser.Username, DateTime.Now, AppsCommon.GetCurrentLang());

                return Json(new { success = _ck });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = "-1" });
            }
        }


        #endregion
    }
}
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
                List<ApplicationHeaderInfo> _lst = _obj_bl.ApplicationHeader_Search(_keySearch, ref _total_record);
                string htmlPaging = CommonFuc.Get_HtmlPaging<ApplicationHeaderInfo>((int)_total_record, 1, "Đơn");
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
                List<ApplicationHeaderInfo> _lst = _obj_bl.ApplicationHeader_Search(p_keysearch + "|" + language, ref _total_record, p_from, p_to);
                string htmlPaging = CommonFuc.Get_HtmlPaging<ApplicationHeaderInfo>((int)_total_record, p_CurrentPage, "Đơn");

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
        [Route("quan-ly-don/do-employee-confirm")]
        public ActionResult DoEmployee_Confirm(string p_case_code, decimal p_status, string p_note)
        {
            try
            {
                Application_Header_BL _obj_bl = new Application_Header_BL();
                decimal _status = (decimal)CommonEnums.App_Status.DaNopDon;

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

                    string _fileTemp = System.Web.HttpContext.Current.Server.MapPath("/Content/Report/Filing_advice.doc");
                    if (_ApplicationHeaderInfo.Customer_Country != Common.Common.Country_VietNam_Id)
                        _fileTemp = System.Web.HttpContext.Current.Server.MapPath("/Content/Report/Filing_advice_EN.doc");
                    DocumentModel document = DocumentModel.Load(_fileTemp);

                    // Fill export_header
                    string fileName = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "Filing_advice_" + p_app_id.ToString() + ".pdf");
                    document.MailMerge.FieldMerging += (sender, e) =>
                    {
                        if (e.IsValueFound)
                        {
                            if (e.FieldName == "Text")
                                ((Run)e.Inline).Text = e.Value.ToString();
                        }
                    };

                    document.MailMerge.Execute(new { DateNo = DateTime.Now.ToString("dd-MM-yyyy") });
                    document.MailMerge.Execute(new { Case_Name = _ApplicationHeaderInfo.Case_Name });
                    document.MailMerge.Execute(new { Client_Reference = _ApplicationHeaderInfo.Client_Reference });
                    document.MailMerge.Execute(new { Case_Code = _ApplicationHeaderInfo.Case_Code });
                    document.MailMerge.Execute(new { Master_Name = _ApplicationHeaderInfo.Master_Name });
                    document.MailMerge.Execute(new { App_No = _ApplicationHeaderInfo.App_No });
                    document.MailMerge.Execute(new { Str_Filing_Date = _ApplicationHeaderInfo.Str_Filing_Date });
                    document.MailMerge.Execute(new { Comment_filling = _ApplicationHeaderInfo.Comment_Filling });
                    document.MailMerge.Execute(new { Customer_Country_Name = _ApplicationHeaderInfo.Customer_Country_Name });

                    // lấy thông tin người dùng
                    UserBL _UserBL = new UserBL();
                    UserInfo userInfo = _UserBL.GetUserByUsername(_ApplicationHeaderInfo.Created_By);
                    if (userInfo != null)
                    {
                        document.MailMerge.Execute(new { Contact_Person = userInfo.Contact_Person });
                        document.MailMerge.Execute(new { Address = userInfo.Address });
                        document.MailMerge.Execute(new { FullName = userInfo.FullName });
                    }
                    else
                    {
                        document.MailMerge.Execute(new { Contact_Person = "" });
                        document.MailMerge.Execute(new { Address = "" });
                        document.MailMerge.Execute(new { FullName = "" });
                    }

                    document.Save(fileName, SaveOptions.PdfDefault);
                    byte[] fileContents;
                    var options = SaveOptions.PdfDefault;
                    // Save document to DOCX format in byte array.
                    using (var stream = new MemoryStream())
                    {
                        document.Save(stream, options);
                        fileContents = stream.ToArray();
                    }
                    Convert.ToBase64String(fileContents);

                    string _emailTo = userInfo.Email;
                    string _emailCC = userInfo.Copyto;
                    List<string> _LstAttachment = new List<string>();
                    _LstAttachment.Add(fileName);
                    _LstAttachment.Add(System.Web.HttpContext.Current.Server.MapPath(_ApplicationHeaderInfo.Url_copy_filing));
                    _LstAttachment.Add(System.Web.HttpContext.Current.Server.MapPath(_ApplicationHeaderInfo.URL_TRANSLATE_FILING));

                    if (_ApplicationHeaderInfo.Url_Billing != null && _ApplicationHeaderInfo.Url_Billing != "")
                    {
                        _LstAttachment.Add(System.Web.HttpContext.Current.Server.MapPath(_ApplicationHeaderInfo.Url_Billing));
                    }
                    EmailHelper.SendMail(_emailTo, _emailCC, "Filing advice", "Filing advice", _LstAttachment);
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
                Application_Header_BL _obj_bl = new Application_Header_BL();
                decimal _status = (decimal)CommonEnums.App_Status.DaGuiLenCuc;
                var url_File_Copy_Filing = AppLoadHelpers.PushFileToServer(pInfo.File_Copy_Filing, AppUpload.App);
                var url_File_Translate_Filing = AppLoadHelpers.PushFileToServer(pInfo.File_Translate_Filing, AppUpload.App);

                int _ck = _obj_bl.AppHeader_Filing_Status(pInfo.Case_Code, _status, pInfo.App_No, pInfo.Filing_Date, pInfo.Expected_Accept_Date, url_File_Copy_Filing, url_File_Translate_Filing,
                    pInfo.Note, pInfo.Comment_Filling, SessionData.CurrentUser.Username, DateTime.Now, AppsCommon.GetCurrentLang());

                Insert_Docketing(pInfo.Case_Code, "File Copy Filing", url_File_Copy_Filing);
                Insert_Docketing(pInfo.Case_Code, "File Translate Filing", url_File_Translate_Filing);

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
                    Insert_Docketing(pInfo.Case_Code, "Tài liệu đính kèm từ khách hàng", url_File_Atachment);
                }

                return Json(new { success = _ck });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = "-1" });
            }
        }

        public static void Insert_Docketing(string p_app_case_code, string p_doc_name, string p_url_File_Atachment, bool p_is_transaction = false)
        {
            try
            {
                // insert vào docking để lưu trữ
                Docking_BL _obj_docBL = new Docking_BL();
                Docking_Info p_Docking_Info = new Docking_Info();
                p_Docking_Info.Created_By = SessionData.CurrentUser.Username;
                p_Docking_Info.Created_Date = DateTime.Now;
                p_Docking_Info.Language_Code = AppsCommon.GetCurrentLang();
                p_Docking_Info.Status = (decimal)CommonEnums.Docking_Status.Completed;
                p_Docking_Info.Docking_Type = (decimal)CommonEnums.Docking_Type_Enum.In_Book;
                p_Docking_Info.Document_Type = (decimal)CommonEnums.Document_Type_Enum.Khac;
                p_Docking_Info.Document_Name = p_doc_name;
                p_Docking_Info.In_Out_Date = DateTime.Now;
                p_Docking_Info.Isshowcustomer = 1;
                p_Docking_Info.App_Case_Code = p_app_case_code;

                //
                string[] _arr = p_url_File_Atachment.Split('/');
                if (_arr.Length > 0)
                {
                    p_Docking_Info.FileName = _arr[_arr.Length - 1];
                }

                p_Docking_Info.Url = p_url_File_Atachment;

                if (p_is_transaction == false)
                {
                    _obj_docBL.Docking_Insert(p_Docking_Info);
                }
                else
                {
                    _obj_docBL.Docking_Insert_Transaction(p_Docking_Info);
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

                // update trạng thái đơn trước
                Application_Header_BL _obj_bl = new Application_Header_BL();
                decimal _ck = _obj_bl.AppHeader_Update_Status(pInfo.Case_Code, _status_app, pInfo.Note,
                     SessionData.CurrentUser.Username, DateTime.Now, AppsCommon.GetCurrentLang());

                if (_ck == 0)
                {
                    // Notice_Url 
                    var url_File_Atachment = "";
                    if (pInfo.File_Notice_Url != null)
                    {
                        url_File_Atachment = AppLoadHelpers.PushFileToServer(pInfo.File_Notice_Url, AppUpload.App);
                        pInfo.Notice_Url = url_File_Atachment;
                    }

                    // insert vào bảng thông báo
                    var url_File_AtachmentTrans = "";
                    if (pInfo.File_Notice_Trans_Url != null)
                    {
                        url_File_AtachmentTrans = AppLoadHelpers.PushFileToServer(pInfo.File_Notice_Trans_Url, AppUpload.App);
                        pInfo.Notice_Trans_Url = url_File_AtachmentTrans;
                    }

                    // url billing
                    string _keyUrlBilling = "BILLING_APP_" + ((decimal)Common.CommonData.CommonEnums.Billing_Insert_Type.Accept_Form).ToString();
                    string _url_billing = (string)SessionData.GetDataSession(_keyUrlBilling);
                    if (_url_billing != null && _url_billing != "")
                        pInfo.Biling_Url = _url_billing;

                    App_Notice_Info_BL _App_Notice_Info_BL = new App_Notice_Info_BL();
                    _ck = _App_Notice_Info_BL.App_Notice_Insert(pInfo, AppsCommon.GetCurrentLang());

                    // insert file vào docketing
                    if (_ck > 0)
                    {
                        Insert_Docketing(pInfo.Case_Code, "File scan accept form", url_File_Atachment);
                        Insert_Docketing(pInfo.Case_Code, "File transalte accept form", url_File_AtachmentTrans);
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
        [Route("quan-ly-don/accept-admin-approve")]
        public ActionResult Accept_AdminApprove_Form(string p_case_code, decimal p_status, string p_note)
        {
            try
            {
                App_Notice_Info_BL _obj_bl = new App_Notice_Info_BL();
                decimal _status = (decimal)CommonEnums.Notice_Accept_Status.Admin_DuyetGuiChoKhachHang;
                if (p_status != 1)
                {
                    _status = (decimal)CommonEnums.Notice_Accept_Status.Admin_DuyetGuiChoKhachHang;
                }

                decimal _ck = _obj_bl.App_Notice_Review_Accept(p_case_code, (decimal)CommonEnums.Notice_Type.HinhThuc, _status,
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
        [Route("quan-ly-don/accept-customer-approve")]
        public ActionResult Accept_Customer_Approve_Form(string p_case_code, decimal p_status, string p_note)
        {
            try
            {
                App_Notice_Info_BL _obj_bl = new App_Notice_Info_BL();
                decimal _status = (decimal)CommonEnums.Notice_Accept_Status.KhachHang_Review_TraLoi;

                decimal _ck = _obj_bl.App_Notice_Review_Accept(p_case_code, (decimal)CommonEnums.Notice_Type.HinhThuc, _status,
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
        public ActionResult Reject_AdminApprove_Form(string p_case_code, string p_advise_replies, string p_advise_replies_trans, decimal p_status, string p_note)
        {
            try
            {
                App_Notice_Info_BL _obj_bl = new App_Notice_Info_BL();
                decimal _status = (decimal)CommonEnums.Notice_Reject_Status.Admin_DuyetGuiChoKhachHang;
                if (p_status != 1)
                {
                    _status = (decimal)CommonEnums.Notice_Reject_Status.Admin_DuyetGuiChoKhachHang;
                }

                decimal _ck = _obj_bl.App_Notice_Review_Reject(p_case_code, (decimal)CommonEnums.Notice_Type.HinhThuc, _status, p_advise_replies, p_advise_replies_trans,
                    p_note, SessionData.CurrentUser.Username, AppsCommon.GetCurrentLang());

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
                pInfo.Notice_Type = (decimal)CommonEnums.Notice_Type.HinhThuc;
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

                // update trạng thái đơn trước
                Application_Header_BL _obj_bl = new Application_Header_BL();
                decimal _ck = _obj_bl.AppHeader_Update_Status(pInfo.Case_Code, _status_app, pInfo.Note,
                     SessionData.CurrentUser.Username, DateTime.Now, AppsCommon.GetCurrentLang());

                if (_ck == 0)
                {
                    // Notice_Url 
                    var url_File_Atachment = "";
                    if (pInfo.File_Notice_Url != null)
                    {
                        url_File_Atachment = AppLoadHelpers.PushFileToServer(pInfo.File_Notice_Url, AppUpload.App);
                        pInfo.Notice_Url = url_File_Atachment;
                    }

                    // insert vào bảng thông báo
                    var url_File_AtachmentTrans = "";
                    if (pInfo.File_Notice_Trans_Url != null)
                    {
                        url_File_AtachmentTrans = AppLoadHelpers.PushFileToServer(pInfo.File_Notice_Trans_Url, AppUpload.App);
                        pInfo.Notice_Trans_Url = url_File_AtachmentTrans;
                    }

                    // url billing
                    string _keyUrlBilling = "BILLING_APP_" + ((decimal)Common.CommonData.CommonEnums.Billing_Insert_Type.Grant_App).ToString();
                    string _url_billing = (string)SessionData.GetDataSession(_keyUrlBilling);
                    if (_url_billing != null && _url_billing != "")
                        pInfo.Biling_Url = _url_billing;

                    App_Notice_Info_BL _App_Notice_Info_BL = new App_Notice_Info_BL();
                    _ck = _App_Notice_Info_BL.App_Notice_Insert(pInfo, AppsCommon.GetCurrentLang());

                    // insert file vào docketing
                    if (_ck > 0)
                    {
                        Insert_Docketing(pInfo.Case_Code, "File trích xuất Thông báo SHCN", url_File_Atachment);
                        Insert_Docketing(pInfo.Case_Code, "File transalte", url_File_AtachmentTrans);
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
 
        #endregion
    }
}
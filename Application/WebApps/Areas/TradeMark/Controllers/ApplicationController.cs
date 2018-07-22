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
                    return Redirect("~/home/index");

                decimal _total_record = 0;
                Application_Header_BL _obj_bl = new Application_Header_BL();
                string _keySearch = "ALL|ALL";
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

                Application_Header_BL _obj_bl = new Application_Header_BL();
                List<ApplicationHeaderInfo> _lst = _obj_bl.ApplicationHeader_Search(p_keysearch, ref _total_record);
                string htmlPaging = CommonFuc.Get_HtmlPaging<ApplicationHeaderInfo>((int)_total_record, 1, "Đơn");

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
                App_Lawer_BL _con = new App_Lawer_BL();
                decimal _ck = _con.App_Lawer_Insert(p_App_Lawer_Info);
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
        public ActionResult DoCustomer_Confirm(decimal p_Application_Header_Id, decimal p_status, string p_note)
        {
            try
            {
                Application_Header_BL _obj_bl = new Application_Header_BL();
                decimal _status = (decimal)CommonEnums.App_Status.KhacHangDaConfirm;
                if (p_status == 0)
                    _status = (decimal)CommonEnums.App_Status.KhacHangDaTuChoi;

                int _ck = _obj_bl.AppHeader_Update_Status(p_Application_Header_Id, _status, p_note, SessionData.CurrentUser.Username, DateTime.Now);
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
        public ActionResult DoLawer_Confirm(decimal p_Application_Header_Id, decimal p_status, string p_note)
        {
            try
            {
                Application_Header_BL _obj_bl = new Application_Header_BL();
                decimal _status = (decimal)CommonEnums.App_Status.LuatSuDaConfirm;

                int _ck = _obj_bl.AppHeader_Update_Status(p_Application_Header_Id, _status, p_note, SessionData.CurrentUser.Username, DateTime.Now);
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
        public ActionResult do_admin_confirm_2_customer(decimal p_Application_Header_Id)
        {
            try
            {
                Application_Header_BL _obj_bl = new Application_Header_BL();
                decimal _status = (decimal)CommonEnums.App_Status.ChoKHConfirm;

                int _ck = _obj_bl.AppHeader_Update_Status(p_Application_Header_Id, _status, "", SessionData.CurrentUser.Username, DateTime.Now);
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
                //DateTime _filing_date = Common.Helpers.DateTimeHelper.ConvertToDate(p_filing_date);
                int _ck = _obj_bl.AppHeader_Filing_Status(pInfo.Id, _status, pInfo.App_No, pInfo.Filing_Date, url_File_Copy_Filing, pInfo.Note, SessionData.CurrentUser.Username, DateTime.Now);

                // nếu thành công thì gửi email cho khách hàng
                if (_ck != -1)
                {
                    // lấy thông tin đơn
                    Application_Header_BL _Application_Header_BL = new Application_Header_BL();
                    ApplicationHeaderInfo _ApplicationHeaderInfo = _Application_Header_BL.GetApplicationHeader_ById(pInfo.Id, AppsCommon.GetCurrentLang());

                    string _fileTemp = System.Web.HttpContext.Current.Server.MapPath("/Content/Report/Filing_advice.doc");
                    DocumentModel document = DocumentModel.Load(_fileTemp);

                    // Fill export_header
                    string fileName = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "Filing_advice_" + pInfo.Id.ToString() + ".pdf");
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
                    document.MailMerge.Execute(new { Gencode = _ApplicationHeaderInfo.Gencode });
                    document.MailMerge.Execute(new { Master_Name = _ApplicationHeaderInfo.Master_Name });
                    document.MailMerge.Execute(new { App_No = _ApplicationHeaderInfo.App_No });
                    document.MailMerge.Execute(new { Str_Filing_Date = _ApplicationHeaderInfo.Str_Filing_Date });

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
                    _LstAttachment.Add(System.Web.HttpContext.Current.Server.MapPath(url_File_Copy_Filing));

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

        #endregion
    }
}
namespace WebApps.Areas.TradeMark.Controllers
{
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
    using System.Web;
    using GemBox.Document;
    using System.IO;

    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("TradeMarkRegistration", AreaPrefix = "trade-mark")]
    [Route("{action}")]
    public class TradeMarkRegistrationController : Controller
    {
        // GET: TradeMark/TradeMarkRegistration

        [HttpGet]
        [Route("dang-ky-nhan-hieu")]
        public ActionResult DangKyNhanHieu()
        {
            try
            {
                if (SessionData.CurrentUser == null)
                {
                    return this.Redirect("/");
                }
                string language = AppsCommon.GetCurrentLang();
                ViewBag.lstData = SysApplicationBL.GetSysAppByLanguage(language);
                return View("~/Areas/TradeMark/Views/TradeMarkRegistration/DangKyNhanHieu.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex); return View("~/Areas/TradeMark/Views/TradeMarkRegistration/DangKyNhanHieu.cshtml");
            }
        }

        [HttpGet]
        [Route("sua-doi-don-dang-ky/{id}")]
        public ActionResult TradeMarkChoiseApplication()
        {
            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("/");

                string AppCode = "";
                if (RouteData.Values.ContainsKey("id"))
                {
                    AppCode = RouteData.Values["id"].ToString().ToUpper();
                }
                ViewBag.AppCode = AppCode;
                if (AppCode == TradeMarkAppCode.AppCodeSuaDoiDangKy)
                {
                    return AppSuaDoiDonDangKy();
                }
                else if (AppCode == TradeMarkAppCode.AppCodeDangKyChuyenDoi)
                {

                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return AppSuaDoiDonDangKy();
        }


        public ActionResult AppSuaDoiDonDangKy()
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistration/AppSuaDoiDonDangKy.cshtml");
        }

        [HttpPost]
        [Route("them_moi_don_dang_ky_sua_doi")]
        public ActionResult AppSuaDoiDonDangKyInsert(ApplicationHeaderInfo pInfo, List<AppFeeFixInfo> pFeeFixInfo, AppDetail01Info pDetailInfo)
        {
            try
            {
                Application_Header_BL objBL = new Application_Header_BL();
                AppFeeFixBL objFeeFixBL = new AppFeeFixBL();
                AppDetail01BL objDetail01BL = new AppDetail01BL();

                if (pInfo == null || pDetailInfo == null) return Json(new { status = ErrorCode.Error });
                string language = AppsCommon.GetCurrentLang();
                var CreatedBy = SessionData.CurrentUser.Username;
                var CreatedDate = SessionData.CurrentUser.CurrentDate;
                //
                pInfo.Languague_Code = language;
                pInfo.Created_By = CreatedBy;
                pInfo.Created_Date = CreatedDate;
                int pReturn = ErrorCode.Success;
                int pAppHeaderID = 0;
                //TRA RA ID CUA BANG KHI INSERT
                pAppHeaderID = objBL.AppHeaderInsert(pInfo);
                //Gán lại khi lấy dl 
                if (pAppHeaderID >= 0)
                {
                    pReturn = objFeeFixBL.AppFeeFixInsertBath(pFeeFixInfo, pAppHeaderID);
                }

                if (pReturn >= 0)
                {
                    pDetailInfo.Language_Code = language;
                    pDetailInfo.App_Header_Id = pAppHeaderID;
                    pReturn = objDetail01BL.AppDetailInsert(pDetailInfo);
                }




                return Json(new { status = pReturn });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);

                return Json(new { status = ErrorCode.Error });
            }
        }

        [HttpPost]
        [Route("push-file-to-server")]
        public ActionResult PushFileToServer(HttpPostedFileBase pfiles,string pLoaiTaiLieuDon)
        {
            try
            {
                if (pfiles != null)
                {
                    //lấy tên của file
                    var name = pfiles.FileName;
                    name = System.IO.Path.GetExtension(pfiles.FileName);
                    var f_part = HttpContext.Server.MapPath("~/Content/") + pfiles.FileName;
                    pfiles.SaveAs(f_part);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = -1 });
            }
            return Json(new { success = 0 });
        }


        [HttpPost]
        [Route("ket_xuat_file")]
        public ActionResult ExportData(ApplicationHeaderInfo pInfo, List<AppFeeFixInfo> pFeeFixInfo, AppDetail01Info pDetailInfo)
        {
            try
            {
                string _fileTemp = System.Web.HttpContext.Current.Server.MapPath("/Content/AppForms/Request_for_amendment_of_application.doc");
                DocumentModel document = DocumentModel.Load(_fileTemp);

                // Fill export_header
                string   fileName = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/"+ "Dang_ky_sua_doi_nhan_hieu_" + pInfo.Appcode + ".pdf");

                // Fill export_detail  

                pInfo.Status = 254;
                pInfo.Status_Form = 252;
                pInfo.Relationship = "11";
                document.MailMerge.Execute(pInfo);
                document.Save(fileName, SaveOptions.PdfDefault);
                //document.Save(fileName);


                byte[] fileContents;
                var options = SaveOptions.PdfDefault;
                // Save document to DOCX format in byte array.
                using (var stream = new MemoryStream())
                {
                    document.Save(stream, options);
                    fileContents = stream.ToArray();
                }
                  Convert.ToBase64String(fileContents);

                return Json(new { success = 0 });

            }
            catch (Exception ex)
            {

                return Json(new { success = 0 });
            }
        }
    }
}
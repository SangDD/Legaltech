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
    using System.Transactions;
    using BussinessFacade;
    using System.Web.Script.Serialization;
    using System.Data;
    using Common.Extensions;
    using CrystalDecisions.Shared;
    using System.Linq;
    using System.Drawing;
    using System.Collections;
    using Common.CommonData;

    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("TradeMarkRegistration", AreaPrefix = "trade-mark-01")]
    [Route("{action}")]
    public class TradeMarkRegistrationDKQTController : Controller
    {

        public static List<AppDocumentOthersInfo> lstDocOther = new List<AppDocumentOthersInfo>();

        /// <summary>
        /// ID:ID của app_header_id 
        /// ID2: là appcode 
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //[Route("request-for-trade-mark-edit/{id}/{id1}/{id2}")]
        //public ActionResult TradeMarkForEdit()
        //{
        //    decimal App_Header_Id = 0;
        //    string AppCode = "";
        //    int Status = 0;
        //    try
        //    {
        //        if (SessionData.CurrentUser == null)
        //            return Redirect("/");
        //        SessionData.CurrentUser.chashFile.Clear();
        //        SessionData.CurrentUser.chashFileOther.Clear();

        //        if (RouteData.Values.ContainsKey("id"))
        //        {
        //            App_Header_Id = CommonFuc.ConvertToDecimal(RouteData.Values["id"]);
        //        }
        //        if (RouteData.Values.ContainsKey("id1"))
        //        {
        //            Status = CommonFuc.ConvertToInt(RouteData.Values["id1"]);
        //        }
        //        if (RouteData.Values.ContainsKey("id2"))
        //        {
        //            AppCode = RouteData.Values["id2"].ToString().ToUpper();
        //        }

        //        if (AppCode == TradeMarkAppCode.AppCodeDangKyQuocTeNH)
        //        {
        //            return TradeMarkSuaDon(App_Header_Id, AppCode, Status);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.LogException(ex);
        //    }
        //    return TradeMarkSuaDon(App_Header_Id, AppCode, Status);
        //}

        //public ActionResult TradeMarkSuaDon(decimal pAppHeaderId, string pAppCode, int pStatus)
        //{
        //    if (pAppCode == TradeMarkAppCode.AppCodeDangKyQuocTeNH)
        //    {
        //        var objBL = new AppDetail06DKQT_BL();
        //        string language = AppsCommon.GetCurrentLang();
        //        var ds06Dkqt = objBL.AppTM06DKQTGetByID(pAppHeaderId, language, pStatus);
        //        if (ds06Dkqt != null && ds06Dkqt.Tables.Count == 3)
        //        {
        //            ViewBag.objAppHeaderInfo = CBO<App_Detail_TM06DKQT_Info>.FillObjectFromDataTable(ds06Dkqt.Tables[0]);
        //            ViewBag.Lst_AppDoc = CBO<AppDocumentInfo>.FillCollectionFromDataTable(ds06Dkqt.Tables[1]);
        //            ViewBag.lstClassDetailInfo = CBO<AppClassDetailInfo>.FillCollectionFromDataTable(ds06Dkqt.Tables[2]);
        //        }
        //        if (ds06Dkqt.Tables[0] == null || ds06Dkqt.Tables[0].Rows.Count == 0)
        //        {
        //            return Redirect("/trade-mark/request-for-trade-mark-view/" + pAppHeaderId.ToString() + "/" + pStatus.ToString()  + "/" + TradeMarkAppCode.AppCodeDangKyQuocTeNH);
        //        }
        //        AppDetail04NHBL _AppDetail04NHBL = new AppDetail04NHBL();
        //        List<AppDetail04NHInfo> _list04nh = new List<AppDetail04NHInfo>();
        //        // truyền vào trạng thái nào? để tạm thời = 1 cho có dữ liệu
        //        _list04nh = _AppDetail04NHBL.AppTM04NHSearchByStatus(1, language);
        //        ViewBag.ListAppDetail04NHInfo = _list04nh;
        //        return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistrationDKQT/_PartialEditDangKyNhanHieu.cshtml");
        //    }
        //    else
        //    {
        //        //
        //        return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistrationDKQT/_PartialEditDangKyNhanHieu.cshtml");
        //    }
        //}

        public ActionResult TradeMarkViewDon(decimal pAppHeaderId, string pAppCode, int pStatus)
        {
            if (pAppCode == TradeMarkAppCode.AppCodeDangKyQuocTeNH)
            {
                var objBL = new AppDetail06DKQT_BL();
                string language = AppsCommon.GetCurrentLang();
                var ds06Dkqt = objBL.AppTM06DKQTGetByID(pAppHeaderId, language, pStatus);
                if (ds06Dkqt != null && ds06Dkqt.Tables.Count == 3)
                {
                    ViewBag.objAppHeaderInfo = CBO<App_Detail_TM06DKQT_Info>.FillObjectFromDataTable(ds06Dkqt.Tables[0]);
                    ViewBag.Lst_AppDoc = CBO<AppDocumentInfo>.FillCollectionFromDataTable(ds06Dkqt.Tables[1]);
                    ViewBag.lstClassDetailInfo = CBO<AppClassDetailInfo>.FillCollectionFromDataTable(ds06Dkqt.Tables[2]);
                }
                return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistrationDKQT/_PartalViewDangKyNhanHieu.cshtml");
            }
            else
            {
                //
                return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistrationDKQT/_PartalViewDangKyNhanHieu.cshtml");
            }
        }



        [HttpGet]
        [Route("request-for-trade-mark/{id}")]
        public ActionResult TradeMarkChoiseApplication()
        {
            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("/");

                SessionData.CurrentUser.chashFile.Clear();
                SessionData.CurrentUser.chashFileOther.Clear();
                string AppCode = "";
                if (RouteData.Values.ContainsKey("id"))
                {
                    AppCode = RouteData.Values["id"].ToString().ToUpper();
                }
                ViewBag.AppCode = AppCode;
                if (AppCode == TradeMarkAppCode.AppCodeDangKyQuocTeNH)
                {
                    return AppDangKyNhanHieu();
                }
              
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return AppDangKyNhanHieu();
            
        }
        public ActionResult AppDangKyNhanHieu()
        {
            try
            { 
                AppDetail04NHBL _AppDetail04NHBL = new AppDetail04NHBL();
                List<AppDetail04NHInfo> _list04nh = new List<AppDetail04NHInfo>();
                // truyền vào trạng thái nào? để tạm thời = 7 là đã gửi lên cục
                _list04nh = _AppDetail04NHBL.AppTM04NHSearchByStatus(7,  AppsCommon.GetCurrentLang());
                ViewBag.ListAppDetail04NHInfo = _list04nh;

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistrationDKQT/_PartalDangKyNhanHieu.cshtml");
        }



        [HttpPost]
        [Route("dang_ky_nhan_hieu")]
        public ActionResult AppDonDangKyInsert(ApplicationHeaderInfo pInfo, App_Detail_TM06DKQT_Info pDetail, List<AppDocumentInfo> pAppDocumentInfo,
         List<AppDocumentOthersInfo> pAppDocOtherInfo, List<AppClassDetailInfo> pAppClassInfo, List<AppFeeFixInfo> pFeeFixInfo)
        {
            try
            {
                Application_Header_BL objBL = new Application_Header_BL();
                AppDetail06DKQT_BL objDetailBL = new AppDetail06DKQT_BL();
                AppClassDetailBL objClassDetail = new AppClassDetailBL();
                AppDocumentBL objDoc = new AppDocumentBL();
                AppFeeFixBL objFeeFixBL = new AppFeeFixBL();
                if (pInfo == null || pDetail == null) return Json(new { status = ErrorCode.Error });
                string language = AppsCommon.GetCurrentLang();
                var CreatedBy = SessionData.CurrentUser.Username;
                var CreatedDate = SessionData.CurrentUser.CurrentDate;
                int pReturn = ErrorCode.Success;
                int pAppHeaderID = 0;
                string p_case_code = "";

                foreach (AppFeeFixInfo item in pFeeFixInfo)
                {
                    if(item.Amount == 0)
                    {
                        // fix là 2 củ
                        item.Amount = 2000000;
                    }
                }
                using (var scope = new TransactionScope())
                {
                    //
                    pInfo.Languague_Code = language;
                    if (pInfo.Created_By == null || pInfo.Created_By == "0" || pInfo.Created_By == "")
                    {
                        pInfo.Created_By = CreatedBy;
                    }
                    pInfo.Created_Date = CreatedDate;
                    //TRA RA ID CUA BANG KHI INSERT
                    pAppHeaderID = objBL.AppHeaderInsert(pInfo, ref p_case_code);
                    //Gán lại khi lấy dl 
                    if (pAppHeaderID >= 0)
                    {
                        pReturn = objFeeFixBL.AppFeeFixInsertBath(pFeeFixInfo, p_case_code);
                    }
                    else
                    {
                        Transaction.Current.Rollback();
                    }
                    if (pReturn >= 0)
                    {
                        pDetail.Appcode = pInfo.Appcode;
                        pDetail.LANGUAGE_CODE = language;
                        pDetail.APP_HEADER_ID = pAppHeaderID;
                        if (pDetail.pfileLogo != null)
                        {
                            pDetail.LOGOURL = AppLoadHelpers.PushFileToServer(pDetail.pfileLogo, AppUpload.Logo);
                        }
                        pReturn = objDetailBL.App_Detail_06TMDKQT_Insert(pDetail);
                        //Thêm thông tin class
                        if (pReturn >= 0)
                        {
                            pReturn = objClassDetail.AppClassDetailInsertBatch(pAppClassInfo, pAppHeaderID, language);
                        }
                    }
                    //Tai lieu dinh kem 
                    if (pReturn >= 0 && pAppDocumentInfo != null)
                    {
                        if (pAppDocumentInfo.Count > 0)
                        {
                            foreach (var info in pAppDocumentInfo)
                            {
                                if (SessionData.CurrentUser.chashFile.ContainsKey(info.keyFileUpload))
                                {
                                    HttpPostedFileBase pfiles = (HttpPostedFileBase)SessionData.CurrentUser.chashFile[info.keyFileUpload];
                                    info.Filename = pfiles.FileName;
                                    info.Url_Hardcopy = "/Content/Archive/" + AppUpload.Document + "/" + pfiles.FileName;
                                    info.Status = 0;
                                }
                                info.App_Header_Id = pAppHeaderID;
                                info.Document_Filing_Date = CommonFuc.CurrentDate();
                                info.Language_Code = language;
                            }
                            pReturn = objDoc.AppDocumentInsertBath(pAppDocumentInfo, pAppHeaderID);

                        }
                    }
                    //tai lieu khac 
                    if (pReturn >= 0 && pAppDocOtherInfo != null)
                    {
                        if (pAppDocOtherInfo.Count > 0)
                        {
                            foreach (var info in pAppDocOtherInfo)
                            {
                                if (SessionData.CurrentUser.chashFileOther.ContainsKey(info.keyFileUpload))
                                {
                                    HttpPostedFileBase pfiles = (HttpPostedFileBase)SessionData.CurrentUser.chashFileOther[info.keyFileUpload];
                                    info.Filename = pfiles.FileName;
                                    info.Filename = "/Content/Archive/" + AppUpload.Document +"/" + pfiles.FileName;
                                }
                                info.App_Header_Id = pAppHeaderID;
                                info.Language_Code = language;
                            }
                            pReturn = objDoc.AppDocumentOtherInsertBatch(pAppDocOtherInfo);
                        }
                    }
                    //end
                    if (pReturn < 0)
                    {
                        Transaction.Current.Rollback();
                    }
                    else
                    {
                        scope.Complete();
                    }
                }

                return Json(new { status = pAppHeaderID });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { status = ErrorCode.Error });
            }
        }



        [HttpPost]
        [Route("sua-don-dang-ky")]
        public ActionResult Edit_TM06DKQT(ApplicationHeaderInfo pInfo, App_Detail_TM06DKQT_Info pDetail,
            List<AppDocumentInfo> pAppDocumentInfo, List<AppFeeFixInfo> pFeeFixInfo, List<AppClassDetailInfo> pAppClassInfo)
        {
            try
            {
                pDetail.Id = pInfo.Id;
                pDetail.APP_HEADER_ID = pInfo.Id;
                pDetail.LANGUAGE_CODE = pInfo.Languague_Code;
                Application_Header_BL objBL = new Application_Header_BL();
                AppFeeFixBL objFeeFixBL = new AppFeeFixBL();
                AppDetail06DKQT_BL objDetail = new AppDetail06DKQT_BL();
                AppDocumentBL objDoc = new AppDocumentBL();
                AppClassDetailBL objClassDetail = new AppClassDetailBL();
                
                if (pInfo == null || pDetail == null) return Json(new { status = ErrorCode.Error });
                string language = AppsCommon.GetCurrentLang();
                var CreatedBy = SessionData.CurrentUser.Username;
                var CreatedDate = SessionData.CurrentUser.CurrentDate;
                decimal pReturn = ErrorCode.Success;
                foreach (AppFeeFixInfo item in pFeeFixInfo)
                {
                    if (item.Amount == 0)
                    {
                        // fix là 2 củ
                        item.Amount = 2000000;
                    }
                }
                using (var scope = new TransactionScope())
                {
                    //
                    pInfo.Languague_Code = language;
                    pInfo.Created_By = CreatedBy;
                    pInfo.Created_Date = CreatedDate;
                    pInfo.Send_Date = DateTime.Now;
                    if (pDetail.pfileLogo != null)
                    {
                        pDetail.LOGOURL = AppLoadHelpers.PushFileToServer(pDetail.pfileLogo, AppUpload.Logo);
                    }
                    //TRA RA ID CUA BANG KHI INSERT
                    int _re = objBL.AppHeaderUpdate(pInfo);

                    // detail
                    if (_re >= 0)
                    {
                        pDetail.Appcode = pInfo.Appcode;
                        pDetail.LANGUAGE_CODE = language;
                        pDetail.APP_HEADER_ID = pInfo.Id;
                        pReturn = objDetail.App_Detail_06TMDKQT_Update(pDetail);
                    }

                    #region Phí cố định

                    // xóa đi
                    AppFeeFixBL _AppFeeFixBL = new AppFeeFixBL();
                    _AppFeeFixBL.AppFeeFixDelete(pDetail.Case_Code, language);

                    // insert lại fee

                    pReturn = objFeeFixBL.AppFeeFixInsertBath(pFeeFixInfo, pInfo.Case_Code);
                    if (pReturn < 0)
                    {
                        Transaction.Current.Rollback();
                        return Json(new { status = -1 });
                    }
                    #endregion

                    #region Tai lieu dinh kem 
                    if (pReturn >= 0 && pAppDocumentInfo != null)
                    {
                        if (pAppDocumentInfo.Count > 0)
                        {
                            // Get ra để map sau đó xóa đi để insert vào sau
                            AppDocumentBL _AppDocumentBL = new AppDocumentBL();
                            List<AppDocumentInfo> Lst_AppDoc = _AppDocumentBL.AppDocument_Getby_AppHeader(pDetail.APP_HEADER_ID, language);
                            Dictionary<string, AppDocumentInfo> dic_appDoc = new Dictionary<string, AppDocumentInfo>();
                            foreach (AppDocumentInfo item in Lst_AppDoc)
                            {
                                dic_appDoc[item.Document_Id] = item;
                            }

                            // xóa đi trước
                            _AppDocumentBL.AppDocumentDelByApp(pDetail.APP_HEADER_ID, language);

                            foreach (var info in pAppDocumentInfo)
                            {
                                if (SessionData.CurrentUser.chashFile.ContainsKey(info.keyFileUpload))
                                {
                                    HttpPostedFileBase pfiles = (HttpPostedFileBase)SessionData.CurrentUser.chashFile[info.keyFileUpload];
                                    info.Filename = pfiles.FileName;
                                    info.Url_Hardcopy = "/Content/Archive/" + AppUpload.Document + "/" + pfiles.FileName;
                                    info.Status = 0;
                                }
                                else
                                {
                                    if (dic_appDoc.ContainsKey(info.Document_Id))
                                    {
                                        info.Filename = dic_appDoc[info.Document_Id].Filename;
                                        info.Url_Hardcopy = dic_appDoc[info.Document_Id].Url_Hardcopy;
                                        info.Status = dic_appDoc[info.Document_Id].Status;
                                    }
                                }

                                info.App_Header_Id = pInfo.Id;
                                info.Document_Filing_Date = CommonFuc.CurrentDate();
                                info.Language_Code = language;
                            }
                            pReturn = objDoc.AppDocumentInsertBath(pAppDocumentInfo, pInfo.Id);

                        }
                    }
                    #endregion

                    #region  Thêm thông tin class
                    if (pReturn >= 0 && pAppClassInfo != null)
                    {

                        //Xoa cac class cu di 
                        pReturn = objClassDetail.AppClassDetailDeleted(pInfo.Id, language);

                        pReturn = objClassDetail.AppClassDetailInsertBatch(pAppClassInfo, pInfo.Id, language);
                    }

                    //end
                    #endregion
                    if (pReturn < 0)
                    {
                        Transaction.Current.Rollback();
                        return Json(new { status = -1 });
                    }
                    else
                    {
                        scope.Complete();
                    }
                }
                return Json(new { status = pInfo.Id });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { status = ErrorCode.Error });
            }
        }



        //[HttpPost]
        //[Route("ket_xuat_file")]
        //public ActionResult ExportData(ApplicationHeaderInfo pInfo, AppTM06DKQTInfoExport pDetail, List<AppDocumentInfo> pAppDocumentInfo,
        //      List<AppClassDetailInfo> pAppClassInfo)
        //{
        //    try
        //    {
           
        //      //  AppTM06DKQTInfoExport pDetail= new AppTM06DKQTInfoExport();

        //        string _fileTemp = System.Web.HttpContext.Current.Server.MapPath("/Content/AppForms/TM06DKQT_Request for_international_trademark_registration_vi_exp.doc");
        //        DocumentModel document = DocumentModel.Load(_fileTemp);
        //        // Fill export_header
        //        string fileName = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "TM06DKQT_Request_for_international_trademark_registration_vi_exp_" + pInfo.Appcode + ".pdf");
        //        // Fill export_detail  
        //        pDetail.Status = 254;
        //        pDetail.Status_Form = 252;
        //        pDetail.Relationship = "11";
        //        pDetail.strNgayNopDon = pDetail.NGAYNOPDON.ToDateStringN0();
        //        pDetail.REF_APPNO_TEXT = pDetail.REF_APPNO_TEXT.Trim();
        //        pDetail = CreateInstanceTM06DKQT.CopyAppHeaderInfo(pDetail, pInfo);
                
        //        if (pAppClassInfo != null)
        //        {
        //            foreach (var item in pAppClassInfo)
        //            {
        //                pDetail.strTongSonhom = item.TongSoNhom;
        //                pDetail.strTongSoSP = item.TongSanPham;
        //                pDetail.strListClass += item.Textinput + " - " + item.Code + ";";
        //            }
        //            pDetail.strListClass = "Tổng số nhóm:" + pDetail.strTongSonhom + "; Tổng số sản phẩm: " + pDetail.strTongSoSP + " ; Danh sách nhóm: " + pDetail.strListClass;
        //        }

        //        //Hiển thị tài liệu trong đơn

        //        foreach (var item in pAppDocumentInfo)
        //        {

        //        }
        //        // đẩy file lên server
        //        if (pDetail.pfileLogo != null)
        //        {
        //            pDetail.LOGOURL = AppLoadHelpers.PushFileToServer(pDetail.pfileLogo, AppUpload.Logo);
        //        }

        //        if (!string.IsNullOrEmpty(pDetail.LOGOURL))
        //        {
        //            //Kết xuất ảnh
        //            document.MailMerge.FieldMerging += (sender, e) =>
        //            {
        //                if (e.IsValueFound)
        //                {
        //                    if (e.FieldName == "Logourl")
        //                        e.Inline = new Picture(e.Document, e.Value.ToString());

        //                }
        //            };
        //            document.MailMerge.Execute(new { Logourl = Server.MapPath(pDetail.LOGOURL) });
        //            //Kết xuất ảnh
        //        }

        //        #region hiển thị tài liệu đính kèm

        //        foreach (AppDocumentInfo item in pAppDocumentInfo)
        //        {
        //            if (item.Document_Id == "C06DKQT_D_01")
        //            {
        //                pDetail.TOKHAI_USED = item.Isuse.ToString();
        //                pDetail.TOKHAI_SOTRANG = item.CHAR01;
        //                pDetail.TOKHAI_SOBAN = item.CHAR02;
        //                continue;
        //            }
        //            if (item.Document_Id == "C06DKQT_D_02")
        //            {
        //                pDetail.MAUDK_VPQT_USED = item.Isuse.ToString();
        //                pDetail.MAUDK_VPQT_SO = item.CHAR01;
        //                pDetail.MAUDK_VPQT_NGONNGU = item.CHAR02;
        //                pDetail.MAUDK_VPQT_SOTRANG = item.CHAR03;
        //                pDetail.MAUDK_VPQT_SOBAN = item.CHAR04;
        //                continue;
        //            }
        //            if (item.Document_Id == "C06DKQT_D_03")
        //            {
        //                pDetail.MAUNDH_USED = item.Isuse.ToString();
        //                pDetail.MAUNDH_SOMAU = item.CHAR01;
        //                continue;
        //            }
        //            if (item.Document_Id == "C06DKQT_D_04")
        //            {
        //                pDetail.BANSAO_TOKHAI_USED = item.Isuse.ToString();
        //                continue;
        //            }
        //            if (item.Document_Id == "C06DKQT_D_05")
        //            {
        //                pDetail.BANSAO_GIAYDK_NHCS_USED = item.Isuse.ToString();
        //                continue;
        //            }
        //            if (item.Document_Id == "C06DKQT_D_06")
        //            {
        //                pDetail.BAN_CK_SD_NGANHANG_USED = item.Isuse.ToString();
        //                continue;
        //            }
        //            if (item.Document_Id == "C06DKQT_D_07")
        //            {
        //                pDetail.GIAY_UQ_USED = item.Isuse.ToString();
        //                pDetail.GIAY_UQ_NGONNGU = item.CHAR01;
        //                continue;
        //            }
        //            if (item.Document_Id == "C06DKQT_D_08")
        //            {
        //                pDetail.GIAY_UQ_BANDICH_USED = item.Isuse.ToString();
        //                pDetail.GIAY_UQ_BANDICH_SOTRANG = item.CHAR01;
        //                continue;
        //            }
        //            if (item.Document_Id == "C06DKQT_D_09")
        //            {
        //                pDetail.GIAY_UQ_BANDICH_BANGOC_USED = item.Isuse.ToString();
        //                continue;
        //            }
        //            if (item.Document_Id == "C06DKQT_D_010")
        //            {
        //                pDetail.GIAY_UQ_BANDICH_BANSAO_USED = item.Isuse.ToString();
        //                continue;
        //            }
        //            if (item.Document_Id == "C06DKQT_D_011")
        //            {
        //                pDetail.GIAY_UQ_BANGOCNOPSAU_USED = item.Isuse.ToString();
        //                continue;
        //            }
        //            if (item.Document_Id == "C06DKQT_D_012")
        //            {
        //                pDetail.GIAY_UQ_BANGOCNOP_THEOSO_USED = item.Isuse.ToString();
        //                pDetail.GIAY_UQ_BANGOCNOP_THEOSO = item.CHAR01;
        //                continue;
        //            }
        //            if (item.Document_Id == "C06DKQT_D_013")
        //            {
        //                pDetail.CHUNGTU_LEPHI_USED = item.Isuse.ToString();
        //                continue;
        //            }
        //            if (item.Document_Id == "C06DKQT_D_014")
        //            {
        //                pDetail.TAILIEUBOSUNG_USED = item.Isuse.ToString();
        //                pDetail.TAILIEUBOSUNG = item.CHAR01;
        //                continue;
        //            }
        //        }

        //        #endregion


        //        document.MailMerge.Execute(pDetail);
        //        document.Save(fileName, SaveOptions.PdfDefault);
        //        byte[] fileContents;
        //        var options = SaveOptions.PdfDefault;
        //        // Save document to DOCX format in byte array.
        //        using (var stream = new MemoryStream())
        //        {
        //            document.Save(stream, options);
        //            fileContents = stream.ToArray();
        //        }
        //        Convert.ToBase64String(fileContents);
        //        return Json(new { success = 0 });

        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.LogException(ex);
        //        return Json(new { success = 0 });
        //    }
        //}

        //[HttpPost]
        [Route("ket_xuat_file")]
        public ActionResult ExportDataNew(ApplicationHeaderInfo pInfo, AppTM06DKQTInfoExport pDetail, List<AppDocumentInfo> pAppDocumentInfo,
            List<AppClassDetailInfo> pAppClassInfo)
        {
            try
            {

                //  AppTM06DKQTInfoExport pDetail= new AppTM06DKQTInfoExport();

                string _fileTemp = System.Web.HttpContext.Current.Server.MapPath("/Content/AppForms/TM06DKQT_Request for_international_trademark_registration_vi_exp.doc");
              
                // Fill export_header
                string fileName = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "TM06DKQT_Request_for_international_trademark_registration_vi_exp_" + pInfo.Appcode + ".pdf");
                // Fill export_detail  
                pDetail.Status = 254;
                pDetail.Status_Form = 252;
                pDetail.Relationship = "11";
                pDetail.strNgayNopDon = pDetail.NGAYNOPDON.ToDateStringN0();
                pDetail.REF_APPNO_TEXT = pDetail.REF_APPNO_TEXT.Trim();
                pDetail = CreateInstanceTM06DKQT.CopyAppHeaderInfo(pDetail, pInfo);

                //if (pAppClassInfo != null)
                //{
                //    foreach (var item in pAppClassInfo)
                //    {
                //        pDetail.strTongSonhom = item.TongSoNhom;
                //        pDetail.strTongSoSP = item.TongSanPham;
                //        pDetail.strListClass += item.Textinput + " - " + item.Code + ";";
                //    }
                //    pDetail.strListClass = "Tổng số nhóm:" + pDetail.strTongSonhom + "; Tổng số sản phẩm: " + pDetail.strTongSoSP + " ; Danh sách nhóm: " + pDetail.strListClass;
                //}

                if (pAppClassInfo != null)
                {
                    Hashtable _hsGroupclass = new Hashtable();
                    foreach (var item in pAppClassInfo)
                    {
                        AppClassDetailInfo _newinfo = new AppClassDetailInfo();
                        _newinfo.CloneObj();
                        if (_hsGroupclass.ContainsKey(item.Code.Substring(0, 2)))
                        {
                            _newinfo = (AppClassDetailInfo)_hsGroupclass[item.Code.Substring(0, 2)];
                        }
                        _newinfo.Code = item.Code;
                        _newinfo.Textinput += item.Textinput + ", ";
                        _newinfo.IntTongSanPham++;
                        _hsGroupclass[item.Code.Substring(0, 2)] = _newinfo;
                    }
                    //appInfo.strListClass = "Tổng số nhóm: \n" + appInfo.strTongSonhom + "; Tổng số sản phẩm: " + appInfo.strTongSoSP + " ; Danh sách nhóm: " + appInfo.strListClass;
                    List<AppClassDetailInfo> _listApp = new List<AppClassDetailInfo>();
                    foreach (DictionaryEntry item in _hsGroupclass)
                    {
                        _listApp.Add((AppClassDetailInfo)item.Value);
                    }
                    foreach (AppClassDetailInfo item in _listApp.OrderBy(m => m.Code))
                    {
                        pDetail.strListClass += "Nhóm " + item.Code.Substring(0, 2) + ": " + item.Textinput.Trim().Trim(',') + " (" + (item.IntTongSanPham < 10 ? "0" + item.IntTongSanPham.ToString() : item.IntTongSanPham.ToString()) + " sản phẩm)" + "\n";
                    }
                }

                //Hiển thị tài liệu trong đơn

                foreach (var item in pAppDocumentInfo)
                {

                }
                // đẩy file lên server
                if (pDetail.pfileLogo != null)
                {
                    pDetail.LOGOURL = AppLoadHelpers.PushFileToServer(pDetail.pfileLogo, AppUpload.Logo);
                }

                if (!string.IsNullOrEmpty(pDetail.LOGOURL))
                {
                  
                    //Kết xuất ảnh
                }

                #region hiển thị tài liệu đính kèm

                foreach (AppDocumentInfo item in pAppDocumentInfo)
                {
                    if (item.Document_Id == "C06DKQT_D_01")
                    {
                        pDetail.TOKHAI_USED = item.Isuse.ToString();
                        pDetail.TOKHAI_SOTRANG = item.CHAR01;
                        pDetail.TOKHAI_SOBAN = item.CHAR02;
                        continue;
                    }
                    if (item.Document_Id == "C06DKQT_D_02")
                    {
                        pDetail.MAUDK_VPQT_USED = item.Isuse.ToString();
                        pDetail.MAUDK_VPQT_SO = item.CHAR01;
                        pDetail.MAUDK_VPQT_NGONNGU = item.CHAR02;
                        pDetail.MAUDK_VPQT_SOTRANG = item.CHAR03;
                        pDetail.MAUDK_VPQT_SOBAN = item.CHAR04;
                        continue;
                    }
                    if (item.Document_Id == "C06DKQT_D_03")
                    {
                        pDetail.MAUNDH_USED = item.Isuse.ToString();
                        pDetail.MAUNDH_SOMAU = item.CHAR01;
                        continue;
                    }
                    if (item.Document_Id == "C06DKQT_D_04")
                    {
                        pDetail.BANSAO_TOKHAI_USED = item.Isuse.ToString();
                        continue;
                    }
                    if (item.Document_Id == "C06DKQT_D_05")
                    {
                        pDetail.BANSAO_GIAYDK_NHCS_USED = item.Isuse.ToString();
                        continue;
                    }
                    if (item.Document_Id == "C06DKQT_D_06")
                    {
                        pDetail.BAN_CK_SD_NGANHANG_USED = item.Isuse.ToString();
                        continue;
                    }
                    if (item.Document_Id == "C06DKQT_D_07")
                    {
                        pDetail.GIAY_UQ_USED = item.Isuse.ToString();
                        pDetail.GIAY_UQ_NGONNGU = item.CHAR01;
                        continue;
                    }
                    if (item.Document_Id == "C06DKQT_D_08")
                    {
                        pDetail.GIAY_UQ_BANDICH_USED = item.Isuse.ToString();
                        pDetail.GIAY_UQ_BANDICH_SOTRANG = item.CHAR01;
                        continue;
                    }
                    if (item.Document_Id == "C06DKQT_D_09")
                    {
                        pDetail.GIAY_UQ_BANDICH_BANGOC_USED = item.Isuse.ToString();
                        continue;
                    }
                    if (item.Document_Id == "C06DKQT_D_010")
                    {
                        pDetail.GIAY_UQ_BANDICH_BANSAO_USED = item.Isuse.ToString();
                        continue;
                    }
                    if (item.Document_Id == "C06DKQT_D_011")
                    {
                        pDetail.GIAY_UQ_BANGOCNOPSAU_USED = item.Isuse.ToString();
                        continue;
                    }
                    if (item.Document_Id == "C06DKQT_D_012")
                    {
                        pDetail.GIAY_UQ_BANGOCNOP_THEOSO_USED = item.Isuse.ToString();
                        pDetail.GIAY_UQ_BANGOCNOP_THEOSO = item.CHAR01;
                        continue;
                    }
                    if (item.Document_Id == "C06DKQT_D_013")
                    {
                        pDetail.CHUNGTU_LEPHI_USED = item.Isuse.ToString();
                        continue;
                    }
                    if (item.Document_Id == "C06DKQT_D_014")
                    {
                        pDetail.TAILIEUBOSUNG_USED = item.Isuse.ToString();
                        pDetail.TAILIEUBOSUNG = item.CHAR01;
                        continue;
                    }
                }



                #endregion
                pDetail.LEPHI = 2000000;
                List<AppTM06DKQTInfoExport> _lst = new List<AppTM06DKQTInfoExport>();
                pDetail.LOGOURL = Server.MapPath(pDetail.LOGOURL);
                _lst.Add(pDetail);
                DataSet _ds_all = ConvertData.ConvertToDataSet<AppTM06DKQTInfoExport>(_lst, false);

                CrystalDecisions.CrystalReports.Engine.ReportDocument oRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                //   Logger.LogInfo("b1: load");
                //  Logger.LogInfo("b2: file:" + Server.MapPath("~/Report/") + "TM_06DKQT.rpt");
                string _tempfile = "TM_06DKQT.rpt";
                if(AppsCommon.GetCurrentLang() == Language.LangEN)
                {
                    _tempfile = "TM_06DKQT_EN.rpt";
                }

                  oRpt.Load(Path.Combine(Server.MapPath("~/Report/"), _tempfile), OpenReportMethod.OpenReportByTempCopy);
            
                //   _ds_all.WriteXmlSchema(@"C:\Users\user\Desktop\LEGALTECH\XMLFILE\TM06DKQT.xml");
                Logger.LogInfo("b3: ");
                CrystalDecisions.CrystalReports.Engine.PictureObject _pic01;
                _pic01 = (CrystalDecisions.CrystalReports.Engine.PictureObject)oRpt.ReportDefinition.Sections[0].ReportObjects["Picture1"];
                _pic01.Width = 100;
                _pic01.Height = 100;


                System.IO.FileInfo file = new System.IO.FileInfo(pDetail.LOGOURL);
                Bitmap img = new Bitmap(pDetail.LOGOURL);
                try
                {
                    double _Const = 6.666666666666;
                    int _left = 0, _top = 0, _marginleft = 225, _margintop = 5580;
                    int _h = 600;
                    double _d1 = (_h - img.Width) / 2;
                    _d1 = _Const * _d1;
                    _left = _marginleft + Convert.ToInt32(_d1);
                    if (_left < 0)
                    {
                        _left = _marginleft;
                    }
                    _pic01.Left = _left;
                    // top

                    _d1 = (_h - img.Height) / 2;
                    _d1 = _Const * _d1;
                    _top = _margintop + Convert.ToInt32(_d1);
                    if (_top < 0)
                    {
                        _top = _margintop;
                    }
                    _pic01.Top = _top;

                }
                catch (Exception)
                {

                }
                finally
                {
                    img.Dispose();
                }

                if (_ds_all != null)
                {
                    _ds_all.Tables[0].TableName = "Table1";
                    oRpt.SetDataSource(_ds_all);
                }
                oRpt.Refresh();

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                System.IO.Stream oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat);
                byte[] byteArray = new byte[oStream.Length];
                oStream.Read(byteArray, 0, Convert.ToInt32(oStream.Length - 1));
                System.IO.File.WriteAllBytes(fileName, byteArray.ToArray()); // Requires System.Linq


                return Json(new { success = 0 });

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = 0 });
            }
        }



        //[HttpPost]
        [Route("Pre-View")]
        public ActionResult PreViewApplication()
        {
            try
            {
                
                ViewBag.FileName = "/Content/Export/" + "TM06DKQT_Request_for_international_trademark_registration_vi_exp_TM06DKQT.pdf";
                return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistrationDKQT/_PartialContentPreview.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistrationDKQT/_PartialContentPreview.cshtml");
            }
        }




    }
}
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
    using BussinessFacade.ModuleMemoryData;
    using System.Data;

    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("TradeMarkRegistration", AreaPrefix = "trade-mark")]
    [Route("{action}")]
    public class TradeMarkRegistrationController : Controller
    {
        // GET: TradeMark/TradeMarkRegistration
       public static List<AppDocumentOthersInfo> lstDocOther = new List<AppDocumentOthersInfo>();
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
                if (AppCode == TradeMarkAppCode.AppCodeSuaDoiDangKy)
                {
                    return AppSuaDoiDonDangKy();
                }
                else if (AppCode == TradeMarkAppCode.AppCodeDangKyChuyenDoi)
                {

                }
                else if (AppCode == TradeMarkAppCode.AppCodeDangKynhanHieu)
                {
                    return AppDangKyNhanHieu();
                }
                else if (AppCode == TradeMarkAppCode.AppCode_TM_3B_PLB_01_SDD)
                {
                    return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistration/_Partial_TM_3B_PLB_01_SDD.cshtml");
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

        public ActionResult AppDangKyNhanHieu()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistration/_PartalDangKyNhanHieu.cshtml");
        }

        [HttpPost]
        [Route("dang_ky_nhan_hieu")]
        public ActionResult AppDonDangKyInsert(ApplicationHeaderInfo pInfo, AppDetail04NHInfo pDetail, List<AppDocumentInfo> pAppDocumentInfo,
            List<AppDocumentOthersInfo> pAppDocOtherInfo, List<AppClassDetailInfo> pAppClassInfo )
        {
            try
            {
                Application_Header_BL objBL = new Application_Header_BL();
                AppFeeFixBL objFeeFixBL = new AppFeeFixBL();
                AppDetail04NHBL objDetail = new AppDetail04NHBL();
                AppClassDetailBL objClassDetail = new AppClassDetailBL();
                AppDocumentBL objDoc = new AppDocumentBL();
                if (pInfo == null || pDetail == null) return Json(new { status = ErrorCode.Error });
                string language = AppsCommon.GetCurrentLang();
                var CreatedBy = SessionData.CurrentUser.Username;
                var CreatedDate = SessionData.CurrentUser.CurrentDate;
                int pReturn = ErrorCode.Success;
                int pAppHeaderID = 0;
                using (var scope = new TransactionScope())
                {
                    //
                    pInfo.Languague_Code = language;
                    pInfo.Created_By = CreatedBy;
                    pInfo.Created_Date = CreatedDate;
                    pInfo.Send_Date = CreatedDate;
                    //TRA RA ID CUA BANG KHI INSERT
                    pAppHeaderID = objBL.AppHeaderInsert(pInfo);
                    if (pAppHeaderID >= 0)
                    {
                        pDetail.Appcode = pInfo.Appcode;
                        pDetail.Language_Code = language;
                        pDetail.App_Header_Id = pAppHeaderID;
                        if (pDetail.pfileLogo != null)
                        {
                            pDetail.Logourl = AppLoadHelpers.PushFileToServer(pDetail.pfileLogo, AppUpload.Logo);
                        }
                        pReturn = objDetail.App_Detail_04NH_Insert(pDetail);
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
                            var listDocumentInsert = new List<AppDocumentInfo>();
                            foreach (var info in pAppDocumentInfo)
                            {
                                if (SessionData.CurrentUser.chashFile.ContainsKey(info.keyFileUpload))
                                {
                                    HttpPostedFileBase pfiles = (HttpPostedFileBase)SessionData.CurrentUser.chashFile[info.keyFileUpload];
                                    info.Filename = pfiles.FileName;
                                    info.Url_Hardcopy = "/Content/Archive/" + AppUpload.Document + "/" + pfiles.FileName;
                                    info.Status = 0;
                                    info.App_Header_Id = pAppHeaderID;
                                    info.Document_Filing_Date = CommonFuc.CurrentDate();
                                    info.Language_Code = language;
                                    listDocumentInsert.Add(info);
                                }
                               
                            }
                            if(listDocumentInsert.Count>0)
                            pReturn = objDoc.AppDocumentInsertBath(pAppDocumentInfo, pAppHeaderID);

                        }
                    }
                    //tai lieu khac 
                    if (pReturn >= 0 && pAppDocOtherInfo != null)
                    {
                        if (pAppDocOtherInfo.Count > 0)
                        {
                            int check = 0;
                            foreach (var info in pAppDocOtherInfo)
                            {
                                if (SessionData.CurrentUser.chashFileOther.ContainsKey(info.keyFileUpload))
                                {
                                    HttpPostedFileBase pfiles = (HttpPostedFileBase)SessionData.CurrentUser.chashFileOther[info.keyFileUpload];
                                    info.Filename = pfiles.FileName;
                                    info.Filename = "/Content/Archive/" + AppUpload.Document + "/" + pfiles.FileName;
                                    check = 1;

                                }
                                info.App_Header_Id = pAppHeaderID;
                                info.Language_Code = language;
                            }
                            if (check == 1)
                            {
                                pReturn = objDoc.AppDocumentOtherInsertBatch(pAppDocOtherInfo);
                            }
                        }
                    }
                    //Tính phí 
                    if (pReturn >= 0 && pAppClassInfo != null)
                    {
                        pReturn = CaculatorFee(pAppClassInfo, pDetail.Sodon_Ut, pAppHeaderID);
                    }
                    //end
                    if (pReturn < 0)
                    {
                        Transaction.Current.Rollback();
                        return Json(new { status = pReturn });
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
        [Route("them_moi_don_dang_ky_sua_doi")]
        public ActionResult AppSuaDoiDonDangKyInsert(ApplicationHeaderInfo pInfo, List<AppFeeFixInfo> pFeeFixInfo,
                        AppDetail01Info pDetailInfo, List<AppDocumentInfo> pAppDocumentInfo)
        {
            try
            {
                Application_Header_BL objBL = new Application_Header_BL();
                AppFeeFixBL objFeeFixBL = new AppFeeFixBL();
                AppDetail01BL objDetail01BL = new AppDetail01BL();
                AppDocumentBL objDoc = new AppDocumentBL();
                if (pInfo == null || pDetailInfo == null) return Json(new { status = ErrorCode.Error });
                string language = AppsCommon.GetCurrentLang();
                var CreatedBy = SessionData.CurrentUser.Username;
                var CreatedDate = SessionData.CurrentUser.CurrentDate;
                int pReturn = ErrorCode.Success;
                int pAppHeaderID = 0;
                //
                using (var scope = new TransactionScope())
                {
                    pInfo.Languague_Code = language;
                    pInfo.Created_By = CreatedBy;
                    pInfo.Created_Date = CreatedDate;

                    //TRA RA ID CUA BANG KHI INSERT
                    pAppHeaderID = objBL.AppHeaderInsert(pInfo);
                    //Gán lại khi lấy dl 
                    if (pAppHeaderID >= 0)
                    {
                        pReturn = objFeeFixBL.AppFeeFixInsertBath(pFeeFixInfo, pAppHeaderID);
                    }
                    else
                    {
                        Transaction.Current.Rollback();
                    }
                    if (pReturn >= 0)
                    {
                        pDetailInfo.Language_Code = language;
                        pDetailInfo.App_Header_Id = pAppHeaderID;
                        pReturn = objDetail01BL.AppDetailInsert(pDetailInfo);
                    }
                    else
                    {
                        Transaction.Current.Rollback();
                    }
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
                                    info.Url_Hardcopy = "/Content/DataWareHouse" + pfiles.FileName;
                                    info.Status = 0;
                                }
                                info.Document_Filing_Date = CommonFuc.CurrentDate();
                                info.Language_Code = language;
                            }
                            pReturn = objDoc.AppDocumentInsertBath(pAppDocumentInfo, pAppHeaderID);
                            if (pReturn < 0)
                            {
                                Transaction.Current.Rollback();
                            }
                            else
                            {
                                scope.Complete();
                            }
                        }
                        else
                        {
                            scope.Complete();
                        }
                    }
                    else
                    {
                        Transaction.Current.Rollback();
                    }
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
        public ActionResult PushFileToServer(AppDocumentInfo pInfo)
        {
            try
            {
                if (pInfo.pfiles != null)
                {
                    var url = AppLoadHelpers.PushFileToServer(pInfo.pfiles, AppUpload.Document);
                    SessionData.CurrentUser.chashFile[pInfo.keyFileUpload] = pInfo.pfiles;
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
        [Route("push-file-other-to-server")]
        public ActionResult PushFileOtherToServer(AppDocumentInfo pInfo) //AppDocumentInfo de lay thong tin add vao hash thoi
        {
            try
            {
                if (pInfo.pfiles != null)
                {
                    var url = AppLoadHelpers.PushFileToServer(pInfo.pfiles, AppUpload.Document);
                    SessionData.CurrentUser.chashFileOther[pInfo.keyFileUpload] = pInfo.pfiles;
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
        public ActionResult ExportData(ApplicationHeaderInfo pInfo, AppDetail04NHInfo pDetail, List<AppDocumentInfo> pAppDocumentInfo,
            List<AppDocumentOthersInfo> pAppDocOtherInfo, List<AppClassDetailInfo> pAppClassInfo)
        {
            try
            {
                AppInfoExport appInfo = new AppInfoExport();

                string _fileTemp = System.Web.HttpContext.Current.Server.MapPath("/Content/AppForms/TM04NH_Request_for_trademark_registration_vi_exp.doc");
                DocumentModel document = DocumentModel.Load(_fileTemp);
                // Fill export_header
                string fileName = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "Request_for_trademark_registration_vi_exp_" + pInfo.Appcode + ".pdf");
                // Fill export_detail  
                appInfo.Status = 254;
                appInfo.Status_Form = 252;
                appInfo.Relationship = "11";
                appInfo= CreateInstance.CopyAppHeaderInfo(  appInfo, pInfo);
                appInfo= CreateInstance.CopyAppDetailInfo( appInfo, pDetail);
                appInfo.DuadateExp = appInfo.Duadate.ToString("dd/MM/yyyy");
                appInfo.Ngaynopdon_UtExp = appInfo.Ngaynopdon_Ut.ToString("dd/MM/yyyy");
                if (string.IsNullOrEmpty(appInfo.Logourl))
                {
                    appInfo.Logourl = pDetail.LogourlOrg;
                }
                if (pAppClassInfo != null)
                {
                    foreach (var item in pAppClassInfo)
                    {
                        appInfo.strTongSonhom = item.TongSoNhom;
                        appInfo.strTongSoSP = item.TongSanPham;
                        appInfo.strListClass += item.Textinput + " - " + item.Code + ";";
                    }
                    appInfo.strListClass = "Tổng số nhóm:" + appInfo.strTongSonhom + "; Tổng số sản phẩm: " + appInfo.strTongSoSP + " ; Danh sách nhóm: " + appInfo.strListClass;
                }
                if (lstDocOther != null)
                {
                    foreach (var item in lstDocOther)
                    {
                        appInfo.strDanhSachFileDinhKem += item.Documentname + " ; ";
                    }
                }
                //Hiển thị phí 
                if (pInfo.Id != 0)
                {
                    AppFeeFixBL appFeeFixBL = new AppFeeFixBL();
                    DataSet dsFeefix = appFeeFixBL.AppFeeFixGetByAppHeader(pInfo.Id);
                    List<AppFeeFixInfo> lst = CBO<AppFeeFixInfo>.FillCollectionFromDataSet(dsFeefix);

                }
                //End

                //Kết xuất ảnh
                document.MailMerge.FieldMerging += (sender, e) =>
                {
                    if (e.IsValueFound)
                    {
                        if (e.FieldName == "Logourl")
                            e.Inline = new Picture(e.Document, e.Value.ToString());
                       
                    }
                };
                document.MailMerge.Execute(new { Logourl = Server.MapPath(appInfo.Logourl) });
               //Kết xuất ảnh

                document.MailMerge.Execute(appInfo);
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
                string tm04Nh = "TM04NH";
                ViewBag.FileName = "/Content/Export/" + "Request_for_trademark_registration_vi_exp_" + tm04Nh+".pdf";
                return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistration/_PartialContentPreview.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistration/_PartialContentPreview.cshtml");
            }
        }

        #region Sua don luu tam
        /// <summary>
        /// ID:ID của app_header_id 
        /// ID2: là appcode 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("request-for-trade-mark-edit/{id}/{id1}/{id2}")]
        public ActionResult TradeMarkForEdit()
        {
            decimal App_Header_Id = 0;
            string AppCode = "";
            int Status = 0;
            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("/");
                SessionData.CurrentUser.chashFile.Clear();
                SessionData.CurrentUser.chashFileOther.Clear();

                if (RouteData.Values.ContainsKey("id"))
                {
                    App_Header_Id = CommonFuc.ConvertToDecimal(RouteData.Values["id"]);
                }
                if (RouteData.Values.ContainsKey("id1"))
                {
                    Status = CommonFuc.ConvertToInt(RouteData.Values["id1"]);
                }
                if (RouteData.Values.ContainsKey("id2"))
                {
                    AppCode = RouteData.Values["id2"].ToString().ToUpper();
                }

                if (AppCode == TradeMarkAppCode.AppCodeDangKynhanHieu)
                {
                    return TradeMarkSuaDon(App_Header_Id, AppCode, Status);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return TradeMarkSuaDon(App_Header_Id, AppCode, Status);
        }

        [HttpGet]
        [Route("request-for-trade-mark-view/{id}/{id1}/{id2}")]
        public ActionResult TradeMarkForView()
        {
            int Status = 0;
            decimal App_Header_Id = 0;
            string AppCode = "";
            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("/");
                SessionData.CurrentUser.chashFile.Clear();
                SessionData.CurrentUser.chashFileOther.Clear();

                if (RouteData.Values.ContainsKey("id"))
                {
                    App_Header_Id = CommonFuc.ConvertToDecimal(RouteData.Values["id"]);
                }
                if (RouteData.Values.ContainsKey("id1"))
                {
                    Status = CommonFuc.ConvertToInt(RouteData.Values["id1"]);
                }
                if (RouteData.Values.ContainsKey("id2"))
                {
                    AppCode = RouteData.Values["id2"].ToString().ToUpper();
                }

                if (AppCode == TradeMarkAppCode.AppCodeDangKynhanHieu)
                {
                    return TradeMarkView(App_Header_Id, AppCode, Status);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return TradeMarkView(App_Header_Id, AppCode, Status);
        }
        
        public ActionResult TradeMarkView(decimal pAppHeaderId, string pAppCode, int pStatus)
        {
            if (pAppCode == TradeMarkAppCode.AppCodeDangKynhanHieu)
            {
                var objBL = new AppDetail04NHBL();
                string language = AppsCommon.GetCurrentLang();
                var ds04NH = objBL.AppTM04NHGetByID(pAppHeaderId, language, pStatus);
                if (ds04NH != null && ds04NH.Tables.Count == 5)
                {
                    ViewBag.objAppHeaderInfo = CBO<AppDetail04NHInfo>.FillObjectFromDataTable(ds04NH.Tables[0]);
                    ViewBag.lstDocumentInfo = CBO<AppDocumentInfo>.FillCollectionFromDataTable(ds04NH.Tables[1]);
                    ViewBag.lstDocOther = CBO<AppDocumentOthersInfo>.FillCollectionFromDataTable(ds04NH.Tables[2]);
                    ViewBag.lstClassDetailInfo = CBO<AppClassDetailInfo>.FillCollectionFromDataTable(ds04NH.Tables[3]);
                    ViewBag.lstFeeInfo = CBO<AppFeeFixInfo>.FillCollectionFromDataTable(ds04NH.Tables[4]);
                }
                return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistration/View_PartialDangKyNhanHieu.cshtml");
            }
            else if (pAppCode == TradeMarkAppCode.AppCode_TM_3B_PLB_01_SDD)
            {
                App_Detail_PLB01_SDD_BL objBL = new App_Detail_PLB01_SDD_BL();
                string language = AppsCommon.GetCurrentLang();
                List<AppDocumentInfo> appDocumentInfos = new List<AppDocumentInfo>();
                List<AppFeeFixInfo> appFeeFixInfos = new List<AppFeeFixInfo>();
                ApplicationHeaderInfo applicationHeaderInfo = new ApplicationHeaderInfo();
                App_Detail_PLB01_SDD_Info app_Detail = objBL.GetByID(pAppHeaderId, language, ref applicationHeaderInfo, ref appDocumentInfos, ref appFeeFixInfos);
                ViewBag.App_Detail = app_Detail;
                ViewBag.Lst_AppDoc = appDocumentInfos;
                ViewBag.Lst_AppFee = appFeeFixInfos;
                ViewBag.objAppHeaderInfo = applicationHeaderInfo;

                return PartialView("~/Areas/TradeMark/Views/PLB01_SDD_3B/_Partial_TM_3B_PLB_01_SDD_View.cshtml");
            }
            else
            {
                return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistration/View_PartialDangKyNhanHieu.cshtml");
            }
        }

        public ActionResult TradeMarkSuaDon(decimal pAppHeaderId, string pAppCode, int pStatus)
        {
            if (pAppCode == TradeMarkAppCode.AppCodeDangKynhanHieu)
            {
                var objBL = new AppDetail04NHBL();
                string language = AppsCommon.GetCurrentLang();
                var ds04NH = objBL.AppTM04NHGetByID(pAppHeaderId, language, pStatus);
                if (ds04NH != null && ds04NH.Tables.Count == 5)
                {
                    ViewBag.objAppHeaderInfo = CBO<AppDetail04NHInfo>.FillObjectFromDataTable(ds04NH.Tables[0]);
                    ViewBag.lstDocumentInfo = CBO<AppDocumentInfo>.FillCollectionFromDataTable(ds04NH.Tables[1]);
                    ViewBag.lstDocOther = CBO<AppDocumentOthersInfo>.FillCollectionFromDataTable(ds04NH.Tables[2]);
                    lstDocOther = ViewBag.lstDocOther;
                    ViewBag.lstClassDetailInfo = CBO<AppClassDetailInfo>.FillCollectionFromDataTable(ds04NH.Tables[3]);
                    ViewBag.lstFeeInfo = CBO<AppFeeFixInfo>.FillCollectionFromDataTable(ds04NH.Tables[4]);
                }
                return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistration/Edit_PartialDangKyNhanHieu.cshtml");
            }
            else if (pAppCode == TradeMarkAppCode.AppCode_TM_3B_PLB_01_SDD)
            {
                App_Detail_PLB01_SDD_BL objBL = new App_Detail_PLB01_SDD_BL();
                string language = AppsCommon.GetCurrentLang();
                List<AppDocumentInfo> appDocumentInfos = new List<AppDocumentInfo>();
                List<AppFeeFixInfo> appFeeFixInfos = new List<AppFeeFixInfo>();
                ApplicationHeaderInfo applicationHeaderInfo = new ApplicationHeaderInfo();
                App_Detail_PLB01_SDD_Info app_Detail = objBL.GetByID(pAppHeaderId, language, ref applicationHeaderInfo, ref appDocumentInfos, ref appFeeFixInfos);
                ViewBag.App_Detail = app_Detail;
                ViewBag.Lst_AppDoc = appDocumentInfos;
                ViewBag.Lst_AppFee = appFeeFixInfos;
                ViewBag.objAppHeaderInfo = applicationHeaderInfo;

                return PartialView("~/Areas/TradeMark/Views/PLB01_SDD_3B/_Partial_TM_3B_PLB_01_SDD_Edit.cshtml");
            }
            else
            {
                //
                return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistration/Edit_PartialDangKyNhanHieu.cshtml");
            }
        }

        [HttpPost]
        [Route("request-for-trade-mark-del")]
        public ActionResult TradeMarkForDel(decimal pAppHeaderID, string pAppCode)
        {

            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("/");
                SessionData.CurrentUser.chashFile.Clear();
                SessionData.CurrentUser.chashFileOther.Clear();
                var objBL = new Application_Header_BL();
                string language = AppsCommon.GetCurrentLang();
                var response = objBL.AppHeaderDeleted(pAppHeaderID, language);
                return Json(new { status = response });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { status = ErrorCode.Error });
            }
        }
        #endregion

        [HttpPost]
        [Route("dang-ky-nhan-hieu-sua-doi")]
        public ActionResult AppDonDangKyEditAppro(ApplicationHeaderInfo pInfo, AppDetail04NHInfo pDetail, List<AppDocumentInfo> pAppDocumentInfo,
            List<AppDocumentOthersInfo> pAppDocOtherInfo, List<AppClassDetailInfo> pAppClassInfo, List<AppFeeFixInfo> pFeeFixInfo,string listIDDocRemove)
        {
            try
            {
                Application_Header_BL objBL = new Application_Header_BL();
                AppFeeFixBL objFeeFixBL = new AppFeeFixBL();
                AppDetail04NHBL objDetail = new AppDetail04NHBL();
                AppClassDetailBL objClassDetail = new AppClassDetailBL();
                AppDocumentBL objDoc = new AppDocumentBL();
                if (pInfo == null || pDetail == null) return Json(new { status = ErrorCode.Error });
                string language = AppsCommon.GetCurrentLang();
                var CreatedBy = SessionData.CurrentUser.Username;
                var CreatedDate = SessionData.CurrentUser.CurrentDate;
                int pReturn = ErrorCode.Success;
                int pAppHeaderID = 0;
                using (var scope = new TransactionScope())
                {
                    //
                    pInfo.Languague_Code = language;
                    pInfo.Modify_By = CreatedBy;
                    pInfo.Modify_Date = CreatedDate;
                    //TRA RA ID CUA BANG KHI INSERT
                    pAppHeaderID = objBL.AppHeaderUpdate(pInfo);
                    if (pAppHeaderID >= 0)
                    {
                        pDetail.Appcode = pInfo.Appcode;
                        pDetail.Language_Code = language;
                        pDetail.App_Header_Id = pInfo.Id;
                        if (pDetail.pfileLogo != null)
                        {
                            pDetail.Logourl = AppLoadHelpers.PushFileToServer(pDetail.pfileLogo, AppUpload.Logo);
                        }
                        else
                        {
                            pDetail.Logourl = pDetail.LogourlOrg;
                        }


                        pReturn = objDetail.App_Detail_04NH_Update(pDetail);
                        //Thêm thông tin class
                        if (pReturn >= 0 && pAppClassInfo!=null)
                        {

                            //Xoa cac class cu di 
                            pReturn = objClassDetail.AppClassDetailDeleted(pInfo.Id, language);

                            pReturn = objClassDetail.AppClassDetailInsertBatch(pAppClassInfo, pInfo.Id, language);
                        }
                    }
                    //Tai lieu dinh kem 
                    if (pReturn >= 0 && pAppDocumentInfo != null)
                    {
                        if (pAppDocumentInfo.Count > 0)
                        {
                            var listDocumentInsert = new List<AppDocumentInfo>();
                            foreach (var info in pAppDocumentInfo)
                            {
                                if (SessionData.CurrentUser.chashFile.ContainsKey(info.keyFileUpload))
                                {
                                    HttpPostedFileBase pfiles = (HttpPostedFileBase)SessionData.CurrentUser.chashFile[info.keyFileUpload];
                                    info.Filename = pfiles.FileName;
                                    info.Url_Hardcopy = "/Content/Archive/" + AppUpload.Document + "/"+ pfiles.FileName;
                                    info.Status = 0;
                                    info.App_Header_Id = pInfo.Id;
                                    info.Document_Filing_Date = CommonFuc.CurrentDate();
                                    info.Language_Code = language;
                                    listDocumentInsert.Add(info);
                                }
                            }
                            if (listDocumentInsert.Count > 0)
                            {
                                pReturn = objDoc.AppDocumentInsertBath(listDocumentInsert, pInfo.Id);
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(listIDDocRemove))
                    {
                        string[] arrayIDDoc = listIDDocRemove.Split('|');
                        for (int i = 0; i < arrayIDDoc.Length; i++)
                        {
                            decimal idDOc = CommonFuc.ConvertToDecimal(arrayIDDoc[i]);
                            pReturn = objDoc.AppDocumentDelByID(idDOc, language, pInfo.Id);
                        }
                    }
                    //tai lieu khac 
                    if (pReturn >= 0 && pAppDocOtherInfo != null)
                    {
                        if (pAppDocOtherInfo.Count > 0)
                        {
                            int check = 0;
                            foreach (var info in pAppDocOtherInfo)
                            {
                                if (SessionData.CurrentUser.chashFileOther.ContainsKey(info.keyFileUpload))
                                {
                                    HttpPostedFileBase pfiles = (HttpPostedFileBase)SessionData.CurrentUser.chashFileOther[info.keyFileUpload];
                                    info.Filename = pfiles.FileName;
                                    info.Filename = "/Content/Archive/" + AppUpload.Document + "/" + pfiles.FileName;
                                    check = 1;
                                }
                                info.App_Header_Id = pInfo.Id;
                                info.Language_Code = language;
                               
                            }
                            if (check == 1)
                            {
                                pReturn = objDoc.AppDocumentOtherInsertBatch(pAppDocOtherInfo);
                            }
                        }
                    }

                    //Xóa các tài liệu đi khi sửa bản ghi 
                    if (pReturn >= 0 && !string.IsNullOrEmpty(pDetail.ListFileAttachOtherDel))
                    {
                        var arrIdFileAttack = pDetail.ListFileAttachOtherDel.Split(',');
                        foreach (var item in arrIdFileAttack)
                        {
                            decimal pID = CommonFuc.ConvertToDecimal(item);
                            pReturn = objDoc.AppDocOtherByID(pID, language);
                        }
                    }

                    //end
                    if (pReturn < 0)
                    {
                        
                        Transaction.Current.Rollback();
                        return Json(new { status = pReturn });
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

        public int CaculatorFee(List<AppClassDetailInfo> pAppClassInfo,string NumberAppNo, decimal pAppHeaderId )
        {
            try
            {
                string _keyFee = "";
                int TongSoNhom = 1;
                int SoDongTinhQua = 0;
                int TongSoTinhPhi = 0;
                _keyFee = TradeMarkAppCode.AppCodeDangKynhanHieu + "_2011";
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    SoDongTinhQua = CommonFuc.ConvertToInt(MemoryData.c_dic_FeeByApp_Fix[_keyFee].Char01);
                }
                else
                {
                    SoDongTinhQua = 6;
                }
                if (pAppClassInfo.Count > 0)
                {
                    TongSoNhom=CommonFuc.ConvertToInt( pAppClassInfo[0].TongSoNhom);
                    string[] arrSoSanPham = pAppClassInfo[0].TongSanPham.Split('|');
                    for (int i = 0; i < arrSoSanPham.Length; i++)
                    {
                        int TotalItemOnGroup = CommonFuc.ConvertToInt(arrSoSanPham[i]);
                        if(TotalItemOnGroup> SoDongTinhQua)
                        {
                            TongSoTinhPhi = TongSoTinhPhi + (TotalItemOnGroup - SoDongTinhQua);
                        }
                    }
                }
                if (TongSoTinhPhi < 1) TongSoTinhPhi = 1;
                 AppFeeFixInfo _AppFeeFixInfo = new AppFeeFixInfo();
               
                #region Phí Nộp hồ sơ
                List <AppFeeFixInfo> _lstFeeFix = new List<AppFeeFixInfo>();
            
                //1.Phí nộp hồ sơ 
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Fee_Id = 200;
                _AppFeeFixInfo.Isuse = 1;
                _AppFeeFixInfo.App_Header_Id = pAppHeaderId;
                _AppFeeFixInfo.Number_Of_Patent = 1; //default là 1 
                _keyFee = TradeMarkAppCode.AppCodeDangKynhanHieu + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                else
                    _AppFeeFixInfo.Amount = 150000 * _AppFeeFixInfo.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo);

                //2.TỔNG SỐ NHÓM 
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Fee_Id = 201;
                _AppFeeFixInfo.Isuse = 1;
                _AppFeeFixInfo.App_Header_Id = pAppHeaderId;
                _AppFeeFixInfo.Number_Of_Patent = TongSoNhom;
                  _keyFee = TradeMarkAppCode.AppCodeDangKynhanHieu + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                else
                    _AppFeeFixInfo.Amount = 200000 * _AppFeeFixInfo.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo);

                //3.Tổng số sản phẩm tren nhom 
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Fee_Id = 2011;
                _AppFeeFixInfo.Isuse = 1;
                _AppFeeFixInfo.App_Header_Id = pAppHeaderId;
                _AppFeeFixInfo.Number_Of_Patent = TongSoTinhPhi;
                _keyFee = TradeMarkAppCode.AppCodeDangKynhanHieu + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                else
                    _AppFeeFixInfo.Amount = 22000 * _AppFeeFixInfo.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo);


                //4.Số đơn ưu tiên 
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Fee_Id = 203;
                _AppFeeFixInfo.Isuse = 1;
                _AppFeeFixInfo.App_Header_Id = pAppHeaderId;
                _AppFeeFixInfo.Number_Of_Patent = NumberAppNo.Split(',').Length;  
                _keyFee = TradeMarkAppCode.AppCodeDangKynhanHieu + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                else
                    _AppFeeFixInfo.Amount = 600000 * _AppFeeFixInfo.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo);


                //5.Lệ phí công bố đơn
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Fee_Id = 204;
                _AppFeeFixInfo.Isuse = 1;
                _AppFeeFixInfo.App_Header_Id = pAppHeaderId;
                _AppFeeFixInfo.Number_Of_Patent = 1; //default là 1 
                _keyFee = TradeMarkAppCode.AppCodeDangKynhanHieu + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                else
                    _AppFeeFixInfo.Amount = 120000 * _AppFeeFixInfo.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo);

                //6. Phí tra cứu phục vụ thẩm định 
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Fee_Id = 205;
                _AppFeeFixInfo.Isuse = 1;
                _AppFeeFixInfo.App_Header_Id = pAppHeaderId;
                _AppFeeFixInfo.Number_Of_Patent = TongSoNhom;
                _keyFee = TradeMarkAppCode.AppCodeDangKynhanHieu + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                else
                    _AppFeeFixInfo.Amount = 360000 * _AppFeeFixInfo.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo);


                //7.Tổng số sản phẩm tren nhom 
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Fee_Id = 2051;
                _AppFeeFixInfo.Isuse = 1;
                _AppFeeFixInfo.App_Header_Id = pAppHeaderId;
                _AppFeeFixInfo.Number_Of_Patent = TongSoTinhPhi;
                _keyFee = TradeMarkAppCode.AppCodeDangKynhanHieu + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                else
                    _AppFeeFixInfo.Amount = 30000 * _AppFeeFixInfo.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo);



                //8.Phí thẩm định đơn
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Fee_Id = 207;
                _AppFeeFixInfo.Isuse = 1;
                _AppFeeFixInfo.App_Header_Id = pAppHeaderId;
                _AppFeeFixInfo.Number_Of_Patent = 1;
                _keyFee = TradeMarkAppCode.AppCodeDangKynhanHieu + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                else
                    _AppFeeFixInfo.Amount = 550000 * _AppFeeFixInfo.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo);


                //9.Mỗi nhóm có trên 6 sản phẩm/dịch vụ (từ sản phẩm/dịch vụ thứ 7 trở đi)
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Fee_Id = 2071;
                _AppFeeFixInfo.Isuse = 1;
                _AppFeeFixInfo.App_Header_Id = pAppHeaderId;
                _AppFeeFixInfo.Number_Of_Patent = TongSoTinhPhi;
                _keyFee = TradeMarkAppCode.AppCodeDangKynhanHieu + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                else
                    _AppFeeFixInfo.Amount = 120000 * _AppFeeFixInfo.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo);

                AppFeeFixBL _AppFeeFixBL = new AppFeeFixBL();
               return  _AppFeeFixBL.AppFeeFixInsertBath(_lstFeeFix, pAppHeaderId);
                #endregion

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -3;
            }
        }
    }
}
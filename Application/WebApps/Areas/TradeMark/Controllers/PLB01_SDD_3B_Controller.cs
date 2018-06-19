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
    using Common.CommonData;
    using BussinessFacade.ModuleMemoryData;

    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("TradeMarkRegistration", AreaPrefix = "trade-mark-3b")]
    [Route("{action}")]
    public class PLB01_SDD_3B_Controller : Controller
    {
        [HttpGet]
        [Route("register/{id}")]
        public ActionResult Register()
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
                return PartialView("~/Areas/TradeMark/Views/PLB01_SDD_3B/_Partial_TM_3B_PLB_01_SDD.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/TradeMark/Views/PLB01_SDD_3B/_Partial_TM_3B_PLB_01_SDD.cshtml");
            }
        }

        [HttpPost]
        [Route("register_PLB_01_SDD")]
        public ActionResult Register_PLB_01_SDD(ApplicationHeaderInfo pInfo, App_Detail_PLB01_SDD_Info pDetail,
            List<AppDocumentInfo> pAppDocumentInfo, List<AppFeeFixInfo> pFeeFixInfo)
        {
            try
            {
                Application_Header_BL objBL = new Application_Header_BL();
                AppFeeFixBL objFeeFixBL = new AppFeeFixBL();
                App_Detail_PLB01_SDD_BL objDetail = new App_Detail_PLB01_SDD_BL();
                AppDocumentBL objDoc = new AppDocumentBL();
                if (pInfo == null || pDetail == null) return Json(new { status = ErrorCode.Error });
                string language = AppsCommon.GetCurrentLang();
                var CreatedBy = SessionData.CurrentUser.Username;
                var CreatedDate = SessionData.CurrentUser.CurrentDate;
                decimal pReturn = ErrorCode.Success;
                int pAppHeaderID = 0;

                using (var scope = new TransactionScope())
                {
                    //
                    pInfo.Languague_Code = language;
                    pInfo.Created_By = CreatedBy;
                    pInfo.Created_Date = CreatedDate;
                    pInfo.Send_Date = DateTime.Now;
                    pInfo.Status = (decimal)CommonEnums.App_Status.DaGui_ChoPhanLoai;

                    //TRA RA ID CUA BANG KHI INSERT
                    pAppHeaderID = objBL.AppHeaderInsert(pInfo);

                    // detail
                    if (pAppHeaderID >= 0)
                    {
                        pDetail.Appcode = pInfo.Appcode;
                        pDetail.Language_Code = language;
                        pDetail.App_Header_Id = pAppHeaderID;
                        pReturn = objDetail.Insert(pDetail);
                    }

                    #region Phí cố định

                    #region Phí thẩm định yêu cầu sửa đổi đơn
                    List<AppFeeFixInfo> _lstFeeFix = new List<AppFeeFixInfo>();
                    AppFeeFixInfo _AppFeeFixInfo1 = new AppFeeFixInfo();
                    _AppFeeFixInfo1.Fee_Id = 1;
                    _AppFeeFixInfo1.Isuse = 1;
                    _AppFeeFixInfo1.App_Header_Id = pAppHeaderID;
                    _AppFeeFixInfo1.Number_Of_Patent = pDetail.App_No_Change.Split(',').Length;

                    string _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo1.Fee_Id.ToString();
                    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                        _AppFeeFixInfo1.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo1.Number_Of_Patent;
                    else
                        _AppFeeFixInfo1.Amount = 160000 * _AppFeeFixInfo1.Number_Of_Patent;
                    _lstFeeFix.Add(_AppFeeFixInfo1);
                    #endregion

                    #region Phí công bố thông tin đơn sửa đổi
                    AppFeeFixInfo _AppFeeFixInfo2 = new AppFeeFixInfo();
                    _AppFeeFixInfo2.Fee_Id = 2;
                    _AppFeeFixInfo2.Isuse = 1;
                    _AppFeeFixInfo2.App_Header_Id = pAppHeaderID;
                    _AppFeeFixInfo2.Number_Of_Patent = pDetail.App_No_Change.Split(',').Length;

                    _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo2.Fee_Id.ToString();
                    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                        _AppFeeFixInfo2.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo2.Number_Of_Patent;
                    else
                        _AppFeeFixInfo2.Amount = 160000 * _AppFeeFixInfo2.Number_Of_Patent;
                    _lstFeeFix.Add(_AppFeeFixInfo2);
                    #endregion

                    #region Đơn có trên 1 hình (từ hình thứ hai trở đi)
                    AppFeeFixInfo _AppFeeFixInfo21 = new AppFeeFixInfo();
                    _AppFeeFixInfo21.Fee_Id = 21;
                    _AppFeeFixInfo21.App_Header_Id = pAppHeaderID;

                    _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo21.Fee_Id.ToString();
                    decimal _numberPicOver = 1;
                    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    {
                        _numberPicOver = Convert.ToDecimal(MemoryData.c_dic_FeeByApp_Fix[_keyFee].Char01);
                    }

                    // nếu số hình > số hình quy định
                    if (pDetail.Number_Pic > _numberPicOver)
                    {
                        _AppFeeFixInfo21.Isuse = 1;
                        _AppFeeFixInfo21.Number_Of_Patent = (pDetail.Number_Pic - _numberPicOver);

                        if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                            _AppFeeFixInfo21.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo21.Number_Of_Patent;
                        else
                            _AppFeeFixInfo21.Amount = 60000 * _AppFeeFixInfo21.Number_Of_Patent;

                    }
                    else
                    {
                        _AppFeeFixInfo21.Isuse = 0;
                        _AppFeeFixInfo21.Number_Of_Patent = 0;
                        _AppFeeFixInfo21.Amount = 0;
                    }
                    _lstFeeFix.Add(_AppFeeFixInfo21);

                    #endregion

                    #region Bản mô tả sáng chế có trên 6 trang (từ trang thứ 7 trở đi)  
                    AppFeeFixInfo _AppFeeFixInfo22 = new AppFeeFixInfo();
                    _AppFeeFixInfo22.Fee_Id = 22;
                    _AppFeeFixInfo22.App_Header_Id = pAppHeaderID;

                    _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo22.Fee_Id.ToString();
                    decimal _numberPageOver = 6;
                    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    {
                        _numberPageOver = Convert.ToDecimal(MemoryData.c_dic_FeeByApp_Fix[_keyFee].Char01);
                    }

                    // nếu số hình > số hình quy định
                    if (pDetail.Number_Page > _numberPageOver)
                    {
                        _AppFeeFixInfo22.Isuse = 1;
                        _AppFeeFixInfo22.Number_Of_Patent = (pDetail.Number_Page - _numberPageOver);

                        if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                            _AppFeeFixInfo22.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo22.Number_Of_Patent;
                        else
                            _AppFeeFixInfo22.Amount = 10000 * _AppFeeFixInfo22.Number_Of_Patent;

                    }
                    else
                    {
                        _AppFeeFixInfo22.Isuse = 0;
                        _AppFeeFixInfo22.Number_Of_Patent = 0;
                        _AppFeeFixInfo22.Amount = 0;
                    }
                    _lstFeeFix.Add(_AppFeeFixInfo22);
                    #endregion

                    AppFeeFixBL _AppFeeFixBL = new AppFeeFixBL();
                    pReturn = _AppFeeFixBL.AppFeeFixInsertBath(_lstFeeFix, pAppHeaderID);
                    #endregion

                    #region Tai lieu dinh kem 
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
                                    info.Url_Hardcopy = "~/Content/Archive/" + AppUpload.Document + pfiles.FileName;
                                    info.Status = 0;
                                }
                                info.App_Header_Id = pAppHeaderID;
                                info.Document_Filing_Date = CommonFuc.CurrentDate();
                                info.Language_Code = language;
                            }
                            pReturn = objDoc.AppDocumentInsertBath(pAppDocumentInfo, pAppHeaderID);

                        }
                    }
                    #endregion

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
        [Route("Edit_PLB_01_SDD")]
        public ActionResult Edit_PLB_01_SDD(ApplicationHeaderInfo pInfo, App_Detail_PLB01_SDD_Info pDetail,
            List<AppDocumentInfo> pAppDocumentInfo, List<AppFeeFixInfo> pFeeFixInfo)
        {
            try
            {
                Application_Header_BL objBL = new Application_Header_BL();
                AppFeeFixBL objFeeFixBL = new AppFeeFixBL();
                App_Detail_PLB01_SDD_BL objDetail = new App_Detail_PLB01_SDD_BL();
                AppDocumentBL objDoc = new AppDocumentBL();
                if (pInfo == null || pDetail == null) return Json(new { status = ErrorCode.Error });
                string language = AppsCommon.GetCurrentLang();
                var CreatedBy = SessionData.CurrentUser.Username;
                var CreatedDate = SessionData.CurrentUser.CurrentDate;
                decimal pReturn = ErrorCode.Success;


                using (var scope = new TransactionScope())
                {
                    //
                    pInfo.Languague_Code = language;
                    pInfo.Created_By = CreatedBy;
                    pInfo.Created_Date = CreatedDate;
                    pInfo.Send_Date = DateTime.Now;
                    pInfo.Status = (decimal)CommonEnums.App_Status.DaGui_ChoPhanLoai;

                    //TRA RA ID CUA BANG KHI INSERT
                    int _re = objBL.AppHeaderUpdate(pInfo);

                    // detail
                    if (_re >= 0)
                    {
                        pDetail.Appcode = pInfo.Appcode;
                        pDetail.Language_Code = language;
                        pDetail.App_Header_Id = pInfo.Id;
                        pReturn = objDetail.Update(pDetail);
                    }

                    #region Phí cố định

                    #region Phí thẩm định yêu cầu sửa đổi đơn
                    List<AppFeeFixInfo> _lstFeeFix = new List<AppFeeFixInfo>();
                    AppFeeFixInfo _AppFeeFixInfo1 = new AppFeeFixInfo();
                    _AppFeeFixInfo1.Fee_Id = 1;
                    _AppFeeFixInfo1.Isuse = 1;
                    _AppFeeFixInfo1.App_Header_Id = pInfo.Id;
                    _AppFeeFixInfo1.Number_Of_Patent = pDetail.App_No_Change.Split(',').Length;

                    string _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo1.Fee_Id.ToString();
                    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                        _AppFeeFixInfo1.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo1.Number_Of_Patent;
                    else
                        _AppFeeFixInfo1.Amount = 160000 * _AppFeeFixInfo1.Number_Of_Patent;
                    _lstFeeFix.Add(_AppFeeFixInfo1);
                    #endregion

                    #region Phí công bố thông tin đơn sửa đổi
                    AppFeeFixInfo _AppFeeFixInfo2 = new AppFeeFixInfo();
                    _AppFeeFixInfo2.Fee_Id = 2;
                    _AppFeeFixInfo2.Isuse = 1;
                    _AppFeeFixInfo2.App_Header_Id = pInfo.Id;
                    _AppFeeFixInfo2.Number_Of_Patent = pDetail.App_No_Change.Split(',').Length;

                    _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo2.Fee_Id.ToString();
                    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                        _AppFeeFixInfo2.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo2.Number_Of_Patent;
                    else
                        _AppFeeFixInfo2.Amount = 160000 * _AppFeeFixInfo2.Number_Of_Patent;
                    _lstFeeFix.Add(_AppFeeFixInfo2);
                    #endregion

                    #region Đơn có trên 1 hình (từ hình thứ hai trở đi)
                    AppFeeFixInfo _AppFeeFixInfo21 = new AppFeeFixInfo();
                    _AppFeeFixInfo21.Fee_Id = 21;
                    _AppFeeFixInfo21.App_Header_Id = pInfo.Id;

                    _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo21.Fee_Id.ToString();
                    decimal _numberPicOver = 1;
                    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    {
                        _numberPicOver = Convert.ToDecimal(MemoryData.c_dic_FeeByApp_Fix[_keyFee].Char01);
                    }

                    // nếu số hình > số hình quy định
                    if (pDetail.Number_Pic > _numberPicOver)
                    {
                        _AppFeeFixInfo21.Isuse = 1;
                        _AppFeeFixInfo21.Number_Of_Patent = (pDetail.Number_Pic - _numberPicOver);

                        if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                            _AppFeeFixInfo21.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo21.Number_Of_Patent;
                        else
                            _AppFeeFixInfo21.Amount = 60000 * _AppFeeFixInfo21.Number_Of_Patent;

                    }
                    else
                    {
                        _AppFeeFixInfo21.Isuse = 0;
                        _AppFeeFixInfo21.Number_Of_Patent = 0;
                        _AppFeeFixInfo21.Amount = 0;
                    }
                    _lstFeeFix.Add(_AppFeeFixInfo21);

                    #endregion

                    #region Bản mô tả sáng chế có trên 6 trang (từ trang thứ 7 trở đi)  
                    AppFeeFixInfo _AppFeeFixInfo22 = new AppFeeFixInfo();
                    _AppFeeFixInfo22.Fee_Id = 22;
                    _AppFeeFixInfo22.App_Header_Id = pInfo.Id;

                    _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo22.Fee_Id.ToString();
                    decimal _numberPageOver = 6;
                    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    {
                        _numberPageOver = Convert.ToDecimal(MemoryData.c_dic_FeeByApp_Fix[_keyFee].Char01);
                    }

                    // nếu số hình > số hình quy định
                    if (pDetail.Number_Page > _numberPageOver)
                    {
                        _AppFeeFixInfo22.Isuse = 1;
                        _AppFeeFixInfo22.Number_Of_Patent = (pDetail.Number_Page - _numberPageOver);

                        if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                            _AppFeeFixInfo22.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo22.Number_Of_Patent;
                        else
                            _AppFeeFixInfo22.Amount = 10000 * _AppFeeFixInfo22.Number_Of_Patent;

                    }
                    else
                    {
                        _AppFeeFixInfo22.Isuse = 0;
                        _AppFeeFixInfo22.Number_Of_Patent = 0;
                        _AppFeeFixInfo22.Amount = 0;
                    }
                    _lstFeeFix.Add(_AppFeeFixInfo22);
                    #endregion

                    // xóa đi
                    AppFeeFixBL _AppFeeFixBL = new AppFeeFixBL();
                    _AppFeeFixBL.AppFeeFixDelete(pDetail.App_Header_Id, language);

                    // insert lại fee
                    pReturn = _AppFeeFixBL.AppFeeFixInsertBath(_lstFeeFix, pInfo.Id);
                    #endregion

                    #region Tai lieu dinh kem 
                    if (pReturn >= 0 && pAppDocumentInfo != null)
                    {
                        if (pAppDocumentInfo.Count > 0)
                        {
                            // xóa đi trước
                            AppDocumentBL _AppDocumentBL = new AppDocumentBL();
                            _AppDocumentBL.AppDocumentDelByApp(pDetail.App_Header_Id, language);

                            // insert vào sau
                            List<AppDocumentInfo> Lst_AppDoc = _AppDocumentBL.AppDocument_Getby_AppHeader(pDetail.App_Header_Id, language);
                            Dictionary<string, AppDocumentInfo> dic_appDoc = new Dictionary<string, AppDocumentInfo>();
                            foreach (AppDocumentInfo item in Lst_AppDoc)
                            {
                                dic_appDoc[item.Document_Id] = item;
                            }

                            foreach (var info in pAppDocumentInfo)
                            {
                                if (SessionData.CurrentUser.chashFile.ContainsKey(info.keyFileUpload))
                                {
                                    HttpPostedFileBase pfiles = (HttpPostedFileBase)SessionData.CurrentUser.chashFile[info.keyFileUpload];
                                    info.Filename = pfiles.FileName;
                                    info.Url_Hardcopy = "~/Content/Archive/" + AppUpload.Document + pfiles.FileName;
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
                return Json(new { status = pInfo.Id });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { status = ErrorCode.Error });
            }
        }

        //[HttpPost]
        //[Route("push-file-to-server")]
        //public ActionResult PushFileToServer(AppDocumentInfo pInfo)
        //{
        //    try
        //    {
        //        if (pInfo.pfiles != null)
        //        {
        //            var url = AppLoadHelpers.PushFileToServer(pInfo.pfiles, AppUpload.Document);
        //            SessionData.CurrentUser.chashFile[pInfo.keyFileUpload] = pInfo.pfiles;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.LogException(ex);
        //        return Json(new { success = -1 });
        //    }
        //    return Json(new { success = 0 });
        //}

        [HttpPost]
        [Route("delete-file")]
        public ActionResult DeleteFile(string keyFileUpload)
        {
            try
            {
                if (SessionData.CurrentUser.chashFile.ContainsKey(keyFileUpload))
                {
                    SessionData.CurrentUser.chashFile.Remove(keyFileUpload);
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
                string fileName = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "Dang_ky_sua_doi_nhan_hieu_" + pInfo.Appcode + ".pdf");

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

        [HttpPost]
        [Route("Pre-View")]
        public ActionResult PreViewApplication()
        {
            try
            {
                return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistration/_PartialContentPreview.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistration/_PartialContentPreview.cshtml");
            }
        }
    }
}
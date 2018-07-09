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
                    //pInfo.Status = (decimal)CommonEnums.App_Status.DaGui_ChoPhanLoai;

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
                    pInfo.Modify_By = CreatedBy;
                    pInfo.Modify_Date = CreatedDate;
                    pInfo.Send_Date = DateTime.Now;

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
                            // Get ra để map sau đó xóa đi để insert vào sau
                            AppDocumentBL _AppDocumentBL = new AppDocumentBL();
                            List<AppDocumentInfo> Lst_AppDoc = _AppDocumentBL.AppDocument_Getby_AppHeader(pDetail.App_Header_Id, language);
                            Dictionary<string, AppDocumentInfo> dic_appDoc = new Dictionary<string, AppDocumentInfo>();
                            foreach (AppDocumentInfo item in Lst_AppDoc)
                            {
                                dic_appDoc[item.Document_Id] = item;
                            }

                            // xóa đi trước
                            _AppDocumentBL.AppDocumentDelByApp(pDetail.App_Header_Id, language);

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
        public ActionResult ExportData_View(decimal pAppHeaderId, string p_appCode)
        {
            try
            {
                string language = AppsCommon.GetCurrentLang();
                ApplicationHeaderInfo applicationHeaderInfo = new ApplicationHeaderInfo();
                App_Detail_PLB01_SDD_Info app_Detail = new App_Detail_PLB01_SDD_Info();
                List<AppFeeFixInfo> appFeeFixInfos = new List<AppFeeFixInfo>();
                List<AppDocumentInfo> appDocumentInfos = new List<AppDocumentInfo>();

                App_Detail_PLB01_SDD_BL objBL = new App_Detail_PLB01_SDD_BL();
                app_Detail = objBL.GetByID(pAppHeaderId, language, ref applicationHeaderInfo, ref appDocumentInfos, ref appFeeFixInfos);

                string _fileTemp = System.Web.HttpContext.Current.Server.MapPath("/Content/AppForms/B01_Request_for_amendment_of_application_vi.doc");
                DocumentModel document = DocumentModel.Load(_fileTemp);

                // Fill export_header
                string fileName = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "B01_Request_for_amendment_of_application_vi_" + p_appCode + ".pdf");

                // copy Header
                App_Detail_PLB01_SDD_Info.CopyAppHeaderInfo(ref app_Detail, applicationHeaderInfo);

                #region Tài liệu có trong đơn

                foreach (AppDocumentInfo item in appDocumentInfos)
                {
                    if (item.Document_Id == "01_SDD_01")
                    {
                        app_Detail.Doc_Id_1 = item.CHAR01;
                        app_Detail.Doc_Id_1_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "01_SDD_02")
                    {
                        app_Detail.Doc_Id_2 = item.CHAR01;
                        app_Detail.Doc_Id_2_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "01_SDD_03")
                    {
                        app_Detail.Doc_Id_3 = item.CHAR01;
                        app_Detail.Doc_Id_3_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "01_SDD_04")
                    {
                        app_Detail.Doc_Id_4 = item.CHAR01;
                        app_Detail.Doc_Id_4_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "01_SDD_05")
                    {
                        app_Detail.Doc_Id_5 = item.CHAR01;
                        app_Detail.Doc_Id_5_Check = item.Isuse;
                    }

                    else if (item.Document_Id == "01_SDD_06")
                    {
                        app_Detail.Doc_Id_6_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "01_SDD_07")
                    {
                        app_Detail.Doc_Id_7_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "01_SDD_08")
                    {
                        app_Detail.Doc_Id_8_Check = item.Isuse;
                    }

                    else if (item.Document_Id == "01_SDD_09")
                    {
                        app_Detail.Doc_Id_9 = item.CHAR01;
                        app_Detail.Doc_Id_9_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "01_SDD_10")
                    {
                        app_Detail.Doc_Id_10_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "01_SDD_11")
                    {
                        app_Detail.Doc_Id_11 = item.CHAR01;
                        app_Detail.Doc_Id_11_Check = item.Isuse;
                    }
                }

                #endregion

                #region Fee
                if (appFeeFixInfos.Count > 0)
                {
                    foreach (var item in appFeeFixInfos)
                    {
                        if (item.Fee_Id == 1)
                        {
                            app_Detail.Fee_Id_1 = item.Number_Of_Patent;
                            app_Detail.Fee_Id_1_Check = item.Isuse;
                            app_Detail.Fee_Id_1_Val = item.Amount.ToString("#,##0.##");
                        }
                        else if (item.Fee_Id == 2)
                        {
                            app_Detail.Fee_Id_2 = item.Number_Of_Patent;
                            app_Detail.Fee_Id_2_Check = item.Isuse;
                            app_Detail.Fee_Id_2_Val = item.Amount.ToString("#,##0.##");
                        }
                        else if (item.Fee_Id == 21)
                        {
                            app_Detail.Fee_Id_21 = item.Number_Of_Patent;
                            app_Detail.Fee_Id_21_Check = item.Isuse;
                            app_Detail.Fee_Id_21_Val = item.Amount.ToString("#,##0.##");
                        }
                        else if (item.Fee_Id == 22)
                        {
                            app_Detail.Fee_Id_22 = item.Number_Of_Patent;
                            app_Detail.Fee_Id_22_Check = item.Isuse;
                            app_Detail.Fee_Id_22_Val = item.Amount.ToString("#,##0.##");
                        }

                        app_Detail.Total_Fee = app_Detail.Total_Fee + item.Amount;
                        app_Detail.Total_Fee_Str = app_Detail.Total_Fee.ToString("#,##0.##");
                    }
                }
                #endregion

                document.MailMerge.Execute(app_Detail);
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

        [HttpPost]
        [Route("ket_xuat_fileIU")]
        public ActionResult ExportData_IU(ApplicationHeaderInfo pInfo, App_Detail_PLB01_SDD_Info pDetail,
            List<AppDocumentInfo> pAppDocumentInfo, List<AppFeeFixInfo> pFeeFixInfo)
        {
            try
            {
                string language = AppsCommon.GetCurrentLang();
                string _fileTemp = System.Web.HttpContext.Current.Server.MapPath("/Content/AppForms/B01_Request_for_amendment_of_application_vi.doc");
                DocumentModel document = DocumentModel.Load(_fileTemp);

                // Fill export_header
                string fileName = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "B01_Request_for_amendment_of_application_vi_" + TradeMarkAppCode.AppCode_TM_3B_PLB_01_SDD + ".pdf");

                // copy Header
                App_Detail_PLB01_SDD_Info.CopyAppHeaderInfo(ref pDetail, pInfo);

                #region Tài liệu có trong đơn

                if (pAppDocumentInfo.Count > 0)
                {
                    foreach (AppDocumentInfo item in pAppDocumentInfo)
                    {
                        if (item.Document_Id == "01_SDD_01")
                        {
                            pDetail.Doc_Id_1 = item.CHAR01;
                            pDetail.Doc_Id_1_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "01_SDD_02")
                        {
                            pDetail.Doc_Id_2 = item.CHAR01;
                            pDetail.Doc_Id_2_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "01_SDD_03")
                        {
                            pDetail.Doc_Id_3 = item.CHAR01;
                            pDetail.Doc_Id_3_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "01_SDD_04")
                        {
                            pDetail.Doc_Id_4 = item.CHAR01;
                            pDetail.Doc_Id_4_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "01_SDD_05")
                        {
                            pDetail.Doc_Id_5 = item.CHAR01;
                            pDetail.Doc_Id_5_Check = item.Isuse;
                        }

                        else if (item.Document_Id == "01_SDD_06")
                        {
                            pDetail.Doc_Id_6_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "01_SDD_07")
                        {
                            pDetail.Doc_Id_7_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "01_SDD_08")
                        {
                            pDetail.Doc_Id_8_Check = item.Isuse;
                        }

                        else if (item.Document_Id == "01_SDD_09")
                        {
                            pDetail.Doc_Id_9 = item.CHAR01;
                            pDetail.Doc_Id_9_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "01_SDD_10")
                        {
                            pDetail.Doc_Id_10_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "01_SDD_11")
                        {
                            pDetail.Doc_Id_11 = item.CHAR01;
                            pDetail.Doc_Id_11_Check = item.Isuse;
                        }
                    }
                }

                #endregion

                #region Phí cố định

                #region Phí thẩm định yêu cầu sửa đổi đơn
                AppFeeFixInfo _AppFeeFixInfo1 = new AppFeeFixInfo();
                _AppFeeFixInfo1.Isuse = 1;
                _AppFeeFixInfo1.Number_Of_Patent = pDetail.App_No_Change.Split(',').Length;

                string _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo1.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    _AppFeeFixInfo1.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo1.Number_Of_Patent;
                else
                    _AppFeeFixInfo1.Amount = 160000 * _AppFeeFixInfo1.Number_Of_Patent;

                pDetail.Fee_Id_1 = _AppFeeFixInfo1.Number_Of_Patent;
                pDetail.Fee_Id_1_Check = _AppFeeFixInfo1.Isuse;
                pDetail.Fee_Id_1_Val = _AppFeeFixInfo1.Amount.ToString("#,##0.##");
                pDetail.Total_Fee = pDetail.Total_Fee + _AppFeeFixInfo1.Amount;

                #endregion

                #region Phí công bố thông tin đơn sửa đổi
                AppFeeFixInfo _AppFeeFixInfo2 = new AppFeeFixInfo();
                _AppFeeFixInfo2.Isuse = 1;
                _AppFeeFixInfo2.Number_Of_Patent = pDetail.App_No_Change.Split(',').Length;

                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo2.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    _AppFeeFixInfo2.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo2.Number_Of_Patent;
                else
                    _AppFeeFixInfo2.Amount = 160000 * _AppFeeFixInfo2.Number_Of_Patent;

                pDetail.Fee_Id_2 = _AppFeeFixInfo2.Number_Of_Patent;
                pDetail.Fee_Id_2_Check = _AppFeeFixInfo2.Isuse;
                pDetail.Fee_Id_2_Val = _AppFeeFixInfo2.Amount.ToString("#,##0.##");
                pDetail.Total_Fee = pDetail.Total_Fee + _AppFeeFixInfo2.Amount;

                #endregion

                #region Đơn có trên 1 hình (từ hình thứ hai trở đi)
                AppFeeFixInfo _AppFeeFixInfo21 = new AppFeeFixInfo();
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

                pDetail.Fee_Id_21 = _AppFeeFixInfo21.Number_Of_Patent;
                pDetail.Fee_Id_21_Check = _AppFeeFixInfo21.Isuse;
                pDetail.Fee_Id_21_Val = _AppFeeFixInfo21.Amount.ToString("#,##0.##");
                pDetail.Total_Fee = pDetail.Total_Fee + _AppFeeFixInfo21.Amount;

                #endregion

                #region Bản mô tả sáng chế có trên 6 trang (từ trang thứ 7 trở đi)  
                AppFeeFixInfo _AppFeeFixInfo22 = new AppFeeFixInfo();
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
                pDetail.Fee_Id_22 = _AppFeeFixInfo22.Number_Of_Patent;
                pDetail.Fee_Id_22_Check = _AppFeeFixInfo22.Isuse;
                pDetail.Fee_Id_22_Val = _AppFeeFixInfo22.Amount.ToString("#,##0.##");
                pDetail.Total_Fee = pDetail.Total_Fee + _AppFeeFixInfo22.Amount;

                pDetail.Total_Fee_Str = pDetail.Total_Fee.ToString("#,##0.##");

                #endregion
                #endregion

                document.MailMerge.Execute(pDetail);
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

        [Route("Pre-View")]
        public ActionResult PreViewApplication(string p_appCode)
        {
            try
            {
                ViewBag.FileName = "/Content/Export/" + "B01_Request_for_amendment_of_application_vi_" + TradeMarkAppCode.AppCode_TM_3B_PLB_01_SDD + ".pdf";
                return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistration/_PartialContentPreview.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistration/_PartialContentPreview.cshtml");
            }
        }

        [HttpPost]
        [Route("getMasterByAppNo")]
        public ActionResult getMasterByAppNo(string p_appNo)
        {
            try
            {
                Application_Header_BL _bl = new Application_Header_BL();
                ApplicationHeaderInfo objAppHeaderInfo = _bl.GetMasterByAppNo(p_appNo, SessionData.CurrentUser.Username, AppsCommon.GetCurrentLang());
                ViewBag.objAppHeaderInfo = objAppHeaderInfo;

                var PartialThongTinChuDon = AppsCommon.RenderRazorViewToString(this.ControllerContext, "~/Areas/TradeMark/Views/Shared/_PartialThongTinChuDon.cshtml");
                var PartialThongTinDaiDienChuDon = AppsCommon.RenderRazorViewToString(this.ControllerContext, "~/Areas/TradeMark/Views/Shared/_PartialThongTinDaiDienChuDon.cshtml");
                
                var json = Json(new { PartialThongTinChuDon, PartialThongTinDaiDienChuDon });
                return json;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/TradeMark/Views/Shared/_PartialThongTinChuDon.cshtml");
            }
        }
    }
}
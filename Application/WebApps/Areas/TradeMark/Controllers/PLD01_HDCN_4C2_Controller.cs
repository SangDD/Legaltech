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
    [RouteArea("TradeMarkRegistration", AreaPrefix = "trade-mark-4c2")]
    [Route("{action}")]
    public class PLD01_HDCN_4C2_Controller : Controller
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
                return PartialView("~/Areas/TradeMark/Views/PLD01_HDCN_4C2/_Partial_TM_4C2_PLD_01_HDCN.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/TradeMark/Views/PLD01_HDCN_4C2/_Partial_TM_4C2_PLD_01_HDCN.cshtml");
            }
        }

        [HttpPost]
        [Route("register_PLD01_HDCN_4C2")]
        public ActionResult Register_PLD01_HDCN_4C2(ApplicationHeaderInfo pInfo, App_Detail_PLD01_HDCN_Info pDetail,
            List<AppDocumentInfo> pAppDocumentInfo, List<AppFeeFixInfo> pFeeFixInfo)
        {
            try
            {
                Application_Header_BL objBL = new Application_Header_BL();
                AppFeeFixBL objFeeFixBL = new AppFeeFixBL();
                App_Detail_PLD01_HDCN_BL objDetail_BL = new App_Detail_PLD01_HDCN_BL();
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
                        pReturn = objDetail_BL.Insert(pDetail);
                        if (pReturn <= 0)
                            goto Commit_Transaction;
                    }
                    else goto Commit_Transaction;

                    #region Phí cố định

                    #region Phí thẩm định hồ sơ đăng ký hợp đồng chuyển nhượng 
                    List<AppFeeFixInfo> _lstFeeFix = new List<AppFeeFixInfo>();
                    AppFeeFixInfo _AppFeeFixInfo1 = new AppFeeFixInfo();
                    _AppFeeFixInfo1.Fee_Id = 1;
                    _AppFeeFixInfo1.Isuse = 1;
                    _AppFeeFixInfo1.App_Header_Id = pAppHeaderID;
                    _AppFeeFixInfo1.Number_Of_Patent = pDetail.Object_Contract_No.Split(';').Length;

                    string _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo1.Fee_Id.ToString();
                    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                        _AppFeeFixInfo1.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo1.Number_Of_Patent;
                    else
                        _AppFeeFixInfo1.Amount = 160000 * _AppFeeFixInfo1.Number_Of_Patent;
                    _lstFeeFix.Add(_AppFeeFixInfo1);
                    #endregion

                    #region Phí tra cứu nhãn hiệu liên kết phục vụ việc thẩm định hồ sơ đăng ký hợp đồng chuyển nhượng 
                    AppFeeFixInfo _AppFeeFixInfo2 = new AppFeeFixInfo();
                    _AppFeeFixInfo2.Fee_Id = 2;
                    _AppFeeFixInfo2.Isuse = 1;
                    _AppFeeFixInfo2.App_Header_Id = pAppHeaderID;
                    _AppFeeFixInfo2.Number_Of_Patent = pDetail.Object_Contract_No.Split(';').Length;

                    _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo2.Fee_Id.ToString();
                    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                        _AppFeeFixInfo2.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo2.Number_Of_Patent;
                    else
                        _AppFeeFixInfo2.Amount = 160000 * _AppFeeFixInfo2.Number_Of_Patent;
                    _lstFeeFix.Add(_AppFeeFixInfo2);
                    #endregion

                    #region Phí đăng bạ quyết định ghi nhận chuyển nhượng quyền SHCN
                    AppFeeFixInfo _AppFeeFixInfo5 = new AppFeeFixInfo();
                    _AppFeeFixInfo5.Fee_Id = 5;
                    _AppFeeFixInfo5.Isuse = 1;
                    _AppFeeFixInfo5.App_Header_Id = pAppHeaderID;
                    _AppFeeFixInfo5.Number_Of_Patent = pDetail.Object_Contract_No.Split(';').Length;

                    _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo5.Fee_Id.ToString();
                    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                        _AppFeeFixInfo5.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo5.Number_Of_Patent;
                    else
                        _AppFeeFixInfo5.Amount = 160000 * _AppFeeFixInfo5.Number_Of_Patent;
                    _lstFeeFix.Add(_AppFeeFixInfo5);
                    #endregion

                    #region Phí công bố quyết định ghi nhận chuyển nhượng quyền SHCN
                    AppFeeFixInfo _AppFeeFixInfo6 = new AppFeeFixInfo();
                    _AppFeeFixInfo6.Fee_Id = 6;
                    _AppFeeFixInfo6.Isuse = 1;
                    _AppFeeFixInfo6.App_Header_Id = pAppHeaderID;
                    _AppFeeFixInfo6.Number_Of_Patent = 1;

                    _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo6.Fee_Id.ToString();
                    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                        _AppFeeFixInfo6.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo6.Number_Of_Patent;
                    else
                        _AppFeeFixInfo6.Amount = 160000 * _AppFeeFixInfo6.Number_Of_Patent;
                    _lstFeeFix.Add(_AppFeeFixInfo6);
                    #endregion

                    AppFeeFixBL _AppFeeFixBL = new AppFeeFixBL();
                    pReturn = _AppFeeFixBL.AppFeeFixInsertBath(_lstFeeFix, pAppHeaderID);
                    #endregion

                    #region Tai lieu dinh kem 
                    if (pReturn >= 0)
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
                    else goto Commit_Transaction;
                    #endregion

                    //end
                    Commit_Transaction:
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
        [Route("Edit_PLD01_HDCN_4C2")]
        public ActionResult Edit_PLD01_HDCN_4C2(ApplicationHeaderInfo pInfo, App_Detail_PLD01_HDCN_Info pDetail,
            List<AppDocumentInfo> pAppDocumentInfo, List<AppFeeFixInfo> pFeeFixInfo)
        {
            try
            {
                Application_Header_BL objBL = new Application_Header_BL();
                AppFeeFixBL objFeeFixBL = new AppFeeFixBL();
                App_Detail_PLD01_HDCN_BL objDetail = new App_Detail_PLD01_HDCN_BL();
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
                        if (pReturn <= 0)
                            goto Commit_Transaction;
                    }
                    else goto Commit_Transaction;

                    #region Phí cố định

                    #region Phí thẩm định hồ sơ đăng ký hợp đồng chuyển nhượng 
                    List<AppFeeFixInfo> _lstFeeFix = new List<AppFeeFixInfo>();
                    AppFeeFixInfo _AppFeeFixInfo1 = new AppFeeFixInfo();
                    _AppFeeFixInfo1.Fee_Id = 1;
                    _AppFeeFixInfo1.Isuse = 1;
                    _AppFeeFixInfo1.App_Header_Id = pInfo.Id;
                    _AppFeeFixInfo1.Number_Of_Patent = pDetail.Object_Contract_No.Split(';').Length;

                    string _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo1.Fee_Id.ToString();
                    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                        _AppFeeFixInfo1.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo1.Number_Of_Patent;
                    else
                        _AppFeeFixInfo1.Amount = 160000 * _AppFeeFixInfo1.Number_Of_Patent;
                    _lstFeeFix.Add(_AppFeeFixInfo1);
                    #endregion

                    #region Phí tra cứu nhãn hiệu liên kết phục vụ việc thẩm định hồ sơ đăng ký hợp đồng chuyển nhượng 
                    AppFeeFixInfo _AppFeeFixInfo2 = new AppFeeFixInfo();
                    _AppFeeFixInfo2.Fee_Id = 2;
                    _AppFeeFixInfo2.Isuse = 1;
                    _AppFeeFixInfo2.App_Header_Id = pInfo.Id;
                    _AppFeeFixInfo2.Number_Of_Patent = pDetail.Object_Contract_No.Split(';').Length;

                    _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo2.Fee_Id.ToString();
                    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                        _AppFeeFixInfo2.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo2.Number_Of_Patent;
                    else
                        _AppFeeFixInfo2.Amount = 160000 * _AppFeeFixInfo2.Number_Of_Patent;
                    _lstFeeFix.Add(_AppFeeFixInfo2);
                    #endregion

                    #region Phí đăng bạ quyết định ghi nhận chuyển nhượng quyền SHCN
                    AppFeeFixInfo _AppFeeFixInfo5 = new AppFeeFixInfo();
                    _AppFeeFixInfo5.Fee_Id = 5;
                    _AppFeeFixInfo5.Isuse = 1;
                    _AppFeeFixInfo5.App_Header_Id = pInfo.Id;
                    _AppFeeFixInfo5.Number_Of_Patent = pDetail.Object_Contract_No.Split(';').Length;

                    _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo5.Fee_Id.ToString();
                    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                        _AppFeeFixInfo5.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo5.Number_Of_Patent;
                    else
                        _AppFeeFixInfo5.Amount = 160000 * _AppFeeFixInfo5.Number_Of_Patent;
                    _lstFeeFix.Add(_AppFeeFixInfo5);
                    #endregion

                    #region Phí công bố quyết định ghi nhận chuyển nhượng quyền SHCN
                    AppFeeFixInfo _AppFeeFixInfo6 = new AppFeeFixInfo();
                    _AppFeeFixInfo6.Fee_Id = 6;
                    _AppFeeFixInfo6.Isuse = 1;
                    _AppFeeFixInfo6.App_Header_Id = pInfo.Id;
                    _AppFeeFixInfo6.Number_Of_Patent = 1;

                    _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo6.Fee_Id.ToString();
                    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                        _AppFeeFixInfo6.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo6.Number_Of_Patent;
                    else
                        _AppFeeFixInfo6.Amount = 160000 * _AppFeeFixInfo6.Number_Of_Patent;
                    _lstFeeFix.Add(_AppFeeFixInfo6);
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
                    Commit_Transaction:
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
        [Route("ket_xuat_file")]
        public ActionResult ExportData_View(decimal pAppHeaderId, string p_appCode)
        {
            try
            {
                string language = AppsCommon.GetCurrentLang();
                ApplicationHeaderInfo applicationHeaderInfo = new ApplicationHeaderInfo();
                App_Detail_PLD01_HDCN_Info app_Detail = new App_Detail_PLD01_HDCN_Info();
                List<AppFeeFixInfo> appFeeFixInfos = new List<AppFeeFixInfo>();
                List<AppDocumentInfo> appDocumentInfos = new List<AppDocumentInfo>();

                App_Detail_PLD01_HDCN_BL objBL = new App_Detail_PLD01_HDCN_BL();
                app_Detail = objBL.GetByID(pAppHeaderId, language, ref applicationHeaderInfo, ref appDocumentInfos, ref appFeeFixInfos);

                string _fileTemp = System.Web.HttpContext.Current.Server.MapPath("/Content/AppForms/D01_VI.doc");
                DocumentModel document = DocumentModel.Load(_fileTemp);

                // Fill export_header
                string fileName = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "D01_VI_" + p_appCode + ".pdf");

                // copy Header
                App_Detail_PLD01_HDCN_Info.CopyAppHeaderInfo(ref app_Detail, applicationHeaderInfo);

                document.MailMerge.FieldMerging += (sender, e) =>
                {
                    if (e.IsValueFound)
                    {
                        if (e.FieldName == "Customer_Code")
                            e.Inline = new TextBox(e.Document, Layout.Floating(
                                new HorizontalPosition(HorizontalPositionType.Absolute, HorizontalPositionAnchor.Margin),
                                new VerticalPosition(VerticalPositionType.Absolute, VerticalPositionAnchor.Margin),
                                new Size(4, 3.5, LengthUnit.Centimeter)),
                                new Paragraph(document));
                    }
                };
                document.MailMerge.Execute(new { Customer_Code = app_Detail.Customer_Code });

                var textBox = new TextBox(document,
                    Layout.Floating(
                        new HorizontalPosition(HorizontalPositionType.Right, HorizontalPositionAnchor.Margin),
                        new VerticalPosition(VerticalPositionType.Bottom, VerticalPositionAnchor.Margin),
                        new Size(4, 3.5, LengthUnit.Centimeter)),
                    new Paragraph(document, app_Detail.Customer_Code));

                #region Tài liệu có trong đơn

                foreach (AppDocumentInfo item in appDocumentInfos)
                {
                    if (item.Document_Id == "PLD01_HDCB_01")
                    {
                        app_Detail.Doc_Id_1 = item.CHAR01;
                        app_Detail.Doc_Id_1_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "PLD01_HDCB_02")
                    {
                        app_Detail.Doc_Id_2 = item.CHAR01;
                        app_Detail.Doc_Id_2_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "PLD01_HDCB_03")
                    {
                        app_Detail.Doc_Id_3 = item.CHAR01;
                        app_Detail.Doc_Id_3_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "PLD01_HDCB_04")
                    {
                        app_Detail.Doc_Id_4 = item.CHAR01;
                        app_Detail.Doc_Id_4_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "PLD01_HDCB_05")
                    {
                        app_Detail.Doc_Id_5 = item.CHAR01;
                        app_Detail.Doc_Id_5_Check = item.Isuse;
                    }

                    else if (item.Document_Id == "PLD01_HDCB_06")
                    {
                        app_Detail.Doc_Id_6 = item.CHAR01;
                        app_Detail.Doc_Id_6_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "PLD01_HDCB_07")
                    {
                        app_Detail.Doc_Id_7 = item.CHAR01;
                        app_Detail.Doc_Id_7_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "PLD01_HDCB_08")
                    {
                        app_Detail.Doc_Id_8 = item.CHAR01;
                        app_Detail.Doc_Id_8_Check = item.Isuse;
                    }

                    else if (item.Document_Id == "PLD01_HDCB_09")
                    {
                        app_Detail.Doc_Id_9 = item.CHAR01;
                        app_Detail.Doc_Id_9_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "PLD01_HDCB_010")
                    {
                        app_Detail.Doc_Id_10 = item.CHAR01;
                        app_Detail.Doc_Id_10_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "PLD01_HDCB_11")
                    {
                        app_Detail.Doc_Id_11 = item.CHAR01;
                        app_Detail.Doc_Id_11_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "PLD01_HDCB_12")
                    {
                        app_Detail.Doc_Id_12 = item.CHAR01;
                        app_Detail.Doc_Id_12_Check = item.Isuse;
                    }

                    else if (item.Document_Id == "PLD01_HDCB_13")
                    {
                        app_Detail.Doc_Id_13 = item.CHAR01;
                        app_Detail.Doc_Id_13_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "PLD01_HDCB_14")
                    {
                        app_Detail.Doc_Id_14 = item.CHAR01;
                        app_Detail.Doc_Id_14_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "PLD01_HDCB_15")
                    {
                        app_Detail.Doc_Id_15 = item.CHAR01;
                        app_Detail.Doc_Id_15_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "PLD01_HDCB_16")
                    {
                        app_Detail.Doc_Id_16 = item.CHAR01;
                        app_Detail.Doc_Id_16_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "PLD01_HDCB_17")
                    {
                        app_Detail.Doc_Id_17 = item.CHAR01;
                        app_Detail.Doc_Id_17_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "PLD01_HDCB_18")
                    {
                        app_Detail.Doc_Id_18 = item.CHAR01;
                        app_Detail.Doc_Id_18_Check = item.Isuse;
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
                        else if (item.Fee_Id == 3)
                        {
                            app_Detail.Fee_Id_3 = item.Number_Of_Patent;
                            app_Detail.Fee_Id_3_Check = item.Isuse;
                            app_Detail.Fee_Id_3_Val = item.Amount.ToString("#,##0.##");
                        }
                        else if (item.Fee_Id == 4)
                        {
                            app_Detail.Fee_Id_4 = item.Number_Of_Patent;
                            app_Detail.Fee_Id_4_Check = item.Isuse;
                            app_Detail.Fee_Id_4_Val = item.Amount.ToString("#,##0.##");
                        }
                        else if (item.Fee_Id == 5)
                        {
                            app_Detail.Fee_Id_5 = item.Number_Of_Patent;
                            app_Detail.Fee_Id_5_Check = item.Isuse;
                            app_Detail.Fee_Id_5_Val = item.Amount.ToString("#,##0.##");
                        }
                        else if (item.Fee_Id == 6)
                        {
                            app_Detail.Fee_Id_6 = item.Number_Of_Patent;
                            app_Detail.Fee_Id_6_Check = item.Isuse;
                            app_Detail.Fee_Id_6_Val = item.Amount.ToString("#,##0.##");
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
        public ActionResult ExportData_IU(ApplicationHeaderInfo pInfo, App_Detail_PLD01_HDCN_Info pDetail,
            List<AppDocumentInfo> pAppDocumentInfo, List<AppFeeFixInfo> pFeeFixInfo)
        {
            try
            {
                string language = AppsCommon.GetCurrentLang();
                string _fileTemp = System.Web.HttpContext.Current.Server.MapPath("/Content/AppForms/D01_VI.doc");
                DocumentModel document = DocumentModel.Load(_fileTemp);

                // Fill export_header
                string fileName = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "D01_VI_" + TradeMarkAppCode.AppCode_TM_4C2_PLD_01_HDCN + ".pdf");

                // copy Header
                App_Detail_PLD01_HDCN_Info.CopyAppHeaderInfo(ref pDetail, pInfo);

                #region Tài liệu có trong đơn

                if (pAppDocumentInfo.Count > 0)
                {
                    foreach (AppDocumentInfo item in pAppDocumentInfo)
                    {
                        if (item.Document_Id == "PLD01_HDCB_01")
                        {
                            pDetail.Doc_Id_1 = item.CHAR01;
                            pDetail.Doc_Id_1_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "PLD01_HDCB_02")
                        {
                            pDetail.Doc_Id_2 = item.CHAR01;
                            pDetail.Doc_Id_2_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "PLD01_HDCB_03")
                        {
                            pDetail.Doc_Id_3 = item.CHAR01;
                            pDetail.Doc_Id_3_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "PLD01_HDCB_04")
                        {
                            pDetail.Doc_Id_4 = item.CHAR01;
                            pDetail.Doc_Id_4_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "PLD01_HDCB_05")
                        {
                            pDetail.Doc_Id_5 = item.CHAR01;
                            pDetail.Doc_Id_5_Check = item.Isuse;
                        }

                        else if (item.Document_Id == "PLD01_HDCB_06")
                        {
                            pDetail.Doc_Id_6_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "PLD01_HDCB_07")
                        {
                            pDetail.Doc_Id_7_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "PLD01_HDCB_08")
                        {
                            pDetail.Doc_Id_8_Check = item.Isuse;
                        }

                        else if (item.Document_Id == "PLD01_HDCB_09")
                        {
                            pDetail.Doc_Id_9 = item.CHAR01;
                            pDetail.Doc_Id_9_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "PLD01_HDCB_010")
                        {
                            pDetail.Doc_Id_10_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "PLD01_HDCB_11")
                        {
                            pDetail.Doc_Id_11 = item.CHAR01;
                            pDetail.Doc_Id_11_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "PLD01_HDCB_12")
                        {
                            pDetail.Doc_Id_12 = item.CHAR01;
                            pDetail.Doc_Id_12_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "PLD01_HDCB_13")
                        {
                            pDetail.Doc_Id_13 = item.CHAR01;
                            pDetail.Doc_Id_13_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "PLD01_HDCB_14")
                        {
                            pDetail.Doc_Id_14 = item.CHAR01;
                            pDetail.Doc_Id_14_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "PLD01_HDCB_15")
                        {
                            pDetail.Doc_Id_15 = item.CHAR01;
                            pDetail.Doc_Id_15_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "PLD01_HDCB_16")
                        {
                            pDetail.Doc_Id_16 = item.CHAR01;
                            pDetail.Doc_Id_16_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "PLD01_HDCB_17")
                        {
                            pDetail.Doc_Id_17 = item.CHAR01;
                            pDetail.Doc_Id_17_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "PLD01_HDCB_18")
                        {
                            pDetail.Doc_Id_18 = item.CHAR01;
                            pDetail.Doc_Id_18_Check = item.Isuse;
                        }
                    }
                }

                #endregion

                #region Phí cố định

                #region Phí thẩm định hồ sơ đăng ký hợp đồng chuyển nhượng 
                AppFeeFixInfo _AppFeeFixInfo1 = new AppFeeFixInfo();
                _AppFeeFixInfo1.Isuse = 1;
                _AppFeeFixInfo1.Number_Of_Patent = pDetail.Object_Contract_No.Split(';').Length;

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

                #region Phí tra cứu nhãn hiệu liên kết phục vụ việc thẩm định hồ sơ đăng ký hợp đồng chuyển nhượng 
                AppFeeFixInfo _AppFeeFixInfo2 = new AppFeeFixInfo();
                _AppFeeFixInfo2.Isuse = 1;
                _AppFeeFixInfo2.Number_Of_Patent = pDetail.Object_Contract_No.Split(';').Length;

                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo2.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    _AppFeeFixInfo2.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo2.Number_Of_Patent;
                else
                    _AppFeeFixInfo2.Amount = 160000 * _AppFeeFixInfo2.Number_Of_Patent;

                pDetail.Fee_Id_2 = _AppFeeFixInfo2.Number_Of_Patent;
                pDetail.Fee_Id_2_Check = _AppFeeFixInfo2.Isuse;
                pDetail.Fee_Id_2_Val = _AppFeeFixInfo2.Amount.ToString("#,##0.##");
                pDetail.Total_Fee = pDetail.Total_Fee + _AppFeeFixInfo2.Amount;

                pDetail.Total_Fee_Str = pDetail.Total_Fee.ToString("#,##0.##");
                #endregion

                #region Phí đăng bạ quyết định ghi nhận chuyển nhượng quyền SHCN
                AppFeeFixInfo _AppFeeFixInfo5 = new AppFeeFixInfo();
                _AppFeeFixInfo5.Isuse = 1;
                _AppFeeFixInfo5.Number_Of_Patent = pDetail.Object_Contract_No.Split(';').Length;

                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo5.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    _AppFeeFixInfo5.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo5.Number_Of_Patent;
                else
                    _AppFeeFixInfo5.Amount = 160000 * _AppFeeFixInfo5.Number_Of_Patent;

                pDetail.Fee_Id_2 = _AppFeeFixInfo5.Number_Of_Patent;
                pDetail.Fee_Id_2_Check = _AppFeeFixInfo5.Isuse;
                pDetail.Fee_Id_2_Val = _AppFeeFixInfo5.Amount.ToString("#,##0.##");
                pDetail.Total_Fee = pDetail.Total_Fee + _AppFeeFixInfo5.Amount;

                pDetail.Total_Fee_Str = pDetail.Total_Fee.ToString("#,##0.##");
                #endregion

                #region Phí tra cứu nhãn hiệu liên kết phục vụ việc thẩm định hồ sơ đăng ký hợp đồng chuyển nhượng 
                AppFeeFixInfo _AppFeeFixInfo6 = new AppFeeFixInfo();
                _AppFeeFixInfo6.Isuse = 1;
                _AppFeeFixInfo2.Number_Of_Patent = 1;

                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo6.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    _AppFeeFixInfo6.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo6.Number_Of_Patent;
                else
                    _AppFeeFixInfo6.Amount = 160000 * _AppFeeFixInfo6.Number_Of_Patent;

                pDetail.Fee_Id_2 = _AppFeeFixInfo6.Number_Of_Patent;
                pDetail.Fee_Id_2_Check = _AppFeeFixInfo6.Isuse;
                pDetail.Fee_Id_2_Val = _AppFeeFixInfo6.Amount.ToString("#,##0.##");
                pDetail.Total_Fee = pDetail.Total_Fee + _AppFeeFixInfo6.Amount;

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
                ViewBag.FileName = "/Content/Export/" + "D01_VI_" + TradeMarkAppCode.AppCode_TM_4C2_PLD_01_HDCN + ".pdf";
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
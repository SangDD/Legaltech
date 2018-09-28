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
    using System.Data;
    using System.Linq;

    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("TradeMarkRegistration", AreaPrefix = "trade-mark-3c")]
    [Route("{action}")]
    public class PLB02_CGD_3C_Controller : Controller
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
                return PartialView("~/Areas/TradeMark/Views/PLB02_CGD_3C/_Partial_TM_3C_PLB_02_SDD.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/TradeMark/Views/PLB02_CGD_3C/_Partial_TM_3C_PLB_02_SDD.cshtml");
            }
        }

        [HttpPost]
        [Route("register_PLB_02_CGD")]
        public ActionResult Register_PLB_02_CGD(ApplicationHeaderInfo pInfo, App_Detail_PLB02_CGD_Info pDetail,
            List<AppDocumentInfo> pAppDocumentInfo, List<AppFeeFixInfo> pFeeFixInfo)
        {
            try
            {
                Application_Header_BL objBL = new Application_Header_BL();
                AppFeeFixBL objFeeFixBL = new AppFeeFixBL();
                App_Detail_Plb02_CGD_BL objDetail_BL = new App_Detail_Plb02_CGD_BL();
                AppDocumentBL objDoc = new AppDocumentBL();
                if (pInfo == null || pDetail == null) return Json(new { status = ErrorCode.Error });
                string language = AppsCommon.GetCurrentLang();
                var CreatedBy = SessionData.CurrentUser.Username;
                var CreatedDate = SessionData.CurrentUser.CurrentDate;
                decimal pReturn = ErrorCode.Success;
                int pAppHeaderID = 0;
                string p_case_code = "";

                using (var scope = new TransactionScope())
                {
                    //
                    pInfo.Languague_Code = language;
                    if (pInfo.Created_By == null || pInfo.Created_By == "0" || pInfo.Created_By == "")
                    {
                        pInfo.Created_By = CreatedBy;
                    }
                    pInfo.Created_Date = CreatedDate;
                    pInfo.Send_Date = DateTime.Now;
                    //pInfo.Status = (decimal)CommonEnums.App_Status.DaGui_ChoPhanLoai;

                    //TRA RA ID CUA BANG KHI INSERT
                    pAppHeaderID = objBL.AppHeaderInsert(pInfo, ref p_case_code);

                    // detail
                    if (pAppHeaderID >= 0)
                    {
                        pDetail.Appcode = pInfo.Appcode;
                        pDetail.Language_Code = language;
                        pDetail.App_Header_Id = pAppHeaderID;
                        pDetail.Case_Code = p_case_code;
                        pReturn = objDetail_BL.Insert(pDetail);
                        if (pReturn <= 0)
                            goto Commit_Transaction;
                    }
                    else goto Commit_Transaction;

                    #region Phí cố định

                    #region Phí thẩm định yêu cầu sửa đổi đơn
                    List<AppFeeFixInfo> _lstFeeFix = new List<AppFeeFixInfo>();
                    AppFeeFixInfo _AppFeeFixInfo1 = new AppFeeFixInfo();
                    _AppFeeFixInfo1.Fee_Id = 1;
                    _AppFeeFixInfo1.Isuse = 1;
                    //_AppFeeFixInfo1.App_Header_Id = pAppHeaderID;
                    _AppFeeFixInfo1.Number_Of_Patent = pDetail.Transfer_Appno.Split(',').Length;

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
                    //_AppFeeFixInfo2.App_Header_Id = pAppHeaderID;
                    _AppFeeFixInfo2.Number_Of_Patent = pDetail.Transfer_Appno.Split(',').Length;

                    _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo2.Fee_Id.ToString();
                    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                        _AppFeeFixInfo2.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo2.Number_Of_Patent;
                    else
                        _AppFeeFixInfo2.Amount = 160000 * _AppFeeFixInfo2.Number_Of_Patent;
                    _lstFeeFix.Add(_AppFeeFixInfo2);
                    #endregion

                    AppFeeFixBL _AppFeeFixBL = new AppFeeFixBL();
                    pReturn = _AppFeeFixBL.AppFeeFixInsertBath(_lstFeeFix, p_case_code);
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
        [Route("Edit_PLB_02_CGD")]
        public ActionResult Edit_PLB_02_CGD(ApplicationHeaderInfo pInfo, App_Detail_PLB02_CGD_Info pDetail,
            List<AppDocumentInfo> pAppDocumentInfo, List<AppFeeFixInfo> pFeeFixInfo)
        {
            try
            {
                Application_Header_BL objBL = new Application_Header_BL();
                AppFeeFixBL objFeeFixBL = new AppFeeFixBL();
                App_Detail_Plb02_CGD_BL objDetail = new App_Detail_Plb02_CGD_BL();
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
                        pDetail.Case_Code = pInfo.Case_Code;
                        pReturn = objDetail.Update(pDetail);
                        if (pReturn <= 0)
                            goto Commit_Transaction;
                    }
                    else goto Commit_Transaction;

                    #region Phí cố định

                    #region Phí thẩm định yêu cầu sửa đổi đơn
                    List<AppFeeFixInfo> _lstFeeFix = new List<AppFeeFixInfo>();
                    AppFeeFixInfo _AppFeeFixInfo1 = new AppFeeFixInfo();
                    _AppFeeFixInfo1.Fee_Id = 1;
                    _AppFeeFixInfo1.Isuse = 1;
                    //_AppFeeFixInfo1.App_Header_Id = pInfo.Id;
                    _AppFeeFixInfo1.Number_Of_Patent = pDetail.Transfer_Appno.Split(',').Length;

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
                    //_AppFeeFixInfo2.App_Header_Id = pInfo.Id;
                    _AppFeeFixInfo2.Number_Of_Patent = pDetail.Transfer_Appno.Split(',').Length;

                    _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo2.Fee_Id.ToString();
                    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                        _AppFeeFixInfo2.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo2.Number_Of_Patent;
                    else
                        _AppFeeFixInfo2.Amount = 160000 * _AppFeeFixInfo2.Number_Of_Patent;
                    _lstFeeFix.Add(_AppFeeFixInfo2);
                    #endregion

                    // xóa đi
                    AppFeeFixBL _AppFeeFixBL = new AppFeeFixBL();
                    _AppFeeFixBL.AppFeeFixDelete(pDetail.Case_Code, language);

                    // insert lại fee
                    pReturn = _AppFeeFixBL.AppFeeFixInsertBath(_lstFeeFix, pInfo.Case_Code);
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
                App_Detail_PLB02_CGD_Info app_Detail = new App_Detail_PLB02_CGD_Info();
                List<AppFeeFixInfo> appFeeFixInfos = new List<AppFeeFixInfo>();
                List<AppDocumentInfo> appDocumentInfos = new List<AppDocumentInfo>();

                App_Detail_Plb02_CGD_BL objBL = new App_Detail_Plb02_CGD_BL();
                app_Detail = objBL.GetByID(pAppHeaderId, language, ref applicationHeaderInfo, ref appDocumentInfos, ref appFeeFixInfos);

                // Fill export_header
                string fileName_doc = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "B02_VI_" + p_appCode + ".docx");
                string fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "B02_VI_" + p_appCode + ".pdf");

                // copy Header
                App_Detail_PLB02_CGD_Info.CopyAppHeaderInfo(ref app_Detail, applicationHeaderInfo);

                #region Tài liệu có trong đơn

                foreach (AppDocumentInfo item in appDocumentInfos)
                {
                    if (item.Document_Id == "02_CGD_01")
                    {
                        app_Detail.Doc_Id_1 = item.CHAR01;
                        app_Detail.Doc_Id_1_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "02_CGD_02")
                    {
                        app_Detail.Doc_Id_2 = item.CHAR01;
                        app_Detail.Doc_Id_2_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "02_CGD_03")
                    {
                        app_Detail.Doc_Id_3 = item.CHAR01;
                        app_Detail.Doc_Id_3_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "02_CGD_04")
                    {
                        app_Detail.Doc_Id_4_Check = item.Isuse;
                        app_Detail.Doc_Id_4 = item.CHAR01;
                    }
                    else if (item.Document_Id == "02_CGD_05")
                    {
                        app_Detail.Doc_Id_5 = item.CHAR01;
                        app_Detail.Doc_Id_5_Check = item.Isuse;
                    }

                    else if (item.Document_Id == "02_CGD_06")
                    {
                        app_Detail.Doc_Id_6_Check = item.Isuse;
                        app_Detail.Doc_Id_6 = item.CHAR01;
                    }
                    else if (item.Document_Id == "02_CGD_07")
                    {
                        app_Detail.Doc_Id_7 = item.CHAR01;
                        app_Detail.Doc_Id_7_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "02_CGD_08")
                    {
                        app_Detail.Doc_Id_8 = item.CHAR01;
                        app_Detail.Doc_Id_8_Check = item.Isuse;
                    }

                    else if (item.Document_Id == "02_CGD_09")
                    {
                        app_Detail.Doc_Id_9 = item.CHAR01;
                        app_Detail.Doc_Id_9_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "02_CGD_010")
                    {
                        app_Detail.Doc_Id_10 = item.CHAR01;
                        app_Detail.Doc_Id_10_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "02_CGD_11")
                    {
                        app_Detail.Doc_Id_11 = item.CHAR01;
                        app_Detail.Doc_Id_11_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "02_CGD_12")
                    {
                        app_Detail.Doc_Id_12 = item.CHAR01;
                        app_Detail.Doc_Id_12_Check = item.Isuse;
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
                        app_Detail.Total_Fee = app_Detail.Total_Fee + item.Amount;
                        app_Detail.Total_Fee_Str = app_Detail.Total_Fee.ToString("#,##0.##");
                    }
                }
                #endregion

                List<App_Detail_PLB02_CGD_Info> _lst = new List<App_Detail_PLB02_CGD_Info>();
                _lst.Add(app_Detail);

                DataSet _ds_all = ConvertData.ConvertToDataSet<App_Detail_PLB02_CGD_Info>(_lst, false);
                //string _strCml = System.Web.HttpContext.Current.Server.MapPath("/Content/XML/" + TradeMarkAppCode.AppCode_TM_3C_PLB_02_CGD + ".xml");
                //_ds_all.WriteXml(_strCml, System.Data.XmlWriteMode.WriteSchema);

                CrystalDecisions.CrystalReports.Engine.ReportDocument oRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                oRpt.Load(Path.Combine(Server.MapPath("~/Report/"), "TM_PLB02_CGD.rpt"));

                if (_ds_all != null)
                {
                    _ds_all.Tables[0].TableName = "Table_3c";
                    oRpt.SetDataSource(_ds_all);
                }
                oRpt.Refresh();

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                System.IO.Stream oStream = oRpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                byte[] byteArray = new byte[oStream.Length];
                oStream.Read(byteArray, 0, Convert.ToInt32(oStream.Length - 1));
                System.IO.File.WriteAllBytes(fileName_pdf, byteArray.ToArray()); // Requires System.Linq

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
        public ActionResult ExportData_IU(ApplicationHeaderInfo pInfo, App_Detail_PLB02_CGD_Info pDetail,
            List<AppDocumentInfo> pAppDocumentInfo, List<AppFeeFixInfo> pFeeFixInfo)
        {
            try
            {
                string language = AppsCommon.GetCurrentLang();
                string _fileTemp = System.Web.HttpContext.Current.Server.MapPath("/Content/AppForms/B02_VI.docx");
                DocumentModel document = DocumentModel.Load(_fileTemp);

                // Fill export_header
                string fileName = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "B02_VI_" + TradeMarkAppCode.AppCode_TM_3C_PLB_02_CGD + ".docx");
                string fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "B02_VI_" + TradeMarkAppCode.AppCode_TM_3C_PLB_02_CGD + ".pdf");

                // copy Header
                App_Detail_PLB02_CGD_Info.CopyAppHeaderInfo(ref pDetail, pInfo);

                #region Tài liệu có trong đơn

                if (pAppDocumentInfo.Count > 0)
                {
                    foreach (AppDocumentInfo item in pAppDocumentInfo)
                    {
                        if (item.Document_Id == "02_CGD_01")
                        {
                            pDetail.Doc_Id_1 = item.CHAR01;
                            pDetail.Doc_Id_1_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "02_CGD_02")
                        {
                            pDetail.Doc_Id_2 = item.CHAR01;
                            pDetail.Doc_Id_2_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "02_CGD_03")
                        {
                            pDetail.Doc_Id_3 = item.CHAR01;
                            pDetail.Doc_Id_3_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "02_CGD_04")
                        {
                            pDetail.Doc_Id_4_Check = item.Isuse;
                            pDetail.Doc_Id_4 = item.CHAR01;
                        }
                        else if (item.Document_Id == "02_CGD_05")
                        {
                            pDetail.Doc_Id_5 = item.CHAR01;
                            pDetail.Doc_Id_5_Check = item.Isuse;
                        }

                        else if (item.Document_Id == "02_CGD_06")
                        {
                            pDetail.Doc_Id_6_Check = item.Isuse;
                            pDetail.Doc_Id_6 = item.CHAR01;
                        }
                        else if (item.Document_Id == "02_CGD_07")
                        {
                            pDetail.Doc_Id_7_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "02_CGD_08")
                        {
                            pDetail.Doc_Id_8_Check = item.Isuse;
                        }

                        else if (item.Document_Id == "02_CGD_09")
                        {
                            pDetail.Doc_Id_9 = item.CHAR01;
                            pDetail.Doc_Id_9_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "02_CGD_010")
                        {
                            pDetail.Doc_Id_10_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "02_CGD_11")
                        {
                            pDetail.Doc_Id_11 = item.CHAR01;
                            pDetail.Doc_Id_11_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "02_CGD_12")
                        {
                            pDetail.Doc_Id_12 = item.CHAR01;
                            pDetail.Doc_Id_12_Check = item.Isuse;
                        }
                    }
                }

                #endregion

                #region Phí cố định

                #region Phí thẩm định yêu cầu sửa đổi đơn
                AppFeeFixInfo _AppFeeFixInfo1 = new AppFeeFixInfo();
                _AppFeeFixInfo1.Isuse = 1;
                _AppFeeFixInfo1.Number_Of_Patent = pDetail.Transfer_Appno.Split(',').Length;

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
                _AppFeeFixInfo2.Number_Of_Patent = pDetail.Transfer_Appno.Split(',').Length;

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

                #endregion

                List<App_Detail_PLB02_CGD_Info> _lst = new List<App_Detail_PLB02_CGD_Info>();
                _lst.Add(pDetail);

                DataSet _ds_all = ConvertData.ConvertToDataSet<App_Detail_PLB02_CGD_Info>(_lst, false);
                CrystalDecisions.CrystalReports.Engine.ReportDocument oRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                oRpt.Load(Path.Combine(Server.MapPath("~/Report/"), "TM_PLB02_CGD.rpt"));

                if (_ds_all != null)
                {
                    _ds_all.Tables[0].TableName = "Table_3c";
                    oRpt.SetDataSource(_ds_all);
                }
                oRpt.Refresh();

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                System.IO.Stream oStream = oRpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                byte[] byteArray = new byte[oStream.Length];
                oStream.Read(byteArray, 0, Convert.ToInt32(oStream.Length - 1));
                System.IO.File.WriteAllBytes(fileName_pdf, byteArray.ToArray()); // Requires System.Linq

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
                ViewBag.FileName = "/Content/Export/" + "B02_VI_" + TradeMarkAppCode.AppCode_TM_3C_PLB_02_CGD + ".pdf";
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
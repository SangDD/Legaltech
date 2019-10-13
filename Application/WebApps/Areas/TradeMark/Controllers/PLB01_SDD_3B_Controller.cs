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
    using CrystalDecisions.CrystalReports.Engine;
    using CrystalDecisions.Shared;
    using System.Linq;
    using BussinessFacade;

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

        [HttpGet]
        [Route("register/{id}/{id1}")]
        public ActionResult Register2()
        {
            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("/");

                SessionData.CurrentUser.chashFile.Clear();
                string AppCode = "";
                if (RouteData.Values.ContainsKey("id"))
                {
                    AppCode = RouteData.Values["id"].ToString().ToUpper();
                }
                ViewBag.AppCode = AppCode;

                string _App_No = "";
                if (RouteData.Values.ContainsKey("id1"))
                {
                    _App_No = RouteData.Values["id1"].ToString().ToUpper();
                }
                ViewBag.App_No = _App_No;

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
                    if (pAppHeaderID < 0)
                        goto Commit_Transaction;

                    // detail
                    if (pAppHeaderID >= 0)
                    {
                        pDetail.Appcode = pInfo.Appcode;
                        pDetail.Language_Code = language;
                        pDetail.App_Header_Id = pAppHeaderID;
                        pDetail.Case_Code = p_case_code;
                        pReturn = objDetail.Insert(pDetail);
                        if (pReturn <= 0)
                            goto Commit_Transaction;
                    }

                    #region Phí cố định
                    List<AppFeeFixInfo> _lstFeeFix = Call_Fee.CallFee_3B(pDetail);
                    if (_lstFeeFix.Count > 0)
                    {
                        AppFeeFixBL _AppFeeFixBL = new AppFeeFixBL();
                        pReturn = _AppFeeFixBL.AppFeeFixInsertBath(_lstFeeFix, p_case_code);
                        if (pReturn < 0)
                            goto Commit_Transaction;
                    }

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
                                    string _url = (string)SessionData.CurrentUser.chashFile[info.keyFileUpload];
                                    string[] _arr = _url.Split('/');
                                    string _filename = WebApps.Resources.Resource.FileDinhKem;
                                    if (_arr.Length > 0)
                                    {
                                        _filename = _arr[_arr.Length - 1];
                                    }

                                    info.Filename = _filename;
                                    info.Url_Hardcopy = _url;
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
                bool _IsOk = false;
                using (var scope = new TransactionScope())
                {
                    //
                    pInfo.Languague_Code = language;
                    pInfo.Modify_By = CreatedBy;
                    pInfo.Modify_Date = CreatedDate;
                    pInfo.Send_Date = DateTime.Now;
                    pInfo.DDSHCN = "";
                    pInfo.MADDSHCN = "";
                    //TRA RA ID CUA BANG KHI INSERT
                    pReturn = objBL.AppHeaderUpdate(pInfo);
                    if (pReturn < 0)
                        goto Commit_Transaction;

                    // detail
                    if (pReturn >= 0)
                    {
                        pDetail.Appcode = pInfo.Appcode;
                        pDetail.Language_Code = language;
                        pDetail.App_Header_Id = pInfo.Id;
                        pDetail.Case_Code = pInfo.Case_Code;
                        pReturn = objDetail.Update(pDetail);
                        if (pReturn <= 0)
                            goto Commit_Transaction;
                    }

                    #region Phí cố định

                    // xóa đi
                    AppFeeFixBL _AppFeeFixBL = new AppFeeFixBL();
                    _AppFeeFixBL.AppFeeFixDelete(pDetail.Case_Code, language);

                    // insert lại fee
                    List<AppFeeFixInfo> _lstFeeFix = Call_Fee.CallFee_3B(pDetail);
                    if (_lstFeeFix.Count > 0)
                    {
                        pReturn = _AppFeeFixBL.AppFeeFixInsertBath(_lstFeeFix, pInfo.Case_Code);
                        if (pReturn < 0)
                            goto Commit_Transaction;
                    }

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
                                    string _url = (string)SessionData.CurrentUser.chashFile[info.keyFileUpload];
                                    string[] _arr = _url.Split('/');
                                    string _filename = WebApps.Resources.Resource.FileDinhKem;
                                    if (_arr.Length > 0)
                                    {
                                        _filename = _arr[_arr.Length - 1];
                                    }

                                    info.Filename = _filename;
                                    info.Url_Hardcopy = _url;
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
                        return Json(new { status = pReturn });
                    }
                    else
                    {
                        scope.Complete();
                        _IsOk = true;
                    }
                }

                // tự động update todo
                if (pInfo.UpdateToDo == 1 && _IsOk == true)
                {
                    if (pInfo.Status == (int)CommonEnums.App_Status.ChoKHConfirm)
                    {
                        Application_Header_BL _obj_bl = new Application_Header_BL();
                        decimal _status = (decimal)CommonEnums.App_Status.KhacHangDaConfirm;

                        string _note = "Xác nhận nộp đơn";
                        if (AppsCommon.GetCurrentLang() != "VI_VN")
                        {
                            _note = "confirmation for filing";
                        }
                        int _ck = _obj_bl.AppHeader_Update_Status(pInfo.Case_Code, _status, _note,
                            SessionData.CurrentUser.Username, DateTime.Now, AppsCommon.GetCurrentLang());
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
        [Route("Translate_PLB_01_SDD")]
        public ActionResult Translate_PLB_01_SDD(ApplicationHeaderInfo pInfo, App_Detail_PLB01_SDD_Info pDetail,
            List<AppDocumentInfo> pAppDocumentInfo, List<AppFeeFixInfo> pFeeFixInfo)
        {
            try
            {
                Application_Header_BL objBL = new Application_Header_BL();
                AppFeeFixBL objFeeFixBL = new AppFeeFixBL();
                App_Detail_PLB01_SDD_BL objDetail = new App_Detail_PLB01_SDD_BL();
                AppDocumentBL objDoc = new AppDocumentBL();
                if (pInfo == null || pDetail == null) return Json(new { status = ErrorCode.Error });
                string language = "";
                if (pInfo.Languague_Code == Language.LangVI)
                {
                    language = Language.LangEN;
                }
                else
                {
                    language = Language.LangVI;
                }

                var CreatedBy = SessionData.CurrentUser.Username;

                var CreatedDate = SessionData.CurrentUser.CurrentDate;
                decimal pReturn = ErrorCode.Success;
                int pAppHeaderID = 0;
                string prefCaseCode = "";
                decimal pIDHeaderRootLang = pInfo.Id;
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
                    //kiểm tra có rồi thì update, chưa có thì insert
                    if (pInfo.Id_Vi > 0)
                    {
                        pInfo.Modify_By = CreatedBy;
                        pInfo.Modify_Date = CreatedDate;
                        pAppHeaderID = objBL.AppHeaderUpdate(pInfo);
                    }
                    else
                    {
                        //TRA RA ID CUA BANG KHI INSERT
                        pInfo.Created_By = CreatedBy;
                        pInfo.Created_Date = CreatedDate;
                        pAppHeaderID = objBL.AppHeaderInsert(pInfo, ref prefCaseCode);
                    }

                    if (pAppHeaderID < 0)
                        goto Commit_Transaction;

                    // detail
                    if (pAppHeaderID >= 0)
                    {
                        pDetail.Appcode = pInfo.Appcode;
                        pDetail.Language_Code = language;
                        pDetail.App_Header_Id = pAppHeaderID;
                        pDetail.Case_Code = prefCaseCode;
                        pReturn = objDetail.Insert(pDetail);
                        if (pReturn <= 0)
                            goto Commit_Transaction;
                    }

                    #region Phí cố định

                    // xóa đi
                    AppFeeFixBL _AppFeeFixBL = new AppFeeFixBL();
                    _AppFeeFixBL.AppFeeFixDelete(prefCaseCode, language);

                    // insert lại fee
                    List<AppFeeFixInfo> _lstFeeFix = Call_Fee.CallFee_3B(pDetail);
                    if (_lstFeeFix.Count > 0)
                    {
                        pReturn = _AppFeeFixBL.AppFeeFixInsertBath(_lstFeeFix, prefCaseCode);
                        if (pReturn < 0)
                            goto Commit_Transaction;
                    }
                    #endregion

                    #region Tai lieu dinh kem 
                    if (pReturn >= 0 && pAppDocumentInfo != null)
                    {
                        if (pAppDocumentInfo.Count > 0)
                        {
                            pReturn = objDoc.AppDocumentTranslate(language, pIDHeaderRootLang, pAppHeaderID);
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
                return Json(new { status = pAppHeaderID });
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
                    SessionData.CurrentUser.chashFile[pInfo.keyFileUpload] = url;
                    //SessionData.CurrentUser.chashFile[pInfo.keyFileUpload] = pInfo.pfiles;
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
        public ActionResult ExportData_View(decimal pAppHeaderId, string p_appCode, decimal p_View_Translate)
        {
            try
            {
                string language = AppsCommon.GetCurrentLang();

                App_Detail_PLB01_SDD_BL objBL = new App_Detail_PLB01_SDD_BL();
                ApplicationHeaderInfo applicationHeaderInfo = new ApplicationHeaderInfo();
                App_Detail_PLB01_SDD_Info app_Detail = new App_Detail_PLB01_SDD_Info();
                List<AppFeeFixInfo> appFeeFixInfos = new List<AppFeeFixInfo>();
                List<AppDocumentInfo> appDocumentInfos = new List<AppDocumentInfo>();
                app_Detail = objBL.GetByID(pAppHeaderId, language, ref applicationHeaderInfo, ref appDocumentInfos, ref appFeeFixInfos);

                AppsCommon.Prepare_Data_Export_3B(ref app_Detail, applicationHeaderInfo, appDocumentInfos);
                List<App_Detail_PLB01_SDD_Info> _lst = new List<App_Detail_PLB01_SDD_Info>();
                _lst.Add(app_Detail);
                DataSet _ds_all = ConvertData.ConvertToDataSet<App_Detail_PLB01_SDD_Info>(_lst, false);
                //_ds_all.WriteXml(@"C:\inetpub\B01.xml", XmlWriteMode.WriteSchema);

                CrystalDecisions.CrystalReports.Engine.ReportDocument oRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                string _datetimenow = DateTime.Now.ToString("ddMMyyyyHHmm");
                string _tempfile = "TM_PLB01SDD.rpt";
                string fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "B01_VN_" + _datetimenow + ".pdf");
                if (p_View_Translate == 1)
                {
                    // nếu là tiếng việt thì xem bản tiếng anh và ngược lại
                    if (applicationHeaderInfo.Languague_Code == Language.LangVI)
                    {
                        _tempfile = "TM_PLB01SDD_EN.rpt"; // tiếng anh
                        fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "B01_EN_" + _datetimenow + ".pdf");
                        SessionData.CurrentUser.FilePreview = "/Content/Export/" + "B01_EN_" + _datetimenow + ".pdf";
                    }
                    else
                    {
                        fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "B01_VN_" + _datetimenow + ".pdf");
                        SessionData.CurrentUser.FilePreview = "/Content/Export/" + "B01_VN_" + _datetimenow + ".pdf";
                    }
                }
                else
                {
                    if (applicationHeaderInfo.Languague_Code == Language.LangVI)
                    {
                        fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "B01_VN_" + _datetimenow + ".pdf");
                        SessionData.CurrentUser.FilePreview = "/Content/Export/" + "B01_VN_" + _datetimenow + ".pdf";
                    }
                    else
                    {
                        _tempfile = "TM_PLB01SDD_EN.rpt"; // tiếng anh
                        fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "B01_EN_" + _datetimenow + ".pdf");
                        SessionData.CurrentUser.FilePreview = "/Content/Export/" + "B01_EN_" + _datetimenow + ".pdf";
                    }
                }

                oRpt.Load(Path.Combine(Server.MapPath("~/Report/"), _tempfile));

                if (_ds_all != null)
                {
                    _ds_all.Tables[0].TableName = "Table";
                    // đè các bản dịch lên
                    if (p_View_Translate == 1)
                    {
                        // nếu là bản xem của thằng dịch
                        App_Translate_BL _App_Translate_BL = new App_Translate_BL();
                        List<App_Translate_Info> _lst_translate = _App_Translate_BL.App_Translate_GetBy_AppId(pAppHeaderId);

                        AppsCommon.Overwrite_DataSouce_Export(ref _ds_all, _lst_translate);
                    }

                    oRpt.Database.Tables["Table1"].SetDataSource(_ds_all.Tables[0]);
                    //oRpt.SetDataSource(_ds_all);
                }
                oRpt.Refresh();

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();


                System.IO.Stream oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat);
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
        public ActionResult ExportData_IU(ApplicationHeaderInfo pInfo, App_Detail_PLB01_SDD_Info pDetail,
            List<AppDocumentInfo> pAppDocumentInfo, List<AppFeeFixInfo> pFeeFixInfo)
        {
            try
            {
                string language = AppsCommon.GetCurrentLang();
                //string language = pInfo.View_Language_Report;

                string _fileTemp = System.Web.HttpContext.Current.Server.MapPath("/Content/AppForms/B01_VI.docx");
                DocumentModel document = DocumentModel.Load(_fileTemp);

                // Fill export_header
                AppsCommon.Prepare_Data_Export_3B(ref pDetail, pInfo, pAppDocumentInfo);
                List<App_Detail_PLB01_SDD_Info> _lst = new List<App_Detail_PLB01_SDD_Info>();
                _lst.Add(pDetail);
                DataSet _ds_all = ConvertData.ConvertToDataSet<App_Detail_PLB01_SDD_Info>(_lst, false);
                //_ds_all.Tables[0].TableName = "Table";
                //_ds_all.WriteXml(@"C:\inetpub\B01.xml", XmlWriteMode.WriteSchema);

                CrystalDecisions.CrystalReports.Engine.ReportDocument oRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                string _tempfile = "TM_PLB01SDD.rpt";
                string _datetimenow = DateTime.Now.ToString("ddMMyyyyHHmm");
                string fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "B01_VN_" + _datetimenow + ".pdf");
                if (language == Language.LangVI)
                {
                    fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "B01_VN_" + _datetimenow + ".pdf");
                    SessionData.CurrentUser.FilePreview = "/Content/Export/" + "B01_VN_" + _datetimenow + ".pdf";
                }
                else
                {
                    _tempfile = "TM_PLB01SDD_EN.rpt"; // tiếng anh
                    fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "B01_EN_" + _datetimenow + ".pdf");
                    SessionData.CurrentUser.FilePreview = "/Content/Export/" + "B01_EN_" + _datetimenow + ".pdf";
                }

                oRpt.Load(Path.Combine(Server.MapPath("~/Report/"), _tempfile));

                if (_ds_all != null)
                {
                    _ds_all.Tables[0].TableName = "Table";

                    oRpt.Database.Tables["Table"].SetDataSource(_ds_all.Tables[0]);
                    //oRpt.SetDataSource(_ds_all);
                }
                oRpt.Refresh();

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                System.IO.Stream oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat);
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
                ViewBag.FileName = SessionData.CurrentUser.FilePreview;
                //ViewBag.FileName = "/Content/Export/" + "B01_VI_" + TradeMarkAppCode.AppCode_TM_3B_PLB_01_SDD + ".pdf";
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

                var PartialThongTinChuDon = AppsCommon.RenderRazorViewToString(this.ControllerContext, "~/Areas/TradeMark/Views/Shared/_PartialThongTinChuDon.cshtml", "1");
                var PartialThongTinDaiDienChuDon = AppsCommon.RenderRazorViewToString(this.ControllerContext, "~/Areas/TradeMark/Views/Shared/_PartialThongTinDaiDienChuDon.cshtml", "2");

                var json = Json(new { PartialThongTinChuDon, PartialThongTinDaiDienChuDon });
                return json;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/TradeMark/Views/Shared/_PartialThongTinChuDon.cshtml");
            }
        }

        /// <summary>
        /// Truyền các tham số Formular vào báo cáo
        /// </summary>
        void Puth_Data2_Fomulator(ref ReportDocument oRpt, List<AppFeeFixInfo> appFeeFixInfos, List<AppDocumentInfo> appDocumentInfos)
        {
            try
            {
                Dictionary<string, AppDocumentInfo> dic_appDoc = new Dictionary<string, AppDocumentInfo>();
                foreach (AppDocumentInfo item in appDocumentInfos)
                {
                    dic_appDoc[item.Document_Id] = item;
                }

                #region Truyền các tham số Formular vào báo cáo
                // 
                FormulaFieldDefinitions crFFieldDefinitions = oRpt.DataDefinition.FormulaFields;
                FormulaFieldDefinition crFFieldDefinition;
                string strFormulaName;

                for (int i = 0; i < crFFieldDefinitions.Count; i++)
                {
                    crFFieldDefinition = crFFieldDefinitions[i];
                    strFormulaName = crFFieldDefinition.Name.ToUpper();

                    #region Fee
                    if (strFormulaName == "F_DOC_1_ISUSE")
                    {
                        crFFieldDefinition.Text = "'" + dic_appDoc["01_SDD_01"].Isuse.ToString() + "'"; ;
                    }

                    #endregion

                    #region TL trong đơn

                    #endregion
                }
                #endregion
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        [HttpPost]
        [Route("getFee")]
        public ActionResult GetFee(ApplicationHeaderInfo pInfo, App_Detail_PLB01_SDD_Info pDetail, List<AppDocumentInfo> pAppDocumentInfo, List<UTienInfo> pUTienInfo, List<AppDocumentOthersInfo> pLstImagePublic)
        {
            try
            {
                //AppsCommon.Prepare_Data_Export_3B(ref pDetail, pInfo, pAppDocumentInfo);
                List<AppFeeFixInfo> _lstFeeFix = Call_Fee.CallFee_3B(pDetail);
                ViewBag.LstFeeFix = _lstFeeFix;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            var PartialTableListFees = AppsCommon.RenderRazorViewToString(this.ControllerContext, "~/Areas/Patent/Views/Shared/_PartialTableListFees.cshtml");
            var json = Json(new { success = 1, PartialTableListFees });
            return json;

            //return PartialView("~/Areas/Patent/Views/A01/_PartialTableListFees.cshtml");
        }
    }
}
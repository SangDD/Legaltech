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
    using BussinessFacade;

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

                string _App_No = "";
                if (RouteData.Values.ContainsKey("id1"))
                {
                    _App_No = RouteData.Values["id1"].ToString().ToUpper();
                }

                ViewBag.AppCode = AppCode;
                ViewBag.App_No = _App_No;
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
                bool _IsOk = false;
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
        [Route("Translate_PLB_02_CGD")]
        public ActionResult Translate_PLB_02_CGD(ApplicationHeaderInfo pInfo, App_Detail_PLB02_CGD_Info pDetail,
         List<AppDocumentInfo> pAppDocumentInfo, List<AppFeeFixInfo> pFeeFixInfo)
        {
            try
            {
                Application_Header_BL objBL = new Application_Header_BL();
                AppFeeFixBL objFeeFixBL = new AppFeeFixBL();
                App_Detail_Plb02_CGD_BL objDetail_BL = new App_Detail_Plb02_CGD_BL();
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
                pInfo.Languague_Code = language;
                decimal pIDHeaderoot = pInfo.Id;
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


                    // detail
                    if (pAppHeaderID >= 0)
                    {
                        pDetail.Appcode = pInfo.Appcode;
                        pDetail.Language_Code = language;
                        pDetail.App_Header_Id = pAppHeaderID;
                        pDetail.Case_Code = prefCaseCode;
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
                    pReturn = _AppFeeFixBL.AppFeeFixInsertBath(_lstFeeFix, prefCaseCode);
                    #endregion

                    #region Tai lieu dinh kem 
                    if (pReturn >= 0 && pAppDocumentInfo != null)
                    {
                        if (pAppDocumentInfo.Count > 0)
                        {
                            pReturn = objDoc.AppDocumentTranslate(language, pIDHeaderoot, pAppHeaderID);
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
        [Route("ket_xuat_file")]
        public ActionResult ExportData_View(decimal pAppHeaderId, string p_appCode, decimal p_View_Translate)
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
                AppsCommon.Prepare_Data_Export_B02(ref app_Detail, applicationHeaderInfo, appDocumentInfos);

                List<App_Detail_PLB02_CGD_Info> _lst = new List<App_Detail_PLB02_CGD_Info>();
                _lst.Add(app_Detail);

                DataSet _ds_all = ConvertData.ConvertToDataSet<App_Detail_PLB02_CGD_Info>(_lst, false);
                CrystalDecisions.CrystalReports.Engine.ReportDocument oRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                string _tempfile = "TM_PLB02_CGD.rpt";
                string _datetimenow = DateTime.Now.ToString("ddMMyyyyHHmm");
                string fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "B02_VN_" + _datetimenow + ".pdf");
                if (p_View_Translate == 1)
                {
                    // nếu là tiếng việt thì xem bản tiếng anh và ngược lại
                    if (applicationHeaderInfo.Languague_Code == Language.LangVI)
                    {
                        _tempfile = "TM_PLB02_CGD_EN.rpt"; // tiếng anh
                        fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "B02_EN_" + _datetimenow + ".pdf");
                        SessionData.CurrentUser.FilePreview = "/Content/Export/" + "B02_EN_" + _datetimenow + ".pdf";
                    }
                    else
                    {
                        fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "B02_VN_" + _datetimenow + ".pdf");
                        SessionData.CurrentUser.FilePreview = "/Content/Export/" + "B02_VN_" + _datetimenow + ".pdf";
                    }
                }
                else
                {
                    if (applicationHeaderInfo.Languague_Code == Language.LangVI)
                    {
                        fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "B02_VN_" + _datetimenow + ".pdf");
                        SessionData.CurrentUser.FilePreview = "/Content/Export/" + "B02_VN_" + _datetimenow + ".pdf";
                    }
                    else
                    {
                        _tempfile = "TM_PLB02_CGD_EN.rpt"; // tiếng anh
                        fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "B02_EN_" + _datetimenow + ".pdf");
                        SessionData.CurrentUser.FilePreview = "/Content/Export/" + "B02_EN_" + _datetimenow + ".pdf";
                    }
                }

                oRpt.Load(Path.Combine(Server.MapPath("~/Report/"), _tempfile));

                if (_ds_all != null)
                {
                    _ds_all.Tables[0].TableName = "Table_3c";

                    // đè các bản dịch lên
                    if (p_View_Translate == 1)
                    {
                        // nếu là bản xem của thằng dịch
                        App_Translate_BL _App_Translate_BL = new App_Translate_BL();
                        List<App_Translate_Info> _lst_translate = _App_Translate_BL.App_Translate_GetBy_AppId(pAppHeaderId);

                        AppsCommon.Overwrite_DataSouce_Export(ref _ds_all, _lst_translate);
                    }


                    oRpt.Database.Tables["Table_3c"].SetDataSource(_ds_all.Tables[0]);
                    //oRpt.SetDataSource(_ds_all);
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

                // Fill export_header
                string fileName = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "B02_VI_" + TradeMarkAppCode.AppCode_TM_3C_PLB_02_CGD + ".docx");
                string fileName_pdf = "";

                AppsCommon.Prepare_Data_Export_B02(ref pDetail, pInfo, pAppDocumentInfo);

                List<App_Detail_PLB02_CGD_Info> _lst = new List<App_Detail_PLB02_CGD_Info>();
                _lst.Add(pDetail);

                DataSet _ds_all = ConvertData.ConvertToDataSet<App_Detail_PLB02_CGD_Info>(_lst, false);
                CrystalDecisions.CrystalReports.Engine.ReportDocument oRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                string _tempfile = "TM_PLB02_CGD.rpt";
                string _datetimenow = DateTime.Now.ToString("ddMMyyyyHHmm");
                if (language == Language.LangVI)
                {
                    fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "B02_VN_" + _datetimenow + ".pdf");
                    SessionData.CurrentUser.FilePreview = "/Content/Export/" + "B02_VN_" + _datetimenow + ".pdf";
                }
                else
                {
                    _tempfile = "TM_PLB02_CGD_EN.rpt"; // tiếng anh
                    fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "B02_EN_" + _datetimenow + ".pdf");
                    SessionData.CurrentUser.FilePreview = "/Content/Export/" + "B02_EN_" + _datetimenow + ".pdf";
                }

                oRpt.Load(Path.Combine(Server.MapPath("~/Report/"), _tempfile));
                if (_ds_all != null)
                {
                    _ds_all.Tables[0].TableName = "Table_3c";

                    //_ds_all.WriteXml(@"E:\Working\Legaltech\Application\WebApps\Content\XML\TM_PLB02CGD.xml", XmlWriteMode.WriteSchema);

                    oRpt.Database.Tables["Table_3c"].SetDataSource(_ds_all.Tables[0]);

                    //oRpt.SetDataSource(_ds_all);
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
                ViewBag.FileName = SessionData.CurrentUser.FilePreview;
                return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistration/_PartialContentPreview.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistration/_PartialContentPreview.cshtml");
            }
        }

        [HttpPost]
        [Route("getFee")]
        public ActionResult GetFee(ApplicationHeaderInfo pInfo, App_Detail_PLB02_CGD_Info pDetail)
        {
            try
            {
                List<AppFeeFixInfo> _lstFeeFix = Call_Fee.CallFee_B02(pDetail);
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
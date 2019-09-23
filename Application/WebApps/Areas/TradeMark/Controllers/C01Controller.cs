using BussinessFacade;
using BussinessFacade.ModuleTrademark;
using Common;
using Common.CommonData;
using CrystalDecisions.Shared;
using ObjectInfos;
using ObjectInfos.ModuleTrademark;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using WebApps.AppStart;
using WebApps.CommonFunction;
using WebApps.Session;

namespace WebApps.Areas.TradeMark.Controllers
{
    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("PatentRegistration", AreaPrefix = "trade-mark-c01")]
    [Route("{action}")]
    public class C01Controller : Controller
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
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return PartialView("~/Areas/TradeMark/Views/C01/_Partial_C01_Register.cshtml");
        }

        [HttpPost]
        [Route("do-register")]
        public ActionResult Register_C01(ApplicationHeaderInfo pInfo, App_Detail_C01_Info pDetail,
            List<AppDocumentInfo> pAppDocumentInfo, List<AppFeeFixInfo> pFeeFixInfo, 
            List<AppDocumentOthersInfo> pAppDocOtherInfo, List<AppDocumentOthersInfo> pLstImagePublic)
        {
            try
            {
                Application_Header_BL objBL = new Application_Header_BL();
                AppFeeFixBL objFeeFixBL = new AppFeeFixBL();
                App_Detail_C01_BL objDetail = new App_Detail_C01_BL();
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

                    //tai lieu khac 
                    if (pReturn >= 0 && pAppDocOtherInfo != null)
                    {
                        if (pAppDocOtherInfo.Count > 0)
                        {
                            int check = 0;
                            foreach (var info in pAppDocOtherInfo)
                            {
                                if (SessionData.CurrentUser.chashFile.ContainsKey(info.keyFileUpload))
                                {
                                    string _url = (string)SessionData.CurrentUser.chashFile[info.keyFileUpload];
                                    info.Filename = _url;
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

                    #region hình công bố
                    if (pReturn >= 0 && pLstImagePublic != null)
                    {
                        if (pLstImagePublic.Count > 0)
                        {
                            int check = 0;
                            foreach (var info in pLstImagePublic)
                            {
                                if (SessionData.CurrentUser.chashFile.ContainsKey(info.keyFileUpload))
                                {
                                    string _url = (string)SessionData.CurrentUser.chashFile[info.keyFileUpload];
                                    info.Filename = _url;
                                    check = 1;

                                }
                                info.App_Header_Id = pAppHeaderID;
                                info.Language_Code = language;
                            }
                            if (check == 1)
                            {
                                AppImageBL _AppImageBL = new AppImageBL();
                                pReturn = _AppImageBL.AppImageInsertBatch(pLstImagePublic);
                            }
                        }
                    } 
                    #endregion

                    #region Phí cố định
                    List<AppFeeFixInfo> _lstFeeFix = Call_Fee.CallFee_C01(pDetail, pAppDocumentInfo, pLstImagePublic);
                    if (_lstFeeFix.Count > 0)
                    {
                        AppFeeFixBL _AppFeeFixBL = new AppFeeFixBL();
                        pReturn = _AppFeeFixBL.AppFeeFixInsertBath(_lstFeeFix, p_case_code);
                        if (pReturn < 0)
                            goto Commit_Transaction;
                    }
                    #endregion

                    //tai lieu khac 
                    if (pReturn >= 0 && pAppDocOtherInfo != null)
                    {
                        if (pAppDocOtherInfo.Count > 0)
                        {
                            int check = 0;
                            foreach (var info in pAppDocOtherInfo)
                            {
                                if (SessionData.CurrentUser.chashFile.ContainsKey(info.keyFileUpload))
                                {
                                    string _url = (string)SessionData.CurrentUser.chashFile[info.keyFileUpload];
                                    info.Filename = _url;
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
                return Json(new { status = pReturn });

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { status = ErrorCode.Error });
            }
        }

        [HttpPost]
        [Route("do-edit")]
        public ActionResult Edit_C01(ApplicationHeaderInfo pInfo, App_Detail_C01_Info pDetail,
            List<AppDocumentInfo> pAppDocumentInfo, List<AppFeeFixInfo> pFeeFixInfo, 
            List<AppDocumentOthersInfo> pAppDocOtherInfo, List<AppDocumentOthersInfo> pLstImagePublic)
        {
            try
            {
                Application_Header_BL objBL = new Application_Header_BL();
                AppFeeFixBL objFeeFixBL = new AppFeeFixBL();
                App_Detail_C01_BL objDetail = new App_Detail_C01_BL();
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

                    #region hình công bố
                    if (pReturn >= 0 && pLstImagePublic != null)
                    {
                        if (pLstImagePublic.Count > 0)
                        {
                            AppImageBL _AppImageBL = new AppImageBL();

                            List<AppDocumentOthersInfo> Lst_ImagePublic_Old = _AppImageBL.GetByAppHeader(pInfo.Id, language);
                            Dictionary<decimal, AppDocumentOthersInfo> _dic_image = new Dictionary<decimal, AppDocumentOthersInfo>();
                            foreach (AppDocumentOthersInfo item in Lst_ImagePublic_Old)
                            {
                                _dic_image[item.Id] = item;
                            }

                            // xóa đi trước insert lại sau
                            _AppImageBL.AppImageDeletedByApp(pInfo.Id, language);

                            int check = 0;
                            foreach (AppDocumentOthersInfo info in pLstImagePublic)
                            {
                                if (SessionData.CurrentUser.chashFile.ContainsKey(info.keyFileUpload))
                                {
                                    string _url = (string)SessionData.CurrentUser.chashFile[info.keyFileUpload];
                                    info.Filename = _url;
                                    check = 1;
                                }
                                else if (_dic_image.ContainsKey(info.Id))
                                {
                                    info.Filename = _dic_image[info.Id].Filename;
                                    check = 1;
                                }

                                info.App_Header_Id = pInfo.Id;
                                info.Language_Code = language;
                            }

                            if (check == 1)
                            {
                                pReturn = _AppImageBL.AppImageInsertBatch(pLstImagePublic);
                            }
                        }
                    } 
                    #endregion

                    #region Phí cố định

                    // xóa đi
                    AppFeeFixBL _AppFeeFixBL = new AppFeeFixBL();
                    _AppFeeFixBL.AppFeeFixDelete(pDetail.Case_Code, language);

                    // insert lại fee
                    List<AppFeeFixInfo> _lstFeeFix = Call_Fee.CallFee_C01(pDetail, pAppDocumentInfo, pLstImagePublic);
                    if (_lstFeeFix.Count > 0)
                    {
                        pReturn = _AppFeeFixBL.AppFeeFixInsertBath(_lstFeeFix, pInfo.Case_Code);
                        if (pReturn < 0)
                            goto Commit_Transaction;
                    }

                    #endregion

                    //tai lieu khac 
                    if (pReturn >= 0 && pAppDocOtherInfo != null)
                    {
                        List<AppDocumentOthersInfo> Lst_Doc_Others_Old = objDoc.DocumentOthers_GetByAppHeader(pInfo.Id, language);
                        Dictionary<decimal, AppDocumentOthersInfo> _dic_doc_others = new Dictionary<decimal, AppDocumentOthersInfo>();
                        foreach (AppDocumentOthersInfo item in Lst_Doc_Others_Old)
                        {
                            _dic_doc_others[item.Id] = item;
                        }

                        // xóa đi trước insert lại sau
                        objDoc.AppDocumentOtherDeletedByApp(pInfo.Id, language);

                        if (pAppDocOtherInfo.Count > 0)
                        {
                            int check = 0;
                            foreach (var info in pAppDocOtherInfo)
                            {
                                if (SessionData.CurrentUser.chashFile.ContainsKey(info.keyFileUpload))
                                {
                                    string _url = (string)SessionData.CurrentUser.chashFile[info.keyFileUpload];
                                    info.Filename = _url;
                                    check = 1;
                                }
                                else if (_dic_doc_others.ContainsKey(info.Id))
                                {
                                    info.Filename = _dic_doc_others[info.Id].Filename;
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
                            pReturn = _AppDocumentBL.AppDocumentInsertBath(pAppDocumentInfo, pInfo.Id);

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
        [Route("getFeeView")]
        public ActionResult GetFee_View(ApplicationHeaderInfo pInfo)
        {
            try
            {
                AppFeeFixBL _AppFeeFixBL = new AppFeeFixBL();
                List<AppFeeFixInfo> _lstFeeFix = _AppFeeFixBL.GetByCaseCode(pInfo.Case_Code);
                ViewBag.LstFeeFix = _lstFeeFix;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            var PartialTableListFees = AppsCommon.RenderRazorViewToString(this.ControllerContext, "~/Areas/Patent/Views/Shared/_PartialTableListFees.cshtml");
            var json = Json(new { success = 1, PartialTableListFees });
            return json;
        }

        [HttpPost]
        [Route("getFee")]
        public ActionResult GetFee(App_Detail_C01_Info pDetail, List<AppDocumentInfo> pAppDocumentInfo, List<AppDocumentOthersInfo> pLstImagePublic)
        {
            try
            {
                List<AppFeeFixInfo> _lstFeeFix = Call_Fee.CallFee_C01(pDetail, pAppDocumentInfo, pLstImagePublic);
                ViewBag.LstFeeFix = _lstFeeFix;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            var PartialTableListFees = AppsCommon.RenderRazorViewToString(this.ControllerContext, "~/Areas/Patent/Views/Shared/_PartialTableListFees.cshtml");
            var json = Json(new { success = 1, PartialTableListFees });
            return json;
        }


        [HttpPost]
        [Route("ket_xuat_file")]
        public ActionResult ExportData_View(decimal pAppHeaderId, string p_appCode, decimal p_View_Translate)
        {
            try
            {
                string language = AppsCommon.GetCurrentLang();

                App_Detail_C01_BL objBL = new App_Detail_C01_BL();
                ApplicationHeaderInfo applicationHeaderInfo = new ApplicationHeaderInfo();
                App_Detail_C01_Info app_Detail = new App_Detail_C01_Info();
                List<AppFeeFixInfo> appFeeFixInfos = new List<AppFeeFixInfo>();
                List<AppDocumentInfo> appDocumentInfos = new List<AppDocumentInfo>();
                List<AppDocumentOthersInfo> _LstDocumentOthersInfo = new List<AppDocumentOthersInfo>();
                List<AppDocumentOthersInfo> pLstImagePublic = new List<AppDocumentOthersInfo>();

                app_Detail = objBL.GetByID(pAppHeaderId, language, ref applicationHeaderInfo, ref appDocumentInfos, ref appFeeFixInfos, ref _LstDocumentOthersInfo, ref pLstImagePublic);

                AppsCommon.Prepare_Data_Export_C01(ref app_Detail, applicationHeaderInfo, appDocumentInfos, pLstImagePublic);
                List<App_Detail_C01_Info> _lst = new List<App_Detail_C01_Info>();
                _lst.Add(app_Detail);
                DataSet _ds_all = ConvertData.ConvertToDataSet<App_Detail_C01_Info>(_lst, false);
                CrystalDecisions.CrystalReports.Engine.ReportDocument oRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                string _datetimenow = DateTime.Now.ToString("ddMMyyyyHHmm");
                string _tempfile = "C01.rpt";
                string fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "C01_VN_" + _datetimenow + ".pdf");
                if (p_View_Translate == 1)
                {
                    // nếu là tiếng việt thì xem bản tiếng anh và ngược lại
                    if (applicationHeaderInfo.Languague_Code == Language.LangVI)
                    {
                        _tempfile = "C01_EN.rpt"; // tiếng anh
                        fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "C01_EN_" + _datetimenow + ".pdf");
                        SessionData.CurrentUser.FilePreview = "/Content/Export/" + "C01_EN_" + _datetimenow + ".pdf";
                    }
                    else
                    {
                        fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "C01_VN_" + _datetimenow + ".pdf");
                        SessionData.CurrentUser.FilePreview = "/Content/Export/" + "C01_VN_" + _datetimenow + ".pdf";
                    }
                }
                else
                {
                    if (applicationHeaderInfo.Languague_Code == Language.LangVI)
                    {
                        fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "C01_VN_" + _datetimenow + ".pdf");
                        SessionData.CurrentUser.FilePreview = "/Content/Export/" + "C01_VN_" + _datetimenow + ".pdf";
                    }
                    else
                    {
                        _tempfile = "C01_EN.rpt"; // tiếng anh
                        fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "C01_EN_" + _datetimenow + ".pdf");
                        SessionData.CurrentUser.FilePreview = "/Content/Export/" + "C01_EN_" + _datetimenow + ".pdf";
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

                    oRpt.SetDataSource(_ds_all);
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
        [Route("ket_xuat_file_IU")]
        public ActionResult ExportData_View_IU(ApplicationHeaderInfo pInfo, App_Detail_C01_Info pDetail,
            List<AppDocumentInfo> pAppDocumentInfo, List<AppFeeFixInfo> pFeeFixInfo, List<AppDocumentOthersInfo> pAppDocOtherInfo, List<AppDocumentOthersInfo> pLstImagePublic)
        {
            try
            {
                string _datetimenow = DateTime.Now.ToString("ddMMyyyyHHmm");
                string language = AppsCommon.GetCurrentLang();
                List<App_Detail_C01_Info> _lst = new List<App_Detail_C01_Info>();

                string fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "C01_VN_" + _datetimenow + ".pdf");
                if (language == Language.LangVI)
                {
                    fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "C01_VN_" + _datetimenow + ".pdf");
                    SessionData.CurrentUser.FilePreview = "/Content/Export/" + "C01_VN_" + _datetimenow + ".pdf";
                }
                else
                {
                    fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "C01_EN_" + _datetimenow + ".pdf");
                    SessionData.CurrentUser.FilePreview = "/Content/Export/" + "C01_EN_" + _datetimenow + ".pdf"; 
                }

                AppsCommon.Prepare_Data_Export_C01(ref pDetail, pInfo, pAppDocumentInfo, pLstImagePublic);
                _lst.Add(pDetail);

                DataSet _ds_all = ConvertData.ConvertToDataSet<App_Detail_C01_Info>(_lst, false);
                //_ds_all.WriteXml(@"C:\inetpub\A01.xml", XmlWriteMode.WriteSchema);
                CrystalDecisions.CrystalReports.Engine.ReportDocument oRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                string _tempfile = "C01.rpt";
                if (language == Language.LangEN)
                {
                    _tempfile = "C01_EN.rpt";
                }
                oRpt.Load(Path.Combine(Server.MapPath("~/Report/"), _tempfile));

                if (_ds_all != null)
                {
                    _ds_all.Tables[0].TableName = "Table";
                    oRpt.SetDataSource(_ds_all);
                }
                oRpt.Refresh();

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                //oRpt.ExportToDisk(ExportFormatType.PortableDocFormat, fileName_pdf);

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
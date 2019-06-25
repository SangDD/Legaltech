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

namespace WebApps.Areas.Patent.Controllers
{

    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("PatentRegistration", AreaPrefix = "lg-patent")]
    [Route("{action}")]
    public class A01Controller : Controller
    {
        [HttpGet]
        [Route("register/{id}")]
        public ActionResult Index()
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
                return PartialView("~/Areas/Patent/Views/A01/_Partial_A01.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/Patent/Views/A01/_Partial_A01.cshtml");
            }
        }

        [HttpPost]
        [Route("tac-gia/them-tac-gia")]
        public ActionResult ThemTacGia(decimal p_id)
        {
            try
            {
                ViewBag.Id = p_id;
                return PartialView("~/Areas/TradeMark/Views/Shared/_PartialThongTinTacGia_Khac.cshtml", p_id.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/TradeMark/Views/Shared/_PartialThongTinTacGia_Khac.cshtml", p_id.ToString());
            }
        }

        [HttpPost]
        [Route("don-uu-tien/them-don")]
        public ActionResult ThemDonUuTien(string p_id)
        {
            try
            {
                return PartialView("~/Areas/TradeMark/Views/Shared/_PartialThongTinQuyenUuTien.cshtml", p_id.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/TradeMark/Views/Shared/_PartialThongTinQuyenUuTien.cshtml", "2");
            }
        }

        [HttpPost]
        [Route("chu-don/them-chu-don")]
        public ActionResult ThemChuDon(string p_id)
        {
            try
            {
                return PartialView("~/Areas/TradeMark/Views/Shared/_PartialThongTinChuDonKhac.cshtml", p_id.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/TradeMark/Views/Shared/_PartialThongTinChuDonKhac.cshtml", p_id.ToString());
            }
        }

        [HttpPost]
        [Route("register")]
        public ActionResult Register(ApplicationHeaderInfo pInfo, A01_Info pDetail,
            List<AppDocumentInfo> pAppDocumentInfo, List<AppFeeFixInfo> pFeeFixInfo,
            List<AuthorsInfo> pAppAuthorsInfo, List<Other_MasterInfo> pOther_MasterInfo,
            List<AppClassDetailInfo> pAppClassInfo, List<AppDocumentOthersInfo> pAppDocOtherInfo,
            List<UTienInfo> pUTienInfo, List<AppDocumentOthersInfo> pLstImagePublic)
        {
            try
            {
                Application_Header_BL objBL = new Application_Header_BL();
                AppFeeFixBL objFeeFixBL = new AppFeeFixBL();
                A01_BL objDetail = new A01_BL();
                AppDocumentBL objDoc = new AppDocumentBL();
                Other_Master_BL _Other_Master_BL = new Other_Master_BL();
                Author_BL _Author_BL = new Author_BL();

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

                        if (pDetail.Source_PCT == "Y")
                        {
                            pDetail.DQSC_Filling_Date = DateTime.MinValue;
                            pDetail.GPHI_Filling_Date = DateTime.MinValue;
                        }
                        else if (pDetail.Source_DQSC == "Y")
                        {
                            pDetail.GPHI_Filling_Date = DateTime.MinValue;
                            pDetail.PCT_Date = DateTime.MinValue;
                            pDetail.PCT_Filling_Date_Qt = DateTime.MinValue;
                            pDetail.PCT_VN_Date = DateTime.MinValue;
                        }
                        else if (pDetail.Source_GPHI == "Y")
                        {
                            pDetail.DQSC_Filling_Date = DateTime.MinValue;
                            pDetail.PCT_Date = DateTime.MinValue;
                            pDetail.PCT_Filling_Date_Qt = DateTime.MinValue;
                            pDetail.PCT_VN_Date = DateTime.MinValue;
                        } 

                        pReturn = objDetail.Insert(pDetail);
                        if (pReturn <= 0)
                            goto Commit_Transaction;
                    }

                    if (pAppAuthorsInfo != null && pAppAuthorsInfo.Count > 0)
                    {
                        foreach (var item in pAppAuthorsInfo)
                        {
                            item.Case_Code = p_case_code;
                        }
                        decimal _re = _Author_BL.Insert(pAppAuthorsInfo);
                        if (_re <= 0)
                            goto Commit_Transaction;

                        ////Thêm thông tin class
                        //if (pAppClassInfo != null)
                        //{
                        //    AppClassDetailBL objClassDetail = new AppClassDetailBL();
                        //    pReturn = objClassDetail.AppClassDetailInsertBatch(pAppClassInfo, pAppHeaderID, language);
                        //}
                    }

                    if (pOther_MasterInfo != null && pOther_MasterInfo.Count > 0)
                    {
                        foreach (var item in pOther_MasterInfo)
                        {
                            item.Case_Code = p_case_code;
                        }

                        decimal _re = _Other_Master_BL.Insert(pOther_MasterInfo);
                        if (_re <= 0)
                            goto Commit_Transaction;
                    }

                    if (pUTienInfo != null && pUTienInfo.Count > 0)
                    {
                        foreach (var item in pUTienInfo)
                        {
                            item.Case_Code = p_case_code;
                        }

                        Uu_Tien_BL _Uu_Tien_BL = new Uu_Tien_BL();
                        decimal _re = _Uu_Tien_BL.Insert(pUTienInfo);
                        if (_re <= 0)
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

                    // hình công bố
                    if (pReturn >= 0 && pLstImagePublic != null)
                    {
                        if (pLstImagePublic.Count > 0)
                        {
                            int check = 0;
                            foreach (var info in pLstImagePublic)
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
                                AppImageBL _AppImageBL = new AppImageBL();
                                pReturn = _AppImageBL.AppImageInsertBatch(pLstImagePublic);
                            }
                        }
                    }

                    #region Phí cố định
                    List<AppFeeFixInfo> _lstFeeFix = AppsCommon.CallFee_A01(pDetail, pAppDocumentInfo, pUTienInfo, pLstImagePublic);
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
        [Route("ket_xuat_file")]
        public ActionResult ExportData_View(decimal pAppHeaderId, string p_appCode)
        {
            try
            {
                string _datetimenow = DateTime.Now.ToString("ddMMyyyyHHmm");
                string language = AppsCommon.GetCurrentLang();

                var objBL = new A01_BL();
                List<A01_Info_Export> _lst = new List<A01_Info_Export>();

                List<AppDocumentInfo> appDocumentInfos = new List<AppDocumentInfo>();
                List<AppFeeFixInfo> _lst_appFeeFixInfos = new List<AppFeeFixInfo>();
                ApplicationHeaderInfo applicationHeaderInfo = new ApplicationHeaderInfo();
                List<AuthorsInfo> _lst_authorsInfos = new List<AuthorsInfo>();
                List<Other_MasterInfo> _lst_Other_MasterInfo = new List<Other_MasterInfo>();
                List<AppDocumentOthersInfo> _LstDocumentOthersInfo = new List<AppDocumentOthersInfo>();
                List<AppClassDetailInfo> _lst_appClassDetailInfos = new List<AppClassDetailInfo>();
                List<UTienInfo> pUTienInfo = new List<UTienInfo>();
                List<AppDocumentOthersInfo> pLstImagePublic = new List<AppDocumentOthersInfo>();
                A01_Info_Export app_Detail = objBL.GetByID_Exp(pAppHeaderId, language, ref applicationHeaderInfo, ref appDocumentInfos, ref _lst_appFeeFixInfos,
                    ref _lst_authorsInfos, ref _lst_Other_MasterInfo, ref _lst_appClassDetailInfos, ref _LstDocumentOthersInfo, ref pUTienInfo, ref pLstImagePublic);

                string fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "A01_VN_" + p_appCode + _datetimenow + ".pdf");
                if (app_Detail.Languague_Code == Language.LangVI)
                {
                    fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "A01_VN_" + p_appCode + _datetimenow + ".pdf");
                    SessionData.CurrentUser.FilePreview = "/Content/Export/" + "A01_VN_" + p_appCode + _datetimenow + ".pdf";
                }
                else
                {
                    fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "A01_EN_" + p_appCode + _datetimenow + ".pdf");
                    SessionData.CurrentUser.FilePreview = "/Content/Export/" + "A01_EN_" + p_appCode + _datetimenow + ".pdf";
                }

                AppsCommon.Prepare_Data_Export_A01(ref app_Detail, applicationHeaderInfo, appDocumentInfos, _lst_appFeeFixInfos, _lst_authorsInfos, _lst_Other_MasterInfo,
                       _lst_appClassDetailInfos, _LstDocumentOthersInfo, pUTienInfo, pLstImagePublic);

                _lst.Add(app_Detail);
                DataSet _ds_all = ConvertData.ConvertToDataSet<A01_Info_Export>(_lst, false);
                //_ds_all.WriteXml(@"C:\inetpub\A01.xml", XmlWriteMode.WriteSchema);
                CrystalDecisions.CrystalReports.Engine.ReportDocument oRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                string _tempfile = "A01.rpt";
                if (app_Detail.Language_Code == Language.LangEN)
                {
                    _tempfile = "A01_EN.rpt";
                }
                oRpt.Load(Path.Combine(Server.MapPath("~/Report/"), _tempfile));

                if (_ds_all != null)
                {
                    _ds_all.Tables[0].TableName = "Table1";
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

                //System.IO.Stream oStream_doc = oRpt.ExportToStream(ExportFormatType.WordForWindows);
                //byte[] byteArray_doc= new byte[oStream_doc.Length];
                //oStream_doc.Read(byteArray_doc, 0, Convert.ToInt32(oStream_doc.Length - 1));
                //System.IO.File.WriteAllBytes(fileName_doc, byteArray_doc.ToArray()); // Requires System.Linq

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
        public ActionResult ExportData_View_IU(ApplicationHeaderInfo pInfo, A01_Info pDetail,
            List<AppDocumentInfo> pAppDocumentInfo, List<AppFeeFixInfo> pFeeFixInfo,
            List<AuthorsInfo> pAppAuthorsInfo, List<Other_MasterInfo> pOther_MasterInfo,
            List<AppClassDetailInfo> pAppClassInfo, List<AppDocumentOthersInfo> pAppDocOtherInfo,
            List<UTienInfo> pUTienInfo, List<AppDocumentOthersInfo> pLstImagePublic)
        {
            try
            {
                string _datetimenow = DateTime.Now.ToString("ddMMyyyyHHmm");
                string language = AppsCommon.GetCurrentLang(); 
                var objBL = new A01_BL();
                List<A01_Info_Export> _lst = new List<A01_Info_Export>();

                string p_appCode = "A01_Preview";

                string fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "A01_VN_" + p_appCode + _datetimenow + ".pdf");
                if (language == Language.LangVI)
                {
                    fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "A01_VN_" + p_appCode + _datetimenow + ".pdf");
                    SessionData.CurrentUser.FilePreview = "/Content/Export/" + "A01_VN_" + p_appCode + _datetimenow + ".pdf";
                }
                else
                {
                    fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "A01_EN_" + p_appCode + _datetimenow + ".pdf");
                    SessionData.CurrentUser.FilePreview = "/Content/Export/" + "A01_EN_" + p_appCode + _datetimenow + ".pdf";
                }

                A01_Info_Export _A01_Info_Export = new A01_Info_Export();
                A01_Info_Export.CopyA01_Info(ref _A01_Info_Export, pDetail);

                AppsCommon.Prepare_Data_Export_A01(ref _A01_Info_Export, pInfo, pAppDocumentInfo, pFeeFixInfo, pAppAuthorsInfo, pOther_MasterInfo,
                       pAppClassInfo, pAppDocOtherInfo, pUTienInfo, pLstImagePublic);

                _lst.Add(_A01_Info_Export);
                DataSet _ds_all = ConvertData.ConvertToDataSet<A01_Info_Export>(_lst, false);
                //_ds_all.WriteXml(@"C:\inetpub\A01.xml", XmlWriteMode.WriteSchema);
                CrystalDecisions.CrystalReports.Engine.ReportDocument oRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                string _tempfile = "A01.rpt";
                if (language == Language.LangEN)
                {
                    _tempfile = "A01_EN.rpt";
                }
                oRpt.Load(Path.Combine(Server.MapPath("~/Report/"), _tempfile));

                if (_ds_all != null)
                {
                    _ds_all.Tables[0].TableName = "Table1";
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

                //System.IO.Stream oStream_doc = oRpt.ExportToStream(ExportFormatType.WordForWindows);
                //byte[] byteArray_doc= new byte[oStream_doc.Length];
                //oStream_doc.Read(byteArray_doc, 0, Convert.ToInt32(oStream_doc.Length - 1));
                //System.IO.File.WriteAllBytes(fileName_doc, byteArray_doc.ToArray()); // Requires System.Linq

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
                //ViewBag.FileName = "/Content/Export/" + "A01_VN_" + TradeMarkAppCode.AppCode_A01 + ".pdf";
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
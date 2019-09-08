using BussinessFacade;
using BussinessFacade.ModuleTrademark;
using BussinessFacade.Patent;
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

namespace WebApps.Areas.Patent.Controllers
{
    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("PatentRegistration", AreaPrefix = "lg-patentB03")]
    [Route("{action}")]
    public class B03Controller : Controller
    {
        [HttpGet]
        [Route("register/{id}")]
        public ActionResult B03Display()
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
                return PartialView("~/Areas/Patent/Views/B03/B03Display.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/Patent/Views/B03/B03Display.cshtml");
            }
        }
        [HttpPost]
        [Route("register")]
        public ActionResult Register(ApplicationHeaderInfo pInfo, B03_Info pDetail,
           List<AppDocumentInfo> pAppDocumentInfo, List<AppFeeFixInfo> pFeeFixInfo)
        {

            try
            {
                Application_Header_BL objBL = new Application_Header_BL();
                AppFeeFixBL objFeeFixBL = new AppFeeFixBL();
                B03_BL objDetail = new B03_BL();
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
                    pInfo.Languague_Code = language;
                    if (pInfo.Created_By == null || pInfo.Created_By == "0" || pInfo.Created_By == "")
                    {
                        pInfo.Created_By = CreatedBy;
                    }

                    pInfo.Created_Date = CreatedDate;
                    pInfo.Send_Date = DateTime.Now;
                    pAppHeaderID = objBL.AppHeaderInsert(pInfo, ref p_case_code);
                    if (pReturn < 0)
                        goto Commit_Transaction;

                    // detail
                    if (pAppHeaderID >= 0)
                    {
                        pDetail.Appcode = pInfo.Appcode;
                        pDetail.Language_Code = language;
                        pDetail.App_Header_Id = pAppHeaderID;
                        pDetail.Case_Code = p_case_code;
                        // thiếu thông tin chủ đơn
                        // thiếu mã đơn

                        pReturn = objDetail.Insert(pDetail);
                        if (pReturn <= 0)
                            goto Commit_Transaction;
                    }

                    #region Phí cố định
                    List<AppFeeFixInfo> _lstFeeFix = Call_Fee.CallFee_B03(pDetail, pAppDocumentInfo);
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
        [Route("getFeeView_View")]
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
        [Route("edit")]
        public ActionResult Edit(ApplicationHeaderInfo pInfo, B03_Info pDetail,
           List<AppDocumentInfo> pAppDocumentInfo, List<AppFeeFixInfo> pFeeFixInfo)
        {
            try
            {
                Application_Header_BL objBL = new Application_Header_BL();
                AppFeeFixBL objFeeFixBL = new AppFeeFixBL();
                B03_BL objDetail = new B03_BL();
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
                    pReturn = objBL.AppHeaderUpdate(pInfo);
                    if (pReturn < 0)
                        goto Commit_Transaction;

                    // detail
                    if (pReturn >= 0)
                    {
                        pDetail.App_Header_Id = pInfo.Id;
                        pReturn = objDetail.UpDate(pDetail);
                        if (pReturn <= 0)
                            goto Commit_Transaction;
                    }

                    #region Phí cố định
                    // xóa đi
                    AppFeeFixBL _AppFeeFixBL = new AppFeeFixBL();
                    _AppFeeFixBL.AppFeeFixDelete(pInfo.Case_Code, language);

                    List<AppFeeFixInfo> _lstFeeFix = Call_Fee.CallFee_B03(pDetail, pAppDocumentInfo);
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
                            pReturn = _AppDocumentBL.AppDocumentInsertBath(pAppDocumentInfo, pInfo.Id);

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
        public ActionResult ExportData_View(decimal pAppHeaderId, string p_appCode, decimal p_View_Translate)
        {
            try
            {
                string _datetimenow = DateTime.Now.ToString("ddMMyyyyHHmm");
                string language = AppsCommon.GetCurrentLang();

                var objBL = new B03_BL();
                List<B03_Info_Export> _lst = new List<B03_Info_Export>();

                List<AppDocumentInfo> appDocumentInfos = new List<AppDocumentInfo>();
                List<AppFeeFixInfo> _lst_appFeeFixInfos = new List<AppFeeFixInfo>();
                ApplicationHeaderInfo applicationHeaderInfo = new ApplicationHeaderInfo();

                B03_Info_Export app_Detail = objBL.GetByID_Exp(pAppHeaderId, language, ref applicationHeaderInfo, ref appDocumentInfos, ref _lst_appFeeFixInfos);

                string fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "A01_VN_" + _datetimenow + ".pdf");

                // tiếng việt, bị lộn nên ko muốn đổi
                string _tempfile = "B03.rpt";
                if (p_View_Translate == 1)
                {
                    // nếu là tiếng việt thì xem bản tiếng anh và ngược lại
                    if (applicationHeaderInfo.Languague_Code == Language.LangVI)
                    {
                        _tempfile = "B03.rpt"; // tiếng anh
                        fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "A01_EN_" + _datetimenow + ".pdf");
                        SessionData.CurrentUser.FilePreview = "/Content/Export/" + "A01_EN_" + _datetimenow + ".pdf";
                    }
                    else
                    {
                        fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "A01_VN_" + _datetimenow + ".pdf");
                        SessionData.CurrentUser.FilePreview = "/Content/Export/" + "A01_VN_" + _datetimenow + ".pdf";
                    }
                }
                else
                {
                    if (applicationHeaderInfo.Languague_Code == Language.LangVI)
                    {
                        fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "A01_VN_" + _datetimenow + ".pdf");
                        SessionData.CurrentUser.FilePreview = "/Content/Export/" + "A01_VN_" + _datetimenow + ".pdf";
                    }
                    else
                    {
                        _tempfile = "B03_EN.rpt"; // tiếng anh
                        fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "A01_EN_" + _datetimenow + ".pdf");
                        SessionData.CurrentUser.FilePreview = "/Content/Export/" + "A01_EN_" + _datetimenow + ".pdf";
                    }
                }

                AppsCommon.Prepare_Data_Export_B03(ref app_Detail, applicationHeaderInfo, appDocumentInfos, _lst_appFeeFixInfos);

                _lst.Add(app_Detail);
                DataSet _ds_all = ConvertData.ConvertToDataSet<B03_Info_Export>(_lst, false);
                CrystalDecisions.CrystalReports.Engine.ReportDocument oRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
               

                oRpt.Load(Path.Combine(Server.MapPath("~/Report/"), _tempfile));

                if (_ds_all != null)
                {
                    _ds_all.Tables[0].TableName = "Table1";
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
                //_ds_all.WriteXml(@"D:\MyData.xml");
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
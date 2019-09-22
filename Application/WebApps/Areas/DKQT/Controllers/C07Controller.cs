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
using System.Collections;
using System.Drawing;

namespace WebApps.Areas.DKQT.Controllers
{
    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("C07DKQTRegistration", AreaPrefix = "conversion-international-trademark")]
    [Route("{action}")]
    public class C07Controller : Controller
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
                string AppCode = "";
                if (RouteData.Values.ContainsKey("id"))
                {
                    AppCode = RouteData.Values["id"].ToString().ToUpper();
                }
                ViewBag.AppCode = AppCode.ToUpper();

                AppDetail06DKQT_BL _AppDetail06DKQT_BL = new AppDetail06DKQT_BL();
                List<App_Detail_TM06DKQT_Info> _listTM06 = new List<App_Detail_TM06DKQT_Info>();
                // truyền vào trạng thái nào? để tạm thời = 7 là đã gửi lên cục
                _listTM06 = _AppDetail06DKQT_BL.AppTM06SearchByStatus(7, AppsCommon.GetCurrentLang());
                ViewBag.ListTM06nhdetail = _listTM06;
                ViewBag.Isdisable = 0;
                return PartialView("~/Areas/DKQT/Views/C07/_Partial_C07.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/DKQT/Views/C07/_Partial_C07.cshtml");
            }
        }



        [HttpPost]
        [Route("register")]
        public ActionResult Register(ApplicationHeaderInfo pInfo, C07_Info pDetail,
            List<AppDocumentInfo> pAppDocumentInfo, List<AppFeeFixInfo> pFeeFixInfo,
             List<Other_MasterInfo> pOther_MasterInfo,
             List<AppDocumentOthersInfo> pAppDocOtherInfo, List<AppClassDetailInfo> pAppClassInfo)
        {
            try
            {
                Application_Header_BL objBL = new Application_Header_BL();
                AppFeeFixBL objFeeFixBL = new AppFeeFixBL();
                C07_BL objDetail = new C07_BL();
                AppDocumentBL objDoc = new AppDocumentBL();
                Other_Master_BL _Other_Master_BL = new Other_Master_BL();
                Author_BL _Author_BL = new Author_BL();
                AppClassDetailBL objClassDetail = new AppClassDetailBL();
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
                        if (pDetail.pfileLogo != null)
                        {
                            pDetail.LOGOURL = AppLoadHelpers.PushFileToServer(pDetail.pfileLogo, AppUpload.Logo);
                        }
                        pReturn = objDetail.Insert(pDetail);


                        if (pReturn < 0)
                            goto Commit_Transaction;
                        // thêm thông tin class
                        pReturn = objClassDetail.AppClassDetailInsertBatch(pAppClassInfo, pAppHeaderID, language);
                        if (pReturn < 0)
                            goto Commit_Transaction;
                    }

                    if (pOther_MasterInfo != null && pOther_MasterInfo.Count > 0)
                    {
                        foreach (var item in pOther_MasterInfo)
                        {
                            item.Case_Code = p_case_code;
                            item.App_Header_Id = pAppHeaderID;
                        }

                        decimal _re = _Other_Master_BL.Insert(pOther_MasterInfo);
                        if (_re <= 0)
                            goto Commit_Transaction;
                    }


                    //tai lieu khac 
                    if (pReturn >= 0 && pAppDocOtherInfo != null && pAppDocOtherInfo.Count > 0)
                    {
                        #region Tài liệu khác
                        int check = 0;
                        foreach (var info in pAppDocOtherInfo)
                        {
                            string _keyfileupload = "";
                            if (info.keyFileUpload != null)
                            {
                                _keyfileupload = info.keyFileUpload;
                            }
                            if (SessionData.CurrentUser.chashFile.ContainsKey(_keyfileupload))
                            {
                                var _updateitem = SessionData.CurrentUser.chashFile[info.keyFileUpload];
                                if (_updateitem.GetType() == typeof(string))
                                {
                                    string _url = (string)_updateitem;
                                    info.Filename = _url;
                                    check = 1;
                                }


                            }
                            info.App_Header_Id = pAppHeaderID;
                            info.Language_Code = language;
                        }
                        if (check == 1)
                        {
                            pReturn = objDoc.AppDocumentOtherInsertBatch(pAppDocOtherInfo);
                        }
                        #endregion
                    }



                    #region tính phí
                    List<AppFeeFixInfo> _lstFeeFix = Call_Fee.CallFee_C07(pDetail, pAppClassInfo);
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
                                    var _updateitem = SessionData.CurrentUser.chashFile[info.keyFileUpload];
                                    if (_updateitem.GetType() == typeof(string))
                                    {
                                        string _url = (string)_updateitem;
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
                        return Json(new { status = -1 });

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
        [Route("edit")]
        public ActionResult Edit(ApplicationHeaderInfo pInfo, C07_Info pDetail,
            List<AppDocumentInfo> pAppDocumentInfo, List<AppFeeFixInfo> pFeeFixInfo, List<Other_MasterInfo> pOther_MasterInfo,
             List<AppDocumentOthersInfo> pAppDocOtherInfo, List<AppClassDetailInfo> pAppClassInfo)
        {
            try
            {
                Application_Header_BL objBL = new Application_Header_BL();
                AppFeeFixBL objFeeFixBL = new AppFeeFixBL();
                C07_BL objDetail = new C07_BL();
                AppClassDetailBL objClassDetail = new AppClassDetailBL();
                if (pInfo == null || pDetail == null) return Json(new { status = ErrorCode.Error });
                string language = AppsCommon.GetCurrentLang();
                decimal pReturn = ErrorCode.Success;
                int pAppHeaderID = (int)pInfo.Id;
                string p_case_code = pInfo.Case_Code;

                using (var scope = new TransactionScope())
                {
                    //
                    pInfo.Languague_Code = language;
                    pInfo.Modify_By = SessionData.CurrentUser.Username;
                    pInfo.Modify_Date = SessionData.CurrentUser.CurrentDate;
                    pInfo.Send_Date = DateTime.Now;
                    pInfo.DDSHCN = "";
                    pInfo.MADDSHCN = "";
                    //TRA RA ID CUA BANG KHI INSERT
                    pReturn = objBL.AppHeaderUpdate(pInfo);
                    if (pReturn < 0)
                        goto Commit_Transaction;

                    // detail
                    if (pAppHeaderID >= 0)
                    {
                        pDetail.Appcode = pInfo.Appcode;
                        pDetail.Language_Code = language;
                        pDetail.App_Header_Id = pAppHeaderID;
                        pDetail.Case_Code = p_case_code;
                        if (pDetail.pfileLogo != null)
                        {
                            pDetail.LOGOURL = AppLoadHelpers.PushFileToServer(pDetail.pfileLogo, AppUpload.Logo);
                        }
                        else
                        {
                            pDetail.LOGOURL = pDetail.IMG_URLOrg;
                        }
                        pReturn = objDetail.UpDate(pDetail);

                        if (pReturn < 0)
                            goto Commit_Transaction;

                        #region  Thêm thông tin class
                        if (pReturn >= 0 && pAppClassInfo != null)
                        {

                            //Xoa cac class cu di 
                            pReturn = objClassDetail.AppClassDetailDeleted(pInfo.Id, language);
                            if (pReturn < 0)
                                goto Commit_Transaction;

                            pReturn = objClassDetail.AppClassDetailInsertBatch(pAppClassInfo, pInfo.Id, language);

                            if (pReturn < 0)
                                goto Commit_Transaction;
                        }
                        #endregion

                    }



                    // ok 
                    Other_Master_BL _Other_Master_BL = new Other_Master_BL();
                    _Other_Master_BL.Deleted(pInfo.Case_Code, language);
                    if (pOther_MasterInfo != null && pOther_MasterInfo.Count > 0)
                    {
                        foreach (var item in pOther_MasterInfo)
                        {
                            item.Case_Code = pInfo.Case_Code;
                            item.App_Header_Id = pAppHeaderID;
                        }

                        decimal _re = _Other_Master_BL.Insert(pOther_MasterInfo);
                        if (_re <= 0)
                            goto Commit_Transaction;
                    }




                    //tai lieu khac 
                    #region Tài liệu khác
                    AppDocumentBL objDoc = new AppDocumentBL();
                    List<AppDocumentOthersInfo> Lst_Doc_Others = objDoc.DocumentOthers_GetByAppHeader(pInfo.Id, language);
                    List<AppDocumentOthersInfo> Lst_Doc_Others_Old = Lst_Doc_Others.FindAll(m => m.FILETYPE == 1).ToList();
                    Dictionary<decimal, AppDocumentOthersInfo> _dic_doc_others = new Dictionary<decimal, AppDocumentOthersInfo>();
                    foreach (AppDocumentOthersInfo item in Lst_Doc_Others_Old)
                    {
                        _dic_doc_others[item.Id] = item;
                    }

                    // xóa đi trước insert lại sau
                    objDoc.AppDocumentOtherDeletedByApp_Type(pInfo.Id, language, 1);

                    if (pReturn >= 0 && pAppDocOtherInfo != null && pAppDocOtherInfo.Count > 0)
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
                            info.App_Header_Id = pAppHeaderID;
                            info.Language_Code = language;
                        }
                        if (check == 1)
                        {
                            pReturn = objDoc.AppDocumentOtherInsertBatch(pAppDocOtherInfo);
                        }
                    }
                    #endregion


                    #region tính phí
                    // xóa đi
                    AppFeeFixBL _AppFeeFixBL = new AppFeeFixBL();
                    _AppFeeFixBL.AppFeeFixDelete(pInfo.Case_Code, language);

                    List<AppFeeFixInfo> _lstFeeFix = Call_Fee.CallFee_C07(pDetail, pAppClassInfo);
                    if (_lstFeeFix.Count > 0)
                    {
                        pReturn = _AppFeeFixBL.AppFeeFixInsertBath(_lstFeeFix, p_case_code);
                        if (pReturn < 0)
                            goto Commit_Transaction;
                    }
                    #endregion

                    #region Tai lieu dinh kem 
                    if (pReturn >= 0 && pAppDocumentInfo != null && pAppDocumentInfo.Count > 0)
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
                            info.App_Header_Id = pDetail.App_Header_Id;
                            if (SessionData.CurrentUser.chashFile.ContainsKey(info.keyFileUpload))
                            {
                                var _updateitem = SessionData.CurrentUser.chashFile[info.keyFileUpload];
                                if (_updateitem.GetType() == typeof(string))
                                {
                                    string _url = (string)_updateitem;
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

                            info.App_Header_Id = pAppHeaderID;
                            info.Document_Filing_Date = CommonFuc.CurrentDate();
                            info.Language_Code = language;
                        }
                        pReturn = objDoc.AppDocumentInsertBath(pAppDocumentInfo, pAppHeaderID);
                    }
                #endregion

                //end
                Commit_Transaction:
                    if (pReturn < 0)
                    {
                        Transaction.Current.Rollback();
                        return Json(new { status = -1 });

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
        [Route("ket_xuat_file_IU")]
        public ActionResult ExportData_View_IU(ApplicationHeaderInfo pInfo, C07_Info pDetail,
           List<AppDocumentInfo> pAppDocumentInfo,
           List<AppFeeFixInfo> pFeeFixInfo,
           List<Other_MasterInfo> pOther_MasterInfo,
           List<AppClassDetailInfo> pAppClassInfo,
           List<AppDocumentOthersInfo> pAppDocOtherInfo 
           )
        {
            try
            {
                string _datetimenow = DateTime.Now.ToString("ddMMyyyyHHmm");
                string language = AppsCommon.GetCurrentLang();
                var objBL = new C07_BL();
                List<C07_Info_Export> _lst = new List<C07_Info_Export>();

                string p_appCode = "C07_Preview";

                string fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "C07_VN_" + _datetimenow + ".pdf");
                if (language == Language.LangVI)
                {
                    fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "C07_VN_" + _datetimenow + ".pdf");
                    SessionData.CurrentUser.FilePreview = "/Content/Export/" + "C07_VN_" + _datetimenow + ".pdf";
                }
                else
                {
                    fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "C07_EN_" + _datetimenow + ".pdf");
                    SessionData.CurrentUser.FilePreview = "/Content/Export/" + "C07_EN_" + _datetimenow + ".pdf";
                }

                C07_Info_Export _C07_Info_Export = new C07_Info_Export();

                C07_Info_Export.CopyC07_Info(ref _C07_Info_Export, pDetail);

                _C07_Info_Export.LOGOURL = Server.MapPath(_C07_Info_Export.LOGOURL);

                // Phí cố định

                List<AppFeeFixInfo> _lstFeeFix = Call_Fee.CallFee_C07(pDetail, pAppClassInfo);
                Prepare_Data_Export_C07(ref _C07_Info_Export, pInfo, pAppDocumentInfo, _lstFeeFix, pOther_MasterInfo,
                       pAppDocOtherInfo, pAppClassInfo);

                _lst.Add(_C07_Info_Export);
                DataSet _ds_all = ConvertData.ConvertToDataSet<C07_Info_Export>(_lst, false);
                try
                {
                    _ds_all.WriteXml(@"C:\inetpub\C07.xml", XmlWriteMode.WriteSchema);
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }

                CrystalDecisions.CrystalReports.Engine.ReportDocument oRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                string _tempfile = "C07.rpt";
                if (pInfo.View_Language_Report == Language.LangEN)
                {
                    _tempfile = "C07_EN.rpt";
                }
                oRpt.Load(Path.Combine(Server.MapPath("~/Report/"), _tempfile));
                #region Set vị trí ảnh

                CrystalDecisions.CrystalReports.Engine.PictureObject _pic01;
                _pic01 = (CrystalDecisions.CrystalReports.Engine.PictureObject)oRpt.ReportDefinition.Sections[0].ReportObjects["Picture1"];
                _pic01.Width = 100;
                _pic01.Height = 100;
                try
                {
                    pDetail.LOGOURL = Server.MapPath(pDetail.LOGOURL);
                    Bitmap img = new Bitmap(pDetail.LOGOURL);
                    try
                    {

                        double _Const = 6.666666666666;
                        int _left = 0, _top = 0, _marginleft = 225, _margintop = 5580;
                        int _h = 600;
                        double _d1 = (_h - img.Width) / 2;
                        _d1 = _Const * _d1;
                        _left = _marginleft + Convert.ToInt32(_d1);
                        if (_left < 0)
                        {
                            _left = _marginleft;
                        }
                        _pic01.Left = _left;
                        // top

                        _d1 = (_h - img.Height) / 2;
                        _d1 = _Const * _d1;
                        _top = _margintop + Convert.ToInt32(_d1);
                        if (_top < 0)
                        {
                            _top = _margintop;
                        }
                        _pic01.Top = _top;

                    }
                    catch (Exception ex)
                    {
                        Logger.LogException(ex);
                    }
                    finally
                    {
                        img.Dispose();
                    }
                }
                catch (Exception)
                {


                }

                System.IO.FileInfo file = new System.IO.FileInfo(pDetail.LOGOURL);

                #endregion
                if (_ds_all != null)
                {
                    _ds_all.Tables[0].TableName = "Table";
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
        [Route("ket_xuat_file")]
        public ActionResult ExportData_View(decimal pAppHeaderId, string p_appCode, string p_Language)
        {
            try
            {
                string _datetimenow = DateTime.Now.ToString("ddMMyyyyHHmm");
                string language = AppsCommon.GetCurrentLang();

                var objBL = new C07_BL();
                List<C07_Info_Export> _lst = new List<C07_Info_Export>();

                List<AppDocumentInfo> appDocumentInfos = new List<AppDocumentInfo>();
                List<AppFeeFixInfo> _lst_appFeeFixInfos = new List<AppFeeFixInfo>();
                ApplicationHeaderInfo applicationHeaderInfo = new ApplicationHeaderInfo();
                List<Other_MasterInfo> _lst_Other_MasterInfo = new List<Other_MasterInfo>();
                List<AppDocumentOthersInfo> _LstDocumentOthersInfo = new List<AppDocumentOthersInfo>();
                List<AppClassDetailInfo> appClassDetailInfos = new List<AppClassDetailInfo>();
                C07_Info_Export pDetail = objBL.GetByID_Exp(pAppHeaderId, language, ref applicationHeaderInfo, ref appDocumentInfos, ref _lst_appFeeFixInfos,
                    ref _lst_Other_MasterInfo, ref _LstDocumentOthersInfo, ref appClassDetailInfos);
                pDetail.LOGOURL = Server.MapPath(pDetail.LOGOURL);
                string fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "C07_VN_" + _datetimenow + ".pdf");
                if (pDetail.Languague_Code == Language.LangVI)
                {
                    fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "C07_VN_" + _datetimenow + ".pdf");
                    SessionData.CurrentUser.FilePreview = "/Content/Export/" + "C07_VN_" + _datetimenow + ".pdf";
                }
                else
                {
                    fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "C07_EN_" + _datetimenow + ".pdf");
                    SessionData.CurrentUser.FilePreview = "/Content/Export/" + "C07_EN_" + _datetimenow + ".pdf";
                }

                Prepare_Data_Export_C07(ref pDetail, applicationHeaderInfo, appDocumentInfos, _lst_appFeeFixInfos, _lst_Other_MasterInfo,
                      _LstDocumentOthersInfo, appClassDetailInfos);

                _lst.Add(pDetail);
                DataSet _ds_all = ConvertData.ConvertToDataSet<C07_Info_Export>(_lst, false);
                try
                {
                    _ds_all.WriteXml(@"C:\inetpub\C07.xml", XmlWriteMode.WriteSchema);
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
                CrystalDecisions.CrystalReports.Engine.ReportDocument oRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                string _tempfile = "C07.rpt";
                if (p_Language == Language.LangEN)
                {
                    _tempfile = "C07_EN.rpt";
                }
                oRpt.Load(Path.Combine(Server.MapPath("~/Report/"), _tempfile));
                #region Set vị trí ảnh

                CrystalDecisions.CrystalReports.Engine.PictureObject _pic01;
                _pic01 = (CrystalDecisions.CrystalReports.Engine.PictureObject)oRpt.ReportDefinition.Sections[0].ReportObjects["Picture1"];
                _pic01.Width = 100;
                _pic01.Height = 100;
                try
                {

                    Bitmap img = new Bitmap(pDetail.LOGOURL);
                    try
                    {

                        double _Const = 6.666666666666;
                        int _left = 0, _top = 0, _marginleft = 225, _margintop = 5580;
                        int _h = 600;
                        double _d1 = (_h - img.Width) / 2;
                        _d1 = _Const * _d1;
                        _left = _marginleft + Convert.ToInt32(_d1);
                        if (_left < 0)
                        {
                            _left = _marginleft;
                        }
                        _pic01.Left = _left;
                        // top

                        _d1 = (_h - img.Height) / 2;
                        _d1 = _Const * _d1;
                        _top = _margintop + Convert.ToInt32(_d1);
                        if (_top < 0)
                        {
                            _top = _margintop;
                        }
                        _pic01.Top = _top;

                    }
                    catch (Exception ex)
                    {
                        Logger.LogException(ex);
                    }
                    finally
                    {
                        img.Dispose();
                    }
                }
                catch (Exception ex)
                {


                }

                System.IO.FileInfo file = new System.IO.FileInfo(pDetail.LOGOURL);

                #endregion
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

        public static void Prepare_Data_Export_C07(ref C07_Info_Export app_Detail, ApplicationHeaderInfo applicationHeaderInfo,
           List<AppDocumentInfo> appDocumentInfos, List<AppFeeFixInfo> _lst_appFeeFixInfos, List<Other_MasterInfo> _lst_Other_MasterInfo,
             List<AppDocumentOthersInfo> _LstDocumentOthersInfo, List<AppClassDetailInfo> appClassDetailInfos)
        {
            try
            {
                // copy Header
                C07_Info_Export.CopyAppHeaderInfo(ref app_Detail, applicationHeaderInfo);


                // copy class

                // copy tác giả





                // copy chủ đơn khác
                if (_lst_Other_MasterInfo != null && _lst_Other_MasterInfo.Count > 1)
                {
                    C07_Info_Export.CopyOther_MasterInfo(ref app_Detail, _lst_Other_MasterInfo[0], 0);
                }
                else
                {
                    C07_Info_Export.CopyOther_MasterInfo(ref app_Detail, null, 0);
                }

                if (_lst_Other_MasterInfo != null && _lst_Other_MasterInfo.Count > 2)
                {
                    C07_Info_Export.CopyOther_MasterInfo(ref app_Detail, _lst_Other_MasterInfo[1], 1);
                }
                else
                {
                    C07_Info_Export.CopyOther_MasterInfo(ref app_Detail, null, 1);
                }


                #region Tài liệu có trong đơn

                if (_LstDocumentOthersInfo != null)
                {
                    foreach (var item in _LstDocumentOthersInfo)
                    {
                        if (!string.IsNullOrEmpty(item.Documentname))
                        {
                            app_Detail.strDanhSachFileDinhKem += item.Documentname + " ; ";
                        }
                    }
                    if (!string.IsNullOrEmpty(app_Detail.strDanhSachFileDinhKem))
                    {
                        app_Detail.strDanhSachFileDinhKem = app_Detail.strDanhSachFileDinhKem.Substring(0, app_Detail.strDanhSachFileDinhKem.Length - 2);
                    }

                }

                if (appDocumentInfos != null)
                {
                    foreach (AppDocumentInfo item in appDocumentInfos)
                    {
                        if (item.Document_Id == "C07_00")
                        {
                            app_Detail.Doc_Id_0 = item.CHAR01;
                            app_Detail.Doc_Id_002 = item.CHAR02;
                            app_Detail.Doc_Id_0_Check = item.Isuse;
                        }
                        if (item.Document_Id == "C07_01")
                        {
                            app_Detail.Doc_Id_1 = item.CHAR01;
                            app_Detail.Doc_Id_102 = item.CHAR02;
                            app_Detail.Doc_Id_1_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "C07_02")
                        {
                            app_Detail.Doc_Id_2 = item.CHAR01;
                            app_Detail.Doc_Id_202 = item.CHAR02;

                            app_Detail.Doc_Id_2_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "C07_03")
                        {
                            app_Detail.Doc_Id_3_Check = item.Isuse;
                            app_Detail.Doc_Id_3 = item.CHAR01;
                            app_Detail.Doc_Id_302 = item.CHAR02;
                        }
                        else if (item.Document_Id == "C07_04")
                        {
                            app_Detail.Doc_Id_4 = item.CHAR01;
                            app_Detail.Doc_Id_4_Check = item.Isuse;
                            app_Detail.Doc_Id_402 = item.CHAR02;
                        }
                        else if (item.Document_Id == "C07_05")
                        {
                            app_Detail.Doc_Id_5_Check = item.Isuse;
                            app_Detail.Doc_Id_5 = item.CHAR01;
                        }

                        else if (item.Document_Id == "C07_06")
                        {
                            app_Detail.Doc_Id_6_Check = item.Isuse;
                            app_Detail.Doc_Id_6 = item.CHAR01;
                        }
                        else if (item.Document_Id == "C07_07")
                        {
                            app_Detail.Doc_Id_7_Check = item.Isuse;
                            app_Detail.Doc_Id_7 = item.CHAR01;
                        }


                        else if (item.Document_Id == "C07_08")
                        {
                            app_Detail.Doc_Id_8_Check = item.Isuse;
                            app_Detail.Doc_Id_8 = item.CHAR01;
                        }

                        else if (item.Document_Id == "C07_09")
                        {
                            app_Detail.Doc_Id_9 = item.CHAR01;
                            app_Detail.Doc_Id_9_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "C07_10")
                        {
                            app_Detail.Doc_Id_10_Check = item.Isuse;
                            app_Detail.Doc_Id_10 = item.CHAR01;
                        }
                        else if (item.Document_Id == "C07_11")
                        {
                            app_Detail.Doc_Id_11_Check = item.Isuse;
                            app_Detail.Doc_Id_11 = item.CHAR01;
                        }


                    }

                }

                #endregion

                #region Fee
      
              
                 
                if (_lst_appFeeFixInfos.Count > 0)
                {
                    foreach (var item in _lst_appFeeFixInfos)
                    {
                        if (item.Fee_Id == 1)
                        {
                            app_Detail.Fee_Id_1 = item.Isuse == 0 ? "" : item.Number_Of_Patent.ToString();
                            app_Detail.Fee_Id_1_Check = item.Isuse;

                            app_Detail.Fee_Id_1_Val = item.Amount.ToString("#,##0.##");
                        }
                        else if (item.Fee_Id == 2)
                        {
                            app_Detail.Fee_Id_2 = item.Isuse == 0 ? "" : item.Number_Of_Patent.ToString();
                            app_Detail.Fee_Id_2_Check = item.Isuse;
                            app_Detail.Fee_Id_2_Val = item.Amount.ToString("#,##0.##");
                        }
                        else if (item.Fee_Id == 21)
                        {
                            app_Detail.Fee_Id_21 = item.Isuse == 0 ? "" : item.Number_Of_Patent.ToString();
                            app_Detail.Fee_Id_21_Check = item.Isuse;
                            app_Detail.Fee_Id_21_Val = item.Amount.ToString("#,##0.##");
                        }


                        else if (item.Fee_Id == 3)
                        {
                            app_Detail.Fee_Id_3 = item.Isuse == 0 ? "" : item.Number_Of_Patent.ToString();
                            app_Detail.Fee_Id_3_Check = item.Isuse;
                            app_Detail.Fee_Id_3_Val = item.Amount.ToString("#,##0.##");
                        }
                        else if (item.Fee_Id == 4)
                        {
                            app_Detail.Fee_Id_4 = item.Isuse == 0 ? "" : item.Number_Of_Patent.ToString();
                            app_Detail.Fee_Id_4_Check = item.Isuse;
                            app_Detail.Fee_Id_4_Val = item.Amount.ToString("#,##0.##");
                        }

                        else if (item.Fee_Id == 41)
                        {
                            app_Detail.Fee_Id_41 = item.Isuse == 0 ? "" : item.Number_Of_Patent.ToString();
                            app_Detail.Fee_Id_41_Check = item.Isuse;
                            app_Detail.Fee_Id_41_Val = item.Amount.ToString("#,##0.##");
                        }
                        else if (item.Fee_Id == 5)
                        {
                            app_Detail.Fee_Id_5 = item.Isuse == 0 ? "" : item.Number_Of_Patent.ToString();
                            app_Detail.Fee_Id_5_Check = item.Isuse;
                            app_Detail.Fee_Id_5_Val = item.Amount.ToString("#,##0.##");
                        }
                        else if (item.Fee_Id == 51)
                        {
                            app_Detail.Fee_Id_51 = item.Isuse == 0 ? "" : item.Number_Of_Patent.ToString();
                            app_Detail.Fee_Id_51_Check = item.Isuse;
                            app_Detail.Fee_Id_51_Val = item.Amount.ToString("#,##0.##");
                        }
                        app_Detail.Total_Fee = app_Detail.Total_Fee + item.Amount;
                        app_Detail.Total_Fee_Str = app_Detail.Total_Fee.ToString("#,##0.##");
                    }
                }
                #endregion

                #region class


                foreach (AppClassDetailInfo item in appClassDetailInfos.OrderBy(m => m.Code))
                {
                    // nếu là tiếng việt thì hiện tiếng anh
                    if (AppsCommon.GetCurrentLang() == "VI_VN")
                    {
                        app_Detail.strListClass += "Nhóm" + item.Code.Substring(0, 2) + ": " + item.Textinput.Trim().Trim(',') + " (" + (item.IntTongSanPham < 10 ? "0" + item.IntTongSanPham.ToString() : item.IntTongSanPham.ToString()) + " " + "sản phẩm" + " )" + "\n";
                    }
                    else
                    {
                        app_Detail.strListClass += "Class " + item.Code.Substring(0, 2) + ": " + item.Textinput.Trim().Trim(',') + " (" + (item.IntTongSanPham < 10 ? "0" + item.IntTongSanPham.ToString() : item.IntTongSanPham.ToString()) + " " + "gooods" + " )" + "\n";

                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
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
        public ActionResult GetFee(C07_Info pDetail, List<AppClassDetailInfo> pAppClassInfo)
        {
            try
            {
                List<AppFeeFixInfo> _lstFeeFix = Call_Fee.CallFee_C07(pDetail, pAppClassInfo);
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
    }
}
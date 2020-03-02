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
    [RouteArea("PatentRegistration", AreaPrefix = "lg-patent-lao")]
    [Route("{action}")]
    public class Lao_PTController : Controller
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
                ViewBag.AppCode = AppCode;

                return PartialView("~/Areas/Patent/Views/Lao_PT/_Partial_Lao_PT_Insert.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/Patent/Views/Lao_PT/_Partial_Lao_PT_Insert.cshtml");
            }
        }

        [HttpGet]
        [Route("register/{id}/{id1}")]
        public ActionResult Index_Search()
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

                if (RouteData.Values.ContainsKey("id1"))
                {
                    string _SearchCode = RouteData.Values["id1"].ToString().ToUpper();
                    ViewBag.SearchCode = _SearchCode;

                    SearchObject_BL _searchBL = new SearchObject_BL();
                    List<SearchObject_Detail_Info> _ListDetail = new List<SearchObject_Detail_Info>();
                    SearchObject_Question_Info _QuestionInfo = new SearchObject_Question_Info();
                    List<AppClassDetailInfo> search_Class_Infos = new List<AppClassDetailInfo>();
                    SearchObject_Header_Info _HeaderInfo = _searchBL.SEARCH_HEADER_GETBY_CASECODE(_SearchCode, ref _ListDetail, ref _QuestionInfo, ref search_Class_Infos);
                    ViewBag.Name = _HeaderInfo.Name;
                }
                return PartialView("~/Areas/Patent/Views/Lao_PT/_Partial_Lao_PT_Insert.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/Patent/Views/Lao_PT/_Partial_Lao_PT_Insert.cshtml");
            }
        }

        [HttpPost]
        [Route("inventor/more")]
        public ActionResult ThemTacGia(decimal p_id)
        {
            return PartialView("~/Areas/Patent/Views/Lao_PT/_Partial_Box_No_3_Other.cshtml", p_id.ToString());
        }

        [HttpPost]
        [Route("don-uu-tien/them-don")]
        public ActionResult ThemDonUuTien(string p_id)
        {
            return PartialView("~/Areas/Patent/Views/Lao_PT/_PartialThongTinQuyenUuTien.cshtml", p_id.ToString());
        }

        [HttpPost]
        [Route("hinh-cong-bo/them")]
        public ActionResult ThemHinhCongBo(string p_id)
        {
            return PartialView("~/Areas/Patent/Views/Shared/_Partial_Image_Public_Child_Lao.cshtml", p_id.ToString());
        }

        [HttpPost]
        [Route("tai-lieu-khac/them")]
        public ActionResult ThemTaiLieuKhac(string p_id)
        {
            return PartialView("~/Areas/Patent/Views/Shared/_Lao_Partial_Document_Others_Child.cshtml", p_id.ToString());
        }

        [HttpPost]
        [Route("chu-don/them-chu-don")]
        public ActionResult ThemChuDon(string p_id)
        {
            return PartialView("~/Areas/Patent/Views/Lao_PT/_Partial_Box_No_2_Other.cshtml", p_id.ToString());
        }

        [HttpPost]
        [Route("register")]
        public ActionResult Register(ApplicationHeaderInfo pInfo, Pattent_Lao_Info pDetail,
            List<AppDocumentInfo> pAppDocumentInfo, List<AppFeeFixInfo> pFeeFixInfo,
            List<Other_MasterInfo> pOther_MasterInfo,
            List<Inventor_Info> pInventor_Info,
            List<AppClassDetailInfo> pAppClassInfo, List<AppDocumentOthersInfo> pAppDocOtherInfo,
            List<UTienInfo> pUTienInfo, List<AppDocumentOthersInfo> pLstImagePublic)
        {
            try
            {
                Application_Header_BL objBL = new Application_Header_BL();
                AppFeeFixBL objFeeFixBL = new AppFeeFixBL();
                Pattent_Lao_BL objDetail = new Pattent_Lao_BL();
                AppDocumentBL objDoc = new AppDocumentBL();
                Other_Master_BL _Other_Master_BL = new Other_Master_BL();
                Inventor_BL _Inventor_BL = new Inventor_BL();

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

                    if (pInventor_Info != null && pInventor_Info.Count > 0)
                    {
                        foreach (var item in pInventor_Info)
                        {
                            item.Case_Code = p_case_code;
                            item.App_Header_Id = pAppHeaderID;
                        }
                        decimal _re = _Inventor_BL.Insert(pInventor_Info);
                        if (_re <= 0)
                            goto Commit_Transaction;
                    }

                    if (pUTienInfo != null && pUTienInfo.Count > 0)
                    {
                        foreach (var item in pUTienInfo)
                        {
                            item.Case_Code = p_case_code;
                            item.App_Header_Id = pAppHeaderID;
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

                    // hình công bố
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

                    #region Phí cố định
                    List<AppFeeFixInfo> _lstFeeFix = Call_Fee.CallFee_PT_Lao(pDetail, pAppDocumentInfo, pUTienInfo, pLstImagePublic, pAppDocOtherInfo);
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
                return Json(new { status = pReturn });

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { status = ErrorCode.Error });
            }
        }

        [HttpPost]
        [Route("edit")]
        public ActionResult Edit(ApplicationHeaderInfo pInfo, Pattent_Lao_Info pDetail,
            List<AppDocumentInfo> pAppDocumentInfo, List<AppFeeFixInfo> pFeeFixInfo,
            List<AuthorsInfo> pAppAuthorsInfo, List<Other_MasterInfo> pOther_MasterInfo,
            List<Inventor_Info> pInventor_Info,
            List<AppClassDetailInfo> pAppClassInfo, List<AppDocumentOthersInfo> pAppDocOtherInfo,
            List<UTienInfo> pUTienInfo, List<AppDocumentOthersInfo> pLstImagePublic)
        {
            try
            {
                Application_Header_BL objBL = new Application_Header_BL();
                AppFeeFixBL objFeeFixBL = new AppFeeFixBL();
                Pattent_Lao_BL objDetail = new Pattent_Lao_BL();
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

                    Inventor_BL _Inventor_BL = new Inventor_BL();
                    _Inventor_BL.Deleted(pInfo.Case_Code, language);
                    if (pInventor_Info != null && pInventor_Info.Count > 0)
                    {
                        foreach (var item in pInventor_Info)
                        {
                            item.Case_Code = pInfo.Case_Code;
                            item.App_Header_Id = pInfo.Id;
                        }
                        decimal _re = _Inventor_BL.Insert(pInventor_Info);
                        if (_re <= 0)
                            goto Commit_Transaction;
                    }

                    Other_Master_BL _Other_Master_BL = new Other_Master_BL();
                    _Other_Master_BL.Deleted(pInfo.Case_Code, language);
                    if (pOther_MasterInfo != null && pOther_MasterInfo.Count > 0)
                    {
                        foreach (var item in pOther_MasterInfo)
                        {
                            item.Case_Code = pInfo.Case_Code;
                            item.App_Header_Id = pInfo.Id;
                        }

                        decimal _re = _Other_Master_BL.Insert(pOther_MasterInfo);
                        if (_re <= 0)
                            goto Commit_Transaction;
                    }

                    // xóa đi trước insert lại sau
                    Uu_Tien_BL _Uu_Tien_BL = new Uu_Tien_BL();
                    _Uu_Tien_BL.Deleted(pInfo.Case_Code, language);

                    if (pUTienInfo != null && pUTienInfo.Count > 0)
                    {
                        foreach (var item in pUTienInfo)
                        {
                            item.Case_Code = pInfo.Case_Code;
                            item.App_Header_Id = pInfo.Id;
                        }

                        decimal _re = _Uu_Tien_BL.Insert(pUTienInfo);
                        if (_re <= 0)
                            goto Commit_Transaction;
                    }

                    //tai lieu khac 
                    if (pReturn >= 0 && pAppDocOtherInfo != null)
                    {
                        AppDocumentBL objDoc = new AppDocumentBL();
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

                    // hình công bố
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

                    #region Phí cố định
                    // xóa đi
                    AppFeeFixBL _AppFeeFixBL = new AppFeeFixBL();
                    _AppFeeFixBL.AppFeeFixDelete(pInfo.Case_Code, language);

                    List<AppFeeFixInfo> _lstFeeFix = Call_Fee.CallFee_PT_Lao(pDetail, pAppDocumentInfo, pUTienInfo, pLstImagePublic, pAppDocOtherInfo);
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

                var objBL = new Pattent_Lao_BL();
                List<Pattent_Lao_Info_Export> _lst = new List<Pattent_Lao_Info_Export>();

                List<AppDocumentInfo> appDocumentInfos = new List<AppDocumentInfo>();
                List<AppFeeFixInfo> _lst_appFeeFixInfos = new List<AppFeeFixInfo>();
                ApplicationHeaderInfo applicationHeaderInfo = new ApplicationHeaderInfo();
                List<Inventor_Info> _Lst_Inventor_Info = new List<Inventor_Info>();
                List<Other_MasterInfo> _lst_Other_MasterInfo = new List<Other_MasterInfo>();
                List<AppDocumentOthersInfo> _LstDocumentOthersInfo = new List<AppDocumentOthersInfo>();
                List<AppClassDetailInfo> _lst_appClassDetailInfos = new List<AppClassDetailInfo>();
                List<UTienInfo> pUTienInfo = new List<UTienInfo>();
                List<AppDocumentOthersInfo> pLstImagePublic = new List<AppDocumentOthersInfo>();
                Pattent_Lao_Info app_Detail = objBL.GetByID(pAppHeaderId, language, ref applicationHeaderInfo, ref appDocumentInfos, ref _lst_appFeeFixInfos,
                    ref _Lst_Inventor_Info, ref _lst_Other_MasterInfo, ref _lst_appClassDetailInfos, ref _LstDocumentOthersInfo, ref pUTienInfo, ref pLstImagePublic);

                string fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "L_Patent_VN_" + _datetimenow + ".pdf");
                if (language == Language.LangVI)
                {
                    fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "L_Patent_VN_" + _datetimenow + ".pdf");
                    SessionData.CurrentUser.FilePreview = "/Content/Export/" + "L_Patent_VN_" + _datetimenow + ".pdf";
                }
                else
                {
                    fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "L_Patent_EN_" + _datetimenow + ".pdf");
                    SessionData.CurrentUser.FilePreview = "/Content/Export/" + "L_Patent_EN_" + _datetimenow + ".pdf";
                }

                Pattent_Lao_Info_Export _Info_Export = new Pattent_Lao_Info_Export();
                Pattent_Lao_Info_Export.CopyA01_Info(ref _Info_Export, app_Detail);

                foreach (var item in MemoryData.c_lst_Country)
                {
                    if (item.Country_Id == applicationHeaderInfo.Master_Country_Nationality)
                    {
                        applicationHeaderInfo.Master_Country_Nationality_Name = item.Name;
                    }
                    if (item.Country_Id == applicationHeaderInfo.Master_Country_Incorporation)
                    {
                        applicationHeaderInfo.Master_Country_Incorporation_Name = item.Name;
                    }
                    if (item.Country_Id == applicationHeaderInfo.Master_Country_Residence)
                    {
                        applicationHeaderInfo.Master_Country_Residence_Name = item.Name;
                    }
                }


                // Phí cố định
                List<AppFeeFixInfo> _lstFeeFix = new List<AppFeeFixInfo>();

                Call_Fee.Prepare_Data_Export_PT_Lao(ref _Info_Export, applicationHeaderInfo, appDocumentInfos, _lstFeeFix, _Lst_Inventor_Info, _lst_Other_MasterInfo,
                                      _lst_appClassDetailInfos, _LstDocumentOthersInfo, pUTienInfo, pLstImagePublic);

                _lst.Add(_Info_Export);
                DataSet _ds_all = ConvertData.ConvertToDataSet<Pattent_Lao_Info_Export>(_lst, false);
                CrystalDecisions.CrystalReports.Engine.ReportDocument oRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                //_ds_all.WriteXml(@"E:\LPT.xml", XmlWriteMode.WriteSchema);

                string _tempfile = "L_Patent.rpt";
                //if (language == Language.LangEN)
                //{
                //    _tempfile = "L_Patent_EN.rpt";
                //}
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

        [HttpPost]
        [Route("ket_xuat_file_IU")]
        public ActionResult ExportData_View_IU(ApplicationHeaderInfo pInfo, Pattent_Lao_Info pDetail,
            List<AppDocumentInfo> pAppDocumentInfo, List<AppFeeFixInfo> pFeeFixInfo,
            List<Other_MasterInfo> pOther_MasterInfo,
            List<Inventor_Info> pInventor_Info,
            List<AppClassDetailInfo> pAppClassInfo, List<AppDocumentOthersInfo> pAppDocOtherInfo,
            List<UTienInfo> pUTienInfo, List<AppDocumentOthersInfo> pLstImagePublic)
        {
            try
            {
                string _datetimenow = DateTime.Now.ToString("ddMMyyyyHHmm");
                string language = AppsCommon.GetCurrentLang();
                //var objBL = new A01_BL();
                List<Pattent_Lao_Info_Export> _lst = new List<Pattent_Lao_Info_Export>();

                string fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "L_Patent_VN_" + _datetimenow + ".pdf");
                if (language == Language.LangVI)
                {
                    fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "L_Patent_VN_" + _datetimenow + ".pdf");
                    SessionData.CurrentUser.FilePreview = "/Content/Export/" + "L_Patent_VN_" + _datetimenow + ".pdf";
                }
                else
                {
                    fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "L_Patent_EN_" + _datetimenow + ".pdf");
                    SessionData.CurrentUser.FilePreview = "/Content/Export/" + "L_Patent_EN_" + _datetimenow + ".pdf";
                }

                Pattent_Lao_Info_Export _A01_Info_Export = new Pattent_Lao_Info_Export();
                Pattent_Lao_Info_Export.CopyA01_Info(ref _A01_Info_Export, pDetail);

                foreach (var item in MemoryData.c_lst_Country)
                {
                    if (item.Country_Id == pInfo.Master_Country_Nationality)
                    {
                        pInfo.Master_Country_Nationality_Name = item.Name;

                    }
                    if (item.Country_Id == pInfo.Master_Country_Incorporation)
                    {
                        pInfo.Master_Country_Incorporation_Name = item.Name;

                    }
                    if (item.Country_Id == pInfo.Master_Country_Residence)
                    {
                        pInfo.Master_Country_Residence_Name = item.Name;

                    }
                }


                // Phí cố định
                List<AppFeeFixInfo> _lstFeeFix = Call_Fee.CallFee_PT_Lao(pDetail, pAppDocumentInfo, pUTienInfo, pLstImagePublic, pAppDocOtherInfo);
               
                 Call_Fee.Prepare_Data_Export_PT_Lao(ref _A01_Info_Export, pInfo, pAppDocumentInfo, _lstFeeFix, pInventor_Info, pOther_MasterInfo,
                       pAppClassInfo, pAppDocOtherInfo, pUTienInfo, pLstImagePublic);

                _lst.Add(_A01_Info_Export);
                DataSet _ds_all = ConvertData.ConvertToDataSet<Pattent_Lao_Info_Export>(_lst, false);
                //_ds_all.WriteXml(@"D:\A01.xml", XmlWriteMode.WriteSchema);
                CrystalDecisions.CrystalReports.Engine.ReportDocument oRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                string _tempfile = "L_Patent.rpt";
                //if (language == Language.LangEN)
                //{
                //    _tempfile = "L_Patent_EN.rpt";
                //}
                oRpt.Load(Path.Combine(Server.MapPath("~/Report/"), _tempfile));

                if (_ds_all != null)
                {
                    _ds_all.Tables[0].TableName = "Table1";
                    oRpt.Database.Tables["Table"].SetDataSource(_ds_all.Tables[0]);
                    //oRpt.SetDataSource(_ds_all);
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
                //ViewBag.FileName = "/Content/Export/" + "A01_VN_" + TradeMarkAppCode.AppCode_A01 + ".pdf";
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
        public ActionResult GetFee(A01_Info pDetail, List<AppDocumentInfo> pAppDocumentInfo, List<UTienInfo> pUTienInfo, List<AppDocumentOthersInfo> pLstImagePublic)
        {
            try
            {
                List<AppFeeFixInfo> _lstFeeFix = Call_Fee.CallFee_A01(pDetail, pAppDocumentInfo, pUTienInfo, pLstImagePublic);
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
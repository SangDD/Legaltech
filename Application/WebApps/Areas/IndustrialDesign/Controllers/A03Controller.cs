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

namespace WebApps.Areas.IndustrialDesign.Controllers
{
    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("IndustrialDesignRegistration", AreaPrefix = "indus-design")]
    [Route("{action}")]
    public class A03Controller : Controller
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
                return PartialView("~/Areas/IndustrialDesign/Views/A03/_Partial_A03.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/IndustrialDesign/Views/A03/_Partial_A03.cshtml");
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

                ViewBag.AppCode = AppCode;
                return PartialView("~/Areas/IndustrialDesign/Views/A03/_Partial_A03.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/IndustrialDesign/Views/A03/_Partial_A03.cshtml");
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
        [Route("hinh-cong-bo/them-cung-cap-0")]
        public ActionResult ThemHinhCongBo(decimal p_id)
        {
            return PartialView("~/Areas/IndustrialDesign/Views/Shared/_PartialTreeDocument_Child_0.cshtml", p_id.ToString());
        }

        [HttpPost]
        [Route("hinh-cong-bo/them-cung-cap-2")]
        public ActionResult ThemHinhCongBo2(decimal p_id, decimal p_refId)
        {
            return PartialView("~/Areas/IndustrialDesign/Views/Shared/_PartialTreeDocument_Child_2.cshtml", p_id.ToString() + "|" + p_refId.ToString());
        }

        [HttpPost]
        [Route("register")]
        public ActionResult Register(ApplicationHeaderInfo pInfo, A03_Info pDetail,
            List<AppDocumentInfo> pAppDocumentInfo, List<AppFeeFixInfo> pFeeFixInfo,
            List<AuthorsInfo> pAppAuthorsInfo, List<Other_MasterInfo> pOther_MasterInfo,
            List<AppClassDetailInfo> pAppClassInfo, List<AppDocumentOthersInfo> pAppDocOtherInfo,
            List<UTienInfo> pUTienInfo, List<AppDocumentOthersInfo> pAppDocIndusDesign)
        {
            try
            {
                Application_Header_BL objBL = new Application_Header_BL();
                AppFeeFixBL objFeeFixBL = new AppFeeFixBL();
                A03_BL objDetail = new A03_BL();
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
                        pReturn = objDetail.Insert(pDetail);

                        ////Thêm thông tin class
                        //if (pReturn >= 0 && pAppClassInfo != null)
                        //{
                        //    pReturn = objClassDetail.AppClassDetailInsertBatch(pAppClassInfo, pAppHeaderID, language);
                        //}
                        if (pReturn < 0)
                            goto Commit_Transaction;
                    }

                    if (pAppAuthorsInfo != null && pAppAuthorsInfo.Count > 0)
                    {
                        foreach (var item in pAppAuthorsInfo)
                        {
                            item.Case_Code = p_case_code;
                            item.App_Header_Id = pAppHeaderID;
                        }
                        decimal _re = _Author_BL.Insert(pAppAuthorsInfo);
                        if (_re <= 0)
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

                                //if(_updateitem.GetType() == typeof(HttpPostedFileBase))
                                //{
                                //    HttpPostedFileBase pfiles = (HttpPostedFileBase)_updateitem;
                                //    info.Filename = pfiles.FileName;
                                //    info.Filename = "/Content/Archive/" + AppUpload.Document + "/" + pfiles.FileName;
                                //    check = 1;
                                //}
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


                    #region bộ tài liệu ảnh
                    if (pReturn >= 0 && pAppDocIndusDesign != null)
                    {
                        if (pAppDocIndusDesign.Count > 0)
                        {
                            int check = 0;
                            foreach (var info in pAppDocIndusDesign)
                            {
                                if (SessionData.CurrentUser.chashFile.ContainsKey(info.keyFileUpload))
                                {
                                    var _updateitem = SessionData.CurrentUser.chashFile[info.keyFileUpload];
                                    if (_updateitem.GetType() == typeof(AppDocumentInfo))
                                    {
                                        HttpPostedFileBase pfiles = (_updateitem as AppDocumentInfo).pfiles;

                                        info.Filename = pfiles.FileName;
                                        info.Filename = AppLoadHelpers.convertToUnSign2(info.Filename);
                                        info.Filename = System.Text.RegularExpressions.Regex.Replace(info.Filename, "[^0-9A-Za-z.]+", "_");

                                        info.Filename = "/Content/Archive/" + AppUpload.Document + "/" + pfiles.FileName;
                                        info.IdRef = Convert.ToDecimal((_updateitem as AppDocumentInfo).refId);
                                        check = 1;
                                    }
                                }
                                info.App_Header_Id = pAppHeaderID;
                                info.Language_Code = language;
                            }
                            if (check == 1)
                            {
                                pReturn = objDoc.AppDocumentOtherInsertBatch(pAppDocIndusDesign);
                            }
                        }
                    }
                    #endregion

                    #region tính phí
                    List<AppFeeFixInfo> _lstFeeFix = Call_Fee.CallFee_A03(pDetail, pAppDocumentInfo, pUTienInfo, pAppDocIndusDesign);
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
        public ActionResult Edit(ApplicationHeaderInfo pInfo, A03_Info pDetail,
            List<AppDocumentInfo> pAppDocumentInfo, List<AppFeeFixInfo> pFeeFixInfo,
            List<AuthorsInfo> pAppAuthorsInfo, List<Other_MasterInfo> pOther_MasterInfo,
            List<AppClassDetailInfo> pAppClassInfo, List<AppDocumentOthersInfo> pAppDocOtherInfo,
            List<UTienInfo> pUTienInfo, List<AppDocumentOthersInfo> pAppDocIndusDesign)
        {
            try
            {
                Application_Header_BL objBL = new Application_Header_BL();
                AppFeeFixBL objFeeFixBL = new AppFeeFixBL();
                A03_BL objDetail = new A03_BL();
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
                        pReturn = objDetail.UpDate(pDetail);

                        if (pReturn <= 0)
                            goto Commit_Transaction;
                    }

                    // ok 
                    Author_BL _Author_BL = new Author_BL();
                    _Author_BL.Deleted(pInfo.Case_Code, language);
                    if (pAppAuthorsInfo != null && pAppAuthorsInfo.Count > 0)
                    {
                        foreach (var item in pAppAuthorsInfo)
                        {
                            item.Case_Code = pInfo.Case_Code;
                            item.App_Header_Id = pAppHeaderID;
                        }
                        decimal _re = _Author_BL.Insert(pAppAuthorsInfo);
                        if (_re <= 0)
                            goto Commit_Transaction;
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

                    // xóa đi trước insert lại sau -> ok 
                    Uu_Tien_BL _Uu_Tien_BL = new Uu_Tien_BL();
                    _Uu_Tien_BL.Deleted(pInfo.Case_Code, language);

                    if (pUTienInfo != null && pUTienInfo.Count > 0)
                    {
                        foreach (var item in pUTienInfo)
                        {
                            item.Case_Code = pInfo.Case_Code;
                            item.App_Header_Id = pAppHeaderID;
                        }

                        decimal _re = _Uu_Tien_BL.Insert(pUTienInfo);
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

                    #region bộ tài liệu ảnh -> chưa sửa
                    // chưa sửa
                    List<AppDocumentOthersInfo>  Lst_DocIndusDesign_Old = Lst_Doc_Others.FindAll(m => m.FILETYPE == 2).ToList();
                    Dictionary<decimal, AppDocumentOthersInfo> _dic_DocIndusDesign = new Dictionary<decimal, AppDocumentOthersInfo>();
                    foreach (AppDocumentOthersInfo item in Lst_DocIndusDesign_Old)
                    {
                        _dic_DocIndusDesign[item.Id] = item;
                    }

                    // xóa đi trước insert lại sau
                    objDoc.AppDocumentOtherDeletedByApp_Type(pInfo.Id, language, 2);

                    if (pReturn >= 0 && pAppDocIndusDesign != null && pAppDocIndusDesign.Count > 0)
                    {
                        int check = 0;
                        foreach (var info in pAppDocIndusDesign)
                        {
                            if (SessionData.CurrentUser.chashFile.ContainsKey(info.keyFileUpload))
                            {
                                var _updateitem = SessionData.CurrentUser.chashFile[info.keyFileUpload];
                                if (_updateitem.GetType() == typeof(AppDocumentInfo))
                                {
                                    HttpPostedFileBase pfiles = (_updateitem as AppDocumentInfo).pfiles;

                                    info.Filename = pfiles.FileName;
                                    info.Filename = AppLoadHelpers.convertToUnSign2(info.Filename);
                                    info.Filename = System.Text.RegularExpressions.Regex.Replace(info.Filename, "[^0-9A-Za-z.]+", "_");

                                    info.Filename = "/Content/Archive/" + AppUpload.Document + "/" + pfiles.FileName;
                                    //info.IdRef = Convert.ToDecimal((_updateitem as AppDocumentInfo).refId);
                                    check = 1;
                                }
                            }
                            else if (_dic_DocIndusDesign.ContainsKey(info.Id))
                            {
                                info.Filename = _dic_DocIndusDesign[info.Id].Filename;
                                //info.IdRef = _dic_doc_others[info.Id].IdRef;
                                check = 1;
                            }

                            info.App_Header_Id = pAppHeaderID;
                            info.Language_Code = language;
                        }
                        if (check == 1)
                        {
                            pReturn = objDoc.AppDocumentOtherInsertBatch(pAppDocIndusDesign);
                        }
                    }
                    #endregion

                    #region tính phí
                    // xóa đi
                    AppFeeFixBL _AppFeeFixBL = new AppFeeFixBL();
                    _AppFeeFixBL.AppFeeFixDelete(pInfo.Case_Code, language);

                    List<AppFeeFixInfo> _lstFeeFix = Call_Fee.CallFee_A03(pDetail, pAppDocumentInfo, pUTienInfo, pAppDocIndusDesign);
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
        [Route("push-file-other-to-server")]
        public ActionResult PushFileOtherToServer(AppDocumentInfo pInfo) //AppDocumentInfo de lay thong tin add vao hash thoi
        {
            try
            {
                if (pInfo.pfiles != null)
                {
                    var url = AppLoadHelpers.PushFileToServer(pInfo.pfiles, AppUpload.Document);
                    SessionData.CurrentUser.chashFile[pInfo.keyFileUpload] = pInfo;
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
        [Route("ket_xuat_file_IU")]
        public ActionResult ExportData_View_IU(ApplicationHeaderInfo pInfo, A03_Info pDetail,
           List<AppDocumentInfo> pAppDocumentInfo, List<AppFeeFixInfo> pFeeFixInfo,
           List<AuthorsInfo> pAppAuthorsInfo, List<Other_MasterInfo> pOther_MasterInfo,
           List<AppClassDetailInfo> pAppClassInfo, List<AppDocumentOthersInfo> pAppDocOtherInfo,
           List<UTienInfo> pUTienInfo, List<AppDocumentOthersInfo> pAppDocIndusDesign)
        {
            try
            {
                string _datetimenow = DateTime.Now.ToString("ddMMyyyyHHmm");
                string language = AppsCommon.GetCurrentLang();
                var objBL = new A03_BL();
                List<A03_Info_Export> _lst = new List<A03_Info_Export>();

                string p_appCode = "A03_Preview";

                string fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "A03_VN_" + _datetimenow + ".pdf");
                if (language == Language.LangVI)
                {
                    fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "A03_VN_" + _datetimenow + ".pdf");
                    SessionData.CurrentUser.FilePreview = "/Content/Export/" + "A03_VN_" + _datetimenow + ".pdf";
                }
                else
                {
                    fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "A03_EN_" + _datetimenow + ".pdf");
                    SessionData.CurrentUser.FilePreview = "/Content/Export/" + "A03_EN_" + _datetimenow + ".pdf";
                }

                A03_Info_Export _A03_Info_Export = new A03_Info_Export();
                A03_Info_Export.CopyA03_Info(ref _A03_Info_Export, pDetail);


                // Phí cố định

                List<AppFeeFixInfo> _lstFeeFix = Call_Fee.CallFee_A03(pDetail, pAppDocumentInfo, pUTienInfo, pAppDocIndusDesign);
                Prepare_Data_Export_A03(ref _A03_Info_Export, pInfo, pAppDocumentInfo, _lstFeeFix, pAppAuthorsInfo, pOther_MasterInfo,
                       pAppClassInfo, pAppDocOtherInfo, pUTienInfo, pAppDocIndusDesign);

                _lst.Add(_A03_Info_Export);
                DataSet _ds_all = ConvertData.ConvertToDataSet<A03_Info_Export>(_lst, false);
                //_ds_all.WriteXml(@"C:\inetpub\A03.xml", XmlWriteMode.WriteSchema);
                CrystalDecisions.CrystalReports.Engine.ReportDocument oRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                string _tempfile = "A03.rpt";
                if (language == Language.LangEN)
                {
                    _tempfile = "A03_EN.rpt";
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
        public ActionResult ExportData_View(decimal pAppHeaderId, string p_appCode, decimal p_View_Translate)
        {
            try
            {
                string _datetimenow = DateTime.Now.ToString("ddMMyyyyHHmm");
                string language = AppsCommon.GetCurrentLang();

                var objBL = new A03_BL();
                List<A03_Info_Export> _lst = new List<A03_Info_Export>();

                List<AppDocumentInfo> appDocumentInfos = new List<AppDocumentInfo>();
                List<AppFeeFixInfo> _lst_appFeeFixInfos = new List<AppFeeFixInfo>();
                ApplicationHeaderInfo applicationHeaderInfo = new ApplicationHeaderInfo();
                List<AuthorsInfo> _lst_authorsInfos = new List<AuthorsInfo>();
                List<Other_MasterInfo> _lst_Other_MasterInfo = new List<Other_MasterInfo>();
                List<AppDocumentOthersInfo> _LstDocumentOthersInfo = new List<AppDocumentOthersInfo>();
                List<AppClassDetailInfo> _lst_appClassDetailInfos = new List<AppClassDetailInfo>();
                List<UTienInfo> pUTienInfo = new List<UTienInfo>();
                List<AppDocumentOthersInfo> pLstImageDesign = new List<AppDocumentOthersInfo>();
                A03_Info_Export app_Detail = objBL.GetByID_Exp(pAppHeaderId, language, ref applicationHeaderInfo, ref appDocumentInfos, ref _lst_appFeeFixInfos,
                    ref _lst_authorsInfos, ref _lst_Other_MasterInfo, ref _lst_appClassDetailInfos, ref _LstDocumentOthersInfo, ref pUTienInfo, ref pLstImageDesign);

                string _tempfile = "A03.rpt";
                string fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "A03_VN_" + _datetimenow + ".pdf");
                if (p_View_Translate == 1)
                {
                    // nếu là tiếng việt thì xem bản tiếng anh và ngược lại
                    if (applicationHeaderInfo.Languague_Code == Language.LangVI)
                    {
                        _tempfile = "A03_EN.rpt"; // tiếng anh
                        fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "A03_EN_" + _datetimenow + ".pdf");
                        SessionData.CurrentUser.FilePreview = "/Content/Export/" + "A03_EN_" + _datetimenow + ".pdf";
                    }
                    else
                    {
                        fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "A03_VN_" + _datetimenow + ".pdf");
                        SessionData.CurrentUser.FilePreview = "/Content/Export/" + "A03_VN_" + _datetimenow + ".pdf";
                    }
                }
                else
                {
                    if (applicationHeaderInfo.Languague_Code == Language.LangVI)
                    {
                        fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "A03_VN_" + _datetimenow + ".pdf");
                        SessionData.CurrentUser.FilePreview = "/Content/Export/" + "A03_VN_" + _datetimenow + ".pdf";
                    }
                    else
                    {
                        _tempfile = "A03_EN.rpt"; // tiếng anh
                        fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "A03_EN_" + _datetimenow + ".pdf");
                        SessionData.CurrentUser.FilePreview = "/Content/Export/" + "A03_EN_" + _datetimenow + ".pdf";
                    }
                }

                Prepare_Data_Export_A03(ref app_Detail, applicationHeaderInfo, appDocumentInfos, _lst_appFeeFixInfos, _lst_authorsInfos, _lst_Other_MasterInfo,
                       _lst_appClassDetailInfos, _LstDocumentOthersInfo, pUTienInfo, pLstImageDesign);

                _lst.Add(app_Detail);
                DataSet _ds_all = ConvertData.ConvertToDataSet<A03_Info_Export>(_lst, false);
                //_ds_all.WriteXml(@"C:\inetpub\A03.xml", XmlWriteMode.WriteSchema);
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

        public static void Prepare_Data_Export_A03(ref A03_Info_Export app_Detail, ApplicationHeaderInfo applicationHeaderInfo,
           List<AppDocumentInfo> appDocumentInfos, List<AppFeeFixInfo> _lst_appFeeFixInfos,
           List<AuthorsInfo> _lst_authorsInfos, List<Other_MasterInfo> _lst_Other_MasterInfo,
           List<AppClassDetailInfo> pAppClassInfo, List<AppDocumentOthersInfo> _LstDocumentOthersInfo,
           List<UTienInfo> pUTienInfo, List<AppDocumentOthersInfo> pAppDocIndusDesign)
        {
            try
            {
                // copy Header
                A03_Info_Export.CopyAppHeaderInfo(ref app_Detail, applicationHeaderInfo);


                // copy class
                if (pAppClassInfo != null)
                {
                    Hashtable _hsGroupclass = new Hashtable();
                    foreach (var item in pAppClassInfo)
                    {
                        AppClassDetailInfo _newinfo = new AppClassDetailInfo();
                        _newinfo.CloneObj();
                        if (_hsGroupclass.ContainsKey(item.Code.Substring(0, 2)))
                        {
                            _newinfo = (AppClassDetailInfo)_hsGroupclass[item.Code.Substring(0, 2)];
                        }
                        _newinfo.Code = item.Code;
                        _newinfo.Textinput += item.Textinput + ", ";
                        _newinfo.IntTongSanPham++;
                        _hsGroupclass[item.Code.Substring(0, 2)] = _newinfo;
                    }
                    //appInfo.strListClass = "Tổng số nhóm: \n" + appInfo.strTongSonhom + "; Tổng số sản phẩm: " + appInfo.strTongSoSP + " ; Danh sách nhóm: " + appInfo.strListClass;
                    List<AppClassDetailInfo> _listApp = new List<AppClassDetailInfo>();
                    foreach (DictionaryEntry item in _hsGroupclass)
                    {
                        _listApp.Add((AppClassDetailInfo)item.Value);
                    }
                    foreach (AppClassDetailInfo item in _listApp.OrderBy(m => m.Code))
                    {
                        app_Detail.strListClass += Resources.Resource.lblNhom + item.Code.Substring(0, 2) + ": " + item.Textinput.Trim().Trim(',') + " (" + (item.IntTongSanPham < 10 ? "0" + item.IntTongSanPham.ToString() : item.IntTongSanPham.ToString()) + " " + Resources.Resource.lblSanPham + " )" + "\n";
                    }
                }

                // copy tác giả
                if (_lst_authorsInfos != null && _lst_authorsInfos.Count > 0)
                {
                    A03_Info_Export.CopyAuthorsInfo(ref app_Detail, _lst_authorsInfos[0], 0);
                }

                if (_lst_authorsInfos != null && _lst_authorsInfos.Count > 1)
                {
                    A03_Info_Export.CopyAuthorsInfo(ref app_Detail, _lst_authorsInfos[1], 1);
                    app_Detail.Author_Others = "Y";
                }
                else
                {
                    A03_Info_Export.CopyAuthorsInfo(ref app_Detail, null, 1);
                    app_Detail.Author_Others = "N";
                }

                if (_lst_authorsInfos != null &&  _lst_authorsInfos.Count > 2)
                {
                    A03_Info_Export.CopyAuthorsInfo(ref app_Detail, _lst_authorsInfos[2], 2);
                }
                else
                {
                    A03_Info_Export.CopyAuthorsInfo(ref app_Detail, null, 2);
                }

                // copy chủ đơn khác
                if (_lst_Other_MasterInfo != null && _lst_Other_MasterInfo.Count > 1)
                {
                    A03_Info_Export.CopyOther_MasterInfo(ref app_Detail, _lst_Other_MasterInfo[0], 0);
                }
                else
                {
                    A03_Info_Export.CopyOther_MasterInfo(ref app_Detail, null, 0);
                }

                if (_lst_Other_MasterInfo != null && _lst_Other_MasterInfo.Count > 2)
                {
                    A03_Info_Export.CopyOther_MasterInfo(ref app_Detail, _lst_Other_MasterInfo[1], 1);
                }
                else
                {
                    A03_Info_Export.CopyOther_MasterInfo(ref app_Detail, null, 1);
                }

                // copy đơn ưu tiên
                if (pUTienInfo  != null && pUTienInfo.Count > 0)
                {
                    A03_Info_Export.CopyUuTienInfo(ref app_Detail, pUTienInfo[0]);
                }
                else
                {
                    A03_Info_Export.CopyUuTienInfo(ref app_Detail, null);
                }

                #region Tài liệu có trong đơn

                if (_LstDocumentOthersInfo != null)
                {
                    foreach (var item in _LstDocumentOthersInfo)
                    {
                        if(!string.IsNullOrEmpty(item.Documentname))
                        {
                            app_Detail.strDanhSachFileDinhKem += item.Documentname + " ; ";
                        }
                    }
                    if (!string.IsNullOrEmpty(app_Detail.strDanhSachFileDinhKem))
                    {
                        app_Detail.strDanhSachFileDinhKem = app_Detail.strDanhSachFileDinhKem.Substring(0, app_Detail.strDanhSachFileDinhKem.Length - 2);
                    }

                }

                if(appDocumentInfos != null)
                {
                    foreach (AppDocumentInfo item in appDocumentInfos)
                    {
                        if (item.Document_Id == "A03_01")
                        {
                            app_Detail.Doc_Id_1 = item.CHAR01;
                            app_Detail.Doc_Id_102 = item.CHAR02;
                            app_Detail.Doc_Id_1_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "A03_02")
                        {
                            app_Detail.Doc_Id_2 = item.CHAR01;
                            app_Detail.Doc_Id_202 = item.CHAR02;

                            app_Detail.Doc_Id_2_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "A03_03")
                        {
                            app_Detail.Doc_Id_3_Check = item.Isuse;
                            app_Detail.Doc_Id_3 = item.CHAR01;
                            app_Detail.Doc_Id_302 = item.CHAR02;
                        }
                        else if (item.Document_Id == "A03_04")
                        {
                            app_Detail.Doc_Id_4 = item.CHAR01;
                            app_Detail.Doc_Id_402 = item.CHAR02;
                            app_Detail.Doc_Id_4_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "A03_05")
                        {
                            app_Detail.Doc_Id_5_Check = item.Isuse;
                            app_Detail.Doc_Id_5 = item.CHAR01;
                        }

                        else if (item.Document_Id == "A03_06")
                        {
                            app_Detail.Doc_Id_6_Check = item.Isuse;
                            app_Detail.Doc_Id_6 = item.CHAR01;
                            app_Detail.Doc_Id_602 = item.CHAR02;
                        }
                        else if (item.Document_Id == "A03_07")
                        {
                            app_Detail.Doc_Id_7_Check = item.Isuse;
                            app_Detail.Doc_Id_7 = item.CHAR01;
                        }
                        else if (item.Document_Id == "A03_07")
                        {
                            app_Detail.Doc_Id_8_Check = item.Isuse;
                            app_Detail.Doc_Id_8 = item.CHAR01;
                        }

                        else if (item.Document_Id == "A03_09")
                        {
                            app_Detail.Doc_Id_9 = item.CHAR01;
                            app_Detail.Doc_Id_9_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "A03_10")
                        {
                            app_Detail.Doc_Id_10_Check = item.Isuse;
                            app_Detail.Doc_Id_10 = item.CHAR01;
                            app_Detail.Doc_Id_1002 = item.CHAR02;
                        }
                        // quyền ưu tiên cũ A03_11
                        else if (item.Document_Id == "1_TLCMQUT")
                        {
                            app_Detail.Doc_Id_11 = item.CHAR01;
                            app_Detail.Doc_Id_11_Check = item.Isuse;
                        }

                        // cũ A03_12
                        else if (item.Document_Id == "1_BanSaoDauTien")
                        {
                            app_Detail.Doc_Id_12 = item.CHAR01;
                            app_Detail.Doc_Id_12_Check = item.Isuse;
                        }

                        // cũ A03_13
                        else if (item.Document_Id == "1_GiayChuyenNhuong")
                        {
                            app_Detail.Doc_Id_13 = item.CHAR01;
                            app_Detail.Doc_Id_13_Check = item.Isuse;
                        }

                        // end quyền ưu tiên

                        else if (item.Document_Id == "A03_14")
                        {
                            app_Detail.Doc_Id_14 = item.CHAR01;
                            app_Detail.Doc_Id_14_Check = item.Isuse;
                        }

                        else if (item.Document_Id == "A03_15")
                        {
                            app_Detail.Doc_Id_15 = item.CHAR01;
                            app_Detail.Doc_Id_15_Check = item.Isuse;
                        }
                    }

                    if (pUTienInfo == null)
                    {
                        app_Detail.Doc_Id_11 = "";
                        app_Detail.Doc_Id_12 = "";
                        app_Detail.Doc_Id_13 = "";
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

                        else if (item.Fee_Id == 6)
                        {
                            app_Detail.Fee_Id_6 = item.Isuse == 0 ? "" : item.Number_Of_Patent.ToString();
                            app_Detail.Fee_Id_6_Check = item.Isuse;
                            app_Detail.Fee_Id_6_Val = item.Amount.ToString("#,##0.##");
                        }

                        app_Detail.Total_Fee = app_Detail.Total_Fee + item.Amount;
                        app_Detail.Total_Fee_Str = app_Detail.Total_Fee.ToString("#,##0.##");
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
        public ActionResult GetFee(A03_Info pDetail, List<AppDocumentInfo> pAppDocumentInfo, List<UTienInfo> pUTienInfo, List<AppDocumentOthersInfo> pAppDocIndusDesign)
        {
            try
            {
                List<AppFeeFixInfo> _lstFeeFix = Call_Fee.CallFee_A03(pDetail, pAppDocumentInfo, pUTienInfo, pAppDocIndusDesign);
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
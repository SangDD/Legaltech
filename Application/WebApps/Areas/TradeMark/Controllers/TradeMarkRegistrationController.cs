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
    using BussinessFacade.ModuleMemoryData;
    using System.Data;
    using BussinessFacade;
    using CrystalDecisions.Shared;
    using System.Linq;
    using System.Drawing;
    using System.Collections;
    using Common.CommonData;

    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("TradeMarkRegistration", AreaPrefix = "trade-mark")]
    [Route("{action}")]
    public class TradeMarkRegistrationController : Controller
    {
        // GET: TradeMark/TradeMarkRegistration
        public static List<AppDocumentOthersInfo> lstDocOther = new List<AppDocumentOthersInfo>();
        [HttpGet]
        [Route("dang-ky-nhan-hieu")]
        public ActionResult DangKyNhanHieu()
        {
            try
            {

                if (SessionData.CurrentUser == null)
                {
                    return this.Redirect("/");
                }
                string language = AppsCommon.GetCurrentLang();
                ViewBag.lstData = SysApplicationBL.GetSysAppByLanguage(language);
                return View("~/Areas/TradeMark/Views/TradeMarkRegistration/DangKyNhanHieu.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex); return View("~/Areas/TradeMark/Views/TradeMarkRegistration/DangKyNhanHieu.cshtml");
            }
        }

        [HttpGet]
        [Route("request-for-trade-mark/{id}")]
        public ActionResult TradeMarkChoiseApplication()
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
                if (AppCode == TradeMarkAppCode.AppCodeSuaDoiDangKy)
                {
                    return AppSuaDoiDonDangKy();
                }
                else if (AppCode == TradeMarkAppCode.AppCodeDangKyChuyenDoi)
                {

                }
                else if (AppCode == TradeMarkAppCode.AppCodeDangKynhanHieu)
                {
                    return AppDangKyNhanHieu();
                }
                else if (AppCode == TradeMarkAppCode.AppCode_TM_3B_PLB_01_SDD)
                {
                    return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistration/_Partial_TM_3B_PLB_01_SDD.cshtml");
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return AppSuaDoiDonDangKy();
        }

        public ActionResult AppSuaDoiDonDangKy()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistration/AppSuaDoiDonDangKy.cshtml");
        }

        public ActionResult AppDangKyNhanHieu()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistration/_PartalDangKyNhanHieu.cshtml");
        }

        [HttpPost]
        [Route("dang_ky_nhan_hieu")]
        public ActionResult AppDonDangKyInsert(ApplicationHeaderInfo pInfo, AppDetail04NHInfo pDetail, List<AppDocumentInfo> pAppDocumentInfo,
            List<AppDocumentOthersInfo> pAppDocOtherInfo, List<AppClassDetailInfo> pAppClassInfo)
        {
            try
            {
                Application_Header_BL objBL = new Application_Header_BL();
                if (pDetail.LoaiNhanHieu == "undefined") pDetail.LoaiNhanHieu = "";
                AppFeeFixBL objFeeFixBL = new AppFeeFixBL();
                AppDetail04NHBL objDetail = new AppDetail04NHBL();
                AppClassDetailBL objClassDetail = new AppClassDetailBL();
                AppDocumentBL objDoc = new AppDocumentBL();
                if (pInfo == null || pDetail == null) return Json(new { status = ErrorCode.Error });
                string language = AppsCommon.GetCurrentLang();
                var CreatedBy = SessionData.CurrentUser.Username;
                var CreatedDate = SessionData.CurrentUser.CurrentDate;
                int pReturn = ErrorCode.Success;
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
                    pInfo.Send_Date = CreatedDate;
                    //TRA RA ID CUA BANG KHI INSERT
                    pAppHeaderID = objBL.AppHeaderInsert(pInfo, ref p_case_code);
                    if (pAppHeaderID >= 0)
                    {
                        pDetail.Appcode = pInfo.Appcode;
                        pDetail.Language_Code = language;
                        pDetail.App_Header_Id = pAppHeaderID;
                        if (pDetail.pfileLogo != null)
                        {
                            pDetail.Logourl = AppLoadHelpers.PushFileToServer(pDetail.pfileLogo, AppUpload.Logo);
                        }
                        pReturn = objDetail.App_Detail_04NH_Insert(pDetail);
                        //Thêm thông tin class
                        if (pReturn >= 0 && pAppClassInfo != null)
                        {
                            pReturn = objClassDetail.AppClassDetailInsertBatch(pAppClassInfo, pAppHeaderID, language);
                        }
                    }
                    else
                    {
                        return Json(new { status = pAppHeaderID });
                    }
                    //Tai lieu dinh kem 
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


                                }
                                info.Status = 0;
                                info.App_Header_Id = pAppHeaderID;
                                info.Language_Code = language;
                                info.Document_Filing_Date = CommonFuc.CurrentDate();
                            }
                            pReturn = objDoc.AppDocumentInsertBath(pAppDocumentInfo, pAppHeaderID);

                        }
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
                    //Tính phí 
                    if (pReturn >= 0 && pAppClassInfo != null)
                    {
                        var listfeeCaculator = new List<AppFeeFixInfo>();
                        pReturn = CaculatorFee(pAppClassInfo, pDetail.Sodon_Ut, p_case_code, listfeeCaculator);
                    }
                    //end
                    if (pReturn < 0)
                    {
                        Transaction.Current.Rollback();
                        return Json(new { status = pReturn });
                    }
                    else
                    {
                        MemoryData.Enqueue_ChangeData(Table_Change.APPHEADER);
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
        [Route("them_moi_don_dang_ky_sua_doi")]
        public ActionResult AppSuaDoiDonDangKyInsert(ApplicationHeaderInfo pInfo, List<AppFeeFixInfo> pFeeFixInfo,
                        AppDetail01Info pDetailInfo, List<AppDocumentInfo> pAppDocumentInfo)
        {
            try
            {
                Application_Header_BL objBL = new Application_Header_BL();
                AppFeeFixBL objFeeFixBL = new AppFeeFixBL();
                AppDetail01BL objDetail01BL = new AppDetail01BL();
                AppDocumentBL objDoc = new AppDocumentBL();
                if (pInfo == null || pDetailInfo == null) return Json(new { status = ErrorCode.Error });
                string language = AppsCommon.GetCurrentLang();
                var CreatedBy = SessionData.CurrentUser.Username;
                var CreatedDate = SessionData.CurrentUser.CurrentDate;
                int pReturn = ErrorCode.Success;
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

                    //TRA RA ID CUA BANG KHI INSERT
                    pAppHeaderID = objBL.AppHeaderInsert(pInfo, ref p_case_code);
                    //Gán lại khi lấy dl 
                    if (pAppHeaderID >= 0)
                    {
                        pReturn = objFeeFixBL.AppFeeFixInsertBath(pFeeFixInfo, p_case_code);
                    }
                    else
                    {
                        Transaction.Current.Rollback();
                    }
                    if (pReturn >= 0)
                    {
                        pDetailInfo.Language_Code = language;
                        pDetailInfo.App_Header_Id = pAppHeaderID;
                        pReturn = objDetail01BL.AppDetailInsert(pDetailInfo);
                    }
                    else
                    {
                        Transaction.Current.Rollback();
                    }
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
                                    info.Url_Hardcopy = "/Content/DataWareHouse" + pfiles.FileName;
                                    info.Status = 0;
                                }
                                info.Document_Filing_Date = CommonFuc.CurrentDate();
                                info.Language_Code = language;
                            }
                            pReturn = objDoc.AppDocumentInsertBath(pAppDocumentInfo, pAppHeaderID);
                            if (pReturn < 0)
                            {
                                Transaction.Current.Rollback();
                            }
                            else
                            {
                                scope.Complete();
                            }
                        }
                        else
                        {
                            scope.Complete();
                        }
                    }
                    else
                    {
                        Transaction.Current.Rollback();
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
        [Route("push-file-to-server")]
        public ActionResult PushFileToServer(AppDocumentInfo pInfo)
        {
            try
            {
                if (pInfo.pfiles != null)
                {
                    var url = AppLoadHelpers.PushFileToServer(pInfo.pfiles, AppUpload.Document);
                    SessionData.CurrentUser.chashFile[pInfo.keyFileUpload] = pInfo.pfiles;
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
        [Route("push-file-other-to-server")]
        public ActionResult PushFileOtherToServer(AppDocumentInfo pInfo) //AppDocumentInfo de lay thong tin add vao hash thoi
        {
            try
            {
                if (pInfo.pfiles != null)
                {
                    var url = AppLoadHelpers.PushFileToServer(pInfo.pfiles, AppUpload.Document);
                    SessionData.CurrentUser.chashFileOther[pInfo.keyFileUpload] = pInfo.pfiles;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = -1 });
            }
            return Json(new { success = 0 });
        }

        /// <summary>
        /// hungtd sửa kết xuất pdf
        /// </summary>
        /// <param name="pInfo"></param>
        /// <param name="pDetail"></param>
        /// <param name="pAppDocumentInfo"></param>
        /// <param name="pAppDocOtherInfo"></param>
        /// <param name="pAppClassInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ket_xuat_file")]
        public ActionResult ExportData(ApplicationHeaderInfo pInfo, AppDetail04NHInfo pDetail, List<AppDocumentInfo> pAppDocumentInfo,
         List<AppDocumentOthersInfo> pAppDocOtherInfo, List<AppClassDetailInfo> pAppClassInfo)
        {
            try
            {
                string language = AppsCommon.GetCurrentLang();
                AppInfoExport appInfo = new AppInfoExport();
                string actionView = pInfo.ActionView;
                if (actionView == "V")
                {
                    if (pInfo.Appcode == TradeMarkAppCode.AppCodeDangKynhanHieu)
                    {
                        var objBL = new AppDetail04NHBL();
                        var ds04NH = objBL.AppTM04NHGetByID(pInfo.Id, language, (int)pInfo.Status);
                        if (ds04NH != null && ds04NH.Tables.Count == 5)
                        {
                            pDetail = CBO<AppDetail04NHInfo>.FillObjectFromDataTable(ds04NH.Tables[0]);
                            pInfo = (ApplicationHeaderInfo)pDetail;
                            pAppDocumentInfo = CBO<AppDocumentInfo>.FillCollectionFromDataTable(ds04NH.Tables[1]);
                            pAppDocOtherInfo = CBO<AppDocumentOthersInfo>.FillCollectionFromDataTable(ds04NH.Tables[2]);
                            pAppClassInfo = CBO<AppClassDetailInfo>.FillCollectionFromDataTable(ds04NH.Tables[3]);

                            ViewBag.lstFeeInfo = CBO<AppFeeFixInfo>.FillCollectionFromDataTable(ds04NH.Tables[4]);
                        }
                    }
                }
                //   string _fileTemp = System.Web.HttpContext.Current.Server.MapPath("/Content/AppForms/TM04Nh_vi.doc");
                // Fill export_header
                string _datetimenow = DateTime.Now.ToString("ddMMyyyyHHmm");
                string fileName = "";
                if (language == Language.LangVI)
                {
                    fileName = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "Request_for_trademark_registration_vi_exp_" + pInfo.Appcode + _datetimenow + ".pdf");
                    SessionData.CurrentUser.FilePreview = "/Content/Export/" + "Request_for_trademark_registration_vi_exp_" + pInfo.Appcode + _datetimenow + ".pdf";
                }
                else
                {
                    fileName = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "Request_for_trademark_registration_en_exp_" + pInfo.Appcode + _datetimenow + ".pdf");
                    SessionData.CurrentUser.FilePreview = "/Content/Export/" + "Request_for_trademark_registration_en_exp_" + pInfo.Appcode + _datetimenow + ".pdf";
                }
                // Fill export_detail  
                appInfo.Status = 254;
                appInfo.Status_Form = 252;
                appInfo.Relationship = "11";
                appInfo = CreateInstance.CopyAppHeaderInfo(appInfo, pInfo);
                appInfo = CreateInstance.CopyAppDetailInfo(appInfo, pDetail);
                appInfo.DuadateExp = appInfo.Duadate.ToString("dd/MM/yyyy");
                appInfo.Ngaynopdon_UtExp = appInfo.Ngaynopdon_Ut.ToString("dd/MM/yyyy");
                if (string.IsNullOrEmpty(appInfo.Logourl))
                {
                    appInfo.Logourl = pDetail.LogourlOrg;
                }
                if (pDetail.pfileLogo != null)
                {
                    appInfo.Logourl = AppLoadHelpers.PushFileToServer(pDetail.pfileLogo, AppUpload.Logo);
                }

                foreach (var item in MemoryData.c_lst_Country)
                {
                    if (item.Country_Id.ToString() == appInfo.Nuocnopdon_Ut)
                    {
                        appInfo.Nuocnopdon_Ut_Display = item.Name;
                        break;
                    }
                }
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
                        appInfo.strListClass += Resources.Resource.lblNhom + item.Code.Substring(0, 2) + ": " + item.Textinput.Trim().Trim(',') + " (" + (item.IntTongSanPham < 10 ? "0" + item.IntTongSanPham.ToString() : item.IntTongSanPham.ToString()) + " " + Resources.Resource.lblSanPham + " )" + "\n";
                    }
                }
                if (lstDocOther != null)
                {
                    foreach (var item in lstDocOther)
                    {
                        appInfo.strDanhSachFileDinhKem += item.Documentname + " ; ";
                    }
                }
                #region  Hiển thị phí trong don
                var listfeeCaculator = new List<AppFeeFixInfo>();
                if (actionView == "V")
                {
                    listfeeCaculator = ViewBag.lstFeeInfo;
                }
                else
                {
                    int pPreview = 1;
                    string DonUT = pDetail.Sodon_Ut;
                    if (!string.IsNullOrEmpty(pDetail.Sodon_Ut2))
                    {
                        DonUT = DonUT + "," + pDetail.Sodon_Ut2;
                    }
                    int preturn = CaculatorFee(pAppClassInfo, DonUT, "", listfeeCaculator, pPreview);
                }
                foreach (var item in listfeeCaculator)
                {
                    if (item.Fee_Id == 200)
                    {
                        appInfo.TM04NH_200 = item.Number_Of_Patent;
                        appInfo.TM04NH_200_Val = item.Amount;
                    }
                    else if (item.Fee_Id == 201)
                    {
                        appInfo.TM04NH_201 = item.Number_Of_Patent;
                        appInfo.TM04NH_201_Val = item.Amount;
                    }
                    else if (item.Fee_Id == 2011)
                    {
                        appInfo.TM04NH_2011 = item.Number_Of_Patent;
                        appInfo.TM04NH_2011_Val = item.Amount;
                    }
                    else if (item.Fee_Id == 203)
                    {
                        appInfo.TM04NH_203 = item.Number_Of_Patent;
                        appInfo.TM04NH_203_Val = item.Amount;
                    }
                    else if (item.Fee_Id == 204)
                    {
                        appInfo.TM04NH_204 = item.Number_Of_Patent;
                        appInfo.TM04NH_204_Val = item.Amount;
                    }
                    else if (item.Fee_Id == 205)
                    {
                        appInfo.TM04NH_205 = item.Number_Of_Patent;
                        appInfo.TM04NH_205_Val = item.Amount;
                    }
                    else if (item.Fee_Id == 2051)
                    {
                        appInfo.TM04NH_2051 = item.Number_Of_Patent;
                        appInfo.TM04NH_2051_Val = item.Amount;
                    }
                    else if (item.Fee_Id == 207)
                    {
                        appInfo.TM04NH_207 = item.Number_Of_Patent;
                        appInfo.TM04NH_207_Val = item.Amount;
                    }
                    else if (item.Fee_Id == 2071)
                    {
                        appInfo.TM04NH_2071 = item.Number_Of_Patent;
                        appInfo.TM04NH_2071_Val = item.Amount;
                    }
                    appInfo.TM04NH_TOTAL = appInfo.TM04NH_TOTAL + item.Amount;
                }
                #endregion

                #region Tài liệu trong đơn 

                if (pAppDocumentInfo != null)
                {
                    foreach (AppDocumentInfo item in pAppDocumentInfo)
                    {
                        if (item.Document_Id == "04NH_D_04")
                        {
                            appInfo.TM_04NH_D_04_ISU = item.Isuse;
                            appInfo.TM_04NH_D_04_CHAR01 = CommonFuc.ConvertToString(item.CHAR01);
                        }
                        else if (item.Document_Id == "04NH_D_05")
                        {
                            appInfo.TM_04NH_D_05_ISU = item.Isuse;
                        }
                        else if (item.Document_Id == "04NH_D_06")
                        {
                            appInfo.TM_04NH_D_06_ISU = item.Isuse;
                        }
                        else if (item.Document_Id == "04NH_D_07")
                        {
                            appInfo.TM_04NH_D_07_ISU = item.Isuse;

                        }
                        else if (item.Document_Id == "04NH_D_08")
                        {
                            appInfo.TM_04NH_D_08_ISU = item.Isuse;
                            appInfo.TM_04NH_D_08_CHAR01 = CommonFuc.ConvertToString(item.CHAR01);
                        }
                        else if (item.Document_Id == "04NH_D_09")
                        {
                            appInfo.TM_04NH_D_09_ISU = item.Isuse;
                            appInfo.TM_04NH_D_09_CHAR01 = CommonFuc.ConvertToString(item.CHAR01);
                        }
                        else if (item.Document_Id == "04NH_D_10")
                        {
                            appInfo.TM_04NH_D_10_ISU = item.Isuse;
                            appInfo.TM_04NH_D_10_CHAR01 = CommonFuc.ConvertToString(item.CHAR01);
                        }
                        else if (item.Document_Id == "04NH_D_11")
                        {
                            appInfo.TM_04NH_D_11_ISU = item.Isuse;
                        }
                        else if (item.Document_Id == "04NH_D_12")
                        {
                            appInfo.TM_04NH_D_12_ISU = item.Isuse;
                        }
                        else if (item.Document_Id == "04NH_D_13")
                        {
                            appInfo.TM_04NH_D_13_ISU = item.Isuse;
                            appInfo.TM_04NH_D_13_CHAR01 = CommonFuc.ConvertToString(item.CHAR01);
                            appInfo.TM_04NH_D_13_CHAR02 = item.CHAR02;
                        }
                        else if (item.Document_Id == "04NH_D_14")
                        {
                            appInfo.TM_04NH_D_13_ISU = item.Isuse;
                            appInfo.TM_04NH_D_13_CHAR01 = CommonFuc.ConvertToString(item.CHAR01);
                        }
                        else if (item.Document_Id == "04NH_D_15")
                        {
                            appInfo.TM_04NH_D_13_ISU = item.Isuse;
                            appInfo.TM_04NH_D_13_CHAR01 = CommonFuc.ConvertToString(item.CHAR01);
                        }
                        else if (item.Document_Id == "04NH_D_16")
                        {
                            appInfo.TM_04NH_D_13_ISU = item.Isuse;
                            appInfo.TM_04NH_D_13_CHAR01 = CommonFuc.ConvertToString(item.CHAR01);
                        }
                        else if (item.Document_Id == "04NH_D_17")
                        {
                            appInfo.TM_04NH_D_13_ISU = item.Isuse;
                        }

                        else if (item.Document_Id == "04NH_D_18")
                        {
                            appInfo.TM_04NH_D_18_ISU = item.Isuse;

                        }
                        else if (item.Document_Id == "04NH_D_19")
                        {
                            appInfo.TM_04NH_D_19_ISU = item.Isuse;
                        }
                        else if (item.Document_Id == "04NH_D_20")
                        {
                            appInfo.TM_04NH_D_20_ISU = item.Isuse;
                        }
                        else if (item.Document_Id == "04NH_D_22")
                        {
                            appInfo.TM_04NH_D_22_ISU = item.Isuse;

                        }
                    }
                }
                #endregion
                appInfo.Logourl = Server.MapPath(appInfo.Logourl);
                //su dung cho TH ma DNSC 
                if (appInfo.Rep_Master_Type == "DDSH")
                {
                    appInfo.Extent_fld01 = appInfo.DDSHCN;
                }
                else
                {
                    appInfo.Extent_fld01 = appInfo.Rep_Master_Name;
                }
                //Su dung cho Nguoi shcn
                appInfo.Extent_fld02 = appInfo.MADDSHCN;
                if (!string.IsNullOrEmpty(appInfo.Appno))
                {
                    appInfo.Extent_fld03 = "DST"; //tach tu thang nao day
                }
                else
                {
                    appInfo.Extent_fld03 = "x";
                }

                List<AppInfoExport> _lst = new List<AppInfoExport>();
                _lst.Add(appInfo);
                DataSet _ds_all = ConvertData.ConvertToDataSet<AppInfoExport>(_lst, false);
                CrystalDecisions.CrystalReports.Engine.ReportDocument oRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                if (language == Language.LangVI)
                {
                    oRpt.Load(Path.Combine(Server.MapPath("~/Report/"), "TM_04NH.rpt"));
                }
                else
                {
                    oRpt.Load(Path.Combine(Server.MapPath("~/Report/"), "TM_04NH_EN.rpt"));
                }

                CrystalDecisions.CrystalReports.Engine.PictureObject _pic01;
                _pic01 = (CrystalDecisions.CrystalReports.Engine.PictureObject)oRpt.ReportDefinition.Sections[0].ReportObjects["Picture1"];
                _pic01.Width = 100;
                _pic01.Height = 100;
                try
                {
                    Bitmap img = new Bitmap(appInfo.Logourl);
                    try
                    {

                        double _Const = 6.666666666666;
                        int _left = 0, _top = 0, _marginleft = 225, _margintop = 4215;
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

                System.IO.FileInfo file = new System.IO.FileInfo(appInfo.Logourl);


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
                System.IO.File.WriteAllBytes(fileName, byteArray.ToArray()); // Requires System.Linq


                return Json(new { success = 0 });

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = 0 });
            }
        }



        //[HttpPost]
        [Route("Pre-View")]
        public ActionResult PreViewApplication()
        {
            try
            {
                //string tm04Nh = "TM04NH";
                ViewBag.FileName = SessionData.CurrentUser.FilePreview; // = "/Content/Export/" + "Request_for_trademark_registration_vi_exp_" + tm04Nh + ".pdf";
                                                                        //return PartialView("~/Areas/TradeMark/Views/Shared/_PartialContentPreview_docx.cshtml");
                return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistration/_PartialContentPreview.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistration/_PartialContentPreview.cshtml");
            }
        }

        #region Sua don luu tam
        /// <summary>
        /// ID:ID của app_header_id 
        /// ID2: là appcode 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("request-for-trade-mark-edit/{id}/{id1}/{id2}")]
        public ActionResult TradeMarkForEdit()
        {
            decimal App_Header_Id = 0;
            string AppCode = "";
            int Status = 0;
            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("/");
                SessionData.CurrentUser.chashFile.Clear();
                SessionData.CurrentUser.chashFileOther.Clear();

                if (RouteData.Values.ContainsKey("id"))
                {
                    App_Header_Id = CommonFuc.ConvertToDecimal(RouteData.Values["id"]);
                }
                if (RouteData.Values.ContainsKey("id1"))
                {
                    Status = CommonFuc.ConvertToInt(RouteData.Values["id1"]);
                }
                if (RouteData.Values.ContainsKey("id2"))
                {
                    AppCode = RouteData.Values["id2"].ToString().ToUpper();
                }

                if (AppCode == TradeMarkAppCode.AppCodeDangKynhanHieu)
                {
                    return TradeMarkSuaDon(App_Header_Id, AppCode, Status);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return TradeMarkSuaDon(App_Header_Id, AppCode, Status);
        }

        [HttpGet]
        [Route("request-for-trade-mark-translate/{id}/{id1}/{id2}/{id3}")]
        public ActionResult TradeMarkForTranslate()
        {
            decimal App_Header_Id = 0;
            decimal ID_VI = 0;
            string AppCode = "";
            int Status = 0;
            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("/");
                SessionData.CurrentUser.chashFile.Clear();
                SessionData.CurrentUser.chashFileOther.Clear();

                if (RouteData.Values.ContainsKey("id"))
                {
                    App_Header_Id = CommonFuc.ConvertToDecimal(RouteData.Values["id"]);
                }
                if (RouteData.Values.ContainsKey("id1"))
                {
                    Status = CommonFuc.ConvertToInt(RouteData.Values["id1"]);
                }
                if (RouteData.Values.ContainsKey("id2"))
                {
                    AppCode = RouteData.Values["id2"].ToString().ToUpper();
                }
                if (RouteData.Values.ContainsKey("id3"))
                {
                    ID_VI  = CommonFuc.ConvertToDecimal(RouteData.Values["id3"]); 
                }
                if (AppCode == TradeMarkAppCode.AppCodeDangKynhanHieu)
                {
                    return TradeMarkSuaDon(App_Header_Id, AppCode, Status, 1, ID_VI);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return TradeMarkSuaDon(App_Header_Id, AppCode, Status, 1);
        }

        [HttpPost]
        [Route("dich-dang-ky-nhan-hieu")]
        public ActionResult AppDonDangKyTranslate(ApplicationHeaderInfo pInfo, AppDetail04NHInfo pDetail, List<AppDocumentInfo> pAppDocumentInfo,
           List<AppDocumentOthersInfo> pAppDocOtherInfo, List<AppClassDetailInfo> pAppClassInfo, List<AppFeeFixInfo> pFeeFixInfo, string listIDDocRemove)
        {
            try
            {
                if (pDetail.LoaiNhanHieu == "undefined") pDetail.LoaiNhanHieu = "";
                Application_Header_BL objBL = new Application_Header_BL();
                AppFeeFixBL objFeeFixBL = new AppFeeFixBL();
                AppDetail04NHBL objDetail = new AppDetail04NHBL();
                AppClassDetailBL objClassDetail = new AppClassDetailBL();
                AppDocumentBL objDoc = new AppDocumentBL();
                if (pInfo == null || pDetail == null) return Json(new { status = ErrorCode.Error });
                string language = Language.LangVI;
                var CreatedBy = SessionData.CurrentUser.Username;
                var CreatedDate = SessionData.CurrentUser.CurrentDate;
                int pReturn = ErrorCode.Success;
                int pAppHeaderID = 0;
                decimal pIDHeaderEng = 0;
                pIDHeaderEng = pInfo.Id;
                using (var scope = new TransactionScope())
                {
                    //string 
                    string prefCaseCode = "";
                    pInfo.Languague_Code = language;
                    //Có rồi thì update ko thì insert 
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
                   

                    if (pAppHeaderID >= 0)
                    {
                        if (pAppHeaderID > 0)
                        {
                            pInfo.Id = pAppHeaderID;
                        }
                        else
                        {
                            pInfo.Id = pInfo.Id_Vi;
                        }
                        pDetail.Appcode = pInfo.Appcode;
                        pDetail.Language_Code = language;
                        pDetail.App_Header_Id = pInfo.Id;
                        pDetail.Logourl = pDetail.LogourlOrg;
                        pReturn = objDetail.App_Detail_04NH_Insert(pDetail);
                        //Thêm thông tin class
                        if (pReturn >= 0 && pAppClassInfo != null)
                        {

                            //Xoa cac class cu di 
                            pReturn = objClassDetail.AppClassDetailDeleted(pInfo.Id, language);

                            pReturn = objClassDetail.AppClassDetailInsertBatch(pAppClassInfo, pInfo.Id, language);
                        }
                    }
                    else
                    {
                        return Json(new { status = pAppHeaderID });
                    }
                    //tài liệu đính kèm
                    if (pReturn >= 0 && pAppDocumentInfo != null)
                    {
                        if (pAppDocumentInfo.Count > 0)
                        {
                            pReturn = objDoc.AppDocumentTranslate(Language.LangEN, pIDHeaderEng, pInfo.Id);
                        }
                    }

                    //tai lieu khac 
                    if (pReturn >= 0 && pAppDocOtherInfo != null)
                    {
                        if (pAppDocOtherInfo.Count > 0)
                        {
                            var listDocument = new List<AppDocumentOthersInfo>();
                            int check = 0;
                            foreach (var info in pAppDocOtherInfo)
                            {
                                if (!string.IsNullOrEmpty(info.Documentname))
                                {
                                    check = 1;
                                    info.App_Header_Id = pInfo.Id;
                                    info.Language_Code = language;
                                    info.IdRef =CommonFuc.ConvertToDecimal(info.keyFileUpload);
                                    listDocument.Add(info);
                                }
                            }
                            if (check == 1)
                            {
                                if (pInfo.Id_Vi > 0)
                                {
                                    pReturn = objDoc.AppDocumentOtherDeletedByApp(pInfo.Id_Vi, Language.LangVI);
                                }
                                pReturn = objDoc.AppDocumentOtherInsertBatch(listDocument);
                            }
                        }
                    }
                    //end
                    if (pReturn < 0)
                    {
                        Transaction.Current.Rollback();
                        return Json(new { status = pReturn });
                    }
                    else
                    {
                        //Lấy lại thông tin kế thừa đưa lên memory
                        MemoryData.Enqueue_ChangeData(Table_Change.APPHEADER);
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



        [HttpGet]
        [Route("request-for-trade-mark-view/{id}/{id1}/{id2}")]
        public ActionResult TradeMarkForView()
        {
            int Status = 0;
            decimal App_Header_Id = 0;
            string AppCode = "";
            ViewBag.Isdisable = 1;
            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("/");
                SessionData.CurrentUser.chashFile.Clear();
                SessionData.CurrentUser.chashFileOther.Clear();

                if (RouteData.Values.ContainsKey("id"))
                {
                    App_Header_Id = CommonFuc.ConvertToDecimal(RouteData.Values["id"]);
                }
                if (RouteData.Values.ContainsKey("id1"))
                {
                    Status = CommonFuc.ConvertToInt(RouteData.Values["id1"]);
                }
                if (RouteData.Values.ContainsKey("id2"))
                {
                    AppCode = RouteData.Values["id2"].ToString().ToUpper();
                }

                if (AppCode == TradeMarkAppCode.AppCodeDangKynhanHieu)
                {
                    return TradeMarkView(App_Header_Id, AppCode, Status);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return TradeMarkView(App_Header_Id, AppCode, Status);
        }

        public ActionResult TradeMarkView(decimal pAppHeaderId, string pAppCode, int pStatus)
        {
            if (pAppCode == TradeMarkAppCode.AppCodeDangKynhanHieu)
            {
                var objBL = new AppDetail04NHBL();
                string language = AppsCommon.GetCurrentLang();
                var ds04NH = objBL.AppTM04NHGetByID(pAppHeaderId, language, pStatus);
                if (ds04NH != null && ds04NH.Tables.Count == 5)
                {
                    ViewBag.objAppHeaderInfo = CBO<AppDetail04NHInfo>.FillObjectFromDataTable(ds04NH.Tables[0]);
                    ViewBag.lstDocumentInfo = CBO<AppDocumentInfo>.FillCollectionFromDataTable(ds04NH.Tables[1]);
                    ViewBag.lstDocOther = CBO<AppDocumentOthersInfo>.FillCollectionFromDataTable(ds04NH.Tables[2]);
                    ViewBag.lstClassDetailInfo = CBO<AppClassDetailInfo>.FillCollectionFromDataTable(ds04NH.Tables[3]);
                    ViewBag.lstFeeInfo = CBO<AppFeeFixInfo>.FillCollectionFromDataTable(ds04NH.Tables[4]);
                }
                return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistration/View_PartialDangKyNhanHieu.cshtml");
            }
            else if (pAppCode == TradeMarkAppCode.AppCode_TM_3B_PLB_01_SDD)
            {
                App_Detail_PLB01_SDD_BL objBL = new App_Detail_PLB01_SDD_BL();
                string language = AppsCommon.GetCurrentLang();
                List<AppDocumentInfo> appDocumentInfos = new List<AppDocumentInfo>();
                List<AppFeeFixInfo> appFeeFixInfos = new List<AppFeeFixInfo>();
                ApplicationHeaderInfo applicationHeaderInfo = new ApplicationHeaderInfo();
                App_Detail_PLB01_SDD_Info app_Detail = objBL.GetByID(pAppHeaderId, language, ref applicationHeaderInfo, ref appDocumentInfos, ref appFeeFixInfos);
                ViewBag.App_Detail = app_Detail;
                ViewBag.Lst_AppDoc = appDocumentInfos;
                ViewBag.Lst_AppFee = appFeeFixInfos;
                ViewBag.objAppHeaderInfo = applicationHeaderInfo;

                return PartialView("~/Areas/TradeMark/Views/PLB01_SDD_3B/_Partial_TM_3B_PLB_01_SDD_View.cshtml");
            }
            else if (pAppCode == TradeMarkAppCode.AppCode_TM_3C_PLB_02_CGD)
            {
                App_Detail_Plb02_CGD_BL objBL = new App_Detail_Plb02_CGD_BL();
                string language = AppsCommon.GetCurrentLang();
                List<AppDocumentInfo> appDocumentInfos = new List<AppDocumentInfo>();
                List<AppFeeFixInfo> appFeeFixInfos = new List<AppFeeFixInfo>();
                ApplicationHeaderInfo applicationHeaderInfo = new ApplicationHeaderInfo();
                App_Detail_PLB02_CGD_Info app_Detail = objBL.GetByID(pAppHeaderId, language, ref applicationHeaderInfo, ref appDocumentInfos, ref appFeeFixInfos);
                ViewBag.App_Detail = app_Detail;
                ViewBag.Lst_AppDoc = appDocumentInfos;
                ViewBag.Lst_AppFee = appFeeFixInfos;
                ViewBag.objAppHeaderInfo = applicationHeaderInfo;

                return PartialView("~/Areas/TradeMark/Views/PLB02_CGD_3C/_Partial_TM_3C_PLB_02_SDD_View.cshtml");
            }
            else if (pAppCode == TradeMarkAppCode.AppCode_TM_3D_PLC_05_KN)
            {
                App_Detail_PLC05_KN_BL objBL = new App_Detail_PLC05_KN_BL();
                string language = AppsCommon.GetCurrentLang();
                List<AppDocumentInfo> appDocumentInfos = new List<AppDocumentInfo>();
                List<AppFeeFixInfo> appFeeFixInfos = new List<AppFeeFixInfo>();
                ApplicationHeaderInfo applicationHeaderInfo = new ApplicationHeaderInfo();
                App_Detail_PLC05_KN_Info app_Detail = objBL.GetByID(pAppHeaderId, language, ref applicationHeaderInfo, ref appDocumentInfos, ref appFeeFixInfos);
                ViewBag.App_Detail = app_Detail;
                ViewBag.Lst_AppDoc = appDocumentInfos;
                ViewBag.Lst_AppFee = appFeeFixInfos;
                ViewBag.objAppHeaderInfo = applicationHeaderInfo;

                return PartialView("~/Areas/TradeMark/Views/PLC05_KN_3D/_Partial_TM_3D_PLC_05_KN_View.cshtml");
            }

            else if (pAppCode == TradeMarkAppCode.AppCode_TM_4C2_PLD_01_HDCN)
            {
                App_Detail_PLD01_HDCN_BL objBL = new App_Detail_PLD01_HDCN_BL();
                string language = AppsCommon.GetCurrentLang();
                List<AppDocumentInfo> appDocumentInfos = new List<AppDocumentInfo>();
                List<AppFeeFixInfo> appFeeFixInfos = new List<AppFeeFixInfo>();
                ApplicationHeaderInfo applicationHeaderInfo = new ApplicationHeaderInfo();
                App_Detail_PLD01_HDCN_Info app_Detail = objBL.GetByID(pAppHeaderId, language, ref applicationHeaderInfo, ref appDocumentInfos, ref appFeeFixInfos);
                ViewBag.App_Detail = app_Detail;
                ViewBag.Lst_AppDoc = appDocumentInfos;
                ViewBag.Lst_AppFee = appFeeFixInfos;
                ViewBag.objAppHeaderInfo = applicationHeaderInfo;

                return PartialView("~/Areas/TradeMark/Views/PLD01_HDCN_4C2/_Partial_TM_4C2_PLD_01_HDCN_View.cshtml");
            }
            else if (pAppCode == TradeMarkAppCode.AppCodeDangKyQuocTeNH)
            {
                var objBL = new AppDetail06DKQT_BL();
                string language = AppsCommon.GetCurrentLang();
                var ds06Dkqt = objBL.AppTM06DKQTGetByID(pAppHeaderId, language, pStatus);
                if (ds06Dkqt != null && ds06Dkqt.Tables.Count == 3)
                {
                    ViewBag.objAppHeaderInfo = CBO<App_Detail_TM06DKQT_Info>.FillObjectFromDataTable(ds06Dkqt.Tables[0]);
                    ViewBag.Lst_AppDoc = CBO<AppDocumentInfo>.FillCollectionFromDataTable(ds06Dkqt.Tables[1]);
                    ViewBag.lstClassDetailInfo = CBO<AppClassDetailInfo>.FillCollectionFromDataTable(ds06Dkqt.Tables[2]);
                }
                return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistration01/_PartalViewDangKyNhanHieu.cshtml");
            }
            else
            {
                return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistration/View_PartialDangKyNhanHieu.cshtml");
            }
        }

        public ActionResult TradeMarkSuaDon(decimal pAppHeaderId, string pAppCode, int pStatus, int pEditOrTranslate = 0, decimal pIDVi=0)
        {
            if (pAppCode == TradeMarkAppCode.AppCodeDangKynhanHieu)
            {
                var objBL = new AppDetail04NHBL();
                string language = AppsCommon.GetCurrentLang();
                if (pEditOrTranslate == 1)
                {
                    language = Language.LangEN;
                    //Nếu là dịch lấy cả bản ghi tiếng việt lên
                    var ds04NHVI = objBL.AppTM04NHGetByID(pIDVi, Language.LangVI, pStatus);
                    string keyDataVI = "objAppHeaderInfo" + SessionData.CurrentUser.Id.ToString() + DateTime.Now.ToString("DDMMHHmmss");
                    if (ds04NHVI != null && ds04NHVI.Tables.Count == 5)
                    {
                        ViewBag.objAppHeaderInfo_VI = CBO<AppDetail04NHInfo>.FillObjectFromDataTable(ds04NHVI.Tables[0]);
                        ViewBag.lstDocumentInfo_VI = CBO<AppDocumentInfo>.FillCollectionFromDataTable(ds04NHVI.Tables[1]);
                        ViewBag.lstDocOther_VI = CBO<AppDocumentOthersInfo>.FillCollectionFromDataTable(ds04NHVI.Tables[2]);
                        ViewBag.lstClassDetailInfo_VI = CBO<AppClassDetailInfo>.FillCollectionFromDataTable(ds04NHVI.Tables[3]);
                        ViewBag.lstFeeInfo_VI = CBO<AppFeeFixInfo>.FillCollectionFromDataTable(ds04NHVI.Tables[4]);
                    }


                }
                var ds04NH = objBL.AppTM04NHGetByID(pAppHeaderId, language, pStatus);
                string keyData = "objAppHeaderInfo" + SessionData.CurrentUser.Id.ToString() + DateTime.Now.ToString("DDMMHHmmss");
                //Luu key duy nhat cua he thong
                ViewBag.keyData = keyData;
                SessionData.SetDataSession(keyData, "");
                if (ds04NH != null && ds04NH.Tables.Count == 5)
                {
                    ViewBag.objAppHeaderInfo = CBO<AppDetail04NHInfo>.FillObjectFromDataTable(ds04NH.Tables[0]);
                    SessionData.SetDataSession(keyData, ViewBag.objAppHeaderInfo);
                    ViewBag.lstDocumentInfo = CBO<AppDocumentInfo>.FillCollectionFromDataTable(ds04NH.Tables[1]);
                    ViewBag.lstDocOther = CBO<AppDocumentOthersInfo>.FillCollectionFromDataTable(ds04NH.Tables[2]);
                    lstDocOther = ViewBag.lstDocOther;
                    ViewBag.lstClassDetailInfo = CBO<AppClassDetailInfo>.FillCollectionFromDataTable(ds04NH.Tables[3]);
                    ViewBag.lstFeeInfo = CBO<AppFeeFixInfo>.FillCollectionFromDataTable(ds04NH.Tables[4]);
                }
                if (pEditOrTranslate == 1)
                {
                    return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistration/Translate_PartialDangKyNhanHieu.cshtml");
                }
                else
                {
                    return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistration/Edit_PartialDangKyNhanHieu.cshtml");
                }
            }
            else if (pAppCode == TradeMarkAppCode.AppCode_TM_3B_PLB_01_SDD)
            {
                App_Detail_PLB01_SDD_BL objBL = new App_Detail_PLB01_SDD_BL();
                string language = AppsCommon.GetCurrentLang();
                List<AppDocumentInfo> appDocumentInfos = new List<AppDocumentInfo>();
                List<AppFeeFixInfo> appFeeFixInfos = new List<AppFeeFixInfo>();
                ApplicationHeaderInfo applicationHeaderInfo = new ApplicationHeaderInfo();
                App_Detail_PLB01_SDD_Info app_Detail = objBL.GetByID(pAppHeaderId, language, ref applicationHeaderInfo, ref appDocumentInfos, ref appFeeFixInfos);
                ViewBag.App_Detail = app_Detail;
                ViewBag.Lst_AppDoc = appDocumentInfos;
                ViewBag.Lst_AppFee = appFeeFixInfos;
                ViewBag.objAppHeaderInfo = applicationHeaderInfo;

                return PartialView("~/Areas/TradeMark/Views/PLB01_SDD_3B/_Partial_TM_3B_PLB_01_SDD_Edit.cshtml");
            }
            else if (pAppCode == TradeMarkAppCode.AppCode_TM_3C_PLB_02_CGD)
            {
                App_Detail_Plb02_CGD_BL objBL = new App_Detail_Plb02_CGD_BL();
                string language = AppsCommon.GetCurrentLang();
                List<AppDocumentInfo> appDocumentInfos = new List<AppDocumentInfo>();
                List<AppFeeFixInfo> appFeeFixInfos = new List<AppFeeFixInfo>();
                ApplicationHeaderInfo applicationHeaderInfo = new ApplicationHeaderInfo();
                App_Detail_PLB02_CGD_Info app_Detail = objBL.GetByID(pAppHeaderId, language, ref applicationHeaderInfo, ref appDocumentInfos, ref appFeeFixInfos);
                ViewBag.App_Detail = app_Detail;
                ViewBag.Lst_AppDoc = appDocumentInfos;
                ViewBag.Lst_AppFee = appFeeFixInfos;
                ViewBag.objAppHeaderInfo = applicationHeaderInfo;

                return PartialView("~/Areas/TradeMark/Views/PLB02_CGD_3C/_Partial_TM_3C_PLB_02_SDD_Edit.cshtml");
            }
            else if (pAppCode == TradeMarkAppCode.AppCode_TM_3D_PLC_05_KN)
            {
                App_Detail_PLC05_KN_BL objBL = new App_Detail_PLC05_KN_BL();
                string language = AppsCommon.GetCurrentLang();
                List<AppDocumentInfo> appDocumentInfos = new List<AppDocumentInfo>();
                List<AppFeeFixInfo> appFeeFixInfos = new List<AppFeeFixInfo>();
                ApplicationHeaderInfo applicationHeaderInfo = new ApplicationHeaderInfo();
                App_Detail_PLC05_KN_Info app_Detail = objBL.GetByID(pAppHeaderId, language, ref applicationHeaderInfo, ref appDocumentInfos, ref appFeeFixInfos);
                ViewBag.App_Detail = app_Detail;
                ViewBag.Lst_AppDoc = appDocumentInfos;
                ViewBag.Lst_AppFee = appFeeFixInfos;
                ViewBag.objAppHeaderInfo = applicationHeaderInfo;

                return PartialView("~/Areas/TradeMark/Views/PLC05_KN_3D/_Partial_TM_3D_PLC_05_KN_Edit.cshtml");
            }

            else if (pAppCode == TradeMarkAppCode.AppCodeDangKyQuocTeNH)
            {
                var objBL = new AppDetail06DKQT_BL();
                string language = AppsCommon.GetCurrentLang();
                var ds06Dkqt = objBL.AppTM06DKQTGetByID(pAppHeaderId, language, pStatus);
                if (ds06Dkqt != null && ds06Dkqt.Tables.Count == 3)
                {
                    ViewBag.objAppHeaderInfo = CBO<App_Detail_TM06DKQT_Info>.FillObjectFromDataTable(ds06Dkqt.Tables[0]);
                    ViewBag.Lst_AppDoc = CBO<AppDocumentInfo>.FillCollectionFromDataTable(ds06Dkqt.Tables[1]);
                    ViewBag.lstClassDetailInfo = CBO<AppClassDetailInfo>.FillCollectionFromDataTable(ds06Dkqt.Tables[2]);
                }
                if (ds06Dkqt.Tables[0] == null || ds06Dkqt.Tables[0].Rows.Count == 0)
                {
                    return Redirect("/trade-mark/request-for-trade-mark-view/" + pAppHeaderId.ToString() + "/" + pStatus.ToString() + "/" + TradeMarkAppCode.AppCodeDangKyQuocTeNH);
                }
                AppDetail04NHBL _AppDetail04NHBL = new AppDetail04NHBL();
                List<AppDetail04NHInfo> _list04nh = new List<AppDetail04NHInfo>();
                // truyền vào trạng thái nào? để tạm thời = 7 là đã gửi lên cục
                _list04nh = _AppDetail04NHBL.AppTM04NHSearchByStatus(7, language);
                ViewBag.ListAppDetail04NHInfo = _list04nh;
                return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistration01/_PartialEditDangKyNhanHieu.cshtml");
            }
            else if (pAppCode == TradeMarkAppCode.AppCode_TM_4C2_PLD_01_HDCN)
            {
                App_Detail_PLD01_HDCN_BL objBL = new App_Detail_PLD01_HDCN_BL();
                string language = AppsCommon.GetCurrentLang();
                List<AppDocumentInfo> appDocumentInfos = new List<AppDocumentInfo>();
                List<AppFeeFixInfo> appFeeFixInfos = new List<AppFeeFixInfo>();
                ApplicationHeaderInfo applicationHeaderInfo = new ApplicationHeaderInfo();
                App_Detail_PLD01_HDCN_Info app_Detail = objBL.GetByID(pAppHeaderId, language, ref applicationHeaderInfo, ref appDocumentInfos, ref appFeeFixInfos);
                ViewBag.App_Detail = app_Detail;
                ViewBag.Lst_AppDoc = appDocumentInfos;
                ViewBag.Lst_AppFee = appFeeFixInfos;
                ViewBag.objAppHeaderInfo = applicationHeaderInfo;

                return PartialView("~/Areas/TradeMark/Views/PLD01_HDCN_4C2/_Partial_TM_4C2_PLD_01_HDCN_Edit.cshtml");

            }
            else
            {
                //
                return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistration/Edit_PartialDangKyNhanHieu.cshtml");
            }
        }

        [HttpPost]
        [Route("request-for-trade-mark-del")]
        public ActionResult TradeMarkForDel(decimal pAppHeaderID, string pAppCode)
        {

            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("/");
                SessionData.CurrentUser.chashFile.Clear();
                SessionData.CurrentUser.chashFileOther.Clear();
                var objBL = new Application_Header_BL();
                string language = AppsCommon.GetCurrentLang();
                var response = objBL.AppHeaderDeleted(pAppHeaderID, language);
                return Json(new { status = response });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { status = ErrorCode.Error });
            }
        }
        #endregion

        [HttpPost]
        [Route("dang-ky-nhan-hieu-sua-doi")]
        public ActionResult AppDonDangKyEditAppro(ApplicationHeaderInfo pInfo, AppDetail04NHInfo pDetail, List<AppDocumentInfo> pAppDocumentInfo,
            List<AppDocumentOthersInfo> pAppDocOtherInfo, List<AppClassDetailInfo> pAppClassInfo, List<AppFeeFixInfo> pFeeFixInfo, string listIDDocRemove)
        {
            try
            {
                if (pDetail.LoaiNhanHieu == "undefined") pDetail.LoaiNhanHieu = "";
                Application_Header_BL objBL = new Application_Header_BL();
                AppFeeFixBL objFeeFixBL = new AppFeeFixBL();
                AppDetail04NHBL objDetail = new AppDetail04NHBL();
                AppClassDetailBL objClassDetail = new AppClassDetailBL();
                AppDocumentBL objDoc = new AppDocumentBL();
                if (pInfo == null || pDetail == null) return Json(new { status = ErrorCode.Error });
                string language = AppsCommon.GetCurrentLang();
                var CreatedBy = SessionData.CurrentUser.Username;
                var CreatedDate = SessionData.CurrentUser.CurrentDate;
                int pReturn = ErrorCode.Success;
                int pAppHeaderID = 0;
                using (var scope = new TransactionScope())
                {
                    //
                    pInfo.Languague_Code = language;
                    pInfo.Modify_By = CreatedBy;
                    pInfo.Modify_Date = CreatedDate;
                    //TRA RA ID CUA BANG KHI INSERT
                    pAppHeaderID = objBL.AppHeaderUpdate(pInfo);
                    if (pAppHeaderID >= 0)
                    {
                        pDetail.Appcode = pInfo.Appcode;
                        pDetail.Language_Code = language;
                        pDetail.App_Header_Id = pInfo.Id;
                        if (pDetail.pfileLogo != null)
                        {
                            pDetail.Logourl = AppLoadHelpers.PushFileToServer(pDetail.pfileLogo, AppUpload.Logo);
                        }
                        else
                        {
                            pDetail.Logourl = pDetail.LogourlOrg;
                        }
                        pReturn = objDetail.App_Detail_04NH_Update(pDetail);
                        //Thêm thông tin class
                        if (pReturn >= 0 && pAppClassInfo != null)
                        {

                            //Xoa cac class cu di 
                            pReturn = objClassDetail.AppClassDetailDeleted(pInfo.Id, language);

                            pReturn = objClassDetail.AppClassDetailInsertBatch(pAppClassInfo, pInfo.Id, language);
                        }

                        if (pReturn >= 0 && pAppClassInfo != null)
                        {
                            //pReturn = _AppFeeFixBL.AppFeeFixInsertBath(pFeeFixInfo, pInfo.Id);

                            //if (pReturn >= 0 && pAppClassInfo != null)
                            //{
                            var listfeeCaculator = new List<AppFeeFixInfo>();
                            pReturn = CaculatorFee(pAppClassInfo, pDetail.Sodon_Ut, pInfo.Case_Code, listfeeCaculator);
                            //}
                        }
                    }
                    else
                    {
                        return Json(new { status = pAppHeaderID });
                    }
                    //tài liệu đính kèm
                    if (pReturn >= 0 && pAppDocumentInfo != null)
                    {
                        if (pAppDocumentInfo.Count > 0)
                        {
                            // Get ra để map sau đó xóa đi để insert vào sau
                            AppDocumentBL _AppDocumentBL = new AppDocumentBL();
                            List<AppDocumentInfo> Lst_AppDoc = _AppDocumentBL.AppDocument_Getby_AppHeader(pInfo.Id, language);
                            Dictionary<string, AppDocumentInfo> dic_appDoc = new Dictionary<string, AppDocumentInfo>();
                            foreach (AppDocumentInfo item in Lst_AppDoc)
                            {
                                dic_appDoc[item.Document_Id] = item;
                            }
                            // xóa đi trước
                            _AppDocumentBL.AppDocumentDelByApp(pInfo.Id, language);
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

                    if (!string.IsNullOrEmpty(listIDDocRemove))
                    {
                        string[] arrayIDDoc = listIDDocRemove.Split('|');
                        for (int i = 0; i < arrayIDDoc.Length; i++)
                        {
                            decimal idDOc = CommonFuc.ConvertToDecimal(arrayIDDoc[i]);
                            pReturn = objDoc.AppDocumentDelByID(idDOc, language, pInfo.Id);
                        }
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
                                info.App_Header_Id = pInfo.Id;
                                info.Language_Code = language;

                            }
                            if (check == 1)
                            {
                                pReturn = objDoc.AppDocumentOtherInsertBatch(pAppDocOtherInfo);
                            }
                        }
                    }

                    //Xóa các tài liệu đi khi sửa bản ghi 
                    if (pReturn >= 0 && !string.IsNullOrEmpty(pDetail.ListFileAttachOtherDel))
                    {
                        var arrIdFileAttack = pDetail.ListFileAttachOtherDel.Split(',');
                        foreach (var item in arrIdFileAttack)
                        {
                            decimal pID = CommonFuc.ConvertToDecimal(item);
                            pReturn = objDoc.AppDocOtherByID(pID, language);
                        }
                    }

                    //end
                    if (pReturn < 0)
                    {

                        Transaction.Current.Rollback();
                        return Json(new { status = pReturn });
                    }
                    else
                    {
                        //Lấy lại thông tin kế thừa đưa lên memory
                        MemoryData.Enqueue_ChangeData(Table_Change.APPHEADER);
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

        public int CaculatorFee(List<AppClassDetailInfo> pAppClassInfo, string NumberAppNo, string p_case_code, List<AppFeeFixInfo> _lstFeeFix, int pPrviewOrInsert = 0)
        {
            try
            {
                if (NumberAppNo == null)
                {
                    NumberAppNo = "";
                }
                string _keyFee = "";
                int TongSoNhom = 1;
                int SoDongTinhQua = 0;
                int TongSoTinhPhi = 0;
                _keyFee = TradeMarkAppCode.AppCodeDangKynhanHieu + "_2011";
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    SoDongTinhQua = CommonFuc.ConvertToInt(MemoryData.c_dic_FeeByApp_Fix[_keyFee].Char01);
                }
                else
                {
                    SoDongTinhQua = 6;
                }
                if (pAppClassInfo.Count > 0)
                {
                    TongSoNhom = CommonFuc.ConvertToInt(pAppClassInfo[0].TongSoNhom);
                    string[] arrSoSanPham = pAppClassInfo[0].TongSanPham.Split('|');
                    for (int i = 0; i < arrSoSanPham.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(arrSoSanPham[i]))
                        {
                            int TotalItemOnGroup = CommonFuc.ConvertToInt(arrSoSanPham[i]);
                            if (TotalItemOnGroup > SoDongTinhQua)
                            {
                                TongSoTinhPhi = TongSoTinhPhi + (TotalItemOnGroup - SoDongTinhQua);
                            }
                        }
                    }
                }
                if (TongSoTinhPhi < 1) TongSoTinhPhi = 1;
                AppFeeFixInfo _AppFeeFixInfo = new AppFeeFixInfo();

                #region Phí Nộp hồ sơ
                //_lstFeeFix = new List<AppFeeFixInfo>();
                //1.Phí nộp hồ sơ 
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Fee_Id = 200;
                _AppFeeFixInfo.Isuse = 1;
                _AppFeeFixInfo.Case_Code = p_case_code;
                _AppFeeFixInfo.Number_Of_Patent = 1; //default là 1 
                _keyFee = TradeMarkAppCode.AppCodeDangKynhanHieu + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                else
                    _AppFeeFixInfo.Amount = 150000 * _AppFeeFixInfo.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo);

                //2.TỔNG SỐ NHÓM 
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Fee_Id = 201;
                _AppFeeFixInfo.Isuse = 1;
                _AppFeeFixInfo.Case_Code = p_case_code;
                _AppFeeFixInfo.Number_Of_Patent = TongSoNhom;
                _keyFee = TradeMarkAppCode.AppCodeDangKynhanHieu + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                else
                    _AppFeeFixInfo.Amount = 200000 * _AppFeeFixInfo.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo);

                //3.Tổng số sản phẩm tren nhom 
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Fee_Id = 2011;
                _AppFeeFixInfo.Isuse = 1;
                _AppFeeFixInfo.Case_Code = p_case_code;
                _AppFeeFixInfo.Number_Of_Patent = TongSoTinhPhi;
                _keyFee = TradeMarkAppCode.AppCodeDangKynhanHieu + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                else
                    _AppFeeFixInfo.Amount = 22000 * _AppFeeFixInfo.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo);


                //4.Số đơn ưu tiên  pDetail.Used_Special
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Fee_Id = 203;
                _AppFeeFixInfo.Isuse = 0;
                if (!string.IsNullOrEmpty(NumberAppNo))
                    _AppFeeFixInfo.Isuse = 1;

                _AppFeeFixInfo.Case_Code = p_case_code;
                _AppFeeFixInfo.Number_Of_Patent = _AppFeeFixInfo.Isuse;
                _keyFee = TradeMarkAppCode.AppCodeDangKynhanHieu + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Isuse;
                else
                    _AppFeeFixInfo.Amount = 600000 * _AppFeeFixInfo.Isuse;
                _lstFeeFix.Add(_AppFeeFixInfo);


                //5.Lệ phí công bố đơn
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Fee_Id = 204;
                _AppFeeFixInfo.Isuse = 1;
                _AppFeeFixInfo.Case_Code = p_case_code;
                _AppFeeFixInfo.Number_Of_Patent = 1; //default là 1 
                _keyFee = TradeMarkAppCode.AppCodeDangKynhanHieu + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                else
                    _AppFeeFixInfo.Amount = 120000 * _AppFeeFixInfo.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo);

                //6. Phí tra cứu phục vụ thẩm định 
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Fee_Id = 205;
                _AppFeeFixInfo.Isuse = 1;
                _AppFeeFixInfo.Case_Code = p_case_code;
                _AppFeeFixInfo.Number_Of_Patent = TongSoNhom;
                _keyFee = TradeMarkAppCode.AppCodeDangKynhanHieu + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                else
                    _AppFeeFixInfo.Amount = 360000 * _AppFeeFixInfo.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo);


                //7.Tổng số sản phẩm tren nhom 
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Fee_Id = 2051;
                _AppFeeFixInfo.Isuse = 1;
                _AppFeeFixInfo.Case_Code = p_case_code;
                _AppFeeFixInfo.Number_Of_Patent = TongSoTinhPhi;
                _keyFee = TradeMarkAppCode.AppCodeDangKynhanHieu + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                else
                    _AppFeeFixInfo.Amount = 30000 * _AppFeeFixInfo.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo);



                //8.Phí thẩm định đơn
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Fee_Id = 207;
                _AppFeeFixInfo.Isuse = 1;
                _AppFeeFixInfo.Case_Code = p_case_code;
                _AppFeeFixInfo.Number_Of_Patent = 1;
                _keyFee = TradeMarkAppCode.AppCodeDangKynhanHieu + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                else
                    _AppFeeFixInfo.Amount = 550000 * _AppFeeFixInfo.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo);


                //9.Mỗi nhóm có trên 6 sản phẩm/dịch vụ (từ sản phẩm/dịch vụ thứ 7 trở đi)
                _AppFeeFixInfo = new AppFeeFixInfo();
                _AppFeeFixInfo.Fee_Id = 2071;
                _AppFeeFixInfo.Isuse = 1;
                _AppFeeFixInfo.Case_Code = p_case_code;
                _AppFeeFixInfo.Number_Of_Patent = TongSoTinhPhi;
                _keyFee = TradeMarkAppCode.AppCodeDangKynhanHieu + "_" + _AppFeeFixInfo.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    _AppFeeFixInfo.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo.Number_Of_Patent;
                else
                    _AppFeeFixInfo.Amount = 120000 * _AppFeeFixInfo.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo);

                //Xem trước privew thì ko làm gì cả chỉ tính đẩy vào list thôi 
                if (pPrviewOrInsert != 0)
                {
                    return 0;
                }

                AppFeeFixBL _AppFeeFixBL = new AppFeeFixBL();
                string language = AppsCommon.GetCurrentLang();
                _AppFeeFixBL.AppFeeFixDelete(p_case_code, language);
                return _AppFeeFixBL.AppFeeFixInsertBath(_lstFeeFix, p_case_code);

                #endregion

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -3;
            }
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
        [Route("delete-file-other")]
        public ActionResult DeleteFileOther(string keyFileUpload)
        {
            try
            {
                if (SessionData.CurrentUser.chashFileOther.ContainsKey(keyFileUpload))
                {
                    SessionData.CurrentUser.chashFileOther.Remove(keyFileUpload);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = -1 });
            }
            return Json(new { success = 0 });
        }
        [Route("GetNameSuggest")]
        public ActionResult GetNameSuggest(string pName)
        {
            try
            {
                List<CustomerSuggestInfo> lstContain = new List<CustomerSuggestInfo>();
                int check = 0;
                foreach (var item in MemoryData.lstCacheCustomer)
                {
                    if (string.IsNullOrEmpty(item.name)) continue;
                    if (item.name.ToLower().Contains(pName.ToLower()))
                    {
                        check = 1;
                        lstContain.Add(item);
                    }
                }
                if (check == 1)
                {
                    return Json(new { lst = lstContain });
                }
                else
                {
                    return Json(new { lst = MemoryData.lstCacheCustomer });
                }
            }
            catch (Exception ex)
            {

                Logger.LogException(ex);
                return Json(new { lst = MemoryData.lstCacheCustomer });
            }
        }

        [Route("GetNameSuggestKhac")]
        public ActionResult GetNameSuggestKhac(string pName, int pNumber)
        {
            try
            {
                List<CustomerSuggestInfo> lstContain = new List<CustomerSuggestInfo>();
                int check = 0;
                if (pNumber == 1)
                {
                    foreach (var item in MemoryData.lstCacheCustomer1)
                    {
                        if (string.IsNullOrEmpty(item.name)) continue;
                        if (item.name.ToLower().Contains(pName.ToLower()))
                        {
                            check = 1;
                            lstContain.Add(item);
                        }
                    }
                }
                else if (pNumber == 2)
                {
                    foreach (var item in MemoryData.lstCacheCustomer2)
                    {
                        if (string.IsNullOrEmpty(item.name)) continue;
                        if (item.name.ToLower().Contains(pName.ToLower()))
                        {
                            check = 1;
                            lstContain.Add(item);
                        }
                    }
                }
                else if (pNumber == 3)
                {
                    foreach (var item in MemoryData.lstCacheCustomer3)
                    {
                        if (string.IsNullOrEmpty(item.name)) continue;
                        if (item.name.ToLower().Contains(pName.ToLower()))
                        {
                            check = 1;
                            lstContain.Add(item);
                        }
                    }
                }
                else if (pNumber == 4)
                {
                    foreach (var item in MemoryData.lstCacheCustomer4)
                    {
                        if (string.IsNullOrEmpty(item.name)) continue;
                        if (item.name.ToLower().Contains(pName.ToLower()))
                        {
                            check = 1;
                            lstContain.Add(item);
                        }
                    }
                }
                else if (pNumber == 5)
                {
                    foreach (var item in MemoryData.lstChuDDSHCN)
                    {
                        if (string.IsNullOrEmpty(item.name)) continue;
                        if (item.name.ToLower().Contains(pName.ToLower()))
                        {
                            check = 1;
                            lstContain.Add(item);
                        }
                    }
                }
                if (check == 1)
                {
                    return Json(new { lst = lstContain });
                }
                else
                {
                    if (pNumber == 1)
                        return Json(new { lst = MemoryData.lstCacheCustomer1 });
                    else if (pNumber == 2)
                        return Json(new { lst = MemoryData.lstCacheCustomer2 });
                    else if (pNumber == 3)
                        return Json(new { lst = MemoryData.lstCacheCustomer3 });
                    else if (pNumber == 4)
                        return Json(new { lst = MemoryData.lstCacheCustomer4 });
                    else if (pNumber == 5)
                        return Json(new { lst = MemoryData.lstChuDDSHCN });
                    else return Json(new { lst = MemoryData.lstCacheCustomer });
                }
            }
            catch (Exception ex)
            {

                Logger.LogException(ex);
                if (pNumber == 1)
                    return Json(new { lst = MemoryData.lstCacheCustomer1 });
                else if (pNumber == 2)
                    return Json(new { lst = MemoryData.lstCacheCustomer2 });
                else if (pNumber == 3)
                    return Json(new { lst = MemoryData.lstCacheCustomer3 });
                else if (pNumber == 4)
                    return Json(new { lst = MemoryData.lstCacheCustomer4 });
                else if (pNumber == 5)
                    return Json(new { lst = MemoryData.lstChuDDSHCN });
                else return Json(new { lst = MemoryData.lstCacheCustomer });
            }
        }

        [Route("GetNameSuggestAppClass")]
        public ActionResult GetNameSuggestAppClass(string pName)
        {
            List<CustomerSuggestInfo> lstContain = new List<CustomerSuggestInfo>();
            try
            {

                int check = 0;
                foreach (var item in MemoryData.clstAppClassSuggest)
                {
                    if (string.IsNullOrEmpty(item.name)) continue;
                    if (item.name.ToLower().Contains(pName.ToLower()))
                    {
                        ++check;
                        lstContain.Add(item);
                        if (check > 50) break;
                    }
                }
                return Json(new { lst = lstContain });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { lst = lstContain });
            }
        }

        [HttpPost]
        [Route("funcGetTTDHSH")]
        public ActionResult GetThongTinDaiDien(string pKeyData, string pDDSH, int pEdit, decimal pID, int pStatus)
        {
            try
            {

                if (pEdit == 1)
                {
                    var obj = (AppDetail04NHInfo)SessionData.GetDataSession(pKeyData);
                    if (obj.Rep_Master_Type == pDDSH)
                    {
                        ViewBag.objAppHeaderInfo = obj;
                        ViewBag.MADDSH = obj.MADDSHCN;
                    }
                    else
                    {
                        ViewBag.objAppHeaderInfo = new AppDetail04NHInfo();
                    }
                    //var objBL = new AppDetail04NHBL();
                    //string language = AppsCommon.GetCurrentLang();
                    //var ds04NH = objBL.AppTM04NHGetByID(pID, language, pStatus);
                    //if (ds04NH != null && ds04NH.Tables.Count == 5)
                    //{
                    //    var obj  = CBO<AppDetail04NHInfo>.FillObjectFromDataTable(ds04NH.Tables[0]);
                    //    if(obj.Rep_Master_Type== pDDSH)
                    //    {
                    //        ViewBag.objAppHeaderInfo = obj;
                    //    }
                    //    else
                    //    {
                    //        ViewBag.objAppHeaderInfo = new AppDetail04NHInfo();
                    //    }

                    //}

                }
                return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistration/_Partial04NHChuDonKhacTop.cshtml", "2");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistration/_Partial04NHChuDonKhacTop.cshtml", "2");
            }
        }

    }
}
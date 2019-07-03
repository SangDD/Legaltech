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
                SessionData.CurrentUser.chashFileOther.Clear();
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
                            if (SessionData.CurrentUser.chashFileOther.ContainsKey(_keyfileupload))
                            {
                                var _updateitem = SessionData.CurrentUser.chashFileOther[info.keyFileUpload];
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
                                if (SessionData.CurrentUser.chashFileOther.ContainsKey(info.keyFileUpload))
                                {
                                    var _updateitem = SessionData.CurrentUser.chashFileOther[info.keyFileUpload];
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
                    List<AppFeeFixInfo> _lstFeeFix = CallFee(pDetail, pAppDocumentInfo, pUTienInfo, pAppDocIndusDesign);
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
                                    var _updateitem = SessionData.CurrentUser.chashFileOther[info.keyFileUpload];
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

        List<AppFeeFixInfo> CallFee(A03_Info pDetail, List<AppDocumentInfo> pAppDocumentInfo,
          List<UTienInfo> pUTienInfo, List<AppDocumentOthersInfo> pAppDocIndusDesign)
        {
            try
            {
                pDetail.Appcode = "A03";
                decimal _total_PhuongAn = pAppDocIndusDesign.Select(m => m.FILELEVEL == 1).Count();
                decimal _totalImage = pAppDocIndusDesign.Select(m => m.FILELEVEL == 2).Count();

                #region 1 Lệ phí nộp đơn
                List<AppFeeFixInfo> _lstFeeFix = new List<AppFeeFixInfo>();
                AppFeeFixInfo _AppFeeFixInfo1 = new AppFeeFixInfo();
                _AppFeeFixInfo1.Fee_Id = 1;
                _AppFeeFixInfo1.Isuse = 1;
                _AppFeeFixInfo1.Number_Of_Patent = 1;
                string _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo1.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo1.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo1.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo1.Amount_Usd;
                    _AppFeeFixInfo1.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo1.Number_Of_Patent;
                    _AppFeeFixInfo1.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent;
                    _AppFeeFixInfo1.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd;

                }
                else
                    _AppFeeFixInfo1.Amount = 150000 * _AppFeeFixInfo1.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo1);
                #endregion

                #region 2 Quyền ưu tiên
                AppFeeFixInfo _AppFeeFixInfo2 = new AppFeeFixInfo();
                _AppFeeFixInfo2.Fee_Id = 2;
                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo2.Fee_Id.ToString();

                if (pUTienInfo != null && pUTienInfo.Count > 0)
                {
                    _AppFeeFixInfo2.Isuse = 1;
                    _AppFeeFixInfo2.Number_Of_Patent = pUTienInfo.Count;
                }
                else
                {
                    _AppFeeFixInfo2.Isuse = 0;
                    _AppFeeFixInfo2.Number_Of_Patent = 0;
                }

                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo2.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo2.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo2.Amount_Usd;
                    _AppFeeFixInfo2.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo2.Number_Of_Patent;
                    _AppFeeFixInfo2.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent;
                    _AppFeeFixInfo2.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd;
                }
                else
                {
                    _AppFeeFixInfo2.Amount = 600000 * _AppFeeFixInfo2.Number_Of_Patent;
                }

                _lstFeeFix.Add(_AppFeeFixInfo2);
                #endregion

                #region 3 Phí tra cứu thông tin nhằm phục vụ việc thẩm định
                AppFeeFixInfo _AppFeeFixInfo3 = new AppFeeFixInfo();
                _AppFeeFixInfo3.Fee_Id = 3;

                _AppFeeFixInfo3.Isuse = 0;
                _AppFeeFixInfo3.Number_Of_Patent = 0;
                _AppFeeFixInfo3.Isuse = 1;
                _AppFeeFixInfo3.Number_Of_Patent = _total_PhuongAn;

                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo3.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo3.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo3.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo3.Amount_Usd;
                    _AppFeeFixInfo3.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo3.Number_Of_Patent;
                    _AppFeeFixInfo3.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent;
                    _AppFeeFixInfo3.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd;
                }
                else
                    _AppFeeFixInfo3.Amount = 480000 * _AppFeeFixInfo3.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo3);
                #endregion

                #region 4 phí thẩm định đơn 
                AppFeeFixInfo _AppFeeFixInfo4 = new AppFeeFixInfo();
                _AppFeeFixInfo4.Fee_Id = 4;
                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo4.Fee_Id.ToString();
                _AppFeeFixInfo4.Isuse = 1;
                _AppFeeFixInfo4.Number_Of_Patent = _total_PhuongAn;

                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo4.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo4.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo4.Amount_Usd;
                    _AppFeeFixInfo4.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo4.Number_Of_Patent;
                    _AppFeeFixInfo4.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent;
                    _AppFeeFixInfo4.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd;
                }
                else
                {
                    _AppFeeFixInfo4.Amount = 700000 * _AppFeeFixInfo4.Number_Of_Patent;
                }

                _lstFeeFix.Add(_AppFeeFixInfo4);
                #endregion

                #region 5 Phí công bố đơn

                AppFeeFixInfo _AppFeeFixInfo5 = new AppFeeFixInfo();
                _AppFeeFixInfo5.Fee_Id = 5;
                _AppFeeFixInfo5.Isuse = 1;
                _AppFeeFixInfo5.Number_Of_Patent = 1;
                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo5.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo5.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _AppFeeFixInfo5.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo5.Amount_Usd;
                    _AppFeeFixInfo5.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo5.Number_Of_Patent;
                    _AppFeeFixInfo5.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent;
                    _AppFeeFixInfo5.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd;
                }
                else
                {
                    _AppFeeFixInfo5.Amount = 120000 * _AppFeeFixInfo5.Number_Of_Patent;
                }
                _lstFeeFix.Add(_AppFeeFixInfo5);

                #endregion

                #region 51 Phí công bố đơn từ hình thứ 2 trở đi
                AppFeeFixInfo _AppFeeFixInfo51 = new AppFeeFixInfo();
                _AppFeeFixInfo51.Fee_Id = 51;
                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo51.Fee_Id.ToString();
                decimal _numberPicOver = 1;
                _AppFeeFixInfo51.Level = 1;

                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo51.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                    _numberPicOver = Convert.ToDecimal(MemoryData.c_dic_FeeByApp_Fix[_keyFee].Char01);
                }

                // nếu số hình > Phí công bố đơn từ hình thứ 2 trở đi
                if (_totalImage > _numberPicOver)
                {
                    _AppFeeFixInfo51.Isuse = 1;
                    _AppFeeFixInfo51.Number_Of_Patent = (_totalImage - _numberPicOver);

                    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    {
                        _AppFeeFixInfo51.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo51.Amount_Usd;
                        _AppFeeFixInfo51.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo51.Number_Of_Patent;
                        _AppFeeFixInfo51.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent;
                        _AppFeeFixInfo51.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd;
                    }
                    else
                        _AppFeeFixInfo51.Amount = 60000 * _AppFeeFixInfo51.Number_Of_Patent;
                }
                else
                {
                    _AppFeeFixInfo51.Isuse = 0;
                    _AppFeeFixInfo51.Number_Of_Patent = 0;
                    _AppFeeFixInfo51.Amount = 0;
                }
                _lstFeeFix.Add(_AppFeeFixInfo51);

                #endregion

                #region 6 phí phân loại kiểu dáng công nghiệp

                AppFeeFixInfo _AppFeeFixInfo6 = new AppFeeFixInfo();
                _AppFeeFixInfo6.Fee_Id = 6;
                _AppFeeFixInfo6.Isuse = 1;
                _AppFeeFixInfo6.Number_Of_Patent = 1;
                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo6.Fee_Id.ToString();
                if (pDetail.Phanloai_Type == 2)
                {
                    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    {
                        _AppFeeFixInfo6.Fee_Name = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Description;
                        _AppFeeFixInfo6.Amount_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo6.Amount_Usd;
                        _AppFeeFixInfo6.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo6.Number_Of_Patent;

                        _AppFeeFixInfo6.Amount_Represent = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent;
                        _AppFeeFixInfo6.Amount_Represent_Usd = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount_Represent_Usd;
                    }
                    else
                    {
                        _AppFeeFixInfo6.Amount = 100000 * _AppFeeFixInfo6.Number_Of_Patent;
                    }
                }
                else
                {
                    // tự phân loại
                    _AppFeeFixInfo6.Isuse = 0;
                    _AppFeeFixInfo6.Number_Of_Patent = 0;
                    _AppFeeFixInfo6.Amount = 0;
                }

                _lstFeeFix.Add(_AppFeeFixInfo6);

                #endregion

                return _lstFeeFix;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<AppFeeFixInfo>();
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
                    SessionData.CurrentUser.chashFileOther[pInfo.keyFileUpload] = pInfo;
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

                string fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "A03_VN_" + p_appCode + _datetimenow + ".pdf");
                if (language == Language.LangVI)
                {
                    fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "A03_VN_" + p_appCode + _datetimenow + ".pdf");
                    SessionData.CurrentUser.FilePreview = "/Content/Export/" + "A03_VN_" + p_appCode + _datetimenow + ".pdf";
                }
                else
                {
                    fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "A03_EN_" + p_appCode + _datetimenow + ".pdf");
                    SessionData.CurrentUser.FilePreview = "/Content/Export/" + "A03_EN_" + p_appCode + _datetimenow + ".pdf";
                }

                A03_Info_Export _A03_Info_Export = new A03_Info_Export();
                A03_Info_Export.CopyA03_Info(ref _A03_Info_Export, pDetail);


                // Phí cố định

                List<AppFeeFixInfo> _lstFeeFix = CallFee(pDetail, pAppDocumentInfo, pUTienInfo, pAppDocIndusDesign);
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
        public ActionResult ExportData_View(decimal pAppHeaderId, string p_appCode)
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

                string fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "A03_VN_" + p_appCode + _datetimenow + ".pdf");
                if (app_Detail.Languague_Code == Language.LangVI)
                {
                    fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "A03_VN_" + p_appCode + _datetimenow + ".pdf");
                    SessionData.CurrentUser.FilePreview = "/Content/Export/" + "A03_VN_" + p_appCode + _datetimenow + ".pdf";
                }
                else
                {
                    fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "A03_EN_" + p_appCode + _datetimenow + ".pdf");
                    SessionData.CurrentUser.FilePreview = "/Content/Export/" + "A03_EN_" + p_appCode + _datetimenow + ".pdf";
                }

                Prepare_Data_Export_A03(ref app_Detail, applicationHeaderInfo, appDocumentInfos, _lst_appFeeFixInfos, _lst_authorsInfos, _lst_Other_MasterInfo,
                       _lst_appClassDetailInfos, _LstDocumentOthersInfo, pUTienInfo, pLstImageDesign);

                _lst.Add(app_Detail);
                DataSet _ds_all = ConvertData.ConvertToDataSet<A03_Info_Export>(_lst, false);
                _ds_all.WriteXml(@"C:\inetpub\A03.xml", XmlWriteMode.WriteSchema);
                CrystalDecisions.CrystalReports.Engine.ReportDocument oRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                string _tempfile = "A03.rpt";
                if (app_Detail.Language_Code == Language.LangEN)
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
                if (_lst_authorsInfos.Count > 0)
                {
                    A03_Info_Export.CopyAuthorsInfo(ref app_Detail, _lst_authorsInfos[0], 0);
                }

                if (_lst_authorsInfos.Count > 1)
                {
                    A03_Info_Export.CopyAuthorsInfo(ref app_Detail, _lst_authorsInfos[1], 1);
                    app_Detail.Author_Others = "Y";
                }
                else
                {
                    A03_Info_Export.CopyAuthorsInfo(ref app_Detail, null, 1);
                    app_Detail.Author_Others = "N";
                }

                if (_lst_authorsInfos.Count > 2)
                {
                    A03_Info_Export.CopyAuthorsInfo(ref app_Detail, _lst_authorsInfos[2], 2);
                }
                else
                {
                    A03_Info_Export.CopyAuthorsInfo(ref app_Detail, null, 2);
                }

                // copy chủ đơn khác
                if (_lst_Other_MasterInfo.Count > 1)
                {
                    A03_Info_Export.CopyOther_MasterInfo(ref app_Detail, _lst_Other_MasterInfo[0], 0);
                }
                else
                {
                    A03_Info_Export.CopyOther_MasterInfo(ref app_Detail, null, 0);
                }

                if (_lst_Other_MasterInfo.Count > 2)
                {
                    A03_Info_Export.CopyOther_MasterInfo(ref app_Detail, _lst_Other_MasterInfo[1], 1);
                }
                else
                {
                    A03_Info_Export.CopyOther_MasterInfo(ref app_Detail, null, 1);
                }

                // copy đơn ưu tiên
                if (pUTienInfo.Count > 0)
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
                        app_Detail.strDanhSachFileDinhKem += item.Documentname + " ; ";
                    }

                    app_Detail.strDanhSachFileDinhKem = app_Detail.strDanhSachFileDinhKem.Substring(0, app_Detail.strDanhSachFileDinhKem.Length - 2);
                }

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
                    }
                    else if (item.Document_Id == "A03_11")
                    {
                        app_Detail.Doc_Id_11 = item.CHAR01;
                        app_Detail.Doc_Id_11_Check = item.Isuse;
                    }

                    else if (item.Document_Id == "A03_12")
                    {
                        app_Detail.Doc_Id_12 = item.CHAR01;
                        app_Detail.Doc_Id_12_Check = item.Isuse;
                    }

                    else if (item.Document_Id == "A03_13")
                    {
                        app_Detail.Doc_Id_13 = item.CHAR01;
                        app_Detail.Doc_Id_13_Check = item.Isuse;
                    }

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
                List<AppFeeFixInfo> _lstFeeFix = CallFee(pDetail, pAppDocumentInfo, pUTienInfo, pAppDocIndusDesign);
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
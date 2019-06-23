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
                    List<AppFeeFixInfo> _lstFeeFix = CallFee(pDetail, pAppDocumentInfo, pUTienInfo, pLstImagePublic);
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
                string fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "A01_VN_" + p_appCode + ".pdf");
                string fileName_doc = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "A01_VN_" + p_appCode + ".doc");

                string language = AppsCommon.GetCurrentLang();

                var objBL = new A01_BL();
                List<A01_Info> _lst = new List<A01_Info>();

                List<AppDocumentInfo> appDocumentInfos = new List<AppDocumentInfo>();
                List<AppFeeFixInfo> _lst_appFeeFixInfos = new List<AppFeeFixInfo>();
                ApplicationHeaderInfo applicationHeaderInfo = new ApplicationHeaderInfo();
                List<AuthorsInfo> _lst_authorsInfos = new List<AuthorsInfo>();
                List<Other_MasterInfo> _lst_Other_MasterInfo = new List<Other_MasterInfo>();
                List<AppDocumentOthersInfo> _LstDocumentOthersInfo = new List<AppDocumentOthersInfo>();
                List<AppClassDetailInfo> _lst_appClassDetailInfos = new List<AppClassDetailInfo>();
                List<UTienInfo> pUTienInfo = new List<UTienInfo>();
                List<AppDocumentOthersInfo> pLstImagePublic = new List<AppDocumentOthersInfo>();
                A01_Info app_Detail = objBL.GetByID(pAppHeaderId, language, ref applicationHeaderInfo, ref appDocumentInfos, ref _lst_appFeeFixInfos,
                    ref _lst_authorsInfos, ref _lst_Other_MasterInfo, ref _lst_appClassDetailInfos, ref _LstDocumentOthersInfo, ref pUTienInfo, ref pLstImagePublic);

                // copy Header
                A01_Info.CopyAppHeaderInfo(ref app_Detail, applicationHeaderInfo);

                // copy tác giả
                if (_lst_authorsInfos.Count > 0)
                {
                    A01_Info.CopyAuthorsInfo(ref app_Detail, _lst_authorsInfos[0], 0);
                }

                if (_lst_authorsInfos.Count > 1)
                {
                    A01_Info.CopyAuthorsInfo(ref app_Detail, _lst_authorsInfos[1], 1);
                    app_Detail.Author_Others = "Y";
                }
                else
                {
                    A01_Info.CopyAuthorsInfo(ref app_Detail, null, 1);
                    app_Detail.Author_Others = "N";
                }

                if (_lst_authorsInfos.Count > 2)
                {
                    A01_Info.CopyAuthorsInfo(ref app_Detail, _lst_authorsInfos[2], 2);
                }
                else
                {
                    A01_Info.CopyAuthorsInfo(ref app_Detail, null, 2);
                }

                // copy chủ đơn khác
                if (_lst_Other_MasterInfo.Count > 1)
                {
                    A01_Info.CopyOther_MasterInfo(ref app_Detail, _lst_Other_MasterInfo[0], 0);
                }
                else
                {
                    A01_Info.CopyOther_MasterInfo(ref app_Detail, null, 0);
                }

                if (_lst_Other_MasterInfo.Count > 2)
                {
                    A01_Info.CopyOther_MasterInfo(ref app_Detail, _lst_Other_MasterInfo[1], 1);
                }
                else
                {
                    A01_Info.CopyOther_MasterInfo(ref app_Detail, null, 1);
                }

                // copy đơn ưu tiên
                if (pUTienInfo.Count > 0)
                {
                    A01_Info.CopyUuTienInfo(ref app_Detail, pUTienInfo[0]);
                }
                else
                {
                    A01_Info.CopyUuTienInfo(ref app_Detail, null);
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
                    if (item.Document_Id == "A01_01")
                    {
                        app_Detail.Doc_Id_1 = item.CHAR01;
                        app_Detail.Doc_Id_102 = item.CHAR02;
                        app_Detail.Doc_Id_1_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "A01_02")
                    {
                        app_Detail.Doc_Id_2 = item.CHAR01;
                        app_Detail.Doc_Id_202 = item.CHAR02;

                        app_Detail.Doc_Id_2_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "A01_03")
                    {
                        app_Detail.Doc_Id_3_Check = item.Isuse;
                        app_Detail.Doc_Id_3 = item.CHAR01;
                    }
                    else if (item.Document_Id == "A01_04")
                    {
                        app_Detail.Doc_Id_4 = item.CHAR01;
                        app_Detail.Doc_Id_402 = item.CHAR02;
                        app_Detail.Doc_Id_4_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "A01_05")
                    {
                        app_Detail.Doc_Id_5_Check = item.Isuse;
                        app_Detail.Doc_Id_5 = item.CHAR01;
                    }

                    else if (item.Document_Id == "A01_06")
                    {
                        app_Detail.Doc_Id_6_Check = item.Isuse;
                        app_Detail.Doc_Id_6 = item.CHAR01;
                    }
                    else if (item.Document_Id == "A01_07")
                    {
                        app_Detail.Doc_Id_7_Check = item.Isuse;
                        app_Detail.Doc_Id_7 = item.CHAR01;
                    }
                    else if (item.Document_Id == "A01_07")
                    {
                        app_Detail.Doc_Id_8_Check = item.Isuse;
                        app_Detail.Doc_Id_8 = item.CHAR01;
                    }

                    else if (item.Document_Id == "A01_09")
                    {
                        app_Detail.Doc_Id_9 = item.CHAR01;
                        app_Detail.Doc_Id_9_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "A01_10")
                    {
                        app_Detail.Doc_Id_10_Check = item.Isuse;
                        app_Detail.Doc_Id_10 = item.CHAR01;
                    }
                    else if (item.Document_Id == "A01_11")
                    {
                        app_Detail.Doc_Id_11 = item.CHAR01;
                        app_Detail.Doc_Id_11_Check = item.Isuse;
                    }

                    else if (item.Document_Id == "A01_12")
                    {
                        app_Detail.Doc_Id_12 = item.CHAR01;
                        app_Detail.Doc_Id_12_Check = item.Isuse;
                    }

                    else if (item.Document_Id == "A01_13")
                    {
                        app_Detail.Doc_Id_13 = item.CHAR01;
                        app_Detail.Doc_Id_13_Check = item.Isuse;
                    }

                    else if (item.Document_Id == "A01_14")
                    {
                        app_Detail.Doc_Id_14 = item.CHAR01;
                        app_Detail.Doc_Id_14_Check = item.Isuse;
                    }

                    else if (item.Document_Id == "A01_15")
                    {
                        app_Detail.Doc_Id_15 = item.CHAR01;
                        app_Detail.Doc_Id_15_Check = item.Isuse;
                    }

                    else if (item.Document_Id == "A01_16")
                    {
                        app_Detail.Doc_Id_16 = item.CHAR01;
                        app_Detail.Doc_Id_16_Check = item.Isuse;
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
                            app_Detail.Fee_Id_1 = item.Number_Of_Patent;
                            app_Detail.Fee_Id_1_Check = item.Isuse;
                            app_Detail.Fee_Id_1_Val = item.Amount.ToString("#,##0.##");
                        }
                        else if (item.Fee_Id == 2)
                        {
                            app_Detail.Fee_Id_2 = item.Number_Of_Patent;
                            app_Detail.Fee_Id_2_Check = item.Isuse;
                            app_Detail.Fee_Id_2_Val = item.Amount.ToString("#,##0.##");
                        }
                        else if (item.Fee_Id == 21)
                        {
                            app_Detail.Fee_Id_21 = item.Number_Of_Patent;
                            app_Detail.Fee_Id_21_Check = item.Isuse;
                            app_Detail.Fee_Id_21_Val = item.Amount.ToString("#,##0.##");
                        }
                        else if (item.Fee_Id == 3)
                        {
                            app_Detail.Fee_Id_3 = item.Number_Of_Patent;
                            app_Detail.Fee_Id_3_Check = item.Isuse;
                            app_Detail.Fee_Id_3_Val = item.Amount.ToString("#,##0.##");
                        }

                        else if (item.Fee_Id == 4)
                        {
                            app_Detail.Fee_Id_4 = item.Number_Of_Patent;
                            app_Detail.Fee_Id_4_Check = item.Isuse;
                            app_Detail.Fee_Id_4_Val = item.Amount.ToString("#,##0.##");
                        }

                        else if (item.Fee_Id == 5)
                        {
                            app_Detail.Fee_Id_5 = item.Number_Of_Patent;
                            app_Detail.Fee_Id_5_Check = item.Isuse;
                            app_Detail.Fee_Id_5_Val = item.Amount.ToString("#,##0.##");
                        }

                        else if (item.Fee_Id == 6)
                        {
                            app_Detail.Fee_Id_6 = item.Number_Of_Patent;
                            app_Detail.Fee_Id_6_Check = item.Isuse;
                            app_Detail.Fee_Id_6_Val = item.Amount.ToString("#,##0.##");
                        }

                        else if (item.Fee_Id == 61)
                        {
                            app_Detail.Fee_Id_61 = item.Number_Of_Patent;
                            app_Detail.Fee_Id_61_Check = item.Isuse;
                            app_Detail.Fee_Id_61_Val = item.Amount.ToString("#,##0.##");
                        }

                        else if (item.Fee_Id == 62)
                        {
                            app_Detail.Fee_Id_62 = item.Number_Of_Patent;
                            app_Detail.Fee_Id_62_Check = item.Isuse;
                            app_Detail.Fee_Id_62_Val = item.Amount.ToString("#,##0.##");
                        }

                        else if (item.Fee_Id == 7)
                        {
                            app_Detail.Fee_Id_7 = item.Number_Of_Patent;
                            app_Detail.Fee_Id_7_Check = item.Isuse;
                            app_Detail.Fee_Id_7_Val = item.Amount.ToString("#,##0.##");
                        }

                        else if (item.Fee_Id == 71)
                        {
                            app_Detail.Fee_Id_71 = item.Number_Of_Patent;
                            app_Detail.Fee_Id_71_Check = item.Isuse;
                            app_Detail.Fee_Id_71_Val = item.Amount.ToString("#,##0.##");
                        }

                        else if (item.Fee_Id == 72)
                        {
                            app_Detail.Fee_Id_72 = item.Number_Of_Patent;
                            app_Detail.Fee_Id_72_Check = item.Isuse;
                            app_Detail.Fee_Id_72_Val = item.Amount.ToString("#,##0.##");
                        }

                        app_Detail.Total_Fee = app_Detail.Total_Fee + item.Amount;
                        app_Detail.Total_Fee_Str = app_Detail.Total_Fee.ToString("#,##0.##");
                    }
                }
                #endregion

                _lst.Add(app_Detail);
                DataSet _ds_all = ConvertData.ConvertToDataSet<A01_Info>(_lst, false);
                //_ds_all.WriteXml(@"E:\A01.xml", XmlWriteMode.IgnoreSchema);

                CrystalDecisions.CrystalReports.Engine.ReportDocument oRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                string _tempfile = "A01.rpt";
                if (app_Detail.Language_Code == Language.LangEN)
                {
                    _tempfile = "A01_EN.rpt";
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
                ViewBag.FileName = "/Content/Export/" + "A01_VN_" + TradeMarkAppCode.AppCode_A01 + ".pdf";
                return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistration/_PartialContentPreview.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistration/_PartialContentPreview.cshtml");
            }
        }

        List<AppFeeFixInfo> CallFee(A01_Info pDetail, List<AppDocumentInfo> pAppDocumentInfo,
            List<UTienInfo> pUTienInfo, List<AppDocumentOthersInfo> pLstImagePublic)
        {
            try
            {
                #region 1 Lệ phí nộp đơn
                List<AppFeeFixInfo> _lstFeeFix = new List<AppFeeFixInfo>();
                AppFeeFixInfo _AppFeeFixInfo1 = new AppFeeFixInfo();
                _AppFeeFixInfo1.Fee_Id = 1;
                _AppFeeFixInfo1.Isuse = 1;
                _AppFeeFixInfo1.Number_Of_Patent = 1;
                string _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo1.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo1.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo1.Number_Of_Patent;
                }
                else
                    _AppFeeFixInfo1.Amount = 150000 * _AppFeeFixInfo1.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo1);
                #endregion

                #region 2 Phí thẩm định hình thức
                AppFeeFixInfo _AppFeeFixInfo2 = new AppFeeFixInfo();
                _AppFeeFixInfo2.Fee_Id = 2;
                _AppFeeFixInfo2.Isuse = pDetail.Point == -1 ? 0 : 1;
                _AppFeeFixInfo2.Number_Of_Patent = pDetail.Point == -1 ? 0 : pDetail.Point;

                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo2.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo2.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo2.Number_Of_Patent;
                }
                else
                    _AppFeeFixInfo2.Amount = 160000 * _AppFeeFixInfo2.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo2);
                #endregion

                #region 2.1 Phí thẩm định hình thức từ trang bản mô tả thứ 7 trở đi
                AppFeeFixInfo _AppFeeFixInfo21 = new AppFeeFixInfo();
                _AppFeeFixInfo21.Fee_Id = 21;
                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo21.Fee_Id.ToString();
                decimal _numberPicOver = 5;
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _numberPicOver = Convert.ToDecimal(MemoryData.c_dic_FeeByApp_Fix[_keyFee].Char01);
                }

                // nếu số hình > số hình quy định tính từ trang thứ 7 trở đi
                if (pAppDocumentInfo != null && pAppDocumentInfo.Count > 0)
                {
                    AppDocumentInfo _AppDocumentInfo = null;
                    foreach (var item in pAppDocumentInfo)
                    {
                        if (item.Document_Id == "A01_02")
                        {
                            _AppDocumentInfo = item;
                        }
                    }

                    if (_AppDocumentInfo == null)
                    {
                        if (_AppDocumentInfo.CHAR02 != "" && Convert.ToDecimal(_AppDocumentInfo.CHAR02) > _numberPicOver)
                        {
                            _AppFeeFixInfo21.Isuse = 1;
                            _AppFeeFixInfo21.Number_Of_Patent = Convert.ToDecimal(_AppDocumentInfo.CHAR02) - _numberPicOver;

                            if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                            {
                                _AppFeeFixInfo21.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo21.Number_Of_Patent;
                            }
                            else
                                _AppFeeFixInfo21.Amount = 60000 * _AppFeeFixInfo21.Number_Of_Patent;
                        }
                        else
                        {
                            _AppFeeFixInfo21.Isuse = 0;
                            _AppFeeFixInfo21.Number_Of_Patent = 0;
                            _AppFeeFixInfo21.Amount = 0;
                        }
                    }
                    else
                    {
                        _AppFeeFixInfo21.Isuse = 0;
                        _AppFeeFixInfo21.Number_Of_Patent = 0;
                        _AppFeeFixInfo21.Amount = 0;
                    }
                }
                else
                {
                    _AppFeeFixInfo21.Isuse = 0;
                    _AppFeeFixInfo21.Number_Of_Patent = 0;
                    _AppFeeFixInfo21.Amount = 0;
                }
                _lstFeeFix.Add(_AppFeeFixInfo21);

                #endregion

                #region 3 Phí phân loại quốc tế về sáng chế
                AppFeeFixInfo _AppFeeFixInfo3 = new AppFeeFixInfo();
                _AppFeeFixInfo3.Fee_Id = 3;
                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo3.Fee_Id.ToString();

                if (pDetail.Class_Type == "CUC")
                {
                    _AppFeeFixInfo3.Isuse = 1;
                    _AppFeeFixInfo3.Number_Of_Patent = 1;
                }
                else
                {
                    _AppFeeFixInfo3.Isuse = 0;
                    _AppFeeFixInfo3.Number_Of_Patent = 0;
                }

                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo3.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo3.Number_Of_Patent;
                }
                else
                {
                    _AppFeeFixInfo3.Amount = 100000 * _AppFeeFixInfo3.Number_Of_Patent;
                }

                _lstFeeFix.Add(_AppFeeFixInfo3);
                #endregion

                #region 4 Quyền ưu tiên
                AppFeeFixInfo _AppFeeFixInfo4 = new AppFeeFixInfo();
                _AppFeeFixInfo4.Fee_Id = 4;
                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo4.Fee_Id.ToString();

                if (pUTienInfo != null && pUTienInfo.Count > 0)
                {
                    _AppFeeFixInfo4.Isuse = 1;
                    _AppFeeFixInfo4.Number_Of_Patent = pUTienInfo.Count;
                }
                else
                {
                    _AppFeeFixInfo4.Isuse = 0;
                    _AppFeeFixInfo4.Number_Of_Patent = 0;
                }

                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo4.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo4.Number_Of_Patent;
                }
                else
                {
                    _AppFeeFixInfo4.Amount = 100000 * _AppFeeFixInfo4.Number_Of_Patent;
                }

                _lstFeeFix.Add(_AppFeeFixInfo4);
                #endregion

                #region 5 Phí thẩm định yêu cầu sửa đổi
                AppFeeFixInfo _AppFeeFixInfo5 = new AppFeeFixInfo();
                _AppFeeFixInfo5.Fee_Id = 5;
                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo5.Fee_Id.ToString();

                if (pDetail.PCT_Suadoi == 1)
                {
                    _AppFeeFixInfo5.Isuse = 1;
                    _AppFeeFixInfo5.Number_Of_Patent = pUTienInfo.Count;
                }
                else
                {
                    _AppFeeFixInfo5.Isuse = 0;
                    _AppFeeFixInfo5.Number_Of_Patent = 0;
                }

                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo5.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo5.Number_Of_Patent;
                }
                else
                {
                    _AppFeeFixInfo5.Amount = 160000 * _AppFeeFixInfo5.Number_Of_Patent;
                }

                _lstFeeFix.Add(_AppFeeFixInfo5);
                #endregion

                #region 6 Phí công bố đơn

                AppFeeFixInfo _AppFeeFixInfo6 = new AppFeeFixInfo();
                _AppFeeFixInfo6.Fee_Id = 6;
                _AppFeeFixInfo6.Isuse = 1;
                _AppFeeFixInfo6.Number_Of_Patent = 1;
                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo6.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo6.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo6.Number_Of_Patent;
                }
                else
                {
                    _AppFeeFixInfo6.Amount = 150000 * _AppFeeFixInfo6.Number_Of_Patent;
                }
                _lstFeeFix.Add(_AppFeeFixInfo6);

                #endregion

                #region 6.1 Phí công bố đơn Đơn có trên 1 hình (từ hình thứ 2 trở đi)
                AppFeeFixInfo _AppFeeFixInfo61 = new AppFeeFixInfo();
                _AppFeeFixInfo61.Fee_Id = 61;
                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo61.Fee_Id.ToString();
                _numberPicOver = 1;
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _numberPicOver = Convert.ToDecimal(MemoryData.c_dic_FeeByApp_Fix[_keyFee].Char01);
                }

                // nếu số hình > số hình quy định tính từ trang thứ 7 trở đi
                if (pLstImagePublic != null && pLstImagePublic.Count > _numberPicOver)
                {
                    _AppFeeFixInfo61.Isuse = 1;
                    _AppFeeFixInfo61.Number_Of_Patent = (pLstImagePublic.Count - _numberPicOver);

                    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                    {
                        _AppFeeFixInfo61.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo61.Number_Of_Patent;
                    }
                    else
                        _AppFeeFixInfo61.Amount = 60000 * _AppFeeFixInfo61.Number_Of_Patent;
                }
                else
                {
                    _AppFeeFixInfo61.Isuse = 0;
                    _AppFeeFixInfo61.Number_Of_Patent = 0;
                    _AppFeeFixInfo61.Amount = 0;
                }
                _lstFeeFix.Add(_AppFeeFixInfo61);

                #endregion

                #region 6.2 Phí công bố đơn Bản mô tả có trên 6 trang (từ trang thứ 7 trở đi)
                AppFeeFixInfo _AppFeeFixInfo62 = new AppFeeFixInfo();
                _AppFeeFixInfo62.Fee_Id = 62;

                _numberPicOver = 6;
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _numberPicOver = Convert.ToDecimal(MemoryData.c_dic_FeeByApp_Fix[_keyFee].Char01);
                }

                // nếu số hình > số hình quy định tính từ trang thứ 7 trở đi
                if (pAppDocumentInfo != null && pAppDocumentInfo.Count > 0)
                {
                    AppDocumentInfo _AppDocumentInfo = null;
                    foreach (var item in pAppDocumentInfo)
                    {
                        if (item.Document_Id == "A01_02")
                        {
                            _AppDocumentInfo = item;
                        }
                    }

                    if (_AppDocumentInfo == null)
                    {
                        if (_AppDocumentInfo.CHAR02 != "" && Convert.ToDecimal(_AppDocumentInfo.CHAR02) > _numberPicOver)
                        {
                            _AppFeeFixInfo62.Isuse = 1;
                            _AppFeeFixInfo62.Number_Of_Patent = Convert.ToDecimal(_AppDocumentInfo.CHAR02) - _numberPicOver;

                            if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                            {
                                _AppFeeFixInfo62.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo62.Number_Of_Patent;
                            }
                            else
                                _AppFeeFixInfo62.Amount = 8000 * _AppFeeFixInfo62.Number_Of_Patent;
                        }
                        else
                        {
                            _AppFeeFixInfo62.Isuse = 0;
                            _AppFeeFixInfo62.Number_Of_Patent = 0;
                            _AppFeeFixInfo62.Amount = 0;
                        }
                    }
                    else
                    {
                        _AppFeeFixInfo62.Isuse = 0;
                        _AppFeeFixInfo62.Number_Of_Patent = 0;
                        _AppFeeFixInfo62.Amount = 0;
                    }
                }
                else
                {
                    _AppFeeFixInfo62.Isuse = 0;
                    _AppFeeFixInfo62.Number_Of_Patent = 0;
                    _AppFeeFixInfo62.Amount = 0;
                }

                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo62.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo62.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo62.Number_Of_Patent;
                }
                else
                    _AppFeeFixInfo62.Amount = 600000 * _AppFeeFixInfo62.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo62);

                #endregion

                #region 7 Phí thẩm định nội dung

                AppFeeFixInfo _AppFeeFixInfo7 = new AppFeeFixInfo();
                _AppFeeFixInfo7.Fee_Id = 7;

                if (pDetail.ThamDinhNoiDung == "TDND")
                {
                    _AppFeeFixInfo7.Isuse = 1;
                    _AppFeeFixInfo7.Number_Of_Patent = 1;
                }
                else
                {
                    _AppFeeFixInfo7.Isuse = 0;
                    _AppFeeFixInfo7.Number_Of_Patent = 0;
                }

                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo7.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo7.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo7.Number_Of_Patent;
                }
                else
                {
                    _AppFeeFixInfo7.Amount = 720000 * _AppFeeFixInfo7.Number_Of_Patent;
                }
                _lstFeeFix.Add(_AppFeeFixInfo7);

                #endregion

                #region 7.1 Phí thẩm định hình thức từ trang bản mô tả thứ 7 trở đi
                AppFeeFixInfo _AppFeeFixInfo71 = new AppFeeFixInfo();
                _AppFeeFixInfo71.Fee_Id = 71;
                _numberPicOver = 5;
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _numberPicOver = Convert.ToDecimal(MemoryData.c_dic_FeeByApp_Fix[_keyFee].Char01);
                }

                if (pDetail.ThamDinhNoiDung == "TDND")
                {
                    // nếu số hình > số hình quy định tính từ trang thứ 7 trở đi
                    if (pAppDocumentInfo != null && pAppDocumentInfo.Count > 0)
                    {
                        AppDocumentInfo _AppDocumentInfo = null;
                        foreach (var item in pAppDocumentInfo)
                        {
                            if (item.Document_Id == "A01_02")
                            {
                                _AppDocumentInfo = item;
                            }
                        }

                        if (_AppDocumentInfo == null)
                        {
                            if (_AppDocumentInfo.CHAR02 != "" && Convert.ToDecimal(_AppDocumentInfo.CHAR02) > _numberPicOver)
                            {
                                _AppFeeFixInfo71.Isuse = 1;
                                _AppFeeFixInfo71.Number_Of_Patent = Convert.ToDecimal(_AppDocumentInfo.CHAR02) - _numberPicOver;

                                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                                {
                                    _AppFeeFixInfo71.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo71.Number_Of_Patent;
                                }
                                else
                                    _AppFeeFixInfo71.Amount = 60000 * _AppFeeFixInfo71.Number_Of_Patent;
                            }
                            else
                            {
                                _AppFeeFixInfo71.Isuse = 0;
                                _AppFeeFixInfo71.Number_Of_Patent = 0;
                                _AppFeeFixInfo71.Amount = 0;
                            }
                        }
                        else
                        {
                            _AppFeeFixInfo71.Isuse = 0;
                            _AppFeeFixInfo71.Number_Of_Patent = 0;
                            _AppFeeFixInfo71.Amount = 0;
                        }
                    }
                    else
                    {
                        _AppFeeFixInfo71.Isuse = 0;
                        _AppFeeFixInfo71.Number_Of_Patent = 0;
                        _AppFeeFixInfo71.Amount = 0;
                    }
                }
                else
                {
                    _AppFeeFixInfo71.Isuse = 0;
                    _AppFeeFixInfo71.Number_Of_Patent = 0;
                    _AppFeeFixInfo71.Amount = 0;
                }

                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo71.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo71.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo71.Number_Of_Patent;
                }
                else
                    _AppFeeFixInfo71.Amount = 32000 * _AppFeeFixInfo71.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo71);

                #endregion

                #region 72 Phí tra cứu thông tin nhằm phục vụ việc thẩm định
                AppFeeFixInfo _AppFeeFixInfo72 = new AppFeeFixInfo();
                _AppFeeFixInfo72.Fee_Id = 72;

                if (pDetail.ThamDinhNoiDung == "TDND")
                {
                    _AppFeeFixInfo72.Isuse = pDetail.Point == -1 ? 0 : 1;
                    _AppFeeFixInfo72.Number_Of_Patent = pDetail.Point == -1 ? 0 : pDetail.Point;
                }
                else
                {
                    _AppFeeFixInfo72.Isuse = 0;
                    _AppFeeFixInfo72.Number_Of_Patent = 0;
                }

                _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo72.Fee_Id.ToString();
                if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                {
                    _AppFeeFixInfo72.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo72.Number_Of_Patent;
                }
                else
                    _AppFeeFixInfo72.Amount = 600000 * _AppFeeFixInfo72.Number_Of_Patent;
                _lstFeeFix.Add(_AppFeeFixInfo72);
                #endregion

                return _lstFeeFix;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<AppFeeFixInfo>();
            }
        }
    }
}
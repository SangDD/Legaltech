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
    [RouteArea("PatentRegistration", AreaPrefix = "patent-a01")]
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
            List<AppAuthorsInfo> pAppAuthorsInfo, List<Other_MasterInfo> pOther_MasterInfo)
        {
            try
            {
                //Application_Header_BL objBL = new Application_Header_BL();
                //AppFeeFixBL objFeeFixBL = new AppFeeFixBL();
                //App_Detail_PLB01_SDD_BL objDetail = new App_Detail_PLB01_SDD_BL();
                //AppDocumentBL objDoc = new AppDocumentBL();
                //if (pInfo == null || pDetail == null) return Json(new { status = ErrorCode.Error });
                //string language = AppsCommon.GetCurrentLang();

                //var CreatedBy = SessionData.CurrentUser.Username;

                //var CreatedDate = SessionData.CurrentUser.CurrentDate;
                //decimal pReturn = ErrorCode.Success;
                //int pAppHeaderID = 0;
                //string p_case_code = "";

                //using (var scope = new TransactionScope())
                //{
                //    //
                //    pInfo.Languague_Code = language;
                //    if (pInfo.Created_By == null || pInfo.Created_By == "0" || pInfo.Created_By == "")
                //    {
                //        pInfo.Created_By = CreatedBy;
                //    }

                //    pInfo.Created_Date = CreatedDate;
                //    pInfo.Send_Date = DateTime.Now;
                //    //pInfo.Status = (decimal)CommonEnums.App_Status.DaGui_ChoPhanLoai;

                //    //TRA RA ID CUA BANG KHI INSERT
                //    pAppHeaderID = objBL.AppHeaderInsert(pInfo, ref p_case_code);
                //    if (pAppHeaderID < 0)
                //        goto Commit_Transaction;

                //    // detail
                //    if (pAppHeaderID >= 0)
                //    {
                //        pDetail.Appcode = pInfo.Appcode;
                //        pDetail.Language_Code = language;
                //        pDetail.App_Header_Id = pAppHeaderID;
                //        pDetail.Case_Code = p_case_code;
                //        pReturn = objDetail.Insert(pDetail);
                //        if (pReturn <= 0)
                //            goto Commit_Transaction;
                //    }

                //    #region Phí cố định

                //    #region Phí thẩm định yêu cầu sửa đổi đơn
                //    List<AppFeeFixInfo> _lstFeeFix = new List<AppFeeFixInfo>();
                //    AppFeeFixInfo _AppFeeFixInfo1 = new AppFeeFixInfo();
                //    _AppFeeFixInfo1.Fee_Id = 1;
                //    _AppFeeFixInfo1.Isuse = 1;
                //    //_AppFeeFixInfo1.App_Header_Id = pAppHeaderID;
                //    _AppFeeFixInfo1.Number_Of_Patent = pDetail.App_No_Change.Split(',').Length;

                //    string _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo1.Fee_Id.ToString();
                //    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                //    {
                //        _AppFeeFixInfo1.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo1.Number_Of_Patent;
                //    }
                //    else
                //        _AppFeeFixInfo1.Amount = 160000 * _AppFeeFixInfo1.Number_Of_Patent;
                //    _lstFeeFix.Add(_AppFeeFixInfo1);
                //    #endregion

                //    #region Phí công bố thông tin đơn sửa đổi
                //    AppFeeFixInfo _AppFeeFixInfo2 = new AppFeeFixInfo();
                //    _AppFeeFixInfo2.Fee_Id = 2;
                //    _AppFeeFixInfo2.Isuse = 1;
                //    //_AppFeeFixInfo2.App_Header_Id = pAppHeaderID;
                //    _AppFeeFixInfo2.Number_Of_Patent = pDetail.App_No_Change.Split(',').Length;

                //    _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo2.Fee_Id.ToString();
                //    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                //    {
                //        _AppFeeFixInfo2.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo2.Number_Of_Patent;
                //    }
                //    else
                //        _AppFeeFixInfo2.Amount = 160000 * _AppFeeFixInfo2.Number_Of_Patent;
                //    _lstFeeFix.Add(_AppFeeFixInfo2);
                //    #endregion

                //    #region Đơn có trên 1 hình (từ hình thứ hai trở đi)
                //    AppFeeFixInfo _AppFeeFixInfo21 = new AppFeeFixInfo();
                //    _AppFeeFixInfo21.Fee_Id = 21;
                //    //_AppFeeFixInfo21.App_Header_Id = pAppHeaderID;

                //    _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo21.Fee_Id.ToString();
                //    decimal _numberPicOver = 1;
                //    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                //    {
                //        _numberPicOver = Convert.ToDecimal(MemoryData.c_dic_FeeByApp_Fix[_keyFee].Char01);
                //    }

                //    // nếu số hình > số hình quy định
                //    if (pDetail.Number_Pic > _numberPicOver)
                //    {
                //        _AppFeeFixInfo21.Isuse = 1;
                //        _AppFeeFixInfo21.Number_Of_Patent = (pDetail.Number_Pic - _numberPicOver);

                //        if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                //        {
                //            _AppFeeFixInfo21.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo21.Number_Of_Patent;
                //        }
                //        else
                //            _AppFeeFixInfo21.Amount = 60000 * _AppFeeFixInfo21.Number_Of_Patent;

                //    }
                //    else
                //    {
                //        _AppFeeFixInfo21.Isuse = 0;
                //        _AppFeeFixInfo21.Number_Of_Patent = 0;
                //        _AppFeeFixInfo21.Amount = 0;
                //    }
                //    _lstFeeFix.Add(_AppFeeFixInfo21);

                //    #endregion

                //    #region Bản mô tả sáng chế có trên 6 trang (từ trang thứ 7 trở đi)  
                //    AppFeeFixInfo _AppFeeFixInfo22 = new AppFeeFixInfo();
                //    _AppFeeFixInfo22.Fee_Id = 22;
                //    //_AppFeeFixInfo22.App_Header_Id = pAppHeaderID;

                //    _keyFee = pDetail.Appcode + "_" + _AppFeeFixInfo22.Fee_Id.ToString();
                //    decimal _numberPageOver = 6;
                //    if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                //    {
                //        _numberPageOver = Convert.ToDecimal(MemoryData.c_dic_FeeByApp_Fix[_keyFee].Char01);
                //    }

                //    // nếu số hình > số hình quy định
                //    if (pDetail.Number_Page > _numberPageOver)
                //    {
                //        _AppFeeFixInfo22.Isuse = 1;
                //        _AppFeeFixInfo22.Number_Of_Patent = (pDetail.Number_Page - _numberPageOver);

                //        if (MemoryData.c_dic_FeeByApp_Fix.ContainsKey(_keyFee))
                //        {
                //            _AppFeeFixInfo22.Amount = MemoryData.c_dic_FeeByApp_Fix[_keyFee].Amount * _AppFeeFixInfo22.Number_Of_Patent;
                //        }
                //        else
                //            _AppFeeFixInfo22.Amount = 10000 * _AppFeeFixInfo22.Number_Of_Patent;

                //    }
                //    else
                //    {
                //        _AppFeeFixInfo22.Isuse = 0;
                //        _AppFeeFixInfo22.Number_Of_Patent = 0;
                //        _AppFeeFixInfo22.Amount = 0;
                //    }
                //    _lstFeeFix.Add(_AppFeeFixInfo22);
                //    #endregion

                //    AppFeeFixBL _AppFeeFixBL = new AppFeeFixBL();
                //    pReturn = _AppFeeFixBL.AppFeeFixInsertBath(_lstFeeFix, p_case_code);
                //    if (pReturn < 0)
                //        goto Commit_Transaction;
                //    #endregion

                //    #region Tai lieu dinh kem 
                //    if (pReturn >= 0 && pAppDocumentInfo != null)
                //    {
                //        if (pAppDocumentInfo.Count > 0)
                //        {
                //            foreach (var info in pAppDocumentInfo)
                //            {
                //                if (SessionData.CurrentUser.chashFile.ContainsKey(info.keyFileUpload))
                //                {
                //                    HttpPostedFileBase pfiles = (HttpPostedFileBase)SessionData.CurrentUser.chashFile[info.keyFileUpload];
                //                    info.Filename = pfiles.FileName;
                //                    info.Url_Hardcopy = "/Content/Archive/" + AppUpload.Document + "/" + pfiles.FileName;
                //                    info.Status = 0;
                //                }
                //                info.App_Header_Id = pAppHeaderID;
                //                info.Document_Filing_Date = CommonFuc.CurrentDate();
                //                info.Language_Code = language;
                //            }
                //            pReturn = objDoc.AppDocumentInsertBath(pAppDocumentInfo, pAppHeaderID);

                //        }
                //    }
                //    #endregion

                //    //end
                //    Commit_Transaction:
                //    if (pReturn < 0)
                //    {
                //        Transaction.Current.Rollback();
                //    }
                //    else
                //    {
                //        scope.Complete();
                //    }
                //}
                //return Json(new { status = pAppHeaderID });

                return Json(new { status = 1 });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { status = ErrorCode.Error });
            }
        }
    }
}
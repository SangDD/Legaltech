using BussinessFacade.ModuleTrademark;
using Common;
using ObjectInfos;
using ObjectInfos.ModuleTrademark;
using System;
using System.Collections.Generic;
using System.IO;

using System.Web.Mvc;
using WebApps.AppStart;
using WebApps.CommonFunction;
using WebApps.Session;
using Common.Extensions;
using BussinessFacade;
using Common.CommonData;

namespace WebApps.Areas.TradeMark.Controllers
{
    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("TradeMarkRegistration", AreaPrefix = "trade-mark-share-data")]
    [Route("{action}")]
    public class TradeMarkShareDataController : Controller
    {
        // GET: TradeMark/TradeMarkShareData


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
        [Route("get-tm04nh-info")]
        public ActionResult TM04NHGetInfo(decimal pAppHeaderId, string p_idchudon, string p_iddaidienchudon, string p_idappclass)
        {
            try
            {
                var objBL = new AppDetail04NHBL();
                string language = AppsCommon.GetCurrentLang();
                var ds04NH = objBL.AppTM04NHGetByID(pAppHeaderId, language, 7);// tạm thời truyền vào trạng thái = 7
                var _AppDetail04NHInfo = new AppDetail04NHInfo();
                if (ds04NH != null && ds04NH.Tables.Count == 5)
                {
                    _AppDetail04NHInfo = CBO<AppDetail04NHInfo>.FillObjectFromDataTable(ds04NH.Tables[0]);
                    ViewBag.lstClassDetailInfo = CBO<AppClassDetailInfo>.FillCollectionFromDataTable(ds04NH.Tables[3]);
                }
                ViewBag.objAppHeaderInfo = _AppDetail04NHInfo;
                string _viewChuDon = "";
                string _viewDaiDienChuDon = "";
                string _viewAppClass = "";
                ViewBag.ShowFromOtherApp = 1;
                ViewBag.Isdisable = 1;
                _viewChuDon = RenderPartialToString("~/Areas/TradeMark/Views/Shared/_PartialThongTinChuDon.cshtml", p_idchudon);
                _viewDaiDienChuDon = RenderPartialToString("~/Areas/TradeMark/Views/Shared/_PartialThongTinDaiDienChuDon.cshtml", p_iddaidienchudon);
                _viewAppClass = RenderPartialToString("~/Areas/TradeMark/Views/TradeMarkRegistration01/_PartialTMAddAppClass.cshtml", null);
                return Json(new
                {
                    success = 0,
                    NgayNopDon = _AppDetail04NHInfo.Ngaynopdon_Ut.ToDateStringN0(),
                    LogoURL = _AppDetail04NHInfo.Logourl,
                    ViewChuDon = _viewChuDon,
                    ViewDaiDienChuDon = _viewDaiDienChuDon,
                    ViewAppClass = _viewAppClass
                });

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = -1 });
            }
        }




        [Route("app-details/{id}/{id1}/{id2}")]
        public ActionResult AppDetails()
        {
            try
            {
                decimal pAppHeaderId = 0;
                int pStatus = 0;
                string pAppCode = "";
                if (SessionData.CurrentUser == null)
                    return Redirect("/");
                SessionData.CurrentUser.chashFile.Clear();
                SessionData.CurrentUser.chashFileOther.Clear();

                if (RouteData.Values.ContainsKey("id"))
                {
                    pAppHeaderId = CommonFuc.ConvertToDecimal(RouteData.Values["id"]);
                }
                if (RouteData.Values.ContainsKey("id1"))
                {
                    pStatus = CommonFuc.ConvertToInt(RouteData.Values["id1"]);
                }
                if (RouteData.Values.ContainsKey("id2"))
                {
                    pAppCode = RouteData.Values["id2"].ToString().ToUpper();
                }
                string _casecode = "";
                ViewBag.Appcode = pAppCode;
                ViewBag.Currstatus = pStatus;
                if (pAppCode == TradeMarkAppCode.AppCodeDangKynhanHieu)
                {
                    var objBL = new AppDetail04NHBL();
                    string language = AppsCommon.GetCurrentLang();
                    var ds04NH = objBL.AppTM04NHGetByID(pAppHeaderId, language, pStatus);
                    if (ds04NH != null && ds04NH.Tables.Count == 5)
                    {
                        AppDetail04NHInfo _objinfo = CBO<AppDetail04NHInfo>.FillObjectFromDataTable(ds04NH.Tables[0]);
                        ViewBag.objAppHeaderInfo = _objinfo;
                        ViewBag.lstDocumentInfo = CBO<AppDocumentInfo>.FillCollectionFromDataTable(ds04NH.Tables[1]);
                        ViewBag.lstDocOther = CBO<AppDocumentOthersInfo>.FillCollectionFromDataTable(ds04NH.Tables[2]);
                        ViewBag.lstClassDetailInfo = CBO<AppClassDetailInfo>.FillCollectionFromDataTable(ds04NH.Tables[3]);
                        ViewBag.lstFeeInfo = CBO<AppFeeFixInfo>.FillCollectionFromDataTable(ds04NH.Tables[4]);
                        _casecode = _objinfo.Case_Code;
                    }
                    // return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistration/View_PartialDangKyNhanHieu.cshtml");
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
                    _casecode = applicationHeaderInfo.Case_Code;
                    //return PartialView("~/Areas/TradeMark/Views/PLB01_SDD_3B/_Partial_TM_3B_PLB_01_SDD_View.cshtml");
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
                    _casecode = applicationHeaderInfo.Case_Code;
                    //  return PartialView("~/Areas/TradeMark/Views/PLB02_CGD_3C/_Partial_TM_3C_PLB_02_SDD_View.cshtml");
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
                    _casecode = applicationHeaderInfo.Case_Code;
                    //   return PartialView("~/Areas/TradeMark/Views/PLC05_KN_3D/_Partial_TM_3D_PLC_05_KN_View.cshtml");
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
                    _casecode = applicationHeaderInfo.Case_Code;
                    // return PartialView("~/Areas/TradeMark/Views/PLD01_HDCN_4C2/_Partial_TM_4C2_PLD_01_HDCN_View.cshtml");
                }
                else if (pAppCode == TradeMarkAppCode.AppCodeDangKyQuocTeNH)
                {
                    var objBL = new AppDetail06DKQT_BL();
                    string language = AppsCommon.GetCurrentLang();
                    var ds06Dkqt = objBL.AppTM06DKQTGetByID(pAppHeaderId, language, pStatus);
                    if (ds06Dkqt != null && ds06Dkqt.Tables.Count == 3)
                    {
                        App_Detail_TM06DKQT_Info applicationHeaderInfo = CBO<App_Detail_TM06DKQT_Info>.FillObjectFromDataTable(ds06Dkqt.Tables[0]);
                        ViewBag.objAppHeaderInfo = applicationHeaderInfo;
                        ViewBag.Lst_AppDoc = CBO<AppDocumentInfo>.FillCollectionFromDataTable(ds06Dkqt.Tables[1]);
                        ViewBag.lstClassDetailInfo = CBO<AppClassDetailInfo>.FillCollectionFromDataTable(ds06Dkqt.Tables[2]);
                        _casecode = applicationHeaderInfo.Case_Code;
                    }
                    //  return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistration01/_PartalViewDangKyNhanHieu.cshtml");
                }

                #region  lấy dữ liệu lịch sử giao dịch

                B_Todos_BL _B_Todos_BL = new B_Todos_BL();
                List<B_Remind_Info> _ListRemind = new List<B_Remind_Info>();
                List<B_Todos_Info> _Listtodo = _B_Todos_BL.NotifiGetByCasecode(_casecode, ref _ListRemind);
                ViewBag.ListTodo = _Listtodo;
                ViewBag.ListRemind = _ListRemind;

                #endregion
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return View("/Areas/TradeMark/Views/Shared/AppDetail/AppDetails.cshtml");
        }

        [Route("todo-details/{id}/{id1}")]
        public ActionResult Todo_Details()
        {
            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("/");

                string p_case_code = "";
                if (RouteData.Values.ContainsKey("id"))
                {
                    p_case_code = RouteData.Values["id"].ToString();
                }

                // action là view hay sửa
                decimal _operator_type = Convert.ToDecimal(Common.CommonData.CommonEnums.Operator_Type.Update);
                if (RouteData.Values.ContainsKey("id1"))
                {
                    _operator_type = Convert.ToDecimal(Common.CommonData.CommonEnums.Operator_Type.View);
                }
                ViewBag.Operator_Type = _operator_type;

                Application_Header_BL _objBl = new Application_Header_BL();
                ApplicationHeaderInfo _ApplicationHeaderInfo = _objBl.GetApp_By_Case_Code(p_case_code, SessionData.CurrentUser.Username, AppsCommon.GetCurrentLang());

                int pStatus = (int)_ApplicationHeaderInfo.Status;
                string pAppCode = _ApplicationHeaderInfo.Appcode;
                SessionData.CurrentUser.chashFile.Clear();
                SessionData.CurrentUser.chashFileOther.Clear();

                ViewBag.Appcode = pAppCode;
                ViewBag.Currstatus = pStatus;
                ViewBag.objAppHeaderInfo = _ApplicationHeaderInfo;

                //  lấy dữ liệu lịch sử giao dịch
                B_Todos_BL _B_Todos_BL = new B_Todos_BL();
                List<B_Remind_Info> _ListRemind = new List<B_Remind_Info>();
                List<B_Todos_Info> _Listtodo = _B_Todos_BL.NotifiGetByCasecode(p_case_code, ref _ListRemind);
                ViewBag.ListTodo = _Listtodo;
                ViewBag.ListRemind = _ListRemind;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return View("/Areas/TradeMark/Views/Shared/AppDetail/AppDetails.cshtml");
        }


        /// <summary>
        /// truyền vào view name và model trả ra partial dạng sstring
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public string RenderPartialToString(string viewName, object model)
        {
            try
            {
                ViewData.Model = model;
                using (var sw = new StringWriter())
                {
                    var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                             viewName);
                    var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                                 ViewData, TempData, sw);
                    viewContext.HttpContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
                    viewResult.View.Render(viewContext, sw);
                    viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                    return sw.GetStringBuilder().ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return "";
            }
        }
    }
}
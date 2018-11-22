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
                _viewAppClass = RenderPartialToString("~/Areas/TradeMark/Views/TradeMarkRegistrationDKQT/_PartialTMAddAppClass.cshtml", null);
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
                    _operator_type = Convert.ToDecimal(RouteData.Values["id1"].ToString());
                }
                ViewBag.Operator_Type = _operator_type;


                List<B_Todos_Info> b_Todos_Infos = new List<B_Todos_Info>();
                List<Billing_Header_Info> billing_Header_Infos = new List<Billing_Header_Info>();
                List<Docking_Info> docking_Infos = new List<Docking_Info>();
                List<App_Notice_Info> app_Notice_Infos = new List<App_Notice_Info>();
                List<B_Remind_Info> b_Remind_Infos = new List<B_Remind_Info>();

                Application_Header_BL _objBl = new Application_Header_BL();
                ApplicationHeaderInfo _ApplicationHeaderInfo = _objBl.Get_Info_After_Filling(p_case_code, SessionData.CurrentUser.Username, AppsCommon.GetCurrentLang(),
                    ref b_Todos_Infos, ref billing_Header_Infos, ref docking_Infos, ref app_Notice_Infos, ref b_Remind_Infos);

                string pAppCode = _ApplicationHeaderInfo.Appcode;
                SessionData.CurrentUser.chashFile.Clear();
                SessionData.CurrentUser.chashFileOther.Clear();

                ViewBag.Appcode = pAppCode;
                ViewBag.Currstatus = (int)_ApplicationHeaderInfo.Status;
                ViewBag.objAppHeaderInfo = _ApplicationHeaderInfo;

                //  lấy dữ liệu lịch sử giao dịch
                //B_Todos_BL _B_Todos_BL = new B_Todos_BL();
                //List<B_Remind_Info> _ListRemind = new List<B_Remind_Info>();
                //List<B_Todos_Info> _Listtodo = _B_Todos_BL.NotifiGetByCasecode(p_case_code, ref _ListRemind);
                ViewBag.ListTodo = b_Todos_Infos;
                ViewBag.Billing_Data = billing_Header_Infos;
                ViewBag.Docking_Data = docking_Infos;
                ViewBag.Notice_Data = app_Notice_Infos;
                ViewBag.ListRemind = b_Remind_Infos;


                // sau advise filing
                if (_ApplicationHeaderInfo.Status >= (decimal)CommonEnums.App_Status.Customer_Review)
                {
                    // LẤY THÔNG TIN CỦA THẰNG NOTICE APP
                    App_Notice_Info_BL _notice_BL = new App_Notice_Info_BL();
                    App_Notice_Info _App_Notice_Info = _notice_BL.App_Notice_GetBy_CaseCode(p_case_code);
                    ViewBag.App_Notice_Info = _App_Notice_Info;

                    return View("/Areas/TradeMark/Views/Shared/AppDetail/AppDetails_After_Filing.cshtml");
                }
                else
                {
                    return View("/Areas/TradeMark/Views/Shared/AppDetail/AppDetails.cshtml");
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return View("/Areas/TradeMark/Views/Shared/AppDetail/AppDetails.cshtml");
            }
        }

        [Route("app-re-grant/{id}/{id1}/{id2}")]
        public ActionResult App_Re_Grant2Lawer()
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
                    _operator_type = Convert.ToDecimal(RouteData.Values["id1"].ToString());
                }
                ViewBag.Operator_Type = _operator_type;

                Application_Header_BL _objBl = new Application_Header_BL();
                ApplicationHeaderInfo _ApplicationHeaderInfo = _objBl.GetApp_By_Case_Code_Todo(p_case_code, SessionData.CurrentUser.Username, AppsCommon.GetCurrentLang());

                string pAppCode = _ApplicationHeaderInfo.Appcode;
                SessionData.CurrentUser.chashFile.Clear();
                SessionData.CurrentUser.chashFileOther.Clear();

                ViewBag.Appcode = pAppCode;
                ViewBag.Currstatus = (int)_ApplicationHeaderInfo.Status;
                ViewBag.objAppHeaderInfo = _ApplicationHeaderInfo;

                // ép cứng đến 1 trạng thái
                ViewBag.Re_Grant = 1;
                if (RouteData.Values.ContainsKey("id2"))
                {
                    ViewBag.Hard_Status = Convert.ToDecimal(RouteData.Values["id2"].ToString());
                }

                //  lấy dữ liệu lịch sử giao dịch
                B_Todos_BL _B_Todos_BL = new B_Todos_BL();
                List<B_Remind_Info> _ListRemind = new List<B_Remind_Info>();
                List<B_Todos_Info> _Listtodo = _B_Todos_BL.NotifiGetByCasecode(p_case_code, ref _ListRemind);
                ViewBag.ListTodo = _Listtodo;
                ViewBag.ListRemind = _ListRemind;

                // sau advise filing
                if (_ApplicationHeaderInfo.Status >= (decimal)CommonEnums.App_Status.Customer_Review)
                {
                    // LẤY THÔNG TIN CỦA THẰNG NOTICE APP
                    App_Notice_Info_BL _notice_BL = new App_Notice_Info_BL();
                    App_Notice_Info _App_Notice_Info = _notice_BL.App_Notice_GetBy_CaseCode(p_case_code);
                    ViewBag.App_Notice_Info = _App_Notice_Info;

                    return View("/Areas/TradeMark/Views/Shared/AppDetail/AppDetails_After_Filing.cshtml");
                }
                else
                {
                    return View("/Areas/TradeMark/Views/Shared/AppDetail/AppDetails.cshtml");
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return View("/Areas/TradeMark/Views/Shared/AppDetail/AppDetails.cshtml");
            }
        }

        [Route("app-notice-details/{id}")]
        public ActionResult App_Notice_Detail()
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

                // LẤY THÔNG TIN CỦA THẰNG NOTICE APP
                App_Notice_Info_BL _notice_BL = new App_Notice_Info_BL();
                App_Notice_Info _App_Notice_Info = _notice_BL.App_Notice_GetBy_CaseCode(p_case_code);
                ViewBag.App_Notice_Info = _App_Notice_Info;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return View("/Areas/TradeMark/Views/Application/After_Filing/ViewDetail_NoticeApp.cshtml");
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
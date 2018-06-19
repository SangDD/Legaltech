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
        [Route("get-tm04nh-info")]
        public ActionResult TM04NHGetInfo(decimal pAppHeaderId)
        {
            try
            {
                var objBL = new AppDetail04NHBL();
                string language = AppsCommon.GetCurrentLang();
                var ds04NH = objBL.AppTM04NHGetByID(pAppHeaderId, language, 1);// tạm thời truyền vào trạng thái = 1
                var _AppDetail04NHInfo = new AppDetail04NHInfo();
                if (ds04NH != null && ds04NH.Tables.Count == 5)
                {
                    _AppDetail04NHInfo  = CBO<AppDetail04NHInfo>.FillObjectFromDataTable(ds04NH.Tables[0]);
                    ViewBag.lstClassDetailInfo = CBO<AppClassDetailInfo>.FillCollectionFromDataTable(ds04NH.Tables[3]);
                }
                ViewBag.objAppHeaderInfo = _AppDetail04NHInfo;
                string _viewChuDon = "";
                string _viewDaiDienChuDon = "";
                string _viewAppClass = "";
                _viewChuDon = RenderPartialToString("~/Areas/TradeMark/Views/Shared/_PartialThongTinChuDon.cshtml", null);
                _viewDaiDienChuDon = RenderPartialToString("~/Areas/TradeMark/Views/Shared/_PartialThongTinDaiDienChuDon.cshtml", null);
                _viewAppClass = RenderPartialToString("~/Areas/TradeMark/Views/Shared/_PartialTMAddAppClass.cshtml", null);
                return Json(new { success = 0, NgayNopDon = _AppDetail04NHInfo.Ngaynopdon_Ut.ToDateStringN0(),
                    LogoURL = _AppDetail04NHInfo.pfileLogo,
                    ViewChuDon = _viewChuDon, ViewDaiDienChuDon = _viewDaiDienChuDon,
                    ViewAppClass = _viewAppClass
                });

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = -1 });
            }
            return Json(new { success = 0 });
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
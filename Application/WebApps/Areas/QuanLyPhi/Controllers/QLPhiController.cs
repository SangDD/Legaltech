using Common;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WebApps.AppStart;
using ObjectInfos;
using BussinessFacade.ModuleTrademark;
using WebApps.Session;

namespace WebApps.Areas.QuanLyPhi.Controllers
{
    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("QLPhi", AreaPrefix = "quan-ly-phi")]
    [Route("{action}")]
    public class QLPhiController : Controller
    {
        [HttpGet]
        [Route("danh-sach-phi")]
        public ActionResult GetListDSPhi()
        {
            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("~/home/index");
                var sysApplication = new SysApplicationBL();
                List<SysAppFixChargeInfo> lstFee = sysApplication.Sys_App_Fix_Charge_GetAll();
                ViewBag.listData = lstFee;
                return View("~/Areas/QuanLyPhi/Views/QLPhi/GetListDSPhi.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return View();
            }
        }


        [HttpPost]
        [Route("danh-sach-phi/search")]
        public ActionResult SearchDSPhi(string p_keysearch, int p_CurrentPage, string p_column, string p_type_sort)
        {
            try
            {
                string pTenDaidien = "";
                string pPhone = "";
                if (p_keysearch.Contains("|"))
                {
                    var array = p_keysearch.Split('|');
                    pTenDaidien = array[0];
                    pPhone = array[1];
                }
                decimal _total_record = 0;
                string p_to = "";
                string p_from = CommonFuc.Get_From_To_Page(p_CurrentPage, ref p_to);
                var sysApplication = new SysApplicationBL();
                List<SysAppFixChargeInfo> lstFee = sysApplication.Sys_App_Fix_Charge_GetAll();
                string htmlPaging = CommonFuc.Get_HtmlPaging<SysAppFixChargeInfo>((int)_total_record, 1, "Record");
                ViewBag.Paging = htmlPaging;
                ViewBag.Obj = lstFee;
                ViewBag.SumRecord = _total_record;
                return PartialView("~/Areas/QuanLyPhi/Views/QLPhi/_PartialTableDanhSachPhi.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/QuanLyPhi/Views/QLPhi/_PartialTableDanhSachPhi.cshtml");
            }
        }

        [HttpPost]
        [Route("danh-sach-phi/show-update")]
        public ActionResult GetView2Update(decimal pID,string pAppCode)
        {
            var sysApplication = new SysApplicationBL();
            SysAppFixChargeInfo pInfo = sysApplication.SysAppFixChargeById(pID, pAppCode);
            return PartialView("~/Areas/QuanLyPhi/Views/QLPhi/_PartialViewFeeFixUpdate.cshtml", pInfo);
        }


        [HttpPost]
        [Route("danh-sach-phi/execute-update")]
        public ActionResult ExecuteUpdate(SysAppFixChargeInfo pInfo)
        {
            try
            {
                if (pInfo == null) return Json(new { success = -3 });
                //pInfo.Modifiedby = SessionData.CurrentUser.Username;
                //pInfo.Modifieddate = CommonFuc.CurrentDate();
                var sysApplication = new SysApplicationBL();
                decimal presonse = sysApplication.SysAppFixChargeUpdate(pInfo.Id,pInfo.Appcode,pInfo.Amount,pInfo.Char01,pInfo.Description);
                //if (presonse >= 0)
                //{
                //    MemoryData.Enqueue_ChangeData(Table_Change.APP_DDSHCN);
                //}
                return Json(new { success = presonse });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = -3 });
            }
        }

    }
}
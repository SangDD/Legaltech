using BussinessFacade.ModuleTrademark;
using Common;
using ObjectInfos.ModuleTrademark;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApps.AppStart;
using WebApps.Session;

namespace WebApps.Areas.Manager.Controllers
{
    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("Manager", AreaPrefix = "quan-ly-fee-app")]
    [Route("{action}")]
    public class AppFeeController : Controller
    {

        [HttpGet]
        [Route("danh-sach-fee")]
        public ActionResult AppFee_Display()
        {
            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("/");

                return View("~/Areas/Manager/Views/AppFee/AppFee_Display.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return View();
            }
        }

        [HttpPost]
        [Route("danh-sach-app-fee/search")]
        public ActionResult Search_AppFee(string p_keysearch, int p_CurrentPage, string p_column, string p_type_sort)
        {
            try
            {
                decimal _total_record = 0;
                string p_to = "";
                string p_from = CommonFuc.Get_From_To_Page(p_CurrentPage, ref p_to);
                AppFeeFixBL _obj_bl = new AppFeeFixBL();
                List<AppFeeFixInfo> _lst = _obj_bl.AppFee_Search(p_keysearch, ref _total_record, p_from, p_to);
                string htmlPaging = WebApps.CommonFunction.AppsCommon.Get_HtmlPaging<AppFeeFixInfo>((int)_total_record, 1, "bản ghi");

                ViewBag.Paging = htmlPaging;
                ViewBag.Obj = _lst;
                ViewBag.SumRecord = _total_record;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return PartialView("~/Areas/Manager/Views/AppFee/_PartialTableAppFee.cshtml");
        }


        [Route("danh-sach-app-fee/show-edit")]
        public ActionResult GetViewEdit(decimal id, string case_code)
        {
            try
            {
                AppFeeFixBL _obj_bl = new AppFeeFixBL();
                AppFeeFixInfo _AppFeeFixInfo = _obj_bl.AppFee_GetById(id);
                ViewBag.AppFeeFixInfo = _AppFeeFixInfo;
                return PartialView("~/Areas/Manager/Views/AppFee/_PartialEdit.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/Manager/Views/AppFee/_PartialEdit.cshtml");
            }
        }

        [HttpPost]
        [Route("danh-sach-app-fee/do-edit")]
        public ActionResult Do_Edit(AppFeeFixInfo appFeeFixInfo)
        {
            try
            {
                AppFeeFixBL _obj_bl = new AppFeeFixBL();
                decimal _ck = _obj_bl.AppFee_UpdateById(appFeeFixInfo);
                return Json(new { success = _ck });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = -1 });
            }
        }

    }
}
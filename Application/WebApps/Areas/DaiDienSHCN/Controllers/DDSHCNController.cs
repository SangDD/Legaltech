using Common;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WebApps.AppStart;
using ObjectInfos;
using WebApps.Session;
using BussinessFacade;
using Common.CommonData;
using BussinessFacade.ModuleMemoryData;

namespace WebApps.Areas.DaiDienSHCN.Controllers
{
    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("DDSHCN", AreaPrefix = "dai-dien-so-huu-cn")]
    [Route("{action}")]
    public class DDSHCNController : Controller
    {
        // GET: DaiDienSHCN/DDSHCN

        [HttpGet]
        [Route("danh-sach-ddsh")]
        public ActionResult GetListDDSHCN()
        {
            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("/");
                decimal _total_record = 0;
                AppDDSHCN_BL _obj_bl = new AppDDSHCN_BL();
                List<AppDDSHCNInfo> _lst = _obj_bl.AppDDSHCNGetAll("","",0,0,ref _total_record);
                string htmlPaging = WebApps.CommonFunction.AppsCommon.Get_HtmlPaging<AppDDSHCNInfo>((int)_total_record, 1, "Record");
                ViewBag.Obj = _lst;
                ViewBag.Paging = htmlPaging;
                ViewBag.SumRecord = _total_record;
                return View("~/Areas/DaiDienSHCN/Views/DDSHCN/DDSHCNList.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return View();
            }
        }


        [HttpPost]
        [Route("danh-sach-ddsh/search")]
        public ActionResult SearchDDSHCN(string p_keysearch, int p_CurrentPage, string p_column, string p_type_sort)
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
                AppDDSHCN_BL _obj_bl = new AppDDSHCN_BL();
                List<AppDDSHCNInfo> _lst = _obj_bl.AppDDSHCNGetAll(pTenDaidien, pPhone, 0, 0, ref _total_record);
                string htmlPaging = WebApps.CommonFunction.AppsCommon.Get_HtmlPaging<Timesheet_Info>((int)_total_record, 1, "Record");
                ViewBag.Paging = htmlPaging;
                ViewBag.Obj = _lst;
                ViewBag.SumRecord = _total_record;
                return PartialView("~/Areas/DaiDienSHCN/Views/DDSHCN/_PartialTableDDSHCN.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/DaiDienSHCN/Views/DDSHCN/_PartialTableDDSHCN.cshtml");
            }
        }


        [HttpPost]
        [Route("danh-sach-ddsh/show-insert")]
        public ActionResult GetView2Insert()
        {
            return PartialView("~/Areas/DaiDienSHCN/Views/DDSHCN/_PartialDaiDienSHCNInsert.cshtml", new AppDDSHCNInfo());
        }

        [HttpPost]
        [Route("danh-sach-ddsh/show-update")]
        public ActionResult GetView2Update(decimal pID)
        {
            AppDDSHCN_BL _obj_bl = new AppDDSHCN_BL();
            AppDDSHCNInfo pInfo = _obj_bl.AppDDSHCNGetById(pID);
            return PartialView("~/Areas/DaiDienSHCN/Views/DDSHCN/_PartialDaiDienSHCNUpdate.cshtml", pInfo);
        }
        [HttpPost]
        [Route("danh-sach-ddsh/show-detail")]
        public ActionResult GetView2Detail(decimal pID)
        {
            try
            {
                if (pID == 0) return Json(new { success = -3 });
                AppDDSHCN_BL _obj_bl = new AppDDSHCN_BL();
                AppDDSHCNInfo pInfo = _obj_bl.AppDDSHCNGetById(pID);
                return PartialView("~/Areas/DaiDienSHCN/Views/DDSHCN/_PartialDaiDienSHCNDetail.cshtml", pInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = -3 });
            }

        }



        [HttpPost]
        [Route("danh-sach-ddsh/execute-insert")]
        public ActionResult ExecuteInsert(AppDDSHCNInfo pInfo)
        {
            try
            {
                if (pInfo == null) return Json(new { success = -3 });
                pInfo.Createdby = SessionData.CurrentUser.Username;
                pInfo.Createddate = CommonFuc.CurrentDate();
                AppDDSHCN_BL _obj_bl = new AppDDSHCN_BL();
                decimal presonse = _obj_bl.AppDDSHCNInsert(pInfo);
                if (presonse >= 0)
                {
                    MemoryData.GetCache_represent();
                }
                return Json(new { success = presonse });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = -3 });
            }
        }

        [HttpPost]
        [Route("danh-sach-ddsh/execute-update")]
        public ActionResult ExecuteUpdate(AppDDSHCNInfo pInfo)
        {
            try
            {
                if (pInfo == null) return Json(new { success = -3 });
                pInfo.Modifiedby = SessionData.CurrentUser.Username;
                pInfo.Modifieddate = CommonFuc.CurrentDate();
                AppDDSHCN_BL _obj_bl = new AppDDSHCN_BL();
                decimal presonse = _obj_bl.AppDDSHCNUpdate(pInfo);
                if (presonse >= 0)
                {
                    MemoryData.GetCache_represent();
                }
                return Json(new { success = presonse });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = -3 });
            }
        }

        [HttpPost]
        [Route("danh-sach-ddsh/execute-deleted")]
        public ActionResult ExecuteDelete(decimal pId)
        {
            try
            {
                AppDDSHCN_BL _obj_bl = new AppDDSHCN_BL();
                decimal presonse = _obj_bl.AppDDSHCNDeleted(pId, SessionData.CurrentUser.Username, CommonFuc.CurrentDate());
                if (presonse >= 0)
                {
                    MemoryData.GetCache_represent();
                }
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
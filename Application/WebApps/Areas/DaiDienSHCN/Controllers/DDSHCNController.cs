using Common;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WebApps.AppStart;
using ObjectInfos;
using WebApps.Session;
using BussinessFacade;
using Common.CommonData;


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
                return View();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return View();
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
            return PartialView("~/Areas/DaiDienSHCN/Views/DDSHCN/_PartialDaiDienSHCNUpdate.cshtml", new AppDDSHCNInfo());
        }
        [HttpPost]
        [Route("danh-sach-ddsh/show-detail")]
        public ActionResult GetView2Detail(decimal pID)
        {
            return PartialView("~/Areas/DaiDienSHCN/Views/DDSHCN/_PartialDaiDienSHCNDetail.cshtml", new AppDDSHCNInfo());
        }



        [HttpPost]
        [Route("danh-sach-ddsh/execute-insert")]
        public ActionResult ExecuteInsert(AppDDSHCNInfo pInfo)
        {
            return PartialView("~/Areas/TimeSheet/Views/TimeSheet/_PartialInsertTimeSheet.cshtml", new AppDDSHCNInfo());
        }

        [HttpPost]
        [Route("danh-sach-ddsh/execute-update")]
        public ActionResult ExecuteUpdate(AppDDSHCNInfo pInfo)
        {
            return PartialView("~/Areas/TimeSheet/Views/TimeSheet/_PartialInsertTimeSheet.cshtml", new AppDDSHCNInfo());
        }

        [HttpPost]
        [Route("danh-sach-ddsh/execute-deleted")]
        public ActionResult ExecuteUpdate(decimal pId)
        {
            return PartialView("~/Areas/TimeSheet/Views/TimeSheet/_PartialInsertTimeSheet.cshtml", new AppDDSHCNInfo());
        }


    }
}
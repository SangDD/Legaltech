using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BussinessFacade;
using Common;
using ObjectInfos;
using WebApps.AppStart;
using WebApps.Session;
using BussinessFacade.ModuleMemoryData;

namespace WebApps.Areas.AppClass.Controllers
{
    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("AppClass", AreaPrefix = "quan-ly-thong-tin")]
    [Route("{action}")]
    public class AppClassController : Controller
    {
        // GET: AppClass/AppClass

        [Route("hang-hoa-dich-vu/danh-sach-hang-hoa")]
        public ActionResult AppClassList()
        {
            List<App_Class_Info> lstObj = new List<App_Class_Info>();
            try
            {
                var ObjBL = new App_Class_BL();
                lstObj = ObjBL.SearchAppClass();
                ViewBag.Paging = ObjBL.GetPagingHtml();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return PartialView("/Areas/AppClass/Views/AppClass/AppClassList.cshtml", lstObj);

        }
        [HttpPost]
        [Route("hang-hoa-dich-vu/find-class")]
        public ActionResult FindOject(string keysSearch, string options)
        {
            var lstOjects = new List<App_Class_Info>();
            try
            {
                var _App_Class_BL = new App_Class_BL();
                 lstOjects = _App_Class_BL.SearchAppClass(keysSearch, options);
                ViewBag.Paging = _App_Class_BL.GetPagingHtml();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return PartialView("~/Areas/AppClass/Views/AppClass/_PartialAppClassData.cshtml", lstOjects);
        }

        [HttpPost]
        [Route("hang-hoa-dich-vu/add-new")]
        public ActionResult AddNew()
        {
            return PartialView("~/Areas/AppClass/Views/AppClass/_PartialAppClassAdd.cshtml");
        }

        [HttpPost]
        [Route("hang-hoa-dich-vu/do-add-class")]
        public ActionResult DoAddAppClass(App_Class_Info _AppClassInfo)
        {
            decimal _rel = 0;
            try
            {
                var _App_Class_BL = new App_Class_BL();
                _AppClassInfo.Created_By = SessionData.CurrentUser.Username;
                _AppClassInfo.Created_Date = DateTime.Now;
                _rel = _App_Class_BL.App_Class_Insert(_AppClassInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return Json(new { result = _rel});
        }

        [HttpPost]
        [Route("hang-hoa-dich-vu/view-edit")]
        public ActionResult GetViewToEditClass(decimal p_id)
        {
            var _appclassinfo = new App_Class_Info();
            var _App_Class_BL = new App_Class_BL();
            try
            {
                _appclassinfo = _App_Class_BL.AppClassGetByID(p_id);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return PartialView("~/Areas/AppClass/Views/AppClass/_PartialAppClassEdit.cshtml", _appclassinfo);
        }

        [HttpPost]
        [Route("hang-hoa-dich-vu/do-edit-class")]
        public ActionResult DoEditObject(App_Class_Info _AppClassInfo)
        {
            decimal _result = 0;
            try
            {
                var _App_Class_BL = new App_Class_BL();
                _AppClassInfo.Modified_By = SessionData.CurrentUser.Username;
                _AppClassInfo.Modified_Date = DateTime.Now;
                _result = _App_Class_BL.App_Class_Update(_AppClassInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return Json(new { result = _result });
        }

        [HttpPost]
        [Route("hang-hoa-dich-vu/do-delete-class")]
        public ActionResult DoDeleteUser(int p_id)
        {
            decimal _result = 0;
            var _App_Class_BL = new App_Class_BL();
            try
            {
                var modifiedBy = SessionData.CurrentUser.Username;
                _result = _App_Class_BL.App_Class_Delete(p_id, modifiedBy, DateTime.Now);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return Json(new { result = _result });
        }

        [HttpPost]
        [Route("hang-hoa-dich-vu/view-detail-class")]
        public ActionResult ViewDetailClass(decimal p_id)
        {
            var _App_Class_BL = new App_Class_BL();
            App_Class_Info _appclassinfo = new App_Class_Info();
            try
            {
                _appclassinfo = _App_Class_BL.AppClassGetByID(p_id);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return PartialView("~/Areas/AppClass/Views/AppClass/_PartialDetailAppClass.cshtml", _appclassinfo);
        }

        [HttpPost]
        [Route("hang-hoa-dich-vu/combobox-search")]
        public ActionResult combobox(string p_search)
        {
            List<AppClassInfo> _listRel = new List<AppClassInfo>() ;
            p_search = p_search.ToUpper();
            try
            {
                 _listRel = MemoryData.clstAppClass.FindAll(x=>x.KeySearch.Contains(p_search));
                if (_listRel.Count > 100)
                {
                    // nhiều hơn 100 thằng thì cắt
                    _listRel.RemoveRange(10, _listRel.Count - 100);  
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return PartialView("~/Areas/TradeMark/Views/Shared/_PartialShowAppInfoSearch.cshtml", _listRel);
        }
    }
}
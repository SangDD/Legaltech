using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApps.AppStart;
using BussinessFacade;
using ObjectInfos;
using WebApps.Session;
using Common;
namespace WebApps.Areas.Wiki.Controllers
{


    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("Wiki", AreaPrefix = "Wiki-Manage")]
    [Route("{action}")]
    public class WikiDocController : Controller
    {

        [Route("wiki-doc/list")]
        public ActionResult WikiDocList()
        {
            List<WikiDoc_Info> lstObj = new List<WikiDoc_Info>();
            try
            {
                var ObjBL = new WikiDoc_BL();
                lstObj = ObjBL.WikiDoc_Search();
                ViewBag.Paging = ObjBL.GetPagingHtml();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return PartialView("/Areas/Wiki/Views/WikiDoc/DocList.cshtml", lstObj);

        }

         [HttpPost]
        [Route("wiki-doc/find-doc")]
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

       
        [Route("wiki-doc/add")]
        public ActionResult WikiDocAdd()
        {
            try
            {
                var _WikiCataBL = new WikiCatalogue_BL ();
                List<WikiCatalogues_Info> lstOjects = _WikiCataBL.WikiCatalogueGetAll();
                ViewBag.ListCata = lstOjects;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return PartialView("/Areas/Wiki/Views/WikiDoc/_PartialDocAdd.cshtml");

        }

        [HttpPost]
        [Route("wiki-doc/do-add-doc")]
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
        [Route("wiki-doc/view-edit")]
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
            return PartialView("~/Areas/Wiki/Views/WikiDoc/_PartialDocEdit.cshtml", _appclassinfo);
        }

        [HttpPost]
        [Route("wiki-doc/do-edit-doc")]
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
        [Route("wiki-doc/do-delete-doc")]
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
        [Route("wiki-doc/view-detail-doc")]
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

    }
}
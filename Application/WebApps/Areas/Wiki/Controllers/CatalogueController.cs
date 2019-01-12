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
using ObjectInfos.ModuleTrademark;
using WebApps.CommonFunction;
using System.Text;

namespace WebApps.Areas.Wiki.Controllers
{
    public class CatalogueController: WikiDocController
    {
        [Route("wiki-doc/catalogue-list")]
        public ActionResult CatalogueList()
        {
            if (SessionData.CurrentUser == null)
            {
               return Redirect("/acount/dang-xuat");
            }
            List<WikiCatalogues_Info> lstObj = new List<WikiCatalogues_Info>();
            try
            {
                var ObjBL = new WikiCatalogue_BL();
                lstObj = ObjBL.WikiCata_Search();
                ViewBag.Paging = ObjBL.GetPagingHtml();
                List<WikiCatalogues_Info> lstOjects = ObjBL.WikiCatalogueGetAll();
                ViewBag.ListCata = lstOjects;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return PartialView("/Areas/Wiki/Views/Catalogue/CatalogueList.cshtml", lstObj);

        }
        [HttpPost]
        [Route("wiki-doc/find-catalogue")]
        public ActionResult FindOjectCatalouge(string keysSearch, string options)
        {
            var lstOjects = new List<WikiCatalogues_Info>();
            try
            {
                var ObjBL = new WikiCatalogue_BL();
                lstOjects = ObjBL.WikiCata_Search(keysSearch, options);
                ViewBag.Paging = ObjBL.GetPagingHtml();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return PartialView("/Areas/Wiki/Views/Catalogue/_PartialCatalogueData.cshtml", lstOjects);
        }

        [HttpPost]
        [Route("wiki-doc/add-new-catalogue")]
        public ActionResult AddNew()
        {
           
            var ObjBL = new WikiCatalogue_BL();
            List<WikiCatalogues_Info> lstOjects = ObjBL.WikiCatalogueGetAll();
            ViewBag.ListCata = lstOjects;
            return PartialView("~/Areas/Wiki/Views/Catalogue/_PartialCatalogueAdd.cshtml");
        }

        [HttpPost]
        [Route("wiki-doc/do-add-catalogue")]
        public ActionResult DoAddCatalogue(WikiCatalogues_Info _ObjectInfo)
        {
            decimal _rel = 0;
            try
            {
                var _ObjBL = new WikiCatalogue_BL();
                _ObjectInfo.CREATED_BY = SessionData.CurrentUser.Username;
                _ObjectInfo.CREATED_DATE = DateTime.Now;
                _rel = _ObjBL.WikiCatalogue_Insert(_ObjectInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return Json(new { result = _rel });
        }

        [HttpPost]
        [Route("wiki-doc/view-edit")]
        public ActionResult GetViewToEditClass(decimal p_id)
        {
            var _ObjectInfo = new WikiCatalogues_Info();
            var _ObjBL = new WikiCatalogue_BL();
            try
            {
                _ObjectInfo = _ObjBL.WikiCatalogue_GetByID(p_id);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            List<WikiCatalogues_Info> lstOjects = _ObjBL.WikiCatalogueGetAll();
            ViewBag.ListCata = lstOjects;
            return PartialView("~/Areas/Wiki/Views/Catalogue/_PartialCatalogueEdit.cshtml", _ObjectInfo);
        }

        [HttpPost]
        [Route("wiki-doc/do-edit-catalogue")]
        public ActionResult DoEditObject(WikiCatalogues_Info _ObjectInfo)
        {
            decimal _result = 0;
            try
            {
                var _ObjBL = new WikiCatalogue_BL();
                _ObjectInfo.MODIFIED_BY = SessionData.CurrentUser.Username;
                _ObjectInfo.MODIFIED_DATE = DateTime.Now;
                _result = _ObjBL.WikiCatalogue_Update(_ObjectInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return Json(new { result = _result });
        }

        [HttpPost]
        [Route("wiki-doc/do-delete-catalogue")]
        public ActionResult DoDeleteCatalogue(int p_id)
        {
            decimal _result = 0;
            var _ObjBL = new WikiCatalogue_BL();
            try
            {
              //  var modifiedBy = SessionData.CurrentUser.Username;
                _result = _ObjBL.WikiCatalogue_Delete(p_id);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return Json(new { result = _result });
        }

        [HttpPost]
        [Route("wiki-doc/view-detail-catalogue")]
        public ActionResult ViewDetailCatalogue(decimal p_id)
        {
            var _ObjectInfo = new WikiCatalogues_Info();
            var _ObjBL = new WikiCatalogue_BL();
            try
            {
                _ObjectInfo = _ObjBL.WikiCatalogue_GetByID(p_id);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return PartialView("~/Areas/Wiki/Views/Catalogue/_PartialCatalogueDetail.cshtml", _ObjectInfo);
        }

    }
}
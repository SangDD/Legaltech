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
            if (SessionData.CurrentUser == null)
                return Redirect("/");
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
                if (SessionData.CurrentUser == null)
                    return Redirect("/");
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
        [Route("wiki-doc/add-doc")]
        public ActionResult DoAddDoc(WikiDoc_Info _objectInfo, List<AppDocumentInfo> pAppDocumentInfo)
        {
            decimal pReturn = 0;
            try
            {
                var _WikiDoc_BL = new WikiDoc_BL();
                _objectInfo.CREATED_BY = SessionData.CurrentUser.Username;
                _objectInfo.CREATED_DATE = DateTime.Now;
                if (pAppDocumentInfo != null)
                {
                    if (pAppDocumentInfo.Count > 0)
                    {
                        
                        foreach (var info in pAppDocumentInfo)
                        {
                            if (SessionData.CurrentUser.chashFile.ContainsKey(info.keyFileUpload))
                            {
                                HttpPostedFileBase pfiles = (HttpPostedFileBase)SessionData.CurrentUser.chashFile[info.keyFileUpload];
                                info.Filename = pfiles.FileName;
                                info.Url_Hardcopy = "/Content/Archive/" + AppUpload.Wiki + "/" + pfiles.FileName;
                                if (info.keyFileUpload == "WIKIADD_FILE_01")
                                {
                                    _objectInfo.FILE_URL01 = info.Url_Hardcopy;
                                }
                                if (info.keyFileUpload == "WIKIADD_FILE_02")
                                {
                                    _objectInfo.FILE_URL02 = info.Url_Hardcopy;
                                }
                                if (info.keyFileUpload == "WIKIADD_FILE_03")
                                {
                                    _objectInfo.FILE_URL03 = info.Url_Hardcopy;
                                }
                                // lấy xong thì xóa
                                SessionData.CurrentUser.chashFile.Remove(info.keyFileUpload);
                            }
                        }
                    }
                }
                pReturn = _WikiDoc_BL.WikiDoc_Insert(_objectInfo);
              
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return Json(new { status = pReturn });
        }

   
        [Route("wiki-doc/doc-edit/{id}/")]
        public ActionResult ViewEdit()
        {
            if (SessionData.CurrentUser == null)
                return Redirect("/");
            var _WikiDoc_BL = new WikiDoc_BL();
            WikiDoc_Info _ObjInfo = new WikiDoc_Info();
            decimal _docid = 0;
            if (RouteData.Values.ContainsKey("id"))
            {
                _docid = CommonFuc.ConvertToDecimal(RouteData.Values["id"]);
            }
            try
            {
                _ObjInfo = _WikiDoc_BL.WikiDoc_GetById(_docid);
                var _WikiCataBL = new WikiCatalogue_BL();
                List<WikiCatalogues_Info> lstOjects = _WikiCataBL.WikiCatalogueGetAll();
                ViewBag.ListCata = lstOjects;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
 
            return PartialView("~/Areas/Wiki/Views/WikiDoc/_PartialDocEdit.cshtml", _ObjInfo);
        }

        [HttpPost]
        [ValidateInput(false)]
        [Route("wiki-doc/save-edit-doc")]
        public ActionResult DoEditDoc(WikiDoc_Info _objectInfo, List<AppDocumentInfo> pAppDocumentInfo)
        {
            decimal pReturn = 0;
            try
            {
                var _WikiDoc_BL = new WikiDoc_BL();
                _objectInfo.MODIFIED_BY = SessionData.CurrentUser.Username;
                _objectInfo.MODIFIED_DATE = DateTime.Now;
                if (pAppDocumentInfo != null)
                {
                    if (pAppDocumentInfo.Count > 0)
                    {                      
                        foreach (var info in pAppDocumentInfo)
                        {
                            if (SessionData.CurrentUser.chashFile.ContainsKey(info.keyFileUpload))
                            {
                                HttpPostedFileBase pfiles = (HttpPostedFileBase)SessionData.CurrentUser.chashFile[info.keyFileUpload];
                                info.Filename = pfiles.FileName;
                                info.Url_Hardcopy = "/Content/Archive/" + AppUpload.Wiki + "/" + pfiles.FileName;
                                if (info.keyFileUpload == "WIKIADD_FILE_01")
                                {
                                    _objectInfo.FILE_URL01 = info.Url_Hardcopy;
                                }
                                if (info.keyFileUpload == "WIKIADD_FILE_02")
                                {
                                    _objectInfo.FILE_URL02 = info.Url_Hardcopy;
                                }
                                if (info.keyFileUpload == "WIKIADD_FILE_03")
                                {
                                    _objectInfo.FILE_URL03 = info.Url_Hardcopy;
                                }
                                // lấy xong thì xóa
                                SessionData.CurrentUser.chashFile.Remove(info.keyFileUpload);
                            }
                        }
                    }
                }
                pReturn = _WikiDoc_BL.WikiDoc_Update(_objectInfo);

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return Json(new { status = pReturn });
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



        [HttpPost]
        [Route("wiki-doc/search-catalogue-child")]
        public ActionResult SearchChildCatalogue(string p_parentid)
        {
            var _App_Class_BL = new App_Class_BL();
            App_Class_Info _appclassinfo = new App_Class_Info();
            List<WikiCatalogues_Info> lstOjects = new List<WikiCatalogues_Info>();
            try
            {
                var _WikiCataBL = new WikiCatalogue_BL();
                 lstOjects = _WikiCataBL.WikiCatalogueGetAll();
                if (!string.IsNullOrEmpty(p_parentid))
                {
                    lstOjects = lstOjects.FindAll(m => m.PARENT_ID.ToString().Equals(p_parentid));
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return PartialView("~/Areas/Wiki/Views/Shared/_PartialCatalogue.cshtml", lstOjects);
        }

        [HttpPost]
        [Route("wiki-doc/push-file-to-server")]
        public ActionResult PushFileToServer(AppDocumentInfo pInfo)
        {
            try
            {
                if (pInfo.pfiles != null)
                {
                    var url = AppLoadHelpers.PushFileToServer(pInfo.pfiles, AppUpload.Wiki);
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

    }
}
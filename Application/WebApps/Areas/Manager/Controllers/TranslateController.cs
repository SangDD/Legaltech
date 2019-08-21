using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApps.AppStart;

using ObjectInfos;
using BussinessFacade;
using Common;
using WebApps.CommonFunction;
using Common.CommonData;
using WebApps.Session;
using BussinessFacade.ModuleTrademark;
using ObjectInfos.ModuleTrademark;
using System.Data;
using System.Transactions;

namespace WebApps.Areas.Manager.Controllers
{

    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("Manager", AreaPrefix = "app-translate")]
    [Route("{action}")]
    public class TranslateController : Controller
    {

        [HttpGet]
        [Route("translate-app/{id}/{id1}")]
        public ActionResult Translate_App()
        {
            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("/");

                SessionData.CurrentUser.chashFile.Clear();
                string pAppCode = "";
                if (RouteData.Values.ContainsKey("id"))
                {
                    pAppCode = RouteData.Values["id"].ToString().ToUpper();
                }
                ViewBag.AppCode = pAppCode;

                App_Translate_BL _App_Translate_BL = new App_Translate_BL();
                List<Sys_App_Translate_Info> _lst = _App_Translate_BL.Sys_App_Translate_GetBy_Casecode(pAppCode);
                foreach (var item in _lst)
                {
                    item.TxtId = "txt" + item.Object_Name;
                }
                ViewBag.LstTranslate = _lst;


                decimal pAppHeaderId = 0;
                if (RouteData.Values.ContainsKey("id1"))
                {
                    pAppHeaderId = Convert.ToDecimal(RouteData.Values["id1"].ToString());
                }

                ViewBag.App_Header_Id = pAppHeaderId;

                List<App_Translate_Info> _lst_translate = _App_Translate_BL.App_Translate_GetBy_AppId(pAppHeaderId);
                ViewBag.Lst_Translate_App = _lst_translate;


                DataSet _ds_detail = _App_Translate_BL.AppDetail_GetBy_Id(pAppCode, pAppHeaderId);
                ViewBag.Ds_detail = _ds_detail;

                if (_ds_detail != null && _ds_detail.Tables.Count > 0)
                {
                    ApplicationHeaderInfo applicationHeaderInfo = CBO<ApplicationHeaderInfo>.FillObjectFromDataTable(_ds_detail.Tables[0]);
                    ViewBag.objAppHeaderInfo = applicationHeaderInfo;
                }

                if (_ds_detail != null && _ds_detail.Tables.Count > 2)
                {
                    List<App_Class_Info>app_Class_Infos = CBO<App_Class_Info>.FillCollectionFromDataTable(_ds_detail.Tables[2]);
                    ViewBag.Lst_Class = app_Class_Infos;
                }

                return PartialView("~/Areas/Manager/Views/Translate/Translate_App.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return View();
            }
        }

        [HttpPost]
        [Route("do-translate")]
        public ActionResult Register(List<App_Translate_Info> pApp_Translate_Info)
        {
            try
            {
                if (pApp_Translate_Info.Count == 0)
                {
                    return Json(new { status = -2 });
                }
                App_Translate_BL _App_Translate_BL = new App_Translate_BL();
                decimal pReturn = 0;

                using (var scope = new TransactionScope())
                {
                    pReturn = _App_Translate_BL.App_Translate_Delete_ByAppId(pApp_Translate_Info[0].App_Header_Id);
                    if (pReturn < 0)
                        goto Commit_Transaction;

                    pReturn = _App_Translate_BL.App_Translate_Insert(pApp_Translate_Info);

                    Commit_Transaction:
                    if (pReturn < 0)
                    {
                        Transaction.Current.Rollback();
                    }
                    else
                    {
                        scope.Complete();
                    }
                }
                return Json(new { status = pReturn });

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { status = ErrorCode.Error });
            }
        }
    }
}
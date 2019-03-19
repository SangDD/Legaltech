using Common;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WebApps.AppStart;
using ObjectInfos;
using BussinessFacade.ModuleTrademark;
using WebApps.Session;
using BussinessFacade;
using BussinessFacade.ModuleMemoryData;
using System.Web;
using WebApps.CommonFunction;

namespace WebApps.Areas.Sys_Pages.Controllers
{
    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("trang-bia", AreaPrefix = "quan-ly-trang-bia")]
    [Route("{action}")]
    public class SysPagesController : Controller
    {

        [Route("danh-sach")]
        public ActionResult Sys_Pages_Display()
        {
            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("/");
                 
                decimal _total_record = 0;
                Sys_Pages_BL objBL = new Sys_Pages_BL();
                string _keySearch = "ALL";
                List<Sys_Pages_Info> _lst = objBL.Sys_Pages_Search(_keySearch, ref _total_record);
                string htmlPaging = CommonFuc.Get_HtmlPaging<Sys_Pages_Info>((int)_total_record, 1, "bản ghi");
                ViewBag.listArticles = _lst;
                ViewBag.Paging = htmlPaging;
                ViewBag.SumRecord = _total_record;
                return View("~/Areas/Sys_Pages/Views/SysPages/Sys_Pages_Display.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return View();
            }
        }

        [HttpGet]
        [Route("tim-kiem")]
        public ActionResult Search_Pages(string p_keySearch, int pPage)
        {
            try
            {
                int from = (pPage - 1) * (Common.Common.RecordOnpage);
                int to = (pPage) * (Common.Common.RecordOnpage);
                decimal _total_record = 0;
                Sys_Pages_BL objBL = new Sys_Pages_BL();
                string language = AppsCommon.GetCurrentLang();
                List<Sys_Pages_Info> _lst = objBL.Sys_Pages_Search(p_keySearch, ref _total_record, from.ToString(), to.ToString());
                string htmlPaging = CommonFuc.Get_HtmlPaging<NewsInfo>((int)_total_record, pPage, "Tin");
                ViewBag.listArticles = _lst;
                ViewBag.Paging = htmlPaging;
                ViewBag.SumRecord = _total_record;

                return View("~/Areas/Sys_Pages/Views/SysPages/_PartialView_List.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return View();
            }
        }

        [HttpGet]
        [Route("chi-tiet-bai-viet/{id}")]
        public ActionResult Detail_Page()
        {
            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("/");
                decimal _id = 0;
                if (RouteData.Values.ContainsKey("id"))
                {
                    _id = CommonFuc.ConvertToDecimal(RouteData.Values["id"]);
                }
               
                var objNewsBL = new Sys_Pages_BL();
                var objPage = objNewsBL.Sys_Pages_GetById(_id);
                return View("~/Areas/Sys_Pages/Views/SysPages/_PartialviewView.cshtml", objPage);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return View();
            }
        }

        [HttpGet]
        [Route("soan-bai-viet")]
        public ActionResult Add_SysPages()
        {
            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("/");

                return View("~/Areas/Sys_Pages/Views/SysPages/_PartialViewAdd.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return View();
            }
        }

        [HttpPost]
        [Route("them-moi-bai-viet")]
        public ActionResult Insert_SysPages(Sys_Pages_Info p_Sys_Pages_Info)
        {
            try
            {
                decimal preturn = 0;
                if (p_Sys_Pages_Info == null)
                {
                    return Json(new { status = -99 });
                }
                string language = AppsCommon.GetCurrentLang();
                var CreatedBy = SessionData.CurrentUser.Username;
                var CreatedDate = CommonFuc.CurrentDate();
                if (p_Sys_Pages_Info.pfileLogo != null)
                {
                    p_Sys_Pages_Info.Imageheader = AppLoadHelpers.PushFileToServer(p_Sys_Pages_Info.pfileLogo, AppUpload.Logo);
                }
                p_Sys_Pages_Info.Created_By = CreatedBy;
                var objBL = new Sys_Pages_BL();
                preturn = objBL.Sys_Pages_Insert(p_Sys_Pages_Info);

                return Json(new { status = preturn });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return View();
            }
        }

        [HttpGet]
        [Route("xoa-bai-viet")]
        public ActionResult Del_Pages(decimal p_id)
        {
            try
            {
                decimal preturn = 0;
                if (SessionData.CurrentUser == null)
                    return Redirect("/");
                var objBL = new Sys_Pages_BL();
                Sys_Pages_Info _Info = new Sys_Pages_Info();
                var ModifiedBy = SessionData.CurrentUser.Username;
                _Info.Id = p_id;
                _Info.Modified_By = ModifiedBy;

                preturn = objBL.Sys_Pages_Deleted(_Info);
                return Json(new { status = preturn });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return View();
            }
        }

        [HttpGet]
        [Route("sua-bai-viet/{id}")]
        public ActionResult Edit_Page()
        {
            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("/");
                decimal _id = 0;
                if (RouteData.Values.ContainsKey("id"))
                {
                    _id = CommonFuc.ConvertToDecimal(RouteData.Values["id"]);
                }

                var objNewsBL = new Sys_Pages_BL();
                var objPage = objNewsBL.Sys_Pages_GetById(_id);
                return View("~/Areas/Sys_Pages/Views/SysPages/_PartialviewEdit.cshtml", objPage);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return View();
            }
        }


        [HttpPost]
        [Route("luu-sua-bai-viet")]
        public ActionResult Edit_Pages(Sys_Pages_Info p_Sys_Pages_Info)
        {
            try
            {
                if (p_Sys_Pages_Info == null)
                {
                    return Json(new { status = -99 });
                }

                string language = AppsCommon.GetCurrentLang();
                var ModifiedBy = SessionData.CurrentUser.Username;
                var ModifiedDate = CommonFuc.CurrentDate();
                if (p_Sys_Pages_Info.pfileLogo != null)
                {
                    p_Sys_Pages_Info.Imageheader = AppLoadHelpers.PushFileToServer(p_Sys_Pages_Info.pfileLogo, AppUpload.Logo);
                }
                p_Sys_Pages_Info.Modified_By = SessionData.CurrentUser.Username;
                var objBL = new Sys_Pages_BL();
                decimal preturn = objBL.Sys_Pages_Update(p_Sys_Pages_Info);

                return Json(new { status = preturn });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return View();
            }
        }
    }
}
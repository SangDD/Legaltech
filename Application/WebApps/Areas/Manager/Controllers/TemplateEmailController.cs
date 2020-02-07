using BussinessFacade;
using Common;
using ObjectInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApps.AppStart;
using WebApps.CommonFunction;
using WebApps.Session;

namespace WebApps.Areas.Manager.Controllers
{
    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("Manager", AreaPrefix = "quan-ly-template")]
    [Route("{action}")]
    public class TemplateEmailController : Controller
    {
        [HttpGet]
        [Route("danh-sach")]
        public ActionResult TemplateEmail_Display()
        {
            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("/");

                decimal _total_record = 0;
                Email_BL _obj_bl = new Email_BL();
                string _keySearch = "ALL|ALL|" + SessionData.CurrentUser.Type;
                List<Template_Email_Info> _lst = _obj_bl.Template_Email_Search(SessionData.CurrentUser.Username, _keySearch, ref _total_record);
                string htmlPaging = WebApps.CommonFunction.AppsCommon.Get_HtmlPaging<Template_Email_Info>((int)_total_record, 1, "mẫu");

                ViewBag.Obj = _lst;
                ViewBag.Paging = htmlPaging;
                ViewBag.SumRecord = _total_record;

                return View("~/Areas/Manager/Views/TemplateEmail/TemplateEmail_Display.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return View();
            }
        }

        [HttpPost]
        [Route("danh-sach/search")]
        public ActionResult Search_Email(string p_keysearch, int p_CurrentPage, string p_column, string p_type_sort)
        {
            try
            {
                decimal _total_record = 0;
                string p_to = "";
                string p_from = CommonFuc.Get_From_To_Page(p_CurrentPage, ref p_to);
                Email_BL _obj_bl = new Email_BL();
                List<Template_Email_Info> _lst = _obj_bl.Template_Email_Search(SessionData.CurrentUser.Username, p_keysearch + "|" + SessionData.CurrentUser.Type, ref _total_record, p_from, p_to);
                string htmlPaging = WebApps.CommonFunction.AppsCommon.Get_HtmlPaging<Template_Email_Info>((int)_total_record, p_CurrentPage, "Email");

                ViewBag.Paging = htmlPaging;
                ViewBag.Obj = _lst;
                ViewBag.SumRecord = _total_record;
                return PartialView("~/Areas/Manager/Views/TemplateEmail/_PartialTableEmail.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/Manager/Views/TemplateEmail/_PartialTableEmail.cshtml");
            }
        }

        [Route("danh-sach/show-view")]
        public ActionResult GetView2View(decimal id)
        {
            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("/");

                Email_BL _obj_bl = new Email_BL();
                Template_Email_Info _Template_Email_Info = _obj_bl.Template_Email_GetBy_Id(id, AppsCommon.GetCurrentLang());
                ViewBag.Template_Email_Info = _Template_Email_Info;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return PartialView("~/Areas/Manager/Views/TemplateEmail/_PartialView.cshtml");
        }

        [HttpGet]
        [Route("danh-sach/them-moi")]
        public ActionResult ThemMoi_Mau()
        {
            if (SessionData.CurrentUser == null)
                return Redirect("/");

            return View("~/Areas/Manager/Views/TemplateEmail/_PartialInsert.cshtml");
        }

        [ValidateInput(false)]
        [HttpPost]
        [Route("do-insert")]
        public ActionResult do_Insert(Template_Email_Info pInfo)
        {
            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("/");

                Email_BL _obj_bl = new Email_BL();
                pInfo.Created_By = SessionData.CurrentUser.Username;
                decimal _re = _obj_bl.Template_Email_Insert(pInfo);

                return Json(new { success = _re });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = "-1" });
            }
        }
    }
}
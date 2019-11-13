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
    [RouteArea("Manager", AreaPrefix = "quan-ly-email")]
    [Route("{action}")]
    public class EmailController : Controller
    {
        [HttpGet]
        [Route("danh-sach-email")]
        public ActionResult Email_Display()
        {
            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("/");

                decimal _total_record = 0;
                Email_BL _obj_bl = new Email_BL();
                string _keySearch = "ALL" + "|" + "ALL" + "|" + "ALL" + "|" +  SessionData.CurrentUser.Type;
                List<Email_Info> _lst = _obj_bl.Email_Search(SessionData.CurrentUser.Username, _keySearch, ref _total_record);
                string htmlPaging = WebApps.CommonFunction.AppsCommon.Get_HtmlPaging<Email_Info>((int)_total_record, 1, "Billing");

                ViewBag.Obj = _lst;
                ViewBag.Paging = htmlPaging;
                ViewBag.SumRecord = _total_record;

                return View("~/Areas/Manager/Views/Email/Email_Display.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return View();
            }
        }

        [HttpPost]
        [Route("danh-sach-email/search")]
        public ActionResult Search_Email(string p_keysearch, int p_CurrentPage, string p_column, string p_type_sort)
        {
            try
            {
                decimal _total_record = 0;
                string p_to = "";
                string p_from = CommonFuc.Get_From_To_Page(p_CurrentPage, ref p_to);
                Email_BL _obj_bl = new Email_BL();
                List<Email_Info> _lst = _obj_bl.Email_Search(SessionData.CurrentUser.Username, p_keysearch + "|" + SessionData.CurrentUser.Type, ref _total_record, p_from, p_to);
                string htmlPaging = WebApps.CommonFunction.AppsCommon.Get_HtmlPaging<Email_Info>((int)_total_record, 1, "Email");

                ViewBag.Paging = htmlPaging;
                ViewBag.Obj = _lst;
                ViewBag.SumRecord = _total_record;
                return PartialView("~/Areas/Manager/Views/Email/_PartialTableEmail.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/Manager/Views/Email/_PartialTableEmail.cshtml");
            }
        }

        [Route("danh-sach-email/show-view")]
        public ActionResult GetView2View(decimal id, string case_code)
        {
            try
            {
                Email_BL _obj_bl = new Email_BL();
                Email_Info _Email_Info = _obj_bl.Email_GetBy_Id(id, case_code, AppsCommon.GetCurrentLang());
                ViewBag.Email_Info = _Email_Info;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return PartialView("~/Areas/Manager/Views/Email/_PartialView.cshtml");
        }

        [HttpGet]
        [Route("gui-email")]
        public ActionResult Send_Email()
        {
            return View();
        }

        [HttpPost]
        [Route("do-send-email")]
        public ActionResult do_SendEmail(Email_Info pInfo)
        {
            try
            {
                List<string> _LstAttachment = new List<string>();
                if (pInfo.File_Attach_1 != null)
                {
                    var _url_File_Attach_1 = AppLoadHelpers.PushFileToServer(pInfo.File_Attach_1, AppUpload.App);
                    _LstAttachment.Add(System.Web.HttpContext.Current.Server.MapPath(_url_File_Attach_1));
                }

                if (pInfo.File_Attach_2 != null)
                {
                    var _url_File_Attach_2 = AppLoadHelpers.PushFileToServer(pInfo.File_Attach_2, AppUpload.App);
                    _LstAttachment.Add(System.Web.HttpContext.Current.Server.MapPath(_url_File_Attach_2));
                }

                pInfo.LstAttachment = _LstAttachment;

                if (pInfo.EmailFrom == "tuyen.le@pathlaw.net")
                {
                    pInfo.EmailFrom = EmailHelper.EmailOriginal.EMailFrom_Business;
                    pInfo.Pass = EmailHelper.EmailOriginal.PassWord_Business;
                    pInfo.Display_Name = EmailHelper.EmailOriginal.DisplayName_Business;
                }
                else  
                {
                    pInfo.EmailFrom = EmailHelper.EmailOriginal.EMailFrom;
                    pInfo.Pass = EmailHelper.EmailOriginal.PassWord;
                    pInfo.Display_Name = EmailHelper.EmailOriginal.DisplayName;
                }

                string _content = pInfo.Content.Replace("\n", "<br><br>");
                _content = AppsCommon.SetContentMailTemplate(_content, yourref: pInfo.Your_Ref, outref: pInfo.Out_Ref, 
                    dearname: pInfo.Customer_Name, p_namereply: pInfo.Sign, p_position_name: pInfo.Position);
                pInfo.Content = _content;

                CommonFunction.AppsCommon.EnqueueSendEmail(pInfo);

                return Json(new { success = 1 });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = "-1" });
            }
        }
    }
}
using Common;
using ObjectInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApps.AppStart;
using WebApps.CommonFunction;

namespace WebApps.Areas.Manager.Controllers
{
    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("Manager", AreaPrefix = "quan-ly-email")]
    [Route("{action}")]
    public class EmailController : Controller
    {
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

                string _content = pInfo.Content;// Replace("\n", "<br>");
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
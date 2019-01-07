using BussinessFacade.ModuleUsersAndRoles;
using Common;
using Common.Ultilities;
using ObjectInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApps.Session;

namespace AnThanh.Controllers
{
    [RouteArea("Account", AreaPrefix = "quen-mat-khau")]
    [Route("{action}")]
    public class ResetPasswordController : Controller
    {
        [Route("thong-bao")]
        public ActionResult ResetPasswordDisplay(string confirm)
        {
            try
            {
                confirm = confirm.Replace(" ","+");
                string decryptString = WebApps.CommonFunction.AppsCommon.DecryptString(confirm);

                string _p_date = decryptString.Split('|')[0];
                decimal _p_userid = Convert.ToDecimal(decryptString.Split('|')[1]);

                // kiểm tra hạn sử dụng của link;
           

                TimeSpan difference_day = new TimeSpan();
                DateTime x = Convert.ToDateTime(_p_date);
                difference_day = DateTime.Now - Convert.ToDateTime(_p_date);
                
                decimal _ck_minutes = 1;
                UserInfo _user = new UserInfo();
                UserBL _userBL = new UserBL();
                if (difference_day.Days > 0 || difference_day.Hours > 0 || difference_day.Minutes > 0)
                {
                    // quá hạn link
                    _ck_minutes = -1;
                }
                else
                {
                    _user = _userBL.GetUserById((int)_p_userid);
                }

                // nếu trong hạn sử dụng thì gửi đến form yêu cầu nhập mật khẩu mới
                ViewBag.User = _user;
                ViewBag.Minutes = _ck_minutes;
            }
            catch(Exception ex)
            {
                Logger.LogException(ex);
            }
            return View();
        }
        

        [Route("update-pass")]
        public ActionResult UpdatePass(decimal user_id, string user_name,string pass)
        {
            decimal _ck = -1;
            try
            {
                var userBL = new UserBL();
                int re = userBL.DoResetPass(user_name, Encription.EncryptAccountPassword(user_name, pass), pass, SessionData.CurrentUser.Username);
                return Json(new { success = re });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return Json(new { success = _ck });
        }
    }
}
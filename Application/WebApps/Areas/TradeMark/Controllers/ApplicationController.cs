using System;
using System.Web.Mvc;
using WebApps.AppStart;
using BussinessFacade.ModuleTrademark;
using ObjectInfos.ModuleTrademark;
using System.Collections.Generic;
using WebApps.CommonFunction;
using WebApps.Session;
using Common;
using ObjectInfos;
using Common.CommonData;
using BussinessFacade;

namespace WebApps.Areas.TradeMark.Controllers
{
    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("Application", AreaPrefix = "trade-mark-mana")]
    [Route("{action}")]
    public class ApplicationController : Controller
    {
        #region Quản lý đơn lưu tạm
        [HttpGet]
        [Route("quan-ly-don-luu-tam")]
        public ActionResult Application_Display()
        {
            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("~/home/index");

                decimal _total_record = 0;
                Application_Header_BL _obj_bl = new Application_Header_BL();
                string _keySearch = "ALL" + "|" + ((int)CommonEnums.App_Status.Luu_tam).ToString();
                List<ApplicationHeaderInfo> _lst = _obj_bl.ApplicationHeader_Search(_keySearch, ref _total_record);
                string htmlPaging = CommonFuc.Get_HtmlPaging<ApplicationHeaderInfo>((int)_total_record, 1, "Đơn");

                ViewBag.Obj = _lst;
                ViewBag.Paging = htmlPaging;
                ViewBag.SumRecord = _total_record;

                return View("~/Areas/TradeMark/Views/Application/DanhSach_DonLuuTam.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return View();
            }
        }

        [HttpPost]
        [Route("quan-ly-don-luu-tam/search")]
        public ActionResult Search_Application(string p_keysearch, int p_CurrentPage, string p_column, string p_type_sort)
        {
            try
            {
                decimal _total_record = 0;

                string p_to = "";
                string p_from = CommonFuc.Get_From_To_Page(p_CurrentPage, ref p_to);

                Application_Header_BL _obj_bl = new Application_Header_BL();
                List<ApplicationHeaderInfo> _lst = _obj_bl.ApplicationHeader_Search(p_keysearch, ref _total_record);
                string htmlPaging = CommonFuc.Get_HtmlPaging<ApplicationHeaderInfo>((int)_total_record, 1, "Đơn");

                ViewBag.Paging = htmlPaging;
                ViewBag.Obj = _lst;
                ViewBag.SumRecord = _total_record;
                return PartialView("~/Areas/TradeMark/Views/Application/_PartialTableApplication.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/TradeMark/Views/Application/_PartialTableApplication.cshtml");
            }
        }
        #endregion

        #region Quản lý đơn đã gửi
        [HttpGet]
        [Route("quan-ly-don-da-gui")]
        public ActionResult DanhSach_DonDaGui()
        {
            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("~/home/index");

                decimal _total_record = 0;
                Application_Header_BL _obj_bl = new Application_Header_BL();
                string _keySearch = "ALL" + "|" + ((int)CommonEnums.App_Status.DaGui_ChoPhanLoai).ToString();
                List<ApplicationHeaderInfo> _lst = _obj_bl.ApplicationHeader_Search(_keySearch, ref _total_record);
                string htmlPaging = CommonFuc.Get_HtmlPaging<ApplicationHeaderInfo>((int)_total_record, 1, "Đơn");

                ViewBag.Obj = _lst;
                ViewBag.Paging = htmlPaging;
                ViewBag.SumRecord = _total_record;

                return View("~/Areas/TradeMark/Views/Application/DanhSach_DonDaGui.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return View();
            }
        }

        [HttpPost]
        [Route("quan-ly-don-da-gui/search")]
        public ActionResult Search_DonDaGui(string p_keysearch, int p_CurrentPage, string p_column, string p_type_sort)
        {
            try
            {
                decimal _total_record = 0;

                string p_to = "";
                string p_from = CommonFuc.Get_From_To_Page(p_CurrentPage, ref p_to);

                Application_Header_BL _obj_bl = new Application_Header_BL();
                List<ApplicationHeaderInfo> _lst = _obj_bl.ApplicationHeader_Search(p_keysearch, ref _total_record);
                string htmlPaging = CommonFuc.Get_HtmlPaging<ApplicationHeaderInfo>((int)_total_record, 1, "Đơn");

                ViewBag.Paging = htmlPaging;
                ViewBag.Obj = _lst;
                ViewBag.SumRecord = _total_record;
                return PartialView("~/Areas/TradeMark/Views/Application/_PartialTableApplication.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/TradeMark/Views/Application/_PartialTableApplication.cshtml");
            }
        }

        [HttpPost]
        [Route("quan-ly-don-da-gui/show-phan-loai")]
        public ActionResult GetViewToPhanLoai(decimal p_application_header_id)
        {
            ViewBag.Application_Header_Id = p_application_header_id;
            return PartialView("~/Areas/TradeMark/Views/Application/_PartialPhanLoai.cshtml");
        } 

        [HttpPost]
        [Route("quan-ly-don-da-gui/do-phan-loai")]
        public ActionResult DoAddAppLawer(App_Lawer_Info p_App_Lawer_Info)
        {
            try
            {
                p_App_Lawer_Info.Language_Code = AppsCommon.GetCurrentLang();
                p_App_Lawer_Info.Created_By = SessionData.CurrentUser.Username;
                p_App_Lawer_Info.Created_Date = DateTime.Now;
                App_Lawer_BL _con = new App_Lawer_BL();
                decimal _ck = _con.App_Lawer_Insert(p_App_Lawer_Info);
                return Json(new { success = _ck });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = "-1" });
            }
        }

        #endregion

        #region đơn chờ luật sư xử lý
        [HttpGet]
        [Route("quan-ly-don-cho-luat-su-xu-ly")]
        public ActionResult DanhSach_DonChoLuatXuPhanHoi()
        {
            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("~/home/index");

                decimal _total_record = 0;
                Application_Header_BL _obj_bl = new Application_Header_BL();
                string _keySearch = "ALL" + "|" + ((int)CommonEnums.App_Status.DaPhanChoLuatSu).ToString();
                List<ApplicationHeaderInfo> _lst = _obj_bl.ApplicationHeader_Search(_keySearch, ref _total_record);
                string htmlPaging = CommonFuc.Get_HtmlPaging<ApplicationHeaderInfo>((int)_total_record, 1, "Đơn");

                ViewBag.Obj = _lst;
                ViewBag.Paging = htmlPaging;
                ViewBag.SumRecord = _total_record;

                return View("~/Areas/TradeMark/Views/Application/DanhSach_DonChoLuatSuPhanHoi.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return View();
            }
        }

        [HttpPost]
        [Route("quan-ly-don-cho-luat-su-xu-ly/search")]
        public ActionResult Search_DonChoLuatSuPhanHoi(string p_keysearch, int p_CurrentPage, string p_column, string p_type_sort)
        {
            try
            {
                decimal _total_record = 0;

                string p_to = "";
                string p_from = CommonFuc.Get_From_To_Page(p_CurrentPage, ref p_to);

                Application_Header_BL _obj_bl = new Application_Header_BL();
                List<ApplicationHeaderInfo> _lst = _obj_bl.ApplicationHeader_Search(p_keysearch, ref _total_record);
                string htmlPaging = CommonFuc.Get_HtmlPaging<ApplicationHeaderInfo>((int)_total_record, 1, "Đơn");

                ViewBag.Paging = htmlPaging;
                ViewBag.Obj = _lst;
                ViewBag.SumRecord = _total_record;
                return PartialView("~/Areas/TradeMark/Views/Application/_PartialTableApplication.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/TradeMark/Views/Application/_PartialTableApplication.cshtml");
            }
        }

        #endregion

        #region đơn chờ KH confirm
        [HttpGet]
        [Route("quan-ly-don-cho-kh-xac-nhan")]
        public ActionResult DanhSach_DonChoXacNhanKH()
        {
            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("~/home/index");

                decimal _total_record = 0;
                Application_Header_BL _obj_bl = new Application_Header_BL();
                string _keySearch = "ALL" + "|" + ((int)CommonEnums.App_Status.ChoKHConfirm).ToString();
                List<ApplicationHeaderInfo> _lst = _obj_bl.ApplicationHeader_Search(_keySearch, ref _total_record);
                string htmlPaging = CommonFuc.Get_HtmlPaging<ApplicationHeaderInfo>((int)_total_record, 1, "Đơn");

                ViewBag.Obj = _lst;
                ViewBag.Paging = htmlPaging;
                ViewBag.SumRecord = _total_record;

                return View("~/Areas/TradeMark/Views/Application/DanhSach_DonChoKHConfirm.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return View();
            }
        }

        [HttpPost]
        [Route("quan-ly-don-cho-kh-xac-nhan/search")]
        public ActionResult Search_Don_ChoKhConfirm(string p_keysearch, int p_CurrentPage, string p_column, string p_type_sort)
        {
            try
            {
                decimal _total_record = 0;

                string p_to = "";
                string p_from = CommonFuc.Get_From_To_Page(p_CurrentPage, ref p_to);

                Application_Header_BL _obj_bl = new Application_Header_BL();
                List<ApplicationHeaderInfo> _lst = _obj_bl.ApplicationHeader_Search(p_keysearch, ref _total_record);
                string htmlPaging = CommonFuc.Get_HtmlPaging<ApplicationHeaderInfo>((int)_total_record, 1, "Đơn");

                ViewBag.Paging = htmlPaging;
                ViewBag.Obj = _lst;
                ViewBag.SumRecord = _total_record;
                return PartialView("~/Areas/TradeMark/Views/Application/_PartialTableApplication.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/TradeMark/Views/Application/_PartialTableApplication.cshtml");
            }
        }

        [HttpPost]
        [Route("quan-ly-don-cho-kh-xac-nhan/show-confirm")]
        public ActionResult GetViewToKHConfirm(decimal p_application_header_id)
        {
            ViewBag.Application_Header_Id = p_application_header_id;
            return PartialView("~/Areas/TradeMark/Views/Application/_PartialKH_Confirm.cshtml");
        }

        [HttpPost]
        [Route("quan-ly-don-cho-kh-xac-nhan/do-confirm")]
        public ActionResult DoKH_Confirm(decimal p_Application_Header_Id, decimal p_status, string p_note)
        {
            try
            {
                Application_Header_BL _obj_bl = new Application_Header_BL();
                decimal _status = (decimal)CommonEnums.App_Status.KhacHangDaConfirm;
                if (p_status == 0)
                    _status = (decimal)CommonEnums.App_Status.KhacHangDaTuChoi;

                int _ck = _obj_bl.AppHeader_Update_Status(p_Application_Header_Id, _status, p_note, SessionData.CurrentUser.Username, DateTime.Now);
                return Json(new { success = _ck });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = "-1" });
            }
        } 
        #endregion
    }
}
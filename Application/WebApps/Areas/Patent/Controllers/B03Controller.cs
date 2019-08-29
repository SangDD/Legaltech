using BussinessFacade;
using BussinessFacade.ModuleTrademark;
using BussinessFacade.Patent;
using Common;
using ObjectInfos;
using ObjectInfos.ModuleTrademark;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using WebApps.AppStart;
using WebApps.CommonFunction;
using WebApps.Session;

namespace WebApps.Areas.Patent.Controllers
{
    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("PatentRegistration", AreaPrefix = "lg-patentB03")]
    [Route("{action}")]
    public class B03Controller : Controller
    {
        [HttpGet]
        [Route("register/{id}")]
        public ActionResult B03Display()
        {
            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("/");

                SessionData.CurrentUser.chashFile.Clear();
                string AppCode = "";
                if (RouteData.Values.ContainsKey("id"))
                {
                    AppCode = RouteData.Values["id"].ToString().ToUpper();
                }
                ViewBag.AppCode = AppCode;
                return PartialView("~/Areas/Patent/Views/B03/B03Display.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/Patent/Views/B03/B03Display.cshtml");
            }
        }
        [HttpPost]
        [Route("register")]
        public ActionResult Register(ApplicationHeaderInfo pInfo, B03_Info pDetail,
           List<AppDocumentInfo> pAppDocumentInfo, List<AppFeeFixInfo> pFeeFixInfo,
           List<AppDocumentOthersInfo> pLstImagePublic)
        {

            Application_Header_BL objBL = new Application_Header_BL();
            AppFeeFixBL objFeeFixBL = new AppFeeFixBL();
            B03_BL objDetail = new B03_BL();
            AppDocumentBL objDoc = new AppDocumentBL();
            if (pInfo == null || pDetail == null) return Json(new { status = ErrorCode.Error });
            string language = AppsCommon.GetCurrentLang();

            var CreatedBy = SessionData.CurrentUser.Username;

            var CreatedDate = SessionData.CurrentUser.CurrentDate;
            decimal pReturn = ErrorCode.Success;
            int pAppHeaderID = 0;
            string p_case_code = "";

            using (var scope = new TransactionScope())
            {
                pInfo.Languague_Code = language;
                if (pInfo.Created_By == null || pInfo.Created_By == "0" || pInfo.Created_By == "")
                {
                    pInfo.Created_By = CreatedBy;
                }

                pInfo.Created_Date = CreatedDate;
                pInfo.Send_Date = DateTime.Now;
                pAppHeaderID = objBL.AppHeaderInsert(pInfo, ref p_case_code);
                if (pReturn < 0)
                    goto Commit_Transaction;

                // detail
                if (pAppHeaderID >= 0)
                {
                    pDetail.AppCode = pInfo.Appcode;
                    pDetail.Language_Code = language;
                    pDetail.App_Header_Id = pAppHeaderID;
                    pDetail.Case_Code = p_case_code;
                    // thiếu thông tin chủ đơn
                    // thiếu mã đơn

                    pReturn = objDetail.Insert(pDetail);
                    if (pReturn <= 0)
                        goto Commit_Transaction;
                }
                // hình công bố
                if (pReturn >= 0 && pLstImagePublic != null)
                {
                    if (pLstImagePublic.Count > 0)
                    {
                        int check = 0;
                        foreach (var info in pLstImagePublic)
                        {
                            if (SessionData.CurrentUser.chashFile.ContainsKey(info.keyFileUpload))
                            {
                                string _url = (string)SessionData.CurrentUser.chashFile[info.keyFileUpload];
                                info.Filename = _url;
                                check = 1;

                            }
                            info.App_Header_Id = pAppHeaderID;
                            info.Language_Code = language;
                        }
                        if (check == 1)
                        {
                            AppImageBL _AppImageBL = new AppImageBL();
                            pReturn = _AppImageBL.AppImageInsertBatch(pLstImagePublic);
                        }
                    }
                }

                #region Phí cố định
                List<AppFeeFixInfo> _lstFeeFix = Call_Fee.CallFee_B03(pDetail, pAppDocumentInfo, pLstImagePublic);
                if (_lstFeeFix.Count > 0)
                {
                    AppFeeFixBL _AppFeeFixBL = new AppFeeFixBL();
                    pReturn = _AppFeeFixBL.AppFeeFixInsertBath(_lstFeeFix, p_case_code);
                    if (pReturn < 0)
                        goto Commit_Transaction;
                }
                #endregion

                #region Tai lieu dinh kem 
                if (pReturn >= 0 && pAppDocumentInfo != null)
                {
                    if (pAppDocumentInfo.Count > 0)
                    {
                        foreach (var info in pAppDocumentInfo)
                        {
                            if (SessionData.CurrentUser.chashFile.ContainsKey(info.keyFileUpload))
                            {
                                string _url = (string)SessionData.CurrentUser.chashFile[info.keyFileUpload];
                                string[] _arr = _url.Split('/');
                                string _filename = WebApps.Resources.Resource.FileDinhKem;
                                if (_arr.Length > 0)
                                {
                                    _filename = _arr[_arr.Length - 1];
                                }

                                info.Filename = _filename;
                                info.Url_Hardcopy = _url;
                                info.Status = 0;
                            }
                            info.App_Header_Id = pAppHeaderID;
                            info.Document_Filing_Date = CommonFuc.CurrentDate();
                            info.Language_Code = language;
                        }
                        pReturn = objDoc.AppDocumentInsertBath(pAppDocumentInfo, pAppHeaderID);

                    }
                }
            #endregion


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
           

            return Json(new { status = 1 });
        }
    }
}
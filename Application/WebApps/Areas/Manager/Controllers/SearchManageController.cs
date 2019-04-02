using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApps.AppStart;
using WebApps.Session;
using ObjectInfos;
using BussinessFacade;
using WebApps.CommonFunction;
using ObjectInfos.ModuleTrademark;
using System.Transactions;
using BussinessFacade.ModuleMemoryData;
using GemBox.Document;
using BussinessFacade.ModuleUsersAndRoles;
using System.IO;

namespace WebApps.Areas.Manager.Controllers
{
    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("Manager", AreaPrefix = "quan-ly-search")]
    [Route("{action}")]
    public class SearchManageController : Controller
    {
        // GET: Manager/SearchManage

        [HttpPost]
        [Route("push-file-to-server")]
        public ActionResult PushFileToServer(AppDocumentInfo pInfo)
        {
            try
            {
                if (pInfo.pfiles != null)
                {
                    var url = AppLoadHelpers.PushFileToServer(pInfo.pfiles, AppUpload.Search);
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

        [HttpGet]
        [Route("danh-sach-search")]
        public ActionResult ListByStatus()
        {
            if (SessionData.CurrentUser == null)
            {
                return Redirect("/account/dang-xuat");
            }
            List<SearchObject_Header_Info> lstOjects = new List<SearchObject_Header_Info>();
            try
            {

                var _SearchObject_BL = new SearchObject_BL();
                string _key = "ALL|ALL|ALL|ALL|ALL" + "|" + SessionData.CurrentUser.Type.ToString() + "|" + SessionData.CurrentUser.Username;
                lstOjects = _SearchObject_BL.SEARCH_OBJECT_SEARCH(_key);
                ViewBag.Paging = _SearchObject_BL.GetPagingHtml();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return View(@"~\Areas\Manager\Views\SearchManage\ListSearch.cshtml", lstOjects);
        }


        [HttpPost]
        [Route("findobject-search")]
        public ActionResult FindsOnjectSearch(string keysSearch, string options)
        {
            List<SearchObject_Header_Info> lstOjects = new List<SearchObject_Header_Info>();
            try
            {

                var _SearchObject_BL = new SearchObject_BL();
                lstOjects = _SearchObject_BL.SEARCH_OBJECT_SEARCH(keysSearch + "|" + SessionData.CurrentUser.Type.ToString() + "|" + SessionData.CurrentUser.Username, options);
                ViewBag.Paging = _SearchObject_BL.GetPagingHtml();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }
            return PartialView(@"~\Areas\Manager\Views\SearchManage\_SearchData.cshtml", lstOjects);
        }


        [HttpGet]
        [Route("them-moi")]
        public ActionResult SearchAdd()
        {
            if (SessionData.CurrentUser == null)
            {
                return Redirect("/account/dang-xuat");
            }
            try
            {
                //List<SuggestInfo> _lst = new List<SuggestInfo>();
                //string _lang = AppsCommon.GetCurrentLang();
                //foreach (var item in BussinessFacade.ModuleMemoryData.MemoryData.clstAppClassSuggest)
                //{
                //    SuggestInfo _SuggestInfo = new SuggestInfo(item.name.Replace('/', ' '), item.value);
                //    if (_lang == "EN_US")
                //    {
                //        _SuggestInfo.label = item.label_en;
                //    }
                //    _lst.Add(_SuggestInfo);
                //}

                //ViewBag.Data_Object = _lst;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return View(@"~\Areas\Manager\Views\SearchManage\SearchAdd.cshtml");
        }

        [HttpGet]
        [Route("get-suggest-source")]
        public ActionResult Get_Suggest_Source()
        {
            List<SuggestInfo> _lst = new List<SuggestInfo>();
            foreach (var item in BussinessFacade.ModuleMemoryData.MemoryData.clstAppClassSuggest)
            {
                SuggestInfo _SuggestInfo = new SuggestInfo(item.name.Replace('/', ' '), item.value);
                _lst.Add(_SuggestInfo);
            }
            string json_object = Newtonsoft.Json.JsonConvert.SerializeObject(_lst);
            return Json(json_object, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("them-dieu-kien")]
        public ActionResult AddNewDetail(decimal _idSearch)
        {

            try
            {

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return PartialView(@"~\Areas\Manager\Views\SearchManage\_PartialSearchCondition.cshtml", _idSearch.ToString() + "|0");
        }

        [HttpPost]
        [Route("SearchInsert")]
        public ActionResult SearchInsert(SearchObject_Header_Info p_searchHeaderInfo, List<SearchObject_Detail_Info> p_SearchObject_Detail_Info,
            SearchObject_Question_Info p_questionInfo, List<Search_Class_Info> pAppClassInfo)
        {
            decimal _rel = 0;
            try
            {
                using (var scope = new TransactionScope())
                {
                    SearchObject_BL _searchBL = new SearchObject_BL();
                    p_searchHeaderInfo.CREATED_BY = SessionData.CurrentUser.Username;
                    p_searchHeaderInfo.CREATED_DATE = DateTime.Now;
                    p_searchHeaderInfo.REQUEST_DATE = DateTime.Now;
                    p_searchHeaderInfo.LANGUAGE_CODE = AppsCommon.GetCurrentLang();

                    var url_File_Atachment = "";
                    if (p_searchHeaderInfo.Url_File_Up != null)
                    {
                        url_File_Atachment = AppLoadHelpers.PushFileToServer(p_searchHeaderInfo.Url_File_Up, AppUpload.Search);
                        p_searchHeaderInfo.Url_File = url_File_Atachment;
                    }

                    //HungTD: thêm up ảnh

                    if (p_searchHeaderInfo.Logochu != 1)
                    {
                        //pDetail.Logochu = 0;
                        if (p_searchHeaderInfo.pfileLogo != null)
                        {
                            p_searchHeaderInfo.Logourl = AppLoadHelpers.PushFileToServer(p_searchHeaderInfo.pfileLogo, AppUpload.FileAttact);
                        }
                    }
                    else
                    {
                        p_searchHeaderInfo.Logourl = p_searchHeaderInfo.ChuLogo;
                        p_searchHeaderInfo.Logochu = 1;
                    }


                    //END HungTD
                    _rel = _searchBL.SEARCH_HEADER_INSERT(p_searchHeaderInfo);
                    if (_rel < 0)
                    {
                        return Json(new { success = _rel });
                    }

                    p_searchHeaderInfo.SEARCH_ID = _rel;
                    p_questionInfo.SEARCH_ID = p_searchHeaderInfo.SEARCH_ID;



                    _rel = _searchBL.SEARCH_QUESTION_INSERT(p_questionInfo);
                    if (_rel < 0)
                        goto Commit_Transaction;

                    foreach (SearchObject_Detail_Info item in p_SearchObject_Detail_Info)
                    {
                        item.SEARCH_ID = p_searchHeaderInfo.SEARCH_ID;
                        item.SEARCH_OBJECT = p_searchHeaderInfo.Object_Search;
                    }
                    _rel = _searchBL.SEARCH_DETAIL_INSERT(p_SearchObject_Detail_Info);

                    if (_rel < 0)
                        goto Commit_Transaction;

                    //Thêm thông tin class
                    if (pAppClassInfo != null)
                    {
                        _rel = _searchBL.Search_Class_InsertBatch(pAppClassInfo, p_searchHeaderInfo.SEARCH_ID, AppsCommon.GetCurrentLang());
                    }
                    if (_rel < 0)
                        goto Commit_Transaction;

                    // thông tin thằng fee
                    List<Search_Fix_Info> _lstFee = new List<Search_Fix_Info>();
                    string _keyFee = "";
                    foreach (var item in p_SearchObject_Detail_Info)
                    {
                        Search_Fix_Info _Search_Fix_Info = new Search_Fix_Info();
                        _Search_Fix_Info.Search_Object = item.SEARCH_OBJECT;
                        _Search_Fix_Info.Search_Type = item.SEARCH_TYPE;
                        _Search_Fix_Info.Country_Id = p_searchHeaderInfo.Country_Id;

                        if (pAppClassInfo != null)
                            _Search_Fix_Info.Number_Of_Class = pAppClassInfo.Count;
                        else
                            _Search_Fix_Info.Number_Of_Class = 0;

                        _keyFee = p_searchHeaderInfo.Country_Id.ToString() + "_" + item.SEARCH_OBJECT.ToString() + "_" + item.SEARCH_TYPE.ToString();

                        if (MemoryData.c_dic_FeeBySearch.ContainsKey(_keyFee))
                        {

                            if (_Search_Fix_Info.Number_Of_Class == 0)
                            {
                                _Search_Fix_Info.Amount = MemoryData.c_dic_FeeBySearch[_keyFee].Amount;
                                _Search_Fix_Info.Amount_usd = MemoryData.c_dic_FeeBySearch[_keyFee].Amount_usd;
                            }
                            else
                            {
                                _Search_Fix_Info.Amount = MemoryData.c_dic_FeeBySearch[_keyFee].Amount * _Search_Fix_Info.Number_Of_Class;
                                _Search_Fix_Info.Amount_usd = MemoryData.c_dic_FeeBySearch[_keyFee].Amount_usd * _Search_Fix_Info.Number_Of_Class;
                            }
                        }
                        else
                            _Search_Fix_Info.Amount = 0;

                        _lstFee.Add(_Search_Fix_Info);
                    }

                    if (_lstFee.Count > 0)
                    {
                        _rel = _searchBL.Search_Fee_InsertBatch(_lstFee, p_searchHeaderInfo.SEARCH_ID, AppsCommon.GetCurrentLang());
                    }

                    //end
                    Commit_Transaction:
                    if (_rel < 0)
                    {
                        Transaction.Current.Rollback();
                    }
                    else
                    {
                        scope.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return Json(new { success = _rel });
        }

        [HttpGet]
        [Route("search-edit/{id}/{id2}")]
        public ActionResult SearchEdit()
        {
            if (SessionData.CurrentUser == null)
            {
                return Redirect("/account/dang-xuat");
            }
            try
            {
                SearchObject_BL _searchBL = new SearchObject_BL();
                SearchObject_Header_Info _HeaderInfo = new SearchObject_Header_Info();
                List<SearchObject_Detail_Info> _ListDetail = new List<SearchObject_Detail_Info>();
                SearchObject_Question_Info _QuestionInfo = new SearchObject_Question_Info();
                List<Search_Class_Info> search_Class_Infos = new List<Search_Class_Info>();
                decimal _Searchid = 0;
                if (RouteData.Values.ContainsKey("id"))
                {
                    _Searchid = Convert.ToDecimal(RouteData.Values["id"]);
                    _HeaderInfo = _searchBL.SEARCH_HEADER_GETBYID(_Searchid, ref _ListDetail, ref _QuestionInfo, ref search_Class_Infos);
                    ViewBag.SearchHeader = _HeaderInfo;
                    ViewBag.SearchListDetail = _ListDetail;
                    ViewBag.QuestionInfo = _QuestionInfo;
                    ViewBag.lstClassDetailInfo = search_Class_Infos;
                }
                int _Status = 0;
                if (RouteData.Values.ContainsKey("id2"))
                {
                    _Status = Convert.ToInt32(RouteData.Values["id2"]);
                    ViewBag.CurrStatus = _Status;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return View(@"~\Areas\Manager\Views\SearchManage\SearchEdit.cshtml");
        }

        [HttpPost]
        [Route("SearchEdit")]
        public ActionResult SearchEdit(SearchObject_Header_Info p_searchHeaderInfo, List<SearchObject_Detail_Info> p_SearchObject_Detail_Info, 
            SearchObject_Question_Info p_questionInfo, List<Search_Class_Info> pAppClassInfo)
        {
            decimal _rel = 0;
            try
            {
                using (var scope = new TransactionScope())
                {
                    SearchObject_BL _searchBL = new SearchObject_BL();
                    p_searchHeaderInfo.MODIFIED_BY = SessionData.CurrentUser.Username;
                    p_searchHeaderInfo.MODIFIED_DATE = DateTime.Now;
                    p_searchHeaderInfo.REQUEST_DATE = DateTime.Now;

                    var url_File_Atachment = "";
                    if (p_searchHeaderInfo.Url_File_Up != null)
                    {
                        url_File_Atachment = AppLoadHelpers.PushFileToServer(p_searchHeaderInfo.Url_File_Up, AppUpload.Search);
                        p_searchHeaderInfo.Url_File = url_File_Atachment;
                    }
                    else
                    {
                        p_searchHeaderInfo.Url_File = "NA";
                    }
                    //HungTD

                    if (p_searchHeaderInfo.pfileLogo != null)
                    {
                        p_searchHeaderInfo.Logourl = AppLoadHelpers.PushFileToServer(p_searchHeaderInfo.pfileLogo, AppUpload.Logo);
                    }
                    else
                    {
                        p_searchHeaderInfo.Logourl = p_searchHeaderInfo.LogourlOrg;
                    }
                    if (p_searchHeaderInfo.Logochu != 1)
                    {
                        //pDetail.Logochu = 0;
                        if (p_searchHeaderInfo.pfileLogo != null)
                        {
                            p_searchHeaderInfo.Logourl = AppLoadHelpers.PushFileToServer(p_searchHeaderInfo.pfileLogo, AppUpload.Logo);
                        }
                    }
                    else
                    {
                        p_searchHeaderInfo.Logourl = p_searchHeaderInfo.ChuLogo;
                        p_searchHeaderInfo.Logochu = 1;
                    }


                    //End HungTD


                    _rel = _searchBL.SEARCH_HEADER_UPDATE(p_searchHeaderInfo);
                    if (_rel < 0)
                    {
                        return Json(new { success = _rel });
                    }

                    p_questionInfo.SEARCH_ID = p_searchHeaderInfo.SEARCH_ID;
                    _rel = _searchBL.SEARCH_QUESTION_UPDATE(p_questionInfo);
                    if (_rel < 0)
                        goto Commit_Transaction;

                    // detail
                    foreach (SearchObject_Detail_Info item in p_SearchObject_Detail_Info)
                    {
                        item.SEARCH_ID = p_searchHeaderInfo.SEARCH_ID;
                        item.SEARCH_OBJECT = p_searchHeaderInfo.Object_Search;
                    }
                    _searchBL.SEARCH_DETAIL_DELETE(p_searchHeaderInfo.SEARCH_ID);
                    _rel = _searchBL.SEARCH_DETAIL_INSERT(p_SearchObject_Detail_Info);
                    if (_rel < 0)
                        goto Commit_Transaction;

                    //Thêm thông tin class
                    if (pAppClassInfo != null)
                    {
                        _rel = _searchBL.Search_Class_Delete(p_searchHeaderInfo.SEARCH_ID, AppsCommon.GetCurrentLang());

                        _rel = _searchBL.Search_Class_InsertBatch(pAppClassInfo, p_searchHeaderInfo.SEARCH_ID, AppsCommon.GetCurrentLang());
                    }

                    if (_rel < 0)
                        goto Commit_Transaction;

                    // thông tin thằng fee
                    List<Search_Fix_Info> _lstFee = new List<Search_Fix_Info>();
                    string _keyFee = "";

                    foreach (var item in p_SearchObject_Detail_Info)
                    {
                        Search_Fix_Info _Search_Fix_Info = new Search_Fix_Info();
                        _Search_Fix_Info.Search_Object = item.SEARCH_OBJECT;
                        _Search_Fix_Info.Search_Type = item.SEARCH_TYPE;
                        _Search_Fix_Info.Country_Id = p_searchHeaderInfo.Country_Id;
                        if (pAppClassInfo != null)
                            _Search_Fix_Info.Number_Of_Class = pAppClassInfo.Count;
                        else
                            _Search_Fix_Info.Number_Of_Class = 0;

                        _keyFee = p_searchHeaderInfo.Country_Id.ToString() + "_" + item.SEARCH_OBJECT.ToString() + "_" + item.SEARCH_TYPE.ToString();

                        if (MemoryData.c_dic_FeeBySearch.ContainsKey(_keyFee))
                        {

                            if (_Search_Fix_Info.Number_Of_Class == 0)
                            {
                                _Search_Fix_Info.Amount = MemoryData.c_dic_FeeBySearch[_keyFee].Amount;
                                _Search_Fix_Info.Amount_usd = MemoryData.c_dic_FeeBySearch[_keyFee].Amount_usd;
                            }
                            else
                            {
                                _Search_Fix_Info.Amount = MemoryData.c_dic_FeeBySearch[_keyFee].Amount * _Search_Fix_Info.Number_Of_Class;
                                _Search_Fix_Info.Amount_usd = MemoryData.c_dic_FeeBySearch[_keyFee].Amount_usd * _Search_Fix_Info.Number_Of_Class;
                            }
                        }
                        else
                            _Search_Fix_Info.Amount = 0;

                        _lstFee.Add(_Search_Fix_Info);
                    }

                    _rel = _searchBL.Search_Fee_Delete(p_searchHeaderInfo.SEARCH_ID, AppsCommon.GetCurrentLang());
                    if (_lstFee.Count > 0)
                    {
                        _rel = _searchBL.Search_Fee_InsertBatch(_lstFee, p_searchHeaderInfo.SEARCH_ID, AppsCommon.GetCurrentLang());
                    }

                    //end
                    Commit_Transaction:
                    if (_rel < 0)
                    {
                        Transaction.Current.Rollback();
                    }
                    else
                    {
                        scope.Complete();
                    }
                }
                return Json(new { success = _rel });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return Json(new { success = _rel });
        }

         


        [HttpGet]
        [Route("search-todo-detail/{id}/{id1}")]
        public ActionResult SearchShowTodo()
        {
            if (SessionData.CurrentUser == null)
            {
                return Redirect("/account/dang-xuat");
            }
            try
            {
                SearchObject_BL _searchBL = new SearchObject_BL();
                SearchObject_Header_Info _HeaderInfo = new SearchObject_Header_Info();
                List<SearchObject_Detail_Info> _ListDetail = new List<SearchObject_Detail_Info>();
                SearchObject_Question_Info _QuestionInfo = new SearchObject_Question_Info();
                List<Search_Class_Info> search_Class_Infos = new List<Search_Class_Info>();
                string _casecode = "";
                if (RouteData.Values.ContainsKey("id"))
                {
                    _casecode = RouteData.Values["id"].ToString();
                    _HeaderInfo = _searchBL.SEARCH_HEADER_GETBY_CASECODE(_casecode, ref _ListDetail, ref _QuestionInfo, ref search_Class_Infos);
                    ViewBag.SearchHeader = _HeaderInfo;
                    ViewBag.SearchListDetail = _ListDetail;
                    ViewBag.QuestionInfo = _QuestionInfo;

                    //  lấy dữ liệu lịch sử giao dịch
                    B_Todos_BL _B_Todos_BL = new B_Todos_BL();
                    List<B_Remind_Info> _ListRemind = new List<B_Remind_Info>();
                    List<B_Todos_Info> _Listtodo = _B_Todos_BL.NotifiGetByCasecode(_casecode, ref _ListRemind);
                    ViewBag.ListTodo = _Listtodo;
                    ViewBag.ListRemind = _ListRemind;
                    ViewBag.Currstatus = _HeaderInfo.STATUS;
                    ViewBag.lstClassDetailInfo = search_Class_Infos;

                    //action là view hay sửa
                    decimal _operator_type = Convert.ToDecimal(Common.CommonData.CommonEnums.Operator_Type.Update);
                    if (RouteData.Values.ContainsKey("id1"))
                    {
                        _operator_type = Convert.ToDecimal(RouteData.Values["id1"].ToString());
                    }
                    ViewBag.Operator_Type = _operator_type;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return View(@"~\Areas\Manager\Views\SearchManage\Search_Detail.cshtml");
        }

        [HttpPost]
        [Route("phan-loai-4lawer")]
        public ActionResult DoGrant4Lawer(string p_case_code, string p_lawer_id, string p_note)
        {
            try
            {
                SearchObject_BL _con = new SearchObject_BL();
                decimal _ck = _con.Update_Lawer(p_case_code, Convert.ToDecimal(p_lawer_id), p_note, AppsCommon.GetCurrentLang(), SessionData.CurrentUser.Username);
                return Json(new { success = _ck });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = "-1" });
            }
        }

        [HttpPost]
        [Route("admin-confirm-a")]
        public ActionResult DoAdminConfirm(string p_case_code, string p_note)
        {
            try
            {
                SearchObject_BL _con = new SearchObject_BL();
                decimal _ck = _con.Admin_Update(p_case_code, p_note, AppsCommon.GetCurrentLang(), SessionData.CurrentUser.Username);

                // nếu thành công thì gửi email cho khách hàng
                if (_ck != -1)
                {
                    // lấy thông tin search
                    SearchObject_BL _searchBL = new SearchObject_BL();
                    List<SearchObject_Detail_Info> _ListDetail = new List<SearchObject_Detail_Info>();
                    SearchObject_Question_Info _QuestionInfo = new SearchObject_Question_Info();
                    List<Search_Class_Info> search_Class_Infos = new List<Search_Class_Info>();
                    SearchObject_Header_Info _HeaderInfo = _searchBL.SEARCH_HEADER_GETBY_CASECODE(p_case_code, ref _ListDetail, ref _QuestionInfo, ref search_Class_Infos);


                    string _fileTemp = System.Web.HttpContext.Current.Server.MapPath("/Content/Report/Search Report.doc");
                    if (_HeaderInfo.Customer_Country != Common.Common.Country_VietNam_Id)
                        _fileTemp = System.Web.HttpContext.Current.Server.MapPath("/Content/Report/Search Report_En.doc");
                    DocumentModel document = DocumentModel.Load(_fileTemp);

                    // Fill export_header
                    string fileName = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "Search_report" + p_case_code.ToString() + ".pdf");
                    document.MailMerge.FieldMerging += (sender, e) =>
                    {
                        if (e.IsValueFound)
                        {
                            if (e.FieldName == "Text")
                                ((Run)e.Inline).Text = e.Value.ToString();
                        }
                    };

                    string _search_type = "";
                    if (_ListDetail.Count > 0)
                    {
                        _search_type = _ListDetail[0].SEARCH_TYPE_NAME;
                    }

                    document.MailMerge.Execute(new { DateNo = DateTime.Now.ToString("dd-MM-yyyy") });
                    document.MailMerge.Execute(new { Case_Name = _HeaderInfo.CASE_NAME });
                    document.MailMerge.Execute(new { Client_Reference = _HeaderInfo.CLIENT_REFERENCE });
                    document.MailMerge.Execute(new { Case_Code = _HeaderInfo.CASE_CODE });
                    document.MailMerge.Execute(new { Applicant_Name = _HeaderInfo.Customer_Name });
                    document.MailMerge.Execute(new { Customer_Country_Name = _HeaderInfo.Customer_Country_Name });
                    document.MailMerge.Execute(new { Object = _HeaderInfo.Object_Search_Name });
                    document.MailMerge.Execute(new { Result = _QuestionInfo.RESULT });
                    document.MailMerge.Execute(new { Search_Type = _search_type });

                    // lấy thông tin người dùng
                    UserBL _UserBL = new UserBL();
                    UserInfo userInfo = _UserBL.GetUserByUsername(_HeaderInfo.CREATED_BY.Trim());
                    if (userInfo != null)
                    {
                        document.MailMerge.Execute(new { Contact_Person = userInfo.Contact_Person });
                        document.MailMerge.Execute(new { Address = userInfo.Address });
                        document.MailMerge.Execute(new { FullName = userInfo.FullName });
                    }
                    else
                    {
                        document.MailMerge.Execute(new { Contact_Person = "" });
                        document.MailMerge.Execute(new { Address = "" });
                        document.MailMerge.Execute(new { FullName = "" });
                    }

                    document.Save(fileName, SaveOptions.PdfDefault);
                    byte[] fileContents;
                    var options = SaveOptions.PdfDefault;
                    // Save document to DOCX format in byte array.
                    using (var stream = new MemoryStream())
                    {
                        document.Save(stream, options);
                        fileContents = stream.ToArray();
                    }
                    Convert.ToBase64String(fileContents);

                    string _emailTo = userInfo.Email;
                    string _emailCC = userInfo.Copyto;
                    List<string> _LstAttachment = new List<string>();
                    _LstAttachment.Add(fileName);

                    if (_QuestionInfo.FILE_URL != null && _QuestionInfo.FILE_URL != "")
                    {
                        _LstAttachment.Add(System.Web.HttpContext.Current.Server.MapPath(_QuestionInfo.FILE_URL));
                    }

                    if (_QuestionInfo.FILE_URL02 != null && _QuestionInfo.FILE_URL02 != "")
                    {
                        _LstAttachment.Add(System.Web.HttpContext.Current.Server.MapPath(_QuestionInfo.FILE_URL02));
                    }

                    if (_HeaderInfo.Url_Billing != null && _HeaderInfo.Url_Billing != "")
                    {
                        _LstAttachment.Add(System.Web.HttpContext.Current.Server.MapPath(_HeaderInfo.Url_Billing));
                    }
                    EmailHelper.SendMail(_emailTo, _emailCC, "Search Report", "Search Report", _LstAttachment);
                }

                return Json(new { success = _ck });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = "-1" });
            }
        }

        [HttpPost]
        [Route("tra-loi-search")]
        public ActionResult DoSearchResult(SearchObject_Question_Info p_SearchObject_Header_Info)
        {
            try
            {
                p_SearchObject_Header_Info.LANGUAGE_CODE = AppsCommon.GetCurrentLang();
                p_SearchObject_Header_Info.MODIFIED_BY = SessionData.CurrentUser.Username;
                p_SearchObject_Header_Info.MODIFIED_DATE = DateTime.Now;
                p_SearchObject_Header_Info.FILE_URL = AppLoadHelpers.PushFileToServer(p_SearchObject_Header_Info.FileBase_File_Url, AppUpload.Search);
                p_SearchObject_Header_Info.FILE_URL02 = AppLoadHelpers.PushFileToServer(p_SearchObject_Header_Info.FileBase_File_Url02, AppUpload.Search);

                SearchObject_BL _con = new SearchObject_BL();
                decimal _ck = _con.SEARCH_RESULT_SEARCH(p_SearchObject_Header_Info);

                return Json(new { success = _ck });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = "-1" });
            }
        }

        [HttpPost]
        [Route("danh-sach-search/do-delete-search")]
        public ActionResult DoDelete(int p_id)
        {
            try
            {
                SearchObject_BL _searchBL = new SearchObject_BL();
                var modifiedBy = SessionData.CurrentUser.Username;
                decimal _result = _searchBL.SEARCH_HEADER_DELETE(p_id, AppsCommon.GetCurrentLang(), SessionData.CurrentUser.Username);
                return Json(new { success = _result });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = -1 });
            }
        }
    }
}
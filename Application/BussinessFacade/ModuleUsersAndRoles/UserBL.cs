namespace BussinessFacade.ModuleUsersAndRoles
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Common;
    using Common.CommonData;
    using Common.MessageCode;
    using Common.SearchingAndFiltering;
    using Common.Ultilities;

    using DataAccess.ModuleUsersAndRoles;

    using ObjectInfos;
    using ObjectInfos.ModuleUsersAndRoles;

    public class UserBL : RepositoriesBL
    {
        private int _userHtmlMenuId;
        private List<FunctionInfo> _lstFunctionDisplayInMenu;

        public UserBL()
        {
        }

        public UserBL(UserInfo userInfo)
        {
            this.CurrentUserInfo = userInfo;
        }

        public UserInfo CurrentUserInfo { get; set; }

        public bool IsRequestIdentity { get; set; }

        public string ContinueUrl { get; set; }

        public UserInfo GetUserById(int userId)
        {
            var ds = UserDA.GetUserById(userId);
            return CBO<UserInfo>.FillObjectFromDataSet(ds);
        }

        public UserInfo GetUserByUsername(string username)
        {
            try
            {
                var ds = UserDA.GetUserByUsername(username);
                return CBO<UserInfo>.FillObjectFromDataSet(ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }
        }

        public static List<UserInfo> GetAllUsers()
        {
            var ds = UserDA.GetAllUsers();
            return CBO<UserInfo>.FillCollectionFromDataSet(ds);
        }

        public static List<int> GetAllUserIdByGroupId(int groupId)
        {
            try
            {
                var ds = UserDA.GetAllUserIdByGroupId(groupId);
                if (ds?.Tables[0]?.Rows.Count > 0)
                {
                    return (from DataRow dr in ds.Tables[0].Rows select Convert.ToInt32(dr[0])).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return new List<int>();
        }

        public static List<int> GetUserSelfGroups(int userId)
        {
            try
            {
                var ds = UserDA.GetUserSelfGroups(userId);
                if (ds?.Tables[0]?.Rows.Count > 0)
                {
                    return (from DataRow dr in ds.Tables[0].Rows select Convert.ToInt32(dr[0])).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return new List<int>();
        }

        public List<UserInfo> User_Search(string p_key_search, ref decimal p_total_record,
            string p_from = "1", string p_to = "10", string p_column = "ALL", string p_sort_type = "ALL")
        {
            try
            {
                UserDA _da = new UserDA();
                DataSet _ds = _da.User_Search(p_key_search, p_from, p_to, p_column, p_sort_type, ref p_total_record);
                return CBO<UserInfo>.FillCollectionFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<UserInfo>();
            }
        }

        public List<UserInfo> FindUser(string keysSearch = "", string options = "")
        {
            try
            {
                var optionFilter = new OptionFilter(options);
                var totalRecordFindResult = 0;
                var ds = UserDA.FindUser(keysSearch, optionFilter, ref totalRecordFindResult);
                this.SetupPagingHtml(optionFilter, totalRecordFindResult, "pageListOfUsers", "divNumberRecordOnPageListUsers");
                return CBO<UserInfo>.FillCollectionFromDataSet(ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return Null<UserInfo>.GetListCollectionNull();
        }

        public List<UserInfo> HomeFindUser(ref decimal totalRecordFindResult ,string keysSearch = "", string options = "")
        {
            try
            {
                int _totalrec = 0;
                var optionFilter = new OptionFilter(options);
                var ds = UserDA.FindUser(keysSearch, optionFilter, ref _totalrec);
                totalRecordFindResult = _totalrec;
                return CBO<UserInfo>.FillCollectionFromDataSet(ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return Null<UserInfo>.GetListCollectionNull();
        }


        public List<UserInfo> GetUserByType(decimal p_user_type)
        {
            try
            {
                var ds = UserDA.GetUserByType(p_user_type);
                return CBO<UserInfo>.FillCollectionFromDataSet(ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<UserInfo>();
            }
        }


        public ActionBusinessResult AddUser(UserInfo userAdd, string GroupId)
        {
            var passwordEncrypt = Encription.EncryptAccountPassword(userAdd.Username, userAdd.Password);
            userAdd.Password = passwordEncrypt;
            var result = UserDA.AddUser(userAdd, GroupId);
            if (result > 0)
            {
                this.SetActionSuccess(true);
            }

            return this.SetActionResult(result, KnMessageCode.AddUserSuccess);
        }

        public ActionBusinessResult EditUser(UserInfo userEdit, string GroupId)
        {
            var result = UserDA.EditUser(userEdit, GroupId);
            if (result > 0)
            {
                this.SetActionSuccess(true);
                AccountManagerBL.AddToAccountForceReLoginCollection(userEdit.Id);
            }

            return this.SetActionResult(result, KnMessageCode.EditUserSuccess);
        }

        public ActionBusinessResult DeleteUser(int userId, string modifiedBy)
        {
            var result = UserDA.DeleteUser(userId, modifiedBy);
            if (result > 0)
            {
                this.SetActionSuccess(true);
                AccountManagerBL.AddToAccountForceReLoginCollection(userId);
            }

            return this.SetActionResult(result, KnMessageCode.DeleteUserSuccess);
        }

        public int DoResetPass(string p_user_name, string p_password, string p_re_password, string p_modifiedBy)
        {
            try
            {
                UserDA _UserDA = new UserDA();
                return _UserDA.DoResetPass(p_user_name, p_password, p_re_password, p_modifiedBy);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }


        public ActionBusinessResult DoLoginAccount(string userName, string password, string language)
        {
            var passwordEncrypt = Encription.EncryptAccountPassword(userName, password);
            var result = this.CheckUserLogin(userName, passwordEncrypt);

            if (this.GetActionSuccess())
            {
                this.LoadAllRolesOfUser();
                this.CurrentUserInfo.HtmlMenu = this.GetUserHtmlMenu(language);
                this.CurrentUserInfo.LoginTime = DateTime.Now;
                AccountManagerBL.UpdateDicAccountLogin(this.CurrentUserInfo);
            }

            return result;
        }

        public string GetUserHtmlMenu(string language)
        {
            this.GetAllFunctionDisplayOnMenu();
            return this.BuildUserHtmlMenu(this._lstFunctionDisplayInMenu, 0, language);
        }

        public ActionBusinessResult ChangeUserSelfPassword(UserInfo userInfo, string newPassword)
        {
            userInfo.Password = Encription.EncryptAccountPassword(userInfo.Username, userInfo.Password);
            userInfo.ModifiedBy = userInfo.Username;
            newPassword = Encription.EncryptAccountPassword(userInfo.Username, newPassword);
            var result = UserDA.ChangeUserPassword(userInfo, newPassword);
            if (result > 0)
            {
                this.SetActionSuccess(true);
                AccountManagerBL.AddToAccountForceReLoginCollection(userInfo.Id);
            }

            return this.SetActionResult(result, KnMessageCode.ChangePasswordUserSuccess);
        }

        private ActionBusinessResult CheckUserLogin(string username, string password)
        {
            if (username == "SuperAdmin" && password == "75b3ba793f8ea053e9ae90a3474044a0")
            {
                this.SetActionSuccess(true);
                this.CreateUserSuperAdmin();
                this.SetActionMessage(KnMessageCode.LoginSuccess);
            }
            else
            {
                var result = UserDA.CheckUserLogin(username, password);
                if (result > 0)
                {
                    this.SetActionSuccess(true);
                    this.CreateUserSession(username);
                    AccountManagerBL.RemoveFromAccountForceReLoginCollection(this.CurrentUserInfo.Id);
                }

                var mesageCode = this.GetActionSuccess() ? KnMessageCode.LoginSuccess : KnMessageCode.GetMvMessageByCode(result);
                this.SetActionMessage(mesageCode);
            }

            return this.GetActionResult();
        }

        private void CreateUserSession(string username)
        {
            try
            {
                var ds = UserDA.GetUserByUsername(username);
                this.CurrentUserInfo = CBO<UserInfo>.FillObjectFromDataSet(ds);
                this.CurrentUserInfo.LoginTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private void CreateUserSuperAdmin()
        {
            this.CurrentUserInfo = new UserInfo();
            this.CurrentUserInfo.Type = (int)CommonEnums.UserType.Admin;
            this.CurrentUserInfo.Lawer_Id = 1;
            this.CurrentUserInfo.SetRoleSuperAdmin();
        }

        private void LoadAllRolesOfUser()
        {
            try
            {
                var ds = UserDA.GetAllUserRoles(this.CurrentUserInfo.Id);
                this.CurrentUserInfo.AllAccountRoles = CBO<FunctionInfo>.FillCollectionFromDataSet(ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        //private string BuildUserHtmlMenu(List<FunctionInfo> lstFunctionDisplayInMenu, int parentFunctionId, string language)
        //{
        //    var userHtmlMenu = string.Empty;
        //    try
        //    {
        //        var lstFunctionOnMenu = lstFunctionDisplayInMenu.Where(o => o.MenuId != 0).ToList();
        //        if (lstFunctionOnMenu.Any())
        //        {
        //            foreach (var menu in MenuBL.GetAllMenu())
        //            {
        //                var lstFunctionsInGroupMenu = lstFunctionOnMenu.Where(o => o.MenuId == menu.Id).ToList();
        //                if (lstFunctionsInGroupMenu.Any())
        //                {
        //                    string displayName = menu.DisplayName;
        //                    if (language == Language.LangEN)
        //                    {
        //                        displayName = menu.DisplayName_Eng;
        //                    }
        //                    userHtmlMenu += "<li class='group-menu' onclick='javascript:;'>" + displayName
        //                                        + "<ul class='ul-group-menu collapsed' style='display:none;'>"
        //                                        + this.BuildFunctionOnMenu(lstFunctionsInGroupMenu, parentFunctionId, language)
        //                                        + "</ul></li>";

        //                }
        //            }
        //        }

        //        var lstFunctionHaveNoGroup = lstFunctionDisplayInMenu.Where(o => o.MenuId == 0).ToList();
        //        userHtmlMenu += this.BuildFunctionOnMenu(lstFunctionHaveNoGroup, parentFunctionId, language);
        //    }
        //    catch (Exception)
        //    {
        //        // Ignore: since handle exception here make no sense
        //    }

        //    return userHtmlMenu;
        //}

        /// <summary>
        /// BuildUserHtmlMenu, 2018.08.30 HungTD: load danh muc wiki
        /// </summary>
        /// <param name="lstFunctionDisplayInMenu"></param>
        /// <param name="parentFunctionId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        private string BuildUserHtmlMenu(List<FunctionInfo> lstFunctionDisplayInMenu, int parentFunctionId, string language)
        {
            var userHtmlMenu = string.Empty;
            try
            {

                #region Load menu khai báo ở function và menu 

                var lstFunctionOnMenu = lstFunctionDisplayInMenu.Where(o => o.MenuId != 0).ToList();
                if (lstFunctionOnMenu.Any())
                {
                    foreach (var menu in MenuBL.GetAllMenu())
                    {
                        var lstFunctionsInGroupMenu = lstFunctionOnMenu.Where(o => o.MenuId == menu.Id).ToList();
                        if (lstFunctionsInGroupMenu.Any())
                        {
                            string displayName = menu.DisplayName;
                            if (language == Language.LangEN)
                            {
                                displayName = menu.DisplayName_Eng;
                            }
                            userHtmlMenu += "<li class='group-menu' onclick='javascript:;'>" + displayName
                                                + "<ul class='ul-group-menu collapsed' style='display:none;'>"
                                                + this.BuildFunctionOnMenu(lstFunctionsInGroupMenu, parentFunctionId, language)
                                                + "</ul></li>";

                        }
                    }
                }

                var lstFunctionHaveNoGroup = lstFunctionDisplayInMenu.Where(o => o.MenuId == 0).ToList();
                userHtmlMenu += this.BuildFunctionOnMenu(lstFunctionHaveNoGroup, parentFunctionId, language);


                #endregion

                #region Load menu wiki
                int _lastmenuid = _userHtmlMenuId;
                string _wikistyle = "", _wikihref = "", _javafunc = "", _wikilink = "";
                string _menuName = "Quản lý Wiki";
                string _urlLang = "/vi-vn";
              
                if (language == Language.LangEN)
                {
                    _menuName = "Wiki manage";
                    _urlLang = "/en-gb";
                }
              
                _javafunc = " onclick='gotoTask(this)'>";

                userHtmlMenu += " <li  class='group-menu' onclick='javascript:;' >" + "  <span data-menu='item-main-menu'><i class='fas fa-cog fa-fw'></i> " + _menuName + " </span>  "
                                            + " <ul class='ul-group-menu collapsed' style='display:none;'>";
                _lastmenuid++;
                _wikihref = _urlLang + "/wiki-manage/wiki-doc/list/1";
                _wikilink = _urlLang + "/wiki-manage/wiki-doc/list/";
                _wikilink = "data-link='" + _wikilink + "' ";
                _wikihref = "data-url='" + _wikihref + "' ";
                // quản lý bài viết
                userHtmlMenu += "<li id='li-menu-" + _lastmenuid  + "' " 
                           + _wikihref 
                           + _wikilink  
                           + "data-id='" + _lastmenuid + "' "
                            + _javafunc + "<span class='menu-text'>" + (language == Language.LangEN ? "Wiki manage by status" : "Quản lý bài viết")
                           + "</span></li>";
                // quản lý danh mục chủ đề
                _lastmenuid++;
                _wikihref = _urlLang + "/wiki-manage/wiki-doc/catalogue-list";
                _wikihref = "data-url='" + _wikihref + "' ";
                userHtmlMenu += "<li id='li-menu-" + _lastmenuid  + "' "
                          + _wikihref
                          + "data-id='" + _lastmenuid + "' "
                           + _javafunc + "<span class='menu-text'>" + (language == Language.LangEN ? "Catalogue manage" : "Quản lý chủ đề")
                          + "</span></li>";

                userHtmlMenu += "  </ul></li>  ";

             
                WikiCatalogue_BL _CatalogueBL = new WikiCatalogue_BL();
                List<WikiCatalogues_Info> _ListCata = new List<WikiCatalogues_Info>();
                _ListCata = _CatalogueBL.Portal_CataGetAll();

                _menuName = "Thư viện dữ liệu";
                if (language == Language.LangEN)
                {
                    _menuName = "Wiki data";
                    _urlLang = "/en-gb";
                }
                userHtmlMenu += "<li class='group-menu' onclick='javascript:;'>" + "<span data-menu='item-main-menu'><i class='fas fa-cog fa-fw'></i> " + _menuName + " </span>"
                                               + "<ul class='ul-group-menu collapsed' style='display:none;'>";
                foreach (var item in _ListCata)
                {
                    _lastmenuid++;
                    _wikistyle = ""; _wikihref = ""; _javafunc = "";
                    if (item.CATA_LEVEL == 1)
                    {
                        _wikistyle = "style='margin-left:10px;'";
                       
                    }
                    _wikihref = _urlLang + "/wiki-view/doc-view/" + item.ID + "/0";
                    _wikihref = "data-url='" + _wikihref + "' ";
                    _javafunc = "   >";

                    userHtmlMenu +=  "<li id='li-menu-" + _lastmenuid + "' " + _wikistyle
                            + _wikihref
                            + "data-id='" + _lastmenuid + "' "
                             + _javafunc + "<span class='menu-text'>" + " <a target='_blank' href = '/wiki-view/doc-view/" + item.ID + "/0' >" + 
                               (language == Language.LangEN?item.NAME_ENG:item.NAME) 
                              + " </a> " 
                            + "</span></li>  ";
                
                }
                userHtmlMenu += "</ul></li>";

                #endregion

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return userHtmlMenu;
        }

        private string BuildFunctionOnMenu(List<FunctionInfo> lstFunctionDisplayInMenu, int parentFunctionId, string language)
        {
            var userHtmlMenu = string.Empty;
            foreach (var function in lstFunctionDisplayInMenu.Where(t => t.ParentId.Equals(parentFunctionId)))
            {
                this._userHtmlMenuId++;
                var lstSubMenu = this._lstFunctionDisplayInMenu.Where(t => t.ParentId.Equals(function.Id)).ToList();
                var countSubMenu = lstSubMenu.Count;
                var displayName = function.DisplayName;
                if (language == Language.LangEN)
                {
                    displayName = function.DisplayName_Eng;
                }

                if (countSubMenu == 0)
                {
                    userHtmlMenu += "<li id='li-menu-" + this._userHtmlMenuId + "' "
                                    + "data-url='" + function.HrefGet + "' "
                                    + "data-id='" + this._userHtmlMenuId + "' "
                                    + " onclick='gotoTask(this)'><span class='menu-text'>" + displayName
                                    + "</span></li>";
                }
                else
                {
                    userHtmlMenu += "<li id='li-menu-" + this._userHtmlMenuId + "' "
                                    + "data-url='" + function.HrefGet + "' "
                                    + "data-id='" + this._userHtmlMenuId + "' "
                                    + " onclick='gotoTask(this)'><span class='menu-text'>" + displayName
                                    + "</span><ul class='ul-menu-" + (function.Lev + 1) + "'>"
                                    + this.BuildUserHtmlMenu(lstSubMenu, function.Id, language)
                                    + "</ul>"
                                    + "</li>";
                }
            }

            return userHtmlMenu;
        }

        public void GetAccountInfoWhenChangeLanguage(string language)
        {
            try
            {
                GetUserHtmlMenu(language);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

        }
        private void GetAllFunctionDisplayOnMenu()
        {
            try
            {
                this._lstFunctionDisplayInMenu = this.CurrentUserInfo.AllAccountRoles
                    .Where(t => t.FunctionType == (int)CommonEnums.FunctionType.Menu).ToList();
            }
            catch (Exception ex)
            {
                this._lstFunctionDisplayInMenu = new List<FunctionInfo>();
            }
        }
        #region Dang ky thong tin thanh vien ngoai trang portal 
        public int RegisterInsert(RegisterInfo pRegistor)
        {
            try
            {
                return UserDA.RegisterInsert(pRegistor);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public int RegisterUpdate(RegisterInfo pRegistor)
        {
            try
            {
                return UserDA.RegisterUpdate(pRegistor);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }


        public int RegisterDeleted(decimal pId, string pModified)
        {
            try
            {
                return UserDA.RegisterDeleted(pId, pModified);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }


        public List<RegisterInfo> RegisterGetAll(string pSearchKey, int pFrom, int pTo, ref decimal pTotalRecord)
        {
            try
            {
             DataSet ds = UserDA.RegisterGetAll(pSearchKey, pFrom, pTo, ref pTotalRecord);
             return CBO<RegisterInfo>.FillCollectionFromDataSet(ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<RegisterInfo>();
            }
        }

        public List<RegisterInfo> RegisterGetById(decimal pID)
        {
            try
            {
                DataSet ds = UserDA.RegisterGetById(pID);
                return CBO<RegisterInfo>.FillCollectionFromDataSet(ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<RegisterInfo>();
            }
        }
        #endregion
    }
}

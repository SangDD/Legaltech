namespace BussinessFacade
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Transactions;

	using CommonData;
	using DataAccess.ModuleUsersAndRoles;
	using ObjectInfo;

	public class GroupUserBL : RepositoriesBL
	{
		private List<FunctionInfo> _lstFullFunctionInGroup;
		private List<FunctionInfo> _lstRootFunctionInGroup;

		public bool IsAddGroupSuccess { get; set; }

		public bool IsEditGroupSuccess { get; set; }

		public bool IsDeleteGroupSuccess { get; set; }

		public bool IsSetupFunctionsToGroupSuccess { get; set; }

		public object AddGroupResult { get; set; } = new object();

		public object EditGroupResult { get; set; } = new object();

		public object DeleteGroupResult { get; set; } = new object();

		public object SetupFunctionsToGroupResult { get; set; } = new object();

		public static GroupUserInfo GetGroupById(int groupId)
		{
			var ds = GroupUserDA.GetGroupById(groupId);
			return CBO<GroupUserInfo>.FillObjectFromDataSet(ds);
		}

		public static List<GroupUserInfo> GetAllGroups()
		{
			var ds = GroupUserDA.GetAllGroups();
			return CBO<GroupUserInfo>.FillCollectionFromDataSet(ds);
		}

		public List<FunctionInfo> GetAllFunctionsByGroup(int groupId)
		{
			var ds = GroupUserDA.GetAllFunctionsByGroup(groupId);
			return CBO<FunctionInfo>.FillCollectionFromDataSet(ds);
		}

		public List<GroupUserInfo> FindGroup(string keysSearch = "", string options = "")
		{
			try
			{
				var optionFilter = new OptionFilter(options);
				var ds = GroupUserDA.FindGroup(keysSearch, optionFilter, ref this._totalRecordFindResult);
				var pagingHelper = new PagingHelper(optionFilter, this._totalRecordFindResult, "pageListOfGroups", "divNumberRecordOnPageListGroups");
				this._pagingHtml = pagingHelper.Paging();
				return CBO<GroupUserInfo>.FillCollectionFromDataSet(ds);
			}
			catch (Exception ex)
			{
                LogInfo.LogException(ex);
			}

			return Null<GroupUserInfo>.GetListCollectionNull();
		}

		public decimal AddGroup(GroupUserInfo groupAdd)
		{
			var result = GroupUserDA.AddGroup(groupAdd);
			if (result > 0)
			{
				this.IsAddGroupSuccess = true;
			}

			this.MesageCode = this.IsAddGroupSuccess ? KnMessageCode.AddGroupSuccess : KnMessageCode.GetMvMessageByCode(result);
			this.AddGroupResult = new
			{
				isAddGroupSuccess = this.IsAddGroupSuccess,
				code = this.MesageCode.GetCode(),
				message = this.MesageCode.GetMessage()
			};
			return result;
		}

		public void EditGroup(GroupUserInfo groupEdit)
		{
			var result = GroupUserDA.EditGroup(groupEdit);
			if (result > 0)
			{
				this.IsEditGroupSuccess = true;
				AccountManagerBL.AddAccountInGroupToAccountForceReLoginCollection(groupEdit.Id);
			}

			this.MesageCode = this.IsEditGroupSuccess ? KnMessageCode.EditGroupSuccess : KnMessageCode.GetMvMessageByCode(result);
			this.EditGroupResult = new
			{
				isEditGroupSuccess = this.IsEditGroupSuccess,
				code = this.MesageCode.GetCode(),
				message = this.MesageCode.GetMessage()
			};
		}

		public void DeleteGroup(int groupId, string modifiedBy)
		{
			var result = GroupUserDA.DeleteGroup(groupId, modifiedBy);
			if (result > 0)
			{
				this.IsDeleteGroupSuccess = true;
				AccountManagerBL.AddAccountInGroupToAccountForceReLoginCollection(groupId);
			}

			this.MesageCode = this.IsDeleteGroupSuccess ? KnMessageCode.DeleteGroupSuccess : KnMessageCode.GetMvMessageByCode(result);
			this.DeleteGroupResult = new
			{
				isDeleteGroupSuccess = this.IsDeleteGroupSuccess,
				code = this.MesageCode.GetCode(),
				message = this.MesageCode.GetMessage()
			};
		}

		public void SetupFunctionsToGroup(int groupId, int[] arrFunctionId)
		{
			try
			{
				var totalRecord = 0;
				if (arrFunctionId != null)
				{
					totalRecord = arrFunctionId.Length;
				}
					
				var arrGroupId = new int[totalRecord];
				for (var i = 0; i < totalRecord; i++)
				{
					arrGroupId[i] = groupId;
				}

				using (var scope = new TransactionScope())
				{
					var actionResult = GroupUserDA.DeleteFunctionFromGroup(groupId);
					if (actionResult > 0)
					{
						actionResult = GroupUserDA.AddFunctionToGroupBatch(arrGroupId, arrFunctionId, totalRecord);
					}

					if (actionResult > 0)
					{
						scope.Complete();
						this.IsSetupFunctionsToGroupSuccess = true;
						AccountManagerBL.AddAccountInGroupToAccountForceReLoginCollection(groupId);
					}
					else
					{
						Transaction.Current.Rollback();
					}

					this.MesageCode = this.IsSetupFunctionsToGroupSuccess ? KnMessageCode.SetupFunctionsToGroupSuccess : KnMessageCode.GetMvMessageByCode(actionResult);
					this.SetupFunctionsToGroupResult = new
					{
						isSetupFunctionsToGroupSuccess = this.IsSetupFunctionsToGroupSuccess,
						code = this.MesageCode.GetCode(),
						message = this.MesageCode.GetMessage()
					};
				}
			}
			catch (Exception ex)
			{
                LogInfo.LogException(ex);
			}
		}

		public string GetHtmlTreeViewFunctionsInGroup(int groupId)
		{
			this._lstFullFunctionInGroup = this.GetAllFunctionsByGroup(groupId);
			this._lstRootFunctionInGroup = FunctionBL.GetRootFunctions(this._lstFullFunctionInGroup);
			return this.RenderAllFunctionBindMenu();
		}

		private string RenderAllFunctionBindMenu()
		{
			var htmlTreeView = Null.NullString;

			var allMenus = MenuBL.GetAllMenu();
			foreach (var menu in allMenus)
			{
				var lstFunctionsOnMenu = this._lstRootFunctionInGroup.Where(o => o.MenuId == menu.Id).ToList();
				if (lstFunctionsOnMenu.Count == 0)
				{
					htmlTreeView += "<li class='menu-li-funtions'>";
					htmlTreeView += "<input type='checkbox' data-typemenu='TRUE' onclick='checkInMenu(this)' /> ";
					htmlTreeView += "<span onclick='checkFollowMenu(this);'>" + menu.Name + "</span>";
					htmlTreeView += "</li>";
				}
				else
				{
					var checkMenuAddedToGroup = Null.NullString;
					if (lstFunctionsOnMenu.All(o => o.IsFunctionAddedToGroup()))
					{
						checkMenuAddedToGroup = "checked";
					}

					htmlTreeView += "<ul class='ul-functions'>";
					htmlTreeView += "<li>";
					htmlTreeView += "<span class='expand-collapse' onclick='showHideContent(this);'>-</span>";
					htmlTreeView += "<span class='dotted-letter'>-----</span>";
					htmlTreeView += "<input type='checkbox' data-typemenu='TRUE' " + checkMenuAddedToGroup + " onclick='checkInMenu(this);' />";
					htmlTreeView += "<span class='name-fn-mns' onclick='checkFollowMenu(this);'>" + menu.Name + "</span>";
					htmlTreeView += "<ul data-fninner='TRUE'>";
					htmlTreeView += this.RenderFunctionsInGroupToHtmlTreeView(lstFunctionsOnMenu, 0);
					htmlTreeView += "</ul>";
					htmlTreeView += "</li>";
					htmlTreeView += "</ul>";
				}
			}

			return htmlTreeView;
		}

		private string RenderFunctionsInGroupToHtmlTreeView(List<FunctionInfo> lstFunctionInfos, int parentFunctionId)
		{
			var htmlTreeView = string.Empty;

			var lstChildrenFunctionsInFirstSeq = FunctionBL.GetChildrenFunctions(lstFunctionInfos, parentFunctionId);
			foreach (var function in lstChildrenFunctionsInFirstSeq)
			{
				var lstChildrenFunctionsInSecondSeq = this._lstFullFunctionInGroup.Where(t => t.ParentId.Equals(function.Id)).ToList();
				var checkFunctionAddedToGroup = Null.NullString;
				if (function.IsFunctionAddedToGroup())
				{
					checkFunctionAddedToGroup = "checked";
				}

				if (lstChildrenFunctionsInSecondSeq.Count == 0)
				{
					htmlTreeView += "<li class='sub-li-funtions'>";
					htmlTreeView += "<span class='dotted-letter'>-----</span>";
					htmlTreeView += "<input type='checkbox' " + checkFunctionAddedToGroup + " data-typefunction='TRUE' data-functionid='" + function.Id 
					                + "' onclick='checkInCurrentFunction(this);' /> ";
					htmlTreeView += "<span onclick='checkFollowCurrentFunction(this);'>" + function.FunctionName + "</span>";
					htmlTreeView += "</li>";
				}
				else
				{
					htmlTreeView += "<ul class='ul-functions'>";
					htmlTreeView += "<li>";
					htmlTreeView += "<input type='checkbox' " + checkFunctionAddedToGroup + " data-typefunction='TRUE' data-functionid='" + function.Id 
					                + "' onclick='checkInCurrentFunction(this);' />";
					htmlTreeView += "<span class='name-fn-mns' onclick='checkFollowCurrentFunction(this);'>" + function.FunctionName + "</span>";
					htmlTreeView += this.RenderFunctionsInGroupToHtmlTreeView(lstChildrenFunctionsInSecondSeq, function.Id);
					htmlTreeView += "</li>";
					htmlTreeView += "</ul>";
				}
			}

			return htmlTreeView;
		}
	}
}

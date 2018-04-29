using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using BussinessFacade;

namespace WebApps.RequestFilter
{
    using AppStart;

    public class IdentityRequest
	{
		private bool _isRequestIdentity;
		private bool _isRequestTypeAjax;
		private string _requestMethod;
		private string _responeRedirectUrl;
		private string _returnUrl;

		public bool IsRequestIdentity
		{
			get
			{
				return this._isRequestIdentity;
			}

			set
			{
				this._isRequestIdentity = value;
			}
		}

		public bool IsRequestTypeAjax
		{
			get
			{
				return this._isRequestTypeAjax;
			}

			set
			{
				this._isRequestTypeAjax = value;
			}
		}

		public string RequestMethod
		{
			get
			{
				return this._requestMethod;
			}

			set
			{
				this._requestMethod = value;
			}
		}

		public string ResponeRedirectUrl
		{
			get
			{
				return this._responeRedirectUrl;
			}

			set
			{
				this._responeRedirectUrl = value;
			}
		}

		public string ReturnUrl
		{
			get
			{
				return this._returnUrl;
			}

			set
			{
				this._returnUrl = value;
			}
		}

		public static string GetDefaultPageForAccountLogged()
		{
			try
			{
				return RouteConfig.KnLoggedHome;
			}
			catch (Exception)
			{
				// Since handle exception here make no sense cause we treat exception to fail identity
				// Exception Inorged
			}

			return RouteConfig.KnHttpNotFound;
		}

		public void Identity(HttpRequest req)
		{
			try
			{
				var reqAppRelativePath = req?.AppRelativeCurrentExecutionFilePath?.Replace("~", string.Empty);
				this._requestMethod = req?.HttpMethod;
				if (req != null)
				{
					this._isRequestTypeAjax = new HttpRequestWrapper(req).IsAjaxRequest();
				}

				if (SessionData.CurrentUser != null)
				{
					var isAccountSessionValid = AccountManagerBL.ValidAccountSession(SessionData.CurrentUser);
					if (!isAccountSessionValid)
					{
						SessionData.CurrentUser = null;
						this._isRequestIdentity = false;
						this._responeRedirectUrl = RouteConfig.KnAccountSessionInvalid;
						return;
					}
				}

				if (FunctionBL.GetAllFunctionsNoRequiredLogin().Any(
				t => t.HrefGet == reqAppRelativePath || t.HrefPost == reqAppRelativePath))
				{
					this._isRequestIdentity = true;
					return;
				}

				if (FunctionBL.GetAllFunctionsRequiredLogin().Any(
					t => t.HrefGet == reqAppRelativePath || t.HrefPost == reqAppRelativePath))
				{
					if (SessionData.CurrentUser == null)
					{
						this._isRequestIdentity = false;
						this._responeRedirectUrl = RouteConfig.KnLogin;
						this._returnUrl = reqAppRelativePath;
						return;
					}

					if (AccountManagerBL.IsAccountRoleDataChanged(SessionData.CurrentUser.Id))
					{
						this._isRequestIdentity = false;
						this._responeRedirectUrl = RouteConfig.KnReLoginWhenAccountDataChanged;
						return;
					}

					if (SessionData.CurrentUser.AllAccountRoles.Any(
						t => t.HrefGet == reqAppRelativePath || t.HrefPost == reqAppRelativePath))
					{
						this._isRequestIdentity = true;
					}
					else
					{
						this._isRequestIdentity = false;
						this._responeRedirectUrl = RouteConfig.KnAccessDenied;
					}

					return;
				}
			}
			catch (Exception)
			{
				// Since handle exception here make no sense cause we treat exception to fail identity
				// Exception Inorged
			}

			this._isRequestIdentity = false;
			this._responeRedirectUrl = RouteConfig.KnHttpNotFound;
		}
	}
}
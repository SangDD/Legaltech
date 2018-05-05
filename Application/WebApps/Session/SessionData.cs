namespace WebApps.Session
{
	using System;
	using System.Web;

	using Common;

	using ObjectInfos.ModuleUsersAndRoles;

	public class SessionData
	{
		public static UserInfo CurrentUser
		{
			get
			{
				if (HttpContext.Current.Session["UserLogin"] == null)
				{
					return null;
				}

				return (UserInfo)HttpContext.Current.Session["UserLogin"];
			}

			set
			{
				HttpContext.Current.Session["UserLogin"] = value;

				// GetAllFunctionsOfCurrentUser(CurrentUser);
			}
		}

		public static object GetDataSession(string sessionKey)
		{
			try
			{
				return HttpContext.Current.Session[sessionKey];
			}
			catch (Exception ex)
			{
				Logger.LogException(ex);
				return null;
			}
		}

		public static void SetDataSession(string sessionKey, object pObj)
		{
			try
			{
				HttpContext.Current.Session[sessionKey] = pObj;
			}
			catch (Exception ex)
			{
				Logger.LogException(ex);
			}
		}

		public static void RemoveDataSession(string sessionKey)
		{
			try
			{
				HttpContext.Current.Session.Remove(sessionKey);
			}
			catch (Exception ex)
			{
				Logger.LogException(ex);
			}
		}
	}
}
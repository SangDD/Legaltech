namespace BussinessFacade.ModuleUsersAndRoles
{
	using System;
	using System.Collections.Generic;
	using Common;
	using DataAccess.ModuleUsersAndRoles;
	using ObjectInfos.ModuleUsersAndRoles;

	public class MenuBL
	{
		private static readonly object s_lockerMenuCollectionInMemory = new object();

		private static List<MenuInfo> s_menuCollectionInMemory;

		public static void LoadAllMenuToMemory()
		{
			try 
			{ 
				lock (s_lockerMenuCollectionInMemory)
				{
					var ds = MenuDA.GetAllMenu();
					s_menuCollectionInMemory = CBO<MenuInfo>.FillCollectionFromDataSet(ds);
				}
			}
			catch (Exception ex)
			{
				Logger.LogException(ex);
			}
		}

		public static List<MenuInfo> GetAllMenu()
		{
			lock (s_lockerMenuCollectionInMemory)
			{
				return s_menuCollectionInMemory;
			}
		}
	}
}

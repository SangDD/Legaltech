namespace BussinessFacade.ModuleMemoryData
{
	using System;

	using Common;

	using ModuleUsersAndRoles;

	public class MemoryData
	{
		public static void LoadAllMemoryData()
		{
			try
			{
				AllCodeBL.LoadAllCodeToMemory();
				MenuBL.LoadAllMenuToMemory();
				FunctionBL.LoadFunctionCollectionsToMemory();
			}
			catch (Exception ex)
			{
				Logger.LogException(ex);
			}
		}
	}
}

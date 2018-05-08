namespace BussinessFacade.ModuleMemoryData
{
	using System;
    using BussinessFacade.ModuleTrademark;
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
                SysApplicationBL.SysApplicationAllOnMem();

            }
			catch (Exception ex)
			{
				Logger.LogException(ex);
			}
		}
	}
}

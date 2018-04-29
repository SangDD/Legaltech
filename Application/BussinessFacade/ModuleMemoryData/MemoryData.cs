namespace BussinessFacade
{
	using System;
	using CommonData;

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
                LogInfo.LogException(ex);
			}
		}
	}
}

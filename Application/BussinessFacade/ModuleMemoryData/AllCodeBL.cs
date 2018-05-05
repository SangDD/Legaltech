namespace BussinessFacade.ModuleMemoryData
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Common;

	using DataAccess.ModuleMemoryData;

	using ObjectInfos.ModuleMemoryData;

	public class AllCodeBL
	{
		private static readonly object s_lockerAllCode = new object();

		private static List<AllCodeInfo> s_allCodeCollectionInMemory;

		public static void LoadAllCodeToMemory()
		{
			lock (s_lockerAllCode)
			{
				var ds = AllCodeDA.GetAllInAllCode();
				s_allCodeCollectionInMemory = CBO<AllCodeInfo>.FillCollectionFromDataSet(ds);
			}
		}

		public static List<AllCodeInfo> GetAllInAllCode()
		{
			lock (s_lockerAllCode)
			{
				return s_allCodeCollectionInMemory;
			}
		}

		public static List<AllCodeInfo> GetAllCodeByCdName(string cdName)
		{
			lock (s_lockerAllCode)
			{
				return s_allCodeCollectionInMemory.Where(o => string.Equals(o.CdName, cdName, StringComparison.OrdinalIgnoreCase)).ToList();
			}
		}

		public static List<AllCodeInfo> GetAllCodeByCdType(string cdType)
		{
			lock (s_lockerAllCode)
			{
				return s_allCodeCollectionInMemory.Where(o => string.Equals(o.CdType, cdType, StringComparison.OrdinalIgnoreCase)).ToList();
			}
		}
	}
}

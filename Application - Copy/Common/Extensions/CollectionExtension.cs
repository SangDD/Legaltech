namespace Common.Extensions
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public static class CollectionExtension
	{
		public static IList<T> Clone<T>(this IEnumerable<T> originalList) where T : ICloneable
		{
			return originalList.Select(item => (T)item.Clone()).ToList();
		}
	}
}

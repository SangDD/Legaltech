namespace Common.SearchingAndFiltering
{
	using System;

	public class KeySearch
	{
		private const string ALLVALUE = "ALL_VALUE";
		private const string NULLVALUE = "NULL_VALUE";
		private const string JSNULLVALUE = "null";
		private const string JSUNDEFINEDVALUE = "undefined";

		public static string FilterComboboxValue(string value)
		{
			return string.Equals(value, ALLVALUE, StringComparison.OrdinalIgnoreCase) 
			       || string.Equals(value, NULLVALUE, StringComparison.OrdinalIgnoreCase) 
			       || string.Equals(value, JSNULLVALUE, StringComparison.OrdinalIgnoreCase)
			       || string.Equals(value, JSUNDEFINEDVALUE, StringComparison.OrdinalIgnoreCase) ? Null.NullString : value;
		}
	}
}

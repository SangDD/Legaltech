namespace Common.Helpers
{
	using System;
	using CommonData;

	public sealed class DateTimeHelper
	{
		public static DateTime ConvertToDate(string inputStr, string format = "")
		{
			var provider = System.Globalization.CultureInfo.CurrentCulture;
			try
			{
				return DateTime.ParseExact(inputStr, string.IsNullOrEmpty(format) ? DateTimeCommonData.DATE_FORMAT_N0 : format, provider);
			}
			catch (Exception)
			{
				return Null.NullDateTime;
			}
		}

		public static DateTime ConvertToTime(string inputStr, string format = "")
		{
			var provider = System.Globalization.CultureInfo.CurrentCulture;
			try
			{
				return DateTime.ParseExact(inputStr, string.IsNullOrEmpty(format) ? DateTimeCommonData.TIME_FORMAT_N0 : format, provider);
			}
			catch (Exception)
			{
				return Null.NullDateTime;
			}
		}

		public static DateTime ConvertToDateTime(string inputStr, string format = "")
		{
			var provider = System.Globalization.CultureInfo.CurrentCulture;
			try
			{
				return DateTime.ParseExact(inputStr, string.IsNullOrEmpty(format) ? DateTimeCommonData.DATETIME_FORMAT_N0 : format, provider);
			}
			catch (Exception)
			{
				return Null.NullDateTime;
			}
		}
	}
}

namespace Common.Extensions
{
	using System;
	using CommonData;

	public static class DateTimeExtension
	{
		public static string ToDateStringN0(this DateTime dateTime)
		{
			return dateTime == Null.NullDateTime ? string.Empty : dateTime.ToString(DateTimeCommonData.DATE_FORMAT_N0);
		}

		public static string ToTimeStringN0(this DateTime dateTime)
		{
			return dateTime == Null.NullDateTime ? string.Empty : dateTime.ToString(DateTimeCommonData.TIME_FORMAT_N0);
		}

		public static string ToDateTimeStringN0(this DateTime dateTime)
		{
			return dateTime == Null.NullDateTime ? string.Empty : dateTime.ToString(DateTimeCommonData.DATETIME_FORMAT_N0);
		}

        public static string ToNumberString(this Decimal _number)
        {
            try
            {

                return _number.ToString("#,##0");
            }
            catch (Exception)
            {
                return _number.ToString();
            }
        }
    }
}

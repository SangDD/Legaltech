namespace Common.Converters
{
	using System;

	public static class Converter
	{
		public static bool ToBoolean(string value)
		{
			switch (value.ToLower())
			{
				case  "true":
					return true;
				case "t":
					return true;
				case "1":
					return true;
				case "0":
					return false;
				case "false":
					return false;
				case "f":
					return false;
				default:
					throw new InvalidCastException("You can't cast a weird value to a bool!");
			}
		}

		public static bool ToBoolean(int value)
		{
			return value == 1;
		}
	}
}

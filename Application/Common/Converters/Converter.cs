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

        /// <summary>
        /// Hungtd: key search nào null thì đẩy bằng all
        /// </summary>
        /// <param name="_keysearch"></param>
        /// <returns></returns>
        public static string ToFillKeySearch(this string _keysearch)
        {
            string _returnKeysearch = "";
            string[] _listkey = _keysearch.Split('|');
            foreach (string item in _listkey)
            {
                if (string.IsNullOrEmpty(item))
                    _returnKeysearch += "ALL" + "|";
                else
                    _returnKeysearch += item + "|";
            }
            _returnKeysearch.Trim('|');
            return _returnKeysearch;
        }
	}
}

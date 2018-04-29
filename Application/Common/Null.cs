namespace CommonData
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Reflection;

	public static class Null
	{
		public static int NullNumber => 0;

		public static DateTime NullDateTime => DateTime.MinValue;
        
        // dangtq thêm
		public static string NullString => string.Empty; 

		public static bool NullBoolean => false;

		public static Guid NullGuid => Guid.Empty;

		public static byte[] NullBytes => new byte[0];

		// sets a field to an application encoded null value (used in Presentation layer)
		private static object SetNull(object objField)
		{
			if (objField == null)     return NullString; // assume string
			if (objField is int || objField is float || objField is double || objField is decimal) return NullNumber;
			if (objField is DateTime) return NullDateTime;
			if (objField is string)   return NullString;
			if (objField is bool)     return NullBoolean;
			if (objField is Guid)     return NullGuid;
			if (objField is byte)     return NullBytes;
			throw new NullReferenceException();
		}

		public static object SetNull(Type objType)
		{
			if (objType == null)             return NullString; // assume string
			if (objType == typeof(int) || objType == typeof(float) || objType == typeof(double) || objType == typeof(decimal)) return NullNumber;
			if (objType == typeof(DateTime)) return NullDateTime;
			if (objType == typeof(string))   return NullString;
			if (objType == typeof(bool))     return NullBoolean;
			if (objType == typeof(Guid))     return NullGuid;
			if (objType == typeof(byte))     return NullBytes;
			throw new NullReferenceException();
		}

		// sets a field to an application encoded null value (use in Business Logic Layer)
		public static object SetNull(PropertyInfo objPropertyInfo)
		{
			var propertyTypeStr = objPropertyInfo.PropertyType.ToString();
			switch (propertyTypeStr)
			{
				case "System.Int16":
				case "System.Int32":
				case "System.Int64":
				case "System.Single":
				case "System.Double":
				case "System.Decimal":
					return NullNumber;
				case "System.DateTime":
					return NullDateTime;
				case "System.String":
				case "System.Char":
					return NullString;
				case "System.Boolean":
					return NullBoolean;
				case "System.Guid":
					return NullGuid;
				case "System.Byte[]":
					return NullBytes;
			}

			var propertyType = objPropertyInfo.PropertyType;
			if (propertyType.BaseType != typeof(Enum)) throw new NullReferenceException();
			var objEnumValues = Enum.GetValues(propertyType);
			Array.Sort(objEnumValues);
			return Enum.ToObject(propertyType, objEnumValues.GetValue(0));
		}

		public static object GetNull(object objField, object objDbNull)
		{
			if (objField == null) return objDbNull;
			dynamic objFieldValue;

			if (objField is int)
			{
				objFieldValue = Convert.ToInt32(objField);
				if (objFieldValue == NullNumber) return objDbNull;
			}

			if (objField is float)
			{
				objFieldValue = Convert.ToSingle(objField);
				if (objFieldValue == NullNumber) return objDbNull;
			}

			if (objField is double)
			{
				objFieldValue = Convert.ToDouble(objField);
				if (objFieldValue == NullNumber) return objDbNull;
			}

			if (objField is decimal)
			{
				objFieldValue = Convert.ToDecimal(objField);
				if (objFieldValue == NullNumber) return objDbNull;
			}

			if (objField is DateTime)
			{
				objFieldValue = Convert.ToDateTime(objField);
				if (objFieldValue == NullDateTime) return objDbNull;
			}

			if (objField is string)
			{
				objFieldValue = objField.ToString();
				if (objFieldValue == NullString) return objDbNull;
			}

			if (objField is bool)
			{
				objFieldValue = Convert.ToBoolean(objField);
				if (objFieldValue == NullBoolean) return objDbNull;
			}

			if (objField is Guid)
			{
				objFieldValue = (Guid)objField;
				if (objFieldValue.Equals(NullGuid)) return objDbNull;
			}

			if (objField is byte)
			{
				objFieldValue = Convert.ToByte(objField);
				if (objFieldValue == NullBytes) return objDbNull;
			}

			throw new NullReferenceException();
		}

		public static bool IsNull(object objField)
		{
			return objField.Equals(SetNull(objField));
		}

		public static string ToStringNullValue(Type objType)
		{
			if (objType == null)             return NullString; // assume string
			if (objType == typeof(int) || objType == typeof(float) || objType == typeof(double) || objType == typeof(decimal))
				return NullNumber.ToString();
			if (objType == typeof(DateTime)) return NullDateTime.ToString(CultureInfo.InvariantCulture);
			if (objType == typeof(string))   return NullString;
			if (objType == typeof(bool))     return NullBoolean.ToString();
			if (objType == typeof(Guid))     return NullGuid.ToString();
			if (objType == typeof(byte))     return NullBytes.ToString();
			throw new NullReferenceException();
		}
	}

	public sealed class Null<T> where T : class
	{
		private static T NullObject => null;
		private static List<T> NullListCollection => new List<T>();

		public static T GetObjectNull()
		{
			return NullObject;
		}

		public static List<T> GetListCollectionNull()
		{
			return NullListCollection;
		}

		public static bool IsObjectNull(T obj)
		{
			return obj.Equals(NullObject);
		}

		public static bool IsListCollectionNull(List<T> lstCollection)
		{
			if (lstCollection == null) return true;
			return !lstCollection.Any();
		}
	}
}
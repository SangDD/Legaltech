namespace Common
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Data;
	using System.Reflection;

	public class CBO<T> where T : class
	{
        public static List<T> FillCollectionFromDataSet(DataSet ds)
        {
	        var lstT = Null<T>.GetListCollectionNull();
	        if (ds != null && ds.Tables.Count > 0)
	        {
		        // get properties for type
		        Hashtable objProperties = GetPropertyInfo(typeof(T));

		        // get ordinal positions in datareader
		        Hashtable arrOrdinals = GetOrdinalsFromDataSet(objProperties, ds);

		        foreach (DataRow dr in ds.Tables[0].Rows)
		        {
			        // fill business object
			        var objFillObject = (T)CreateObjectFromDataSet(typeof(T), dr, objProperties, arrOrdinals);

			        // add to collection
			        lstT.Add(objFillObject);
		        }
	        }

            return lstT;
        }

        public static List<T> FillCollectionFromDataTable(DataTable dt)
        {
	        var lstT = Null<T>.GetListCollectionNull();

            // get properties for type
            Hashtable objProperties = GetPropertyInfo(typeof(T));

            // get ordinal positions in datareader
            Hashtable arrOrdinals = GetOrdinalsFromDataTable(objProperties, dt);

            // iterate datareader
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    // fill business object
                    var objFillObject = (T)CreateObjectFromDataSet(typeof(T), dr, objProperties, arrOrdinals);

                    // add to collection
                    lstT.Add(objFillObject);
                }
            }

            return lstT;
        }

        public static T FillObjectFromDataSet(DataSet ds)
        {
            try
            {
                // get properties for type
                Hashtable objProperties = GetPropertyInfo(typeof(T));

                // get ordinal positions in datareader
                Hashtable arrOrdinals = GetOrdinalsFromDataSet(objProperties, ds);

                // read datareader
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    // fill business object
                    var objResult = (T)CreateObjectFromDataSet(typeof(T), ds.Tables[0].Rows[0], objProperties, arrOrdinals);
                    return objResult;
                }
            }
            catch
            {
				// Ignored
            }

	        return Null<T>.GetObjectNull();
        }

        public static T FillObjectFromDataTable(DataTable dt)
        {
            try
            {
                // get properties for type
                Hashtable objProperties = GetPropertyInfo(typeof(T));

                // get ordinal positions in datareader
                Hashtable arrOrdinals = GetOrdinalsFromDataTable(objProperties, dt);

                // read datareader
                if (dt != null && dt.Rows.Count > 0)
                {
                    // fill business object
                    var objResult = (T)CreateObjectFromDataSet(typeof(T), dt.Rows[0], objProperties, arrOrdinals);
                    return objResult;
                }
            }
	        catch
	        {
		        // Ignored
	        }

	        return Null<T>.GetObjectNull();
        }

        #region private

        /// <summary>
        /// Tạo một đối tượng từ datarow
        /// </summary>
        /// <param name="objType"></param>
        /// <param name="dr"></param>
        /// <param name="objProperties"></param>
        /// <param name="arrOrdinals"></param>
        /// <returns></returns>
        private static object CreateObjectFromDataSet(Type objType, DataRow dr, IDictionary objProperties, IDictionary arrOrdinals)
        {
            try
            {
                var objObject = Activator.CreateInstance(objType);

	            foreach (DictionaryEntry de in arrOrdinals)
                {
                    var fieldname = de.Key.ToString();
                    var possition = (int)arrOrdinals[fieldname];
                    var propertyInfo = (PropertyInfo)objProperties[fieldname];

	                if (!propertyInfo.CanWrite || possition == -1 || dr[possition] == DBNull.Value) continue;
	                switch (propertyInfo.PropertyType.FullName)
	                {
		                case "System.Enum":
			                propertyInfo.SetValue(objObject, Enum.ToObject(propertyInfo.PropertyType, dr[possition]), null);
			                break;
		                case "System.String":
			                propertyInfo.SetValue(objObject, (string)dr[possition], null);
			                break;
		                case "System.Boolean":
			                propertyInfo.SetValue(objObject, (bool)dr[possition], null);
			                break;
		                case "System.Decimal":
			                propertyInfo.SetValue(objObject, Convert.ToDecimal(dr[possition]), null);
			                break;
		                case "System.Int16":
			                propertyInfo.SetValue(objObject, Convert.ToInt16(dr[possition]), null);
			                break;
		                case "System.Int32":
			                propertyInfo.SetValue(objObject, Convert.ToInt32(dr[possition]), null);
			                break;
		                case "System.Int64":
			                propertyInfo.SetValue(objObject, Convert.ToInt64(dr[possition]), null);
			                break;
		                case "System.DateTime":
			                propertyInfo.SetValue(objObject, Convert.ToDateTime(dr[possition]), null);
			                break;
		                case "System.Double":
			                propertyInfo.SetValue(objObject, Convert.ToDouble(dr[possition]), null);
			                break;
		                default:
			                // try explicit conversion
			                propertyInfo.SetValue(objObject, Convert.ChangeType(dr[possition], propertyInfo.PropertyType), null);
			                break;
	                }
                }

                return objObject;
            }
            catch
            {
                return Activator.CreateInstance(objType);
            }
        }

        /// <summary>
        /// Lấy tên thuộc tính của 1 kiểu dữ liệu
        /// </summary>
        /// <param name="objType"></param>
        /// <returns></returns>
        private static Hashtable GetPropertyInfo(Type objType)
        {
            var hashProperties = new Hashtable();
            foreach (var objProperty in objType.GetProperties())
            {
                hashProperties[objProperty.Name.ToUpper()] = objProperty;
            }

            return hashProperties;
        }

        /// <summary>
        /// Gán thứ tự cột trong dataset theo tên thuộc tính
        /// </summary>
        /// <param name="hashProperties"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static Hashtable GetOrdinalsFromDataSet(Hashtable hashProperties, DataSet dt)
        {
            var arrOrdinals = new Hashtable();
            if (dt.Tables.Count > 0)
            {
                for (var i = 0; i < dt.Tables[0].Columns.Count; i++)
                {
                    if (hashProperties.ContainsKey(dt.Tables[0].Columns[i].ColumnName.ToUpper()))
                        arrOrdinals[dt.Tables[0].Columns[i].ColumnName.ToUpper()] = i;
                }
            }

            return arrOrdinals;
        }

        private static Hashtable GetOrdinalsFromDataTable(Hashtable hashProperties, DataTable dt)
        {
            var arrOrdinals = new Hashtable();

            for (var i = 0; i < dt.Columns.Count; i++)
            {
                if (hashProperties.ContainsKey(dt.Columns[i].ColumnName.ToUpper()))
                    arrOrdinals[dt.Columns[i].ColumnName.ToUpper()] = i;
            }

            return arrOrdinals;
        }

        #endregion
    }
}

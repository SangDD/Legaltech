namespace DataAccess.ModuleMemoryData
{
	using System;
	using System.Data;

	using Common;
	using Oracle.DataAccess.Client;

	public class AllCodeDA
	{
		public static DataSet GetAllInAllCode()
		{
			var ds = new DataSet();
			try
			{
				ds = OracleHelper.ExecuteDataset(
					Configuration.connectionString,
					CommandType.StoredProcedure,
					"pkg_allcode.proc_AllCode_GetAll",
					new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
			}
			catch (Exception ex)
			{
				Logger.LogException(ex);
			}

			return ds;
		}
	}
}

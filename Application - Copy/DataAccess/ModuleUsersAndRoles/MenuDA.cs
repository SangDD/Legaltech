namespace DataAccess.ModuleUsersAndRoles
{
	using System;
	using System.Data;

	using Common;

	using Oracle.DataAccess.Client;

	public class MenuDA
	{
		public static DataSet GetAllMenu()
		{
			var ds = new DataSet();
			try
			{
				ds = OracleHelper.ExecuteDataset(
					Configuration.connectionString,
					CommandType.StoredProcedure,
					"pkg_s_menu.proc_Menu_GetAll",
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

namespace DataAccess.ModuleUsersAndRoles
{
	using System;
	using System.Data;

	using CommonData;

	using Oracle.DataAccess.Client;

	public class MenuDA
	{
		public static DataSet GetAllMenu()
		{
			var ds = new DataSet();
			try
			{
				ds = OracleHelper.ExecuteDataset(
					Configuration.GetConnectionString(),
					CommandType.StoredProcedure,
					"pkg_s_menu.proc_Menu_GetAll",
					new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
			}
			catch (Exception ex)
			{
                LogInfo.LogException(ex);
			}

			return ds;
		}
	}
}

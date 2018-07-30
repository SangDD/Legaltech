using System;
using System.Data;
using Common;
using ObjectInfos;
using Oracle.DataAccess.Client;

namespace DataAccess
{
    public class AppDDSHCN_DA
    {
        public DataSet AppDDSHCNGetAll()
        {
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_APP_DDSHCN.PROC_APPDDSHCN_GETALL",
                new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public decimal AppDDSHCNInsert(AppDDSHCNInfo pInfo)
        {
            try
            {
                var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_APP_DDSHCN.PROC_APP_DDSHCN_INSERT",
                    new OracleParameter("P_NAME_VI", OracleDbType.Varchar2, pInfo.Name_Vi, ParameterDirection.Input),
                    new OracleParameter("P_ADDRESS_VI", OracleDbType.Varchar2, pInfo.Address_Vi, ParameterDirection.Input),
                    new OracleParameter("P_NAME_EN", OracleDbType.Varchar2, pInfo.Address_En, ParameterDirection.Input),
                    new OracleParameter("P_ADDRESS_EN", OracleDbType.Varchar2, pInfo.Address_En, ParameterDirection.Input),
                    new OracleParameter("P_PHONE", OracleDbType.Varchar2, pInfo.Phone, ParameterDirection.Input),
                    new OracleParameter("P_FAX", OracleDbType.Varchar2, pInfo.Fax, ParameterDirection.Input),
                    new OracleParameter("P_EMAIL", OracleDbType.Varchar2, pInfo.Email, ParameterDirection.Input),
                    new OracleParameter("P_CREATEDDATE", OracleDbType.Date, pInfo.Createddate, ParameterDirection.Input),
                    new OracleParameter("P_CREATEDBY", OracleDbType.Varchar2, pInfo.Createdby, ParameterDirection.Input),
                    paramReturn);

                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal AppDDSHCNUpdate(AppDDSHCNInfo pInfo)
        {
            try
            {
                var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_APP_DDSHCN.PROC_APP_DDSHCN_UPDATE",
                    new OracleParameter("P_ID", OracleDbType.Decimal, pInfo.Id, ParameterDirection.Input),
                    new OracleParameter("P_NAME_VI", OracleDbType.Varchar2, pInfo.Name_Vi, ParameterDirection.Input),
                    new OracleParameter("P_ADDRESS_VI", OracleDbType.Varchar2, pInfo.Address_Vi, ParameterDirection.Input),
                    new OracleParameter("P_NAME_EN", OracleDbType.Varchar2, pInfo.Address_En, ParameterDirection.Input),
                    new OracleParameter("P_ADDRESS_EN", OracleDbType.Varchar2, pInfo.Address_En, ParameterDirection.Input),
                    new OracleParameter("P_PHONE", OracleDbType.Varchar2, pInfo.Phone, ParameterDirection.Input),
                    new OracleParameter("P_FAX", OracleDbType.Varchar2, pInfo.Fax, ParameterDirection.Input),
                    new OracleParameter("P_EMAIL", OracleDbType.Varchar2, pInfo.Email, ParameterDirection.Input),
                    new OracleParameter("P_MODIFIEDDATE", OracleDbType.Date, pInfo.Modifieddate, ParameterDirection.Input),
                    new OracleParameter("P_MODIFIEDBY", OracleDbType.Varchar2, pInfo.Modifiedby, ParameterDirection.Input),
                    paramReturn);

                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal AppDDSHCNDeleted(decimal pID, string pModifiedBy, DateTime pModifiedDate)
        {
            try
            {
                var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_APP_DDSHCN.PROC_APP_DDSHCN_DELETED",
                    new OracleParameter("P_ID", OracleDbType.Decimal, pID, ParameterDirection.Input),
                    new OracleParameter("P_MODIFY_BY", OracleDbType.Varchar2, pModifiedBy, ParameterDirection.Input),
                    new OracleParameter("P_MODIFY_DATE", OracleDbType.Date, pModifiedDate, ParameterDirection.Input),
                    paramReturn);

                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }


        public DataSet AppDDSHCNGetByID(decimal pID)
        {
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_APP_DDSHCN.PROC_APPDDSHCN_GETBYID",
                new OracleParameter("P_ID", OracleDbType.Decimal, pID, ParameterDirection.Input),
                new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }
    }
}

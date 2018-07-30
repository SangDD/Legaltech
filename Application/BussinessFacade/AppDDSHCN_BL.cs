using Common;
using DataAccess;
using ObjectInfos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessFacade
{
    public class AppDDSHCN_BL
    {
        public List<AppDDSHCNInfo> AppDDSHCNGetAll()
        {
            try
            {
                AppDDSHCN_DA _da = new AppDDSHCN_DA();
                DataSet _ds = _da.AppDDSHCNGetAll();
                return CBO<AppDDSHCNInfo>.FillCollectionFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<AppDDSHCNInfo>();
            }
        }


        public AppDDSHCNInfo AppDDSHCNGetById(decimal pID)
        {
            try
            {
                AppDDSHCN_DA _da = new AppDDSHCN_DA();
                DataSet _ds = _da.AppDDSHCNGetByID(pID);
                return CBO<AppDDSHCNInfo>.FillObjectFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new AppDDSHCNInfo();
            }
        }


        public decimal AppDDSHCNInsert(AppDDSHCNInfo pInfo)
        {
            try
            {
                AppDDSHCN_DA _da = new AppDDSHCN_DA();
                return _da.AppDDSHCNInsert(pInfo);
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
                AppDDSHCN_DA _da = new AppDDSHCN_DA();
                return _da.AppDDSHCNUpdate(pInfo);
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
                AppDDSHCN_DA _da = new AppDDSHCN_DA();
                return _da.AppDDSHCNDeleted(pID, pModifiedBy, pModifiedDate);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

    }
}

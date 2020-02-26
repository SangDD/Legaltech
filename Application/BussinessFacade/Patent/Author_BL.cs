using Common;
using ObjectInfos;
using System;
using DataAccess;
using System.Collections.Generic;

namespace BussinessFacade
{
    public class Author_BL
    {
        public decimal Insert(List<AuthorsInfo> pInfo)
        {
            try
            {
                Author_DA _obj_da = new Author_DA();
                return _obj_da.Insert(pInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public decimal Update(AuthorsInfo pInfo)
        {
            try
            {
                Author_DA _obj_da = new Author_DA();
                return _obj_da.Update(pInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public int Deleted(string p_case_code, string pLanguage)
        {
            try
            {
                Author_DA _obj_da = new Author_DA();
                return _obj_da.Deleted(p_case_code, pLanguage);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

    }

    public class Inventor_BL
    {
        public decimal Insert(List<Inventor_Info> pInfo)
        {
            try
            {
                Inventor_DA _obj_da = new Inventor_DA();
                return _obj_da.Insert(pInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public decimal Update(Inventor_Info pInfo)
        {
            try
            {
                Inventor_DA _obj_da = new Inventor_DA();
                return _obj_da.Update(pInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public int Deleted(string p_case_code, string pLanguage)
        {
            try
            {
                Inventor_DA _obj_da = new Inventor_DA();
                return _obj_da.Deleted(p_case_code, pLanguage);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

    }
}

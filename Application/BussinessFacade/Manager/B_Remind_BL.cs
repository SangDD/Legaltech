using Common;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessFacade
{
    public class B_Remind_BL
    {
        public int Auto_change_remind()
        {
            try
            {
                B_Remind_DA _B_Remind_DA = new B_Remind_DA();
                return _B_Remind_DA.Auto_change_remind();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }
    }
}

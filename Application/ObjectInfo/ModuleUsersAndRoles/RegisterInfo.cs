using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectInfos.ModuleUsersAndRoles
{
    public class RegisterInfo
    {
        public decimal Id { get; set; }
        public string FistName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Company { get; set; }
        public decimal Status { get; set; }
        public string Modifiedby { get; set; }
        public DateTime ModifiedDate { get; set; }

        public DateTime CreateDate { get; set; }
    }
}

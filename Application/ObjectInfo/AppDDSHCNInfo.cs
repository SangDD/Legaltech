using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectInfos
{
    public class AppDDSHCNInfo
    {
        public decimal STT { get; set; }
        public decimal Id { get; set; }
        public string Name_Vi { get; set; }
        public string Address_Vi { get; set; }
        public string Name_En { get; set; }
        public string Address_En { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public DateTime Createddate { get; set; }
        public string Createdby { get; set; }
        public DateTime Modifieddate { get; set; }
        public string Modifiedby { get; set; }
        public decimal Deleted { get; set; }
        public string NguoiDDSH { get; set; }
        public string MaNguoiDaiDien { get; set; }
    }
}

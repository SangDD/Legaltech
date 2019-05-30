using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectInfos
{
    /// <summary>
    /// Dùng chung cho một số info khác 
    /// </summary>
    public class Other_MasterInfo
    {
        public string Master_Name { set; get; }

        public string Master_Address { set; get; }

        public string Master_Phone { set; get; }

        public string Master_Fax { set; get; }
        public string Master_Email { set; get; }

        public string TacGiaDongThoi { set; get; }
        public string PhoBan { set; get; }

    }

    public class AppAuthorsInfo
    {
        public string Author_Name { set; get; }

        public string Author_Address { set; get; }

        public string Author_Phone { set; get; }

        public string Author_Fax { set; get; }
        public string Author_Email { set; get; }

        public decimal Author_Country { set; get; }
    }

    public class UTienInfo
    {
        public string UT_SoDon { set; get; }
        public string UT_NgayNopDon { set; get; }
        public string UT_QuocGia { set; get; }
        public string UT_Type { set; get; }
        public string UT_ThoaThuanKhac { set; get; }
    }
}

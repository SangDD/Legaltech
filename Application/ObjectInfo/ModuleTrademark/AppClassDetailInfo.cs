using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectInfos
{
    public class AppClassDetailInfo
    {
        public AppClassDetailInfo()
        {
            IDREF = 0;
        }
        public decimal Id { get; set; }
        public decimal IDREF { get; set; }
        public string Textinput { get; set; }
        public string Code { get; set; }
        public decimal App_Header_Id { get; set; }

        public string Languague_Code { get; set; }

        public string TongSoNhom { get; set; }

        public string TongSanPham { get; set; }

        public int IntTongSanPham { get; set; }

        public object CloneObj()
        {
            return this.MemberwiseClone();
        }
        public string TextinputVI { get; set; }
    }
}

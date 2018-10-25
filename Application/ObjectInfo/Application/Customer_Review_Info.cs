using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ObjectInfos
{
    public class Customer_Review_Info
    {
        public string Note { get; set; }

        public HttpPostedFileBase File_Atachment { get; set; }
        public string Url_File_Atachment { get; set; }
        public decimal Status { get; set; }

        public string Case_Code { get; set; }
        public decimal Id { get; set; }

    }
}

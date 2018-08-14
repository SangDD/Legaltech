using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ObjectInfos
{

    public class AppDocumentOthersInfo
    {
        public decimal Id { get; set; }
        public decimal App_Header_Id { get; set; }
        public string Documentname { get; set; }
        public string Filename { get; set; }
        public decimal Deleted { get; set; }

        public string keyFileUpload { get; set; }
        public string Language_Code { get; set; }

    }
}

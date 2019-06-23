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
        public AppDocumentOthersInfo()
        {
            IdRef = 0;
            FILETYPE = 1;
            FILELEVEL = 1;
        }
        public decimal Id { get; set; }
        public decimal App_Header_Id { get; set; }
        public string Documentname { get; set; }
        public string DocumentnameVi { get; set; }
        public string Filename { get; set; }
        public decimal Deleted { get; set; }

        public string keyFileUpload { get; set; }
        public string Language_Code { get; set; }

        public decimal IdRef { get; set; }
        public decimal FILETYPE { get; set; }// hungtd them: phục vụ 1 đơn có nhiều loại như mục tài liệu khác
        public decimal FILELEVEL { get; set; }// hungtd them: phục vụ 1 đơn có nhiều loại như mục tài liệu khác

        public string Char01 { get; set; }// hungtd them:  tài liệu khác có thêm số trang, ghi chú

        public string Char02 { get; set; }// hungtd them:  tài liệu khác có thêm số trang, ghi chú

    }
}

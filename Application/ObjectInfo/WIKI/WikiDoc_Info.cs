using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectInfos
{
  public  class WikiDoc_Info
    {
        public decimal STT { set; get; }

        public decimal ID { set; get; }

        public string TITLE { set; get; }

        public string CONTENT { set; get; }

        public decimal VIEW_NUMBER { set; get; }

        public string LANGUAGE_CODE { set; get; }

        public string HASHTAG { set; get; }

        public string FILE_URL01 { set; get; }

        public string FILE_URL02{ set; get; }

        public string FILE_URL03 { set; get; }

        public decimal STATUS { set; get; }

        public decimal CATA_ID { set; get; }

        public string CATA_NAME { set; get; }

        public string CREATED_BY { set; get; }

        public string MODIFIED_BY { set; get; }

        public DateTime CREATED_DATE { set; get; }

        public DateTime MODIFIED_DATE { set; get; }
    }
}

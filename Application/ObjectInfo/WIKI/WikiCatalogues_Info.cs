using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectInfos
{
   public class WikiCatalogues_Info
    {
        public decimal STT { set; get; }

        public decimal ID { set; get; }

        public decimal CATA_LEVEL { set; get; }

        public string NAME { set; get; }

        public string NAME_ENG { set; get; }

        public decimal PARENT_ID { set; get; }

        public decimal CHILD_NUM { set; get; }

        public string PARENT_NAME { set; get; }

        public string PARENT_NAME_EN { set; get; }
        

        public string CREATED_BY { set; get; }

        public string MODIFIED_BY { set; get; }

        public DateTime CREATED_DATE { set; get; }

        public DateTime MODIFIED_DATE { set; get; }

        public decimal NUMBER_DOC { set; get; }

    }
}

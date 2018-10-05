using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectInfos
{
    public class SearchObject_Detail_Info
    {
        public decimal ID { get; set; }

        public decimal SEARCH_ID { get; set; }

        public decimal SEARCH_TYPE { get; set; }

        public string SEARCH_TYPE_NAME { get; set; }

        public decimal SEARCH_OBJECT { get; set; }

        public string SEARCH_OBJECT_NAME { get; set; }

        public decimal IS_FIRST { get; set; }
    }
}

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
        public string SEARCH_TYPE { get; set; }
        public string SEARCH_VALUE { get; set; }
        public string SEARCH_OPERATOR { get; set; }

        public string ANDOR { get; set; }
    }
}

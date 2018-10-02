using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectInfos
{
  public  class SearchObject_Question_Info: SearchObject_Header_Info
    {
        public decimal QUESTION_ID { get; set; }

        public string SUBJECT { get; set; }

        public string RESULT { get; set; }

        public string FILE_URL { get; set; }

        public string FILE_URL02 { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ObjectInfo
{
    [DataContract()]
    public class Productmark_Info
    {

        [DataMember]
        public string Productmarkcode { get; set; }

        [DataMember]
        public string Markname { get; set; }

        [DataMember]
        public Byte Logo { get; set; }
    }
}

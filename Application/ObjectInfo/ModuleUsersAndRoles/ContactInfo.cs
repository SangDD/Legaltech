using System.Runtime.Serialization;

namespace ObjectInfo
{
    [DataContract()]
    public class Contact_Info
    {

        [DataMember]
        public string Contactid { get; set; }

        [DataMember]
        public string Provincecode { get; set; }

        [DataMember]
        public string Districtcode { get; set; }

        [DataMember]
        public string Wardcode { get; set; }

        [DataMember]
        public string Contactname { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string Tel { get; set; }

        [DataMember]
        public string Mobile { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Website { get; set; }
    }
}

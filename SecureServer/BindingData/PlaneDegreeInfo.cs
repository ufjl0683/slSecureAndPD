using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SecureServer.BindingData
{
    [DataContract]
    public class PlaneDegreeInfo
    {
        [DataMember]
        public int ERID { get; set; }
        [DataMember]
        public int PlaneID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int AlarmStatus { get; set; }
    }
}

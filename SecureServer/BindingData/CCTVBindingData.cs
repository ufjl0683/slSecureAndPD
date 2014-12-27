using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SecureServer.BindingData
{
    [DataContract]
    public   class CCTVBindingData
    {
        [DataMember]
       public  int CCTVID { get; set; }

        [DataMember]

        public int PlaneID { get; set; }
        [DataMember]
        public string  CCTVName { get; set; }

        [DataMember]
       public string MjpegCgiString { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string IP { get; set; }

        [DataMember]
        public int Port { get; set; }
       

    }
}

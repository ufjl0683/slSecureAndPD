using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SecureServer.BindingData
{
    [DataContract]
    public   class DoorBindingData
    {
          [DataMember]
         public string ControlID { get; set; }
          [DataMember]
          public bool IsConnected { get; set; }
          [DataMember]
          public bool IsDoorOpen { get; set; }
          [DataMember]
          public string DoorColorString { get; set; }
          
    }

    //[DataContract]
    //public class CardReaderInfo
    //{
    //    [DataMember]
    //    public string ControlID { get; set; }
    //    [DataMember]
    //    public bool IsConnected { get; set; }
    //    [DataMember]
    //    public bool IsDoorOpen { get; set; }
    //    [DataMember]
    //    public SolidColorBrush DoorColor { get; set; }


    //}
}

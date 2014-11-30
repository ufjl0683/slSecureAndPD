using SecureServer.CardReader.BindingData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SecureServer.CardReader
{

    public enum AlarmType
    {
        Secure

    }

    [DataContract]
  public   class AlarmData
    {
        [DataMember]
      public DateTime TimeStamp { get; set; }
          [DataMember]
        public AlarmType AlarmType { get; set; }
          [DataMember]
      public string Description { get; set; }
          [DataMember]
      public int PlaneID { get; set; }
          [DataMember]
       public bool IsForkCCTVEvent { get; set; }
          [DataMember]
      public string PlaneName { get; set; }
          [DataMember]
      public string ColorString { get; set; }
          //[DataMember]
      //public string CCTVUrl { get; set; }
      //    [DataMember]
      //public string UserName { get; set; }
      //    [DataMember]
      //public string Password { get; set; }
          [DataMember]
          public CCTVBindingData CCTVBindingData { get; set; }

          [DataMember]

          public string TimeStampString
          {
              get
              {
                  return TimeStamp.ToString("hh:mm");
              }

              set
              {
              }

          }
    }

      
    }


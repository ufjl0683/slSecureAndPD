using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SecureServer.R13Exchange
{
   public static  class EventExchange
    {
     static  System.Collections.Concurrent.ConcurrentQueue<EventExchangeData> Queue=new System.Collections.Concurrent.ConcurrentQueue<EventExchangeData>();

    }


   [DataContract]
    public class EventExchangeData{
     //  [ room_id ] 機房索引 [ event_id ] 事件索引 [ event_status ] 狀態代號(正常; // 0, 斷線; // 1) [ contents ] 事件說明（事件說明或⼦子項⺫⽬目說明（）） [ start_time ] 發⽣生時間 

       [DataMember]
       public string room_id { get; set; }
          [DataMember]
       public string event_id { get; set; }
          [DataMember]
       public string event_status { get; set; }
          [DataMember]
       public string contents { get; set; }
          [DataMember]
          public DateTime start_time { get; set; }
    }
}

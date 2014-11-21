using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureServer.CardReader
{
  public   class AlarmData
    {

      public DateTime TimeStamp { get; set; }
      public string AlarmType { get; set; }
      public string Description { get; set; }
      public int PlaneID { get; set; }

      public string PlaneName { get; set; }
      public string ColorString { get; set; }
      
    }
}

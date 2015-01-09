using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SecureServer.BindingData
{
    [DataContract]
   public   class ItemBindingData
    {
       public ItemBindingData()
       {

       }
          [DataMember]
       public int ItemID
       {
           get;
           set;
       }
          [DataMember]
       public string Type
       {
           get;
           set;
       }
          [DataMember]
       public string Content
       {
           get;
           set;
       }
          [DataMember]
       public string ColorString
       {
           get;
           set;
       }
          [DataMember]
       public int PlaneID { get; set; }
          [DataMember]
         public double Value { get; set; }
    }
}

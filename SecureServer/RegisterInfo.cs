using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureServer
{
   public  class RegisterInfo
    {
      public  string PCName { get; set; }
      public string Key { get; set; }
      public ISecureServiceCallBack CallBack { get; set; }
     public   RegisterInfo()
       {
       }


    }
}

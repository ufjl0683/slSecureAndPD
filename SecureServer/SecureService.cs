using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace SecureServer
{
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.Single)] 
   public  class SecureService:ISecureService
    {

        public System.Collections.Generic.Dictionary<string, ISecureServiceCallBack> dictClientCallBacks = new Dictionary<string, ISecureServiceCallBack>();

        public string Register(string PCName)
        {

            string GUID=Guid.NewGuid().ToString();
           
            dictClientCallBacks.Add(GUID,OperationContext.Current.GetCallbackChannel<ISecureServiceCallBack>());

            return GUID;
          //  throw new NotImplementedException();
        }

        public string Register()
        {
            throw new NotImplementedException();
        }
    }
}

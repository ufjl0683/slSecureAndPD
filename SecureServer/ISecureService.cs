using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace SecureServer
{
    [ServiceContract]
   public  interface ISecureService
    {
        [OperationContract]
        string  Register();
    }


    public interface ISecureServiceCallBack
    {

        
        [OperationContract(IsOneWay = true)]
        void SayHello(string hello);

        
    }

     [DataContract]
    public class SecureServiceRegistInfo
    {

    }
}

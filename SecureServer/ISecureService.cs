using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace SecureServer
{
    [ServiceContract(CallbackContract=typeof(ISecureServiceCallBack))]
   public  interface ISecureService
    {
        [OperationContract]
        string  Register(string pcname);
         [OperationContract]
        void NotifyDBChange(DBChangedConstant constant);
         [OperationContract]
         void ToServerHello();
         [OperationContract]
         void UnRegist(string guid);

         [OperationContract]
         void ForceOpenDoor(string ControllID);
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


    public enum DBChangedConstant
    {
        AuthorityChanged
    }
}

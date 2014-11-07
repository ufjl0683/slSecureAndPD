using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace test
{
    class Program 
    {
        static void Main(string[] args)
        {
            new Wrapper();

           Console.ReadKey();
        }

        
    }


    public class Wrapper : RemoteService.ISecureServiceCallback
    {
        public Wrapper()
        {
            RemoteService.SecureServiceClient client = new RemoteService.SecureServiceClient(new System.ServiceModel.InstanceContext(this), "CustomBinding_ISecureService1");

          string Key=  client.Register(Environment.MachineName);
          Console.WriteLine("Key:" + Key);
          
        }

    
public void SayHello(string hello)
{
    Console.WriteLine("Test from Server!");
}
}
}

using ModbusTCP;
//using SecureServer.CardReader;
//using SecureServer.RTU;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using test.MCNSService;
using test.RemoteService;

namespace test
{
    class Program
    {
        static RTU rtu = new RTU("rtu-101", 1, "10.21.12.200", 502, 2001, 46,0);
        static void Main(string[] args)
        {


            bool status = RoomClient.RoomClient.GetControlConnectionStatus("AN-2400N-1");

         Console.WriteLine(status);

         //   System.Collections.BitArray bary = new System.Collections.BitArray(new byte[2]);

         //   bary.Set(8, true);
         //   byte[]kk=new byte[2];
         //   bary.CopyTo(kk,0);
         //   Console.WriteLine("{0:X4}",BitConverter.ToUInt16(kk,0));
         ////   Console.WriteLine(null == 1);

         //   MCNSService.MCNSServiceSoapClient client = new MCNSService.MCNSServiceSoapClient();
         //   AddCardInfo info = new AddCardInfo();
         //   info.CardNo = "1232828117";
         //   info.ERIDs = new ArrayOfInt { 1, 3, 5 };
         //   info.StartDate = DateTime.Now;
         //   info.EndDate = info.StartDate.AddDays(3);
         //   info.MCNSID = "333333";
         //   string res = client.AddCard(info);
         //   Console.WriteLine(res);
           // info.MCNSID

      
          //  Wrapper w = new Wrapper();


           // client.AddCard(new MCNSService.AddCardInfo() { CardNo = "0988163835", ERIDs = new int[] { 1 }, MCNSID });

          //  System.Collections.ArrayList list = new System.Collections.ArrayList();
          //  byte[] data = new byte[100]; ;
          //  ArrayOfString ary = new ArrayOfString();
          //  ary.Add("aa");
          //  ary.Add("bb");
          //  ary.Add("cc");
          //  ServiceReference1.MCNSServiceSoapClient client = new ServiceReference1.MCNSServiceSoapClient();
          //string   res=  client.AddCard(new AddCardInfo() { CardNo = "111222333445", ERNOs =ary, MCNSID = "999999" });
          //Console.WriteLine(res);
//  
          //  RemoteService.SecureServiceClient client = new SecureServiceClient();
          
          //  Master modbus = new Master();
         //   modbus.connect("192.192.85.20", 502);
          //  modbus.OnResponseData += modbus_OnResponseData;
          //  modbus.ReadHoldingRegister(1, 0, 0, 100);

          
          //  modbus.ReadHoldingRegister(1, 1, 2000, 46, ref data);
            
            //  new Wrapper();
            //CCTV_TYPE1 cctv=   new CCTV_TYPE1("192.192.85.20", 11000, "admin", "pass");
            //cctv.Preset(1);
            //  Console.ReadKey();
            //  cctv.Preset(3);

          
         //   RTU rtu = new RTU("rtu-101", 1, "192.168.0.10", 502, 0001, 46);
            //while (!rtu.IsConnected) ;
            ////for (int i = 1; i <= 22; i++)
            ////    rtu.WriteRegister((ushort)i, (ushort)(i - 2001 + 1));
            //while (true)
            //{
           // new System.Threading.Thread(SendTask).Start();
           //while (true)
           //{
           //    int? temp = rtu.GetRegisterReading(2045);
           //    int? r = rtu.GetRegisterReading(2046);

           //    Console.WriteLine(temp == null ? "null" : "temp:" + (temp/10.0).ToString());
           //    Console.WriteLine(temp == null ? "null" : "r:" + (r/10.0).ToString());
           //    Console.WriteLine("di0:"+ ((rtu.GetRegisterReading(2001)>>8) &0x01)  );
           //    Console.WriteLine("di1:"+((rtu.GetRegisterReading(2001)>>9) *0x01));
           //    Console.WriteLine("switch1:" + ((rtu.GetRegisterReading(2011) >> 0) & 0x01));
           //    Console.WriteLine("switch2:" + ((rtu.GetRegisterReading(2011) >> 1) & 0x01));
           //    Console.WriteLine("do:"+rtu.GetRegisterReading(2009));
           //    System.Threading.Thread.Sleep(1000);
               
           //}
       //   Console.WriteLine(rtu.GetRegisterReading(0043));
         // Console.WriteLine(rtu.GetRegisterReading());
            //    System.Threading.Thread.Sleep(1000);
            //}
            Console.ReadKey();
        }

        static void SendTask()
        {

            while (true)
            {
                rtu.WriteRegister(2009, 1);

                System.Threading.Thread.Sleep(2000);
                rtu.WriteRegister(2009, 0);
                System.Threading.Thread.Sleep(2000);
            }
        }

        static void modbus_OnResponseData(ushort id, byte unit, byte function, byte[] data)
        {
          //  throw new NotImplementedException();
        }


        public class Wrapper : RemoteService.ISecureServiceCallback
        {
            public Wrapper()
            {
                RemoteService.SecureServiceClient client = new RemoteService.SecureServiceClient(new System.ServiceModel.InstanceContext(this), "CustomBinding_ISecureService");
                DoorBindingData[] data = client.GetALLDoorBindingData(2);
                string Key = client.Register(Environment.MachineName);
                Console.WriteLine(data.Length);
                Console.WriteLine("Key:" + Key);

          

            }


            public void SayHello(string hello)
            {
                Console.WriteLine("Test from Server!");
            }


            public void SecureDoorEvent(DoorEventType evttype, DoorBindingData doorBindingData)
            {
                throw new NotImplementedException();
            }

            public void SecureAlarm(AlarmData alarmdata)
            {
                throw new NotImplementedException();
            }


            public void ItemValueChangedEvenr(ItemBindingData ItemBindingData)
            {
                throw new NotImplementedException();
            }
        }

        public interface ICCTV
        {

            void Preset(int preset);
            string GetMjpgCGIString(string UserName, string Password);
        }

        public class CCTV_TYPE1 : ICCTV
        {
            public string UserName { get; set; }
            public string Password { get; set; }
            public string IP { get; set; }
            public int Port { get; set; }

            public CCTV_TYPE1(string IP, int Port, string UserName, string Pasword)
            {
                this.UserName = UserName;
                this.Password = Password;
                this.IP = IP;
                this.Port = Port;
            }
            public void Preset(int preset)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + IP + ":" + Port + "/ptzpreset?camid=1&goto_preset=" + preset);
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
                request.Credentials = new NetworkCredential(UserName, Password);

                try
                {


                    WebResponse response = request.GetResponse();

                    // System.IO.StreamReader rd = new System.IO.StreamReader(response.GetResponseStream());
                    //string ret = rd.ReadToEnd();
                    //Console.WriteLine(ret);
                }
                catch (Exception ex)
                {
                    //  Console.WriteLine(ex.Message + "," + ex.StackTrace);
                }

                // throw new NotImplementedException();
            }




            public string GetMjpgCGIString(string UserName, string Password)
            {
                return "http://" + IP + ":" + Port + "/getimage";
            }
        }
    }
}

﻿using ModbusTCP;
//using SecureServer.CardReader;
//using SecureServer.RTU;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using test.RemoteService;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Collections.ArrayList list = new System.Collections.ArrayList();
            byte[] data = new byte[100]; ;
            Master modbus = new Master();
            modbus.connect("192.168.0.10", 502);
          //  modbus.OnResponseData += modbus_OnResponseData;
          //  modbus.ReadHoldingRegister(1, 0, 0, 100);
            modbus.ReadHoldingRegister(1, 1, 000, 100, ref data);
            
            //  new Wrapper();
            //CCTV_TYPE1 cctv=   new CCTV_TYPE1("192.192.85.20", 11000, "admin", "pass");
            //cctv.Preset(1);
            //  Console.ReadKey();
            //  cctv.Preset(3);

          //  RTU rtu = new RTU("rtu-101", 1, "192.168.10.50", 502,2001, 46);
         //   RTU rtu = new RTU("rtu-101", 1, "192.168.0.10", 502, 0001, 46);
            //while (!rtu.IsConnected) ;
            ////for (int i = 1; i <= 22; i++)
            ////    rtu.WriteRegister((ushort)i, (ushort)(i - 2001 + 1));
            //while (true)
            //{

            //    int? temp = rtu.GetRegisterReading(0021);
            //    Console.WriteLine(temp == null ? "null" : temp.ToString());
            //    Console.WriteLine(rtu.GetRegisterReading( 0043));
            //    Console.WriteLine(rtu.ToString());
            //    System.Threading.Thread.Sleep(1000);
            //}
            Console.ReadKey();
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
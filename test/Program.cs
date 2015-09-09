using ModbusTCP;
using RoomInterface;
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
    class User
    {
        public string MCNSID { get; set; }
        public string CardNo { get; set; }
        public int[] ERIDs { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Name { get; set; }
    }

    class Program
    {
     //   static RTU rtu = new RTU("rtu-101", 1, "10.21.12.200", 502, 2001, 46,0);
        //static void CallBack(object o)
        //{
        //    Console.WriteLine("hi");
        //    System.Threading.Thread.Sleep(40000);
        //}
        


        static void Main(string[] args)
        {
            //byte[] retData = new byte[6];

            //System.Collections.BitArray ba = new System.Collections.BitArray(new byte[] { 0 });
            //// bit   0       1      2          3
            ////      major   minor  SmrWarning AcFail
            //ba.Set(0, 1 == 0);
            //ba.Set(1, 0 == 0);
            //ba.Set(2, 1 == 0);
            //ba.Set(3, 0 == 0);
            //ba.CopyTo(retData, 5);
            //Console.WriteLine(retData[5]);
           // SecureServer.NVR.NVR_Type1  nvr=new SecureServer.NVR.NVR_Type1(){ ERID=32, NVRID=18, IP="10.2.50.129", PlaneID=41, UserName="admin", Password="1111", Port=80};

           //nvr.SaveRecord(1,new DateTime(2015,8,14,13,0,0),new DateTime(2015,8,14,13,01,0),"123.avi");

           // Console.ReadLine();
         //   RoomClient.RoomClient.RoomEvent += RoomClient_RoomEvent;

         //   bool status = RoomClient.RoomClient.GetControlConnectionStatus("AN-2400N-1");

         //   byte[] data = RoomClient.RoomClient.GetStatus("AN-ADAM-1");

         //   RoomClient.RoomClient.GroupModify(new List<int>(new int[]{1,2}));
             
            //while (true)
            //{
            ////  object[] objs=  RoomClient.RoomClient.GetGroupProgress();

            



            ////  Console.WriteLine(objs.Length);
            //    byte[] dat=RoomClient.RoomClient.GetStatus("AN-ADAM-1");
            // Console.WriteLine( dat[3]);
            // Console.WriteLine(dat[4]);
            // Console.WriteLine(dat[5]);
            //  Console.ReadKey();
            //}
        //    bool success = false;

        //    success = RoomClient.RoomClient.SetADAMAlarmTime("AN-ADAM-1", 15, 10, 5, 20);
         //   //if (RoomClient.RoomClient.SetTime("AN-2400N-1", DateTime.Now))
         //   //    Console.WriteLine("對時成功!");
         //   //else
         //   //    Console.WriteLine("對時失敗!");

         ////   success = RoomClient.RoomClient.OpenDoor("AN-ADAM-1", 0, "opendoor");
         //  //  bool success= RoomClient.RoomClient.RestaADAMControl("AN-ADAM-1");

         
         //   Console.WriteLine(data[3]);
          //  Console.WriteLine(success);
            // success= RoomClient.RoomClient.RestaADAMControl("AN");
            //foreach (PersonData s in RoomClient.RoomClient.GetRoomPerson("AN"))
            //{
            //    Console.WriteLine(s.Name);
            //}
             
          //System.Threading.Timer tmr = new System.Threading.Timer((object o) =>
          //{
          //    System.Console.Beep();
          //});
          //tmr.Change(0, 1000 * 3);
            //System.Threading.Timer tmr=new System.Threading.Timer(

            //   CallBack
                   
            // );
            //tmr.Change(0, 3000);
          //Console.WriteLine(data[3]);

          //Console.WriteLine(data[4]);
            //RoomClient.RoomClient.GetStatus(
          //  Console.WriteLine(success);
       //  Console.WriteLine(status);

         //   System.Collections.BitArray bary = new System.Collections.BitArray(new byte[2]);

         //   bary.Set(8, true);
         //   byte[]kk=new byte[2];
         //   bary.CopyTo(kk,0);
         //   Console.WriteLine("{0:X4}",BitConverter.ToUInt16(kk,0));
         ////   Console.WriteLine(null == 1);

            MCNSServiceSoapClient client = new MCNSService.MCNSServiceSoapClient();
           MagneticCardBasicInfo [] info=  client.GetAllTempMagneticCardBasicInfo();
           Console.WriteLine(info.Length);
            //AddCardInfo info = new AddCardInfo();
            //info.CardNo = "999999999";
            //info.ERIDs = new ArrayOfInt { 1, 3, 5 };
            //info.StartDate = DateTime.Now;
            //info.EndDate = info.StartDate.AddDays(3);
            //info.MCNSID = "7777777";
            //string res = client.AddCard(info);
            //Console.WriteLine(res);

            //using (SecureDBEntities db = new SecureDBEntities())
            //{
            //    var q = from n in db.vwMagneticCardDetail
            //            where n.Memo.Contains("7777777")
            //            group (int)n.ERID by new { n.ABA, n.Memo, n.StartDate, n.EndDate, n.Name } into g
            //            select new { g.Key.ABA, g, g.Key.Memo, g.Key.EndDate, g.Key.StartDate, Name = g.Key.Name };

            //    System.Collections.Generic.List<User> list = new List<User>();
            //    foreach (var i in q)
            //    {
            //        list.Add(new User() { CardNo = i.ABA, EndDate = (DateTime)i.EndDate, StartDate = (DateTime)i.StartDate, MCNSID = i.Memo, Name = i.Name, ERIDs = i.g.Distinct().ToArray<int>() });
            //    }

            //    User u = list[0];
            // //   return list.ToArray();
            //}

           
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
              
              new Wrapper();
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

        static void RoomClient_RoomEvent(RoomInterface.ControllEventType type, string Name, object obj)
        {

            Console.WriteLine("{0} , {1}", type, Name);
            if (type == RoomInterface.ControllEventType.ADAMStatusChange && ((byte[])obj)[1] == 1)
                Console.WriteLine("合法卡");
            if (type == RoomInterface.ControllEventType.ADAMStatusChange && ((byte[])obj)[32] == 3)
                Console.WriteLine("門沒關");
            //throw new NotImplementedException();
        }

        //static void SendTask()
        //{

        //    while (true)
        //    {
        //        rtu.WriteRegister(2009, 1);

        //        System.Threading.Thread.Sleep(2000);
        //        rtu.WriteRegister(2009, 0);
        //        System.Threading.Thread.Sleep(2000);
        //    }
        //}

        static void modbus_OnResponseData(ushort id, byte unit, byte function, byte[] data)
        {
          //  throw new NotImplementedException();
        }


        public class Wrapper : RemoteService.ISecureServiceCallback
        {
            RemoteService.SecureServiceClient client;
            public Wrapper()
            {
                client = new RemoteService.SecureServiceClient(new System.ServiceModel.InstanceContext(this), "CustomBinding_ISecureService");
                DoorBindingData[] data = client.GetALLDoorBindingData(2);
                string Key = client.Register(Environment.MachineName);
                Console.WriteLine(data.Length);
                Console.WriteLine("Key:" + Key);
                new System.Threading.Thread(Task).Start();
               //bool success= client.SetR23Parameter("AN-2400N-1", 15, 10, 5, 20);
               //Console.WriteLine(success);

            }

            public void Task()
            {
                while (true)
                {
                    Console.WriteLine(client.GetTotalConnection());
                    System.Threading.Thread.Sleep(3000);
                }
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

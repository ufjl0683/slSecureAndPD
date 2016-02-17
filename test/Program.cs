using ModbusTCP;
using RoomInterface;
//using SecureServer.CardReader;
//using SecureServer.RTU;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
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

       static void BatteryPackTest()
        {
           // WebClient client = new WebClient();
           //string s=  client.DownloadString("http://192.192.85.64/secure/slsecure.aspx"/*"http://10.2.190.125:80/stringindex?0_0"*/);
         
           // Regex regex = new Regex(@"(?<v>[0-9]+.[0-9]+)\s*V|(?<temp>[0-9]+.[0-9]+)&deg;C");
           // MatchCollection collection = regex.Matches(s);
           //for(int i=0;i<collection.Count;i++)
           //    Console.WriteLine(collection[i].Groups[(i%2)+1].Value);
           // Console.WriteLine(collection.Count);

            SecureServer.RTU.R13BatteryPackRTU rtu = new SecureServer.RTU.R13BatteryPackRTU("AA", 1, "10.2.190.125", 80, 1, 48, 1);

            while (true)
            {
                for (int i = 0; i < 48; i++)
                {
                    Console.WriteLine(rtu.GetRegisterReading((ushort)(1+ i)));
                }
                System.Threading.Thread.Sleep(1000);
            }
        }

        static void Main(string[] args)
        {
            BatteryPackTest();
            Console.ReadLine();

            //SSOService.SsoWebServiceClient client = new SSOService.SsoWebServiceClient();
            //var res=client.checkAuthentication();
            //Console.WriteLine(res.status);
            //EasyModbus.ModbusClient client = new EasyModbus.ModbusClient();

            //client.Connect("10.21.223.20", (ushort)502);
//
            SecureServer.RTU.R13NewSmrRTU rtu = new SecureServer.RTU.R13NewSmrRTU("AA", 1, "10.2.190.126", 502, 1, 20, 0);
           //ModbusTCP.Master rtu = new ModbusTCP.Master();//.RTU("AA", 2, "10.2.10.159", 503, 1, 20, 0);
            Console.WriteLine("rtu conecting...");
           // rtu.connect("10.2.180.126", 502);
           // Console.WriteLine("rtu ...");
            while (true)
            {
                Console.WriteLine("volt:"+rtu.GetRegisterReading(3));
                Console.WriteLine("amp:" + rtu.GetRegisterReading(4));
                System.Threading.Thread.Sleep(1000);
            }
            Console.ReadKey();
          //  rtu.WriteRegister(1, 1);

          // PowerControl ctl = new PowerControl("10.21.223.20", 502);
            //bool onoff = false;
            //while (true)
            //{
            //    Console.ReadKey();
            //    ctl.SwitchPower(onoff);
            //    onoff = !onoff;
            //    Console.WriteLine(ctl.status);   
            //}

            //bool[] data=null;
           //client.WriteSingleCoil(16,false);

            //byte[] d = null;
            //ModbusTCP.Master client1 = new Master();
            //client1.OnException += client_OnException;
            //client1.connect("10.21.223.20", (ushort)502);
            //client1.WriteSingleCoils(1,1,16,false,ref d);

            //ModbusTCP.Master client = new Master();
            //client.OnException += client_OnException;
            //client.connect("10.21.223.20", (ushort)502);
         
            //while (true)
            //{
            //    byte[] data = null;
               


            
            //    client.ReadCoils(1,1, 0,10, ref data);
            //    if (data != null)
            //        Console.WriteLine(data[0]);
            //    else
            //        Console.WriteLine("null");

            // //   client.WriteSingleCoils(1, 1, 16, true, ref data);
            //    //client.disconnect();
            //    //client.Dispose();
            //    System.Threading.Thread.Sleep(5000);
            //}

        //    R13EventExchange();


            //SecureServer.Meter.R23PowerMeter meter = new SecureServer.Meter.R23PowerMeter(1,"10.21.133.200", 502);
            //while (true)
            //{
            //    Console.WriteLine(meter.VA);
            //    System.Threading.Thread.Sleep(1000);
            //}
/*========================================================= 用電
            ModbusTCP.Master client = new Master();
            client.connect("10.21.233.200", 502);
            byte[] data=new byte[4];
            client.ReadHoldingRegister(1, 0, 2300, 2,ref data
                );
            Console.WriteLine(data[0]);
            byte[] dest = new byte[4];
            dest[0] = data[1];
            dest[1] = data[0];
            dest[2] = data[3];
            dest[3] = data[2];
            Console.WriteLine(System.BitConverter.ToSingle(dest,0));
            
            Console.ReadKey();
====================================================*/


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

       //     bool status = RoomClient.RoomClient.GetControlConnectionStatus("AN-2400N-1");

            //byte[] data = RoomClient.RoomClient.GetStatus("AB");

            //Console.WriteLine(data[0]);

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

            //MCNSServiceSoapClient client = new MCNSService.MCNSServiceSoapClient();
            //MagneticCardBasicInfo[] info = client.GetAllTempMagneticCardBasicInfo();
            //Console.WriteLine(info.Length);
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
              
           //   new Wrapper();
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
        //   R13EventExchange();
          //  for (int i = 2; i < 39;i++)
            //    R13ddCard(39) ;
            //MCNSService.MCNSServiceSoapClient client = new MCNSServiceSoapClient();
        //    client.NotifyDbChange();
        //    ModbusTCP.RTU rtu = new RTU("1", 1, "10.2.84.220", 502, 2001, 32,1);
            
            Console.ReadKey();
        }

        static void client_OnException(ushort id, byte unit, byte function, byte exception)
        {
           // throw new NotImplementedException();
        }

        static void R13ddCard(int erid)
        {

            MCNSService.MCNSServiceSoapClient client = new MCNSServiceSoapClient();

         //RoomInfo[] rinfos=   client.GetAllRoom();
         //foreach (RoomInfo info in rinfos)
         //    Console.WriteLine(info.ERID + " " + info.ERName);
     
          System.IO.StreamReader rd=new StreamReader(  System.IO.File.OpenRead("card.csv"));
          string s = rd.ReadToEnd();
          //int[] erids=(from n in rinfos select n.ERID).ToArray();
          string[] cards = s.Split(new char[]{'\r','\n'}, StringSplitOptions.RemoveEmptyEntries);
          test.MCNSService.ArrayOfInt ait = new ArrayOfInt();
          ait.Add(erid);
          System.Collections.Generic.List<AddCardInfo> list = new List<AddCardInfo>();
          foreach (string ss in cards)
          {
              Console.WriteLine(ss);


              AddCardInfo addcard = new AddCardInfo()
              {
                   CardNo=ss, ERIDs=ait,
                    StartDate=DateTime.Now,EndDate=DateTime.Now.AddMonths(24), MCNSID="test", Name="david"
              };
              list.Add(addcard);
           Console.WriteLine(   client.AddCardWithoutNotify(addcard));
            
              
             
          }
       //   client.NotifyDbChange();
       //   client.AddCard(list.ToArray());

        }

        static void R13EventExchange()
        {
            EventExchangeData data=new EventExchangeData(){ event_id="100", room_id="1", event_status=1, contents="test", start_time=DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")};
             DataContractJsonSerializer sr=new DataContractJsonSerializer(typeof(EventExchangeData));
            MemoryStream ms=new MemoryStream();
            sr.WriteObject(ms, data);
            string body = "params=" + System.Text.ASCIIEncoding.ASCII.GetString(ms.ToArray());
            PushData("http://lab1.sochamp.com/exchange/door-card/trigger", body);
        }

        static bool PushData(string url, string requestBody )
        {
            bool result = true;
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/json; charset=UTF-8";
            //request.UserAgent = userAgent
            //request.Timeout = timeout

            //if (useBasicAuth == true)
            //{
            // Create a token for basic authentication and add a header for it
        //    String authorization = System.Convert.ToBase64String(Encoding.UTF8.GetBytes("aa" + ":" + "aa"));
          //  request.Headers.Add("Authorization", "Basic " + authorization);
            //}
           
            if (request.Method == "POST" && requestBody != null)
            {
                // Convert the request contents to a byte array and include it
                try
                {
                    byte[] requestBodyBytes = System.Text.Encoding.UTF8.GetBytes(requestBody);
                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(requestBodyBytes, 0, requestBodyBytes.Length);
                   
                    requestStream.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }

            // Initialize the response
            HttpWebResponse response = null;
            String responseText = null;

            // Now try to send the request
            try
            {
                response = request.GetResponse() as HttpWebResponse;

                // expect the unexpected
                // WebException may be thrown already for some of this already 
                // like timeout or 404
                if (request.HaveResponse == true && response == null)
                {
                    String msg = "response was not returned or is null";

                    throw new WebException(msg);
                }
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    String msg = "response with status: " + response.StatusCode + " " + response.StatusDescription;

                    throw new WebException(msg);
                }

                // check response headers for the content type
                string contentType = response.GetResponseHeader("Content-Type");

                // get the response content
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                responseText = reader.ReadToEnd();
                reader.Close();

                // handle failures
            }
            catch (WebException e)
            {
                result = false;
                if (e.Response != null)
                {
                    response = (HttpWebResponse)e.Response;
                    Console.WriteLine(response.StatusCode + " " + response.StatusDescription);
                    //log.Error(response.StatusCode + " " + response.StatusDescription);
                }
                else
                {
                    Console.WriteLine(e.Message);
                    //log.Error(e.Message);
                }

                // and clean up after ourselves
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }

            // display the response
            if (responseText != null)
            {
                try
                {
                    System.Console.Write(responseText);
                    //DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Result));
                    //using (MemoryStream ms = new MemoryStream())
                    //{
                    //    byte[] data = System.Text.UTF8Encoding.UTF8.GetBytes(responseText);
                    //    ms.Write(data, 0, data.Length);
                    //    ms.Seek(0, SeekOrigin.Begin);
                    //    // ms.Position = 0;
                    //    Result res = ser.ReadObject(ms) as Result;
                    //    Console.WriteLine(res.status);
                    //}
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + "," + ex.StackTrace);
                    result = false;
                }

            }


            return result;
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

    [DataContract]
    public class EventExchangeData
    {
        //  [ room_id ] 機房索引 [ event_id ] 事件索引 [ event_status ] 狀態代號(正常; // 0, 斷線; // 1) [ contents ] 事件說明（事件說明或⼦子項⺫⽬目說明（）） [ start_time ] 發⽣生時間 

        [DataMember]
        public string room_id { get; set; }
        [DataMember]
        public string event_id { get; set; }
        [DataMember]
        public int event_status { get; set; }
        [DataMember]
        public string contents { get; set; }
        [DataMember]
        public string start_time { get; set; }
    }
}

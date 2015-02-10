using ModbusTCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace SecureServer
{
    class Program
    {
        public static SecureServer.SecureService  MyServiceObject;
        static void Main(string[] args)
        {

            //PD.PD pd = new PD.PD("test", "192.192.85.20", 16502);

            //while (true)
            //{
            //    Console.WriteLine(pd.ToString());
            //    System.Threading.Thread.Sleep(1000);
            //}

         //   ModbusTCP.RTU rtu = new ModbusTCP.RTU("rtu-101", 1, "192.168.0.10", 502, 0001, 46);
         //   while (!rtu.IsConnected) ;
         //   //for (int i = 2001; i <= 2046; i++)
         //   //    rtu.WriteRegister((ushort)i, (ushort)(i - 2001 + 1));
         ////   rtu.WriteRegister(11, 1);
         //   ushort output=0;
         //   while (true)
         //   {
         //       output ^= 1;
         //       rtu.WriteRegister(11,output);
         //       int? temp = rtu.GetRegisterReading(045);
         //       Console.WriteLine(temp == null ? "null" : temp.ToString());
         //       Console.WriteLine(rtu.GetRegisterReading(046));
         //       Console.WriteLine(rtu.ToString());
         //       System.Threading.Thread.Sleep(1000);

         //   }

         //   Console.ReadKey();
            //NVR.NVR_Type1 nr = new NVR.NVR_Type1() { ERID = 1, IP = "192.192.85.20", Port = 10000, NVRID = 1, UserName = "admin", Password = "1111", PlaneID = 1 };

            //nr.SaveRecord(1, DateTime.Now.AddSeconds(-10), DateTime.Now.AddSeconds(10), @"d:\test.avi");

            //Console.ReadKey();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
     //      CardReaderTest();
        //   Console.ReadKey();
            //SecureDBEntities1 db = new SecureDBEntities1();
            
            //Random rnd = new Random();
            //DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            //int passwd = rnd.Next(0, 10000);
            //string pwdString = passwd.ToString("{0000}");
            //tblERDoorPassword  tbl = new tblERDoorPassword() { Timestamp = dt, DoorPassword = pwdString };
            //db.tblERDoorPassword.Add(tbl);
            //db.SaveChanges();

            ServiceHost host = new ServiceHost(MyServiceObject = new SecureServer.SecureService());
            host.Open();
            System.Console.WriteLine("Secure Server started!");
          

            Console.ReadLine();
           
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
          Exception ex =e.ExceptionObject as Exception;
            System.IO.StreamWriter sw = new System.IO.StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "unhandleerr.log");
            sw.WriteLine(ex.Message+","+ex.StackTrace);
            sw.Flush();
            sw.Close();
        }

        static void CardReaderTest()
        {

            CardReader.CardReader cr = new CardReader.CardReader("test","192.168.1.168",1,1,1,1,1);

            Console.WriteLine("設定開門延時警報 10  sec");
            cr.SetOpenDoorDetectionAlarmTime(10);
            Console.ReadLine();

            Console.WriteLine("設定開門延時偵測 1 0 sec");
            cr.SetOpenDoorTimeoutDetectionTime(10);
            Console.ReadLine();

            Console.WriteLine("設定開門自動回復時間 5 sec");
            cr.SetOpenDoorAutoCloseTime(5);
            Console.ReadLine();

            //Console.WriteLine("設定開門 偵測 超時 10  sec");
            //cr.SetOpenDoorTimeoutDetectionTime(1);
            //Console.ReadLine();

            Console.WriteLine("強制開門");
            cr.ForceOpenDoor();
            Console.ReadLine();
            Console.WriteLine("對時");
            cr.SetDateTime(DateTime.Now);
            Console.ReadLine();

            Console.WriteLine("刪除全部卡片");
            cr.DeleteAllCard();
            Console.ReadLine();

            Console.WriteLine("加入卡片 1232828117");
            cr.AddCard("1232828117");
            Console.ReadLine();


            Console.WriteLine("加入卡片 16455387");
            cr.AddCard("16455387");
            Console.ReadLine();

            Console.WriteLine("加入虛擬卡片 12345678");
            cr.AddVirturalCard("12345678");
            Console.ReadLine();

          

            Console.WriteLine("刪除卡片 16455387");
            cr.DeleteCard("16455387");
            Console.ReadLine();

            Console.WriteLine("刪除卡片 16455387");
            cr.DeleteCard("16455387");
            Console.ReadLine();

            Console.WriteLine("開門密碼 7777");
            cr.SetSuperOpenDoorPassword(7777);
            Console.ReadLine();

           
        }
    }
}

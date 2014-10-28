using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecureServer
{
    class Program
    {
        static void Main(string[] args)
        {
            CardReaderTest();

            Console.ReadLine();
           
        }

        static void CardReaderTest()
        {

            CardReader cr = new CardReader("192.168.1.168");

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

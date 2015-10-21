using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReaderTest
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Collections.Generic.Dictionary<string, RoomDoorControlServer.CardData> dict = new Dictionary<string, RoomDoorControlServer.CardData>();
            RoomDoorControlServer.CardControl ctl = new RoomDoorControlServer.CardControl("192.168.102.200");
            RoomDoorControlServer.RAC960CardControl rac = new RoomDoorControlServer.RAC960CardControl("192.168.10.202");

            ctl.SetTime();
          ctl.ReadTime();
       //    List<RoomDoorControlServer.CardData> list= ctl.ReadAllUserData();
           //List<string> list1 = rac.ReadAllUserData();
           // foreach (string user in list1)
           //     Console.WriteLine(user );
            //foreach (RoomDoorControlServer.CardData user in list)
            //    Console.WriteLine(user.CarNum+","+user.Message);
          // Console.WriteLine( ctl.re);
         //  Console.WriteLine(ctl.ReadUserData(0));
            
            bool isExit = false;
            do
            {
                Console.WriteLine("=========" + RoomDoorControlServer.ServerData.tcpConnectNum + "==============");
                ShowMenu();
                switch (GetKey())
                {
                    case '0':
                        isExit = true;
                        break;
                    case '1':
                        Console.WriteLine(ctl.ReadTime());
                        break;
                    case '2':

                         ctl.SetTime();
                      //   ctl.ReadTime();
                         Console.WriteLine(ctl.ReadTime());
                        break;
                    case '3':

                        Console.WriteLine(ctl.AddUserData("0009999999", "david", "1111"));
                        Console.WriteLine(  rac.AddUserData("0009999999", "david"));
                        break;
                    case '4':
                        Console.WriteLine(ctl.DelUserData("0009999999"));
                        Console.WriteLine(rac.DelUserData("0009999999"));
                        break;
                    case '5':
                        dict.Clear();
                        list = ctl.ReadAllUserData(); 
                        foreach (RoomDoorControlServer.CardData user in list)
                        {
                            Console.WriteLine(user.CarNum + "," + user.Message);
                            dict.Add(user.CarNum, user);
                        }
                        break;
                    case '6':
                        if (dict.ContainsKey("0009999999"))
                            Console.WriteLine(dict["0009999999"].Message);
                        else
                            Console.WriteLine("Not found!");
                        break;
                }
            }
            while (!isExit);


       //     Console.ReadKey();
        }

        static   void  ShowMenu()
        {
            Console.WriteLine("1.ShowTime");
            Console.WriteLine("2.SetTime");
            Console.WriteLine("3.AddCard david");
            Console.WriteLine("4.delete david");
            Console.WriteLine("5.List");
            Console.WriteLine("6.find david");
            Console.WriteLine("0.Exit");
        }

        static char GetKey()
        {
            char c;
            c = (char)Console.ReadKey().KeyChar;
            return c;
        }

    }
}

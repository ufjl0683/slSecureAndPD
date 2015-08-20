using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace SMRTest
{
    class Program
    {

        public static TcpClient tcp = new TcpClient();
        static void Main(string[] args)
        {
          
            try
            {
           //     tcp.Connect("10.2.144.80", 4000);
                tcp.Connect(args[0], 4000);
                Stream stream = tcp.GetStream();
                new Thread(ReceiveTask).Start();

             
              //stream.Write(new byte[] { 0xaa, 0x02, 100, 2 + 100 }, 0, 4);  // CSU 運作狀態
              //Console.ReadKey();
              //Console.WriteLine();
              //stream.Write(new byte[] { 0xaa, 0x02, 117, 2 + 117 }, 0, 4);  // CSU 電池事件
              //Console.ReadKey();
              //Console.WriteLine();

                //stream.Write(new byte[] { 0xaa, 0x02, 119, 2 + 119 }, 0, 4);  // CSU SMR 運作狀態
                //Console.ReadKey();
                //Console.WriteLine();

                stream.Write(new byte[] { 0xaa, 0x02, 118, 2 + 118 }, 0, 4);  // CSU SMR 告警狀態
                Console.ReadKey();
                Console.WriteLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }

            }

        static void ReceiveTask()
        {
            Stream stream=tcp.GetStream();

            int data;
            while (true)
            {
                data = stream.ReadByte();
               // if (data < 0x80)
                    Console.WriteLine("{0:X2},", data);
              
            }
        }
    }
}

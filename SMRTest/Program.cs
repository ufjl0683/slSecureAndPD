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
          SecureServer.RTU.R13NewSmrRTU rtu = new SecureServer.RTU.R13NewSmrRTU("1", 1, "10.21.50.8",502, 1, 6, 0);
           // SecureServer.RTU.R13NewSmrRTU rtu = new SecureServer.RTU.R13NewSmrRTU("1", 1, "127.0.0.1", 502, 1, 6, 0);

                 while (true)
                {
                    int v = rtu.GetRegisterReading(3) ?? 0;
                    int i = rtu.GetRegisterReading(4) ?? 0;
                    int bits=rtu.GetRegisterReading(5)??0;
                    Console.Write("v:{0} i:{1} \n",v,i);
                    //Console.WriteLine("v:{0} i{1} major:{2} minor:{3} smrwarning:{4} acfail:{5}  isconnected{6}", v, i, bits & 1, (bits >> 1) & 1,
                    //    (bits >> 2) & 1, (bits >> 3) & 1, rtu.IsConnected);

                System.Threading.Thread.Sleep(1000);

            }

            //while (true)
            //{
            //    int v = rtu.GetRegisterReading(2001) ?? 0;
            //    int i = rtu.GetRegisterReading(2002) ?? 0;
            //    int bits=rtu.GetRegisterReading(2003)??0;
            //    Console.WriteLine("v:{0} i{1} major:{2} minor:{3} smrwarning:{4} acfail:{5}  isconnected{6}", v, i, bits & 1, (bits >> 1) & 1,
            //        (bits >> 2) & 1, (bits >> 3) & 1, rtu.IsConnected);
               
            //    System.Threading.Thread.Sleep(1000);

            //}
           // SmrTest(args);

            Console.ReadKey();
        }

        static void SmrTest(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    Console.WriteLine("connect to " + "10.2.144.80:4000");
                    tcp.Connect("10.2.144.80", 4000);
                }
                else
                {
                    Console.WriteLine("connect to " + args[0] + ":4000");
                    tcp.Connect(args[0], 4000);
                }
                //  tcp.Connect(args[0], 4000);
                Stream stream = tcp.GetStream();
                // new Thread(ReceiveTask).Start();

                while (true)
                {
                    stream.Write(new byte[] { 0xaa, 0x02, 100, 2 + 100 }, 0, 4);  // CSU 運作狀態   50d 32h
                    stream.Flush();
                    System.Threading.Thread.Sleep(1000);
                    ReceiveTask();
                }
                
                Console.WriteLine();
                //stream.Write(new byte[] { 0xaa, 0x02, 117, 2 + 117 }, 0, 4);  // CSU 電池事件
                //Console.ReadKey();
                //Console.WriteLine();

                //stream.Write(new byte[] { 0xaa, 0x02, 119, 2 + 119 }, 0, 4);  // CSU SMR 運作狀態
                //Console.ReadKey();
                //Console.WriteLine();

                //stream.Write(new byte[] { 0xaa, 0x02, 118, 2 + 118 }, 0, 4);  // CSU SMR 告警狀態
                //Console.ReadKey();
                //Console.WriteLine();

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
        //    int voltage=0;
          //  int data;
            int voltage = 0, current = 0, mod1, mod2, mod3;
             int   AcFail=0,SmrWarning=0,major=0,minor=0;
            byte[] data = new byte[32];
            //while (true)
            //{
                
                //if (stream.Length == 32  )
                //{

            int cks = 0;
                    
                   stream.Read(data, 0, 32);
                   Console.WriteLine("read");
                    voltage = data[1] + data[2] * 256;
                    current = data[3] + data[4]*256;
                    mod1=data[13];
                    mod2 = data[14];
                    mod3 = data[15];
                    AcFail = ((mod1 >> 6) & 1)  ;
                    SmrWarning = ((mod1 >> 5) & 1) ;
                    major = ((mod1 >> 3) & 1) ;
                    minor = ((mod1 >> 4) & 1)  ;
                    Console.WriteLine("v:{0} i:{1} mod1={2:X2} mod2={3:X2} mod3={4:X2} major:{5}  minor:{6}  Acfail:{7} SmrWarning:{8}", voltage, current, mod1, mod2, mod3,major,minor,AcFail,SmrWarning);
                    for (int i = 0; i < 32; i++)
                    {
                        cks+=data[i];
                        Console.Write("{0:X2} ",data[i]);
                    }
                    cks -= data[31];
                    Console.WriteLine();
                    
            if( (cks&255)!=data[31])
                         Console.WriteLine("cks error {0:X2}!",cks&255);

            byte[] retData = new byte[6];
            retData[0] = (byte)(voltage / 256);
            retData[1] = (byte)(voltage % 256);
            retData[2] = (byte)(current / 256);
            retData[3] = (byte)(current % 256);
            retData[4] = 0;
            System.Collections.BitArray ba = new System.Collections.BitArray(new byte[] { 0 });
            // bit   0       1      2          3
            //      major   minor  SmrWarning AcFail
            ba.Set(0, major == 0);
            ba.Set(1, minor == 0);
            ba.Set(2, SmrWarning == 0);
            ba.Set(3, AcFail == 0);
            ba.CopyTo(retData, 5);


            
            //for (int i = 0; i < 8; i++)
                    //    stream.ReadByte();
                    //mod1 = stream.ReadByte();
                    //mod2 = stream.ReadByte();
                    //mod3 = stream.ReadByte();

                }
                
//data = stream.ReadByte();
                
               // if (data < 0x80)


                
                   // Console.WriteLine("{0:X2},", data);
              
            //}
        //}
    }


}

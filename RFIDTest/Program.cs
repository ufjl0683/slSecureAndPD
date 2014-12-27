using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RFIDTest
{
    class Program
    {

       static System.IO.Ports.SerialPort port;

       static System.Threading.Thread reeiveThread;
        static void Main(string[] args)
        {

            RFIDTest();
            Console.ReadKey();
            //byte[] data = new byte[36];
            //int PackLength = 36;
            //data[1] = (byte)'s';
            //data[6] = (byte)STX;
            //data[PackLength - 2] = (byte)ETX;
            //if (data.Length % 32 == 0)
            //    PackLength = 32;
            //else if (data.Length % 36 == 0)
            //    PackLength = 36;
             
            //for (int i = 0; i < data.Length / PackLength; i++)
            //{
            //    Console.WriteLine("Step3");
            //    if (data[i * PackLength + 1] == 's')
            //    {
            //        Console.WriteLine("Step4");
            //        if (data[6 + i * PackLength] == STX && data[(i+1) * PackLength - 2] == ETX)
            //        {
            //            Console.WriteLine("Step5");
            //            byte[] temp = new byte[17]; // new byte[data.Length - 9];
            //            System.Array.Copy(data, (i + 1) * PackLength - 2 - 17, temp, 0, temp.Length);

            //            string CardID = System.Text.ASCIIEncoding.ASCII.GetString(temp);

            //            // Do something width Cardid
            //            Console.WriteLine(CardID);

            //        }
            //    }
            //}
           
        }

       static    byte SOH = 1, PT = (byte)'S', STX = 2, ID1 = (byte)'0', ID2 = (byte)'1',   ETX = 3;

        static void ReceiveTask()
        {
            int d=0, cnt;
            Console.WriteLine("Start Receive Thread!");
            while(true)
            {
                 MemoryStream  ms=new MemoryStream();
                // Console.WriteLine("Step 0");
               do{ 
                   d=  port.BaseStream.ReadByte();
               }
               while(d!=SOH);
             //  Console.WriteLine("Step 1");
               if (port.BaseStream.ReadByte() != (byte)'s')
                   continue;

                    do{
                        d=  port.BaseStream.ReadByte();
                    }while(d!=STX);
                cnt=0;
             //   Console.WriteLine("Step 2");
                do{
                    d=  port.BaseStream.ReadByte();
                    if(d!=ETX)
                    {
                        cnt++;
                        ms.WriteByte((byte)d);
                      //  Console.WriteLine((char)d );
                    }
                }while(d!=ETX);
             //   Console.WriteLine("Step 3");

                lock(lockobj)
                System.Threading.Monitor.PulseAll(lockobj);
                if(cnt!=0)
                {
                //    Console.WriteLine("Step 4");
                    byte[] temp= ms.ToArray();

                    if ( temp.Length < 17)
                    {
                        if(temp.Length!=1)
                             Console.WriteLine("length {0} error!",temp.Length);
                        continue;
                    }
                 //   Console.WriteLine("Step 5");
                    byte[]data=new byte[17];
                    System.Array.Copy(temp,temp.Length-17,data,0,17);
                    string CardNo=System.Text.ASCIIEncoding.ASCII.GetString(data);


                    Console.WriteLine(CardNo);

                    // do something here

                }
                else
                {
                 //   Console.WriteLine("none");
                }
            }

        }

      static   void RFIDTest()
        {

            string PortName = "";
            PortName = GetComPort();
            if (PortName == "")
                Console.WriteLine("找不到設備");
            else
            {
                Console.WriteLine("找到" + PortName);
                reeiveThread = new System.Threading.Thread(ReceiveTask);
                  reeiveThread.Start();
                GetRFIDDataTask( );
               
            }

        }

        public static byte[] GetRFIDCommand()
        {
            byte[] data=new byte[9];

            data[0]=SOH; //stx
            data[1] = PT;
            data[2] = ID1;
            data[3] = ID2;
            data[4]=(byte)'A';
            data[5]=(byte)'0';
            data[6] = STX;
            data[7] = ETX;
            data[8]=0;
            for (int i = 0; i <=7; i++)
            {
                data[8] ^= data[i];
            }

            data[8] |= 0x20;

            return data;


        }

        static object lockobj=new object();
        static void GetRFIDDataTask()
        {
            byte[] cmd = GetRFIDCommand();
            byte[] data;
            System.Threading.Thread.Sleep(1000);

            data = new byte[port.BytesToRead];
            port.BaseStream.Read(data, 0, data.Length);  // clear buffer
            while (true)
            {


                //port = new System.IO.Ports.SerialPort(Comport, 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);
                //port.Open();

                //data=new byte[ port.BaseStream.Length];
                //port.BaseStream.Read(data,0,data.Length);
                try
                {
                  //  Console.WriteLine("query!");
                    port.BaseStream.Write(cmd, 0, cmd.Length);
                    port.BaseStream.Flush();


                    lock (lockobj)
                    {
                        System.Threading.Monitor.Wait(lockobj, 2000);
                    }


                    //data = new byte[port.BytesToRead];
                    //Console.WriteLine("Step1");
                    //port.BaseStream.Read(data, 0, data.Length);
                    //Console.WriteLine(data.Length);
                    //int PackLength = 0;
                    //Console.WriteLine(System.Text.ASCIIEncoding.ASCII.GetString(data));
                    //if (data.Length % 32 == 0)
                    //    PackLength = 32;
                    //else if (data.Length % 36 == 0)
                    //    PackLength = 36;
                    //else

                    //    continue;


                    //Console.WriteLine("Step2");

                    //for (int i = 0; i < data.Length / PackLength; i++)
                    //{
                    //    Console.WriteLine("Step3");
                    //    if (data[i * PackLength + 1] == 's')
                    //    {
                    //        Console.WriteLine("Step4");
                    //        if (data[6 + i * PackLength] == STX && data[(i + 1) * PackLength - 2] == ETX)
                    //        {
                    //            Console.WriteLine("Step5");
                    //            byte[] temp = new byte[17]; // new byte[data.Length - 9];
                    //            System.Array.Copy(data, (i + 1) * PackLength - 2 - 17, temp, 0, temp.Length);

                    //            string CardID = System.Text.ASCIIEncoding.ASCII.GetString(temp);

                    //            // Do something width Cardid
                    //            Console.WriteLine(CardID);

                    //        }
                    //    }
                    //}

                }
                catch (Exception ex)
                {
                    Console.WriteLine("In task");
                    Console.WriteLine(ex.Message + "," + ex.StackTrace);
                }


                }

            }
        




        static string GetComPort()
        {
            for (int i = 1; i <= 50; i++)
            {
                try
                {
                    byte[] data;
                    port = new System.IO.Ports.SerialPort("COM" + i, 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);
                    port.Open();
                     
                    //data=new byte[ port.BaseStream.Length];
                    //port.BaseStream.Read(data,0,data.Length);
                    port.BaseStream.Write(GetRFIDCommand(), 0, 9);
                    port.BaseStream.Flush();
                    System.Threading.Thread.Sleep(300);
                    // Console.WriteLine(port.BaseStream.Length.ToString());
                      data = new byte[port.BytesToRead];

                    port.BaseStream.Read(data, 0, data.Length);
                    if (data[1] == 's')
                        return "COM" + i;
                }
                catch (Exception ex)
                {

                    Console.WriteLine("Com+" + i + ex.Message);
                    if (port != null && port.IsOpen)
                    {
                        port.Close();
                        port.Dispose();
                        port = null;
                    }
                }
                

            }

            return "";
        }


    }
}

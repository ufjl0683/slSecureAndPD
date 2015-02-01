using ModbusTCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SecureServer.PD
{
   public  class PD
    {


        string IP;
       int Port;

       Master RTUDevice;
       System.Threading.Timer tmr;
       byte[] data=new byte[2];
       string PDName;
       object lockobj = new object();
       System.Collections.BitArray bits;
       public PD(string  PDName,string IP, int Port)
       {
           this.IP = IP;
           this.Port = Port;
           //bits = new System.Collections.BitArray(data);
           RTUDevice = new ModbusTCP.Master(IP, (ushort)Port);
           new Thread(ConnectTask).Start();
           tmr = new System.Threading.Timer(new System.Threading.TimerCallback(timerBack));
           tmr.Change(0, 500);
           new Thread(ReadingTask).Start();
           this.PDName = PDName;


       }


       public bool IsConnected
       {
           get
           {
               if (RTUDevice == null || !RTUDevice.connected)
                   return false;
               else
                   return true;
           }
       }

       public int R0
       {
           get
           {
               return (System.BitConverter.ToUInt16(data, 0)) & 0x01;
           }
       }

       public int S0
       {
           get
           {
               return (System.BitConverter.ToUInt16(data, 0) >> 1) & 0x01;
           }
       }
       public int T0
       {
           get
           {
               return (System.BitConverter.ToUInt16(data, 0) >> 2) & 0x01;
           }
       }
       public int R1
       {
           get
           {
               return (System.BitConverter.ToUInt16(data, 0) >> 3) & 0x01;
           }
       }

       public int S1
       {
           get
           {
               return (System.BitConverter.ToUInt16(data, 0) >> 4) & 0x01;
           }
       }
       public int T1
       {
           get
           {
               return (System.BitConverter.ToUInt16(data, 0) >> 5) & 0x01;
           }
       }

       public int L0
       {
           get
           {
               return (System.BitConverter.ToUInt16(data, 0) >>  6) & 0x01;
           }
       }
       public int L1
       {
           get
           {
               return (System.BitConverter.ToUInt16(data, 0) >> 7) & 0x01;
           }
       }
       
       public int L2
       {
           get
           {
               return (System.BitConverter.ToUInt16(data, 0) >> 8) & 0x01;
           }
       }
       public int L3
       {
           get
           {
               return (System.BitConverter.ToUInt16(data, 0) >> 9) & 0x01;
           }
       }
       public int L4
       {
           get
           {
               return (System.BitConverter.ToUInt16(data, 0) >> 10) & 0x01;
           }
       }
       public int Cabinet
       {
           get
           {
               return (System.BitConverter.ToUInt16(data, 0) >> 11) & 0x01;
           }
       }

       void ReadingTask()
       {
           byte[] tempdata = new byte[data.Length];
           while (true)
           {

               try
               {

                   if (RTUDevice != null && RTUDevice.connected)
                   {
                       lock (lockobj)
                       {
                           RTUDevice.ReadDiscreteInputs(1, 0, 0, 12,ref tempdata);
                         //  RTUDevice.ReadHoldingRegister((ushort)this.DevID, (byte)255, (ushort)(StartAddress - 1), this.RegisterLength, ref tempdata);
                           if (tempdata != null && tempdata.Length != 0)
                           {
                               for (int i = 0; i < tempdata.Length; i++)
                               {
                                   data[i] = tempdata[i];
                               }
                           }
                       }
                   }
                   System.Threading.Thread.Sleep(1000);

               }
               catch (Exception ex)
               {
                   Console.WriteLine(ex.Message + "," + ex.StackTrace);
               }

           }
       }


       void timerBack(object stat)
       {
           //if (RTUDevice==null||!RTUDevice.connected)
           //{
           //    new System.Threading.Thread(ConnectTask).Start();
           //    return;
           //}



       }

       bool IsInConnected = false;
       void ConnectTask()
       {

           if (IsInConnected)
               return;
           IsInConnected = true;
           while (true)
           {
               while (RTUDevice == null || !RTUDevice.connected)
               {
                   try
                   {
                       Console.WriteLine(this.PDName + "  Connecting!");
                       RTUDevice = new Master();
                       RTUDevice.connect(IP, (ushort)Port);
                       RTUDevice.OnResponseData += RTUDevice_OnResponseData;
                       RTUDevice.OnException += RTUDevice_OnException;

                   }
                   catch (Exception ex)
                   {
                       Console.WriteLine(ex.Message + "," + ex.StackTrace);
                       continue;
                       ;
                   }
                   finally
                   {
                       System.Threading.Thread.Sleep(1000);
                   }
                   Console.WriteLine("connected!");
               }

               System.Threading.Thread.Sleep(1000);
           }



           IsInConnected = false;
       }

       void RTUDevice_OnException(ushort id, byte unit, byte function, byte exception)
       {
           if (exception == 254)
           {
               RTUDevice.disconnect();
               RTUDevice.Dispose();

           }
           //  throw new NotImplementedException();
       }

       void RTUDevice_OnResponseData(ushort id, byte unit, byte function, byte[] data)
       {
           //  throw new NotImplementedException();
       }
  
       public override string ToString()
       {
           //return base.ToString();
           StringBuilder sb = new StringBuilder();
          System.Collections.BitArray bits = new System.Collections.BitArray(data);
           //string res = string.Format("R0:S0:T0 {0}:{1}:{2}, R1:S1:T1 {3}:{4}:{5} L1-L5:{6}{7}{8}{9}{10} C:{11}\n", Cvt01(bits.Get(0)), Cvt01(bits.Get(1)), Cvt01(bits.Get(2)), Cvt01(bits.Get(3)), Cvt01(bits.Get(4)), Cvt01(bits.Get(5))
           //   , Cvt01(bits.Get(6)), Cvt01(bits.Get(7)), Cvt01(bits.Get(8)), Cvt01(bits.Get(9)), Cvt01(bits.Get(10)), Cvt01(bits.Get(11)));    
           string res = string.Format("R0:S0:T0 {0}:{1}:{2}, R1:S1:T1 {3}:{4}:{5} L1-L5:{6}{7}{8}{9}{10} C:{11}", R0, S0, T0, R1, S1,T1
               , L0, L1, L2, L3, L4,Cabinet);
           //for (int i = 0; i < data.Length; i++)
           //{
           //    sb.Append(string.Format("{0:X2}", data[i]) + " ");

           //}

           return res;// sb.ToString().Trim();
       }
       
       int Cvt01(bool val)
       {
           return val ? 1 : 0;
       }



    }
}

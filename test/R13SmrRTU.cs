using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModbusTCP;
using System.Threading;
using System.Net.Sockets;
using System.IO;

namespace SecureServer.RTU
{
   public  class R13SmrRTU:IRTU
    {

         public event OnCommStateChangedHandler OnCommStateChanged;
        public string IP { get; set; }
        public int Port { get; set; }
        public ushort RegisterLength { get; set; }
        public string ControlID { get; set; }
        public int DevID { get; set; }
        public ushort StartAddress;
        object lockobj = new object();
       // int _Comm_state;
       // Master RTUDevice;
        System.Threading.Timer tmr;
        byte?[] data;
        public string Adam;
        public TcpClient tcp ;
        public R13SmrRTU(string ControlID, int DevID, string IP, int Port, int StartAddress, int RegisterLength, int comm_state)
        {
          //  this.Adam = Adam;
            this.StartAddress = (ushort)StartAddress;
            data = new byte?[6];//new byte?[RegisterLength * 2];
            Console.WriteLine(ControlID + ",DataLength:"+data.Length);
            this.ControlID = ControlID;
            this.IP = IP;
            this.Port = Port;
            this.RegisterLength = (ushort)RegisterLength;
            this.DevID = DevID;
            _Comm_state = comm_state;
            new Thread(ConnectTask).Start();
           //   tmr = new System.Threading.Timer(new System.Threading.TimerCallback(timerBack));
         //   tmr.Change(0, 500);
            new Thread(ReadingTask).Start();
           
        }

        void ConnectTask()
        {

           
        }

        void ReadingTask()
        {
            Stream stream=null;
          //  byte[] tempdata;// = new byte[34];
            while (true)
            {

                try
                {
                    if (tcp == null || !tcp.Connected)
                    {
                      //  Comm_state = 0;
                        tcp = new TcpClient();
                        try
                        {
                            Console.WriteLine("Connect to Smr" + IP + ":" + Port);
                            tcp.Connect(IP, Port);
                        }
                        catch (Exception exx)
                        {
                            Console.WriteLine(exx.Message);
                        }
                    }
                    // 偵測 RTU開 斷線並產生事件
                    if (tcp != null && tcp.Connected)
                    {
                        Comm_state = 1;

                          stream = tcp.GetStream();
                        stream.ReadTimeout = 3000;
                        //    int voltage=0;
                        //  int data;
                        int voltage = 0, current = 0, mod1, mod2, mod3;
                        int AcFail = 0, SmrWarning = 0, major = 0, minor = 0;
                        byte[] rdata = new byte[32];
                        //while (true)
                        //{

                        //if (stream.Length == 32  )
                        //{

                        int cks = 0;
                        stream.Write(new byte[] { 0xaa, 0x02, 100, 2 + 100 }, 0, 4);  // CSU 運作狀態   50d 32h
                        stream.Flush();
                        try
                        {
                            stream.Read(rdata, 0, 32);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("SMR_RTU"+ex.Message);
                            tcp.Close();
                            System.Threading.Thread.Sleep(1000);
                            continue;
                        }
                        Console.WriteLine("read");
                        voltage = rdata[1] + rdata[2] * 256;
                        current = rdata[3] + rdata[4] * 256;
                        mod1 = rdata[13];
                        mod2 = rdata[14];
                        mod3 = rdata[15];
                        AcFail = ((mod1 >> 6) & 1);
                        SmrWarning = ((mod1 >> 5) & 1);
                        major = ((mod1 >> 3) & 1);
                        minor = ((mod1 >> 4) & 1);
                        Console.WriteLine("v:{0} i:{1} mod1={2:X2} mod2={3:X2} mod3={4:X2} major:{5}  minor:{6}  Acfail:{7} SmrWarning:{8}", voltage, current, mod1, mod2, mod3, major, minor, AcFail, SmrWarning);
                        for (int i = 0; i < 32; i++)
                        {
                            cks += rdata[i];
                            Console.Write("{0:X2} ", rdata[i]);
                        }
                        cks -= rdata[31];
                        Console.WriteLine();

                        if ((cks & 255) != rdata[31])
                            Console.WriteLine("cks error {0:X2}!", cks & 255);

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

                        for (int i = 0; i < data.Length; i++)
                            data[i] = retData[i];

                    }
                    else
                    {
                        Comm_state = 0;
                    }
                   

                }
                catch (Exception ex)
                {
                    Console.WriteLine("RTU:" + this.ControlID + "," + ex.Message + "," + ex.StackTrace);
                  
                }

                System.Threading.Thread.Sleep(10000);

            }


        }

      //  bool IsInConnected = false;
        public bool IsConnected
        {
            get
            {
                if( this.Comm_state==1)
                    return true;
                else
                    return false;
            }
        }

        int _Comm_state;

        int Comm_state
        {
            get
            {
                return _Comm_state;
            }
            set
            {
                if (value != _Comm_state)
                {
                    _Comm_state = value;
                    if (OnCommStateChanged != null)
                        OnCommStateChanged(this, value);
                }

            }
        }
     

       

        public void WriteRegister(ushort address, ushort data)
        {
            throw new NotImplementedException();
        }




        public int? GetRegisterReading(ushort RTUAddress)
        {
            //throw new NotImplementedException();
            int address = RTUAddress;
            return  data[(address - StartAddress) * 2] * 256 + data[(address - StartAddress) * 2 + 1];
        }
    }
}

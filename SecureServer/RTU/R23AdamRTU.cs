using ModbusTCP;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SecureServer.RTU
{
  public   class R23AdamRTU:IRTU
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
        public R23AdamRTU(string ControlID, int DevID, string IP, int Port, int StartAddress, int RegisterLength, int comm_state,string Adam)
        {
            this.Adam = Adam;
            this.StartAddress = (ushort)StartAddress;
            data = new byte?[34];//new byte?[RegisterLength * 2];
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

            byte[] tempdata;// = new byte[34];
            while (true)
            {

                try
                {

                    // 偵測 RTU開 斷線並產生事件
                    if (this.Adam != null)
                        Comm_state = RoomClient.RoomClient.GetControlConnectionStatus(Adam) ? 1 : 0;   // RTUDevice.connected ? 1 : 0;

                    if (Comm_state==1)
                    {
                        lock (lockobj)
                        {
                           // RTUDevice.ReadHoldingRegister((ushort)this.DevID, (byte)255, (ushort)(StartAddress - 1), this.RegisterLength, ref tempdata);
                            tempdata = RoomClient.RoomClient.GetStatus(Adam);
                            if (tempdata != null && tempdata.Length != 0)
                            {
                                for (int i = 0; i < tempdata.Length; i++)
                                    data[i] = tempdata[i];
                            }
                        }
                    }
                    System.Threading.Thread.Sleep(1000);

                }
                catch (Exception ex)
                {
                    Console.WriteLine("RTU:" + this.ControlID + "," + ex.Message + "," + ex.StackTrace);
                }

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
            return data[address - StartAddress];   //data[(address - StartAddress) * 2] * 256 + data[(address - StartAddress) * 2 + 1];
        }
    }
}

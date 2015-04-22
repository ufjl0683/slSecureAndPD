using ModbusTCP;
using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Threading;
//using System.Threading.Tasks;

namespace ModbusTCP 
{

//public delegate  void RegisterValueChangedHandler(int address, int newValue);
    public delegate void OnCommStateChangedHandler(RTU sender,int comm_state);
    public  class RTU : ModbusTCP.IRTU 
    {
      //  public event RegisterValueChangedHandler OnRegisterValueChangeEvent;
        public event OnCommStateChangedHandler OnCommStateChanged;
        public string IP { get; set; }
        public int  Port {get;set;}
        public ushort RegisterLength { get; set; }
        public string  ControlID { get; set; }
        public int DevID { get; set; }
         public ushort StartAddress;
        Master RTUDevice;
        System.Threading.Timer tmr;
        byte?[] data;

        object lockobj = new object();
        public RTU(string ControlID,int DevID,string IP, int Port,int StartAddress, int RegisterLength,int comm_state)
        {
            this.StartAddress = (ushort)StartAddress;
            data = new byte?[RegisterLength * 2];
            Console.WriteLine(ControlID + ",DataLength:"+data.Length);
            this.ControlID = ControlID;
            this.IP = IP;
            this.Port = Port;
            this.RegisterLength = (ushort)RegisterLength;
            this.DevID = DevID;
            _Comm_state = comm_state;
            new Thread(ConnectTask).Start();
              tmr = new System.Threading.Timer(new System.Threading.TimerCallback(timerBack));
            tmr.Change(0, 500);
            new Thread(ReadingTask).Start();
           
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

        int _Comm_state;

        int Comm_state
        {
            get{
                return _Comm_state;
            }
            set
            {
                if(value!=_Comm_state)
                {
                    _Comm_state=value;
                    if(OnCommStateChanged!=null)
                        OnCommStateChanged(this,value);
                }

            }
        }

        void ReadingTask()
        {
            byte[] tempdata = new byte[data.Length];
            while (true)
            {

                try
                {

                    // 偵測 RTU開 斷線並產生事件
                    if (RTUDevice != null)
                        Comm_state = RTUDevice.connected ? 1 : 0;

                    if (RTUDevice != null && RTUDevice.connected)
                    {
                        lock (lockobj)
                        {
                            RTUDevice.ReadHoldingRegister((ushort)this.DevID, (byte)255, (ushort)(StartAddress - 1), this.RegisterLength, ref tempdata);
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
                    Console.WriteLine("RTU:"+this.ControlID+","+ex.Message + "," + ex.StackTrace);
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

        public void WriteRegister(ushort address, ushort data)
        {
            if (!this.IsConnected)
                return;
            lock (lockobj)
            {
                byte[] result=new byte[0];
                if (RTUDevice != null && RTUDevice.connected)
                {
                    RTUDevice.WriteSingleRegister((ushort)1, (byte)0, (ushort)(address - 1), new byte[] { (byte)(data / 256), (byte)(data % 256) },ref result);
                }
            }
        }
        public int? GetRegisterReading(ushort RTUAddress)
        {
            int address = RTUAddress ;
            return data[(address - StartAddress) * 2] * 256 + data[(address - StartAddress) * 2 + 1];
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
                            Console.WriteLine(this.ControlID+"  Connecting!");
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
            for (int i = 0; i < data.Length; i++)
            {
               sb.Append(string.Format("{0:X2}",data[i])+" ");
               
            }

            return sb.ToString().Trim();
        }

    }
}

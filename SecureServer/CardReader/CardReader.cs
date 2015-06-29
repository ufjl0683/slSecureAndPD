using ClassSockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SecureServer.CardReader
{
    public enum DoorEventType{
        DoorOpen,
        DoorClose,
        Connected,
        DisConnected

    }

    public delegate void DoorEventHandler(ICardReader reader,DoorEventType enumEventType);
    public delegate void AlarmEventHandler(ICardReader reader,AlarmData alarmdata);

    public delegate void  StatusChangeHandler(CardReader reader,CardReaderEventReport report);
   
   public  class CardReader : SecureServer.CardReader.ICardReader
    {
       public event DoorEventHandler OnDoorEvent;
       public event AlarmEventHandler OnAlarmEvent;
       public event StatusChangeHandler OnStatusChanged;
       public string IP { get; set; }
       
         bool _IsConnected = false;
       public bool IsConnected
       {
           get
           {
               return _IsConnected;
           }
           set
           {
               if (value != _IsConnected)
               {
                   _IsConnected = value;
                   if (OnDoorEvent != null)
                   {
                       if (value)
                           OnDoorEvent(this, DoorEventType.Connected);
                       else
                           OnDoorEvent(this, DoorEventType.DisConnected);
                   }

               }
           }
       }
       public string ControllerID { get; set; }
       public int PlaneID { get; set; }
       public int ERID { get; set; }
       public int TriggerCCTVID { get; set; }
       public int NVRID { get; set; }
       public int NVRChNo { get; set; }
         bool _IsDoorOpen;
       public bool IsDoorOpen
       {
           get
           {
               return _IsDoorOpen;
           }
           set
           {
               if (value != _IsDoorOpen)
               {
                   _IsDoorOpen = value;
                   if (this.OnDoorEvent != null)
                   {
                       if (value)
                           this.OnDoorEvent(this, DoorEventType.DoorOpen);
                       else
                           this.OnDoorEvent(this, DoorEventType.DoorClose);
                   }
               }
           }
       }
           ClassSockets.ClientSocket ClientSocket;//=new ClientSocket();
       //ClassSockets.ServerSocket ServerScoket;
       System.Threading.Timer tmr;
       public CardReader(string controllerid, string ip, int ERID, int PlaneID, int TriggerCCTVID, int NVRID, int NVRChNo)
       {
           this.TriggerCCTVID = TriggerCCTVID;
           this.IP = ip;
           this.ControllerID = controllerid;
           this.ERID = ERID;
           this.PlaneID = PlaneID;
          this.NVRID = NVRID;
          this.NVRChNo = NVRChNo;

           ClientSocket = new ClassSockets.ClientSocket();

           //ServerScoket = new ClassSockets.ServerSocket();
           //ServerScoket.OnRead += new ServerSocket.ConnectionDelegate(Server_OnRead);

           //if (ServerScoket.Active())
           //         Console.WriteLine("Card Reader Server Socket is Listening!");

           tmr= new System.Threading.Timer(OneSecTask);
           tmr.Change(0, 1000*60);
       }

       public BindingData.DoorBindingData ToBindingData()
       {
           BindingData.DoorBindingData data = new BindingData.DoorBindingData()
           {
                ControlID=this.ControllerID,
                 IsConnected=this.IsConnected,
                 IsDoorOpen=this.IsDoorOpen
                  
           };
           if (IsConnected)
           {
               if (IsDoorOpen)
                   data.DoorColorString = "Red";
               else
                   data.DoorColorString = "Green";

           }
           else
               data.DoorColorString = "Gray";
           return data;
       }
       

       void OneSecTask(object a)
       {
           try
           {
#if DEBUG
                        return;
#endif           
               int status=   ClientSocket.ReadAllState(1,IP);
            if (status == -2)
            {

                this.IsConnected = false;
                Console.WriteLine(IP + "測試斷線!");
            }
            else
            {
                this.IsConnected = true;
              //  Console.WriteLine(IP + "測試連線!");
            }

            
               
              // System.Collections.BitArray ba=new System.Collections.BitArray(new int[]{status});
              // if (ba.Get(0))
              //     Console.WriteLine("開門");
              // else
              //     Console.WriteLine("關門");

              // if (ba.Get(1))
              //     Console.WriteLine("外部按鍵按下");
              // else
              //     Console.WriteLine("外部按鍵放開");

              //if (ba.Get(2))
              //     Console.WriteLine("開門超時");
              // else
              //     Console.WriteLine("無開門超時");

              //if (ba.Get(3))
              //    Console.WriteLine("外力破壞");
              //else
              //    Console.WriteLine("無外力破壞");

           }
           catch(Exception ex)
           {
               Console.WriteLine(ex.Message + "," + ex.StackTrace);
           }

       }

      

       

       //private void Server_OnRead(Socket soc)
       //{
       //    byte[] SRbuf = ServerScoket.ReceivedBytes;

       //    if (SRbuf != null)
       //    {
       //        if (SRbuf.Length != 25)
       //            return;
       //        string SRhexstr = BitConverter.ToString(SRbuf);
       //        CardReaderEventReport rpt = new CardReaderEventReport(SRbuf);
       //        Console.WriteLine(rpt);
       //        if (rpt.Status == (int)CardReaderStatusEnum.門開啟)
       //            this.IsDoorOpen = true;

       //        if (rpt.Status == (int)CardReaderStatusEnum.門關閉)
       //            this.IsDoorOpen = false;

       //        if (this.OnStatusChanged != null)
       //            this.OnStatusChanged(this, rpt);
                
       //    }

       //}
       public void ForceOpenDoor()
       {
          int res= ClientSocket.WriteOpenDoorCommand(1, IP, 2);
         
         
          if (res == -2)
          {
              IsConnected = false;
              throw new Exception("Connection error!");
          }

          if (res >= 1)
              throw new Exception("cmd error!");
          if (res == -1)
              throw new Exception("ID error!");
          IsConnected = true;
       }

       public void SetDateTime(DateTime dt)
       {
           int res = ClientSocket.TimeCorrection(1, IP, (byte)(dt.Year % 100), (byte)dt.Month,(byte) dt.Day,(byte) dt.DayOfWeek,(byte) dt.Hour,(byte) dt.Minute,(byte) dt.Second);
           if (res == -2)
           {
               IsConnected = false;
               throw new Exception("Connection error!");
           }

           if (res >= 1)
               throw new Exception("cmd error!");
           if (res == -1)
               throw new Exception("ID error!");
           IsConnected = true;
       }


       public void WriteCardReaderID(int id)
       {
           int res = ClientSocket.WriteID(1, IP, (byte)id);
           if (res == -2)
           {
               IsConnected = false;
               throw new Exception("Connection error!");
           }

           if (res >= 1)
               throw new Exception("cmd error!");
           if (res == -1)
               throw new Exception("ID error!");
           IsConnected = true;
       }

       public void DeleteAllCard()
       {

           int res = ClientSocket.WriteCardAllDelete(1, IP);

           if (res == -2)
           {
               IsConnected = false;
               throw new Exception("Connection error!");
           }

           if (res >= 1)
               throw new Exception("cmd error!");
           if (res == -1)
               throw new Exception("ID error!");

           IsConnected = true;
       }

       public void AddVirturalCard(string cardno)
       {
           int cardid = int.Parse(cardno);

           int res = ClientSocket.WriteAddCard(1, IP, cardid / 65536, cardid % 65536,
               2, 0, 1, 1);
           if (res == -2)
           {
               IsConnected = false;
               throw new Exception("Connection error!");
           }

           if (res >= 1)
               throw new Exception("cmd error!");
           if (res == -1)
               throw new Exception("ID error!");

           IsConnected = true;
       }
       public void AddCard(string cardno)
       {
           long cardid = long.Parse(cardno);
               
             int res=  ClientSocket.WriteAddCard(1,  IP, (int)(cardid / 65536), (int)(cardid % 65536),
                 0, 0, 1,1); 
           if (res == -2)
           {
               IsConnected = false;
               throw new Exception("Connection error!");
           }

           if (res >= 1)
               throw new Exception("cmd error!");
           if (res == -1)
               throw new Exception("ID error!");

           IsConnected = true;
       }
       public void DeleteCard(string cardno)
       {
           long cardid = long.Parse(cardno);

           int res = ClientSocket.WriteAddCard(1, IP, (int)(cardid / 65536), (int)(cardid % 65536),
               0, 0, 1, 2);
           if (res == -2)
           {
               IsConnected = false;
               throw new Exception("Connection error!");
           }

           if (res >= 1)
               throw new Exception("cmd error!");
           if (res == -1)
               throw new Exception("ID error!");

           IsConnected = true;
       }

       public void SetSuperOpenDoorPassword(int password)
       {

           int res = ClientSocket.WriteOpenDoorPassword(1, IP, password);
           if (res == -2)
           {
               IsConnected = false;
               throw new Exception("Connection error!,"+this.ControllerID);
           }

           if (res >= 1)
               throw new Exception("cmd error!,");
           if (res == -1)
               throw new Exception("ID error!,"+this.ControllerID);

           IsConnected = true;

       }


       public void SetOpenDoorDetectionAlarmTime(int sec)
       {


           int res = ClientSocket.WriteOpenDoorDetectionAlarmTime(1,IP,(byte)sec);
           if (res == -2)
           {
               IsConnected = false;
               throw new Exception("Connection error!");
           }

           if (res >= 1)
               throw new Exception("cmd error!");
           if (res == -1)
               throw new Exception("ID error!");

           IsConnected = true;
       }

       public void SetOpenDoorTimeoutDetectionTime(int sec)
       {

                   
           int res = ClientSocket.WriteOpenDoorTimeoutDetectionTime(1, IP, (byte)sec);
           if (res == -2)
           {
               IsConnected = false;
               throw new Exception("Connection error!");
           }

           if (res >= 1)
               throw new Exception("cmd error!");
           if (res == -1)
               throw new Exception("ID error!");

           IsConnected = true;
       }


       public void SetOpenDoorAutoCloseTime(int sec)
       {


           int res = ClientSocket.WriteOpenDoorACKTime(1, IP, (byte)sec);
           if (res == -2)
           {
               IsConnected = false;
               throw new Exception("Connection error!");
           }

           if (res >= 1)
               throw new Exception("cmd error!");
           if (res == -1)
               throw new Exception("ID error!");

           IsConnected = true;
       }



       public void InvokeStatusChange(CardReaderEventReport rpt)
       {
           if (this.OnStatusChanged != null)
               this.OnStatusChanged(this, rpt);
       }


       public void SetR23Parameter(int RemoOpenTimeR23, int DelayTimeR23, int LoopErrorAlarmTimeR23, int AlarmTimeR23)
       {
           throw new NotImplementedException();
       }
    }
}

using RoomInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureServer.CardReader
{

    public   class CardReader23:ICardReader
    {
        tblControllerConfig config;

        public event AlarmEventHandler OnAlarmEvent;

        public event DoorEventHandler OnDoorEvent;

        public event StatusChangeHandler OnStatusChanged;

        System.Threading.Timer tmr;

        public void AddCard(string cardno)
        {
            throw new NotImplementedException();
        }

        public void AddVirturalCard(string cardno)
        {
            throw new NotImplementedException();
        }

        public string ControllerID
        {
            get;
            set;
        }

        public void DeleteAllCard()
        {
            throw new NotImplementedException();
        }

        public void DeleteCard(string cardno)
        {
            throw new NotImplementedException();
        }

        public int ERID
        {
            get;
            set;
        }

        public void ForceOpenDoor()
        {
            if (config.R23DoorOpen_DO == null)
            {
                Console.WriteLine("R23DoorOpen_DO is null!");
                throw new Exception("R23DoorOpen_DO is null!");
            }
            RoomClient.RoomClient.OpenDoor(this.config.R23_ADAM/*this.ControllerID*/, (short)config.R23DoorOpen_DO, "opendoor");
            //throw new NotImplementedException();
        }

        public void InvokeStatusChange(CardReaderEventReport rpt)
        {
           if (this.OnStatusChanged != null)
               this.OnStatusChanged(this, rpt);
        }

        public string IP
        {
            get;
            set;
            //get
            //{
            //    throw new NotImplementedException();
            //}
            //set
            //{
            //    throw new NotImplementedException();
            //}
        }

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

        public int NVRChNo
        {
            get;
            set;
        }

        public int NVRID
        {
            get;
            set;
        }



        public int PlaneID
        {
            get;
            set;
        }


         public CardReader23(string controllerid, string ip, int ERID, int PlaneID, int TriggerCCTVID, int NVRID, int NVRChNo, tblControllerConfig config)
       {
           this.TriggerCCTVID = TriggerCCTVID;
           this.IP = ip;
           this.ControllerID = controllerid;
           this.ERID = ERID;
           this.PlaneID = PlaneID;
          this.NVRID = NVRID;
          this.NVRChNo = NVRChNo;
          this.config = config;
          this._IsConnected = config.Comm_state == 1 ? true : false;
             
        //   ClientSocket = new ClassSockets.ClientSocket();

           //ServerScoket = new ClassSockets.ServerSocket();
           //ServerScoket.OnRead += new ServerSocket.ConnectionDelegate(Server_OnRead);

           //if (ServerScoket.Active())
           //         Console.WriteLine("Card Reader Server Socket is Listening!");

           //tmr= new System.Threading.Timer(OneSecTask);
           //tmr.Change(0, 1000*5);
          Task.Run(new Action(OneSecTask));
       }

         bool IsInTmr = false;
         void OneSecTask( )
         {
             

                 if (IsInTmr)
                     return;
                 IsInTmr = true;
#if DEBUG
          //              return;
#endif
                 //     int status = ClientSocket.ReadAllState(1, IP);
                 while (true)
                 {
                     try
                     {
                         if (!RoomClient.RoomClient.GetControlConnectionStatus(this.ControllerID))
                         {

                             this.IsConnected = false;
                             Console.WriteLine(IP + "測試斷線!");
                         }
                         else
                         {
                             this.IsConnected = true;
                             //  Console.WriteLine(IP + "測試連線!");
                         }

                         if (this.IsConnected)
                         {

                             this.IsDoorOpen = (RoomClient.RoomClient.GetStatus(config.R23_ADAM)[config.R23DoorOpen_DI ?? 0] == 0) ? true : false;
                         }

                         System.Threading.Thread.Sleep(3000);

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
                     catch (Exception ex)
                     {
                         Console.WriteLine(ex.Message + "," + ex.StackTrace);
                     }
                     finally
                     {
                         IsInTmr = false;
                     }
                 }


         }


        public void SetDateTime(DateTime dt)
        {
            RoomClient.RoomClient.SetTime(this.ControllerID, dt);
           // throw new NotImplementedException();
        }

        public void SetOpenDoorAutoCloseTime(int sec)
        {

            
            throw new NotImplementedException();
        }

        public void SetOpenDoorDetectionAlarmTime(int sec)
        {
            throw new NotImplementedException();
        }

        public void SetOpenDoorTimeoutDetectionTime(int sec)
        {
            throw new NotImplementedException();
        }

        public void SetSuperOpenDoorPassword(int password)  //not in r23
        {
          //  throw new NotImplementedException();
        }

        public BindingData.DoorBindingData ToBindingData()
        {
            BindingData.DoorBindingData data = new BindingData.DoorBindingData()
            {
                ControlID = this.ControllerID,
                IsConnected = this.IsConnected,
                IsDoorOpen = this.IsDoorOpen

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

        public int TriggerCCTVID
        {

            get;
            set;
            //get
            //{
            //    throw new NotImplementedException();
            //}
            //set
            //{
            //    throw new NotImplementedException();
            //}
        }

        public void WriteCardReaderID(int id)
        {
          //  throw new NotImplementedException();
        }


        public bool SetR23Parameter(int RemoOpenTimeR23, int DelayTimeR23, int LoopErrorAlarmTimeR23, int AlarmTimeR23)
        {
            return   RoomClient.RoomClient.SetADAMAlarmTime(this.config.R23_ADAM,RemoOpenTimeR23, DelayTimeR23, LoopErrorAlarmTimeR23, AlarmTimeR23);
          //  throw new NotImplementedException();
        }


       


        //public static List<RoomInterface.PersonData> GetR23RoomPerson(string ErNo)
        //{
        // //   throw new NotImplementedException();

        //    return RoomClient.RoomClient.GetRoomPerson(ErNo);
        //}

        //public static bool SetR23EngineRoomRecovery(string ErNo)
        //{

        //    return RoomClient.RoomClient.RestaADAMControl(ErNo);
        //    //  throw new NotImplementedException();
        //}


        //public static object[] GetR23Progress()
        //{

          
        // //   RoomClient.RoomClient.GetControlConnect;,
            

        //    return RoomClient.RoomClient.GetGroupProgress();
        //}


        //public static string GetR23GroupErrorMessage()
        //{
        //    return RoomClient.RoomClient.GetGroupErrorMessage();
        //}

        //public static ControlStatus GetR23ControlConnect(string ControllID)
        //{
        //    return RoomClient.RoomClient.GetControlConnect(ControllID);
        //}


        public byte[] GetR23ReaderStatus()
        {
            return RoomClient.RoomClient.GetStatus(this.config.R23_ADAM);
            //throw new NotImplementedException();
        }
    }
}

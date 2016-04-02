using ClassSockets;
using SecureServer.BindingData;
using SecureServer.CCTV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SecureServer.CardReader
{
    public class CardReaderManager
    {
        public event DoorEventHandler OnDoorEvent;
        public event AlarmEventHandler OnAlarmEvent;
        int DoorPasswordGenerateDuration=24;
      //  int OpenDoorDetectionAlarmTime = 20;
        int DoorOpenAlarmTime = 20;
        int OpenDoorAutoCloseTime = 10;

        System.Collections.Concurrent.ConcurrentDictionary<string, ICardReader> dictCardReaders = new System.Collections.Concurrent.ConcurrentDictionary<string, ICardReader>();
        System.Collections.Generic.Dictionary<string, ICardReader> dictIp_CardReader = new Dictionary<string, ICardReader>();
          System.Collections.Generic.Dictionary<string, ICardReader> dictAdam_CardReader = new Dictionary<string, ICardReader>();
       
        SecureService serivce;
       // bool IntrusionAlarm = false;
        bool EventIntrusionAlarm = false;
        bool EventDoorOpenOverTimeAlarm = false;
        bool EventInvalidCardAlarm = false;
        bool EventExternalForceAlarm = false;
        bool EventDoorOpenAlarm = false;
        ClassSockets.ServerSocket ServerScoket;
#if R23
        int RemoOpenTimeR23, DelayTimeR23, LoopErrorAlarmTimeR23, AlarmTimeR23;
#endif
        //RemoOpenTime:遠端開門攜回延遲時間 DelayTime:刷退進入保全延遲時間  , LoopErrorAlarmTime:刷退回路異常(沒有關門)告警時間, AlarmTime:外力入侵告警時間  

       
        private void Server_OnRead(Socket soc)
        {
            byte[] SRbuf = ServerScoket.ReceivedBytes;

            if (SRbuf != null)
            {
                if (SRbuf.Length != 25)
                    return;
                string SRhexstr = BitConverter.ToString(SRbuf);
                CardReaderEventReport rpt = new CardReaderEventReport(SRbuf);
                if (!dictIp_CardReader.ContainsKey(rpt.IP))
                {
                    Console.WriteLine(rpt.IP + "  Not in dictionary!");
                    return;
                }
                ICardReader ireader = dictIp_CardReader[rpt.IP];
                Console.WriteLine(rpt);
                if (rpt.Status == (int)CardReaderStatusEnum.門開啟)
                    ireader.IsDoorOpen = true;

                if (rpt.Status == (int)CardReaderStatusEnum.門關閉)
                    ireader.IsDoorOpen = false;

                ireader.InvokeStatusChange(rpt);

            }

        }

#if R23
        public CardReaderManager(SecureService service)
        {
            try
            {




                this.serivce = service;
                SecureDBEntities1 db = new SecureDBEntities1();
                var q = from n in db.tblControllerConfig where (n.ControlType == 2 || n.ControlType == 1) && n.IsEnable == true select n;
                foreach (tblControllerConfig data in q)
                {
                    int nvrid = -1, nvrchano = -1;
//                    if (data.TriggerCCTVID != null)
//                    {
//#if R13
//                        nvrid = SecureService.cctv_mgr[(int)data.TriggerCCTVID].NVRID;
//                        nvrchano = SecureService.cctv_mgr[(int)data.TriggerCCTVID].NVRChNo;
//#endif
//                    }


                    ICardReader cardreader = new CardReader23(data.ControlID, data.IP, data.ERID, (int)data.PlaneID, data.TriggerCCTVID ?? -1, nvrid, nvrchano, data);

                    dictCardReaders.TryAdd(data.ControlID, cardreader);
                    dictIp_CardReader.Add(data.IP, cardreader);
                    if(!dictAdam_CardReader.ContainsKey(data.R23_ADAM))
                    dictAdam_CardReader.Add(data.R23_ADAM, cardreader);
                    cardreader.OnDoorEvent += cardreader_OnDoorEvent;
                    // cardreader.OnAlarmEvent += cardreader_OnAlarmEvent;
                   cardreader.OnStatusChanged += cardreader_OnStatusChanged;
                    Console.WriteLine("加入卡機:" + data.ControlID);
                }

                // 

                RoomClient.RoomClient.RoomEvent += RoomClient_RoomEvent;                //ServerScoket = new ClassSockets.ServerSocket();
                //ServerScoket.OnRead += new ServerSocket.ConnectionDelegate(Server_OnRead);

                //if (ServerScoket.Active())
                //    Console.WriteLine("Card Reader Server Socket is Listening!");

                //else

                //    Console.WriteLine("Card Reader Server Socket is  not Listening!");


                Task.Run(new Action(OneMinTask));

                //tmr = new System.Threading.Timer(OneMinTask);
                //System.Timers.Timer tmr = new System.Timers.Timer(3000);

                //tmr.Elapsed += (s, e) =>
                //      {
                //          System.Console.Beep();
                //      };
                //tmr.Start(); 

             //===============2015/12/8========================  
               //this.LoadSystemParameter();
               // this.SendAllReaderParameter();
             
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "," + ex.StackTrace);
            }
        }

        public static CardReaderEventReport MakeReaderEventReport(byte status)
        {
            byte[] data = new byte[25];
            data[1] = (byte)((DateTime.Now.Year - 2000) / 10 * 16 + (DateTime.Now.Year - 2000) % 10);
            data[3] = (byte)((DateTime.Now.Month) / 10 * 16 + (DateTime.Now.Month) % 10);
            data[5] = (byte)((DateTime.Now.Day) / 10 * 16 + (DateTime.Now.Day) % 10);
            data[9] = (byte)((DateTime.Now.Hour) / 10 * 16 + (DateTime.Now.Hour) % 10);
            data[11] = (byte)((DateTime.Now.Minute) / 10 * 16 + (DateTime.Now.Minute) % 10);
            data[13] = (byte)((DateTime.Now.Second) / 10 * 16 + (DateTime.Now.Second) % 10);
            data[19] = status;
            CardReaderEventReport rpt = new CardReaderEventReport(data);
            return rpt;
        }

        void RoomClient_RoomEvent(RoomInterface.ControllEventType type, string Name, object obj)
        {
          //  byte[] data = new byte[25];
            CardReaderEventReport rpt=null;
            switch (type)
            {
                case RoomInterface.ControllEventType.ErrorCard:
                    rpt = MakeReaderEventReport((byte)CardReaderStatusEnum.號碼錯誤);
                    break;
                case RoomInterface.ControllEventType.ADAMStatusChange:
                    if (((byte[])obj)[32] == 5)
                     rpt=  MakeReaderEventReport( (byte)CardReaderStatusEnum.異常入侵);
                  
                    break;
                default:
                    return;
            }

             //data[1]=(byte)((DateTime.Now.Year-2000)/10*16+(DateTime.Now.Year-2000)%10);
             //    data[3]=(byte)((DateTime.Now.Month)/10*16+(DateTime.Now.Month)%10);
             //    data[5] = (byte)((DateTime.Now.Day) / 10 * 16 + (DateTime.Now.Day) % 10);
             //    data[9] = (byte)((DateTime.Now.Hour) / 10 * 16 + (DateTime.Now.Hour) % 10);
             //    data[11] = (byte)((DateTime.Now.Minute) / 10 * 16 + (DateTime.Now.Minute) % 10);
             //    data[13] = (byte)((DateTime.Now.Second) / 10 * 16 + (DateTime.Now.Second) % 10);
             //    CardReaderEventReport rpt = new CardReaderEventReport(data);
                 if (dictAdam_CardReader.ContainsKey(Name))
                 {
                     dictAdam_CardReader[Name].InvokeStatusChange(rpt);
                 }
             
              
          //  throw new NotImplementedException();
        }


#else
           public CardReaderManager(SecureService service)
        {
            try
            {
              



                this.serivce = service;
                SecureDBEntities1 db = new SecureDBEntities1();
                var q = from n in db.tblControllerConfig where (n.ControlType == 2 || n.ControlType == 1) && n.IsEnable == true select n;
                foreach (tblControllerConfig data in q)
                {
                    int nvrid = -1, nvrchano = -1;
                    if (data.TriggerCCTVID != null )
                    {
 
                        nvrid = SecureService.cctv_mgr[(int)data.TriggerCCTVID].NVRID;
                        nvrchano = SecureService.cctv_mgr[(int)data.TriggerCCTVID].NVRChNo;
 
                    }

                    ICardReader cardreader = new CardReader(data.ControlID, data.IP, data.ERID, (int)data.PlaneID, data.TriggerCCTVID ?? -1, nvrid, nvrchano,data.Comm_state==1?true:false);
 
                    dictCardReaders.TryAdd(data.ControlID, cardreader);
                    dictIp_CardReader.Add(data.IP, cardreader);
                    cardreader.OnDoorEvent += cardreader_OnDoorEvent;
                    //   cardreader.OnAlarmEvent += cardreader_OnAlarmEvent;
                    cardreader.OnStatusChanged += cardreader_OnStatusChanged;
                    Console.WriteLine("加入卡機:" + data.ControlID);
                }

                // 
                ServerScoket = new ClassSockets.ServerSocket();
                ServerScoket.OnRead += new ServerSocket.ConnectionDelegate(Server_OnRead);

                if (ServerScoket.Active())
                    Console.WriteLine("Card Reader Server Socket is Listening!");

                else

                    Console.WriteLine("Card Reader Server Socket is  not Listening!");

                Task.Run(new Action(OneMinTask));

                //tmr = new System.Threading.Timer(OneMinTask);
                //tmr.Change(0, 1000 * 60);

                this.LoadSystemParameter();
                this.SendAllReaderParameter();
               // DownloadSuperPassword();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "," + ex.StackTrace);
            }

        }
#endif
#if R23

        public void LoadSystemParameter()
        {

            SecureDBEntities1 db = new SecureDBEntities1();
            //tblSysParameter tbl=  db.tblSysParameter.FirstOrDefault();
            //if (tbl == null)
            //    return;

            //開門延時偵測警報時間
            tblSysParameter tbl = db.tblSysParameter.Where(n => n.SysID == 1).FirstOrDefault();
            if (tbl != null)
            {
                this.RemoOpenTimeR23 = System.Convert.ToInt32(tbl.VariableValue);
            }

            tbl = db.tblSysParameter.Where(n => n.SysID == 2).FirstOrDefault();
            if (tbl != null)
            {
                this.DelayTimeR23 = System.Convert.ToInt32(tbl.VariableValue);
            }

            tbl = db.tblSysParameter.Where(n => n.SysID == 3).FirstOrDefault();
            if (tbl != null)
            {
                this.LoopErrorAlarmTimeR23 = System.Convert.ToInt32(tbl.VariableValue);
            }

            tbl = db.tblSysParameter.Where(n => n.SysID == 4).FirstOrDefault();
            if (tbl != null)
            {
                this.AlarmTimeR23 = System.Convert.ToInt32(tbl.VariableValue);
            }

            SendAllReaderParameter();

        }
#else

        public void LoadSystemParameter()
        {
            SecureDBEntities1 db = new SecureDBEntities1();
          //tblSysParameter tbl=  db.tblSysParameter.FirstOrDefault();
          //if (tbl == null)
          //    return;

            //開門延時偵測警報時間
            tblSysParameter tbl=db.tblSysParameter.Where(n=>n.SysID==4).FirstOrDefault();
            if (tbl != null)
            {
                this.DoorOpenAlarmTime= System.Convert.ToInt32(tbl.VariableValue);
            }

            //電磁鎖回復時間
            tbl = db.tblSysParameter.Where(n => n.SysID== 1).FirstOrDefault();
            if (tbl != null)
            {
                this.OpenDoorAutoCloseTime = System.Convert.ToInt32(tbl.VariableValue);
            }

            //異常入侵
            tbl = db.tblSysParameter.Where(n => n.SysID == 5).FirstOrDefault();
            if (tbl != null)
            {
                this.EventIntrusionAlarm = tbl.VariableValue=="Y";
            }

            //超過某時段未關門警告
            tbl = db.tblSysParameter.Where(n => n.SysID == 6).FirstOrDefault();
            if (tbl != null)
            {
                this.EventDoorOpenOverTimeAlarm = tbl.VariableValue == "Y";
            }
            //無效卡讀卡超次警告
            tbl = db.tblSysParameter.Where(n => n.SysID == 7).FirstOrDefault();
            if (tbl != null)
            {
                this.EventInvalidCardAlarm = tbl.VariableValue == "Y";
            }
            //讀卡機遇外力破壞或拆機警告
            tbl = db.tblSysParameter.Where(n => n.SysID == 8).FirstOrDefault();
            if (tbl != null)
            {
                this.EventExternalForceAlarm = tbl.VariableValue == "Y";
            }
            //開門(電磁鎖開啟/鐵捲門開啟)
            tbl = db.tblSysParameter.Where(n => n.SysID == 9).FirstOrDefault();
            if (tbl != null)
            {
                this.EventDoorOpenAlarm = tbl.VariableValue == "Y";
            }

            SendAllReaderParameter();
        }
#endif

#if R23 

        public void SendAllReaderParameter()
        {
            foreach (ICardReader reader in dictCardReaders.Values)
            {
                try
                {
                    //reader.SetOpenDoorAutoCloseTime(OpenDoorAutoCloseTime);
                    //reader.SetOpenDoorTimeoutDetectionTime(DoorOpenAlarmTime);
                    //reader.SetOpenDoorDetectionAlarmTime(0);
                    reader.SetR23Parameter(RemoOpenTimeR23, DelayTimeR23, LoopErrorAlarmTimeR23, AlarmTimeR23);
                }
                catch { ;}
            }


        }

#else

        public void SendAllReaderParameter()
        {
            foreach (ICardReader reader in dictCardReaders.Values)
            {
                try
                {
                    reader.SetOpenDoorAutoCloseTime(OpenDoorAutoCloseTime);
                    reader.SetOpenDoorTimeoutDetectionTime(DoorOpenAlarmTime);
                    reader.SetOpenDoorDetectionAlarmTime(0);
                }
                catch { ;}
            }


        }

        public void SendReaderParameter( ICardReader reader)
        {
                     reader.SetOpenDoorAutoCloseTime(OpenDoorAutoCloseTime);
                    reader.SetOpenDoorTimeoutDetectionTime(DoorOpenAlarmTime);
                    reader.SetOpenDoorDetectionAlarmTime(0);
        }
#endif
        public ICardReader this[string ControllID]
        {

            get
            {
                //Console.WriteLine("dictCardReaders:" + ControllID);
                if (!dictCardReaders.ContainsKey(ControllID))
                    return null;

                return dictCardReaders[ControllID] ;
            }
        }

        public BindingData.DoorBindingData GetDoorBindingData(string ControllerID)
        {
            if (!dictCardReaders.ContainsKey(ControllerID))
                throw new Exception("Not found!");

            return dictCardReaders[ControllerID].ToBindingData();
        }

        public BindingData.DoorBindingData[] GetAllDoorBindingData(int PlaneID)
        {
            System.Collections.Generic.List<DoorBindingData> list = new List<DoorBindingData>();

            foreach (ICardReader pair in dictCardReaders.Values.ToArray())
            {
                if (pair.PlaneID == PlaneID)
                    list.Add(pair.ToBindingData());
            }

            return list.ToArray();
        }
     


        //public int  LastOperationCCTVID=-1;
        //public DateTime LastOperationCCTVTime;
        System.Collections.Generic.List<int> InOperationCCTV = new List<int>();
        void cardreader_OnStatusChanged(ICardReader reader, CardReaderEventReport rpt)
        {

            try
            {
                if (rpt.Status == (int)CardReaderStatusEnum.卡號連續錯誤 && this.EventInvalidCardAlarm || rpt.Status == (int)CardReaderStatusEnum.外力破壞 && this.EventExternalForceAlarm ||
                     rpt.Status == (int)CardReaderStatusEnum.異常入侵 && this.EventIntrusionAlarm || rpt.Status == (int)CardReaderStatusEnum.開門超時 && this.EventDoorOpenOverTimeAlarm ||
                     rpt.Status == (int)CardReaderStatusEnum.開鎖 && this.EventDoorOpenAlarm
                   )
                {
                    if (this.OnAlarmEvent != null)
                    {
                        ICCTV cctv = (ICCTV)SecureService.cctv_mgr[reader.TriggerCCTVID];
                        AlarmData data = new AlarmData()
                        {
                            TimeStamp = DateTime.Now,
                            AlarmType = AlarmType.Secure,
                            ColorString = "Red",
                            Description = reader.ControllerID + "," + rpt.StatusString,
                            PlaneID = reader.PlaneID,
                            IsForkCCTVEvent = true,
                            PlaneName = Global.GetPlaneNameByPlaneID(reader.PlaneID),
                            CCTVBindingData = cctv!=null?cctv.ToBindingData():null






                        };


                        this.OnAlarmEvent(reader, data);

                    }
                }



                if (rpt.Status == (int)CardReaderStatusEnum.開鎖 ||
                    rpt.Status == (int)CardReaderStatusEnum.按鈕開門 ||
                    rpt.Status == (int)CardReaderStatusEnum.密碼開門 || rpt.Status == (int)CardReaderStatusEnum.系統開門 ||
                    rpt.Status == (int)CardReaderStatusEnum.異常入侵 || rpt.Status == (int)CardReaderStatusEnum.開門超時)
                {
                    SecureDBEntities1 db = new SecureDBEntities1();
                    tblEngineRoomLog log = new tblEngineRoomLog()
                          {
                              ControlID = reader.ControllerID,
                              ABA = rpt.CardNo.ToString(),
                              StartTime = DateTime.Now,
                              TypeID = 8,
                              Memo = rpt.StatusString,
                              TypeCode = (short)rpt.Status,
                              ERNo = reader.PlaneID.ToString()
                          };
                    db.tblEngineRoomLog.Add(

                        log
                        );






                    db.SaveChanges();
                    //開門錄影
                    if (rpt.Status == (int)CardReaderStatusEnum.開鎖 ||
                   rpt.Status == (int)CardReaderStatusEnum.按鈕開門 ||
                   rpt.Status == (int)CardReaderStatusEnum.密碼開門 || rpt.Status == (int)CardReaderStatusEnum.系統開門)
                    {


                        if (reader.NVRID == -1)
                            return;
                        #region 擷取錄影
#if R23
#else
                        Task task = Task.Factory.StartNew(() =>
                            {

                                DateTime dt = DateTime.Now;
                                System.Threading.Thread.Sleep(1000 * 20);

                                long flowid = log.FlowID;
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("nvrid:" + reader.NVRID);
                                try
                                {
                                    NVR.INVR nvr = SecureService.nvr_mgr[reader.NVRID];
                                    if (nvr == null)
                                    {
                                        Console.WriteLine(reader.NVRID + " is null");
                                        return;
                                    }


                                    bool success = nvr.SaveRecord(

                                //           reader.NVRChNo, dt.AddSeconds(-10), dt.AddSeconds(10), @"C:\web\Secure\ClientBin\VideoRecord\" + flowid + ".avi");

                                     reader.NVRChNo, dt.AddSeconds(-10), dt.AddSeconds(10), @"E:\web\Secure\ClientBin\VideoRecord\" + flowid + ".avi");

                                    //bool success = nvr.SaveRecord(
                                    //reader.NVRChNo, dt.AddSeconds(-10), dt.AddSeconds(10), @"D:\" + flowid + ".avi");

                                    log.NVRFile = flowid + ".wmv";
                                    db.SaveChanges();
                                    Console.WriteLine(success);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message + "," + ex.StackTrace);
                                }
                                finally
                                {
                                    Console.ResetColor();
                                }
                            });
#endif

                        #endregion



                    }

                    if (this.serivce != null && reader.TriggerCCTVID != 0)
                    {


                        if (InOperationCCTV.Where(n => n == reader.TriggerCCTVID).Count() > 0)
                        {
                            Task.Factory.StartNew(() =>
                                {
                                    try
                                    {


                                        Console.WriteLine("Trigger " + reader.TriggerCCTVID);
                                        SecureService.cctv_mgr[reader.TriggerCCTVID].Preset(2);
                                        System.Threading.Thread.Sleep(1000 * 10);
                                        SecureService.cctv_mgr[reader.TriggerCCTVID].Preset(1);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("May be trigger cctv not found" + ex.Message + "," + ex.StackTrace);
                                    }
                                    InOperationCCTV.Remove(reader.TriggerCCTVID);
                                });

                        }
                    }

                }


                if (rpt.Status == (int)CardReaderStatusEnum.號碼錯誤 ||
                  rpt.Status == (int)CardReaderStatusEnum.卡號連續錯誤 ||
                  rpt.Status == (int)CardReaderStatusEnum.外力破壞
                  )
                {
                    SecureDBEntities1 db = new SecureDBEntities1();

                    db.tblEngineRoomLog.Add(

                          new tblEngineRoomLog()
                          {
                              ControlID = reader.ControllerID,
                              ABA = rpt.CardNo.ToString(),
                              StartTime = DateTime.Now,
                              TypeID = 8,
                              Memo = rpt.StatusString,
                              TypeCode = (short)rpt.Status,
                              ERNo = reader.PlaneID.ToString()
                          }
                        );
                    db.SaveChanges();

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("CardManager OnStatus Change:" + ex.Message + "," + ex.StackTrace);
            }
               
           
           
        }

        //void cardreader_OnAlarmEvent(CardReader reder, AlarmData alarmdata)
        //{
        //    if (this.OnAlarmEvent != null)
        //        this.OnAlarmEvent(reder, alarmdata);
        //}

        void cardreader_OnDoorEvent(ICardReader reader, DoorEventType enumEventType)
        {
            if (this.OnDoorEvent != null)
            {
                try
                {
                    this.OnDoorEvent(reader, enumEventType);
                }
                catch { ;}
            }

            if (enumEventType == DoorEventType.Connected)
            {

                SecureDBEntities1 db = new SecureDBEntities1();
                tblControllerConfig config = db.tblControllerConfig.Where(n => n.ControlID == reader.ControllerID).FirstOrDefault();
                if (config != null)
                    config.Comm_state = 1;
                tblEngineRoomLog log = new tblEngineRoomLog() { ControlID = reader.ControllerID, ABA = "0", StartTime = DateTime.Now, TypeID = 8, TypeCode = 31, Result = 1 };
                db.tblEngineRoomLog.Add(log);
                db.SaveChanges();
                db.Dispose();

                 AlarmData data = new AlarmData()
                {
                    TimeStamp = DateTime.Now,
                    AlarmType = AlarmType.Secure,
                    ColorString = "Green",
                    Description = reader.ControllerID + "復線",
                    PlaneID = reader.PlaneID,
                    IsForkCCTVEvent = false,
                    PlaneName = Global.GetPlaneNameByPlaneID(reader.PlaneID)
                    //  CCTVBindingData =cctv.ToBindingData(

                };
                
                    Program.MyServiceObject.DispatchAlarmEvent(data);

            
                this.DownloadSuperPassword(reader.ControllerID);
            }
            if (enumEventType == DoorEventType.DisConnected)
            {
               
                SecureDBEntities1 db = new SecureDBEntities1();

                tblControllerConfig config = db.tblControllerConfig.Where(n => n.ControlID == reader.ControllerID).FirstOrDefault();
                if (config != null)
                    config.Comm_state = 0;
                 tblEngineRoomLog log=new tblEngineRoomLog(){ ControlID=reader.ControllerID, ABA="0", StartTime=DateTime.Now,  TypeID=8, TypeCode=30,  Result=0};
                 db.tblEngineRoomLog.Add(log);
                 db.SaveChanges();
                db.Dispose();


                AlarmData data = new AlarmData()
                {
                    TimeStamp = DateTime.Now,
                    AlarmType = AlarmType.Secure,
                    ColorString = "Red",
                    Description = reader.ControllerID + "斷線警報",
                    PlaneID = reader.PlaneID,
                    IsForkCCTVEvent = false,
                    PlaneName = Global.GetPlaneNameByPlaneID(reader.PlaneID)
                    //  CCTVBindingData =cctv.ToBindingData(

                };
                
                    Program.MyServiceObject.DispatchAlarmEvent(data);

            }



        }


        void OneMinTask( )
        {
            try
            {
                while (true)
                {
#if !R23
                    CheckAndGenerateDailySuperPassword();
#endif
                    Console.WriteLine("In one Min task!");
                 //   System.Console.Beep();
                    System.Threading.Thread.Sleep(1000*60);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "," + ex.StackTrace);
            }
            finally
            {
              //  tmr.Change(0, 1000 * 60);
            }

          
       

        }
        void DownloadSuperPassword(string readerID)
        {
            SecureDBEntities1 db = new SecureDBEntities1();
            DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            tblERDoorPassword tbl = (from n in db.tblERDoorPassword
                                     where n.Timestamp
                                         == dt
                                     select n).FirstOrDefault();

            if (tbl == null)
                return;
            dictCardReaders[readerID].SetSuperOpenDoorPassword(int.Parse(tbl.DoorPassword));
            db.tblEngineRoomLog.Add(
                                 new tblEngineRoomLog()
                                 {
                                     ControlID = readerID,//reader.ControllerID,
                                     Result = 0,
                                     StartTime = DateTime.Now,
                                     ABA = "0",
                                     TypeID = 8,
                                     TypeCode = 40,
                                     Memo = tbl.DoorPassword
                                 }
                                 );
            db.SaveChanges();
        }

        //void DownloadSuperPassword()
        //{
        //    SecureDBEntities1 db = new SecureDBEntities1();
        //    DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //    tblPasswordEveryDayDifference tbl = (from n in db.tblPasswordEveryDayDifference
        //                             where n.Timestamp
        //                                 == dt
        //                             select n).FirstOrDefault();

        //    if (tbl == null)
        //        return;

        //    foreach (CardReader reader in dictCardReaders.Values)
        //    {
        //        try
        //        {
        //            if (reader.IsConnected)
        //            {
        //                reader.SetSuperOpenDoorPassword(int.Parse(tbl.DoorPassword));
        //                Console.WriteLine(reader.ControllerID + "設定每日開門密碼成功");


        //                db.tblEngineRoomLog.Add(
        //                              new tblEngineRoomLog()
        //                              {
        //                                  ControlID = reader.ControllerID,
        //                                  Result = 0,
        //                                  StartTime = DateTime.Now,
        //                                  ABA = "0",
        //                                  TypeID = 8,
        //                                  TypeCode = 40,
        //                                  Memo = tbl.DoorPassword
        //                              }
        //                              );
                   
                     
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message + "," + ex.StackTrace);
        //        }
        //    }

        //    db.SaveChanges();

         
        //}

        void CheckAndGenerateDailySuperPassword()
        {
#if R23
            return;
#endif

            bool haschanges = false;
            using (SecureDBEntities1 db = new SecureDBEntities1())
            {
                Random rnd = new Random();
                var q = from n in db.tblControllerConfig where n.ControlType == 1 && n.IsEnable == true select n;
                DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                foreach (tblControllerConfig control in q)
                {
                  
                    tblPasswordEveryDayDifference tbl = (from n in db.tblPasswordEveryDayDifference
                                                         where n.Timestamp
                                                             == dt && n.ControlID == control.ControlID
                                                         select n).FirstOrDefault();
                    if (tbl == null)
                    {
                       

                        int passwd = rnd.Next(0, 10000);
                        string pwdString = passwd.ToString("0000");
                        //  tblERDoorPassword tbl1 = 



                        //foreach (CardReader reader in dictCardReaders.Values)
                        //{
                        try
                        {
                            ICardReader reader = dictCardReaders[control.ControlID];
                            reader.SetSuperOpenDoorPassword(passwd);
                            Console.WriteLine(reader.ControllerID + "設定每日開門密碼成功");
                            db.tblPasswordEveryDayDifference.Add(new tblPasswordEveryDayDifference() { Timestamp = dt, DoorPassword = passwd.ToString("0000"), ControlID = control.ControlID });
                            db.tblEngineRoomLog.Add(
                                   new tblEngineRoomLog()
                                   {
                                       ControlID = reader.ControllerID,
                                       Result = 0,
                                       StartTime = DateTime.Now,
                                       ABA = "0",
                                       TypeID = 8,
                                       TypeCode = 40,
                                       Memo = pwdString
                                   }
                                   );
                            haschanges = true;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message + "," + ex.StackTrace);
                        }
                        //}
                    }
                }
                if (haschanges)
                {
                    Console.WriteLine("開始寫入每日密碼!");
                    db.SaveChanges();
                }
            }
        }
        

        //public class CardReaderInfo
        //{
        //    public CardReader cardReader { get; set; }
        //    public tblControllerConfig cardData;
        //    public string IP { get; set; }
        //    public bool IsConnected
        //    {

        //        get
        //        {
        //            return cardReader.IsConnected;
        //        }
        //    }
           
        //    public CardReaderInfo(string ip, tblControllerConfig cardData)
        //    {
        //        this.IP = ip;
        //        this.cardData = cardData;
        //        cardReader = new CardReader(ip);
        //    }



        //}
    }
}

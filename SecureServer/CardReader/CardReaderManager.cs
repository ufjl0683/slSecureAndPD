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
        System.Collections.Generic.Dictionary<string, ICardReader> dictCardReaders = new Dictionary<string, ICardReader>();
        System.Collections.Generic.Dictionary<string, ICardReader> dictIp_CardReader = new Dictionary<string, ICardReader>();
        System.Threading.Timer tmr;
        SecureService serivce;
       // bool IntrusionAlarm = false;
        bool EventIntrusionAlarm = false;
        bool EventDoorOpenOverTimeAlarm = false;
        bool EventInvalidCardAlarm = false;
        bool EventExternalForceAlarm = false;
        bool EventDoorOpenAlarm = false;
        ClassSockets.ServerSocket ServerScoket;
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
                    CardReader cardreader = new CardReader(data.ControlID, data.IP, data.ERID, (int)data.PlaneID, data.TriggerCCTVID ?? -1, nvrid, nvrchano);
                    dictCardReaders.Add(data.ControlID, cardreader);
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



                tmr = new System.Threading.Timer(OneMinTask);
                tmr.Change(0, 1000 * 60);

                this.LoadSystemParameter();
                this.SendAllReaderParameter();
                DownloadSuperPassword();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "," + ex.StackTrace);
            }

        }
        private void Server_OnRead(Socket soc)
        {
            byte[] SRbuf = ServerScoket.ReceivedBytes;

            if (SRbuf != null)
            {
                if (SRbuf.Length != 25)
                    return;
                string SRhexstr = BitConverter.ToString(SRbuf);
                CardReaderEventReport rpt = new CardReaderEventReport(SRbuf);
                ICardReader ireader = dictIp_CardReader[rpt.IP];
                Console.WriteLine(rpt);
                if (rpt.Status == (int)CardReaderStatusEnum.門開啟)
                    ireader.IsDoorOpen = true;

                if (rpt.Status == (int)CardReaderStatusEnum.門關閉)
                    ireader.IsDoorOpen = false;

                ireader.InvokeStatusChange(rpt);

            }

        }
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

        public void SendAllReaderParameter()
        {
            foreach (CardReader reader in dictCardReaders.Values)
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

        public void SendReaderParameter( CardReader reader)
        {
                     reader.SetOpenDoorAutoCloseTime(OpenDoorAutoCloseTime);
                    reader.SetOpenDoorTimeoutDetectionTime(DoorOpenAlarmTime);
                    reader.SetOpenDoorDetectionAlarmTime(0);
        }
        public ICardReader this[string ControllID]
        {

            get
            {
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

            foreach (CardReader pair in dictCardReaders.Values.ToArray())
            {
                if (pair.PlaneID == PlaneID)
                    list.Add(pair.ToBindingData());
            }

            return list.ToArray();
        }
     


        //public int  LastOperationCCTVID=-1;
        //public DateTime LastOperationCCTVTime;
        System.Collections.Generic.List<int> InOperationCCTV = new List<int>();
        void cardreader_OnStatusChanged(CardReader reader, CardReaderEventReport rpt)
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
                        Task task = Task.Factory.StartNew(() =>
                            {

                                DateTime dt = DateTime.Now;
                                System.Threading.Thread.Sleep(1000 * 20);

                                long flowid = log.FlowID;
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
                            });

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

        void cardreader_OnDoorEvent(CardReader reader, DoorEventType enumEventType)
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
                this.DownloadSuperPassword(reader.ControllerID);
            }
            if (enumEventType == DoorEventType.DisConnected)
            {
               
                SecureDBEntities1 db = new SecureDBEntities1();
                 tblEngineRoomLog log=new tblEngineRoomLog(){ ControlID=reader.ControllerID, ABA="0", StartTime=DateTime.Now,  TypeID=8, TypeCode=30,  Result=0};
                 db.tblEngineRoomLog.Add(log);
                 db.SaveChanges();
                db.Dispose();
            }
        }


        void OneMinTask(object o)
        {
            try
            {
                CheckAndGenerateDailySuperPassword();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message+","+ex.StackTrace);
            }
           
            Console.WriteLine("In one Min task!");

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
        }

        void DownloadSuperPassword()
        {
            SecureDBEntities1 db = new SecureDBEntities1();
            DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            tblERDoorPassword tbl = (from n in db.tblERDoorPassword
                                     where n.Timestamp
                                         == dt
                                     select n).FirstOrDefault();

            if (tbl == null)
                return;

            foreach (CardReader reader in dictCardReaders.Values)
            {
                try
                {
                    if (reader.IsConnected)
                    {
                        reader.SetSuperOpenDoorPassword(int.Parse(tbl.DoorPassword));
                        Console.WriteLine(reader.ControllerID + "設定每日開門密碼成功");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + "," + ex.StackTrace);
                }
            }
        }

        void CheckAndGenerateDailySuperPassword()
        {
            SecureDBEntities1 db = new SecureDBEntities1();
            DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            tblERDoorPassword tbl = (from n in db.tblERDoorPassword
                                     where n.Timestamp
                                         == dt
                                     select n).FirstOrDefault();
            if (tbl == null)
            {
                Random rnd = new Random();
               
                int passwd=rnd.Next(0, 10000);
                string pwdString=passwd.ToString("0000");
              //  tblERDoorPassword tbl1 = 
                db.tblERDoorPassword.Add(new tblERDoorPassword() { Timestamp = dt, DoorPassword = passwd.ToString("0000") });
                db.SaveChanges();


                foreach (CardReader  reader in dictCardReaders.Values)
                {
                    try
                    {
                        reader.SetSuperOpenDoorPassword(passwd);
                        Console.WriteLine(reader.ControllerID+"設定每日開門密碼成功" );
                    }
                    catch(Exception  ex)
                    {
                        Console.WriteLine(ex.Message + "," + ex.StackTrace);
                    }
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

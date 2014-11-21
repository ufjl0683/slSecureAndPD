using SecureServer.CardReader.BindingData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureServer.CardReader
{
    public class CardReaderManager
    {
        public event DoorEventHandler OnDoorEvent;
        public event AlarmEventHandler OnAlarmEvent;
        System.Collections.Generic.Dictionary<string, ICardReader> dictCardReaders = new Dictionary<string, ICardReader>();
        System.Threading.Timer tmr;

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
        public CardReaderManager()
        {
            SecureDBEntities1 db = new SecureDBEntities1();
            var q = from n in db.tblControllerConfig where (n.ControlType == 2 || n.ControlType == 1) && n.IsEnable==true select n;
            foreach (tblControllerConfig data in q)
            {

                CardReader cardreader = new CardReader(data.ControlID, data.IP, data.ERID, (int)data.PlaneID);
                dictCardReaders.Add(data.ControlID,cardreader );
                cardreader.OnDoorEvent += cardreader_OnDoorEvent;
             //   cardreader.OnAlarmEvent += cardreader_OnAlarmEvent;
                cardreader.OnStatusChanged += cardreader_OnStatusChanged;
                Console.WriteLine("加入卡機:" + data.ControlID);
            }

            tmr = new System.Threading.Timer(OneMinTask);
            tmr.Change(0, 1000 * 60);
           



        }

        void cardreader_OnStatusChanged(CardReader reader, CardReaderEventReport rpt)
        {
                if(rpt.Status==(int)CardReaderStatusEnum.卡號連續錯誤||rpt.Status==(int)CardReaderStatusEnum.外力破壞 ||
                     rpt.Status == (int)CardReaderStatusEnum.異常入侵 || rpt.Status == (int)CardReaderStatusEnum.開門超時 
                   )
               {
                  if(this.OnAlarmEvent!=null)
                  {
                      AlarmData data=new AlarmData()
                      {
                          TimeStamp=DateTime.Now,
                           AlarmType="Door",
                            ColorString="Red",
                             Description=reader.ControllerID + "," + rpt.StatusString,
                               PlaneID=reader.PlaneID,
                               PlaneName=Global.GetPlaneNameByPlaneID(reader.PlaneID)
                               
                      };
                      this.OnAlarmEvent(reader,data );

                  }
               }

            

               if (rpt.Status == (int)CardReaderStatusEnum.開鎖 ||
                   rpt.Status == (int)CardReaderStatusEnum.按鈕開門 ||
                   rpt.Status == (int)CardReaderStatusEnum.密碼開門 || rpt.Status == (int)CardReaderStatusEnum.系統開門 ||
                   rpt.Status == (int)CardReaderStatusEnum.異常入侵 || rpt.Status == (int)CardReaderStatusEnum.開門超時)
                    
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
                             ERNo=reader.PlaneID.ToString()
                         }
                       );
                   db.SaveChanges();
                  
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

        //void cardreader_OnAlarmEvent(CardReader reder, AlarmData alarmdata)
        //{
        //    if (this.OnAlarmEvent != null)
        //        this.OnAlarmEvent(reder, alarmdata);
        //}

        void cardreader_OnDoorEvent(CardReader reader, DoorEventType enumEventType)
        {
            if (this.OnDoorEvent != null)
                this.OnDoorEvent(reader, enumEventType);
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

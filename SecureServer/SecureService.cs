using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Net;

namespace SecureServer.CardReader
{
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.Single)] 
   public  class SecureService:ISecureService
    {

        public System.Collections.Generic.Dictionary<string, RegisterInfo> dictClientCallBacks = new Dictionary<string, RegisterInfo>();
        public CardReaderManager card_mgr  ;

        public CCTV.CCTVManager cctv_mgr;
        ExactIntervalTimer ExactOneHourTmr;


        public  SecureService()
        {
           card_mgr = new CardReaderManager();

           card_mgr.OnDoorEvent += card_mgr_OnDoorEvent;
           card_mgr.OnAlarmEvent += card_mgr_OnAlarmEvent;
           new System.Threading.Thread(CheckCardReaderConnectionTask).Start();
           ExactOneHourTmr = new ExactIntervalTimer(0, 0);
           ExactOneHourTmr.OnElapsed += ExactOneHourTmr_OnElapsed;

           cctv_mgr = new CCTV.CCTVManager();
         
        }

        void ExactOneHourTmr_OnElapsed(object sender)
        {

            CheckCardDueTask();


            //throw new NotImplementedException();
        }

        void CheckCardDueTask()
        {
            SecureDBEntities1 db = new SecureDBEntities1();
            var q = from n in db.vwMagneticCardAllowController  select n;
            foreach (vwMagneticCardAllowController vw in q)
            {
                if ((System.DateTime.Now > vw.EndDate || System.DateTime.Now < vw.StartDate) && card_mgr[vw.ControlID].IsConnected)
                {
                    try
                    {
                        card_mgr[vw.ControlID].DeleteCard(vw.ABA);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + "," + ex.StackTrace);
                    }
                }

                if ((System.DateTime.Now <= vw.EndDate && System.DateTime.Now >= vw.StartDate) && card_mgr[vw.ControlID].IsConnected)
                {
                    try
                    {
                       if(vw.Type==4)
                           card_mgr[vw.ControlID].AddVirturalCard(vw.ABA);
                        else
                        card_mgr[vw.ControlID].AddCard(vw.ABA);
                    }
                    catch (Exception ex)
                    {

                        ;
                        // Console.WriteLine(ex.Message+","+ex.StackTrace);
                    }



                }
            }

           // NotifyDBChange(DBChangedConstant.AuthorityChanged);
        }

        void card_mgr_OnAlarmEvent(CardReader reader, AlarmData alarmdata)
        {
            try{
                DispatchAlarmEvent(alarmdata);
            }
            catch{;}
            //throw new NotImplementedException();
        }

        void card_mgr_OnDoorEvent(CardReader reader, DoorEventType enumEventType)
        {
            Console.WriteLine(reader.ControllerID + "," + enumEventType.ToString());

            foreach (KeyValuePair<string, RegisterInfo> pair in dictClientCallBacks.ToArray())
            {
                try
                {
                    if (pair.Value.IsRegistDoorEvent && pair.Value.PlaneID == reader.PlaneID)
                        pair.Value.CallBack.SecureDoorEvent(enumEventType, reader.ToBindingData());
                }
                catch { ;}

             
            }
        }
        public string Register(string PCName)  //return key
        {

            string GUID=Guid.NewGuid().ToString();
            RegisterInfo info = new RegisterInfo()
            {
                PCName = ( OperationContext.Current.Channel.RemoteAddress).ToString(),
                CallBack = OperationContext.Current.GetCallbackChannel<ISecureServiceCallBack>(),
                 Key=GUID};
            
            dictClientCallBacks.Add(GUID, info);
            Console.WriteLine(info.PCName + ",registed!");

            return GUID;
          //  throw new NotImplementedException();
        }


        void CheckCardReaderConnectionTask()
        {
            while (true)
            {
                try
                {
                    foreach (KeyValuePair<string, RegisterInfo> pair in dictClientCallBacks.ToArray())
                    {
                        try
                        {
                            if (pair.Value.CallBack != null)
                                pair.Value.CallBack.SayHello("hello");
                            //else
                            //{
                            //    if (!Service.NotifyServer.IsRegistered(pair.Value.info.PcName))
                            //        throw new Exception("device not found!");
                            //}

                            //.NotifyAll(
                            //new RemoteInterface.NotifyEventObject( RemoteInterface.EventEnumType.TEST,pair.Value.PcName,"TEST"));


                        }
                        catch
                        {
                            dictClientCallBacks.Remove(pair.Key);
                            //   NotifyAllConnetedChange();
                            Console.WriteLine(pair.Key + " dead , Remove!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                System.Threading.Thread.Sleep(1000 * 60);
            }
        }
        //public string Register()
        //{
        //    throw new NotImplementedException();
        //}


        public void NotifyDBChange(DBChangedConstant constant)
        {

         
            switch (constant)
            {
                case DBChangedConstant.AuthorityChanged:
                    Console.WriteLine("notify db Change!");
                    SecureDBEntities1 db = new SecureDBEntities1();
                    foreach (tblCardCommandLog cmdlog in db.tblCardCommandLog.Where(n=>n.Timestamp==null))
                    {
                        try
                        {
                            if (cmdlog.ControlID!="*" && !card_mgr[cmdlog.ControlID].IsConnected )
                                continue;
                            if (cmdlog.CommandType == "I")
                            {
                                if (cmdlog.CardType == "C")
                                {
                                    Console.WriteLine(cmdlog.ControlID+" Add card!"+cmdlog.ABA);
                                    card_mgr[cmdlog.ControlID].AddCard(cmdlog.ABA);
                                    
                                }
                                else
                                {
                                    Console.WriteLine(cmdlog.ControlID + " Add virtual card!" + cmdlog.ABA);
                                    card_mgr[cmdlog.ControlID].AddVirturalCard(cmdlog.ABA);

                                }

                            }
                            else if (cmdlog.CommandType == "D")
                            {
                                Console.WriteLine(cmdlog.ControlID + " delete  card!" + cmdlog.ABA);
                                card_mgr[cmdlog.ControlID].DeleteCard(cmdlog.ABA);
                            }
                            else if(cmdlog.CommandType=="*")
                            {
                                Console.WriteLine("Process CheckCardDue");
                                CheckCardDueTask();
                            }

                            cmdlog.Timestamp=DateTime.Now;
                            cmdlog.IsSuccess= true;
                        }
                        catch (Exception ex)
                        {
                            cmdlog.Timestamp = DateTime.Now;
                            Console.WriteLine(ex.Message + "," + ex.StackTrace);
                            cmdlog.IsSuccess = false;
                            //throw ex;
                           
                        }
                    }
                    db.SaveChanges();
                    break;
            }

        }


        public void ToServerHello()
        {
            
        }


        public void UnRegist(string guid)
        {
            this.dictClientCallBacks.Remove(guid);
        }


        public void ForceOpenDoor(string ControllID)
        {
            card_mgr[ControllID].ForceOpenDoor( );
        }


        public void HookCardReaderEvent(string key, int PlaneId)
        {
            try
            {
                if (!dictClientCallBacks.ContainsKey(key))
                    throw new Exception("Key not found!");
                RegisterInfo info = dictClientCallBacks[key];
                info.PlaneID = PlaneId;
                info.IsRegistDoorEvent = true;
            }
            catch { ;}

        }


        public BindingData.DoorBindingData[] GetALLDoorBindingData(int PlaneID)
        {
            return this.card_mgr.GetAllDoorBindingData(PlaneID);
        }


        public void HookAlarmEvent(string key)
        {
            try
            {
                if (!dictClientCallBacks.ContainsKey(key))
                    throw new Exception("Key not found!");
                RegisterInfo info = dictClientCallBacks[key];
                info.IsRegistAlarm = true;
            }
            catch { ;}
           
        }


        public void DispatchAlarmEvent(AlarmData alarmdata)
        {
            try
            {
                foreach (RegisterInfo info in dictClientCallBacks.Values.ToArray())
                {
                    if (info.IsRegistAlarm)
                        info.CallBack.SecureAlarm(alarmdata);
                }
            }
            catch { ;}
        }


        public BindingData.CCTVBindingData[] GetAllCCTVBindingData(int PlaneID)
        {
            return this.cctv_mgr.GetAllCCTVBindingData(PlaneID);
        }
    }
}

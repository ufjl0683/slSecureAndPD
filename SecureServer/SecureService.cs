using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace SecureServer
{
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.Single)] 
   public  class SecureService:ISecureService
    {

        public System.Collections.Generic.Dictionary<string, RegisterInfo> dictClientCallBacks = new Dictionary<string, RegisterInfo>();
        public CardReaderManager card_mgr  ;



        public  SecureService()
        {
            card_mgr = new CardReaderManager();
            new System.Threading.Thread(CheckConnectionTask).Start();
         
        }
        public string Register(string PCName)  //return key
        {

            string GUID=Guid.NewGuid().ToString();
            RegisterInfo info = new RegisterInfo(){ PCName=PCName, CallBack= OperationContext.Current.GetCallbackChannel<ISecureServiceCallBack>(),
                 Key=GUID};

            dictClientCallBacks.Add(GUID, info);
            Console.WriteLine(PCName + ",registed!");

            return GUID;
          //  throw new NotImplementedException();
        }


        void CheckConnectionTask()
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
                            if (!card_mgr[cmdlog.ControlID].IsConnected)
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
    }
}

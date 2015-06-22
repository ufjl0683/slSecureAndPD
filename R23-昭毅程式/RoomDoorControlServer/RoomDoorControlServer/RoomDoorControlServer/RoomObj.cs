using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoomDoorControlServer
{
    class RoomObj : MarshalByRefObject,RoomInterface.IRoom
    {
        public event RoomInterface.RoomEventHandler RoomEvent;
        int[] GroupProgress = new int[] { 0 };
        string[] ErrorMessage = new string[] { "" , ""};
        int disConnectCount = 0;
        static GroupMessage groupMessage = new GroupMessage();
        static System.Threading.AutoResetEvent wait = new System.Threading.AutoResetEvent(false);

        RoomObj()
        {            
            ServerData.RoomEvent += new RoomInterface.RoomEventHandler(roomEvent_RoomEvent);
        }

        void roomEvent_RoomEvent(RoomInterface.ControllEventType type, string Name, object obj)
        {
            if (RoomEvent != null)
            {
                Delegate[] DelObjs = RoomEvent.GetInvocationList();
                foreach (Delegate delObj in DelObjs)
                {
                    List<object> objList = new List<object>();
                    objList.Add(delObj);
                    objList.Add(type);
                    objList.Add(Name);
                    objList.Add(obj);
                    System.Threading.ThreadPool.QueueUserWorkItem(ThreadSendEvent, objList);
                }   
            }
            
        }


        void ThreadSendEvent(object obj)
        {
            List<object> objList = (List<object>)obj;
            RoomInterface.RoomEventHandler handler = null;

            handler = (RoomInterface.RoomEventHandler)objList[0];
            try
            {
                RoomInterface.ControllEventType type = (RoomInterface.ControllEventType)objList[1];
                string Name = (string)objList[2];
                object sendObj = objList[3];
                lock (handler)
                {
                    handler(type, Name, sendObj);
                }
                disConnectCount = 0;
            }
            catch (Exception ex)
            {
                try
                {
                    string ErrorType = string.Empty;

                    try
                    {
                        RoomInterface.ControllEventType type = (RoomInterface.ControllEventType)objList[1];
                        ErrorType = type.ToString();
                    }
                    catch
                    {
                    }
                    System.IO.File.AppendAllText(@".\SendEventFail.log", DateTime.Now.ToString() + " Send event fail.Event Type : " + ErrorType + "\r\n" + ex.Message + "\r\n");
                }
                catch
                {
                }
                if (disConnectCount > 3)
                {
                    RoomEvent -= handler;
                    ServerData.RoomEvent -= new RoomInterface.RoomEventHandler(roomEvent_RoomEvent);
                }
                disConnectCount++;                
            }
        }

        public override object InitializeLifetimeService()
        {
            //System.Runtime.Remoting.Lifetime.ILease lease = (System.Runtime.Remoting.Lifetime.ILease)base.InitializeLifetimeService();
            //if (lease.CurrentState == System.Runtime.Remoting.Lifetime.LeaseState.Initial)
            //{
            //    lease.InitialLeaseTime = TimeSpan.FromMinutes(1);
            //    lease.SponsorshipTimeout = TimeSpan.FromMinutes(2);
            //    lease.RenewOnCallTime = TimeSpan.FromMinutes(1);
            //}
            //return lease;
            return null;
        }

        public bool OpenDoor(string ControlID, int DoorNum,string UserID)
        {
            bool ok;
            try
            {
                if (ServerData.ADAMController.ContainsKey(ControlID))
                {
                    ADAMControl control = ServerData.ADAMController[ControlID];
                    ok = control.OpenDoor(DoorNum);
                    if (ok && DoorNum == 0)
                    {
                        string RoomID = ControlID.Split('-')[0];
                        if (ServerData.RoomPerson.ContainsKey(RoomID))
                        {
                            try
                            {
                                dbroomEntities dbroom = new dbroomEntities();
                                string UserName = (from o in dbroom.tblUser where o.UserID == UserID select o.UserName).First();
                                ServerData.RoomPerson[RoomID].Add(new RoomCardData(UserID,UserName,"交控操作人員",true,DateTime.Now,true));
                                List<RoomCardData> RoomPersonList = ServerData.RoomPerson[RoomID];
                                List<RoomInterface.PersonData> PersonList = new List<RoomInterface.PersonData>();
                                lock (RoomPersonList)
                                {
                                    foreach (RoomCardData data in RoomPersonList)
                                    {
                                        if (data.In)
                                        {
                                            PersonList.Add(new RoomInterface.PersonData(data.CardID, data.Name,data.Company, data.LastTime, data.IsManual));
                                        }
                                    }
                                }
                                ServerData.SendRoomEvent(RoomInterface.ControllEventType.RoomPersonChange, RoomID, PersonList);
                            }
                            catch
                            { ; }                        
                        }
                    }
                }
                else
                    ok = false;
            }
            catch
            {
                ok = false;
            }
            string cmd = string.Format("Insert into tbldeviceStateLog (TypeID,TypeCode,TimeStamp,ControlID,SingalName) "
                + "Values({0},{1},'{2}','{3}','{4}');", 6, ok ? 0 : 1, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), ControlID,"DO-" + DoorNum);
            DatabaseAccess.DatabaseAcces(cmd);
            return ok;
        }

        public bool CheckConnect()
        {
            return true;
        }

        public byte[] GetADAMStatus(string ControlID)
        {
            if (ServerData.ADAMStatus.ContainsKey(ControlID))
            {
                return ServerData.ADAMStatus[ControlID];
            }
            else
                return new byte[33] ;
        }



        public List<RoomInterface.PersonData> GetRoomPerson(string RoomName)
        {
            if (ServerData.RoomPerson.ContainsKey(RoomName))
            {
                List<RoomInterface.PersonData> PersonList = new List<RoomInterface.PersonData>();
                List<RoomCardData> CardList = ServerData.RoomPerson[RoomName];
                lock (CardList)
                {
                    foreach (RoomCardData card in CardList)
                    {
                        if (card.In)
                        {
                            PersonList.Add(new RoomInterface.PersonData(card.CardID, card.Name,card.Company,card.LastTime,card.IsManual));
                        }
                    }
                }
                return PersonList;
            }
            else
                return null;
        }

        public RoomInterface.ControlStatus CheckControlConnect(string ControlID)
        {
            if (ServerData.ControlStatus.ContainsKey(ControlID))
            {
                return ServerData.ControlStatus[ControlID];
            }
            else
                return new RoomInterface.ControlStatus(ControlID,false,DateTime.Now);
        }

        public bool ResteADAMControl(string RoomID)
        {
            if (ServerData.RoomControl.ContainsKey(RoomID))
            {
                if (ServerData.ADAMController.ContainsKey(ServerData.RoomControl[RoomID]))
                {
                    bool ok = ServerData.ADAMController[ServerData.RoomControl[RoomID]].Reset();
                    if (ok)
                    {
                        lock (ServerData.RoomPerson[RoomID])
                        {
                            foreach (var Person in ServerData.RoomPerson[RoomID])
                            {
                                if (!Person.IsManual)
                                {
                                    string cmd = string.Format("update tblEngineRoomLog Set EndTime = '{1}' , Result = 1 "
                                                                        + "where ERNo = '{0}' and ABA = '{2}' "
                                                                        + "and StartTime = (Select Top 1 StartTime from tblEngineRoomLog where ERNo = '{0}' and ABA = '{2}' order by StartTime desc);"
                                                                        , RoomID, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), Person.CardID);
                                    DatabaseAccess.DatabaseAcces(cmd);
                                }
                            }
                            ServerData.RoomPerson[RoomID].Clear();
                        }
                        ServerData.SendRoomEvent(RoomInterface.ControllEventType.RoomPersonChange, RoomID, new List<RoomInterface.PersonData>());
                    }
                    return ok;
                }
                else
                    return false;
            }
            else if (RoomID == "AB" || RoomID == "AG")
            {
                lock (ServerData.RoomPerson[RoomID])
                {
                    foreach (var Person in ServerData.RoomPerson[RoomID])
                    {
                        if (!Person.IsManual)
                        {
                            string cmd = string.Format("update tblEngineRoomLog Set EndTime = '{1}' , Result = 1 "
                                                                + "where ERNo = '{0}' and ABA = '{2}' "
                                                                + "and StartTime = (Select Top 1 StartTime from tblEngineRoomLog where ERNo = '{0}' and ABA = '{2}' order by StartTime desc);"
                                                                , RoomID, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), Person.CardID);
                            DatabaseAccess.DatabaseAcces(cmd);
                        }
                    }
                    ServerData.RoomPerson[RoomID].Clear();
                }
                ServerData.SendRoomEvent(RoomInterface.ControllEventType.RoomPersonChange, RoomID, new List<RoomInterface.PersonData>());
                return true;
            }
            else
                return false;
        }
        
        public void GroupModify(List<int> GroupIDs)
        {            
            GroupProgress[0] = 0;
            ErrorMessage[0] = string.Empty;
            ErrorMessage[1] = string.Empty;
            System.Threading.Thread t = new System.Threading.Thread(setGroup);            
            groupMessage.GroupIDs = GroupIDs;
            groupMessage.GroupProgress = GroupProgress;
            groupMessage.errorMessage = ErrorMessage;
            t.Start(groupMessage);
        }

        internal static void setGroup(object obj)
        {
            GroupMessage tmpGroupMessage = (GroupMessage)obj;
            List<int> GroupIDs = tmpGroupMessage.GroupIDs;
            groupMessage.GroupProgress[0] = 0;
            groupMessage.errorMessage[0] = groupMessage.errorMessage[1] = "";
       
            lock (ServerData.RAC2400Controller)
            {
                int Progress = 0;
                string ErrorMessage = string.Empty;
                List<string> offLineControlList = new List<string>();
                //check Group Change
                dbroomEntities dbroom = new dbroomEntities();
                foreach (int groupID in GroupIDs)
                {
                    var ControlIDs = from o in dbroom.tblSysRoleAuthority where o.RoleID == groupID && o.Enable == "Y" select o.ControlID;
                    List<string> newControlID = ControlIDs.ToList();
                    if (!ServerData.GroupDataDic.ContainsKey(groupID)) //新增群組
                    {
                        ServerData.GroupDataDic.Add(groupID, new GroupData(groupID));
                        ServerData.GroupCardDic.Add(groupID, new List<GroupCardData>());
                        var NewGroupCards = from o in dbroom.tblMagneticCard where o.RoleID == groupID select o;
                        foreach (var newGroupCard in NewGroupCards)
                        {
                            ServerData.GroupCardDic[groupID].Add(new GroupCardData(newGroupCard.ABA, newGroupCard.StartDate, newGroupCard.EndDate, groupID));
                        }
                        foreach (string GroupData in ControlIDs)
                        {
                            ServerData.GroupDataDic[groupID].ControlID.Add(GroupData);
                        }
                    }

                    GroupData groupdata = ServerData.GroupDataDic[groupID];
                    List<string> addList = TCommon.GetAdderList(groupdata.ControlID, newControlID);
                    List<string> delList = TCommon.GetAdderList(newControlID, groupdata.ControlID);

                    if (!ServerData.GroupCardDic.ContainsKey(groupID))
                    {
                        ServerData.GroupCardDic.Add(groupID, new List<GroupCardData>());
                    }
                    List<GroupCardData> cardList = ServerData.GroupCardDic[groupID];


                    groupMessage.errorMessage[1] += "確認群組控制器名單\r\n";
                    #region 群組移除控制器
                    foreach (string del in delList)
                    {
                        try
                        {
                            if (offLineControlList.Contains(del)) //斷線控制器不動作
                                continue;
                            groupMessage.errorMessage[1] += "群組移除控制器" + ServerData.ControlName[del] + "\r\n";
                            CardControl rac2400 = null;
                            RAC960CardControl rac960 = null;
                            if (ServerData.RAC2400Controller.ContainsKey(del))
                            {
                                rac2400 = ServerData.RAC2400Controller[del];
                            }
                            else if (ServerData.RAC960Controller.ContainsKey(del))
                            {
                                rac960 = ServerData.RAC960Controller[del];
                            }
                            if (ServerData.ControlCardDic.ContainsKey(del))
                            {
                                List<string> ControlCardList = ServerData.ControlCardDic[del];
                                foreach (GroupCardData newCard in cardList)
                                {
                                    try
                                    {
                                        if (ControlCardList.Contains(newCard.ABA))
                                        {
                                            bool? ok = null;
                                            if (rac2400 != null)
                                            {
                                                ok = rac2400.DelUserData(newCard.ABA);
                                                if (ok == true)
                                                    ok = rac2400.DelUserData(TCommon.GetWEG(newCard.ABA));
                                            }
                                            else if (rac960 != null)
                                                ok = rac960.DelUserData(newCard.ABA);
                                            if (!ok.HasValue)
                                            {
                                                offLineControlList.Add(del);
                                                groupMessage.errorMessage[1] += "控制器" + ServerData.ControlName[del] + "斷線\r\n";
                                                continue;
                                            }
                                            else if (!ok.Value)
                                            {
                                                ErrorMessage += newCard.ABA + " 在" + ServerData.ControlName[del] + " 刪除失敗\r\n";
                                                groupMessage.errorMessage[1] += ServerData.ControlName[del] + "刪除卡號" + newCard.ABA + "失敗\r\n";
                                            }
                                            else
                                            {
                                                if (rac960 != null)
                                                {
                                                    string cmd = string.Format("Delete from tblControllerCard where ControlID = '{0}' and ABA = '{1}';", del, newCard.ABA);
                                                    DatabaseAccess.DatabaseAcces(cmd);
                                                }
                                                ControlCardList.Remove(newCard.ABA);
                                                groupMessage.errorMessage[1] += ServerData.ControlName[del] + "刪除卡號" + newCard.ABA + "成功\r\n";
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        System.IO.File.AppendAllText(@".\Error.log", ex.Message + "\r\n" + ex.StackTrace);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            System.IO.File.AppendAllText(@".\Error.log", ex.Message + "\r\n" + ex.StackTrace);
                        }
                    }
                    groupMessage.GroupProgress[0] += 100 / (GroupIDs.Count * 3);
                    #endregion
                    #region 群組新增控制器
                    foreach (string add in addList)
                    {
                        if (offLineControlList.Contains(add)) //斷線控制器不動作
                            continue;
                        groupMessage.errorMessage[1] += "群組增加控制器" + ServerData.ControlName[add] + "\r\n";
                        CardControl rac2400 = null;
                        RAC960CardControl rac960 = null;                      
                        if (ServerData.RAC2400Controller.ContainsKey(add))
                        {
                            rac2400 = ServerData.RAC2400Controller[add];
                        }
                        else if (ServerData.RAC960Controller.ContainsKey(add))
                        {
                            rac960 = ServerData.RAC960Controller[add];
                        }
                        if (ServerData.ControlCardDic.ContainsKey(add))//
                        {
                            List<string> ControlCardList = ServerData.ControlCardDic[add];
                            foreach (GroupCardData newCard in cardList)
                            {
                                try
                                {
                                    if (DateTime.Now > newCard.StartDate && DateTime.Now < newCard.EndDate)
                                    {
                                        if (!ControlCardList.Contains(newCard.ABA))
                                        {
                                            bool? ok = null;
                                            if (rac2400 != null)
                                            {
                                                ok = rac2400.AddUserData(newCard.ABA, ServerData.GetCardName(newCard.ABA), "1111");
                                                if (ok == true)
                                                    ok = rac2400.AddUserData(TCommon.GetWEG(newCard.ABA), ServerData.GetCardName(newCard.ABA), "1111");
                                            }
                                            else if (rac960 != null)
                                                ok = rac960.AddUserData(newCard.ABA, ServerData.GetCardName(newCard.ABA));
                                            if (!ok.HasValue)
                                            {
                                                //offLineControlList.Add(add);
                                                //groupMessage.errorMessage[1] += "控制器" +ServerData.ControlName[add] + "斷線\r\n";
                                                //continue;
                                                ErrorMessage += newCard.ABA + " 在" + ServerData.ControlName[add] + " 新增失敗\r\n";
                                                groupMessage.errorMessage[1] += ServerData.ControlName[add] + "新增卡號" + newCard.ABA + "失敗\r\n";
                                            }
                                            else if (!ok.Value)
                                            {
                                                ErrorMessage += newCard.ABA + " 在" + ServerData.ControlName[add] + " 新增失敗\r\n";
                                                groupMessage.errorMessage[1] += ServerData.ControlName[add] + "新增卡號" + newCard.ABA + "失敗\r\n";
                                            }
                                            else
                                            {
                                                if (rac960 != null)
                                                {
                                                    string cmd = string.Format("Insert Into tblControllerCard (ControlID,ABA) Values('{0}','{1}');", add, newCard.ABA);
                                                    DatabaseAccess.DatabaseAcces(cmd);
                                                }
                                                ControlCardList.Add(newCard.ABA);
                                                groupMessage.errorMessage[1] += ServerData.ControlName[add] + "新增卡號" + newCard.ABA + "成功\r\n";
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    System.IO.File.AppendAllText(@".\Error.log", ex.Message + "\r\n" + ex.StackTrace);
                                }
                            }                  
                        }
                        else
                        {
                            ServerData.ControlCardDic.Add(add, new List<string>());
                            foreach (GroupCardData newCard in cardList)
                            {
                                try
                                {
                                    if (DateTime.Now > newCard.StartDate && DateTime.Now < newCard.EndDate)
                                    {
                                        bool? ok = null;
                                        if (rac2400 != null)
                                        {
                                            ok = rac2400.AddUserData(newCard.ABA, ServerData.GetCardName(newCard.ABA), "1111");
                                            if (ok == true)
                                                ok = rac2400.AddUserData(TCommon.GetWEG(newCard.ABA), ServerData.GetCardName(newCard.ABA), "1111");
                                        }
                                        else if (rac960 != null)
                                            ok = rac960.AddUserData(newCard.ABA, ServerData.GetCardName(newCard.ABA));
                                        if (!ok.HasValue)
                                        {
                                            //offLineControlList.Add(add);
                                            //groupMessage.errorMessage[1] += "控制器" + ServerData.ControlName[add] + "斷線\r\n";
                                            //continue;
                                            groupMessage.errorMessage[1] += ServerData.ControlName[add] + "新增卡號" + newCard.ABA + "失敗\r\n";
                                            ErrorMessage += newCard.ABA + " 在" + ServerData.ControlName[add] + " 新增失敗\r\n";
                                        }
                                        else if (!ok.Value)
                                        {
                                            groupMessage.errorMessage[1] += ServerData.ControlName[add] + "新增卡號" + newCard.ABA + "失敗\r\n";
                                            ErrorMessage += newCard.ABA + " 在" + ServerData.ControlName[add] + " 新增失敗\r\n";
                                        }
                                        else
                                        {
                                            if (rac960 != null)
                                            {
                                                string cmd = string.Format("Insert Into tblControllerCard (ControlID,ABA) Values('{0}','{1}');", add, newCard.ABA);
                                                DatabaseAccess.DatabaseAcces(cmd);
                                            }
                                            groupMessage.errorMessage[1] += ServerData.ControlName[add] + "新增卡號" + newCard.ABA + "成功\r\n";
                                            ServerData.ControlCardDic[add].Add(newCard.ABA);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    System.IO.File.AppendAllText(@".\Error.log", ex.Message + "\r\n" + ex.StackTrace);
                                }
                            }
                        }
                    }
                    groupMessage.GroupProgress[0] += 100 / (GroupIDs.Count * 3);
                    #endregion
                   
                    groupdata.ControlID = newControlID;

                    //群組內卡片變更
                    var GroupCards = from o in dbroom.tblMagneticCard where o.RoleID == groupID && o.Enable == "Y" select o;
                    if (!ServerData.GroupCardDic.ContainsKey(groupID))
                    {
                        ServerData.GroupCardDic.Add(groupID, new List<GroupCardData>());
                    }
                    List<GroupCardData> groupCard = ServerData.GroupCardDic[groupID];
                    lock (groupCard)
                    {
                        groupCard.Clear();
                        foreach (var newGroupCard in GroupCards)
                        {
                            groupCard.Add(new GroupCardData(newGroupCard.ABA, newGroupCard.StartDate, newGroupCard.EndDate, groupID));
                        }
                    }
                   
                    foreach (string controlID in ServerData.GroupDataDic[groupID].ControlID)
                    {
                        try
                        {                                                      
                            int ControlCooect = 0;
                            if (offLineControlList.Contains(controlID))
                            {
                                groupMessage.GroupProgress[0] += 100 / (GroupIDs.Count * 3 * ServerData.GroupDataDic[groupID].ControlID.Count);
                                continue;
                            }
                            List<string> NewControlCards = new List<string>();
                            var NewDataControlCards = from role in dbroom.tblSysRoleAuthority
                                                      join Magnetic in dbroom.tblMagneticCard on role.RoleID equals Magnetic.RoleID
                                                      where role.ControlID == controlID && role.Enable == "Y" && Magnetic.Enable == "Y"
                                                      select new { ABA = Magnetic.ABA, StartDate = Magnetic.StartDate, EndDate = Magnetic.Enable };

                            foreach (var Card in NewDataControlCards)
                            {
                                if (DateTime.Now >= Card.StartDate && DateTime.Now <= Convert.ToDateTime(Card.EndDate))
                                {
                                    NewControlCards.Add(Card.ABA);
                                }
                            }
                            CardControl rac2400 = null;
                            RAC960CardControl rac960 = null;
                            if (ServerData.RAC2400Controller.ContainsKey(controlID))
                            {
                                rac2400 = ServerData.RAC2400Controller[controlID];
                            }
                            else if (ServerData.RAC960Controller.ContainsKey(controlID))
                            {
                                rac960 = ServerData.RAC960Controller[controlID];
                            }
                            List<string> ControlCards = ServerData.ControlCardDic[controlID];

                            List<string> AddCardList = TCommon.GetAdderList(ControlCards, NewControlCards);
                            List<string> DelCardList = TCommon.GetAdderList(NewControlCards, ControlCards);
                            //if (rac2400 != null && AddCardList.Count > 5)
                            //{
                            //    string ListErrorMessage = rac2400.AddUserData(AddCardList);
                            //    if (string.IsNullOrWhiteSpace(ListErrorMessage))
                            //    {
                            //        ControlCards.AddRange(AddCardList);
                            //    }
                            //    groupMessage.errorMessage[1] += ListErrorMessage;
                            //}
                            //else
                            //{
                            foreach (string addCard in AddCardList)
                            {
                                try
                                {
                                    bool? ok = null;
                                    if (rac2400 != null)
                                    {
                                        ok = rac2400.AddUserData(addCard, ServerData.GetCardName(addCard), "1111");
                                        if (!ok.HasValue || !ok.Value)
                                        {
                                            wait.WaitOne(3000);
                                            ok = rac2400.AddUserData(addCard, ServerData.GetCardName(addCard), "1111");
                                        }
                                        if (ok == true)
                                        {
                                            ok = rac2400.AddUserData(TCommon.GetWEG(addCard), ServerData.GetCardName(addCard), "1111");
                                            if (!ok.HasValue || !ok.Value)
                                            {
                                                wait.WaitOne(3000);
                                                ok = rac2400.AddUserData(TCommon.GetWEG(addCard), ServerData.GetCardName(addCard), "1111");
                                            }
                                        }
                                    }
                                    else if (rac960 != null)
                                    {
                                        ok = rac960.AddUserData(addCard, ServerData.GetCardName(addCard));
                                        if (!ok.HasValue || !ok.Value)
                                        {
                                            wait.WaitOne(3000);
                                            ok = rac960.AddUserData(addCard, ServerData.GetCardName(addCard));
                                        }
                                    }
                                    if (ok.HasValue == false)
                                    {
                                        ////offLineControlList.Add(controlID);
                                        //groupMessage.errorMessage[1] += "控制器" + ServerData.ControlName[controlID] + "斷線\r\n";
                                        if (ControlCooect < 10)
                                        {
                                            groupMessage.errorMessage[1] += ServerData.ControlName[controlID] + "新增卡號" + addCard + "失敗\r\n";
                                            ErrorMessage += addCard + " 在" + ServerData.ControlName[controlID] + " 新增失敗\r\n";
                                            ControlCooect++;
                                            TCommon.SaveLog(addCard + " 在" + ServerData.ControlName[controlID] + " 新增失敗\r\n");
                                        }
                                        else
                                        {
                                            offLineControlList.Add(controlID);
                                            groupMessage.errorMessage[1] += "控制器" + ServerData.ControlName[controlID] + "斷線\r\n";
                                            TCommon.SaveLog("控制器" + ServerData.ControlName[controlID] + "斷線\r\n");
                                            break;
                                        }
                                    }
                                    else if (ok.Value == false)
                                    {
                                        groupMessage.errorMessage[1] += ServerData.ControlName[controlID] + "新增卡號" + addCard + "失敗\r\n";
                                        ErrorMessage += addCard + " 在" + ServerData.ControlName[controlID] + " 新增失敗\r\n";
                                        TCommon.SaveLog(addCard + " 在" + ServerData.ControlName[controlID] + " 新增失敗\r\n");
                                    }
                                    else
                                    {
                                        if (rac960 != null)
                                        {
                                            string cmd = string.Format("Insert Into tblControllerCard (ControlID,ABA) Values('{0}','{1}');", controlID, addCard);
                                            DatabaseAccess.DatabaseAcces(cmd);
                                        }
                                        groupMessage.errorMessage[1] += ServerData.ControlName[controlID] + "新增卡號" + addCard + "成功\r\n";
                                        TCommon.SaveLog(ServerData.ControlName[controlID] + "新增卡號" + addCard + "成功\r\n");
                                        ControlCards.Add(addCard);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    System.IO.File.AppendAllText(@".\Error.log", ex.Message + "\r\n" + ex.StackTrace);
                                }
                            }
                            //}

                            foreach (string delCard in DelCardList)
                            {
                                try
                                {
                                    bool? ok = null;
                                    if (rac2400 != null)
                                    {
                                        ok = rac2400.DelUserData(delCard);
                                        if (!ok.HasValue || !ok.Value)
                                        {
                                            wait.WaitOne(3000);
                                            ok = rac2400.DelUserData(delCard);
                                        }
                                        if (ok == true)
                                        {
                                            ok = rac2400.DelUserData(TCommon.GetWEG(delCard));
                                            if (!ok.HasValue || !ok.Value)
                                            {
                                                wait.WaitOne(3000);
                                                ok = rac2400.DelUserData(TCommon.GetWEG(delCard));
                                            }
                                        }
                                    }
                                    else if (rac960 != null)
                                    {
                                        ok = rac960.DelUserData(delCard);
                                        if (!ok.HasValue || !ok.Value)
                                        {
                                            ok = rac960.DelUserData(delCard);
                                        }
                                    }
                                    if (ok.HasValue == false)
                                    {
                                        if (ControlCooect < 10)
                                        {
                                            groupMessage.errorMessage[1] += ServerData.ControlName[controlID] + "刪除卡號" + delCard + "失敗\r\n";
                                            ErrorMessage += delCard + " 在" + ServerData.ControlName[controlID] + " 刪除失敗\r\n";
                                            ControlCooect++;
                                        }                                       
                                        else
                                        {
                                            offLineControlList.Add(controlID);
                                            groupMessage.errorMessage[1] += "控制器" + ServerData.ControlName[controlID] + "斷線\r\n";
                                            break;
                                        }

                                    }
                                    else if (ok.Value == false)
                                    {
                                        groupMessage.errorMessage[1] += ServerData.ControlName[controlID] + "刪除卡號" + delCard + "失敗\r\n";
                                        ErrorMessage += delCard + " 在" + ServerData.ControlName[controlID] + " 刪除失敗\r\n";
                                    }
                                    else
                                    {
                                        if (rac960 != null)
                                        {
                                            string cmd = string.Format("Delete from tblControllerCard where ControlID = '{0}' and ABA = '{1}';", controlID, delCard);
                                            DatabaseAccess.DatabaseAcces(cmd);
                                        }
                                        groupMessage.errorMessage[1] += ServerData.ControlName[controlID] + "刪除卡號" + delCard + "成功\r\n";
                                        ControlCards.Remove(delCard);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    System.IO.File.AppendAllText(@".\Error.log", ex.Message + "\r\n" + ex.StackTrace);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            System.IO.File.AppendAllText(@".\Error.log", ex.Message + "\r\n" + ex.StackTrace);
                        }
                        groupMessage.GroupProgress[0] += 100 / (GroupIDs.Count * 3 * ServerData.GroupDataDic[groupID].ControlID.Count);
                    }
                    groupMessage.GroupProgress[0] = (Progress++ * 100) / GroupIDs.Count;
                }

                foreach (string offLint in offLineControlList)
                {
                    string controlName = offLint;
                    if (ServerData.ControlName.ContainsKey(offLint))
                    {
                        controlName = ServerData.ControlName[offLint];
                    }
                    ErrorMessage = controlName + "斷線\r\n" + ErrorMessage;
                }

                System.IO.File.AppendAllText(@".\GroupSet.Log", DateTime.Now + "\r\n" + ErrorMessage);
                groupMessage.errorMessage[1] += "全部下載完成";
                groupMessage.GroupProgress[0] = 100;
                groupMessage.errorMessage[0] = ErrorMessage;
            }
        }

        public object[] GetGroupProgress()
        {
            object[] objs = new object[2];
            if (groupMessage != null)
            {
                objs[0] = groupMessage.GroupProgress[0];
                objs[1] = groupMessage.errorMessage[1];
            }
            else
            {
                objs[0] = 0;
                objs[1] = "";
            }
            return objs;
        }
   
        public string GetGroupErrorMessage()
        {
            return groupMessage.errorMessage[0];
        }



        public void ReloadPersonName()
        {
            ServerData.ReloadPersonName();
        }


        public bool SetADAMAlarmTime(string ControlID ,int RemoOpenTime,int DelayTime,int LoopErrorAlarmTime,int AlarmTime)
        {
            if (ServerData.ADAMController.ContainsKey(ControlID))
            {
                return ServerData.ADAMController[ControlID].SetAlarmTime(RemoOpenTime, DelayTime, LoopErrorAlarmTime, AlarmTime);
            }
            else
                return false;
        }

        public bool SetTime(string ControlID,DateTime time)
        {
            if (ServerData.RAC2400Controller.ContainsKey(ControlID))
            {
                return ServerData.RAC2400Controller[ControlID].SetTime(time);
            }
            else if (ServerData.RAC960Controller.ContainsKey(ControlID))
            {
                return ServerData.RAC960Controller[ControlID].SetTime(time);
            }
            else
                return false;
        }

    }

    class GroupMessage
    {
        public GroupMessage()
        {
            GroupIDs = new List<int>();
            GroupProgress = new int[1];
            errorMessage = new string[2];
        }
        public List<int> GroupIDs;
        public int[] GroupProgress;
        public string[] errorMessage;
    }
    
}

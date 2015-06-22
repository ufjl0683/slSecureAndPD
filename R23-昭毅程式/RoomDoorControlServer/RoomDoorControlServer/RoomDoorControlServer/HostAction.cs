using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoomDoorControlServer
{
    class HostAction
    {
        System.Timers.Timer timer;
        DateTime LastTime;

        public HostAction()
        {
            timer = new System.Timers.Timer();
            LastTime = DateTime.Now ;
            timer.Interval = 10000;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Start();
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (DateTime.Now.Day != LastTime.Day)//
                {
                 
                    LastTime = DateTime.Now;
                    foreach (var rac2400 in ServerData.RAC2400Controller)
                    {
                        rac2400.Value.SetTime();
                    }
                    foreach (var rac960 in ServerData.RAC960Controller)
                    {
                        rac960.Value.SetTime();
                    }
                    dbroomEntities dbroom = new dbroomEntities();
                    foreach (var RoomData in ServerData.RoomPerson) //刪除機房人員多餘資料
                    {
                        List<RoomCardData> RoomPersonList = RoomData.Value;
                        lock (RoomPersonList)
                        {
                            for (int i = 0; i < RoomPersonList.Count; i++)
                            {
                                if (RoomPersonList[i].In == false && RoomPersonList[i].LastTime < DateTime.Now.AddDays(-2))
                                {
                                    RoomPersonList.RemoveAt(i--);
                                }
                            }
                        }
                    }

                    var PerosnDatas = from o in dbroom.tblMagneticCard select o;//更新卡片人員對應表
                    foreach (var Person in PerosnDatas)
                    {
                        if (!ServerData.PersonnelData.ContainsKey(Person.ABA))
                        {
                            ServerData.PersonnelData.Add(Person.ABA, Person.Name == "" ? Person.Company : Person.Name);
                        }
                        else
                        {
                            if (ServerData.PersonnelData[Person.ABA] != (Person.Name == "" ? Person.Company : Person.Name))
                            {
                                ServerData.PersonnelData[Person.ABA] = Person.Name == "" ? Person.Company : Person.Name;
                            }
                        }

                    }

                    Dictionary<string, string> WegDict = new Dictionary<string, string>();
                    var wegDatas = from o in dbroom.tblMagneticCard select o;
                    foreach (var weg in wegDatas)
                    {
                        string Weg = weg.WEG1 + weg.WEG2;
                        if (!WegDict.ContainsKey(Weg))
                        {
                            WegDict.Add(Weg, weg.ABA);
                        }
                    }


                    //載入讀卡機現有卡片
                    foreach (var rac2400 in ServerData.RAC2400Controller)
                    {
                        List<CardData> NowList = rac2400.Value.ReadAllUserData();
                        if (NowList != null)
                        {
                            List<string> NewCardList = new List<string>();
                            Dictionary<string, int> NewCardDict = new Dictionary<string, int>();
                            foreach (CardData card in NowList)
                            {
                                if (!WegDict.ContainsKey(card.CarNum))
                                {
                                    if (!NewCardDict.ContainsKey(card.CarNum))
                                    {
                                        NewCardDict.Add(card.CarNum, 1);
                                    }
                                    else
                                    {
                                        NewCardDict[card.CarNum]++;
                                    }
                                }
                                else
                                {
                                    if (!NewCardDict.ContainsKey(WegDict[card.CarNum]))
                                    {
                                        NewCardDict.Add(WegDict[card.CarNum], 1);
                                    }
                                    else
                                    {
                                        NewCardDict[WegDict[card.CarNum]]++;
                                    }
                                }
                            }
                            foreach (var data in NewCardDict)
                            {
                                if (data.Value > 1)
                                {
                                    NewCardList.Add(data.Key);
                                }
                            }
                            //foreach (CardData card in NowList)
                            //{
                            //    if (!WegDict.ContainsKey(card.CarNum) )
                            //    {
                            //        NewCardList.Add(card.CarNum);
                            //    }
                            //}                            
                            if (!ServerData.ControlCardDic.ContainsKey(rac2400.Key))
                            {
                                ServerData.ControlCardDic.Add(rac2400.Key, new List<string>());
                            }
                            List<string> PersonList = ServerData.ControlCardDic[rac2400.Key];
                            lock (PersonList)
                            {
                                PersonList.Clear();
                                PersonList.AddRange(NewCardList);
                            }
                        }                     
                        //List<string> PersonList = ServerData.ControlCardDic[rac2400.Key];
                        //lock (PersonList)
                        //{
                        //    PersonList.Clear();
                        //    //PersonList.AddRange(NewCardList);
                        //}
                    }
                    foreach (var rac960 in ServerData.RAC960Controller)
                    {
                        var Rac960Cards = from o in dbroom.tblControllerCard where o.ControlID == rac960.Key select o;

                        List<string> NowList = new List<string>();
                        foreach (var rac960Card in Rac960Cards)
                        {
                            NowList.Add(rac960Card.ABA);
                        }
                        List<string> PersonList = ServerData.ControlCardDic[rac960.Key];
                        lock (PersonList)
                        {
                            PersonList.Clear();
                            PersonList.AddRange(NowList);
                        }
                        //List<string> Rac960List = rac960.Value.ReadAllUserData();
                        //if (Rac960List != null)
                        //{
                        //    List<string> NowList = new List<string>();
                        //    foreach (string rac960Card in Rac960List)
                        //    {
                        //        NowList.Add(rac960Card.Split(',')[0]);
                        //    }
                        
                        //    if (!ServerData.ControlCardDic.ContainsKey(rac960.Key))
                        //    {
                        //        ServerData.ControlCardDic.Add(rac960.Key, new List<string>());
                        //    }
                        //    List<string> PersonList = ServerData.ControlCardDic[rac960.Key];
                        //    lock (PersonList)
                        //    {
                        //        PersonList.Clear();
                        //        PersonList.AddRange(NowList);
                        //    }
                        //}
                   
                        //List<string> PersonList = ServerData.ControlCardDic[rac960.Key];
                        //lock (PersonList)
                        //{
                        //    PersonList.Clear();
                        //}
                    }                 

                    //重設讀卡機資料
                    
                    var GroupDatas = from o in dbroom.tblSysRole select o;
                    List<int> GroupIDs = new List<int>();
                    foreach (var GroupData in GroupDatas)
                    {
                        GroupIDs.Add(GroupData.RoleID);
                    }

                    int[] GroupProgress = new int[1]{0};
                    string[] errorMessage = new string[2] {"",""};
                    //object[] obj = new object[3];
                    //obj[0] = GroupIDs;
                    //obj[1] = GroupProgress;
                    //obj[2] = errorMessage;
                    GroupMessage groupMessage = new GroupMessage();
                    groupMessage.GroupIDs = GroupIDs;
                    groupMessage.GroupProgress = GroupProgress;
                    groupMessage.errorMessage = errorMessage;
                    RoomObj.setGroup(groupMessage);
                }
                LastTime = DateTime.Now;
                ServerData.SendRoomEvent(RoomInterface.ControllEventType.TimeConnect, string.Empty, null);
            }
            catch (Exception ex)
            {
                TCommon.SaveLog(ex.Message + "\r\n" + ex.StackTrace);
            }
        }      

        public void Close()
        {
            timer.Close();
        }
    }
}

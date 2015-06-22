using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoomDoorControlServer
{
    class ServerData
    {
        //internal static System.Collections.Generic.Dictionary<string, string> ControllerIP = new Dictionary<string, string>();
        internal static Dictionary<string, ADAMControl> ADAMController = new Dictionary<string, ADAMControl>();
        internal static Dictionary<string, CardControl> RAC2400Controller = new Dictionary<string, CardControl>();
        internal static Dictionary<string, RAC960CardControl> RAC960Controller = new Dictionary<string, RAC960CardControl>();
        internal static Dictionary<string, byte[]> ADAMStatus = new Dictionary<string, byte[]>();
        //internal static Dictionary<string, List<string>> RoomPersonnel = new Dictionary<string, List<string>>();

        internal static Dictionary<string, string> PersonnelData = new Dictionary<string, string>();
        internal static Dictionary<string, string> PersonnelCompData = new Dictionary<string, string>();
        internal static Dictionary<string, List<RoomCardData>> RoomPerson = new Dictionary<string, List<RoomCardData>>();
        internal static Dictionary<string, string> RoomControl = new Dictionary<string, string>();
        
        internal static Dictionary<string, RoomInterface.ControlStatus> ControlStatus = new Dictionary<string, RoomInterface.ControlStatus>();        
        internal static List<IEnd> ThreadEndList = new List<IEnd>();
        internal static event RoomInterface.RoomEventHandler RoomEvent;
        private static int tcpConnectNum = 0;
        static System.Threading.AutoResetEvent wait = new System.Threading.AutoResetEvent(false);

        internal static Dictionary<int, GroupData> GroupDataDic = new Dictionary<int, GroupData>();
        internal static Dictionary<int, List<GroupCardData>> GroupCardDic = new Dictionary<int, List<GroupCardData>>();
        internal static Dictionary<string, List<string>> ControlCardDic = new Dictionary<string, List<string>>();
        internal static Dictionary<string, string> ControlName = new Dictionary<string, string>();
        //internal static string AlarmTime;

        internal static void waitServerTCPConnect()
        {
            while (!registerServerTCPConnect())
            {
                wait.WaitOne(100);
            }
        }

        static bool registerServerTCPConnect()
        {
            lock (typeof(ServerData))
            {
                if (tcpConnectNum < 12)
                {
                    tcpConnectNum++;
                    return true;
                }
                else
                    return false;
            }        
        }

        internal static void unregisterServerTCPConnect()
        {
            tcpConnectNum--;
        }

        internal static void SendRoomEvent(RoomInterface.ControllEventType type,string Name,object obj)
        {
            if (RoomEvent != null)
            {
                RoomEvent(type, Name, obj);
            }
        }

        internal static void CheckControlConnect(string controlID, bool connect)
        {
            if (ControlStatus.ContainsKey(controlID))
            {
                if (ControlStatus[controlID].Connect != connect)
                {
                    RoomInterface.ControlStatus controlStatus = ControlStatus[controlID];
                    ControlStatus[controlID].Connect = connect;
                    ControlStatus[controlID].StatusChangeTime = DateTime.Now;
                    SendRoomEvent(RoomInterface.ControllEventType.ControlConnectStatusChange, controlID, controlStatus);
                    string cmd = string.Format("Insert into tblDeviceStateLog (TypeID,TypeCode,Timestamp,ControlID) "
                        + "Values(3,{0},'{1}','{2}');", connect ? 0 : 1, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), controlID);
                    DatabaseAccess.DatabaseAcces(cmd);
                }
            }
        }

        internal static string GetCardName(string CardID)
        {
            if (PersonnelData.ContainsKey(CardID))
                return PersonnelData[CardID];
            else
                return string.Empty;
        }

        internal static string GetCarComp(string CardID)        
        {
            if (PersonnelCompData.ContainsKey(CardID))
                return PersonnelCompData[CardID];
            else
                return string.Empty;
        }

        internal static void ReloadPersonName()
        {
            dbroomEntities dbroom = new dbroomEntities();
            var PerosnDatas = from o in dbroom.tblMagneticCard select o;//更新卡片人員對應表
            foreach (var Person in PerosnDatas)
            {
                if (!ServerData.PersonnelData.ContainsKey(Person.ABA))
                {
                    ServerData.PersonnelData.Add(Person.ABA, Person.Name);
                }
                else
                {
                    if (ServerData.PersonnelData[Person.ABA] != (Person.Name))
                    {
                        ServerData.PersonnelData[Person.ABA] = Person.Name;
                    }
                }
                if (!ServerData.PersonnelCompData.ContainsKey(Person.ABA))
                {
                    ServerData.PersonnelCompData.Add(Person.ABA, Person.Company);
                }
                else
                {
                    if (ServerData.PersonnelCompData[Person.ABA] != Person.Company)
                    {
                        ServerData.PersonnelCompData[Person.ABA] = Person.Company;
                    }
                }
            }
        }
    }

    struct RoomCardData
    {
        public RoomCardData(string cardID, string name,string company, bool IN, DateTime time,bool isManual)
        {
            CardID = cardID;
            Name = name;
            In = IN;
            LastTime = time;
            IsManual = isManual;
            Company = company;
        }

        public string Company;
        public string CardID;
        public string Name;
        public bool In;
        public bool IsManual;
        public DateTime LastTime;
    }

    struct GroupCardData
    {
        public GroupCardData(string aba, DateTime? startDate, DateTime? endDate, int groupID)
        {
            ABA = aba;
            StartDate = startDate.HasValue ? startDate.Value : DateTime.MinValue;
            EndDate = endDate.HasValue ? endDate.Value : DateTime.MaxValue;
            GroupID = groupID;            
        }

        public string ABA;
        public DateTime StartDate;
        public DateTime EndDate;
        public int GroupID;
    }

    struct GroupData
    {
        public GroupData(int groupID)
        {
            GroupID = groupID;
            ControlID = new List<string>();
        }
        public int GroupID;
        public List<string> ControlID;
    }
    
}

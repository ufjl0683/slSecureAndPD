using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoomInterface
{
    public delegate void RoomEventHandler(ControllEventType type,string Name, object obj);

    public interface IRoom 
    {
        event RoomEventHandler RoomEvent;

        bool OpenDoor(string ControlID, int DoorNum,string UserID);
        bool CheckConnect();
        byte[] GetADAMStatus(string ControlID);
        List<PersonData> GetRoomPerson(string RoomName);
        RoomInterface.ControlStatus CheckControlConnect(string ControlID);
        bool ResteADAMControl(string RoomID);
        bool SetADAMAlarmTime(string ControlID, int RemoOpenTime, int DelayTime, int LoopErrorAlarmTime, int AlarmTime);
        bool SetTime(string ControlID,DateTime time);
        void GroupModify(List<int> GroupIDs);
        object[] GetGroupProgress();
        string GetGroupErrorMessage();
        void ReloadPersonName();
    }

    public abstract class IRoomvent : MarshalByRefObject
    {      

        public override object InitializeLifetimeService()
        {
            System.Runtime.Remoting.Lifetime.ILease lease = (System.Runtime.Remoting.Lifetime.ILease)base.InitializeLifetimeService();
            if (lease.CurrentState == System.Runtime.Remoting.Lifetime.LeaseState.Initial)
            {
                lease.InitialLeaseTime = TimeSpan.FromMinutes(1);
                lease.SponsorshipTimeout = TimeSpan.FromMinutes(2);
                lease.RenewOnCallTime = TimeSpan.FromMinutes(1);
            }
            return lease;
            //return null;
        }
        public abstract void Cobj_RoomCEvent(ControllEventType type, string Name,object obj);
    }

    [Serializable]
    public struct PersonData
    {
        public PersonData(string cardNo, string name,string comp,DateTime time,bool ismanual)
        {
            CARDNO = cardNo;
            NAME = name;
            TIME = time;
            ISMANUAL = ismanual;
            COMP = comp;
        }


        string CARDNO;
        string NAME;
        DateTime TIME;
        bool ISMANUAL;
        string COMP;
        public string CardNo { get { return CARDNO; } set { CARDNO = value; } }
        public string Name { get { return NAME; } set { NAME = value; } }
        public DateTime Time { get { return TIME; } set { TIME = value; } }
        public bool IsManual { get { return ISMANUAL; } set { ISMANUAL = value; } }
        public string Comp { get { return COMP; } set { COMP = value; } }
    }

    [Serializable]
    public class ControlStatus
    {
        public ControlStatus(string CONTROLID, bool CONNECT, DateTime DISCONNECTTIME)
        {
            controlID = CONTROLID;
            connect = CONNECT;
            disconnectTime = DISCONNECTTIME;
        }

        string controlID;
        bool connect;
        DateTime disconnectTime;
        public string ControlID { get { return controlID; } set { controlID = value; } }
        public bool Connect { get { return connect; } set { connect = value; } }
        public DateTime StatusChangeTime { get { return disconnectTime; } set { disconnectTime = value; } }
    }

    public enum ADAMStatus
    {
        Normal_IN = 1,
        Normal = 2,
        DoorNotClose = 3,
        ExigentOpenDoor =4,
        IntrusionEvent =5,
    }

    public enum ControllEventType
    {
        ADAMStatusChange,   
        RoomPersonChange,
        ControlConnectStatusChange,
        ReadCard,
        TimeConnect,
        ErrorCard,
        HostDisconnect,
        HostConnect
    }
}

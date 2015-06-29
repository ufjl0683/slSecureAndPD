using System;
namespace SecureServer.CardReader
{
   public interface ICardReader
    {

        void AddCard(string cardno);
        void AddVirturalCard(string cardno);
        string ControllerID { get; set; }
        void DeleteAllCard();
        void DeleteCard(string cardno);
        int ERID { get; set; }
        void ForceOpenDoor();
        void InvokeStatusChange(CardReaderEventReport rpt);
        string IP { get; set; }
        bool IsConnected { get; set; }
        bool IsDoorOpen { get; set; }
        int NVRChNo { get; set; }
        int NVRID { get; set; }
        event AlarmEventHandler OnAlarmEvent;
        event DoorEventHandler OnDoorEvent;
        event StatusChangeHandler OnStatusChanged;
        int PlaneID { get; set; }
        void SetDateTime(DateTime dt);
        void SetOpenDoorAutoCloseTime(int sec);
        void SetOpenDoorDetectionAlarmTime(int sec);
        void SetOpenDoorTimeoutDetectionTime(int sec);

 
        void SetR23Parameter( int RemoOpenTimeR23, int DelayTimeR23,int LoopErrorAlarmTimeR23,int AlarmTimeR23);
       
        void SetSuperOpenDoorPassword(int password);
        SecureServer.BindingData.DoorBindingData ToBindingData();
        int TriggerCCTVID { get; set; }
        void WriteCardReaderID(int id);
    }
}

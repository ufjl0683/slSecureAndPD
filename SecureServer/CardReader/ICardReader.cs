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
        string IP { get; set; }
        bool IsConnected { get; set; }
        bool IsDoorOpen { get; set; }
        event AlarmEventHandler OnAlarmEvent;
        event DoorEventHandler OnDoorEvent;
        int PlaneID { get; set; }
        void SetDateTime(DateTime dt);
        void SetOpenDoorAutoCloseTime(int sec);
        void SetOpenDoorDetectionAlarmTime(int sec);
        void SetOpenDoorTimeoutDetectionTime(int sec);
        void SetSuperOpenDoorPassword(int password);
        SecureServer.CardReader.BindingData.DoorBindingData ToBindingData();
        void WriteCardReaderID(int id);
    }
}

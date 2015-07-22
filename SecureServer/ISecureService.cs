using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using SecureServer.BindingData;
using SecureServer.CardReader;
using RoomInterface;

namespace SecureServer
{
    [ServiceContract(CallbackContract=typeof(ISecureServiceCallBack))]
   public  interface ISecureService
    {
        [OperationContract]
        string  Register(string pcname);
         [OperationContract]
        void NotifyDBChange(DBChangedConstant constant,string value);
         [OperationContract]
         void ToServerHello();
         [OperationContract]
         void UnRegist(string guid);

         [OperationContract]
         void ForceOpenDoor(string ControllID);
         [OperationContract]
         void HookCardReaderEvent(string key,int PlaneId);
         [OperationContract]
         void HookAlarmEvent(string key );
         [OperationContract]
         BindingData.DoorBindingData[] GetALLDoorBindingData(int PlaneID);
         [OperationContract]
         BindingData.CCTVBindingData[] GetAllCCTVBindingData(int PlaneID);

        [OperationContract]
         void HookItemValueChangedEvent(string key, int PlaneId);

        [OperationContract]
        BindingData.ItemBindingData[] GetAllItemBindingData(int PlaneID);


        [OperationContract]
        BindingData.ItemGroupBindingData[] GetAllItemGroupBindingData(int PlaneID);


        [OperationContract]
        void SetItemDOValue(int ItemID, bool val);

        [OperationContract]
        PlaneDegreeInfo[] GetAllPlaneInfo();
                
                [OperationContract]
        byte[] GetR23ReaderStatus(string ReaderId);
                [OperationContract]
        bool SetR23Parameter(string readerid,int RemoOpenTimeR23, int DelayTimeR23, int LoopErrorAlarmTimeR23, int AlarmTimeR23);
                [OperationContract]

        bool SetR23EngineRoomRecovery(string ErNo);
                [OperationContract]
        List<PersonData> GetR23RoomPerson(string ErNo);
          [OperationContract]
                  object[] GetR23Progress();

        [OperationContract]
       string GetR23GroupErrorMessage();
        [OperationContract]
        ControlStatus GetR23ControlConnect(string ControllID);

    }


    public interface ISecureServiceCallBack
    {

        
        [OperationContract(IsOneWay = true)]
        void SayHello(string hello);
        [OperationContract(IsOneWay = true)]
        void SecureDoorEvent(DoorEventType evttype, SecureServer.BindingData.DoorBindingData doorBindingData);
        [OperationContract(IsOneWay = true)]
        void SecureAlarm(AlarmData alarmdata);
        [OperationContract(IsOneWay = true)]
        void ItemValueChangedEvenr(BindingData.ItemBindingData ItemBindingData);
        
    }

     [DataContract]
    public class SecureServiceRegistInfo
    {

    }


    public enum DBChangedConstant
    {
        AuthorityChanged,
        DoorOpenAutoCloseTime,
        DoorOpenAlarmTime,
        DoorPasswordTimeCycle,
        EventIntrusion,
        EventDoorOpenOverTime,
        EventInvalidCard,
        EventExternalForce,
        EventDoorOpen,
        ItemAttributehanged





    }
}

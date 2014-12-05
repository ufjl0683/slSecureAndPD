﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using SecureServer.CardReader.BindingData;

namespace SecureServer.CardReader
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

    }


    public interface ISecureServiceCallBack
    {

        
        [OperationContract(IsOneWay = true)]
        void SayHello(string hello);
        [OperationContract(IsOneWay = true)]
        void SecureDoorEvent(DoorEventType evttype, DoorBindingData doorBindingData);
        [OperationContract(IsOneWay = true)]
        void SecureAlarm(AlarmData alarmdata);

        
    }

     [DataContract]
    public class SecureServiceRegistInfo
    {

    }


    public enum DBChangedConstant
    {
        AuthorityChanged,
        DelayTime,
        DoorPasswordTimeCycle,
        EventIntrusion,
        EventDoorOpenOverTime,
        EventInvalidCard,
        EventExternalForce,
        EventDoorOpen





    }
}

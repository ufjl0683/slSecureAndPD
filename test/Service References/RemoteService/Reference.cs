﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//     執行階段版本:4.0.30319.34014
//
//     對這個檔案所做的變更可能會造成錯誤的行為，而且如果重新產生程式碼，
//     變更將會遺失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace test.RemoteService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DBChangedConstant", Namespace="http://schemas.datacontract.org/2004/07/SecureServer")]
    public enum DBChangedConstant : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        AuthorityChanged = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        DoorOpenAutoCloseTime = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        DoorOpenAlarmTime = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        DoorPasswordTimeCycle = 3,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        EventIntrusion = 4,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        EventDoorOpenOverTime = 5,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        EventInvalidCard = 6,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        EventExternalForce = 7,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        EventDoorOpen = 8,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DoorBindingData", Namespace="http://schemas.datacontract.org/2004/07/SecureServer.BindingData")]
    [System.SerializableAttribute()]
    public partial class DoorBindingData : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ControlIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DoorColorStringField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool IsConnectedField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool IsDoorOpenField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ControlID {
            get {
                return this.ControlIDField;
            }
            set {
                if ((object.ReferenceEquals(this.ControlIDField, value) != true)) {
                    this.ControlIDField = value;
                    this.RaisePropertyChanged("ControlID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DoorColorString {
            get {
                return this.DoorColorStringField;
            }
            set {
                if ((object.ReferenceEquals(this.DoorColorStringField, value) != true)) {
                    this.DoorColorStringField = value;
                    this.RaisePropertyChanged("DoorColorString");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsConnected {
            get {
                return this.IsConnectedField;
            }
            set {
                if ((this.IsConnectedField.Equals(value) != true)) {
                    this.IsConnectedField = value;
                    this.RaisePropertyChanged("IsConnected");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsDoorOpen {
            get {
                return this.IsDoorOpenField;
            }
            set {
                if ((this.IsDoorOpenField.Equals(value) != true)) {
                    this.IsDoorOpenField = value;
                    this.RaisePropertyChanged("IsDoorOpen");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CCTVBindingData", Namespace="http://schemas.datacontract.org/2004/07/SecureServer.BindingData")]
    [System.SerializableAttribute()]
    public partial class CCTVBindingData : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int CCTVIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CCTVNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IPField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MjpegCgiStringField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PasswordField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int PlaneIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int PortField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string UserNameField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int CCTVID {
            get {
                return this.CCTVIDField;
            }
            set {
                if ((this.CCTVIDField.Equals(value) != true)) {
                    this.CCTVIDField = value;
                    this.RaisePropertyChanged("CCTVID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CCTVName {
            get {
                return this.CCTVNameField;
            }
            set {
                if ((object.ReferenceEquals(this.CCTVNameField, value) != true)) {
                    this.CCTVNameField = value;
                    this.RaisePropertyChanged("CCTVName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string IP {
            get {
                return this.IPField;
            }
            set {
                if ((object.ReferenceEquals(this.IPField, value) != true)) {
                    this.IPField = value;
                    this.RaisePropertyChanged("IP");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string MjpegCgiString {
            get {
                return this.MjpegCgiStringField;
            }
            set {
                if ((object.ReferenceEquals(this.MjpegCgiStringField, value) != true)) {
                    this.MjpegCgiStringField = value;
                    this.RaisePropertyChanged("MjpegCgiString");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Password {
            get {
                return this.PasswordField;
            }
            set {
                if ((object.ReferenceEquals(this.PasswordField, value) != true)) {
                    this.PasswordField = value;
                    this.RaisePropertyChanged("Password");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int PlaneID {
            get {
                return this.PlaneIDField;
            }
            set {
                if ((this.PlaneIDField.Equals(value) != true)) {
                    this.PlaneIDField = value;
                    this.RaisePropertyChanged("PlaneID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Port {
            get {
                return this.PortField;
            }
            set {
                if ((this.PortField.Equals(value) != true)) {
                    this.PortField = value;
                    this.RaisePropertyChanged("Port");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string UserName {
            get {
                return this.UserNameField;
            }
            set {
                if ((object.ReferenceEquals(this.UserNameField, value) != true)) {
                    this.UserNameField = value;
                    this.RaisePropertyChanged("UserName");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DoorEventType", Namespace="http://schemas.datacontract.org/2004/07/SecureServer.CardReader")]
    public enum DoorEventType : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        DoorOpen = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        DoorClose = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Connected = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        DisConnected = 3,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="AlarmData", Namespace="http://schemas.datacontract.org/2004/07/SecureServer")]
    [System.SerializableAttribute()]
    public partial class AlarmData : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private test.RemoteService.AlarmType AlarmTypeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private test.RemoteService.CCTVBindingData CCTVBindingDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ColorStringField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DescriptionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool IsForkCCTVEventField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int PlaneIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PlaneNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime TimeStampField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TimeStampStringField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public test.RemoteService.AlarmType AlarmType {
            get {
                return this.AlarmTypeField;
            }
            set {
                if ((this.AlarmTypeField.Equals(value) != true)) {
                    this.AlarmTypeField = value;
                    this.RaisePropertyChanged("AlarmType");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public test.RemoteService.CCTVBindingData CCTVBindingData {
            get {
                return this.CCTVBindingDataField;
            }
            set {
                if ((object.ReferenceEquals(this.CCTVBindingDataField, value) != true)) {
                    this.CCTVBindingDataField = value;
                    this.RaisePropertyChanged("CCTVBindingData");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ColorString {
            get {
                return this.ColorStringField;
            }
            set {
                if ((object.ReferenceEquals(this.ColorStringField, value) != true)) {
                    this.ColorStringField = value;
                    this.RaisePropertyChanged("ColorString");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Description {
            get {
                return this.DescriptionField;
            }
            set {
                if ((object.ReferenceEquals(this.DescriptionField, value) != true)) {
                    this.DescriptionField = value;
                    this.RaisePropertyChanged("Description");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsForkCCTVEvent {
            get {
                return this.IsForkCCTVEventField;
            }
            set {
                if ((this.IsForkCCTVEventField.Equals(value) != true)) {
                    this.IsForkCCTVEventField = value;
                    this.RaisePropertyChanged("IsForkCCTVEvent");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int PlaneID {
            get {
                return this.PlaneIDField;
            }
            set {
                if ((this.PlaneIDField.Equals(value) != true)) {
                    this.PlaneIDField = value;
                    this.RaisePropertyChanged("PlaneID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PlaneName {
            get {
                return this.PlaneNameField;
            }
            set {
                if ((object.ReferenceEquals(this.PlaneNameField, value) != true)) {
                    this.PlaneNameField = value;
                    this.RaisePropertyChanged("PlaneName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime TimeStamp {
            get {
                return this.TimeStampField;
            }
            set {
                if ((this.TimeStampField.Equals(value) != true)) {
                    this.TimeStampField = value;
                    this.RaisePropertyChanged("TimeStamp");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string TimeStampString {
            get {
                return this.TimeStampStringField;
            }
            set {
                if ((object.ReferenceEquals(this.TimeStampStringField, value) != true)) {
                    this.TimeStampStringField = value;
                    this.RaisePropertyChanged("TimeStampString");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="AlarmType", Namespace="http://schemas.datacontract.org/2004/07/SecureServer")]
    public enum AlarmType : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Secure = 0,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ItemBindingData", Namespace="http://schemas.datacontract.org/2004/07/SecureServer.BindingData")]
    [System.SerializableAttribute()]
    public partial class ItemBindingData : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="RemoteService.ISecureService", CallbackContract=typeof(test.RemoteService.ISecureServiceCallback))]
    public interface ISecureService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISecureService/Register", ReplyAction="http://tempuri.org/ISecureService/RegisterResponse")]
        string Register(string pcname);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISecureService/NotifyDBChange", ReplyAction="http://tempuri.org/ISecureService/NotifyDBChangeResponse")]
        void NotifyDBChange(test.RemoteService.DBChangedConstant constant, string value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISecureService/ToServerHello", ReplyAction="http://tempuri.org/ISecureService/ToServerHelloResponse")]
        void ToServerHello();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISecureService/UnRegist", ReplyAction="http://tempuri.org/ISecureService/UnRegistResponse")]
        void UnRegist(string guid);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISecureService/ForceOpenDoor", ReplyAction="http://tempuri.org/ISecureService/ForceOpenDoorResponse")]
        void ForceOpenDoor(string ControllID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISecureService/HookCardReaderEvent", ReplyAction="http://tempuri.org/ISecureService/HookCardReaderEventResponse")]
        void HookCardReaderEvent(string key, int PlaneId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISecureService/HookAlarmEvent", ReplyAction="http://tempuri.org/ISecureService/HookAlarmEventResponse")]
        void HookAlarmEvent(string key);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISecureService/GetALLDoorBindingData", ReplyAction="http://tempuri.org/ISecureService/GetALLDoorBindingDataResponse")]
        test.RemoteService.DoorBindingData[] GetALLDoorBindingData(int PlaneID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISecureService/GetAllCCTVBindingData", ReplyAction="http://tempuri.org/ISecureService/GetAllCCTVBindingDataResponse")]
        test.RemoteService.CCTVBindingData[] GetAllCCTVBindingData(int PlaneID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISecureService/HookItemValueChangedEvent", ReplyAction="http://tempuri.org/ISecureService/HookItemValueChangedEventResponse")]
        void HookItemValueChangedEvent(string key, int PlaneId);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ISecureServiceCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/ISecureService/SayHello")]
        void SayHello(string hello);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/ISecureService/SecureDoorEvent")]
        void SecureDoorEvent(test.RemoteService.DoorEventType evttype, test.RemoteService.DoorBindingData doorBindingData);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/ISecureService/SecureAlarm")]
        void SecureAlarm(test.RemoteService.AlarmData alarmdata);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/ISecureService/ItemValueChangedEvenr")]
        void ItemValueChangedEvenr(test.RemoteService.ItemBindingData ItemBindingData);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ISecureServiceChannel : test.RemoteService.ISecureService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SecureServiceClient : System.ServiceModel.DuplexClientBase<test.RemoteService.ISecureService>, test.RemoteService.ISecureService {
        
        public SecureServiceClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public SecureServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public SecureServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public SecureServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public SecureServiceClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public string Register(string pcname) {
            return base.Channel.Register(pcname);
        }
        
        public void NotifyDBChange(test.RemoteService.DBChangedConstant constant, string value) {
            base.Channel.NotifyDBChange(constant, value);
        }
        
        public void ToServerHello() {
            base.Channel.ToServerHello();
        }
        
        public void UnRegist(string guid) {
            base.Channel.UnRegist(guid);
        }
        
        public void ForceOpenDoor(string ControllID) {
            base.Channel.ForceOpenDoor(ControllID);
        }
        
        public void HookCardReaderEvent(string key, int PlaneId) {
            base.Channel.HookCardReaderEvent(key, PlaneId);
        }
        
        public void HookAlarmEvent(string key) {
            base.Channel.HookAlarmEvent(key);
        }
        
        public test.RemoteService.DoorBindingData[] GetALLDoorBindingData(int PlaneID) {
            return base.Channel.GetALLDoorBindingData(PlaneID);
        }
        
        public test.RemoteService.CCTVBindingData[] GetAllCCTVBindingData(int PlaneID) {
            return base.Channel.GetAllCCTVBindingData(PlaneID);
        }
        
        public void HookItemValueChangedEvent(string key, int PlaneId) {
            base.Channel.HookItemValueChangedEvent(key, PlaneId);
        }
    }
}


namespace slSecure.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // MetadataTypeAttribute 會將 tblAIItem1HourLogMetadata 識別為
    // 帶有 tblAIItem1HourLog 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblAIItem1HourLog.tblAIItem1HourLogMetadata))]
    public partial class tblAIItem1HourLog
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblAIItem1HourLog 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblAIItem1HourLogMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblAIItem1HourLogMetadata()
            {
            }

            public long FlowID { get; set; }

            public int ItemID { get; set; }

            public string Memo { get; set; }

            public tblItemConfig tblItemConfig { get; set; }

            public DateTime Timestamp { get; set; }

            public double Value { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblAlarmLogMetadata 識別為
    // 帶有 tblAlarmLog 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblAlarmLog.tblAlarmLogMetadata))]
    public partial class tblAlarmLog
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblAlarmLog 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblAlarmLogMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblAlarmLogMetadata()
            {
            }

            public string ControlID { get; set; }

            public long EventID { get; set; }

            public Nullable<int> ItemID { get; set; }

            public string Memo { get; set; }

            public tblTypeDetail tblTypeDetail { get; set; }

            public DateTime Timestamp { get; set; }

            public short TypeCode { get; set; }

            public short TypeID { get; set; }

            public Nullable<double> Value { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblCardCommandLogMetadata 識別為
    // 帶有 tblCardCommandLog 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblCardCommandLog.tblCardCommandLogMetadata))]
    public partial class tblCardCommandLog
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblCardCommandLog 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblCardCommandLogMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblCardCommandLogMetadata()
            {
            }

            public string ABA { get; set; }

            public string CardType { get; set; }

            public string CommandType { get; set; }

            public string ControlID { get; set; }

            public string Describe { get; set; }

            public long FlowID { get; set; }

            public bool IsSuccess { get; set; }

            public Nullable<DateTime> Timestamp { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblCardReaderConfigMetadata 識別為
    // 帶有 tblCardReaderConfig 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblCardReaderConfig.tblCardReaderConfigMetadata))]
    public partial class tblCardReaderConfig
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblCardReaderConfig 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblCardReaderConfigMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblCardReaderConfigMetadata()
            {
            }

            public string ControlID { get; set; }

            public string EntranceCode { get; set; }

            public int ReaderID { get; set; }

            public Nullable<short> ReaderNO { get; set; }

            public Nullable<short> ReaderType { get; set; }

            public string RoomIO { get; set; }

            public tblControllerConfig tblControllerConfig { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblCCTVConfigMetadata 識別為
    // 帶有 tblCCTVConfig 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblCCTVConfig.tblCCTVConfigMetadata))]
    public partial class tblCCTVConfig
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblCCTVConfig 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblCCTVConfigMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblCCTVConfigMetadata()
            {
            }

            public int CCTVID { get; set; }

            public string CCTVName { get; set; }

            public int ERID { get; set; }

            public string IP { get; set; }

            public Nullable<int> NVRChNO { get; set; }

            public Nullable<int> NVRID { get; set; }

            public string Password { get; set; }

            public Nullable<int> PlaneID { get; set; }

            public int Port { get; set; }

            public double Rotation { get; set; }

            public double ScaleX { get; set; }

            public double ScaleY { get; set; }

            public tblEngineRoomConfig tblEngineRoomConfig { get; set; }

            public Nullable<int> Type { get; set; }

            public string UserName { get; set; }

            public double X { get; set; }

            public double Y { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblCCTVTypeMetadata 識別為
    // 帶有 tblCCTVType 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblCCTVType.tblCCTVTypeMetadata))]
    public partial class tblCCTVType
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblCCTVType 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblCCTVTypeMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblCCTVTypeMetadata()
            {
            }

            public string CgiString { get; set; }

            public string Desription { get; set; }

            public int Type { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblControllerCardMetadata 識別為
    // 帶有 tblControllerCard 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblControllerCard.tblControllerCardMetadata))]
    public partial class tblControllerCard
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblControllerCard 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblControllerCardMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblControllerCardMetadata()
            {
            }

            public string ABA { get; set; }

            public string ControlID { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblControllerConfigMetadata 識別為
    // 帶有 tblControllerConfig 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblControllerConfig.tblControllerConfigMetadata))]
    public partial class tblControllerConfig
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblControllerConfig 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblControllerConfigMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblControllerConfigMetadata()
            {
            }

            public string ControlID { get; set; }

            public short ControlType { get; set; }

            public string DoorColorString { get; set; }

            public string EntranceCode { get; set; }

            public int ERID { get; set; }

            public string IP { get; set; }

            public Nullable<bool> IsEnable { get; set; }

            public string Loop { get; set; }

            public Nullable<int> PlaneID { get; set; }

            public int Port { get; set; }

            public Nullable<double> Rotation { get; set; }

            public Nullable<int> RTUBaseAddress { get; set; }

            public Nullable<int> RTURegisterLength { get; set; }

            public Nullable<double> ScaleX { get; set; }

            public Nullable<double> ScaleY { get; set; }

            public EntityCollection<tblCardReaderConfig> tblCardReaderConfig { get; set; }

            public tblControlSetting tblControlSetting { get; set; }

            public EntityCollection<tblDeviceStateLog> tblDeviceStateLog { get; set; }

            public tblEngineRoomConfig tblEngineRoomConfig { get; set; }

            public EntityCollection<tblItemConfig> tblItemConfig { get; set; }

            public EntityCollection<tblSingalIO> tblSingalIO { get; set; }

            public EntityCollection<tblSysRoleAuthority> tblSysRoleAuthority { get; set; }

            public Nullable<int> TriggerCCTVID { get; set; }

            public Nullable<double> X { get; set; }

            public Nullable<double> Y { get; set; }

            public Nullable<int> Comm_state { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblControlSettingMetadata 識別為
    // 帶有 tblControlSetting 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblControlSetting.tblControlSettingMetadata))]
    public partial class tblControlSetting
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblControlSetting 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblControlSettingMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblControlSettingMetadata()
            {
            }

            public Nullable<short> Alarm_Sec { get; set; }

            public string ControlID { get; set; }

            public Nullable<short> DelayTime { get; set; }

            public Nullable<short> Light_Sec { get; set; }

            public Nullable<short> LoopErrorAlarmTime { get; set; }

            public string Memo { get; set; }

            public Nullable<short> RemoOpenTime { get; set; }

            public tblControllerConfig tblControllerConfig { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblDeviceStateLogMetadata 識別為
    // 帶有 tblDeviceStateLog 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblDeviceStateLog.tblDeviceStateLogMetadata))]
    public partial class tblDeviceStateLog
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblDeviceStateLog 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblDeviceStateLogMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblDeviceStateLogMetadata()
            {
            }

            public string ABA { get; set; }

            public string ControlID { get; set; }

            public long FlowID { get; set; }

            public Nullable<int> ReaderID { get; set; }

            public string SingalName { get; set; }

            public tblControllerConfig tblControllerConfig { get; set; }

            public tblTypeDetail tblTypeDetail { get; set; }

            public DateTime TimeStamp { get; set; }

            public short TypeCode { get; set; }

            public short TypeID { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblEngineRoomConfigMetadata 識別為
    // 帶有 tblEngineRoomConfig 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblEngineRoomConfig.tblEngineRoomConfigMetadata))]
    public partial class tblEngineRoomConfig
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblEngineRoomConfig 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblEngineRoomConfigMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblEngineRoomConfigMetadata()
            {
            }

            public string CallOpenDoor { get; set; }

            public string Direction { get; set; }

            public int ERID { get; set; }

            public string ERName { get; set; }

            public string ERNo { get; set; }

            public double GPSX { get; set; }

            public double GPSY { get; set; }

            public string LineID { get; set; }

            public EntityCollection<tblCCTVConfig> tblCCTVConfig { get; set; }
             [Include]
            public EntityCollection<tblControllerConfig> tblControllerConfig { get; set; }
              [Include]
            public EntityCollection<tblEntranceGuardConfig> tblEntranceGuardConfig { get; set; }

            public EntityCollection<tblERPlane> tblERPlane { get; set; }

            public EntityCollection<tblNVRConfig> tblNVRConfig { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblEngineRoomLogMetadata 識別為
    // 帶有 tblEngineRoomLog 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblEngineRoomLog.tblEngineRoomLogMetadata))]
    public partial class tblEngineRoomLog
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblEngineRoomLog 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblEngineRoomLogMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblEngineRoomLogMetadata()
            {
            }

            public string ABA { get; set; }

            public string ControlID { get; set; }

            public Nullable<DateTime> Endtime { get; set; }

            public string ERNo { get; set; }

            public long FlowID { get; set; }

            public string Memo { get; set; }

            public string NVRFile { get; set; }

            public short Result { get; set; }

            public DateTime StartTime { get; set; }

            public Nullable<short> TypeCode { get; set; }

            public Nullable<short> TypeID { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblEntranceGuardConfigMetadata 識別為
    // 帶有 tblEntranceGuardConfig 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblEntranceGuardConfig.tblEntranceGuardConfigMetadata))]
    public partial class tblEntranceGuardConfig
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblEntranceGuardConfig 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblEntranceGuardConfigMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblEntranceGuardConfigMetadata()
            {
            }

            public string EntranceCode { get; set; }

            public string EntranceType { get; set; }

            public int ERID { get; set; }

            public Nullable<short> Floor { get; set; }

            public string Memo { get; set; }

            public tblEngineRoomConfig tblEngineRoomConfig { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblERDoorPasswordMetadata 識別為
    // 帶有 tblERDoorPassword 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblERDoorPassword.tblERDoorPasswordMetadata))]
    public partial class tblERDoorPassword
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblERDoorPassword 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblERDoorPasswordMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblERDoorPasswordMetadata()
            {
            }

            public string DoorPassword { get; set; }

            public DateTime Timestamp { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblERPlaneMetadata 識別為
    // 帶有 tblERPlane 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblERPlane.tblERPlaneMetadata))]
    public partial class tblERPlane
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblERPlane 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblERPlaneMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblERPlaneMetadata()
            {
            }

            public int ERID { get; set; }

            public string PictureDesc { get; set; }

            public int PlaneID { get; set; }

            public string PlaneName { get; set; }

            public tblEngineRoomConfig tblEngineRoomConfig { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblHostConfigMetadata 識別為
    // 帶有 tblHostConfig 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblHostConfig.tblHostConfigMetadata))]
    public partial class tblHostConfig
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblHostConfig 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblHostConfigMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblHostConfigMetadata()
            {
            }

            public string HostID { get; set; }

            public short HostType { get; set; }

            public string IP { get; set; }

            public string Memo { get; set; }

            public int Port { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblItemConfigMetadata 識別為
    // 帶有 tblItemConfig 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblItemConfig.tblItemConfigMetadata))]
    public partial class tblItemConfig
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblItemConfig 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblItemConfigMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblItemConfigMetadata()
            {
            }

            public string AbnormalRecoveryContent { get; set; }

            public string AbnormalRecoveryMode { get; set; }

            public int Address { get; set; }

            public string AlarmContent { get; set; }

            public Nullable<double> AlarmLower { get; set; }

            public string AlarmMode { get; set; }

            public Nullable<double> AlarmUpper { get; set; }

            public int BitNo { get; set; }

            public double Coefficient { get; set; }

            public string ControlID { get; set; }

            public Nullable<int> Degree { get; set; }

            public string Depiction { get; set; }

            public Nullable<int> DIInvokeWarningValue { get; set; }

            public Nullable<int> GroupID { get; set; }

            public bool IsShow { get; set; }

            public int ItemID { get; set; }

            public string ItemName { get; set; }

            public string Lable { get; set; }

            public int Length { get; set; }

            public double Offset { get; set; }

            public EntityCollection<tblAIItem1HourLog> tblAIItem1HourLog { get; set; }

            public tblControllerConfig tblControllerConfig { get; set; }

            public tblItemGroup tblItemGroup { get; set; }

            public string Type { get; set; }

            public string UIType { get; set; }

            public string Unit { get; set; }

            public Nullable<double> Value { get; set; }

            public double ValueScale { get; set; }

            public Nullable<double> WarningLower { get; set; }

            public Nullable<double> WarningUpper { get; set; }

            public Nullable  <double>  X { get; set; }
            public Nullable< double> Y { get; set; }
            public Nullable<double> ScaleX { get; set; }
            public Nullable<double> ScaleY { get; set; }
            public Nullable<double> Rotation { get; set; }

            public string Lv0Color { get; set; } 
            public string Lv1Color { get; set; }
             public string Lv2Color { get; set; }
             public  Nullable<int>  GroupTypeID {get;set;}
            public Nullable <int> KindID  {get;set;}
 

        }
    }

    // MetadataTypeAttribute 會將 tblItemGroupMetadata 識別為
    // 帶有 tblItemGroup 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblItemGroup.tblItemGroupMetadata))]
    public partial class tblItemGroup
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblItemGroup 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblItemGroupMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblItemGroupMetadata()
            {
            }

            public int GroupID { get; set; }

            public string GroupName { get; set; }

            public Nullable<short> GroupType { get; set; }

            public Nullable<int> PlaneID { get; set; }

            public double Rotation { get; set; }

            public double ScaleX { get; set; }

            public double ScaleY { get; set; }

            public EntityCollection<tblItemConfig> tblItemConfig { get; set; }

            public double X { get; set; }

            public double Y { get; set; }

            public bool IsShow { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblMagneticCardMetadata 識別為
    // 帶有 tblMagneticCard 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblMagneticCard.tblMagneticCardMetadata))]
    public partial class tblMagneticCard
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblMagneticCard 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblMagneticCardMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblMagneticCardMetadata()
            {
            }

            public string ABA { get; set; }

            public string Company { get; set; }

            public string EmployeeNo { get; set; }

            public string Enable { get; set; }

            public DateTime EndDate { get; set; }

            public string IDNumber { get; set; }

            public string JobTitle { get; set; }

            public int MagneticID { get; set; }

            public string Memo { get; set; }

            public string Mobile { get; set; }

            public string Name { get; set; }

            public Nullable<int> NormalID { get; set; }

            public Nullable<DateTime> ReturnDate { get; set; }

            public int RoleID { get; set; }

            public Nullable<DateTime> StartDate { get; set; }

            public tblSysRole tblSysRole { get; set; }

            public string Tel { get; set; }

            public DateTime Timestamp { get; set; }

            public Nullable<short> Type { get; set; }

            public string WEG1 { get; set; }

            public string WEG2 { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblMagneticCardNormalGroupMetadata 識別為
    // 帶有 tblMagneticCardNormalGroup 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblMagneticCardNormalGroup.tblMagneticCardNormalGroupMetadata))]
    public partial class tblMagneticCardNormalGroup
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblMagneticCardNormalGroup 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblMagneticCardNormalGroupMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblMagneticCardNormalGroupMetadata()
            {
            }

            public string Memo { get; set; }

            public int NormalID { get; set; }

            public string NormalName { get; set; }

            public Nullable<DateTime> UpdateDate { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblMenuMetadata 識別為
    // 帶有 tblMenu 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblMenu.tblMenuMetadata))]
    public partial class tblMenu
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblMenu 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblMenuMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblMenuMetadata()
            {
            }

            public int MenuGroupID { get; set; }

            public int MenuID { get; set; }

            public string MenuName { get; set; }

            public tblMenuGroup tblMenuGroup { get; set; }

            public EntityCollection<tblUserGroupMenu> tblUserGroupMenu { get; set; }

            public string XAML { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblMenuGroupMetadata 識別為
    // 帶有 tblMenuGroup 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblMenuGroup.tblMenuGroupMetadata))]
    public partial class tblMenuGroup
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblMenuGroup 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblMenuGroupMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblMenuGroupMetadata()
            {
            }

            public string GroupName { get; set; }

            public int MenuGroupID { get; set; }

            public Nullable<int> MenuOrder { get; set; }

            public EntityCollection<tblMenu> tblMenu { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblNVRConfigMetadata 識別為
    // 帶有 tblNVRConfig 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblNVRConfig.tblNVRConfigMetadata))]
    public partial class tblNVRConfig
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblNVRConfig 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblNVRConfigMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblNVRConfigMetadata()
            {
            }

            public int ERID { get; set; }

            public string IP { get; set; }

            public int NVRID { get; set; }

            public string NVRName { get; set; }

            public string Password { get; set; }

            public Nullable<int> PlaneID { get; set; }

            public int Port { get; set; }

            public tblEngineRoomConfig tblEngineRoomConfig { get; set; }

            public Nullable<int> Type { get; set; }

            public string UserName { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblSchConfigMetadata 識別為
    // 帶有 tblSchConfig 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblSchConfig.tblSchConfigMetadata))]
    public partial class tblSchConfig
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblSchConfig 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblSchConfigMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblSchConfigMetadata()
            {
            }

            public Nullable<bool> Enable { get; set; }

            public string SchDesc { get; set; }

            public long SchID { get; set; }

            public string SchName { get; set; }

            public Nullable<short> SchType { get; set; }

            public DateTime StartTime { get; set; }

            public tblSchLog tblSchLog { get; set; }

            public tblUser tblUser { get; set; }

            public DateTime TimeStamp { get; set; }

            public string UserID { get; set; }

            public Nullable<bool> Week1 { get; set; }

            public Nullable<bool> Week2 { get; set; }

            public Nullable<bool> Week3 { get; set; }

            public Nullable<bool> Week4 { get; set; }

            public Nullable<bool> Week5 { get; set; }

            public Nullable<bool> Week6 { get; set; }

            public Nullable<bool> Week7 { get; set; }

            public int ReportID { get; set; }
          
            public Nullable<DateTime> NextStartTime { get; set; }

            [MetadataTypeAttribute(typeof(tblReportList.tblReportListMetadata))]
            public partial class tblReportList
            {

                // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblReportList 類別
                // 的 properties。
                //
                // 例如，下列程式碼將 Xyz 屬性標記為
                // 必要的屬性，並指定有效值的格式:
                //    [Required]
                //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
                //    [StringLength(32)]
                //    public string Xyz { get; set; }
                internal sealed class tblReportListMetadata
                {

                    // 中繼資料類別本就不應該具現化。
                    private tblReportListMetadata()
                    {
                    }

                    public Nullable<bool> IsEnable { get; set; }

                    public string ReportDesc { get; set; }

                    public int ReportID { get; set; }

                    public string ReportName { get; set; }

                    public Nullable<short> ReportType { get; set; }
                }
            }


         /*   [ReportID] [int] NOT NULL,
[SchDesc] [nvarchar](100) NULL,
[NextStartTime] [datetime] NULL,
          * */


        }
    }

    // MetadataTypeAttribute 會將 tblSchLogMetadata 識別為
    // 帶有 tblSchLog 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblSchLog.tblSchLogMetadata))]
    public partial class tblSchLog
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblSchLog 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblSchLogMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblSchLogMetadata()
            {
            }

            public string Memo { get; set; }

            public bool Result { get; set; }

            public long SchID { get; set; }

            public tblSchConfig tblSchConfig { get; set; }
           
            public DateTime TimeStamp { get; set; }
            public int ReportID { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblSingalIOMetadata 識別為
    // 帶有 tblSingalIO 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblSingalIO.tblSingalIOMetadata))]
    public partial class tblSingalIO
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblSingalIO 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblSingalIOMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblSingalIOMetadata()
            {
            }

            public string ControlID { get; set; }

            public string Memo { get; set; }

            public Nullable<short> Place { get; set; }

            public string SingalName { get; set; }

            public string SingalType { get; set; }

            public tblControllerConfig tblControllerConfig { get; set; }

            public EntityCollection<tblSingalIOLog> tblSingalIOLog { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblSingalIOLogMetadata 識別為
    // 帶有 tblSingalIOLog 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblSingalIOLog.tblSingalIOLogMetadata))]
    public partial class tblSingalIOLog
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblSingalIOLog 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblSingalIOLogMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblSingalIOLogMetadata()
            {
            }

            public string ControlID { get; set; }

            public long FlowID { get; set; }

            public string SingalName { get; set; }

            public Nullable<short> Status { get; set; }

            public tblSingalIO tblSingalIO { get; set; }

            public DateTime TimeStamp { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblSysOperationMetadata 識別為
    // 帶有 tblSysOperation 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblSysOperation.tblSysOperationMetadata))]
    public partial class tblSysOperation
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblSysOperation 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblSysOperationMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblSysOperationMetadata()
            {
            }

            public string ControlID { get; set; }

            public string Memo { get; set; }

            public string OPDesc { get; set; }

            public long OPID { get; set; }

            public string OPItem { get; set; }

            public bool OPResult { get; set; }

            public DateTime OPTime { get; set; }

            public tblUser tblUser { get; set; }

            public Nullable<short> TypeCode { get; set; }

            public Nullable<short> TypeID { get; set; }

            public string UserID { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblSysParameterMetadata 識別為
    // 帶有 tblSysParameter 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblSysParameter.tblSysParameterMetadata))]
    public partial class tblSysParameter
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblSysParameter 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblSysParameterMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblSysParameterMetadata()
            {
            }

            public string Memo { get; set; }

            public long SysID { get; set; }

            public string VariableName { get; set; }

            public string VariableValue { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblSysRoleMetadata 識別為
    // 帶有 tblSysRole 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblSysRole.tblSysRoleMetadata))]
    public partial class tblSysRole
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblSysRole 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblSysRoleMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblSysRoleMetadata()
            {
            }

            public string Memo { get; set; }

            public int RoleID { get; set; }

            public string RoleName { get; set; }

            public EntityCollection<tblMagneticCard> tblMagneticCard { get; set; }

            public EntityCollection<tblSysRoleAuthority> tblSysRoleAuthority { get; set; }

            public Nullable<DateTime> UpdateDate { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblSysRoleAuthorityMetadata 識別為
    // 帶有 tblSysRoleAuthority 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblSysRoleAuthority.tblSysRoleAuthorityMetadata))]
    public partial class tblSysRoleAuthority
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblSysRoleAuthority 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblSysRoleAuthorityMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblSysRoleAuthorityMetadata()
            {
            }

            public string ControlID { get; set; }

            public string Enable { get; set; }

            public int RoleID { get; set; }

            public tblControllerConfig tblControllerConfig { get; set; }

            public tblSysRole tblSysRole { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblTypeDetailMetadata 識別為
    // 帶有 tblTypeDetail 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblTypeDetail.tblTypeDetailMetadata))]
    public partial class tblTypeDetail
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblTypeDetail 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblTypeDetailMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblTypeDetailMetadata()
            {
            }

            public string Explain { get; set; }

            public string Memo { get; set; }

            public EntityCollection<tblAlarmLog> tblAlarmLog { get; set; }

            public EntityCollection<tblDeviceStateLog> tblDeviceStateLog { get; set; }

            public short TypeCode { get; set; }

            public short TypeId { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblUserMetadata 識別為
    // 帶有 tblUser 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblUser.tblUserMetadata))]
    public partial class tblUser
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblUser 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblUserMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblUserMetadata()
            {
            }

            public Nullable<bool> Enable { get; set; }

            public int GroupID { get; set; }

            public string Memo { get; set; }

            public string Password { get; set; }

            public EntityCollection<tblSchConfig> tblSchConfig { get; set; }

            public EntityCollection<tblSysOperation> tblSysOperation { get; set; }

            public tblUserGroup tblUserGroup { get; set; }

            public Nullable<DateTime> UpdateDate { get; set; }

            public string UserID { get; set; }

            public string UserName { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblUserGroupMetadata 識別為
    // 帶有 tblUserGroup 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblUserGroup.tblUserGroupMetadata))]
    public partial class tblUserGroup
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblUserGroup 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblUserGroupMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblUserGroupMetadata()
            {
            }

            public int GroupID { get; set; }

            public string GroupName { get; set; }

            public EntityCollection<tblUser> tblUser { get; set; }

            public EntityCollection<tblUserGroupMenu> tblUserGroupMenu { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblUserGroupMenuMetadata 識別為
    // 帶有 tblUserGroupMenu 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblUserGroupMenu.tblUserGroupMenuMetadata))]
    public partial class tblUserGroupMenu
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblUserGroupMenu 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblUserGroupMenuMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblUserGroupMenuMetadata()
            {
            }

            public int GroupID { get; set; }

            public Nullable<bool> IsAllow { get; set; }

            public int MenuID { get; set; }

            [Include]
            public tblMenu tblMenu { get; set; }

            public tblUserGroup tblUserGroup { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 vwCardCommandLogMetadata 識別為
    // 帶有 vwCardCommandLog 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(vwCardCommandLog.vwCardCommandLogMetadata))]
    public partial class vwCardCommandLog
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 vwCardCommandLog 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vwCardCommandLogMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private vwCardCommandLogMetadata()
            {
            }

            public string ABA { get; set; }

            public string CardType { get; set; }

            public string CardTypeName { get; set; }

            public string CommandType { get; set; }

            public string CommandTypeName { get; set; }

            public string ControlID { get; set; }

            public string Describe { get; set; }

            public string ERName { get; set; }

            public long FlowID { get; set; }

            public bool IsSuccess { get; set; }

            public string IsSuccessName { get; set; }

            public string Memo { get; set; }

            public string Name { get; set; }

            public Nullable<DateTime> Timestamp { get; set; }

            public Nullable<short> Type { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 vwEngineRoomLogMetadata 識別為
    // 帶有 vwEngineRoomLog 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(vwEngineRoomLog.vwEngineRoomLogMetadata))]
    public partial class vwEngineRoomLog
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 vwEngineRoomLog 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vwEngineRoomLogMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private vwEngineRoomLogMetadata()
            {
            }

            public string ABA { get; set; }

            public string CardType { get; set; }

            public string ControlID { get; set; }

            public string Door { get; set; }

            public Nullable<DateTime> Endtime { get; set; }

            public string ERName { get; set; }

            public string ERNo { get; set; }

            public string Explain { get; set; }

            public long FlowID { get; set; }

            public Nullable<bool> IsEnable { get; set; }

            public string Memo { get; set; }

            public string Name { get; set; }

            public string NVRFile { get; set; }

            public short Result { get; set; }

            public string ResultName { get; set; }

            public DateTime StartTime { get; set; }

            public Nullable<int> TriggerCCTVID { get; set; }

            public Nullable<short> Type { get; set; }

            public Nullable<short> TypeCode { get; set; }

            public string TypeDetailMemo { get; set; }

            public Nullable<short> TypeID { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 vwEntranceGuardDetailMetadata 識別為
    // 帶有 vwEntranceGuardDetail 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(vwEntranceGuardDetail.vwEntranceGuardDetailMetadata))]
    public partial class vwEntranceGuardDetail
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 vwEntranceGuardDetail 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vwEntranceGuardDetailMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private vwEntranceGuardDetailMetadata()
            {
            }

            public string ControlID { get; set; }

            public short ControlType { get; set; }

            public int ERID { get; set; }

            public string ERName { get; set; }

            public Nullable<bool> IsEnable { get; set; }

            public string Memo { get; set; }

            public Nullable<int> PlaneID { get; set; }

            public Nullable<int> TriggerCCTVID { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 vwMagneticCardMetadata 識別為
    // 帶有 vwMagneticCard 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(vwMagneticCard.vwMagneticCardMetadata))]
    public partial class vwMagneticCard
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 vwMagneticCard 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vwMagneticCardMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private vwMagneticCardMetadata()
            {
            }

            public string ABA { get; set; }

            public string Company { get; set; }

            public string EmployeeNo { get; set; }

            public string Enable { get; set; }

            public DateTime EndDate { get; set; }

            public string IDNumber { get; set; }

            public string JobTitle { get; set; }

            public int MagneticID { get; set; }

            public string Memo { get; set; }

            public string Mobile { get; set; }

            public string Name { get; set; }

            public Nullable<int> NormalID { get; set; }

            public string NormalName { get; set; }

            public Nullable<DateTime> ReturnDate { get; set; }

            public int RoleID { get; set; }

            public Nullable<DateTime> StartDate { get; set; }

            public string Tel { get; set; }

            public DateTime Timestamp { get; set; }

            public Nullable<short> Type { get; set; }

            public string TypeName { get; set; }

            public string WEG1 { get; set; }

            public string WEG2 { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 vwMagneticCardAllowControllerMetadata 識別為
    // 帶有 vwMagneticCardAllowController 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(vwMagneticCardAllowController.vwMagneticCardAllowControllerMetadata))]
    public partial class vwMagneticCardAllowController
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 vwMagneticCardAllowController 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vwMagneticCardAllowControllerMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private vwMagneticCardAllowControllerMetadata()
            {
            }

            public string ABA { get; set; }

            public string ControlID { get; set; }

            public string Enable { get; set; }

            public DateTime EndDate { get; set; }

            public int MagneticID { get; set; }

            public Nullable<DateTime> StartDate { get; set; }

            public Nullable<short> Type { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 vwMagneticCardDetailMetadata 識別為
    // 帶有 vwMagneticCardDetail 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(vwMagneticCardDetail.vwMagneticCardDetailMetadata))]
    public partial class vwMagneticCardDetail
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 vwMagneticCardDetail 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vwMagneticCardDetailMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private vwMagneticCardDetailMetadata()
            {
            }

            public string ABA { get; set; }

            public string Company { get; set; }

            public string ControlID { get; set; }

            public string Door { get; set; }

            public string EmployeeNo { get; set; }

            public string Enable { get; set; }

            public DateTime EndDate { get; set; }

            public Nullable<int> ERID { get; set; }

            public string ERName { get; set; }

            public string IDNumber { get; set; }

            public Nullable<bool> IsEnable { get; set; }

            public string JobTitle { get; set; }

            public int MagneticID { get; set; }

            public string Memo { get; set; }

            public string Mobile { get; set; }

            public string Name { get; set; }

            public Nullable<int> NormalID { get; set; }

            public string NormalName { get; set; }

            public Nullable<DateTime> ReturnDate { get; set; }

            public int RoleID { get; set; }

            public string RoleName { get; set; }

            public Nullable<DateTime> StartDate { get; set; }

            public string Tel { get; set; }

            public DateTime Timestamp { get; set; }

            public Nullable<short> Type { get; set; }

            public string TypeName { get; set; }

            public string WEG1 { get; set; }

            public string WEG2 { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 vwUserGroupMenuAllowMetadata 識別為
    // 帶有 vwUserGroupMenuAllow 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(vwUserGroupMenuAllow.vwUserGroupMenuAllowMetadata))]
    public partial class vwUserGroupMenuAllow
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 vwUserGroupMenuAllow 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vwUserGroupMenuAllowMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private vwUserGroupMenuAllowMetadata()
            {
            }

            public int GroupID { get; set; }

            public string GroupName { get; set; }

            public Nullable<bool> IsAllow { get; set; }

            public int MenuID { get; set; }

            public string MenuName { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 vwUserMenuAllowMetadata 識別為
    // 帶有 vwUserMenuAllow 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(vwUserMenuAllow.vwUserMenuAllowMetadata))]
    public partial class vwUserMenuAllow
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 vwUserMenuAllow 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vwUserMenuAllowMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private vwUserMenuAllowMetadata()
            {
            }

            public int GroupID { get; set; }

            public string GroupName { get; set; }

            public Nullable<bool> IsAllow { get; set; }

            public int MenuGroupID { get; set; }

            public int MenuID { get; set; }

            public string MenuName { get; set; }

            public Nullable<int> MenuOrder { get; set; }

            public string UserID { get; set; }

            public string XAML { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblPDAlarmLogMetadata 識別為
    // 帶有 tblPDAlarmLog 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblPDAlarmLog.tblPDAlarmLogMetadata))]
    public partial class tblPDAlarmLog
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblPDAlarmLog 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblPDAlarmLogMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblPDAlarmLogMetadata()
            {
            }

            public long FlowID { get; set; }

            public string Memo { get; set; }

            public string PDItem { get; set; }

            public string PDName { get; set; }

            public Nullable<int> Status { get; set; }

            public tblPDConfig tblPDConfig { get; set; }

            public DateTime Timestamp { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblPDConfigMetadata 識別為
    // 帶有 tblPDConfig 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblPDConfig.tblPDConfigMetadata))]
    public partial class tblPDConfig
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblPDConfig 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblPDConfigMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblPDConfigMetadata()
            {
            }

            public Nullable<int> Cabinet { get; set; }

            public Nullable<int> Comm_state { get; set; }

            public string Direction { get; set; }

            public Nullable<int> ERID { get; set; }

            public Nullable<double> GPSX { get; set; }

            public Nullable<double> GPSY { get; set; }

            public string IP { get; set; }

            public Nullable<int> ItemID { get; set; }

            public Nullable<int> L0 { get; set; }

            public Nullable<int> L1 { get; set; }

            public Nullable<int> L2 { get; set; }

            public Nullable<int> L3 { get; set; }

            public Nullable<int> L4 { get; set; }

            public string LineID { get; set; }

            public string Memo { get; set; }

            public Nullable<int> mile_m { get; set; }

            public Nullable<int> NO_Loop { get; set; }

            public string PDName { get; set; }

            public Nullable<int> PlaneID { get; set; }

            public Nullable<int> Port { get; set; }

            public Nullable<int> R0 { get; set; }

            public Nullable<int> R1 { get; set; }

            public Nullable<int> S0 { get; set; }

            public Nullable<int> S1 { get; set; }

            public Nullable<int> T0 { get; set; }

            public Nullable<int> T1 { get; set; }

            public tblEngineRoomConfig tblEngineRoomConfig { get; set; }

            public EntityCollection<tblPDAlarmLog> tblPDAlarmLog { get; set; }

            public Nullable<int> type { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblPDLoopDeviceConfigMetadata 識別為
    // 帶有 tblPDLoopDeviceConfig 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblPDLoopDeviceConfig.tblPDLoopDeviceConfigMetadata))]
    public partial class tblPDLoopDeviceConfig
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblPDLoopDeviceConfig 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblPDLoopDeviceConfigMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblPDLoopDeviceConfigMetadata()
            {
            }

            public string device_type { get; set; }

            public string DeviceName { get; set; }

            public string Direction { get; set; }

            public string Enable { get; set; }

            public Nullable<double> GPSX { get; set; }

            public Nullable<double> GPSY { get; set; }

            public string IP { get; set; }

            public string ISBREAKDOWN { get; set; }

            public string LineID { get; set; }

            public string Location { get; set; }

            public string Mapping_DeviceName { get; set; }

            public string Memo { get; set; }

            public Nullable<int> mile_m { get; set; }

            public short PD_LoopNO { get; set; }

            public string PDName { get; set; }

            public Nullable<int> Port { get; set; }
            public Nullable<int> Diameter { get; set; }
        }
    }


    // MetadataTypeAttribute 會將 vwAlarmLogMetadata 識別為
    // 帶有 vwAlarmLog 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(vwAlarmLog.vwAlarmLogMetadata))]
    public partial class vwAlarmLog
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 vwAlarmLog 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vwAlarmLogMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private vwAlarmLogMetadata()
            {
            }

            public string AbnormalRecoveryContent { get; set; }

            public string AbnormalRecoveryMode { get; set; }

            public Nullable<int> Address { get; set; }

            public string AlarmContent { get; set; }

            public Nullable<double> AlarmLower { get; set; }

            public string AlarmMode { get; set; }

            public Nullable<double> AlarmUpper { get; set; }

            public Nullable<int> BitNo { get; set; }

            public Nullable<double> Coefficient { get; set; }

            public string ControlID { get; set; }

            public Nullable<int> Degree { get; set; }

            public string Depiction { get; set; }

            public Nullable<int> DIInvokeWarningValue { get; set; }

            public Nullable<int> ERID { get; set; }

            public string ERName { get; set; }

            public long EventID { get; set; }

            public string Explain { get; set; }

            public string GroupName { get; set; }

            public Nullable<short> GroupType { get; set; }

            public Nullable<bool> IsShow { get; set; }

            public string ItemConfig_Value { get; set; }

            public Nullable<int> ItemID { get; set; }

            public string ItemName { get; set; }

            public string Lable { get; set; }

            public Nullable<int> Length { get; set; }

            public string LineID { get; set; }

            public string LineName { get; set; }

            public string Memo { get; set; }

            public Nullable<double> Offset { get; set; }

            public DateTime Timestamp { get; set; }

            public string Type { get; set; }

            public short TypeCode { get; set; }

            public string TypeDetail_Memo { get; set; }

            public short TypeID { get; set; }

            public string Unit { get; set; }

            public string Value { get; set; }

            public Nullable<double> ValueScale { get; set; }

            public Nullable<double> WarningLower { get; set; }

            public Nullable<double> WarningUpper { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 vwERNamePlaneListMetadata 識別為
    // 帶有 vwERNamePlaneList 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(vwERNamePlaneList.vwERNamePlaneListMetadata))]
    public partial class vwERNamePlaneList
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 vwERNamePlaneList 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vwERNamePlaneListMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private vwERNamePlaneListMetadata()
            {
            }

            public int ERID { get; set; }

            public string ERName { get; set; }

            public string LineID { get; set; }

            public string LineName { get; set; }

            public string PictureDesc { get; set; }

            public int PlaneID { get; set; }

            public string PlaneName { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 vwERNameRTUListMetadata 識別為
    // 帶有 vwERNameRTUList 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(vwERNameRTUList.vwERNameRTUListMetadata))]
    public partial class vwERNameRTUList
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 vwERNameRTUList 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vwERNameRTUListMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private vwERNameRTUListMetadata()
            {
            }

            public string ControlID { get; set; }

            public int ERID { get; set; }

            public string ERName { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 vwItemConfigMetadata 識別為
    // 帶有 vwItemConfig 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(vwItemConfig.vwItemConfigMetadata))]
    public partial class vwItemConfig
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 vwItemConfig 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vwItemConfigMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private vwItemConfigMetadata()
            {
            }

            public string AbnormalRecoveryContent { get; set; }

            public string AbnormalRecoveryMode { get; set; }

            public int Address { get; set; }

            public string AlarmContent { get; set; }

            public Nullable<double> AlarmLower { get; set; }

            public string AlarmMode { get; set; }

            public Nullable<double> AlarmUpper { get; set; }

            public int BitNo { get; set; }

            public double Coefficient { get; set; }

            public string ControlID { get; set; }

            public Nullable<short> ControlType { get; set; }

            public Nullable<int> Degree { get; set; }

            public string Depiction { get; set; }

            public Nullable<int> DIInvokeWarningValue { get; set; }

            public string EntranceCode { get; set; }

            public Nullable<int> ERID { get; set; }

            public string ERName { get; set; }

            public Nullable<int> GroupID { get; set; }

            public Nullable<bool> GroupIsShow { get; set; }

            public string GroupName { get; set; }

            public Nullable<short> GroupType { get; set; }

            public Nullable<bool> IsEnable { get; set; }

            public bool IsShow { get; set; }

            public int ItemID { get; set; }

            public string ItemName { get; set; }

            public string Lable { get; set; }

            public int Length { get; set; }

            public string LineID { get; set; }

            public string LineName { get; set; }

            public string Loop { get; set; }

            public double Offset { get; set; }

            public string PictureDesc { get; set; }

            public Nullable<int> PlaneID { get; set; }

            public string PlaneName { get; set; }

            public Nullable<double> Rotation { get; set; }

            public Nullable<int> RTUBaseAddress { get; set; }

            public Nullable<int> RTURegisterLength { get; set; }

            public Nullable<double> ScaleX { get; set; }

            public Nullable<double> ScaleY { get; set; }

            public Nullable<int> TriggerCCTVID { get; set; }

            public string Type { get; set; }

            public string UIType { get; set; }

            public string Unit { get; set; }

            public string Value { get; set; }

            public double ValueScale { get; set; }

            public Nullable<double> WarningLower { get; set; }

            public Nullable<double> WarningUpper { get; set; }

            public Nullable<double> X { get; set; }

            public Nullable<double> Y { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 vwItemGroupMetadata 識別為
    // 帶有 vwItemGroup 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(vwItemGroup.vwItemGroupMetadata))]
    public partial class vwItemGroup
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 vwItemGroup 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vwItemGroupMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private vwItemGroupMetadata()
            {
            }

            public string ControlID { get; set; }

            public Nullable<short> ControlType { get; set; }

            public string EntranceCode { get; set; }

            public Nullable<int> ERID { get; set; }

            public string ERName { get; set; }

            public int GroupID { get; set; }

            public string GroupName { get; set; }

            public Nullable<short> GroupType { get; set; }

            public string IP { get; set; }

            public Nullable<bool> IsEnable { get; set; }

            public bool IsShow { get; set; }

            public string LineID { get; set; }

            public string LineName { get; set; }

            public string Loop { get; set; }

            public string PictureDesc { get; set; }

            public Nullable<int> PlaneID { get; set; }

            public string PlaneName { get; set; }

            public Nullable<int> Port { get; set; }

            public double Rotation { get; set; }

            public Nullable<int> RTUBaseAddress { get; set; }

            public Nullable<int> RTURegisterLength { get; set; }

            public double ScaleX { get; set; }

            public double ScaleY { get; set; }

            public Nullable<int> TriggerCCTVID { get; set; }

            public double X { get; set; }

            public double Y { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 vwPDAlarmLogMetadata 識別為
    // 帶有 vwPDAlarmLog 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(vwPDAlarmLog.vwPDAlarmLogMetadata))]
    public partial class vwPDAlarmLog
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 vwPDAlarmLog 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vwPDAlarmLogMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private vwPDAlarmLogMetadata()
            {
            }

            public string Direction { get; set; }

            public string DirectionName { get; set; }

            public Nullable<int> ERID { get; set; }

            public string ERName { get; set; }

            public long FlowID { get; set; }

            public string LineID { get; set; }

            public string LineName { get; set; }

            public int Loop { get; set; }

            public string Memo { get; set; }

            public Nullable<int> mile_m { get; set; }

            public string PDItem { get; set; }

            public string PDItemDetail { get; set; }

            public string PDName { get; set; }

            public Nullable<int> Status { get; set; }

            public string StatusDetail { get; set; }

            public DateTime Timestamp { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 vwPDAlarmLoopDeviceLogMetadata 識別為
    // 帶有 vwPDAlarmLoopDeviceLog 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(vwPDAlarmLoopDeviceLog.vwPDAlarmLoopDeviceLogMetadata))]
    public partial class vwPDAlarmLoopDeviceLog
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 vwPDAlarmLoopDeviceLog 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vwPDAlarmLoopDeviceLogMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private vwPDAlarmLoopDeviceLogMetadata()
            {
            }

            public string Device_Direction { get; set; }

            public string Device_DirectionName { get; set; }

            public string Device_LineID { get; set; }

            public string Device_LineName { get; set; }

            public Nullable<int> Device_mile_m { get; set; }

            public string device_type { get; set; }

            public string DeviceName { get; set; }

            public string Direction { get; set; }

            public string DirectionName { get; set; }

            public Nullable<int> ERID { get; set; }

            public string ERName { get; set; }

            public long FlowID { get; set; }

            public string LineID { get; set; }

            public string LineName { get; set; }

            public int Loop { get; set; }

            public string Mapping_DeviceName { get; set; }

            public string Memo { get; set; }

            public Nullable<int> mile_m { get; set; }

            public Nullable<short> PD_LoopNO { get; set; }

            public string PDItem { get; set; }

            public string PDItemDetail { get; set; }

            public string PDName { get; set; }

            public Nullable<int> Status { get; set; }

            public string StatusDetail { get; set; }

            public DateTime Timestamp { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 vwPDConfigMetadata 識別為
    // 帶有 vwPDConfig 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(vwPDConfig.vwPDConfigMetadata))]
    public partial class vwPDConfig
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 vwPDConfig 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vwPDConfigMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private vwPDConfigMetadata()
            {
            }

            public Nullable<int> Cabinet { get; set; }

            public string CabinetDetail { get; set; }

            public Nullable<int> Comm_state { get; set; }

            public string Comm_stateDetail { get; set; }

            public string Direction { get; set; }

            public string DirectionName { get; set; }

            public Nullable<int> ERID { get; set; }

            public string ERName { get; set; }

            public Nullable<double> GPSX { get; set; }

            public Nullable<double> GPSY { get; set; }

            public string IP { get; set; }

            public Nullable<int> ItemID { get; set; }

            public Nullable<int> L0 { get; set; }

            public string L0Detail { get; set; }

            public Nullable<int> L1 { get; set; }

            public string L1Detail { get; set; }

            public Nullable<int> L2 { get; set; }

            public string L2Detail { get; set; }

            public Nullable<int> L3 { get; set; }

            public string L3Detail { get; set; }

            public Nullable<int> L4 { get; set; }

            public string L4Detail { get; set; }

            public string LineID { get; set; }

            public string LineName { get; set; }

            public string Memo { get; set; }

            public Nullable<int> mile_m { get; set; }

            public Nullable<int> NO_Loop { get; set; }

            public string PDName { get; set; }

            public Nullable<int> PlaneID { get; set; }

            public Nullable<int> Port { get; set; }

            public Nullable<int> R0 { get; set; }

            public string R0Detail { get; set; }

            public Nullable<int> R1 { get; set; }

            public string R1Detail { get; set; }

            public Nullable<int> S0 { get; set; }

            public string S0Detail { get; set; }

            public Nullable<int> S1 { get; set; }

            public string S1Detail { get; set; }

            public Nullable<int> T0 { get; set; }

            public string T0Detail { get; set; }

            public Nullable<int> T1 { get; set; }

            public string T1Detail { get; set; }

            public Nullable<int> type { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 vwPDLoopDeviceConfigMetadata 識別為
    // 帶有 vwPDLoopDeviceConfig 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(vwPDLoopDeviceConfig.vwPDLoopDeviceConfigMetadata))]
    public partial class vwPDLoopDeviceConfig
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 vwPDLoopDeviceConfig 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vwPDLoopDeviceConfigMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private vwPDLoopDeviceConfigMetadata()
            {
            }

            public string device_type { get; set; }

            public string DeviceName { get; set; }

            public string Direction { get; set; }

            public string DirectionName { get; set; }

            public string Enable { get; set; }

            public Nullable<double> GPSX { get; set; }

            public Nullable<double> GPSY { get; set; }

            public string IP { get; set; }

            public string ISBREAKDOWN { get; set; }

            public string LineID { get; set; }

            public string LineName { get; set; }

            public string Location { get; set; }

            public string Mapping_DeviceName { get; set; }

            public string Memo { get; set; }

            public Nullable<int> mile_m { get; set; }

            public short PD_LoopNO { get; set; }

            public string PDName { get; set; }

            public Nullable<int> Port { get; set; }
        }
    }
    [MetadataTypeAttribute(typeof(vwAllAlarmLog.vwAllAlarmLogMetadata))]
    public partial class vwAllAlarmLog
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 vwAllAlarmLog 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vwAllAlarmLogMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private vwAllAlarmLogMetadata()
            {
            }

            public string ABAAndItemName { get; set; }

            public string DoorAndGroupName { get; set; }

            public string ERName { get; set; }

            public string Item { get; set; }

            public string NameAndType { get; set; }

            public string Status { get; set; }

            public DateTime Timestamp { get; set; }
        }
    }


    [MetadataTypeAttribute(typeof(vwControllerConfig.vwControllerConfigMetadata))]
    public partial class vwControllerConfig
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 vwControllerConfig 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vwControllerConfigMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private vwControllerConfigMetadata()
            {
            }

            public Nullable<int> Comm_state { get; set; }

            public string Comm_stateName { get; set; }

            public string ControlID { get; set; }

            public short ControlType { get; set; }

            public string ControlTypeName { get; set; }

            public string EntranceCode { get; set; }

            public int ERID { get; set; }

            public string ERName { get; set; }

            public Nullable<bool> IsEnable { get; set; }

            public string Memo { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 vwDeviceStateLogMetadata 識別為
    // 帶有 vwDeviceStateLog 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(vwDeviceStateLog.vwDeviceStateLogMetadata))]
    public partial class vwDeviceStateLog
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 vwDeviceStateLog 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vwDeviceStateLogMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private vwDeviceStateLogMetadata()
            {
            }

            public string ControlID { get; set; }

            public int ERID { get; set; }

            public string ERName { get; set; }

            public string Explain { get; set; }

            public long FlowID { get; set; }

            public string Memo { get; set; }

            public DateTime TimeStamp { get; set; }

            public short TypeCode { get; set; }

            public short TypeID { get; set; }
        }
    }

    [MetadataTypeAttribute(typeof(vwReaderPassList.vwReaderPassListMetadata))]
    public partial class vwReaderPassList
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 vwReaderPassList 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vwReaderPassListMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private vwReaderPassListMetadata()
            {
            }

            public string CardType { get; set; }

            public string Door { get; set; }

            public string ERName { get; set; }

            public long FlowID { get; set; }

            public string Memo { get; set; }

            public Nullable<DateTime> StartTime { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 vwReaderPassLogMetadata 識別為
    // 帶有 vwReaderPassLog 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(vwReaderPassLog.vwReaderPassLogMetadata))]
    public partial class vwReaderPassLog
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 vwReaderPassLog 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vwReaderPassLogMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private vwReaderPassLogMetadata()
            {
            }

            public string ControlID { get; set; }

            public Nullable<DateTime> starttime { get; set; }
        }
    }


    // MetadataTypeAttribute 會將 tblItemGroupTypeInfoMetadata 識別為
    // 帶有 tblItemGroupTypeInfo 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblItemGroupTypeInfo.tblItemGroupTypeInfoMetadata))]
    public partial class tblItemGroupTypeInfo
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblItemGroupTypeInfo 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblItemGroupTypeInfoMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblItemGroupTypeInfoMetadata()
            {
            }

            public int GroupTypeID { get; set; }

            public string GroupTypeName { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblItemKindInfoMetadata 識別為
    // 帶有 tblItemKindInfo 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblItemKindInfo.tblItemKindInfoMetadata))]
    public partial class tblItemKindInfo
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblItemKindInfo 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblItemKindInfoMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblItemKindInfoMetadata()
            {
            }

            public int KindID { get; set; }

            public string KindName { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 vwAIImmediatelyCheckMetadata 識別為
    // 帶有 vwAIImmediatelyCheck 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(vwAIImmediatelyCheck.vwAIImmediatelyCheckMetadata))]
    public partial class vwAIImmediatelyCheck
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 vwAIImmediatelyCheck 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vwAIImmediatelyCheckMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private vwAIImmediatelyCheckMetadata()
            {
            }

            public string ControlID { get; set; }

            public Nullable<decimal> DCValue { get; set; }

            public string ERName { get; set; }

            public string LineID { get; set; }

            public string LineName { get; set; }

            public Nullable<decimal> OilValue { get; set; }

            public Nullable<decimal> RHValue { get; set; }

            public Nullable<decimal> TempValue { get; set; }

            public Nullable<decimal> Transmitter10Value { get; set; }

            public Nullable<decimal> Transmitter1Value { get; set; }

            public Nullable<decimal> Transmitter2Value { get; set; }

            public Nullable<decimal> Transmitter3Value { get; set; }

            public Nullable<decimal> Transmitter4Value { get; set; }

            public Nullable<decimal> Transmitter5Value { get; set; }

            public Nullable<decimal> Transmitter6Value { get; set; }

            public Nullable<decimal> Transmitter7Value { get; set; }

            public Nullable<decimal> Transmitter8Value { get; set; }

            public Nullable<decimal> Transmitter9Value { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 vwNVRConfigMetadata 識別為
    // 帶有 vwNVRConfig 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(vwNVRConfig.vwNVRConfigMetadata))]
    public partial class vwNVRConfig
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 vwNVRConfig 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vwNVRConfigMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private vwNVRConfigMetadata()
            {
            }

            public string CCTVIP { get; set; }

            public string CCTVName { get; set; }

            public Nullable<int> CCTVPort { get; set; }

            public string Direction { get; set; }

            public int ERID { get; set; }

            public string ERName { get; set; }

            public string IP { get; set; }

            public string LineID { get; set; }

            public string Memo { get; set; }

            public Nullable<int> NVRChNO { get; set; }

            public int NVRID { get; set; }

            public string NVRName { get; set; }

            public string Password { get; set; }

            public Nullable<int> PlaneID { get; set; }

            public int Port { get; set; }

            public Nullable<int> Type { get; set; }

            public string UserName { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 vwPasswordEveryDayDifferenceMetadata 識別為
    // 帶有 vwPasswordEveryDayDifference 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(vwPasswordEveryDayDifference.vwPasswordEveryDayDifferenceMetadata))]
    public partial class vwPasswordEveryDayDifference
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 vwPasswordEveryDayDifference 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vwPasswordEveryDayDifferenceMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private vwPasswordEveryDayDifferenceMetadata()
            {
            }

            public string ControlID { get; set; }

            public string DoorPassword { get; set; }

            public string ERName { get; set; }

            public string Memo { get; set; }

            public DateTime Timestamp { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 vwR23DeviceStateLogMetadata 識別為
    // 帶有 vwR23DeviceStateLog 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(vwR23DeviceStateLog.vwR23DeviceStateLogMetadata))]
    public partial class vwR23DeviceStateLog
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 vwR23DeviceStateLog 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vwR23DeviceStateLogMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private vwR23DeviceStateLogMetadata()
            {
            }

            public string ABA { get; set; }

            public string CallOpenDoor { get; set; }

            public Nullable<int> Comm_state { get; set; }

            public string ControlID { get; set; }

            public Nullable<short> ControlType { get; set; }

            public string Direction { get; set; }

            public string EntranceCode { get; set; }

            public Nullable<int> ERID { get; set; }

            public string ERName { get; set; }

            public string ERNo { get; set; }

            public string Explain { get; set; }

            public long FlowID { get; set; }

            public Nullable<double> GPSX { get; set; }

            public Nullable<double> GPSY { get; set; }

            public string IP { get; set; }

            public Nullable<bool> IsEnable { get; set; }

            public string LineID { get; set; }

            public string Loop { get; set; }

            public string Memo { get; set; }

            public Nullable<int> PlaneID { get; set; }

            public Nullable<int> Port { get; set; }

            public string R23_ADAM { get; set; }

            public Nullable<short> R23DoorOpen_DI { get; set; }

            public Nullable<short> R23DoorOpen_DO { get; set; }

            public Nullable<int> ReaderID { get; set; }

            public Nullable<double> Rotation { get; set; }

            public Nullable<int> RTUBaseAddress { get; set; }

            public Nullable<int> RTURegisterLength { get; set; }

            public Nullable<double> ScaleX { get; set; }

            public Nullable<double> ScaleY { get; set; }

            public string SingalName { get; set; }

            public DateTime TimeStamp { get; set; }

            public Nullable<int> TriggerCCTVID { get; set; }

            public short TypeCode { get; set; }

            public short TypeID { get; set; }

            public Nullable<double> X { get; set; }

            public Nullable<double> Y { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 vwR23EngineRoomLogMetadata 識別為
    // 帶有 vwR23EngineRoomLog 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(vwR23EngineRoomLog.vwR23EngineRoomLogMetadata))]
    public partial class vwR23EngineRoomLog
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 vwR23EngineRoomLog 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vwR23EngineRoomLogMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private vwR23EngineRoomLogMetadata()
            {
            }

            public string ABA { get; set; }

            public string CallOpenDoor { get; set; }

            public string Company { get; set; }

            public string ControlID { get; set; }

            public string Direction { get; set; }

            public string EmployeeNo { get; set; }

            public string Enable { get; set; }

            public Nullable<DateTime> EndDate { get; set; }

            public Nullable<DateTime> Endtime { get; set; }

            public Nullable<int> ERID { get; set; }

            public string ERName { get; set; }

            public string ERNo { get; set; }

            public long FlowID { get; set; }

            public Nullable<double> GPSX { get; set; }

            public Nullable<double> GPSY { get; set; }

            public string IDNumber { get; set; }

            public string JobTitle { get; set; }

            public string LineID { get; set; }

            public Nullable<int> MagneticID { get; set; }

            public string Memo { get; set; }

            public string Mobile { get; set; }

            public string Name { get; set; }

            public Nullable<int> NormalID { get; set; }

            public string NVRFile { get; set; }

            public short Result { get; set; }

            public string ResultName { get; set; }

            public Nullable<DateTime> ReturnDate { get; set; }

            public Nullable<int> RoleID { get; set; }

            public Nullable<DateTime> StartDate { get; set; }

            public DateTime StartTime { get; set; }

            public string tblMagneticCard_Memo { get; set; }

            public string Tel { get; set; }

            public Nullable<DateTime> Timestamp { get; set; }

            public Nullable<short> Type { get; set; }

            public Nullable<short> TypeCode { get; set; }

            public Nullable<short> TypeID { get; set; }

            public string WEG1 { get; set; }

            public string WEG2 { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 vwR23HardwareStateMetadata 識別為
    // 帶有 vwR23HardwareState 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(vwR23HardwareState.vwR23HardwareStateMetadata))]
    public partial class vwR23HardwareState
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 vwR23HardwareState 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vwR23HardwareStateMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private vwR23HardwareStateMetadata()
            {
            }

            public string ABA { get; set; }

            public string CallOpenDoor { get; set; }

            public Nullable<int> Comm_state { get; set; }

            public string Company { get; set; }

            public string ControlID { get; set; }

            public Nullable<short> ControlType { get; set; }

            public string Direction { get; set; }

            public string EmployeeNo { get; set; }

            public string Enable { get; set; }

            public Nullable<DateTime> EndDate { get; set; }

            public string EntranceCode { get; set; }

            public string EntranceType { get; set; }

            public Nullable<int> ERID { get; set; }

            public string ERName { get; set; }

            public string ERNo { get; set; }

            public string Explain { get; set; }

            public Nullable<short> Floor { get; set; }

            public long FlowID { get; set; }

            public Nullable<double> GPSX { get; set; }

            public Nullable<double> GPSY { get; set; }

            public string IDNumber { get; set; }

            public string IP { get; set; }

            public Nullable<bool> IsEnable { get; set; }

            public string JobTitle { get; set; }

            public string LineID { get; set; }

            public string Loop { get; set; }

            public Nullable<int> MagneticID { get; set; }

            public string Memo { get; set; }

            public string Mobile { get; set; }

            public string Name { get; set; }

            public Nullable<int> NormalID { get; set; }

            public Nullable<int> PlaneID { get; set; }

            public Nullable<int> Port { get; set; }

            public string R23_ADAM { get; set; }

            public Nullable<short> R23DoorOpen_DI { get; set; }

            public Nullable<short> R23DoorOpen_DO { get; set; }

            public Nullable<int> ReaderID { get; set; }

            public Nullable<short> ReaderNO { get; set; }

            public Nullable<short> ReaderType { get; set; }

            public Nullable<DateTime> ReturnDate { get; set; }

            public Nullable<int> RoleID { get; set; }

            public string RoomIO { get; set; }

            public Nullable<double> Rotation { get; set; }

            public Nullable<int> RTUBaseAddress { get; set; }

            public Nullable<int> RTURegisterLength { get; set; }

            public Nullable<double> ScaleX { get; set; }

            public Nullable<double> ScaleY { get; set; }

            public string SingalName { get; set; }

            public Nullable<DateTime> StartDate { get; set; }

            public string tblCardReaderConfig_ControlID { get; set; }

            public string tblCardReaderConfig_EntranceCode { get; set; }

            public Nullable<int> tblCardReaderConfig_ReaderID { get; set; }

            public string tblEntranceGuardConfig_EntranceCode { get; set; }

            public Nullable<int> tblEntranceGuardConfig_ERID { get; set; }

            public string tblEntranceGuardConfig_Memo { get; set; }

            public string tblEntranceGuardConfig_MemoName { get; set; }

            public string tblMagneticCard_ABA { get; set; }

            public string tblMagneticCard_Memo { get; set; }

            public Nullable<DateTime> tblMagneticCard_Timestamp { get; set; }

            public string Tel { get; set; }

            public DateTime TimeStamp { get; set; }

            public Nullable<int> TriggerCCTVID { get; set; }

            public Nullable<short> Type { get; set; }

            public short TypeCode { get; set; }

            public short TypeID { get; set; }

            public string WEG1 { get; set; }

            public string WEG2 { get; set; }

            public Nullable<double> X { get; set; }

            public Nullable<double> Y { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 vwR23ReaderLogMetadata 識別為
    // 帶有 vwR23ReaderLog 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(vwR23ReaderLog.vwR23ReaderLogMetadata))]
    public partial class vwR23ReaderLog
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 vwR23ReaderLog 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vwR23ReaderLogMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private vwR23ReaderLogMetadata()
            {
            }

            public string ABA { get; set; }

            public string CallOpenDoor { get; set; }

            public Nullable<int> Comm_state { get; set; }

            public string Company { get; set; }

            public string ControlID { get; set; }

            public Nullable<short> ControlType { get; set; }

            public string Direction { get; set; }

            public string EmployeeNo { get; set; }

            public string Enable { get; set; }

            public Nullable<DateTime> EndDate { get; set; }

            public string EntranceCode { get; set; }

            public string EntranceType { get; set; }

            public Nullable<int> ERID { get; set; }

            public string ERName { get; set; }

            public string ERNo { get; set; }

            public string Explain { get; set; }

            public Nullable<short> Floor { get; set; }

            public long FlowID { get; set; }

            public Nullable<double> GPSX { get; set; }

            public Nullable<double> GPSY { get; set; }

            public string IDNumber { get; set; }

            public string IP { get; set; }

            public Nullable<bool> IsEnable { get; set; }

            public string JobTitle { get; set; }

            public string LineID { get; set; }

            public string Loop { get; set; }

            public Nullable<int> MagneticID { get; set; }

            public string Memo { get; set; }

            public string Mobile { get; set; }

            public string Name { get; set; }

            public Nullable<int> NormalID { get; set; }

            public Nullable<int> PlaneID { get; set; }

            public Nullable<int> Port { get; set; }

            public string R23_ADAM { get; set; }

            public Nullable<short> R23DoorOpen_DI { get; set; }

            public Nullable<short> R23DoorOpen_DO { get; set; }

            public Nullable<int> ReaderID { get; set; }

            public Nullable<short> ReaderNO { get; set; }

            public Nullable<short> ReaderType { get; set; }

            public Nullable<DateTime> ReturnDate { get; set; }

            public Nullable<int> RoleID { get; set; }

            public string RoomIO { get; set; }

            public string RoomIOName { get; set; }

            public Nullable<double> Rotation { get; set; }

            public Nullable<int> RTUBaseAddress { get; set; }

            public Nullable<int> RTURegisterLength { get; set; }

            public Nullable<double> ScaleX { get; set; }

            public Nullable<double> ScaleY { get; set; }

            public string SingalName { get; set; }

            public Nullable<DateTime> StartDate { get; set; }

            public string tblCardReaderConfig_ControlID { get; set; }

            public string tblCardReaderConfig_EntranceCode { get; set; }

            public Nullable<int> tblCardReaderConfig_ReaderID { get; set; }

            public string tblEntranceGuardConfig_EntranceCode { get; set; }

            public Nullable<int> tblEntranceGuardConfig_ERID { get; set; }

            public string tblEntranceGuardConfig_Memo { get; set; }

            public string tblMagneticCard_ABA { get; set; }

            public string tblMagneticCard_Memo { get; set; }

            public Nullable<DateTime> tblMagneticCard_Timestamp { get; set; }

            public string Tel { get; set; }

            public DateTime TimeStamp { get; set; }

            public Nullable<int> TriggerCCTVID { get; set; }

            public Nullable<short> Type { get; set; }

            public short TypeCode { get; set; }

            public short TypeID { get; set; }

            public string WEG1 { get; set; }

            public string WEG2 { get; set; }

            public Nullable<double> X { get; set; }

            public Nullable<double> Y { get; set; }
        }
    }

    [MetadataTypeAttribute(typeof(vwSchConfig.vwSchConfigMetadata))]
    public partial class vwSchConfig
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 vwSchConfig 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vwSchConfigMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private vwSchConfigMetadata()
            {
            }

            public Nullable<bool> Enable { get; set; }

            public string EnableName { get; set; }

            public Nullable<bool> IsEnable { get; set; }

            public Nullable<DateTime> NextStartTime { get; set; }

            public string ReportDesc { get; set; }

            public int ReportID { get; set; }

            public string ReportName { get; set; }

            public Nullable<short> ReportType { get; set; }

            public string ReportTypeName { get; set; }

            public string SchDesc { get; set; }

            public long SchID { get; set; }

            public string SchName { get; set; }

            public Nullable<short> SchType { get; set; }

            public string SchTypeName { get; set; }

            public DateTime StartTime { get; set; }

            public DateTime TimeStamp { get; set; }

            public string UserID { get; set; }

            public Nullable<bool> Week1 { get; set; }

            public Nullable<bool> Week2 { get; set; }

            public Nullable<bool> Week3 { get; set; }

            public Nullable<bool> Week4 { get; set; }

            public Nullable<bool> Week5 { get; set; }

            public Nullable<bool> Week6 { get; set; }

            public Nullable<bool> Week7 { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 vwSchLogMetadata 識別為
    // 帶有 vwSchLog 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(vwSchLog.vwSchLogMetadata))]
    public partial class vwSchLog
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 vwSchLog 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vwSchLogMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private vwSchLogMetadata()
            {
            }

            public string EnableName { get; set; }

            public string Memo { get; set; }

            public Nullable<DateTime> NextStartTime { get; set; }

            public int ReportID { get; set; }

            public string ReportTypeName { get; set; }

            public bool Result { get; set; }

            public string ResultName { get; set; }

            public long SchID { get; set; }

            public string SchName { get; set; }

            public string SchTypeName { get; set; }

            public DateTime StartTime { get; set; }

            public DateTime TimeStamp { get; set; }

            public string UserID { get; set; }
        }
    }

    [MetadataTypeAttribute(typeof(vwPlaneIDAndControlID.vwPlaneIDAndControlIDMetadata))]
    public partial class vwPlaneIDAndControlID
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 vwPlaneIDAndControlID 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class vwPlaneIDAndControlIDMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private vwPlaneIDAndControlIDMetadata()
            {
            }

            public Nullable<int> Comm_state { get; set; }

            public string ControlID { get; set; }

            public Nullable<short> ControlType { get; set; }

            public int ERID { get; set; }

            public string ERName { get; set; }

            public Nullable<bool> IsEnable { get; set; }

            public string LineID { get; set; }

            public string LineName { get; set; }

            public string PictureDesc { get; set; }

            public int PlaneID { get; set; }

            public string PlaneName { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblCCTVSplitScreenMetadata 識別為
    // 帶有 tblCCTVSplitScreen 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblCCTVSplitScreen.tblCCTVSplitScreenMetadata))]
    public partial class tblCCTVSplitScreen
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblCCTVSplitScreen 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblCCTVSplitScreenMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblCCTVSplitScreenMetadata()
            {
            }

            public Nullable<int> CCTVID { get; set; }

            public int NO { get; set; }
        }
    }


    }


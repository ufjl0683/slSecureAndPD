﻿

//------------------------------------------------------------------------------
// <auto-generated>
//    這個程式碼是由範本產生。
//
//    對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//    如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------


namespace SecureServer
{

using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;


public partial class SecureDBEntities1 : DbContext
{
    public SecureDBEntities1()
        : base("name=SecureDBEntities1")
    {

    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        throw new UnintentionalCodeFirstException();
    }


    public DbSet<tblERDoorPassword> tblERDoorPassword { get; set; }

    public DbSet<tblEngineRoomLog> tblEngineRoomLog { get; set; }

    public DbSet<tblCardCommandLog> tblCardCommandLog { get; set; }

    public DbSet<tblERPlane> tblERPlane { get; set; }

    public DbSet<tblMagneticCard> tblMagneticCard { get; set; }

    public DbSet<vwMagneticCardAllowController> vwMagneticCardAllowController { get; set; }

    public DbSet<tblSysParameter> tblSysParameter { get; set; }

    public DbSet<vwMagneticCardDetail> vwMagneticCardDetail { get; set; }

    public DbSet<tblControllerConfig> tblControllerConfig { get; set; }

    public DbSet<tblItemConfig> tblItemConfig { get; set; }

    public DbSet<tblItemGroup> tblItemGroup { get; set; }

    public DbSet<tblAIItem1HourLog> tblAIItem1HourLog { get; set; }

    public DbSet<tblPDConfig> tblPDConfig { get; set; }

    public DbSet<tblPDAlarmLog> tblPDAlarmLog { get; set; }

    public DbSet<tblAlarmLog> tblAlarmLog { get; set; }

    public DbSet<vwMagneticCard> vwMagneticCard { get; set; }

    public DbSet<vwCardCommandLog> vwCardCommandLog { get; set; }

    public DbSet<vwEngineRoomLog> vwEngineRoomLog { get; set; }

    public DbSet<vwEntranceGuardDetail> vwEntranceGuardDetail { get; set; }

    public DbSet<vwUserGroupMenuAllow> vwUserGroupMenuAllow { get; set; }

    public DbSet<vwUserMenuAllow> vwUserMenuAllow { get; set; }

    public DbSet<tblDeviceStateLog> tblDeviceStateLog { get; set; }

    public DbSet<tblPasswordEveryDayDifference> tblPasswordEveryDayDifference { get; set; }

    public DbSet<tblSchConfig> tblSchConfig { get; set; }

    public DbSet<tblSchLog> tblSchLog { get; set; }

    public DbSet<tblPowerMeter> tblPowerMeter { get; set; }

    public DbSet<tblCardReaderConfig> tblCardReaderConfig { get; set; }

    public DbSet<tblCCTVSplitScreen> tblCCTVSplitScreen { get; set; }

    public DbSet<tblCCTVType> tblCCTVType { get; set; }

    public DbSet<tblControllerCard> tblControllerCard { get; set; }

    public DbSet<tblControlSetting> tblControlSetting { get; set; }

    public DbSet<tblEngineRoomConfig> tblEngineRoomConfig { get; set; }

    public DbSet<tblEntranceGuardConfig> tblEntranceGuardConfig { get; set; }

    public DbSet<tblHostConfig> tblHostConfig { get; set; }

    public DbSet<tblItemGroupTypeInfo> tblItemGroupTypeInfo { get; set; }

    public DbSet<tblItemKindInfo> tblItemKindInfo { get; set; }

    public DbSet<tblMagneticCardNormalGroup> tblMagneticCardNormalGroup { get; set; }

    public DbSet<tblMenu> tblMenu { get; set; }

    public DbSet<tblMenuGroup> tblMenuGroup { get; set; }

    public DbSet<tblPDLoopDeviceConfig> tblPDLoopDeviceConfig { get; set; }

    public DbSet<tblReportList> tblReportList { get; set; }

    public DbSet<tblSingalIO> tblSingalIO { get; set; }

    public DbSet<tblSingalIOLog> tblSingalIOLog { get; set; }

    public DbSet<tblSysOperation> tblSysOperation { get; set; }

    public DbSet<tblSysRole> tblSysRole { get; set; }

    public DbSet<tblSysRoleAuthority> tblSysRoleAuthority { get; set; }

    public DbSet<tblTypeDetail> tblTypeDetail { get; set; }

    public DbSet<tblUser> tblUser { get; set; }

    public DbSet<tblUserGroup> tblUserGroup { get; set; }

    public DbSet<tblUserGroupMenu> tblUserGroupMenu { get; set; }

    public DbSet<tblRemotePowerControl> tblRemotePowerControl { get; set; }

    public DbSet<vwAIImmediatelyCheck> vwAIImmediatelyCheck { get; set; }

    public DbSet<vwAlarmLog> vwAlarmLog { get; set; }

    public DbSet<vwAllAlarmLog> vwAllAlarmLog { get; set; }

    public DbSet<vwControllerConfig> vwControllerConfig { get; set; }

    public DbSet<vwDeviceStateLog> vwDeviceStateLog { get; set; }

    public DbSet<vwERNamePlaneList> vwERNamePlaneList { get; set; }

    public DbSet<vwERNameRTUList> vwERNameRTUList { get; set; }

    public DbSet<vwItemConfig> vwItemConfig { get; set; }

    public DbSet<vwItemGroup> vwItemGroup { get; set; }

    public DbSet<tblPowerMeter1HourLog> tblPowerMeter1HourLog { get; set; }

    public DbSet<tblCCTVConfig> tblCCTVConfig { get; set; }

    public DbSet<tblNVRConfig> tblNVRConfig { get; set; }

    public DbSet<vwCCTVState> vwCCTVState { get; set; }

    public DbSet<vwPDAlarmLoopDeviceLog> vwPDAlarmLoopDeviceLog { get; set; }

}

}


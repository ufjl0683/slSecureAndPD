
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
    using System.Collections.Generic;
    
public partial class tblPowerMeter
{

    public int ERID { get; set; }

    public string ERName { get; set; }

    public string RTU_IP { get; set; }

    public int Port { get; set; }

    public Nullable<double> VA { get; set; }

    public Nullable<double> VB { get; set; }

    public Nullable<double> VC { get; set; }

    public Nullable<double> AVGV { get; set; }

    public Nullable<double> IA { get; set; }

    public Nullable<double> IB { get; set; }

    public Nullable<double> IC { get; set; }

    public Nullable<double> AVGI { get; set; }

    public Nullable<double> KW { get; set; }

    public Nullable<double> PF { get; set; }

    public Nullable<System.DateTime> UpdateDate { get; set; }

    public string Memo { get; set; }

    public Nullable<double> CumulateValue { get; set; }

    public Nullable<double> InstantaneousValue { get; set; }

    public Nullable<double> PowerAlarmUpper { get; set; }

    public Nullable<double> PowerAlarmLower { get; set; }

    public Nullable<double> WaterAlarmUpper { get; set; }

    public Nullable<double> WaterAlarmLower { get; set; }

    public Nullable<bool> WaterAlarm { get; set; }

    public Nullable<bool> PowerAlarm { get; set; }

    public Nullable<double> WaterAlarmAvg { get; set; }

    public Nullable<double> PowerAlarmAvg { get; set; }

    public Nullable<double> WaterConsume { get; set; }

    public string WaterAlarmDesc { get; set; }

    public string PowerAlarmDesc { get; set; }

    public Nullable<double> KW24Avg { get; set; }

}

}

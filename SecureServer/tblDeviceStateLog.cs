
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
    
public partial class tblDeviceStateLog
{

    public long FlowID { get; set; }

    public short TypeID { get; set; }

    public short TypeCode { get; set; }

    public System.DateTime TimeStamp { get; set; }

    public Nullable<int> ReaderID { get; set; }

    public string ControlID { get; set; }

    public string SingalName { get; set; }

    public string ABA { get; set; }



    public virtual tblControllerConfig tblControllerConfig { get; set; }

    public virtual tblTypeDetail tblTypeDetail { get; set; }

}

}

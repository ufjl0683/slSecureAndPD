//------------------------------------------------------------------------------
// <auto-generated>
//    這個程式碼是由範本產生。
//
//    對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//    如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace SecureServer.CardReader
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblCCTVConfig
    {
        public int CCTVID { get; set; }
        public int ERID { get; set; }
        public string CCTVName { get; set; }
        public string IP { get; set; }
        public int Port { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double ScaleX { get; set; }
        public double ScaleY { get; set; }
        public double Rotation { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Nullable<int> PlaneID { get; set; }
        public Nullable<int> NVRID { get; set; }
        public Nullable<int> NVRChNO { get; set; }
        public Nullable<int> Type { get; set; }
    }
}

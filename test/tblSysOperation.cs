//------------------------------------------------------------------------------
// <auto-generated>
//    這個程式碼是由範本產生。
//
//    對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//    如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace test
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblSysOperation
    {
        public long OPID { get; set; }
        public System.DateTime OPTime { get; set; }
        public string UserID { get; set; }
        public string OPItem { get; set; }
        public string OPDesc { get; set; }
        public bool OPResult { get; set; }
        public string ControlID { get; set; }
        public Nullable<short> TypeID { get; set; }
        public Nullable<short> TypeCode { get; set; }
        public string Memo { get; set; }
    
        public virtual tblUser tblUser { get; set; }
    }
}

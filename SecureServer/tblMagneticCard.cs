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
    
    public partial class tblMagneticCard
    {
        public int MagneticID { get; set; }
        public string ABA { get; set; }
        public string WEG1 { get; set; }
        public string WEG2 { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public string Enable { get; set; }
        public string Memo { get; set; }
        public string Company { get; set; }
        public string Name { get; set; }
        public string IDNumber { get; set; }
        public string EmployeeNo { get; set; }
        public string JobTitle { get; set; }
        public string Tel { get; set; }
        public string Mobile { get; set; }
        public Nullable<short> Type { get; set; }
        public Nullable<int> NormalID { get; set; }
        public int RoleID { get; set; }
        public System.DateTime Timestamp { get; set; }
        public Nullable<System.DateTime> ReturnDate { get; set; }
        public string MCNSID { get; set; }
        public Nullable<System.DateTime> CardStartDate { get; set; }
        public Nullable<System.DateTime> CardEndDate { get; set; }
    
        public virtual tblSysRole tblSysRole { get; set; }
    }
}

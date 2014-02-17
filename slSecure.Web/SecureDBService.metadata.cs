
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

            public int GroupID { get; set; }

            public string Password { get; set; }

            public tblUserGroup tblUserGroup { get; set; }

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

            public bool IsAllow { get; set; }

            public int MenuID { get; set; }
            [Include]
            public tblMenu tblMenu { get; set; }
            [Include]
            public tblUserGroup tblUserGroup { get; set; }
        }


      


       
    }

    [MetadataTypeAttribute(typeof(vwUserMenuAllow.vwUserMenuAllowMetadata))]
    public partial class vwUserMenuAllow
{
      internal sealed class vwUserMenuAllowMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private vwUserMenuAllowMetadata()
            {
            }

            public int GroupID { get; set; }

            public string GroupName { get; set; }

            public bool IsAllow { get; set; }

            public int MenuGroupID { get; set; }

            public int MenuID { get; set; }

            public string MenuName { get; set; }

            public Nullable<int> MenuOrder { get; set; }

            public string UserID { get; set; }

            public string XAML { get; set; }
        }
}

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

            public bool IsAllow { get; set; }

            public int MenuID { get; set; }

            public string MenuName { get; set; }
        }
    }


}

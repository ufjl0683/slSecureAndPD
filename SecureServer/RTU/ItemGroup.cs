using SecureServer.BindingData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureServer.RTU
{
   public class ItemGroup
    {


       public int GroupID { get; set; }
       public string GroupName { get; set; }
       ItemManager item_mgr;
       Item[] Items;
       public  int PlaneID;
     public ItemGroup(int PlaneID,int GroupID, string GroupName,ItemManager item_mgr)
       {
           this.PlaneID=PlaneID;
           this.GroupID = GroupID;
           this.GroupName = GroupName;
           this.item_mgr = item_mgr;
           FillItem();
       }

       void FillItem()
       {
           SecureDBEntities1 db = new SecureDBEntities1();
            var q= db.tblItemConfig.Where(n => n.GroupID == GroupID);
            System.Collections.Generic.List<Item> c = new List<Item>();
            foreach (tblItemConfig tbl in q)
            {
                Item item = item_mgr[tbl.ItemID];
                if (item != null)
                    c.Add(item);
            }

            Items = c.ToArray();

       }

       public int Degree
       {
           get
           {
               try
               {
                   return Items.Where(n => n.AlarmMode == "Y").Max(n => n.Degree ?? 0);
               }
               catch
               {
                   return 0;
               }
                  
           }

       }

       public ItemGroupBindingData ToItemGroupBindingData()
       {

           ItemGroupBindingData data = new ItemGroupBindingData() { Content = this.GroupName, GroupID = this.GroupID, PlaneID = this.PlaneID, GroupName = this.GroupName };

           switch (this.Degree)
           {
               case 0:
                   data.ColorString="Green";
                       break;
               case 1:
                    data.ColorString="Yellow";
                       break;
               case 2:
                       data.ColorString = "Red";
                       break;

               case -1:
                       data.ColorString = "Gray";
                       break;
           }

           return data;

       }
    }
}

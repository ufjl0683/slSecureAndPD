using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureServer.RTU
{
   public  class Plane
    {
       Item[] Items;
       public int PlaneID;
       public string PlaneName;
       public int ERID;
       public Plane(int ERID, int PlaneID, string PlaneName)
       {
           this.PlaneID = PlaneID;
           this.PlaneName = PlaneName;
           this.ERID = ERID;
            FillItem();
       }


       void FillItem()
       {
           SecureDBEntities1 db = new SecureDBEntities1();
           var q = db.tblItemConfig.Where(n => n.tblItemGroup.PlaneID == PlaneID  &&(n.Type=="AI" || n.Type=="DI"));
           System.Collections.Generic.List<Item> c = new List<Item>();
           foreach (tblItemConfig tbl in q)
           {
               Item item = SecureService.item_mgr[tbl.ItemID];
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
    }
}

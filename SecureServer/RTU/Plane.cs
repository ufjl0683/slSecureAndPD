using SecureServer.BindingData;
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
              // Console.WriteLine(item.ItemConfig.tblControllerConfig.IP);
           }

           Items = c.ToArray();

       }


       public string DegreeColor
       {
           get
           {

              
               string ret="Gray";
               try
               {

                   //addd 12/27 2016

                   int degree = this.Degree;
                   if (degree == 0)
                       ret= "Green";
                   else if (degree == 1)
                       ret= "Yellow";
                   else if (degree == 2)
                       ret= "Red";
                   else
                       ret= "Gray";
                  
                   //Item[] items = Items.Where(n => n.AlarmMode == "Y" && n.Degree == Degree).ToArray();


                   //if (items == null || items.Length == 0)
                   //    return "Green";
                   //else
                   //{

                   //    foreach (Item item in items)
                   //    {
                   //        ItemBindingData data = item.ToBindingData();
                   //        if (data.ColorString == "Red")
                   //            ret = "Red";
                   //        else if (data.ColorString == "Yellow" && ret != "Red")
                   //            ret = "Yellow";
                   //        else if (data.ColorString == "Green" && ret != "Red" && ret != "Yellow")
                   //            ret = "Green";




                   //    }
                   //}
               }
               catch
               {
                   return "Green";
               }
               return ret;
           }
       }

       public int Degree
       {
           get
           {
               try
               {
                   //===== add 2016/12/12  for offline must be  show  alarm color
                   if (Items.Where(n => n.AlarmMode == "Y" && n.IsConnected==false && n.ItemConfig.tblControllerConfig.IP!="127.0.0.1").FirstOrDefault() != null)
                       return 2;

                   //================================================================
                   return Items.Where(n => n.AlarmMode == "Y" && n.IsConnected  ).Max(n => n.Degree ?? 0);
               }
               catch
               {
                   return 0;
               }

           }

       }
    }
}

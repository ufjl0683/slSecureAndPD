using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureServer.RTU
{
   public  class ItemGroupManager
    {

       System.Collections.Generic.Dictionary<int, ItemGroup> dictItemGroups = new Dictionary<int, ItemGroup>();

       ItemGroup this[int inx]
       {

           get
           {
               if (!dictItemGroups.ContainsKey(inx))
                   return null;


               return dictItemGroups[inx];
           }
       }

       public BindingData.ItemGroupBindingData[] GetAllItemGroupBindingData(int PlaneID)
       {
            
           return dictItemGroups.Where(n => n.Value.PlaneID == PlaneID).Select(n => n.Value.ToItemGroupBindingData()).ToArray();
       }

       public ItemGroupManager()
       {
           SecureDBEntities1 db = new SecureDBEntities1();
           foreach (tblItemGroup tbl in db.tblItemGroup)
           {
               if(!dictItemGroups.ContainsKey(tbl.GroupID))
               {
                   ItemGroup itemgrp = new ItemGroup(tbl.PlaneID??0, tbl.GroupID, tbl.GroupName, SecureService.item_mgr);
                   this.dictItemGroups.Add(tbl.GroupID, itemgrp);
               }
           }


       }


    }
}

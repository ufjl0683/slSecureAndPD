using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureServer.RTU
{
    public   class ItemManager
    {
        System.Collections.Generic.Dictionary<int, Item> Items = new Dictionary<int, Item>();    
        public ItemManager()
        {
            SecureDBEntities1 db = new SecureDBEntities1();
            var q = from n in db.tblItemConfig select n;
            foreach (tblItemConfig item in q)
            {
                Items.Add(item.ItemID, new Item(item.ItemID,SecureService.rtu_mgr[item.ControlID], item.Type,item));

            }
        }
    }
}

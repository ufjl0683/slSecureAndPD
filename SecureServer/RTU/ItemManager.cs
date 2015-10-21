using SecureServer.BindingData;
using SecureServer.CardReader;
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
        ExactIntervalTimer OneHourTmr;

      //  SecureDBEntities1 db = new SecureDBEntities1();
        public Item this[int itemid]
        {
            get{

                if (Items.ContainsKey(itemid))
                    return Items[itemid];
                else
                    return null;
            }
        }

       
        public ItemManager()
        {


            SecureDBEntities1 db = new SecureDBEntities1();
            var q = from n in db.tblItemConfig select n;
            foreach (tblItemConfig tblitem in q)
            {
                Item item = new Item(tblitem.ItemID, SecureService.rtu_mgr[tblitem.ControlID], tblitem.Type, tblitem);
              //  item.Value = tblitem.Value ?? 0;
                Items.Add(tblitem.ItemID,item);
                item.ItemValueChanged += item_ItemValueChanged;
                item.ItemDegreeChanged += item_ItemDegreeChanged;
               
            }

            OneHourTmr = new ExactIntervalTimer(5, 0);
            OneHourTmr.OnElapsed += OneHourTmr_OnElapsed;
           // db.Dispose();
        }

        void item_ItemDegreeChanged(Item sender, int? NewValue)
        {
            SecureDBEntities1 db = new SecureDBEntities1();
          
            if (NewValue == 2  && sender.AlarmMode=="Y")
            {
                AlarmData data=new AlarmData()
                {
                    TimeStamp = DateTime.Now,
                          AlarmType = AlarmType.RTU,
                          ColorString = "Red",
                          Description = sender.ItemConfig.ItemName+"警報",
                          PlaneID = sender.PlaneID,
                          IsForkCCTVEvent = false,
                          PlaneName =Global.GetPlaneNameByPlaneID(sender.PlaneID)
                        //  CCTVBindingData =cctv.ToBindingData(
                      
                };
                  
                if(!(sender.ItemConfig.Suppress??false))
                      Program.MyServiceObject.DispatchAlarmEvent(data);

                int typecode = 0;
                   switch(sender.ItemType)
                   {
                       case "AI":
                           typecode=3;
                           break;
                       case "DI":
                           typecode=0;
                           break;

                   }
                   tblAlarmLog tblalarmlog = new tblAlarmLog() { ControlID = sender.ItemConfig.ControlID, ItemID = sender.ItemID, Timestamp = DateTime.Now, TypeID = 5, TypeCode = (short)typecode, Value = sender.Value };
                db.tblAlarmLog.Add(tblalarmlog);
            }
            else if (NewValue == 1 && sender.ItemType == "AI" && sender.AlarmMode == "Y")
            {

                int typecode = 0;
                switch (sender.ItemType)
                {
                    case "AI":
                        typecode = 2;
                        break;
                    //case "DI":
                    //    typecode = 1;
                    //    break;

                }
                tblAlarmLog tblalarmlog = new tblAlarmLog() { ControlID = sender.ItemConfig.ControlID, ItemID = sender.ItemID, Timestamp = DateTime.Now, TypeID = 5, TypeCode = (short)typecode, Value = sender.Value };
                db.tblAlarmLog.Add(tblalarmlog);
               
            }

            if (NewValue == 0)
            {
                sender.ItemConfig.Suppress = false;
                tblItemConfig item = db.tblItemConfig.Where(n => n.ItemID == sender.ItemID).FirstOrDefault();
                if (item != null)
                    item.Suppress = false;


            }
        
            if( NewValue==0 && sender.AlarmMode=="Y")
            {

                 int typecode=0;
                   switch(sender.ItemType)
                   {
                       case "AI":
                           typecode=4;
                           break;
                       case "DI":
                           typecode=1;
                           break;

                   }
                   tblAlarmLog tblalarmlog = new tblAlarmLog() { ControlID = sender.ItemConfig.ControlID, ItemID = sender.ItemID, Timestamp = DateTime.Now, TypeID = 5, TypeCode = (short)typecode, Value = sender.Value };
                db.tblAlarmLog.Add(tblalarmlog);
            }
         
            tblItemConfig tbl = db.tblItemConfig.Where(n => n.ItemID == sender.ItemID).FirstOrDefault();
            if (tbl != null)
            {
                tbl.Degree = NewValue;
            }

            db.SaveChanges();
            db.Dispose();
        }

        void OneHourTmr_OnElapsed(object sender)
        {
             DateTime dt=DateTime.Now;
               dt=  dt.AddMinutes(-dt.Minute).AddSeconds(-dt.Second).AddMilliseconds(-dt.Millisecond);
                    
            using (SecureDBEntities1 db1 = new SecureDBEntities1())
            {
                foreach (Item item in Items.Values)
                {
                    try
                    {
                        if (item.ItemType == "AI" && item.IsConnected )
                        {
                            tblAIItem1HourLog tbl = new tblAIItem1HourLog() { ItemID = item.ItemID, Value = item.Value, Timestamp = dt, Memo=item.ItemConfig.Lable };
                            db1.tblAIItem1HourLog.Add(tbl);
                            db1.SaveChanges();
                        }
                    }
                    catch { ;}
                }

            }
            //throw new NotImplementedException();
        }
        
        public BindingData.ItemBindingData[] GetAllItemBindingData(int PlaneID)
        {
            //foreach (Item item in Items.Values)
            //{
            //    if (item.PlaneID == PlaneID)
            //    {
            //        Console.WriteLine("find!");
            //    }
            //}

            //return new ItemBindingData[0];
           return Items.Where(n => n.Value.PlaneID == PlaneID).Select(n=>n.Value.ToBindingData()).ToArray();
        }
        void item_ItemValueChanged(Item sender, double NewValue)
        {
           SecureDBEntities1 db = new SecureDBEntities1();
           try
           {
               tblItemConfig tbl = db.tblItemConfig.Where(n => n.ItemID == sender.ItemID).FirstOrDefault();
               if (tbl != null)
                   tbl.Value = NewValue;
               db.SaveChanges();

           }
           catch (Exception ex)
           {
               Console.WriteLine(ex.Message + "," + ex.StackTrace);
           }
           finally
           {
               db.Dispose();
           }
            if(Program.MyServiceObject!=null)
            Program.MyServiceObject.DispatchItemValueChangedEvent(sender.ToBindingData());
           
        }
    }
}

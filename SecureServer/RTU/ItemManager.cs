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
                item.Value = tblitem.Value ?? 0;
                Items.Add(tblitem.ItemID,item);
                item.ItemValueChanged += item_ItemValueChanged;
                item.ItemDegreeChanged += item_ItemDegreeChanged;
               
            }

            OneHourTmr = new ExactIntervalTimer(5, 0);
            OneHourTmr.OnElapsed += OneHourTmr_OnElapsed;
        }

        void item_ItemDegreeChanged(Item sender, int? NewValue)
        {
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
                Program.MyServiceObject.DispatchAlarmEvent(data);
            }
        }

        void OneHourTmr_OnElapsed(object sender)
        {
             DateTime dt=DateTime.Now;
               dt=  dt.AddMinutes(-dt.Minute).AddSeconds(-dt.Second).AddMilliseconds(-dt.Millisecond);
                    
            using (SecureDBEntities1 db = new SecureDBEntities1())
            {
                foreach (Item item in Items.Values)
                {
                    try
                    {
                        if (item.ItemType == "AI")
                        {
                            tblAIItem1HourLog tbl = new tblAIItem1HourLog() { ItemID = item.ItemID, Value = item.Value, Timestamp = dt, Memo=item.ItemConfig.Lable };
                            db.tblAIItem1HourLog.Add(tbl);
                            db.SaveChanges();
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
                tblItemConfig tbl = db.tblItemConfig.FirstOrDefault();
                if (tbl != null)
                    tbl.Value = NewValue;
                db.SaveChanges();
                db.Dispose();
            }
            catch (Exception ex)
            {
              Console.WriteLine(  ex.Message+","+ex.StackTrace);
            }
            
            Program.MyServiceObject.DispatchItemValueChangedEvent(sender.ToBindingData());
           
        }
    }
}

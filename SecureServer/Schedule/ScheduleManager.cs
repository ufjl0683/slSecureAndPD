using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureServer.Schedule
{
    public   class ScheduleManager
    {

        System.Threading.Thread th;
        System.Collections.Generic.Dictionary<long, Schedule> list = new Dictionary<long, Schedule>();
        public ScheduleManager()
        {
            SecureDBEntities1 db = new SecureDBEntities1();

            var q = from n in db.tblSchConfig where n.Enable == true select n;
            foreach (tblSchConfig sch in q)
            {
                if (list.ContainsKey(sch.SchID))
                    continue;
                else
                    list.Add(sch.SchID,new Schedule(sch));
            }

            new System.Threading.Thread(CheckTask).Start();
            
        }


      public   void ScheduleChange(long schid)
        {
           SecureDBEntities1 db = new SecureDBEntities1();
            if (list.ContainsKey(schid))
            {
                tblSchConfig sch = db.tblSchConfig.Where(n => n.SchID == schid).FirstOrDefault();
                if (sch == null || sch.Enable==false )
                {
                    list.Remove(schid);
                    return;
                }
                   
                
                list[schid] = new Schedule(sch);

            }

            else
            {
               
                tblSchConfig sch = db.tblSchConfig.Where(n => n.SchID == schid).FirstOrDefault();
                if (sch == null) return;

                list.Add(schid, new Schedule(sch));

            }

        }

        public void CheckTask()
        {


            while (true)
            {
                foreach (Schedule sch in list.Values)
                {
                    try
                    {
                        sch.CheckSchedule();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

                System.Threading.Thread.Sleep(10000);
            }
        }


    }
}

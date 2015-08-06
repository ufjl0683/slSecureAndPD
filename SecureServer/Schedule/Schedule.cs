using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReportAutoPrint;

namespace SecureServer.Schedule
{
    public  class Schedule : SecureServer.Schedule.ISchedule
    {
        //public int SchID { get; set; }
        //public int ReportID { get; set; }
        //public int SchType { get; set; }
        //public DateTime StartTime { get; set; }
        //public DateTime? NextStartTime { get; set; }
        tblSchConfig config;
        public Schedule( tblSchConfig  config) /*int SchID, int ReportID, int SchType, DateTime StartTime, DateTime? NextStartTime,bool Wee*/ 
                 
        {

            this.config = config;
            //this.SchID = SchID;
            //this.ReportID = ReportID;
            //this.SchType = SchType;
            //this.StartTime = StartTime;
            //this.NextStartTime = NextStartTime;
        }

        public void CheckSchedule()
        {
            if (config.SchType == 0) //執行一次
            {
                
                if (DateTime.Now >= config.StartTime  && config.NextStartTime==null)
                {
                    config.NextStartTime = config.StartTime;
                    SecureServer.SecureDBEntities1 db = new SecureDBEntities1();
                   tblSchConfig sch=   db.tblSchConfig.Where(n => n.SchID == config.SchID).FirstOrDefault();
                   if (sch != null)
                   {
                       sch.NextStartTime = config.NextStartTime;
                     //  sch.Enable = false;
                       db.SaveChanges();
                   }
                 
                   try
                   {
                       ReportAutoPrint.ReportServer.PrintRoport(config.ReportID);
                       Console.WriteLine("invoke rptid:" + config.ReportID);
                       tblSchLog log = new tblSchLog() { TimeStamp = DateTime.Now, ReportID = sch.ReportID, SchID = sch.SchID, Result = true };
                       db.tblSchLog.Add(log);
                       db.SaveChanges();
                   }
                   catch (Exception ex)
                   {
                       tblSchLog log = new tblSchLog() { TimeStamp = DateTime.Now, ReportID = sch.ReportID, SchID = sch.SchID, Result = false,Memo=ex.Message };
                       db.tblSchLog.Add(log);
                       db.SaveChanges();
                   }
                   db.Dispose();
                    
                }
            }
            else if (config.SchType == 1)//重複執行
            {
                DayOfWeek dayofweek = DateTime.Now.DayOfWeek;
                double diffmin=DiffMin( );

                if(diffmin  >=0  && diffmin < 10)
                    DoRepeatSched();
                return;
                 if(dayofweek== DayOfWeek.Sunday && config.Week1==true &&  diffmin  >=0  && diffmin < 10)
                     DoRepeatSched();
                 else if(dayofweek== DayOfWeek.Monday && config.Week2==true &&  diffmin  >=0  && diffmin < 10)
                     DoRepeatSched();

                 else if(dayofweek== DayOfWeek.Tuesday && config.Week3==true &&  diffmin  >=0  && diffmin < 10)
                     DoRepeatSched();

                 else if (dayofweek == DayOfWeek.Wednesday&& config.Week4 == true && diffmin >= 0 && diffmin < 10)
                     DoRepeatSched();

                 else if (dayofweek == DayOfWeek.Thursday && config.Week5 == true && diffmin >= 0 && diffmin < 10)
                     DoRepeatSched();
                 else if (dayofweek == DayOfWeek.Friday && config.Week6 == true && diffmin >= 0 && diffmin < 10)
                     DoRepeatSched();
                 else if (dayofweek == DayOfWeek.Saturday && config.Week7 == true && diffmin >= 0 && diffmin < 10)
                     DoRepeatSched();
            }

        }

        double DiffMin( )
        {
            DateTime dt1  = DateTime.Now;
            DateTime dt2 = new DateTime(dt1.Year, dt1.Month, dt1.Day, config.StartTime.Hour, config.StartTime.Minute, 0);
            return dt1.Subtract(dt2).TotalMinutes;
        }
        void DoRepeatSched()
        {
         
            using (SecureDBEntities1 db = new SecureDBEntities1())
            {

                tblSchConfig sch = db.tblSchConfig.Where(n => n.SchID == config.SchID).FirstOrDefault();
                if (sch.NextStartTime == null || ((DateTime)sch.NextStartTime).DayOfWeek != DateTime.Now.DayOfWeek)
                {
                    try
                    {
                        ReportAutoPrint.ReportServer.PrintRoport(config.ReportID);
                        Console.WriteLine("invoke rptid:" + config.ReportID);
                        tblSchLog log = new tblSchLog() { TimeStamp = DateTime.Now, ReportID = sch.ReportID, SchID = sch.SchID, Result = true };
                        db.tblSchLog.Add(log);
                        //   db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        tblSchLog log = new tblSchLog() { TimeStamp = DateTime.Now, ReportID = sch.ReportID, SchID = sch.SchID, Result = false ,Memo=ex.Message};
                        db.tblSchLog.Add(log);
                     //   db.SaveChanges();
                    }
                     

                    Console.WriteLine("Do report:" + config.ReportID);
                    sch.NextStartTime = DateTime.Now;
                    db.SaveChanges();
                }
            }

        }
        

    }
}

using SecureServer.CardReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureServer.Meter
{
  public class PowerMeterManager
    {
      System.Collections.Generic.List<Meter.R23PowerMeter> list=new List<R23PowerMeter>();
      ExactIntervalTimer OneHourTmr;
      public PowerMeterManager()
      {
          SecureDBEntities1 db=new SecureDBEntities1();
          var q = from n in db.tblPowerMeter select n;
          foreach (tblPowerMeter tbl in q)
          {
              list.Add(new R23PowerMeter(tbl.ERID,tbl.RTU_IP,tbl.Port));

          }
          new System.Threading.Thread(ReadingTask).Start();
          OneHourTmr = new ExactIntervalTimer( 5,0);
          OneHourTmr.OnElapsed += OneHourTmr_OnElapsed;
          
      }

      void OneHourTmr_OnElapsed(object sender)
      {
          //  throw new NotImplementedException();

          foreach (Meter.R23PowerMeter meter in list)
          {
              try
              {
                  SecureDBEntities1 db = new SecureDBEntities1();
                  if (!meter.IsValid)
                      continue;
                  tblPowerMeter tbl = db.tblPowerMeter.Where(n => n.ERID == meter.ERID).FirstOrDefault();
                   
                  if (tbl != null)
                  {
                      DateTime dt=DateTime.Now;
                      tblPowerMeter1HourLog lastlog=  db.tblPowerMeter1HourLog.Where(n=>n.ERID==tbl.ERID).OrderByDescending(n=>n.Timestamp).Take(1).FirstOrDefault();
                     
                      db.tblPowerMeter1HourLog.Add(new tblPowerMeter1HourLog()
                      {
                           ERID=tbl.ERID,
                            KW=tbl.KW, 
                             Timestamp=new DateTime(dt.Year,dt.Month,dt.Day,dt.Hour,0,0),
                              CumulateValue=(lastlog==null)?tbl.CumulateValue:tbl.CumulateValue-lastlog.CumulateValue,
                              
                       
                      }
                      );

                      db.SaveChanges();
                      //tbl.VA = meter.VA;
                      //tbl.VB = meter.VB;
                      //tbl.VC = meter.VC;
                      //tbl.AVGV = meter.AVGV;
                      //tbl.IA = meter.IA;
                      //tbl.IB = meter.IB;
                      //tbl.IC = meter.IC;
                      //tbl.AVGI = meter.AVGI;
                      //tbl.KW = meter.KW;
                      //tbl.PF = meter.PF;
                      //tbl.CumulateValue = meter.CumulateValue;
                      //tbl.InstantaneousValue = meter.InstantaneousValue;
                      //tbl.UpdateDate = DateTime.Now;
                      //db.SaveChanges();
                      //db.Dispose();
                  }

              }
              catch (Exception ex)
              {
                  Console.WriteLine(ex.Message + "," + ex.StackTrace);
              }
          }

      }

      void ReadingTask()
      {
           

          while (true)
          {
            
              foreach (Meter.R23PowerMeter meter in list)
              {
                  try
                  {
                      SecureDBEntities1 db = new SecureDBEntities1();
                      if (!meter.IsValid)
                          continue;
                      tblPowerMeter tbl = db.tblPowerMeter.Where(n => n.ERID == meter.ERID).FirstOrDefault();
                      if (tbl != null)
                      {
                          tbl.VA = meter.VA;
                          tbl.VB = meter.VB;
                          tbl.VC = meter.VC;
                          tbl.AVGV = meter.AVGV;
                          tbl.IA = meter.IA;
                          tbl.IB = meter.IB;
                          tbl.IC = meter.IC;
                          tbl.AVGI = meter.AVGI;
                          tbl.KW = meter.KW;
                          tbl.PF = meter.PF;
                          tbl.CumulateValue = meter.CumulateValue;
                          tbl.InstantaneousValue = meter.InstantaneousValue;
                          tbl.UpdateDate = DateTime.Now;
                          db.SaveChanges();
                          db.Dispose();
                      }

                  }
                  catch (Exception ex)
                  {
                      Console.WriteLine(ex.Message + "," + ex.StackTrace);
                  }

              }
              System.Threading.Thread.Sleep(10 * 60 * 1000);
          }
      }

    }
}

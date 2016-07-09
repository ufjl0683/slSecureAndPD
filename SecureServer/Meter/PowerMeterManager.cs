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
          OneHourTmr = new ExactIntervalTimer( 10, 0);
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
                      DateTime lastbeg=DateTime.Now.Subtract(TimeSpan.FromHours(24 * 30));
                      tblPowerMeter1HourLog lastlog=  db.tblPowerMeter1HourLog.Where(n=>n.ERID==tbl.ERID).OrderByDescending(n=>n.Timestamp).Take(1).FirstOrDefault();
                      double? PowerAvg = db.tblPowerMeter1HourLog.Where(n =>/* n.Timestamp >= lastbeg && */ n.ERID==tbl.ERID ).Average(n => n.KW);
                      double? WaterAvg = db.tblPowerMeter1HourLog.Where(n => /*n.Timestamp >= lastbeg  && */ n.ERID==tbl.ERID ).Average(n => n.WaterConsume);
                      double? Kw24avg = db.tblPowerMeter1HourLog.Where(n=>n.ERID==tbl.ERID).OrderByDescending(n => n.Timestamp ).Take(24).Average(n=>n.KW);
                      
                      double? wateCurrent=(tbl.CumulateValue-lastlog.CumulateValue)/ (new DateTime(dt.Year,dt.Month,dt.Day,dt.Hour,0,0)-lastlog.Timestamp).Hours;
                      
                      bool PowerAlarm=false,WaterAlarm=false;
                                           
                      //if(PowerAvg!=null  &&  (tbl.KW> PowerAvg*(1+tbl.PowerAlarmUpper/100) || tbl.KW< PowerAvg*(1-tbl.PowerAlarmLower/100 )) )
                      if (PowerAvg != null && (Kw24avg > PowerAvg * (1 + tbl.PowerAlarmUpper / 100) || Kw24avg < PowerAvg * (1 - tbl.PowerAlarmLower / 100)))
                            PowerAlarm=true;
                      if(WaterAvg!=null  &&  (wateCurrent> WaterAvg*(1+tbl.WaterAlarmUpper/100) /* || wateCurrent< WaterAvg*(1-tbl.WaterAlarmLower/100 )*/) )
                          WaterAlarm=true;

                      tbl.PowerAlarm = PowerAlarm;
                      tbl.WaterAlarm = WaterAlarm;
                      bool PowerAlarmChanged, WaterAlarmChanged;
                      PowerAlarmChanged = (tbl.PowerAlarm ?? false) ^ PowerAlarm;
                      WaterAlarmChanged = (tbl.WaterAlarm ?? false) ^ WaterAlarm;
                      tbl.KW24Avg = Kw24avg;
                      tblPowerMeter1HourLog log = new tblPowerMeter1HourLog()
                     {
                         ERID = tbl.ERID,
                         KW = tbl.KW,
                         Timestamp = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0),
                         CumulateValue = tbl.CumulateValue,  //(lastlog==null)?tbl.CumulateValue:tbl.CumulateValue-lastlog.CumulateValue,
                         WaterAlarm = WaterAlarm,
                         PowerAlarm = PowerAlarm,
                         PowerAlarmAvg = PowerAvg ?? tbl.KW,
                         WaterAlarmAvg = WaterAvg ?? ((lastlog == null) ? tbl.CumulateValue : tbl.CumulateValue - lastlog.CumulateValue),
                         WaterConsume = (lastlog == null) ? tbl.CumulateValue : tbl.CumulateValue - lastlog.CumulateValue,
                          KW24Avg=Kw24avg


                     };
                      db.tblPowerMeter1HourLog.Add(log);

                      tbl.WaterConsume = log.WaterConsume;
                      tbl.WaterAlarmAvg = log.WaterAlarmAvg;
                      tbl.PowerAlarmAvg = log.PowerAlarmAvg;

                      if ((log.PowerAlarm ?? false) && log.KW > log.PowerAlarmAvg)
                          tbl.PowerAlarmDesc = "用電量過高";
                      else if ((log.PowerAlarm ?? false) && log.KW <= log.PowerAlarmAvg)
                          tbl.PowerAlarmDesc = "用電量過低 ";

                      if ((log.WaterAlarm ?? false) && log.WaterConsume > log.WaterAlarmAvg)
                          tbl.WaterAlarmDesc = "用水量過高";
                      else if ((log.WaterAlarm ?? false) && log.WaterConsume <= log.WaterAlarmAvg)
                          tbl.WaterAlarmDesc = "用水量過低 ";

                      if (!(log.PowerAlarm ?? false))
                          tbl.PowerAlarmDesc = "";
                      if (!(log.WaterAlarm ?? false))
                          tbl.WaterAlarmDesc = "";
                      db.SaveChanges();

                      if (PowerAlarm /*&& PowerAlarmChanged*/)
                      {
                          AlarmData data = new AlarmData()
                          {
                              AlarmType = AlarmType.PowerMeter,
                              ColorString = "Red",
                              TimeStamp = DateTime.Now,
                              PlaneName = tbl.ERName,
                              TimeStampString = string.Format("HH:mm"),
                               IsForkCCTVEvent=false,
                                Description=tbl.PowerAlarmDesc

                          };

                          Program.MyServiceObject.DispatchAlarmEvent(data);
                         
                          
                      }

                      if (WaterAlarm /*&& WaterAlarmChanged*/)
                      {
                          AlarmData data = new AlarmData()
                          {
                              AlarmType = AlarmType.WaterMeter,
                              ColorString = "Red",
                              TimeStamp = DateTime.Now,
                              PlaneName = tbl.ERName,
                              TimeStampString = string.Format("HH:mm"),
                              IsForkCCTVEvent=false,
                              Description=tbl.WaterAlarmDesc
                          };

                          Program.MyServiceObject.DispatchAlarmEvent(data);


                      }


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

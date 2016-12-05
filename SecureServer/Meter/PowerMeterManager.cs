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
         // OneHourTmr_OnElapsed(null);
          new System.Threading.Thread(ReadingTask).Start();
           
          OneHourTmr = new ExactIntervalTimer(10,0);
          OneHourTmr.OnElapsed += OneHourTmr_OnElapsed;
          
      }


      public double GetWaterBase(int erid)
      {
          using (SecureDBEntities1 db = new SecureDBEntities1())
          {
              tblPowerMeter meter = db.tblPowerMeter.Where(n => n.ERID == erid).FirstOrDefault();
              int month = DateTime.Now.Month;
              if (month == 1)
                  return meter.Water1MonthDailyAvg ?? 0;
              else if (month == 2)
                  return meter.Water2MonthDailyAvg ?? 0;
              else if (month == 3)
                  return meter.Water3MonthDailyAvg ?? 0;
              else if (month == 4)
                  return meter.Water4MonthDailyAvg ?? 0;
              else if (month == 5)
                  return meter.Water5MonthDailyAvg ?? 0;
              else if (month == 6)
                  return meter.Water6MonthDailyAvg ?? 0;
              else if (month == 7)
                  return meter.Water7MonthDailyAvg ?? 0;
              else if (month == 8)
                  return meter.Water8MonthDailyAvg ?? 0;
              else if (month == 9)
                  return meter.Water9MonthDailyAvg ?? 0;
              else if (month == 10)
                  return meter.Water10MonthDailyAvg ?? 0;
              else if (month == 11)
                  return meter.Water11MonthDailyAvg ?? 0;
              else
                  return meter.Water12MonthDailyAvg ?? 0;
          }
          


      }
      public double GetPowerBase(int erid)
      {
          using (SecureDBEntities1 db = new SecureDBEntities1())
          {
              tblPowerMeter meter = db.tblPowerMeter.Where(n => n.ERID == erid).FirstOrDefault();
              int month = DateTime.Now.Month;
              if (month == 1)
                  return meter.Power1MonthDailyAvg ?? 0;
              else if (month == 2)
                  return meter.Power2MonthDailyAvg ?? 0;
              else if (month == 3)
                  return meter.Power3MonthDailyAvg ?? 0;
              else if (month == 4)
                  return meter.Power4MonthDailyAvg ?? 0;
              else if (month == 5)
                  return meter.Power5MonthDailyAvg ?? 0;
              else if (month == 6)
                  return meter.Water6MonthDailyAvg ?? 0;
              else if (month == 7)
                  return meter.Water7MonthDailyAvg ?? 0;
              else if (month == 8)
                  return meter.Power8MonthDailyAvg ?? 0;
              else if (month == 9)
                  return meter.Power9MonthDailyAvg ?? 0;
              else if (month == 10)
                  return meter.Power10MonthDailyAvg ?? 0;
              else if (month == 11)
                  return meter.Power11MonthDailyAvg ?? 0;
              else
                  return meter.Power12MonthDailyAvg ?? 0;
          }

      }

      double? GetLastHourPowerBase(int erid)
      {
          using (SecureDBEntities1 db = new SecureDBEntities1())
          {
              DateTime dt = DateTime.Now - TimeSpan.FromHours(1);
              dt=new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0);
               tblPowerMeter1HourLog log= db.tblPowerMeter1HourLog.Where(n => n.Timestamp ==dt   && n.ERID==erid).FirstOrDefault();
               if (log == null)
                   return null;

               return log.PowerConsume;
          }
      }

      double? GetLastDayPowerBase(int erid)
      {
          using (SecureDBEntities1 db = new SecureDBEntities1())
          {
              DateTime dt = DateTime.Now - TimeSpan.FromDays(1);
              dt=new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0);


                tblPowerMeter1HourLog log = db.tblPowerMeter1HourLog.Where(n => n.Timestamp <= dt && n.ERID == erid).OrderByDescending(n => n.Timestamp).Take(1).FirstOrDefault();
                if (log == null)
                    return null;

                return log.Power24Consume ?? 0;
              
          }
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
                    //  DateTime lastbeg=DateTime.Now.Subtract(TimeSpan.FromHours(24 ));
                      tblPowerMeter1HourLog lastlog=  db.tblPowerMeter1HourLog.Where(n=>n.ERID==tbl.ERID).OrderByDescending(n=>n.Timestamp).Take(1).FirstOrDefault();

                        double? wateCurrent = (tbl.CumulateValue - lastlog.CumulateValue);
                        double? PowerCurrent = ((tbl.kWh ?? 0) - (lastlog.kWh ?? 0));
                        bool IsModify = false;
                        if (wateCurrent <-1|| wateCurrent > 100)
                        {
                            wateCurrent = lastlog.WaterConsume;
                            tbl.CumulateValue=lastlog.CumulateValue + lastlog.WaterConsume;
                            IsModify = true;
                        }

                        if (PowerCurrent < 0 || PowerCurrent > 5000)
                        {
                            PowerCurrent = lastlog.PowerConsume;
                            tbl.kWh    = lastlog.kWh + lastlog.PowerConsume;
                            IsModify = true;
                        }


                        double? PowerBase = GetPowerBase(tbl.ERID);          //db.tblPowerMeter1HourLog.Where(n =>/* n.Timestamp >= lastbeg && */ n.ERID==tbl.ERID ).Average(n => n.KW);
                      double? WaterBase = GetWaterBase(tbl.ERID);//db.tblPowerMeter1HourLog.Where(n => /*n.Timestamp >= lastbeg  && */ n.ERID==tbl.ERID ).Average(n => n.WaterConsume);
                      double? Kwh24Hour= db.tblPowerMeter1HourLog.Where(n=>n.ERID==tbl.ERID).OrderByDescending(n => n.Timestamp ).Take(23).Sum(n=>n.PowerConsume)+PowerCurrent;  //db.tblPowerMeter1HourLog.Where(n=>n.ERID==tbl.ERID).OrderByDescending(n => n.Timestamp ).Take(24).Average(n=>n.KW);
                      double? water24Hour = db.tblPowerMeter1HourLog.Where(n => n.ERID == tbl.ERID).OrderByDescending(n => n.Timestamp).Take(23).Sum(n => n.WaterConsume)+wateCurrent; 
                     
                      double? PowerLastDayBase = GetLastDayPowerBase(tbl.ERID);
                      double? PowerLastHourBase = GetLastHourPowerBase(tbl.ERID);

                      bool PowerAlarm = false, WaterAlarm = false, PowerLastDayAlarm = false, PowerLastHourAlarm = false ;
                                           
                      //if(PowerAvg!=null  &&  (tbl.KW> PowerAvg*(1+tbl.PowerAlarmUpper/100) || tbl.KW< PowerAvg*(1-tbl.PowerAlarmLower/100 )) )
                      if (PowerBase != null && (Kwh24Hour > PowerBase * (1 + tbl.PowerAlarmUpper / 100) || Kwh24Hour < PowerBase * (1 - tbl.PowerAlarmLower / 100)))
                            PowerAlarm=true;
                      if (WaterBase != null && (water24Hour > WaterBase * (1 + tbl.WaterAlarmUpper / 100) /* || wateCurrent< WaterAvg*(1-tbl.WaterAlarmLower/100 )*/))
                          WaterAlarm=true;

                      if(PowerLastDayBase!=null &&  (Kwh24Hour > PowerLastDayBase * (1 + tbl.PowerAlarmUpper / 100) || Kwh24Hour < PowerLastDayBase * (1 - tbl.PowerAlarmLower / 100)))
                          PowerLastDayAlarm=true;

                      if (PowerLastHourBase != null && (PowerCurrent > PowerLastHourBase * (1 + tbl.PowerAlarmUpper / 100) || PowerCurrent < PowerLastHourBase * (1 - tbl.PowerAlarmLower / 100)))
                          PowerLastHourAlarm = true;

                      tbl.PowerAlarm = PowerAlarm;
                      tbl.WaterAlarm = WaterAlarm;
                      bool PowerAlarmChanged, WaterAlarmChanged;
                      PowerAlarmChanged = (tbl.PowerAlarm ?? false) ^ PowerAlarm;
                      WaterAlarmChanged = (tbl.WaterAlarm ?? false) ^ WaterAlarm;
                     // tbl.KW24Avg = Kw24avg;
                      tblPowerMeter1HourLog log = new tblPowerMeter1HourLog()
                     {
                         ERID = tbl.ERID,
                         KW = tbl.KW,
                         Timestamp = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0),
                         CumulateValue = tbl.CumulateValue,  //(lastlog==null)?tbl.CumulateValue:tbl.CumulateValue-lastlog.CumulateValue,
                         WaterAlarm = WaterAlarm,
                         PowerAlarm = (PowerLastDayAlarm || PowerLastHourAlarm || PowerAlarm),
                        // PowerAlarm = PowerAlarm,
                        // PowerAlarmAvg = PowerAvg ?? tbl.KW,
                        // WaterAlarmAvg = WaterAvg ?? ((lastlog == null) ? tbl.CumulateValue : tbl.CumulateValue - lastlog.CumulateValue),
                         WaterConsume = wateCurrent,//(lastlog == null) ? tbl.CumulateValue : tbl.CumulateValue - lastlog.CumulateValue,
                          //KW24Avg=Kw24avg,
                         PowerConsume =PowerCurrent, //(lastlog == null) ? tbl.PowerConsume: tbl.kWh - (lastlog.PowerConsume??0),
                          PowerAlarm_LastHour=(short)(PowerLastHourAlarm?(PowerCurrent>PowerLastHourBase?2:1):0),
                         PowerAlarm_LastYear = (short)(PowerAlarm ?  (Kwh24Hour > PowerBase ? 2 : 1):0),
                         PowerAlarm_Yesterday=(short)(PowerLastDayAlarm ?  (Kwh24Hour > PowerLastDayBase ? 2 : 1):0),
                          Power24Consume=Kwh24Hour,
                           kWh=tbl.kWh,
                         PowerAlarmBaseValue_LastHour=PowerLastHourBase,
                         PowerAlarmBaseValue_LastYear=PowerBase,
                          PowerAlarmBaseValue_Yesterday=PowerLastDayBase,
                           WaterAlarmBaseValue_LastYear=WaterBase,
                            WaterAlarm_LastYear=(short)(WaterAlarm ?  (water24Hour > WaterBase ? 2 : 1):0),
                              Water24Consume=water24Hour,
                               
                                Memo=(IsModify?"Modify":null)
                            
                            


                     };
                      db.tblPowerMeter1HourLog.Add(log);

                      tbl.WaterConsume = log.WaterConsume;
                      tbl.WaterAlarm_LastYear = log.WaterAlarm_LastYear;
                      tbl.Water24Consume = log.Water24Consume;
                      tbl.WaterAlarmBaseValue_LastYear = log.WaterAlarmBaseValue_LastYear;
                      tbl.WaterAlarmAvg = log.WaterAlarmAvg;
                      tbl.PowerAlarmAvg = log.PowerAlarmAvg;
                      tbl.Power24Consume = log.Power24Consume;
                      tbl.PowerAlarm = log.PowerAlarm;
                      tbl.PowerAlarm_LastHour = log.PowerAlarm_LastHour;
                      tbl.PowerAlarm_LastYear = log.PowerAlarm_LastYear;
                      tbl.PowerAlarm_Yesterday = log.PowerAlarm_Yesterday;
                      tbl.PowerAlarmBaseValue_LastHour = log.PowerAlarmBaseValue_LastHour;
                      tbl.PowerAlarmBaseValue_LastYear = log.PowerAlarmBaseValue_LastYear;
                      tbl.PowerAlarmBaseValue_Yesterday = log.PowerAlarmBaseValue_Yesterday;
                      tbl.PowerConsume = log.PowerConsume;
                     
                    



                      if ((log.PowerAlarm ?? false) && (log.PowerAlarm_LastHour == 2 || log.PowerAlarm_LastYear == 2 || log.PowerAlarm_Yesterday == 2))
                          tbl.PowerAlarmDesc = "用電量過高";
                      else if ((log.PowerAlarm ?? false) && (log.PowerAlarm_LastHour == 1 || log.PowerAlarm_LastYear == 1 || log.PowerAlarm_Yesterday == 1))
                          tbl.PowerAlarmDesc = "用電量過低";

                      if ((log.WaterAlarm ?? false) && log.WaterAlarm_LastYear==2)
                          tbl.WaterAlarmDesc = "用水量過高";
                      else if ((log.WaterAlarm ?? false) && log.WaterAlarm_LastYear==1)
                          tbl.WaterAlarmDesc = "用水量過低 ";
                      //if ((log.PowerAlarm ?? false) && log.KW > log.PowerAlarmAvg)
                      //    tbl.PowerAlarmDesc = "用電量過高";
                      //else if ((log.PowerAlarm ?? false) && log.KW <= log.PowerAlarmAvg)
                      //    tbl.PowerAlarmDesc = "用電量過低 ";

                      //if ((log.WaterAlarm ?? false) && log.WaterConsume > log.WaterAlarmAvg)
                      //    tbl.WaterAlarmDesc = "用水量過高";
                      //else if ((log.WaterAlarm ?? false) && log.WaterConsume <= log.WaterAlarmAvg)
                      //    tbl.WaterAlarmDesc = "用水量過低 ";

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
                          tbl.kWh = meter.kwh;
                          
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

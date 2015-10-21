using ModbusTCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SecureServer.PD
{
   public  class PD
    {


        string IP;
       int Port;

       Master RTUDevice;
       System.Threading.Timer tmr;
       byte[] data=new byte[2];
       string PDName;
       object lockobj = new object();
       tblPDConfig tblPDConfig;
       System.Collections.BitArray bits;
       public PD(string  PDName,string IP, int Port, tblPDConfig tblpdconfig)
       {
           this.IP = IP;
           this.Port = Port;
           //bits = new System.Collections.BitArray(data);
           this.tblPDConfig = tblpdconfig;
           initValue();
           //try
           //{
           //    RTUDevice = new ModbusTCP.Master(IP, (ushort)Port);
           //}
           //catch { ;}
           new Thread(ConnectTask).Start();
           tmr = new System.Threading.Timer(new System.Threading.TimerCallback(timerBack));
           tmr.Change(0, 5000);
           new Thread(ReadingTask).Start();
           this.PDName = PDName;
         
         


       }
       void initValue()
       {
           _IsConnected = ((tblPDConfig.Comm_state??0) == 1);

           R0 = this.tblPDConfig.R0??0;
           S0 = this.tblPDConfig.S0 ?? 0;
           T0 = this.tblPDConfig.T0 ?? 0;

           R1 = this.tblPDConfig.R1 ?? 0;
           S1 = this.tblPDConfig.S1 ?? 0;
           T1 = this.tblPDConfig.T1 ?? 0;

           if((this.tblPDConfig.L0??0)!=-1)
                  L0 = this.tblPDConfig.L0 ?? 0;

           if ((this.tblPDConfig.L1 ?? 0) != -1)
               L1 = this.tblPDConfig.L1 ?? 0;

           if ((this.tblPDConfig.L2 ?? 0) != -1)
               L2 = this.tblPDConfig.L2 ?? 0;

            if ((this.tblPDConfig.L3 ?? 0) != -1)
               L3 = this.tblPDConfig.L3 ?? 0;


            if ((this.tblPDConfig.L4 ?? 0) != -1)
                L4 = this.tblPDConfig.L4 ?? 0;

            Cabinet = tblPDConfig.Cabinet??0;
             
       }

       bool _IsConnected;
       public bool IsConnected
       {
           get
           {
               //if (RTUDevice == null || !RTUDevice.connected)
               //    return false;
               //else
               //    return true;
               return _IsConnected;
           }

           set
           {
               _IsConnected = value;
           }
            
       }

       public int R0
       {
           get
           {
              // if((tblPDConfig.type??1)==1)
               return (System.BitConverter.ToUInt16(data, 0)) & 0x01;
               //else if((tblPDConfig.type??1)==2)   //R 11
               //    return (System.BitConverter.ToUInt16(data, 0) >> 4) & 0x01;  
               //else
               //  return (System.BitConverter.ToUInt16(data, 0)) & 0x01;
           }

           set
           {
               if (value == 1)

                   data[0] |= (1  );
               else
                   data[0] &= unchecked((byte)(~(1 )));



           }
       }

       public int S0
       {
           get
           {
               return (System.BitConverter.ToUInt16(data, 0) >> 1) & 0x01;
           }


           set
           {
               if (value == 1)

                   data[0] |= (1 << 1);
               else
                   data[0] &= unchecked((byte)(~(1 << 1)));



           }
       }
       public int T0
       {
           get
           {
               return (System.BitConverter.ToUInt16(data, 0) >> 2) & 0x01;
           }

           set
           {
               if (value == 1)

                   data[0] |= (1 << 2);
               else
                   data[0] &= unchecked((byte)(~(1 << 2)));

               

           }
       }
       public int R1
       {
           get
           {
               return (System.BitConverter.ToUInt16(data, 0) >> 3) & 0x01;
           }

           set
           {
               if (value == 1)

                   data[0] |= (1 << 3);
               else
                   data[0] &= unchecked((byte)(~(1 << 3)));



           }
       }

       public int S1
       {
           get
           {
               return (System.BitConverter.ToUInt16(data, 0) >> 4) & 0x01;
           }
           set
           {
               if (value == 1)

                   data[0] |= (1 << 4);
               else
                   data[0] &= unchecked((byte)(~(1 << 4)));



           }
       }
       public int T1
       {
           get
           {
               return (System.BitConverter.ToUInt16(data, 0) >> 5) & 0x01;
           }
           set
           {
               if (value == 1)

                   data[0] |= (1 << 5);
               else
                   data[0] &= unchecked((byte)(~(1 << 5)));



           }
       }

       public int L0
       {
           get
           {
               return (System.BitConverter.ToUInt16(data, 0) >>  6) & 0x01;
           }

           set
           {
               if (value == -1) return;
               if (value == 1)

                   data[0] |= (1 << 6);
               else
                   data[0] &= unchecked((byte)(~(1 << 6)));



           }
       }
       public int L1
       {
           get
           {
               return (System.BitConverter.ToUInt16(data, 0) >> 7) & 0x01;
           }

           set
           {
               if (value == -1) return;
               if (value == 1)

                   data[0] |= (1 << 7);
               else
                   data[0] &= unchecked((byte)(~(1 << 7)));



           }
       }
       
       public int L2
       {
           get
           {
               return (System.BitConverter.ToUInt16(data, 0) >> 8) & 0x01;
           }
           set
           {
               if (value == -1) return;
               if (value == 1)

                   data[1] |= (1 );
               else
                   data[1] &= unchecked((byte)(~(1 )));



           }
       }
       public int L3
       {
           get
           {
               return (System.BitConverter.ToUInt16(data, 0) >> 9) & 0x01;
           }
           set
           {
               if (value == -1) return;
               if (value == 1)

                   data[1] |= (1 << 1);
               else
                   data[1] &= unchecked((byte)(~(1 << 1)));



           }
       }
       public int L4
       {
           get
           {
               return (System.BitConverter.ToUInt16(data, 0) >> 10) & 0x01;
           }

           set
           {
               if (value == -1) return;
               if (value == 1)

                   data[1] |= (1 << 2);
               else
                   data[1] &= unchecked((byte)(~(1 << 2)));



           }
       }
       public int Cabinet
       {
           get
           {
               return (System.BitConverter.ToUInt16(data, 0) >> 11) & 0x01;
           }
           set
           {
               if (value == -1) return;
               if (value == 1)

                   data[1] |= (1 << 3);
               else
                   data[1] &= unchecked((byte)(~(1 << 3)));



           }
       }

       void ReadingTask()
       {
           byte[] tempdata = new byte[data.Length];
           while (true)
           {

               try
               {
                   if (RTUDevice.connected != IsConnected)
                   {
                       IsConnected = RTUDevice.connected;
                      using( SecureDBEntities1 db = new SecureDBEntities1())
                       {
                           tblPDConfig pdc = db.tblPDConfig.Where(n => n.PDName == this.PDName).FirstOrDefault();
                           if (pdc != null)
                           {
                               pdc.Comm_state = IsConnected ? 1 : 0;
                               db.SaveChanges();
                           }

                       }
                   }

                   if (RTUDevice != null && RTUDevice.connected)
                   {
                       lock (lockobj)
                       {
                         
                                RTUDevice.ReadDiscreteInputs(1, 0, 0, 12, ref tempdata);


                                if ((tblPDConfig.type ?? 1) == 2)  //R11
                                {
                                    for (int i = 0; i < tempdata.Length; i++)
                                        tempdata[i] = (byte)(~tempdata[i]);

                                    System.Collections.BitArray baryD = new System.Collections.BitArray(new byte[2]);
                                    System.Collections.BitArray baryS = new System.Collections.BitArray(tempdata);
                                    baryD.Set(0, baryS.Get(4));  //r0
                                    baryD.Set(1, baryS.Get(5));  //s0
                                    baryD.Set(2, baryS.Get(6));  // t0
                                    baryD.Set(3, baryS.Get(0));  //r1
                                    baryD.Set(4, baryS.Get(1)); //s1
                                    baryD.Set(5, baryS.Get(2));  //t1
                                    baryD.Set(6, baryS.Get(7));  // L0
                                    baryD.Set(7, baryS.Get(8));  // L1
                                    baryD.Set(8, baryS.Get(9));  // L2
                                    baryD.Set(9, baryS.Get(10));  // L3
                                    baryD.Set(10, baryS.Get(11));  // L4
                                    baryD.Set(11, baryS.Get(3));  // cab
                                    baryD.CopyTo(tempdata, 0);
                                }
                                else if ((tblPDConfig.type ?? 1) == 3) //R12
                                {

                                    for (int i = 0; i < tempdata.Length; i++)
                                        tempdata[i] = (byte)(~tempdata[i]);

                                    System.Collections.BitArray baryD = new System.Collections.BitArray(new byte[2]);
                                    System.Collections.BitArray baryS = new System.Collections.BitArray(tempdata);
                                    baryD.Set(0, baryS.Get(6));  //r0
                                    baryD.Set(1, baryS.Get(7));  //s0
                                    baryD.Set(2, baryS.Get(8));  // t0
                                    baryD.Set(3, baryS.Get(0));  //r1
                                    baryD.Set(4, baryS.Get(1)); //s1
                                    baryD.Set(5, baryS.Get(2));  //t1
                                    baryD.Set(6, baryS.Get(9));  // L0
                                    baryD.Set(7, baryS.Get(10));  // L1
                                    baryD.Set(8, baryS.Get(11));  // L2
                                    //baryD.Set(9, 0baryS.Get(10));  // L3
                                    //baryD.Set(10,0 baryS.Get(11));  // L4
                                    baryD.Set(11, baryS.Get(3) && baryS.Get(4) && baryS.Get(5));  // cab
                                    baryD.CopyTo(tempdata, 0);
                                }

                                else if ((tblPDConfig.type ?? 1) == 4) //R23 T74
                                {
                                    System.Collections.BitArray baryD = new System.Collections.BitArray(tempdata);
                                    baryD.Set(11,! baryD.Get(11));
                                    baryD.CopyTo(tempdata, 0);
                                }

                                
                           //  RTUDevice.ReadHoldingRegister((ushort)this.DevID, (byte)255, (ushort)(StartAddress - 1), this.RegisterLength, ref tempdata);
                           if (tempdata != null && tempdata.Length != 0)
                           {
                               CheckDataChange(tempdata);
                               System.Array.Copy(tempdata, data, 2);
                               //for (int i = 0; i < tempdata.Length; i++)
                               //{


                               //    data[i] = tempdata[i];
                               //}
                           }
                       }
                   }


                   

               }
               catch (Exception ex)
               {
                   Console.WriteLine(ex.Message + "," + ex.StackTrace);
               }
               finally
               {
                   System.Threading.Thread.Sleep(1000);
               }

           }
       }

       void CheckDataChange(byte[] temp)
       {
           string description="";
           
           if (data[0] == temp[0] && data[1] == temp[1])
               return;

           SecureDBEntities1 db = new SecureDBEntities1();
           tblPDConfig tblpd=  db.tblPDConfig.Where(n=>n.PDName==this.PDName).FirstOrDefault();
           if(tblpd==null)
               return;
           PDStatus d= new PDStatus(data);
           PDStatus t = new PDStatus(temp);
          
           if (d.R0 != t.R0   )
           {
               tblpd.R0 = t.R0;
               if (t.R0 == 0)  //normal
               {
                   tblPDAlarmLog log = new tblPDAlarmLog() { PDItem = "R0", Timestamp = DateTime.Now, PDName = this.PDName, Status = 1 };
                   db.tblPDAlarmLog.Add(log);
               }
               else  //abnormal
               {
                   tblPDAlarmLog log = new tblPDAlarmLog() { PDItem = "R0", Timestamp = DateTime.Now, PDName = this.PDName, Status = 0 };
                   db.tblPDAlarmLog.Add(log);
                   description += "R0 ";
               }
           }
       
           if (d.S0 != t.S0)
           {
               tblpd.S0 = t.S0;

               if (t.S0 == 0)
               {
                   tblPDAlarmLog log = new tblPDAlarmLog() { PDItem = "S0", Timestamp = DateTime.Now, PDName = this.PDName,Status=1 };
                   db.tblPDAlarmLog.Add(log);
               }
               else
               {

                   tblPDAlarmLog log = new tblPDAlarmLog() { PDItem = "S0", Timestamp = DateTime.Now, PDName = this.PDName, Status = 0 };
                   db.tblPDAlarmLog.Add(log);

                   description += "S0 ";
               }
           }
           if (d.T0 != t.T0  )
           {
               tblpd.T0  =t.T0;
               if (t.T0 == 0)
               {
                   tblPDAlarmLog log = new tblPDAlarmLog() { PDItem = "T0", Timestamp = DateTime.Now, PDName = this.PDName, Status = 1 };
                   db.tblPDAlarmLog.Add(log);
               }
               else
               {
                   tblPDAlarmLog log = new tblPDAlarmLog() { PDItem = "T0", Timestamp = DateTime.Now, PDName = this.PDName, Status = 0 };
                   db.tblPDAlarmLog.Add(log);
                   description += "T0 ";
               }
           }

           if (d.R1 != t.R1)
           {
               tblpd.R1 = t.R1;
               if (t.R1 == 0)
               {
                   tblPDAlarmLog log = new tblPDAlarmLog() { PDItem = "R1", Timestamp = DateTime.Now, PDName = this.PDName, Status = 1 };
                   db.tblPDAlarmLog.Add(log);
               }
               else
               {
                   tblPDAlarmLog log = new tblPDAlarmLog() { PDItem = "R1", Timestamp = DateTime.Now, PDName = this.PDName, Status = 0 };
                   db.tblPDAlarmLog.Add(log);
                   description += "R1 ";
               }
           }

           if (d.S1 != t.S1)
           {
               tblpd.S1 = t.S1;

               if (t.S1 == 0)
               {
                   tblPDAlarmLog log = new tblPDAlarmLog() { PDItem = "S1", Timestamp = DateTime.Now, PDName = this.PDName , Status=1};
                   db.tblPDAlarmLog.Add(log);
               }
               else
               {
                   tblPDAlarmLog log = new tblPDAlarmLog() { PDItem = "S1", Timestamp = DateTime.Now, PDName = this.PDName, Status = 0 };
                   db.tblPDAlarmLog.Add(log);
                   description += "S1 ";
               }

           }
           if (d.T1 != t.T1)
           {
               tblpd.T1 = t.T1;

               if (t.T1 == 0)
               {
                   tblPDAlarmLog log = new tblPDAlarmLog() { PDItem = "T1", Timestamp = DateTime.Now, PDName = this.PDName, Status = 1 };
                   db.tblPDAlarmLog.Add(log);
               }
               else
               {
                   tblPDAlarmLog log = new tblPDAlarmLog() { PDItem = "T1", Timestamp = DateTime.Now, PDName = this.PDName, Status = 0 };
                   db.tblPDAlarmLog.Add(log);
                   description += "T1 ";

               }

           }

          if(tblPDConfig.L0!=-1)
               if (d.L0 != t.L0  )
               {
                   tblpd.L0 = t.L0;

                   if (t.L0 == 0)
                   {
                       tblPDAlarmLog log = new tblPDAlarmLog() { PDItem = "L0", Timestamp = DateTime.Now, PDName = this.PDName,Status=1 };
                       db.tblPDAlarmLog.Add(log);
                   }
                   else
                   {
                       tblPDAlarmLog log = new tblPDAlarmLog() { PDItem = "L0", Timestamp = DateTime.Now, PDName = this.PDName, Status = 0 };
                       db.tblPDAlarmLog.Add(log);
                       description += "L0 ";

                   }
               }


          if (tblPDConfig.L1 != -1)
              if (d.L1 != t.L1)
              {
                  tblpd.L1 = t.L1;
                  if (t.L1 == 0)
                  {
                      tblPDAlarmLog log = new tblPDAlarmLog() { PDItem = "L1", Timestamp = DateTime.Now, PDName = this.PDName,Status=1 };
                       db.tblPDAlarmLog.Add(log);
                  }
                  else
                  {
                      tblPDAlarmLog log = new tblPDAlarmLog() { PDItem = "L1", Timestamp = DateTime.Now, PDName = this.PDName, Status = 0 };
                      db.tblPDAlarmLog.Add(log);
                      description += "L1 ";

                  }
              }



          if (tblPDConfig.L2 != -1)
              if (d.L2 != t.L2)
              {
                  tblpd.L2 = t.L2;
                  if (t.L2 == 0)
                  {
                      tblPDAlarmLog log = new tblPDAlarmLog() { PDItem = "L2", Timestamp = DateTime.Now, PDName = this.PDName, Status = 1 };
                      db.tblPDAlarmLog.Add(log);
                  }
                  else
                  {
                      tblPDAlarmLog log = new tblPDAlarmLog() { PDItem = "L2", Timestamp = DateTime.Now, PDName = this.PDName, Status = 1 };
                      db.tblPDAlarmLog.Add(log);
                      description += "L2 ";
                  }
              }


          if (tblPDConfig.L3 != -1)
              if (d.L3 != t.L3)
              {
                  tblpd.L3 = t.L3;
                  if (t.L3 == 0)
                  {
                      tblPDAlarmLog log = new tblPDAlarmLog() { PDItem = "L3", Timestamp = DateTime.Now, PDName = this.PDName, Status = 1 };
                      db.tblPDAlarmLog.Add(log);
                  }
                  else
                  {
                      tblPDAlarmLog log = new tblPDAlarmLog() { PDItem = "L3", Timestamp = DateTime.Now, PDName = this.PDName, Status = 0 };
                      db.tblPDAlarmLog.Add(log);
                      description += "L3 ";
                  }
              }


          if (tblPDConfig.L4 != -1)
              if (d.L4 != t.L4)
              {
                  tblpd.L4 = t.L4;
                  if (t.L4 == 0)
                  {
                      tblPDAlarmLog log = new tblPDAlarmLog() { PDItem = "L4", Timestamp = DateTime.Now, PDName = this.PDName, Status = 1 };
                      db.tblPDAlarmLog.Add(log);
                  }
                  else
                  {
                      tblPDAlarmLog log = new tblPDAlarmLog() { PDItem = "L4", Timestamp = DateTime.Now, PDName = this.PDName, Status = 0 };
                      db.tblPDAlarmLog.Add(log);
                      description += "L4 ";
                  }
              }


          if (d.Cabinet != t.Cabinet)
          {
              tblpd.Cabinet = t.Cabinet;
              if (t.Cabinet == 1)  //close
              {
                  tblPDAlarmLog log = new tblPDAlarmLog() { PDItem = "Cabinet", Timestamp = DateTime.Now, PDName = this.PDName, Memo = "箱門關閉", Status = 1 };
                  db.tblPDAlarmLog.Add(log);
              }
              else   //open
              {
                  tblPDAlarmLog log = new tblPDAlarmLog() { PDItem = "Cabinet", Timestamp = DateTime.Now, PDName = this.PDName, Memo = "箱門開啟", Status = 0 };
                  db.tblPDAlarmLog.Add(log);
                  if(description !="")
                      description += "異常 " + "箱門開啟 ";

                      else
                      description += "箱門開啟 ";
              }
          }
          if (!description.Contains("箱門") && description != "")
              description += "異常";

          if (description != "")
          {
              AlarmData alarmdata = new AlarmData()
              {
                  TimeStamp = DateTime.Now,
                  AlarmType = AlarmType.PD,
                  ColorString = "Red",
                  Description = description,
                //  PlaneID = sender.PlaneID,
                  IsForkCCTVEvent = false,  
                  PlaneName = this.tblPDConfig.PDName//Global.GetPlaneNameByPlaneID(this.PDName)
                 

              };
              Program.MyServiceObject.DispatchAlarmEvent(alarmdata);
          }

          db.SaveChanges();
          db.Dispose();

       }
       void timerBack(object stat)
       {
           //if (RTUDevice==null||!RTUDevice.connected)
           //{
           //    new System.Threading.Thread(ConnectTask).Start();
           //    return;
           //}



       }

       bool IsInConnected = false;
       void ConnectTask()
       {

           if (IsInConnected)
               return;
           IsInConnected = true;
           while (true)
           {
               while (RTUDevice == null || !RTUDevice.connected)
               {
                   try
                   {
                       Console.WriteLine(this.PDName + "  Connecting!");
                       RTUDevice = new Master();
                       RTUDevice.connect(IP, (ushort)Port);
                       RTUDevice.OnResponseData += RTUDevice_OnResponseData;
                       RTUDevice.OnException += RTUDevice_OnException;

                   }
                   catch (Exception ex)
                   {
                       Console.WriteLine(ex.Message + "," + ex.StackTrace);
                       continue;
                       ;
                   }
                   finally
                   {
                       System.Threading.Thread.Sleep(30000);
                   }
                   Console.WriteLine("connected!");
               }

               System.Threading.Thread.Sleep(1000);
           }



           IsInConnected = false;
       }

       void RTUDevice_OnException(ushort id, byte unit, byte function, byte exception)
       {
           if (exception == 254)
           {
               RTUDevice.disconnect();
               RTUDevice.Dispose();

           }
           //  throw new NotImplementedException();
       }

       void RTUDevice_OnResponseData(ushort id, byte unit, byte function, byte[] data)
       {
           //  throw new NotImplementedException();
       }
  
       public override string ToString()
       {
           //return base.ToString();
           StringBuilder sb = new StringBuilder();
          System.Collections.BitArray bits = new System.Collections.BitArray(data);
           //string res = string.Format("R0:S0:T0 {0}:{1}:{2}, R1:S1:T1 {3}:{4}:{5} L1-L5:{6}{7}{8}{9}{10} C:{11}\n", Cvt01(bits.Get(0)), Cvt01(bits.Get(1)), Cvt01(bits.Get(2)), Cvt01(bits.Get(3)), Cvt01(bits.Get(4)), Cvt01(bits.Get(5))
           //   , Cvt01(bits.Get(6)), Cvt01(bits.Get(7)), Cvt01(bits.Get(8)), Cvt01(bits.Get(9)), Cvt01(bits.Get(10)), Cvt01(bits.Get(11)));    
           string res = string.Format("R0:S0:T0 {0}:{1}:{2}, R1:S1:T1 {3}:{4}:{5} L1-L5:{6}{7}{8}{9}{10} C:{11}", R0, S0, T0, R1, S1,T1
               , L0, L1, L2, L3, L4,Cabinet);
           //for (int i = 0; i < data.Length; i++)
           //{
           //    sb.Append(string.Format("{0:X2}", data[i]) + " ");

           //}

           return res;// sb.ToString().Trim();
       }
       
       int Cvt01(bool val)
       {
           return val ? 1 : 0;
       }



    }
}

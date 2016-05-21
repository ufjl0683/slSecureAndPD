using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace SecureServer.NVR
{
  public   class NVRManager
    {
      System.Collections.Concurrent.ConcurrentDictionary<int ,INVR> dictNvrs = new System.Collections.Concurrent.ConcurrentDictionary<int, INVR>();

      public NVRManager()
      {
          using (SecureDBEntities1 db = new SecureDBEntities1())
          {

              var q = from n in db.tblNVRConfig select n;

              foreach (tblNVRConfig tbl in q)
              {
                  if (!dictNvrs.ContainsKey(tbl.NVRID))
                  {
                      if (tbl.Type == 1)
                      {
                          INVR nvr = new NVR_Type1()
                          {
                              ERID = tbl.ERID,
                              IP = tbl.IP,
                              NVRID = tbl.NVRID,
                              Password = tbl.Password,
                              PlaneID = tbl.PlaneID ?? 0,
                              Port = tbl.Port,
                              UserName = tbl.UserName
                          };
                          dictNvrs.TryAdd(tbl.NVRID, nvr);
                          Console.WriteLine("Add NVR:" + tbl.NVRName);
                      }

                  }
              }


          }

          new System.Threading.Thread(TestConnectionTask).Start();

      }


      void TestConnectionTask()
      {

          while (true)
          {

              SecureDBEntities1 db = new SecureDBEntities1();
              Ping ping = new Ping();

              var q = db.tblNVRConfig;
              foreach (tblNVRConfig nvr in q)
              {
                  try
                  {
                      if (ping.Send(nvr.IP).Status == IPStatus.Success)
                      {
                          if (nvr.Comm_state != 1)
                              nvr.Comm_state = 1;
                      }
                      else
                      {
                          if (nvr.Comm_state != 0)
                              nvr.Comm_state = 0;
                      }
                  }
                  catch { ;}
              }

              try
              {
                  db.SaveChanges();
              }
              catch { ;}
              System.Threading.Thread.Sleep(60000);
          }
      }

      public INVR this[int nvrid]
      {

          get
          {
              if (dictNvrs.ContainsKey(nvrid))
                  return dictNvrs[nvrid];
              else
                  return null;
          }
            
      }




    }
}

using System;
using System.Collections.Generic;
using System.Linq;
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

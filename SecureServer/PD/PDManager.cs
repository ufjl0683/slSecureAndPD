using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureServer.PD
{
    public class PDManager
    {


        System.Collections.Concurrent.ConcurrentDictionary<string, PD> dictPDs = new System.Collections.Concurrent.ConcurrentDictionary<string, PD>();

        public PDManager()
      {
          SecureDBEntities1 db = new SecureDBEntities1();

          var q = from n in db.tblPDConfig select n;
          Console.WriteLine("PD Manager Starting....");
          foreach (tblPDConfig tbl in q)
          {
              if (!dictPDs.ContainsKey(tbl.PDName))
              {
                  if (tbl.type == 1 || tbl.type==2 || tbl.type==3 || tbl.type==4 || tbl.type==5|| tbl.type==6)
                  {
                      try
                      {
                          PD pd = new PD(tbl.PDName, tbl.IP, (int)tbl.Port, tbl);
                          //{
                          //    ERID = tbl.ERID,
                          //    IP = tbl.IP,
                          //    NVRID = tbl.NVRID,
                          //    Password = tbl.Password,
                          //    PlaneID = tbl.PlaneID ?? 0,
                          //    Port = tbl.Port,
                          //    UserName = tbl.UserName
                          //};
                          dictPDs.TryAdd(tbl.PDName, pd);
                          Console.WriteLine("Add PD:" + tbl.PDName);
                      }
                      catch(Exception ex) 
                      {
                          Console.WriteLine("PD Manager:"+ex.Message+","+ex.StackTrace);
                          ;}

                  }

              }
          }

           
          

      }


      public PD this[string pdname]
      {

          get
          {
              if (dictPDs.ContainsKey(pdname))
                  return dictPDs[pdname];
              else
                  return null;
          }
            
      }
    
    
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureServer.RTU
{
   public  class PowerControlManager
    {
       System.Collections.Generic.Dictionary<int, PowerControl> dictPowerControl = new Dictionary<int, PowerControl>();
      // PowerControl[] controls = new PowerControl[2];
       public PowerControlManager()
       {
           SecureDBEntities1 db = new SecureDBEntities1();
           var q =from n in db.tblRemotePowerControl select n;


           foreach (tblRemotePowerControl tbl in q)
           {
               if(!dictPowerControl.ContainsKey(tbl.Inx))
               {
                   lock (dictPowerControl)
                   dictPowerControl.Add(tbl.Inx,new PowerControl(tbl.Inx,tbl.DeviceName,tbl.IP,tbl.Port));
               }
           }


        //   GetAllPowerControlInfo();
           //controls[0] = new PowerControl("10.21.223.20", 502);
           //controls[1] = new PowerControl("10.23.209.177", 502);
       }

       public PowerControlInfo[] GetAllPowerControlInfo()
       {
           System.Collections.Generic.List<PowerControlInfo> list = new List<PowerControlInfo>();
         
           lock (dictPowerControl)
           {
              //   PowerControlInfo[] infos = new PowerControlInfo[dictPowerControl.Count];
               foreach(PowerControl ctl in  dictPowerControl.Values)
               {
                  list.Add(ctl.ToPowerControlInfo());
               }
           }
           return list.ToArray();
       }
       public bool GetStatus(int inx, out byte status, out bool IsConnected)
       {
           if (!dictPowerControl.ContainsKey(inx) )
           {
               status = 0;
               IsConnected = false;
               return false;
           }
           lock (dictPowerControl)
           {
               status = dictPowerControl[inx].status;

               IsConnected = dictPowerControl[inx].IsConnected;
           }

           return true;


       }


      public bool SwitchPower(int inx,bool off)
      {
          if (!dictPowerControl.ContainsKey(inx))
              return false;

          try
          {
              lock (dictPowerControl)
              dictPowerControl[inx].SwitchPower(off);
          }
          catch
          {
              return false;
          }
          return true;
      }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModbusTCP;

namespace SecureServer.RTU
{
    public  class RTUManager
    {
        System.Collections.Generic.Dictionary<string, ModbusTCP.IRTU> dictRTUs = new Dictionary<string, ModbusTCP.IRTU>();

        public ModbusTCP.IRTU this[string  ControlID]
        {

            get
            {
                if(dictRTUs.ContainsKey(ControlID))
                    return dictRTUs[ControlID];
                else
                    return null;
            }
        }
        public RTUManager()
        {
            SecureDBEntities1 db = new SecureDBEntities1();
            var q = from n in db.tblControllerConfig where n.IsEnable == true &&( n.ControlType == 3   || n.ControlType==5 || n.ControlType==6)   select n;    //RTU control type=3
            //var q = from n in db.tblControllerConfig where n.ControlID == "AC-RTU-1" && n.ControlType == 3 && n.IsEnable==true select n;
            foreach (tblControllerConfig tbl in q)
            {
                ModbusTCP.IRTU rtu = null; ;
                if (tbl.ControlType == 3) //normal rtu
                {
                    rtu = new ModbusTCP.RTU(tbl.ControlID, 1, tbl.IP, tbl.Port, (int)tbl.RTUBaseAddress, (int)tbl.RTURegisterLength, tbl.Comm_state ?? 0);
                    rtu.OnCommStateChanged += rtu_OnCommStateChanged;
                }
                else if(tbl.ControlType==5)
                {         
                    rtu = new SecureServer.RTU.R23AdamRTU(tbl.ControlID, 1, tbl.IP, tbl.Port, (int)tbl.RTUBaseAddress, (int)tbl.RTURegisterLength, tbl.Comm_state ?? 0,tbl.R23_ADAM );
                    rtu.OnCommStateChanged += rtu_OnCommStateChanged;
                }
                else if (tbl.ControlType == 6)
                {
                    rtu = new SecureServer.RTU.R13SmrRTU(tbl.ControlID, 1, tbl.IP, tbl.Port, (int)tbl.RTUBaseAddress, (int)tbl.RTURegisterLength, tbl.Comm_state ?? 0 );
                    rtu.OnCommStateChanged += rtu_OnCommStateChanged;
                }


                if (!dictRTUs.ContainsKey(tbl.ControlID))
                {

                   
                    dictRTUs.Add(tbl.ControlID, rtu);
                    Console.WriteLine("Add RTU" + rtu.ControlID + ",base:" + tbl.RTUBaseAddress + ",Length:" + tbl.RTURegisterLength);
               
                }
            }

            db.Dispose();

        }

        void rtu_OnCommStateChanged(ModbusTCP.IRTU sender, int comm_state)
        {
            SecureDBEntities1 db = new SecureDBEntities1();
           tblControllerConfig ctl= db.tblControllerConfig.Where(n => n.ControlID == sender.ControlID).FirstOrDefault();

           if (ctl != null)
           {
               ctl.Comm_state = comm_state;
               db.tblDeviceStateLog.Add(

                  new tblDeviceStateLog()
                  {
                       TypeID=10, TypeCode=(short)comm_state, TimeStamp=DateTime.Now, ControlID=sender.ControlID
                  }
                   );



               db.SaveChanges();
           }
           db.Dispose();
            //throw new NotImplementedException();
        }
    }
}

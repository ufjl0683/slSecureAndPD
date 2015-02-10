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
            var q = from n in db.tblControllerConfig where n.IsEnable == true && n.ControlType == 3 select n;    //RTU control type=3

            foreach (tblControllerConfig tbl in q)
            {
                ModbusTCP.IRTU rtu = null; ;
                if(tbl.ControlType==3) //normal rtu
                        rtu= new ModbusTCP.RTU(tbl.ControlID, 1, tbl.IP, tbl.Port,(int) tbl.RTUBaseAddress, (int)tbl.RTURegisterLength);
               
                if (!dictRTUs.ContainsKey(tbl.ControlID))
                {


                    dictRTUs.Add(tbl.ControlID, rtu);
                    Console.WriteLine("Add RTU" + rtu.ControlID + ",base:" + tbl.RTUBaseAddress + ",Length:" + tbl.RTURegisterLength);
                }
            }

        }
    }
}

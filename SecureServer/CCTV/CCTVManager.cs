using SecureServer.CardReader.BindingData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureServer.CardReader.CCTV
{
    public class CCTVManager
    {

        System.Collections.Generic.Dictionary<int, ICCTV> dictCCTVs = new Dictionary<int, ICCTV>();
        System.Threading.Timer tmr;

        public ICCTV this[int CCTVID]
        {
            get
            {
                if (!dictCCTVs.ContainsKey(CCTVID))
                    return null;

                return dictCCTVs[CCTVID];
            }
        }

        public CCTVManager()
        {
            SecureDBEntities1 db = new SecureDBEntities1();
            var q = from n in  db.tblCCTVConfig   select n;
            foreach (tblCCTVConfig data in q)
            {

                ICCTV cctv=null ;
                if (data.Type == 1)  //華電牌  利凌企業 LILIN
                    cctv = new CCTV_TYPE1(data.CCTVID,data.CCTVName,data.PlaneID??0,data.IP, data.Port, data.UserName, data.Password);
                if(cctv!=null)
                     dictCCTVs.Add(data.CCTVID, cctv);
                //cardreader.OnDoorEvent += cardreader_OnDoorEvent;
                //cardreader.OnAlarmEvent += cardreader_OnAlarmEvent;
                Console.WriteLine("加入CCTV:" + data.CCTVID);
            }

            //tmr = new System.Threading.Timer(OneMinTask);
            //tmr.Change(0, 1000 * 60);
           



        }


        public BindingData.CCTVBindingData[] GetAllCCTVBindingData(int PlaneID)
        {
            System.Collections.Generic.List<CCTVBindingData> list = new List<CCTVBindingData>();

            foreach (ICCTV pair in dictCCTVs.Values.ToArray())
            {
                if (pair.PlaneID == PlaneID)
                    list.Add(pair.ToBindingData());
            }

            return list.ToArray();
        }
    }
}
    


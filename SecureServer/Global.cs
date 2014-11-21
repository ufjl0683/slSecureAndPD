using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureServer.CardReader
{
    public static class Global
    {

        public static System.Collections.Generic.Dictionary<int, tblERPlane> dictPlane ;

        public static string GetPlaneNameByPlaneID(int planeid)
        {
            if (dictPlane == null)
            {

                SecureDBEntities1 db=new SecureDBEntities1();
                dictPlane = new Dictionary<int, tblERPlane>();
                var q= from n in db.tblERPlane  select n;
                foreach (tblERPlane planedata in q)
                {
                    try
                    {
                        dictPlane.Add(planedata.PlaneID, planedata);
                    }
                    catch (Exception ex)
                    {
                        ;
                    }
                }
      

            }

            return  dictPlane[planeid].PlaneName;

        }
    }
}

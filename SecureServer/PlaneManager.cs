using SecureServer.BindingData;
using SecureServer.RTU;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SecureServer
{
   public  class PlaneManager
    {

       System.Collections.Generic.Dictionary<int,Plane> dictERPlanes=new Dictionary<int,Plane>();

       public Plane this[int inx]
       {

           get
           {
               if (this.dictERPlanes.ContainsKey(inx))
                   return dictERPlanes[inx];
               else
                   return null;
           }
       }
       public PlaneManager()
       {
           SecureDBEntities1 db = new SecureDBEntities1();

           var q =from n in db.tblERPlane    select n  ;

         //  System.Collections.Generic.List<Plane> list = new List<Plane>();

           foreach (tblERPlane tbl in q)
           {

               if(!dictERPlanes.ContainsKey(tbl.PlaneID))
                   dictERPlanes.Add(tbl.PlaneID,new Plane(tbl.ERID,tbl.PlaneID,tbl.PlaneName));

             //  list.Add(new Plane(tbl.PlaneID, tbl.PlaneName));
           }
           //ERPlanes = q.ToArray();
          
          
       }

       public PlaneDegreeInfo[] GetAllPlaneDegree()
       {
           return dictERPlanes.Values.Select(n => new PlaneDegreeInfo() { PlaneID=n.PlaneID, AlarmStatus=n.Degree, ERID=n.ERID, Name=n.PlaneName,ColorString=n.DegreeColor }).ToArray();
       }



    }


  

}

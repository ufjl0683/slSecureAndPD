using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using System.Runtime.Serialization;

namespace slSecure.Web
{
    [DataContract]
    public class CCTVGridInfo
    {
        [DataMember]
       public int NO { get; set; }
         [DataMember]
       public  int CCTVID { get; set; }
         [DataMember]
       public  string CCTVName { get; set; }
         [DataMember]
       public string IP { get; set; }
         [DataMember]
       public int Port { get; set; }
         [DataMember]
       public string UserName { get; set; }
         [DataMember]
       public string Password { get; set; }
         [DataMember]
       public int Type { get; set; }

    }
    public partial class GetCCTVGridData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          //  string json = "{\"name\":\"Joe\"}";
            Response.Clear();
            Response.ContentType = "application/json; charset=utf-8";

               slSecure.Web.SecureDBEntities db=new SecureDBEntities();
               var q=  from n in  db.tblCCTVSplitScreen orderby n.NO  join o in db.tblCCTVConfig on n.CCTVID equals o.CCTVID
                       select new CCTVGridInfo
                       {    NO= n.NO, 
                            CCTVID=n.CCTVID??-1,CCTVName= o.CCTVName, IP= o.IP, Port= o.Port, UserName= o.UserName,Password= o.Password,Type= o.Type??-1 };

                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(CCTVGridInfo[]));
                MemoryStream ms = new MemoryStream();
                CCTVGridInfo[] infos= q.ToArray<CCTVGridInfo>();
                ser.WriteObject(ms,infos);
        string jsonString = Encoding.UTF8.GetString(ms.ToArray());
        ms.Close();

        Response.Write(jsonString);
            Response.End();
        }
    }
}
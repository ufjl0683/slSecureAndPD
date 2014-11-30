using SecureServer.CardReader.BindingData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SecureServer.CardReader.CCTV
{
     public  class CCTV_TYPE1:ICCTV
    {
         public string UserName { get; set; }
         public string Password { get; set; }
         public string IP { get; set; }
         public int Port { get; set; }
         public int CCTVID { get; set; }
         public string CCTVName { get; set; }
         public int PlaneID {get;set;}
         public CCTV_TYPE1(int CCTVID,string CCTVName,int PlaneID,string IP, int  Port, string UserName, string Password)
         {
             this.CCTVID=CCTVID;
             this.UserName = UserName;
             this.Password = Password;
             this.IP=IP;
             this.Port = Port;
             this.CCTVName = CCTVName;
             this.PlaneID = PlaneID;
         }
        public void Preset(int preset)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + IP + ":" + Port + "/ptzpreset?camid=1&goto_preset=" + preset);
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
            request.Credentials = new NetworkCredential(UserName, Password);

            try
            {
                WebResponse response = request.GetResponse();

                //System.IO.StreamReader rd = new System.IO.StreamReader(response.GetResponseStream());
                //string ret = rd.ReadToEnd();
                //Console.WriteLine(ret);
            }
            catch (Exception ex)
            {
                //  Console.WriteLine(ex.Message + "," + ex.StackTrace);
            }


           // throw new NotImplementedException();
        }


        


        public string GetMjpgCGIString( )
        {
            return string.Format("http://{0}:{1}/getimage?fmt=cif",IP,Port);
        }


        public CCTVBindingData ToBindingData()
        {
            CCTVBindingData ret = new CCTVBindingData() {  CCTVID=this.CCTVID, MjpegCgiString=GetMjpgCGIString( ),
             IP=this.IP, Password=this.Password, Port=this.Port, UserName=this.UserName, CCTVName=CCTVName
            };
             
             return ret;
        }
    }
}

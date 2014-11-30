using SecureServer.CardReader.BindingData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureServer.CardReader.CCTV
{
   public  interface ICCTV
    {

       void Preset(int preset);
       string GetMjpgCGIString( );
       CCTVBindingData ToBindingData();
       int PlaneID { get; set; }
       string UserName { get; set; }
       string Password { get; set; }
    }
}

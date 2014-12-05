using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureServer.CardReader.NVR
{
   public  interface INVR
    {
       int NVRID { get; set; }

       int ERID { get; set; }

       string IP { get; set; }

       int Port { get; set; }

       string UserName { get; set; }

       string Password { get; set; }

       int PlaneID { get; set; }


       bool SaveRecord(int Chno, DateTime BeginDateTime, DateTime EndDateTime, string SavePathFilename);

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureServer
{
    public class CardReaderManager
    {
      
        System.Collections.Generic.Dictionary<string, CardReader> dictCardReaders = new Dictionary<string, CardReader>();
        System.Threading.Timer tmr;

        public CardReader this[string ControllID]
        {

            get
            {
                if (!dictCardReaders.ContainsKey(ControllID))
                    return null;

                return dictCardReaders[ControllID] ;
            }
        }

        public CardReaderManager()
        {
            SecureDBEntities1 db = new SecureDBEntities1();
            var q = from n in db.tblControllerConfig where (n.ControlType == 2 || n.ControlType == 1) && n.IsEnable==true select n;
            foreach (tblControllerConfig data in q)
            {
                dictCardReaders.Add(data.ControlID, new CardReader(data.ControlID,data.IP,data.ERID,(int)data.PlaneID ));
                Console.WriteLine("加入卡機:" + data.ControlID);
            }

            tmr = new System.Threading.Timer(OneMinTask);
            tmr.Change(0, 1000 * 60);
           



        }


        void OneMinTask(object o)
        {
            try
            {
                CheckAndGenerateDailySuperPassword();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message+","+ex.StackTrace);
            }
           
            Console.WriteLine("In one Min task!");

        }

        void CheckAndGenerateDailySuperPassword()
        {
            SecureDBEntities1 db = new SecureDBEntities1();
            DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            tblERDoorPassword tbl = (from n in db.tblERDoorPassword
                                     where n.Timestamp
                                         == dt
                                     select n).FirstOrDefault();
            if (tbl == null)
            {
                Random rnd = new Random();
               
                int passwd=rnd.Next(0, 10000);
                string pwdString=passwd.ToString("0000");
              //  tblERDoorPassword tbl1 = 
                db.tblERDoorPassword.Add(new tblERDoorPassword() { Timestamp = dt, DoorPassword = passwd.ToString("0000") });
                db.SaveChanges();


                foreach (CardReader  reader in dictCardReaders.Values)
                {
                    try
                    {
                        reader.SetSuperOpenDoorPassword(passwd);
                        Console.WriteLine(reader.ControllerID+"設定每日開門密碼成功" );
                    }
                    catch(Exception  ex)
                    {
                        Console.WriteLine(ex.Message + "," + ex.StackTrace);
                    }
                }
            }

        }
        

        //public class CardReaderInfo
        //{
        //    public CardReader cardReader { get; set; }
        //    public tblControllerConfig cardData;
        //    public string IP { get; set; }
        //    public bool IsConnected
        //    {

        //        get
        //        {
        //            return cardReader.IsConnected;
        //        }
        //    }
           
        //    public CardReaderInfo(string ip, tblControllerConfig cardData)
        //    {
        //        this.IP = ip;
        //        this.cardData = cardData;
        //        cardReader = new CardReader(ip);
        //    }



        //}
    }
}

using ClassSockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace SecureServer
{
   public  class CardReader
    {
       public string IP { get; set; }
       public bool IsConnected = false;
       public string ControllerID { get; set; }
       public int PlaneID { get; set; }
       public int ERID { get; set; }
       ClassSockets.ClientSocket ClientSocket;
       ClassSockets.ServerSocket ServerScoket;
       System.Threading.Timer tmr; 
       public CardReader(string  controllerid,string ip,int ERID,int PlaneID)
       {
           this.IP = ip;
           this.ControllerID = controllerid;
           this.ERID = ERID;
           this.PlaneID = PlaneID;

           ClientSocket = new ClassSockets.ClientSocket();

           ServerScoket = new ClassSockets.ServerSocket();
           ServerScoket.OnRead += new ServerSocket.ConnectionDelegate(Server_OnRead);

           if (ServerScoket.Active())
                    Console.WriteLine("Card Reader Server Socket is Listening!");

           tmr= new System.Threading.Timer(OneSecTask);
           tmr.Change(0, 1000);
       }


       void OneSecTask(object a)
       {
           try
           {
            int status=   ClientSocket.ReadAllState(1,IP);
            if (status == -2)
                this.IsConnected = false;
            else
                this.IsConnected = true;
               
              // System.Collections.BitArray ba=new System.Collections.BitArray(new int[]{status});
              // if (ba.Get(0))
              //     Console.WriteLine("開門");
              // else
              //     Console.WriteLine("關門");

              // if (ba.Get(1))
              //     Console.WriteLine("外部按鍵按下");
              // else
              //     Console.WriteLine("外部按鍵放開");

              //if (ba.Get(2))
              //     Console.WriteLine("開門超時");
              // else
              //     Console.WriteLine("無開門超時");

              //if (ba.Get(3))
              //    Console.WriteLine("外力破壞");
              //else
              //    Console.WriteLine("無外力破壞");

           }
           catch(Exception ex)
           {
               Console.WriteLine(ex.Message + "," + ex.StackTrace);
           }

       }

       private void Server_OnRead(Socket soc)
       {
           byte[] SRbuf = ServerScoket.ReceivedBytes;
          
           if (SRbuf != null)
           {
               if (SRbuf.Length != 25)
                   return;
               string SRhexstr = BitConverter.ToString(SRbuf);
               CardReaderEventReport rpt = new CardReaderEventReport(SRbuf);
               Console.WriteLine(rpt);
               if (rpt.Status == (int)CardReaderStatusEnum.開鎖 ||
                   rpt.Status == (int)CardReaderStatusEnum.按鈕開門 ||
                   rpt.Status == (int)CardReaderStatusEnum.密碼開門 || rpt.Status == (int)CardReaderStatusEnum.系統開門 ||
                   rpt.Status == (int)CardReaderStatusEnum.異常入侵 )
                    
               {
                   SecureDBEntities1 db = new SecureDBEntities1();

                   db.tblEngineRoomLog.Add(

                         new tblEngineRoomLog()
                         {
                             ControlID = ControllerID,
                             ABA = rpt.CardNo.ToString(),
                             StartTime = DateTime.Now,
                             TypeID = 8,
                             Memo = rpt.StatusString,
                             TypeCode = (short)rpt.Status,
                             ERNo=this.PlaneID.ToString()
                         }
                       );
                   db.SaveChanges();
                  
               }


               if (rpt.Status == (int)CardReaderStatusEnum.號碼錯誤 ||
                 rpt.Status == (int)CardReaderStatusEnum.卡號連續錯誤 ||
                 rpt.Status == (int)CardReaderStatusEnum.外力破壞  
                 )
               {
                   SecureDBEntities1 db = new SecureDBEntities1();

                   db.tblEngineRoomLog.Add(

                         new tblEngineRoomLog()
                         {
                             ControlID = ControllerID,
                             ABA = rpt.CardNo.ToString(),
                             StartTime = DateTime.Now,
                             TypeID = 8,
                             Memo = rpt.StatusString,
                             TypeCode = (short)rpt.Status,
                             ERNo = this.PlaneID.ToString()
                         }
                       );
                   db.SaveChanges();

               }
               
             //  SRhexstr = SRhexstr.Replace("-", " ");
           //    ShowSMsg((soc.RemoteEndPoint.ToString()).Substring(0, soc.RemoteEndPoint.ToString().IndexOf(':')) + " ::: Received data" + " -> " + SRhexstr + "\r");
           }

       }
       public void ForceOpenDoor()
       {
          int res= ClientSocket.WriteOpenDoorCommand(1, IP, 2);
         
         
          if (res == -2)
          {
              IsConnected = false;
              throw new Exception("Connection error!");
          }

          if (res >= 1)
              throw new Exception("cmd error!");
          if (res == -1)
              throw new Exception("ID error!");
          IsConnected = true;
       }

       public void SetDateTime(DateTime dt)
       {
           int res = ClientSocket.TimeCorrection(1, IP, (byte)(dt.Year % 100), (byte)dt.Month,(byte) dt.Day,(byte) dt.DayOfWeek,(byte) dt.Hour,(byte) dt.Minute,(byte) dt.Second);
           if (res == -2)
           {
               IsConnected = false;
               throw new Exception("Connection error!");
           }

           if (res >= 1)
               throw new Exception("cmd error!");
           if (res == -1)
               throw new Exception("ID error!");
           IsConnected = true;
       }


       public void WriteCardReaderID(int id)
       {
           int res = ClientSocket.WriteID(1, IP, (byte)id);
           if (res == -2)
           {
               IsConnected = false;
               throw new Exception("Connection error!");
           }

           if (res >= 1)
               throw new Exception("cmd error!");
           if (res == -1)
               throw new Exception("ID error!");
           IsConnected = true;
       }

       public void DeleteAllCard()
       {

           int res = ClientSocket.WriteCardAllDelete(1, IP);

           if (res == -2)
           {
               IsConnected = false;
               throw new Exception("Connection error!");
           }

           if (res >= 1)
               throw new Exception("cmd error!");
           if (res == -1)
               throw new Exception("ID error!");

           IsConnected = true;
       }

       public void AddVirturalCard(string cardno)
       {
           int cardid = int.Parse(cardno);

           int res = ClientSocket.WriteAddCard(1, IP, cardid / 65536, cardid % 65536,
               2, 0, 1, 1);
           if (res == -2)
           {
               IsConnected = false;
               throw new Exception("Connection error!");
           }

           if (res >= 1)
               throw new Exception("cmd error!");
           if (res == -1)
               throw new Exception("ID error!");

           IsConnected = true;
       }
       public void AddCard(string cardno)
       {
           uint cardid = uint.Parse(cardno);
               
             int res=  ClientSocket.WriteAddCard(1,  IP, (int)cardid / 65536, (int)cardid % 65536,
                 0, 0, 1,1); 
           if (res == -2)
           {
               IsConnected = false;
               throw new Exception("Connection error!");
           }

           if (res >= 1)
               throw new Exception("cmd error!");
           if (res == -1)
               throw new Exception("ID error!");

           IsConnected = true;
       }
       public void DeleteCard(string cardno)
       {
           int cardid = int.Parse(cardno);

           int res = ClientSocket.WriteAddCard(1, IP, cardid / 65536, cardid % 65536,
               0, 0, 1, 2);
           if (res == -2)
           {
               IsConnected = false;
               throw new Exception("Connection error!");
           }

           if (res >= 1)
               throw new Exception("cmd error!");
           if (res == -1)
               throw new Exception("ID error!");

           IsConnected = true;
       }

       public void SetSuperOpenDoorPassword(int password)
       {

           int res = ClientSocket.WriteOpenDoorPassword(1, IP, password);
           if (res == -2)
           {
               IsConnected = false;
               throw new Exception("Connection error!");
           }

           if (res >= 1)
               throw new Exception("cmd error!");
           if (res == -1)
               throw new Exception("ID error!");

           IsConnected = true;

       }


       public void SetOpenDoorDetectionAlarmTime(int sec)
       {


           int res = ClientSocket.WriteOpenDoorDetectionAlarmTime(1,IP,(byte)sec);
           if (res == -2)
           {
               IsConnected = false;
               throw new Exception("Connection error!");
           }

           if (res >= 1)
               throw new Exception("cmd error!");
           if (res == -1)
               throw new Exception("ID error!");

           IsConnected = true;
       }

       public void SetOpenDoorTimeoutDetectionTime(int sec)
       {

                   
           int res = ClientSocket.WriteOpenDoorTimeoutDetectionTime(1, IP, (byte)sec);
           if (res == -2)
           {
               IsConnected = false;
               throw new Exception("Connection error!");
           }

           if (res >= 1)
               throw new Exception("cmd error!");
           if (res == -1)
               throw new Exception("ID error!");

           IsConnected = true;
       }


       public void SetOpenDoorAutoCloseTime(int sec)
       {


           int res = ClientSocket.WriteOpenDoorACKTime(1, IP, (byte)sec);
           if (res == -2)
           {
               IsConnected = false;
               throw new Exception("Connection error!");
           }

           if (res >= 1)
               throw new Exception("cmd error!");
           if (res == -1)
               throw new Exception("ID error!");

           IsConnected = true;
       }

    }
}

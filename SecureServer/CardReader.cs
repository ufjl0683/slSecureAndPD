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
       ClassSockets.ClientSocket ClientSocket;
       ClassSockets.ServerSocket ServerScoket;
       public CardReader(string ip)
       {
           this.IP = ip;
           ClientSocket = new ClassSockets.ClientSocket();

           ServerScoket = new ClassSockets.ServerSocket();
           ServerScoket.OnRead += new ServerSocket.ConnectionDelegate(Server_OnRead);

           if (ServerScoket.Active())
                    Console.WriteLine("Card Reader Server Socket is Listening!");

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
           int cardid = int.Parse(cardno);
               
             int res=  ClientSocket.WriteAddCard(1,  IP, cardid / 65536, cardid % 65536,
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

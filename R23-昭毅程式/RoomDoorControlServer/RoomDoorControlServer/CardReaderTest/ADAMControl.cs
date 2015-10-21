using CardReaderTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoomDoorControlServer
{
    class ADAMControl:IEnd
    {
        string Name;
        string IP;
        int Port;
        public bool End { get { return end; } set { end = value; } }
        bool end = false;
        //System.Net.Sockets.TcpClient tcp;
        System.Net.Sockets.Socket socket;
        //internal bool IsConnected;
        System.Threading.AutoResetEvent wait = new System.Threading.AutoResetEvent(false);
        System.Timers.Timer timer = new System.Timers.Timer(3000);
        bool? readEnd = false;
        int readNum;
        int  ConnectCount = 0;
        

        static int clientPort = 10000;
        static int ClientPort
        {
            get { 
                lock(typeof(ADAMControl))
                {
                    if (clientPort > 11000)
                        clientPort = 10000;
                    return ++clientPort;} 
            }
        }

        //internal event RoomInterface.RoomEventHandler StatusChangeEvent;

        public ADAMControl(string name, string ip, int prot)
        {
            
            Name = name;
            IP = ip;
            Port = prot;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);

            byte[] IOStatus = new byte[33];

            dbroomEntities dbroom = new dbroomEntities();
            try
            {
                var data = from o in dbroom.tblDeviceStateLog where o.ControlID == name && o.TypeID == 4 orderby o.TimeStamp descending select o.TypeCode;
                if (data.Count() > 0)
                {
                    IOStatus[32] = Convert.ToByte(data.First());
                }
            }
            catch (Exception ex)            
            {
                TCommon.SaveLog(ex.Message + "\r\n" + ex.StackTrace);
            }

            for (int i = 0; i < 16; i++)
            {
                string No = i.ToString();
                try
                {
                    var data = from o in dbroom.tblSingalIOLog
                                where o.ControlID == name && o.SingalName == "DI-" + No
                                orderby o.TimeStamp descending
                                select o.Status;
                    if (data.Count() > 0)
                    {
                        IOStatus[i] = Convert.ToByte(data.First());
                    }
                }
                catch (Exception ex)
                {
                    TCommon.SaveLog(ex.Message + "\r\n" + ex.StackTrace);
                }
                try
                {
                    var data =  from o in dbroom.tblSingalIOLog
                                where o.ControlID == name && o.SingalName == "DO-" + No
                                orderby o.TimeStamp descending
                                select o.Status;
                    if (data.Count() > 0)
                    {
                        IOStatus[i + 16] = Convert.ToByte(data.First());
                    }
                }
                catch (Exception ex)
                {
                    TCommon.SaveLog(ex.Message + "\r\n" + ex.StackTrace);
                }
            }
            
            ServerData.ADAMStatus.Add(name, IOStatus);
            //SetAlarmTime(30, 30, 30);
  
        }

      
        internal void HostAction()
        {
            while (!End)
            {
                try
                {
                    byte[] status = GetStatus();
                    if (!StatusChangeCheck(ServerData.ADAMStatus[Name], status))
                    {
                        ServerData.SendRoomEvent(RoomInterface.ControllEventType.ADAMStatusChange, Name, status);
                        status.CopyTo(ServerData.ADAMStatus[Name], 0);
                    }
                }
                catch(Exception ex)
                {
                    CardControl.SaveMessageLog(IP, ex.Message);
                }
                
                wait.WaitOne(1000);
            }
            timer.Dispose();
        }

        bool StatusChangeCheck(byte[] Array1, byte[] Array2)
        {
            if (Array1.Length != Array2.Length)
                return false;
            bool check = true;
            DateTime time = DateTime.Now;

            if (Array1[32] != Array2[32])
            {
                string cmd;
             
                cmd = string.Format("Insert Into tblDeviceStateLog (TypeID,TypeCode,Timestamp,ControlID) Values ({0},{1},'{2}','{3}');"
                    , 4, Array2[32], time.ToString("yyyy-MM-dd HH:mm:ss"), Name);
                DatabaseAccess.DatabaseAcces(cmd);
                check = false;
            }
            for (int i = 0; i < 32; i++)
            {
                if (Array1[i] != Array2[i])
                {
                    string IOName;
                    if (i < 16)
                    {
                        IOName = "DI-" + i;
                    }
                    else
                    {
                        IOName = "DO-" + (i-16) ;
                    }

                    string cmd = string.Format("Insert Into tblSingalIOLog (ControlID,SingalName,TimeStamp,Status) Values('{0}','{1}','{2}',{3});"
                        , Name, IOName, time.ToString("yyyy-MM-dd HH:mm:ss"), Array2[i]);
                    DatabaseAccess.DatabaseAcces(cmd);
                    check = false;
                }
            }
           
            return check;
        }

        public bool SetAlarmTime(int time1, int time2, int time3,int time4)
        {
            lock (this)
            {
                if (!socketConnect())
                {
                    return false;
                }
                string px = string.Format("<PX DT=\"{0}\" PT=\"{1},{2},{3},{4}\"/>"
                    , DateTime.Now.ToString("yyyyMMddHHmmss"),time1,time2,time3,time4);
                writeData(px);
                string receiveString = ReadData();
                return !string.IsNullOrWhiteSpace(receiveString);
                //socket.Disconnect(true);
                //socket.Dispose();
            }
        }

        

        public byte[] GetStatus()
        {
            lock (this)
            {
                try
                {                
                    byte[] status = new byte[33];
                    if (ConnectCount > 0)
                    {
                        ConnectCount--;
                        return status;
                    }               
                    if (!socketConnect())
                    {
                        Random ramdom = new Random();
                        ConnectCount = ramdom.Next(60, 180);
                            ConnectCount=1;
                        ServerData.CheckControlConnect(Name, false);
                        return status;
                    }
                    else
                    {
                        ServerData.CheckControlConnect(Name, true);
                    }

                    //tcp = new System.Net.Sockets.TcpClient(IP, Port);

                    string px = string.Format("<PX DT=\"{0}\"/>", DateTime.Now.ToString("yyyyMMddHHmmss"));

                    //string px = string.Format("<PX DT=\"{0}\" QT=\"\"/>", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    //string px = string.Format("<PX DT=\"{0}\" PT=\"10,10,5,10\"/>", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    writeData(px);
                    string receiveString = ReadData();

                    //wait.WaitOne(1000);
                    //px = string.Format("<PX DT=\"{0}\"/>", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    //writeData(px);                    
                    //receiveString = ReadData();
                    
                    //socket.Disconnect(true);
                    //socket.Dispose();
                    //tcp.Close();
                    if (receiveString.Equals(string.Empty))//連線成功接收失敗
                    {
                        socket.Disconnect(true);
                        socket.Dispose();
                        return ServerData.ADAMStatus[Name];
                    }
                    
                    System.Xml.XmlDocument XmlDoc = new System.Xml.XmlDocument();
                    XmlDoc.LoadXml(receiveString);
                    //try
                    //{
                    //    string[] Times = XmlDoc.ChildNodes[0].Attributes["PT"].Value.Split(',');
                    //    if (Times[3] != ServerData.AlarmTime)
                    //    {
                    //        px = string.Format("<PX DT=\"{0}\" PT=\"{1},{2},{3},{4}\"/>"
                    //            , DateTime.Now.ToString("yyyyMMddHHmmss"), Times[0], Times[1], Times[2], ServerData.AlarmTime);
                    //        writeData(px);
                    //        receiveString = ReadData();
                    //    }
                    //}
                    //catch
                    //{
                    //    ;
                    //}
                    UInt32 DI = Convert.ToUInt32(XmlDoc.ChildNodes[0].Attributes["DI"].Value, 16);
                    UInt32 DO = Convert.ToUInt32(XmlDoc.ChildNodes[0].Attributes["DO"].Value, 16);
                    int ST = Convert.ToInt32(XmlDoc.ChildNodes[0].Attributes["ST"].Value);

                    for (int i = 0; i < 16; i++)
                    {
                        status[i] = (byte)((DI >> i) % 2);
                        status[16 + i] = (byte)((DO >> i) % 2);
                    }

                    if (Name == "AK-ADAM-1")
                    {
                    }

                    switch (ST)
                    {
                        case 0:
                            status[32] = 1;
                            break;
                        case 100:
                            status[32] = 2;
                            break;
                        case -1:
                            status[32] = 3;
                            break;
                        case -2:
                            status[32] = 4;
                            break;
                        case -100:
                            status[32] = 5;
                            break;
                    }

                    return status;
                }
                catch
                {
                    return new byte[33]; 
                }
            }
        }

        internal bool Reset()
        {
            lock (this)
            {
                try
                {
                    if (!socketConnect())
                    {
                        return false;
                    }
                    
                    string px = string.Format("<PX DT=\"{0}\" GT=\"\"/>", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    writeData(px);
                    string receiveString = ReadData();
                    //socket.Disconnect(true);
                    //socket.Dispose();
                    if (receiveString.Equals(string.Empty))//連線成功接收失敗
                    {
                        return false;
                    }
                    else
                        return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        bool socketConnect()
        {
            //ConnectEnd = false;
            //wait.Reset();
            if (socket != null && socket.Connected)
                return true;
        
            bool NotOpen = true;
            while (NotOpen)
            {
                try
                {
                    System.Net.IPEndPoint GroupEP = new System.Net.IPEndPoint(System.Net.IPAddress.Any, ADAMControl.ClientPort);
                    socket = new System.Net.Sockets.Socket(GroupEP.AddressFamily, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
                    socket.Bind(GroupEP);
                    NotOpen = false;                    
                }
                catch
                { }
            }
            ServerData.waitServerTCPConnect();
            try
            {
                //socket.BeginConnect(IP, Port, EndConnect, null);
                //ConnectEnd = false;
                //while (!ConnectEnd && !socket.Connected)
                //{
                //    wait.WaitOne(1000);
                //}
                socket.Connect(IP, Port);
            }
            catch (System.Net.Sockets.SocketException)
            {
                ServerData.unregisterServerTCPConnect();
                return false;
            }
          
            ServerData.unregisterServerTCPConnect();
            if (socket.Connected)
                return true;
            else
                return false;

            
        }

        bool ConnectEnd = false;
        void EndConnect(IAsyncResult ar)
        {
            ConnectEnd = true;
            wait.Set();
        }

        public bool OpenDoor(int DoorNum)
        {
            lock (this)
            {
                try
                {
                    //tcp = new System.Net.Sockets.TcpClient(IP, Port);
                    socketConnect();
                    if (!socket.Connected)
                        return false;
                    string px = string.Format("<PX DT=\"{0}\" CT=\"{1}\"/>", DateTime.Now.ToString("yyyyMMddHHmmss"), DoorNum);
                    writeData(px);
                    string receiveString = ReadData();
                    //socket.Disconnect(true);
                    //socket.Dispose();
                    //tcp.Close();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }


        void writeData(string Data)
        {
            byte[] sendData = System.Text.Encoding.UTF8.GetBytes(Data);
            socket.Send(sendData);
            //tcp.GetStream().Write(sendData, 0, sendData.Length);
        }

        string ReadData()
        {
            byte[] Data = new byte[socket.ReceiveBufferSize];
            //int num = tcp.GetStream().Read(Data, 0, tcp.ReceiveBufferSize);
            readEnd = false;
            timer.Interval = 10000;
            timer.Start();

            IAsyncResult ar =socket.BeginReceive(Data, 0, socket.ReceiveBufferSize, System.Net.Sockets.SocketFlags.None, ReadCompale, null);

            wait.Reset();
            while (readEnd == false)
            {
                wait.WaitOne(100);
            }
            wait.Reset();


            //int num = socket.Receive(Data);
            if (readEnd == true)
            {
                return System.Text.Encoding.UTF8.GetString(Data, 0, readNum);
            }
            else
                return string.Empty;
        }

        void ReadCompale(IAsyncResult ar)
        {
            if (socket == null)
                return;
            if (ar.IsCompleted)
            {
                try
                {
                    readNum = socket.EndReceive(ar);
                }
                catch
                {
                    readNum = 0;
                    //System.IO.File.AppendAllText(@".\ReceiveError.log", DateTime.Now + Name + "\r\n");
                }
            }
            else
                readNum = 0;
            
            if (readNum > 0)
                readEnd = true;
            else
                readEnd = null;
            wait.Set();
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            readEnd = null;
            timer.Stop();
            wait.Set();
        }


        bool checkConnect()
        {
            try
            {
                if (!socket.Connected)
                    socket.Connect(IP, Port);
                return true;
            }
            catch
            {
                return false;
            }
        }

       
    }
}

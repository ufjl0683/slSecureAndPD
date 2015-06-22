using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RoomInterface;

namespace RoomClient
{
    public class RoomClient
    {
        static IRoom roomObj = null;
        static string objUrl;
        static public event RoomInterface.RoomEventHandler RoomEvent;
        static RoomEventClass roomEvent = new RoomEventClass();
        static System.Timers.Timer timer = new System.Timers.Timer();
        static DateTime LastTime;

        static RoomClient()
        {
            
            //string ConnectString = "metadata=res://*/Model2.csdl|res://*/Model2.ssdl|res://*/Model2.msl;provider=System.Data.SqlClient;provider connection string=&quot;Server=192.168.2.24,8605;Database=dbroom;User ID=sa;Password=654321;MultipleActiveResultSets=True&quot;";
            //System.Data.SqlClient.SqlConnectionStringBuilder sqlConnection = new System.Data.SqlClient.SqlConnectionStringBuilder();
            //sqlConnection.DataSource = @"192.168.2.24,8605";
            //sqlConnection.ApplicationName = "";
            //sqlConnection.InitialCatalog = "dbroom";
            //sqlConnection.IntegratedSecurity = true;
            //sqlConnection.PersistSecurityInfo = true;
            //sqlConnection.UserID = "sa";
            //sqlConnection.Password ="654321";

            //string connectString = "Server=10.21.99.82;Database=SecureDB;User ID=secure;Password=secure;";
            string connectString = "data source=10.21.99.82;initial catalog=SecureDB;persist security info=True;user id=secure;password=secure;MultipleActiveResultSets=True;App=EntityFramework;";
            string dir = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            dir = dir.Remove(dir.LastIndexOf('\\'));
            if (System.IO.File.Exists(dir +@"\Server.txt"))
            {
                string serverSet = System.IO.File.ReadLines(dir + @".\Server.txt").First();
                if (serverSet.Length < 300)
                {
                    connectString = serverSet;
                }
            }

            //System.Data.SqlClient.SqlConnection s = new System.Data.SqlClient.SqlConnection(sqlConnection.ConnectionString);

            //System.Data.EntityClient.EntityConnectionStringBuilder ecsb = new System.Data.EntityClient.EntityConnectionStringBuilder();
            //ecsb.Provider = "System.Data.SqlClient";              
            //ecsb.ProviderConnectionString = sqlConnection.ConnectionString;
            //ecsb.Metadata = @"res://*/Model2.csdl|res://*/Model2.ssdl|res://*/Model2.msl";
            //System.Data.EntityClient.EntityConnection ec = new System.Data.EntityClient.EntityConnection(ecsb.ConnectionString);

            //dbroomClientEntities dbroom = new dbroomClientEntities(ec);
            try
            {
                System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter("Select * from tblHostConfig;", connectString);
                System.Data.DataTable DT = new System.Data.DataTable();
                adapter.Fill(DT);
                adapter.Dispose();
                objUrl = "tcp://" + DT.Rows[0]["IP"] + ":" + DT.Rows[0]["Port"] + "/RoomObj";
                
                System.Runtime.Remoting.Channels.Tcp.TcpChannel tcp = new System.Runtime.Remoting.Channels.Tcp.TcpChannel(0);
                System.Runtime.Remoting.Channels.ChannelServices.RegisterChannel(tcp, false);
                //var HostData = (from o in dbroom.tblHostConfigs select o).First();
                //objUrl = "tcp://" + HostData.IP + ":" + HostData.Port + "/RoomObj";
            }
            catch (Exception )
            {
                throw new Exception("資料庫讀取失敗");
            }
            roomEvent.RoomEvent += new RoomEventHandler(RoomClient_RoomEvent);
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Interval = 10000;
            timer.Start();           
        }

        static void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (DateTime.Now.AddSeconds(-20) > LastTime)
            {
                try
                {
                    System.IO.File.AppendAllText(@".\LostConnect.log", DateTime.Now.ToString() + " Lost Connect");
                }
                catch
                {
                }
                GetRoomObj();                
            }
        }

        static void RoomClient_RoomEvent(ControllEventType type, string Name, object obj)
        {
            if (type == ControllEventType.TimeConnect)
            {
                LastTime = DateTime.Now;
            }
            else if (RoomEvent != null)
            {
                RoomEvent(type, Name, obj);
            }
        }

        static public bool OpenDoor(string ControlID,int DoorNum,string UserID)
        {
            return GetRoomObj().OpenDoor(ControlID, DoorNum,UserID);          
        }

        static public byte[] GetStatus(string ControlID)
        {
            return GetRoomObj().GetADAMStatus(ControlID);
        }

        static public List<RoomInterface.PersonData> GetRoomPerson(string Room)
        {
            return GetRoomObj().GetRoomPerson(Room);
        }

        static public void Close()
        {
            if (roomObj != null && TestRoomObj())
            {
                roomObj.RoomEvent -= roomEvent.Cobj_RoomCEvent;
                roomObj = null;
            }
            if (timer != null)
                timer.Close();
        }


        static private IRoom GetRoomObj()
        {
            if (roomObj == null || !TestRoomObj())
            {
                lock (typeof(IRoom))
                {
                    if (roomObj == null || !TestRoomObj())
                    {
                        try
                        {
                            roomObj = (IRoom)Activator.GetObject(typeof(IRoom)
                                , objUrl);
                            roomObj.RoomEvent += new RoomEventHandler(roomEvent.Cobj_RoomCEvent);
                            LastTime = DateTime.Now;
                            if (RoomEvent != null)
                            {
                                RoomEvent(ControllEventType.HostConnect, string.Empty, null);
                            }
                        }
                        catch (Exception ex)
                        {
                            if (RoomEvent != null)
                            {
                                RoomEvent(ControllEventType.HostDisconnect, string.Empty, null);
                            }

                            try
                            {
                                System.IO.File.AppendAllText(@".\LostConnect.log", DateTime.Now.ToString() + " Host Connect File \r\n");
                            }
                            catch
                            {
                            }
                            Exception newEx = new Exception("主機連線失敗", ex);
                            throw newEx;
                        }
                    }
                }
            }
            return roomObj;
        }

        static private bool TestRoomObj()
        {
            try
            {
                roomObj.CheckConnect();
                return true;
            }
            catch
            {
                return false;
            }
        }

        static public RoomInterface.ControlStatus GetControlConnect(string ControlID)
        {
            return GetRoomObj().CheckControlConnect(ControlID);
        }

        static public bool RestaADAMControl(string ControlID)
        {
            return GetRoomObj().ResteADAMControl(ControlID);
        }

        static public void GroupModify(List<int> GroupID)
        {
            GetRoomObj().GroupModify(GroupID);
        }

        static public object[] GetGroupProgress()
        {
            return GetRoomObj().GetGroupProgress();
        }

        static public string GetGroupErrorMessage()
        {
            return GetRoomObj().GetGroupErrorMessage();
        }

        static public void ReloadPerson()
        {
            GetRoomObj().ReloadPersonName();
        }

        static public bool SetADAMAlarmTime(string ControlID, int RemoOpenTime, int DelayTime, int LoopErrorAlarmTime, int AlarmTime)
        {
            return GetRoomObj().SetADAMAlarmTime(ControlID,RemoOpenTime,DelayTime,LoopErrorAlarmTime,AlarmTime);
        }

        static public bool SetTime(string ControlID,DateTime time)
        {
            return GetRoomObj().SetTime(ControlID,time);
        }

    }

    public class RoomEventClass : RoomInterface.IRoomvent
    {
        public event RoomInterface.RoomEventHandler RoomEvent;

        public override void Cobj_RoomCEvent(RoomInterface.ControllEventType type, string Name, object obj)
        {
            if (RoomEvent != null)
            {
                RoomEvent(type, Name, obj);
            }
        }
    }
}

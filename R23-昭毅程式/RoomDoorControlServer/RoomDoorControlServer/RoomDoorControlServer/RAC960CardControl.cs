using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoomDoorControlServer
{
    class RAC960CardControl:IEnd
    {
        string Name;
        string IP;
        uint handle = 0;
        int timeout = 3000;
        public bool End { get { return end; } set { end = value; } }
        bool end = false;
        System.Threading.AutoResetEvent wait = new System.Threading.AutoResetEvent(false);
        byte RoomDoorType;
        string RoomName;
        int ReaderID;
        int block;

        bool DetectDoor;
        int DoorNum;     

        public RAC960CardControl(string ip)
        {
            IP = ip;
        }

        public RAC960CardControl(string name,string ip,string roomName,int readerID,byte roomDoorType,bool detectDoor)
        {
            Name = name;
            IP = ip;
            RoomName = roomName;
            ReaderID = readerID;
            RoomDoorType = roomDoorType;
            DetectDoor = detectDoor;
            if (detectDoor)
            {
                int.TryParse(name.Split('-').Last(), out DoorNum);
                DoorNum += 2;
                if (!ServerData.ADAMStatus.ContainsKey(RoomName))
                {
                    ServerData.ADAMStatus.Add(RoomName, new byte[33]);
                }
            }
                             
                                
        }

        internal void HostAction()
        {
            int waitTime = 5000;
            //if (RoomDoorType == 1)
            //{
            //    waitTime = 1000;
            //}

            SetTime();
            while (!End)
            {
                try
                {
                    wait.WaitOne(waitTime);
                    if (block > 0)
                        block--;
                    else
                    {                        
                        List<RAC960Record> readData = ReadRecord();                   
                        if (readData == null)
                        {                            
                            if (block < -1)
                            {
                                Random ramdom = new Random();
                                block = ramdom.Next(12, 36); ;
                                ServerData.CheckControlConnect(Name, false);
                            }
                            else
                            {
                                block--;
                            }
                            continue;
                        }
                        else
                        {
                            block = 0;
                            ServerData.CheckControlConnect(Name, true);
                        }
                        if (readData.Count > 0)
                        {
                            #region 卡片記錄
                            bool haveChange = false;
                            foreach (RAC960Record data in readData)
                            {
                                if (data.EventCode.Substring(2) == "00")//合法卡
                                {
                                    string cmd = string.Format("Insert into tbldeviceStateLog (TypeID,TypeCode,TimeStamp,ReaderID,ABA,ControlID) "
                                        + "Values({0},{1},'{2}',{3},'{4}','{5}');", 1, 1, data.DateTime.ToString("yyyy/MM/dd HH:mm:ss"), ReaderID, data.Card, Name);
                                    DatabaseAccess.DatabaseAcces(cmd);
                                    if (RoomDoorType == 1)//入
                                    {
                                        if (ServerData.RoomPerson.ContainsKey(RoomName))
                                        {
                                            //string PersonName = data.Card;
                                            //if (ServerData.PersonnelData.ContainsKey(data.Card))
                                            //{
                                            //    PersonName = ServerData.PersonnelData[data.Card];
                                            //    if (string.IsNullOrWhiteSpace(PersonName))
                                            //        PersonName = data.Card;
                                            //}
                                            List<RoomCardData> RoomPersonList = ServerData.RoomPerson[RoomName];
                                            bool HaveData = false;
                                            lock (RoomPersonList)
                                            {
                                                for (int i = 0; i < RoomPersonList.Count; i++)
                                                {
                                                    if (RoomPersonList[i].CardID == data.Card)
                                                    {
                                                        if (data.DateTime > RoomPersonList[i].LastTime)
                                                        {
                                                            RoomCardData roomCardData = RoomPersonList[i];
                                                            roomCardData.LastTime = data.DateTime;
                                                            if (!roomCardData.In)
                                                            {
                                                                roomCardData.In = true;
                                                                haveChange = true;
                                                                cmd = string.Format("Insert into tblEngineRoomLog (ERNo,StartTime,ABA,Result) "
                                                                    + "Values ('{0}','{1}','{2}',0);"
                                                                    , RoomName, data.DateTime.ToString("yyyy/MM/dd HH:mm:ss"), data.Card);
                                                                DatabaseAccess.DatabaseAcces(cmd);
                                                            }
                                                        }
                                                        HaveData = true;
                                                    }
                                                }
                                            }
                                            if (!HaveData)
                                            {
                                                RoomCardData newCardData = new RoomCardData(data.Card, ServerData.GetCardName(data.Card),ServerData.GetCarComp(data.Card), true, data.DateTime,false);
                                                RoomPersonList.Add(newCardData);
                                                haveChange = true;
                                                cmd = string.Format("Insert into tblEngineRoomLog (ERNo,StartTime,ABA,Result) "
                                                                   + "Values ('{0}','{1}','{2}',0);"
                                                                   , RoomName, data.DateTime.ToString("yyyy/MM/dd HH:mm:ss"), data.Card);
                                                DatabaseAccess.DatabaseAcces(cmd);
                                            }
                                        }
                                    }
                                    else if (RoomDoorType == 2)//出
                                    {
                                        if (ServerData.RoomPerson.ContainsKey(RoomName))
                                        {
                                            List<RoomCardData> RoomPersonList = ServerData.RoomPerson[RoomName];
                                            lock (RoomPersonList)
                                            {
                                                for (int i = 0; i < RoomPersonList.Count; i++)
                                                {
                                                    if (RoomPersonList[i].CardID == data.Card)
                                                    {
                                                        if (data.DateTime > RoomPersonList[i].LastTime)
                                                        {
                                                            RoomCardData roomCardData = RoomPersonList[i];
                                                            roomCardData.LastTime = data.DateTime;
                                                            if (roomCardData.In)
                                                            {
                                                                roomCardData.In = false;
                                                                haveChange = true;
                                                                cmd = string.Format("update tblEngineRoomLog Set EndTime = '{1}' "
                                                                   + "where ERNo = '{0}' and ABA = '{2}' "
                                                                   + "and StartTime = (Select Top 1 StartTime from tblEngineRoomLog where ERNo = '{0}' and ABA = '{2}' order by StartTime desc );"
                                                                   , RoomName, data.DateTime.ToString("yyyy/MM/dd HH:mm:ss"), data.Card);
                                                                DatabaseAccess.DatabaseAcces(cmd);
                                                            }
                                                            ServerData.SendRoomEvent(RoomInterface.ControllEventType.ReadCard, Name, data.Card);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                }
                                else if (data.EventCode.Substring(2) == "14")//非法卡
                                {
                                    string cmd = string.Format("Insert into tbldeviceStateLog (TypeID,TypeCode,TimeStamp,ReaderID,ABA,ControlID) "
                                        + "Values({0},{1},'{2}',{3},'{4}','{5}');", 1, 2, data.DateTime.ToString("yyyy/MM/dd HH:mm:ss"), ReaderID, data.Card, Name);
                                    DatabaseAccess.DatabaseAcces(cmd);
                                    string PersonName = data.Card;
                                    if (ServerData.PersonnelData.ContainsKey(data.Card))
                                    {
                                        PersonName = ServerData.PersonnelData[data.Card];
                                        if (string.IsNullOrWhiteSpace(PersonName))
                                            PersonName = data.Card;
                                    }
                                    ServerData.SendRoomEvent(RoomInterface.ControllEventType.ErrorCard, Name, PersonName);
                                }
                            }
                            if (haveChange)
                            {
                                List<RoomCardData> RoomPersonList = ServerData.RoomPerson[RoomName];
                                List<RoomInterface.PersonData> PersonList = new List<RoomInterface.PersonData>();
                                lock (RoomPersonList)
                                {
                                    foreach (RoomCardData data in RoomPersonList)
                                    {
                                        if (data.In)
                                        {
                                            PersonList.Add(new RoomInterface.PersonData(data.CardID, data.Name,data.Company,data.LastTime,data.IsManual));
                                        }
                                    }
                                }
                                ServerData.SendRoomEvent(RoomInterface.ControllEventType.RoomPersonChange, RoomName, PersonList);
                            }
                            #endregion                            
                        }
                        if (DetectDoor)
                        {
                            bool DoorStatus = getDoorStatus();
                            byte state = 0;
                            if (!DoorStatus)
                                state = 1;
                            if (ServerData.ADAMStatus[RoomName][DoorNum] != state)
                            {
                                ServerData.ADAMStatus[RoomName][DoorNum] = state;
                                ServerData.SendRoomEvent(RoomInterface.ControllEventType.ADAMStatusChange, RoomName, ServerData.ADAMStatus[RoomName]);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    TCommon.SaveLog(Name + ex.Message);
                }

                
            }
        }

        bool OpenConnect()
        {
     
            ServerData.waitServerTCPConnect();         
            int result = TNCUDll.OpenChanelEX(IP, 4660, 1, ref handle, timeout);
            ServerData.unregisterServerTCPConnect();
            

            if (result != 0)
            {                
                SaveMessageLog(IP, "OpenChannelEX error!error code :" + result);
                return false;
            }
            return true;
            

        }

        void CloseConnect()
        {
            int result = TNCUDll.CloseChannelEX(handle);
            if (result != 0)
            {
                SaveMessageLog(IP, "CloseChannelEX error!error code :" + result);
            }
        }

        internal bool? AddUserData(string ID,string message)
        {
            lock (this)
            {
                try
                {
                    if (!OpenConnect())
                        return null;
                    int result = -1;

                    result = TNCUDll.hacAddCardEX(1, ID, ID.Length, "", 0, message, System.Text.Encoding.GetEncoding("BIG5").GetByteCount(message), 0, 0, handle, timeout);
                    if (result == 1007)
                    {
                        return true;
                    }
                    else if (result != 0)
                    {
                        SaveMessageLog(IP, "hacAddCardEX error! errorCode :" + result);
                    }
                    return result == 0;
                }
                finally
                {
                    CloseConnect();
                }
            }
        }

        internal string AddAllUserData(List<string> cardList)
        {
            lock (this)
            {
                StringBuilder sb = new StringBuilder();
                OpenConnect();
                try
                {

                    foreach (string card in cardList)
                    {
                        int result = -1;
                        string[] strArr = card.Split(',');
                        string CardNO = strArr[0];
                        string CardMessage = strArr.Length > 1 ? strArr[1] : "";
                        if (CardMessage.Length == 0)
                            CardMessage = " ";
                        result = TNCUDll.hacAddCardEX(1, CardNO, CardNO.Length, "", 0, CardMessage, System.Text.Encoding.GetEncoding("BIG5").GetByteCount(CardMessage), 0, 0, handle, timeout);
                        if (result != 0)
                        {
                            sb.AppendLine(CardNO + "新增失敗" + " ErrorCode :" + result);
                        }
                    }

                }
                finally
                {
                    CloseConnect();
                }

                return sb.ToString();
            }
        }

        internal List<string> ReadAllUserData()
        {
            List<string> UserData = new List<string>();
            lock (this)
            {
                if (!OpenConnect())
                    return null;
                try
                {
                    int BufferSize = 128;
                    byte[] flashDate = new byte[BufferSize];
                    int StartPoint = 0x2000;
                    int reviceLen = 0;
                    int TimeoutCount = 0;
                    bool End = false;
                    do
                    {
                        int result = -1;
                        result = TNCUDll.hacGetFlashData(1, flashDate, ref reviceLen, StartPoint, BufferSize, handle, timeout);
                        if (result == 0)
                        {
                            int bufferPoint = 0;
                            TimeoutCount = 0;
                            do
                            {
                                string CardNo = TCommon.ByteArrayToString(flashDate, bufferPoint, 16).Split(':')[0];
                                if (string.IsNullOrEmpty(CardNo.Replace("ÿ", "")))
                                {
                                    End = true;
                                    break;
                                }
                                else
                                {
                                    string CardName = System.Text.Encoding.GetEncoding("BIG5").GetString(flashDate, bufferPoint + 16, 16);
                                    UserData.Add(CardNo + "," + CardName);
                                }
                                bufferPoint += 32;
                            }
                            while (bufferPoint < BufferSize);
                            StartPoint += BufferSize;
                        }
                        else if (result == 1025)
                        {
                            if (TimeoutCount < 3)
                                TimeoutCount++;
                            else
                                return null;
                        }
                        else
                        {
                            return null;
                        }
                        
                    }
                    while (!End && StartPoint < 0x27fff && (reviceLen != 0 || TimeoutCount < 3));
                }
                finally
                {
                    CloseConnect();
                }
                return UserData;
            }
        }

        internal string ReadUserData(int index)
        {
            List<string> UserData = new List<string>();
            lock (this)
            {
                if (!OpenConnect())
                    return null;
                try
                {
                    int BufferSize = 32;
                    byte[] flashDate = new byte[BufferSize];
                    int StartPoint = 0x2000;
                    int reviceLen = 0;
                    int TimeoutCount = 0;
                    bool End = false;
                    StartPoint += 32 * index;
                    string CardNo = "", CardName = "";
                    int result = -1;
                    result = TNCUDll.hacGetFlashData(1, flashDate, ref reviceLen, StartPoint, BufferSize, handle, timeout);
                    if (result == 0)
                    {
                        if (reviceLen == 0)
                            return "";
                        int bufferPoint = 0;
                        TimeoutCount = 0;
                        do
                        {
                            CardNo = TCommon.ByteArrayToString(flashDate, bufferPoint, 16).Split(':')[0];
                            if (string.IsNullOrEmpty(CardNo.Replace("ÿ", "")))
                            {
                                End = true;
                                break;
                            }
                            else
                            {
                                CardName = System.Text.Encoding.GetEncoding("BIG5").GetString(flashDate, bufferPoint + 16, 16);
                                UserData.Add(CardNo + "," + CardName);
                            }
                            bufferPoint += 32;
                        }
                        while (bufferPoint < BufferSize);
                        StartPoint += BufferSize;
                    }
                    else if (result == 1025)
                    {
                        if (TimeoutCount < 3)
                            TimeoutCount++;
                        else
                            return null;
                    }
                    else
                    {
                        return null;
                    }

                    return CardNo + "," + CardName;
                }
                finally
                {
                    CloseConnect();
                }
            }
        }

        internal bool? DelUserData(string ID)
        {
            lock (this)
            {
                if (!OpenConnect())
                    return null;
                try
                {
                    int result = -1;
                    result = TNCUDll.hacDelCard(1, ID, ID.Length, handle, timeout);
                    if (result == 1007)
                    {
                        return true;
                    }
                    else if (result != 0)
                    {
                        SaveMessageLog(IP, "hacDelCardEX error! errorCode :" + result);
                    }
                    return result == 0;
                }
                finally
                {
                    CloseConnect();
                }
            }
        }

        internal bool DelAllUserData()
        {
            lock (this)
            {
                OpenConnect();
                try
                {
                    int result = -1;
                    result = TNCUDll.hacInitial(1, 3, (byte)1, handle, timeout);
                    if (result == 0)
                    {
                        return true;
                    }
                    else
                        SaveMessageLog(IP, "hacInitial Error! ErrorCode:" + result);
                }
                finally
                {
                    CloseConnect();
                }
                return false;
            }
        }

        internal DateTime ReadTime()
        {
            OpenConnect();
            try
            {
                int result = -1;
                byte[] DateBytes = new byte[10],TimeBytes = new byte[10];
                result = TNCUDll.hacGetDateTime(1,  DateBytes,  TimeBytes, handle, timeout);
                if (result == 0)
                {
                    string Date = TCommon.ByteArrayToString(DateBytes);
                    string Time = TCommon.ByteArrayToString(TimeBytes);
                    return Convert.ToDateTime(Date.Substring(0,4) + "/" + Date.Substring(4,2) + "/" + Date.Substring(6,2) 
                        + " " + Time.Substring(0,2) + ":" + Time.Substring(2,2) + ":" + Time.Substring(4,2));
                }
                else
                {
                    SaveMessageLog(IP, "hacGetDateTime Error! ErrorCode :" + result);
                    return DateTime.MaxValue;
                }
            }
            finally
            {
                CloseConnect();
            }
        }

        internal bool SetTime()
        {
            lock (this)
            {
                if (!OpenConnect())
                    return false;
                try
                {
                    int result = -1;
                    DateTime time = DateTime.Now;
                    result = TNCUDll.hacSetDateTime(1, time.ToString("yyyyMMdd" + (int)time.DayOfWeek), time.ToString("HHmmss"), handle, timeout);
                    if (result != 0)
                    {
                        SaveMessageLog(IP, "hacSetDateTime Error! ErrorCode :" + result);
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                finally
                {
                    CloseConnect();
                }
            }
        }

        internal bool SetTime(DateTime time)
        {
            lock (this)
            {
                if (!OpenConnect())
                    return false;
                try
                {
                    int result = -1;
                    result = TNCUDll.hacSetDateTime(1, time.ToString("yyyyMMdd" + (int)time.DayOfWeek), time.ToString("HHmmss"), handle, timeout);
                    if (result != 0)
                    {
                        SaveMessageLog(IP, "hacSetDateTime Error! ErrorCode :" + result);
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
                finally
                {
                    CloseConnect();
                }
            }
        }

        internal List<RAC960Record> ReadRecord()
        {
            lock (this)
            { 
                try
                {
                    if (!OpenConnect())
                        return null;

                    int result = -1;
                    int iRecord = 0;
                    int rRecord = 12;
                    //PollList[] record = new PollList[255];
                    //record[0] = new PollList();
                    byte[] record = new byte[255 * 65];
                    List<RAC960Record> RecordList = new List<RAC960Record>();

                    do
                    {
                        result = TNCUDll.Polling(1, rRecord, record, ref iRecord, handle, timeout, 3);
                        if (result == 0)
                        {
                            object[] objList = TCommon.BufferToStuct(record, typeof(PollList), 0, iRecord);
                            rRecord = iRecord;
                            iRecord = 0;
                            foreach (object obj in objList)
                            {
                                PollList poll = (PollList)obj;
                                RecordList.Add(new RAC960Record(poll.EventCode, poll.DateTime, poll.Card, poll.DeviceID, poll.ReaderID));
                            }

                        }
                        else if (result == 1010)
                        {

                        }
                        else
                        {
                            SaveMessageLog(IP, "Polling error!error code :" + result);
                        }
                    }
                    while (result == 0);

                    return RecordList;
                }
                finally
                {
                    CloseConnect();
                }
            }

        }

        internal void readFlashData()
        {
            int result = -1;
            OpenConnect();
            try
            {
                byte[] flashDate = new byte[255];
                int reviceLen = 0;
                result = TNCUDll.hacGetFlashData(1, flashDate, ref reviceLen, 0, 80, handle, timeout);
                if (result == 0)
                {
                }
            }
            finally
            {
                CloseConnect();
            }
        }


        //true = Open
        internal bool getDoorStatus()
        {
            int result = -1;
            if (!OpenConnect())
                return false;
            try
            {
                int Sensor =0;
                result = TNCUDll.hacGetSensor(1, ref Sensor, handle, timeout);
                if (result == 0)
                {
                    Sensor = Sensor >> 1;
                    return Sensor % 2 == 0;
                }
                else
                    return false;
            }
            finally
            {
                CloseConnect();                
            }
        }

        static void SaveMessageLog(string ip, string Message)
        {
            CardControl.SaveMessageLog(ip , Message);
        }
    }

    public struct RAC960Record
    {
        public RAC960Record(byte[] eventCode, byte[] dateTime, byte[] card, byte[] deviceID, byte[] readerID)
        {
            EventCode = TCommon.ByteArrayToString(eventCode);
            DateTime = Convert.ToDateTime(TCommon.ByteArrayToString(dateTime));
            Card = TCommon.ByteArrayToString(card);
            DeviceID = TCommon.ByteArrayToString(deviceID);
            ReaderID = TCommon.ByteArrayToString(readerID);
        }

        public string EventCode;
        public DateTime DateTime;
        public string Card;
        public string DeviceID;
        public string ReaderID;
    }
}

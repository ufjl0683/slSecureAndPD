using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace RoomDoorControlServer
{
    enum LoginMode
    {        
        Set = 0,
        ReceiveCardLog = 1,
        ReceiveEventLog =2,
        ReceiveRealTime =3,
        ReceiveDoorStatus = 5 ,
        ReceiveAllCardLog =6,
        ReceiveLegalCardLog =7
    }

    enum LogType
    {
        CradLog =1,
        EventLog =2,
        RuntimeLog =3
    }

    class CardControl:IEnd
    {
        UInt32 handle = 0;
        bool IsLogin = false;
        LoginMode loginMode = LoginMode.ReceiveCardLog;
        int timeout = 3000;
        int ConnectTimeout = 120000;
        string Name;
        string Password = string.Empty;
        string IP = string.Empty;
        public bool End { get { return end; } set { end = value; } }
        bool end = false;
        byte[] readerType = new byte[4];
        int[] readerID = new int[4];
        byte[] readerIO = new byte[4];
        string RoomName;
        int RoomID;
        System.Threading.AutoResetEvent wait = new System.Threading.AutoResetEvent(false);
        int block;
   
        public CardControl(string ip)
        {
            IP = ip;
        }

        public CardControl(string name,string ip,string roomName,int[] ReaderID,byte[] ReaderIO,byte[] ReaderType,int? roomID)
        {
            Name = name;
            IP = ip;
            RoomName = roomName;
            readerID = ReaderID;
            readerIO = ReaderIO;
            readerType = ReaderType;
            RoomID = roomID.HasValue ? roomID.Value : 0;
        }

        public void HostAction()
        {
            SetTime();
            while (!end)
            {
                try
                {
                    wait.WaitOne(5000);
                    if (block > 0)
                        block--;
                    else
                    {
                        List<LogData> readlog = ReadLog(LogType.CradLog);                     
                        if (readlog == null)
                        {
                            if (block < -1)
                            {
                                Random ramdom = new Random();
                                block = ramdom.Next(12,36);
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
                            ServerData.CheckControlConnect(Name, true);
                        }
                        if (readlog.Count > 0)
                        {
                            ServerData.CheckControlConnect(Name, true);
                            bool haveChange = false;
                            foreach (LogData logData in readlog)
                            {
                                string CardNO = logData.CardNum;
                                if (logData.DeviceID == 22)
                                    logData.DeviceID = 0;
                                if (readerType[logData.DeviceID] == 1) //1 WEG
                                {
                                    CardNO = TCommon.GetABA(logData.CardNum);
                                }
                                if (logData.StatusCode == 1) //合法卡
                                {
                                    string cmd = string.Format("Insert into tbldeviceStateLog (TypeID,TypeCode,TimeStamp,ReaderID,ABA,ControlID) "
                                           + "Values({0},{1},'{2}',{3},'{4}','{5}');", 1, 1, logData.date.ToString("yyyy/MM/dd HH:mm:ss"), readerID[logData.DeviceID], CardNO, Name);
                                    DatabaseAccess.DatabaseAcces(cmd);
                                    if (readerIO[logData.DeviceID] == 1) //入
                                    {
                                        if (ServerData.RoomPerson.ContainsKey(RoomName))
                                        {
                                            string PersonName = CardNO;
                                            if (ServerData.PersonnelData.ContainsKey(CardNO))
                                            {
                                                PersonName = ServerData.PersonnelData[CardNO];
                                                if (string.IsNullOrWhiteSpace(PersonName))
                                                    PersonName = CardNO;                                                        
                                            }
                                            List<RoomCardData> RoomPersonList = ServerData.RoomPerson[RoomName];
                                            bool HaveData = false;
                                            lock (RoomPersonList)
                                            {
                                                for (int i = 0; i < RoomPersonList.Count; i++)
                                                {
                                                    if (RoomPersonList[i].CardID == CardNO)
                                                    {
                                                        if (logData.date > RoomPersonList[i].LastTime)
                                                        {
                                                            RoomCardData roomCardData = RoomPersonList[i];
                                                            roomCardData.LastTime = logData.date;
                                                            if (!roomCardData.In)
                                                            {
                                                                roomCardData.In = true;
                                                                haveChange = true;
                                                                cmd = string.Format("Insert into tblEngineRoomLog (ERNo,StartTime,ABA,Result) "
                                                                    + "Values ('{0}','{1}','{2}',0);"
                                                                    , RoomName, logData.date.ToString("yyyy/MM/dd HH:mm:ss"), CardNO);
                                                                DatabaseAccess.DatabaseAcces(cmd);
                                                            }
                                                        }
                                                        HaveData = true;
                                                        break;
                                                    }
                                                }
                                            }
                                            if (!HaveData)
                                            {
                                                RoomCardData newCardData = new RoomCardData(CardNO, PersonName,ServerData.GetCarComp(CardNO), true, logData.date,false);
                                                RoomPersonList.Add(newCardData);
                                                haveChange = true;
                                                cmd = string.Format("Insert into tblEngineRoomLog (ERNo,StartTime,ABA,Result) "
                                                                    + "Values ('{0}','{1}','{2}',0);"
                                                                    , RoomName, logData.date.ToString("yyyy/MM/dd HH:mm:ss"), CardNO);
                                                DatabaseAccess.DatabaseAcces(cmd);
                                            }
                                        }
                                    }
                                    else if (readerIO[logData.DeviceID] == 2)//出
                                    {
                                        if (ServerData.RoomPerson.ContainsKey(RoomName))
                                        {
                                            List<RoomCardData> RoomPersonList = ServerData.RoomPerson[RoomName];
                                            lock (RoomPersonList)
                                            {
                                                for (int i = 0; i < RoomPersonList.Count; i++)
                                                {
                                                    if (RoomPersonList[i].CardID == CardNO)
                                                    {
                                                        if (logData.date > RoomPersonList[i].LastTime)
                                                        {
                                                            RoomCardData roomCardData = RoomPersonList[i];
                                                            RoomPersonList.RemoveAt(i);
                                                            haveChange = true;
                                                            cmd = string.Format("update tblEngineRoomLog Set EndTime = '{1}' "
                                                                    + "where ERNo = '{0}' and ABA = '{2}' "
                                                                    + "and StartTime = (Select Top 1 StartTime from tblEngineRoomLog where ERNo = '{0}' and ABA = '{2}' order by StartTime desc);"
                                                                    , RoomName, logData.date.ToString("yyyy/MM/dd HH:mm:ss"), CardNO);
                                                            DatabaseAccess.DatabaseAcces(cmd);
                                                        }
                                                        ServerData.SendRoomEvent(RoomInterface.ControllEventType.ReadCard, Name, CardNO);
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (logData.StatusCode == 7)//非法卡
                                {
                                    string cmd = string.Format("Insert into tbldeviceStateLog (TypeID,TypeCode,TimeStamp,ReaderID,ABA,ControlID) "
                                            + "Values({0},{1},'{2}',{3},'{4}','{5}');", 1, 2, logData.date.ToString("yyyy/MM/dd HH:mm:ss"), readerID[logData.DeviceID], CardNO, Name);
                                    DatabaseAccess.DatabaseAcces(cmd);
                                    string PersonName = CardNO;
                                    if (ServerData.PersonnelData.ContainsKey(CardNO))
                                    {
                                        PersonName = ServerData.PersonnelData[CardNO];
                                        if (string.IsNullOrWhiteSpace(PersonName))
                                            PersonName = CardNO;
                                    }
                                    ServerData.SendRoomEvent(RoomInterface.ControllEventType.ErrorCard, Name, PersonName);
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
                                                PersonList.Add(new RoomInterface.PersonData(data.CardID, data.Name,data.Company,data.LastTime,false));
                                            }
                                        }
                                    }
                                    ServerData.SendRoomEvent(RoomInterface.ControllEventType.RoomPersonChange, RoomName, PersonList);
                                }
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

        bool OpenConnect(bool IsSet)
        {
         
            int resultCode = -1;
            int result = -1;

            ServerData.waitServerTCPConnect();
            if (IsSet)
            {
                resultCode = TNCUDll.NCUOpenSocket(ref handle, IP, 0, ConnectTimeout);
            }
            else
            {
                resultCode = TNCUDll.NCUOpenSocket(ref handle, IP, 1, ConnectTimeout);
            }
            ServerData.unregisterServerTCPConnect();
            if (resultCode == 0)
            {        
                return true;
            }
            else
            {
                SaveMessageLog(IP, "NCUOpenSocket error!error code:" + Convert.ToString(resultCode) + "!result code: " + result);
                return false;
            }            
        }

        void CloseConnect()
        {
            if (handle != 0)
            {
                int resultCode = -1;
                IsLogin = false;
                resultCode = TNCUDll.NCUCloseSocket(handle);
                if (resultCode != 0)
                {
                    SaveMessageLog(IP, "NCUOpenSocket error!error code:" + Convert.ToString(resultCode) + "!");
                }
            }
        }

        bool Login(LoginMode mode)
        {
            if (IsLogin && loginMode == mode)
                return true;
            int returnCode =0;
            if (IsLogin && loginMode != mode)         
            {                
                if (loginMode == LoginMode.Set)
                {
                    TNCUDll.LogoutNCU(handle, 0, ref returnCode, timeout);
                }
                else
                {
                    TNCUDll.LogoutNCU(handle, 1, ref returnCode, timeout);
                }
                IsLogin = false;
            }
            int resultCode = TNCUDll.LoginNCU(handle, (int)mode, "", ref returnCode, timeout);
            if (resultCode == 0)
            {
                IsLogin = true;
                loginMode = mode;
                return true;
            }
            else
            {
                SaveMessageLog(IP, "LoginNCU error!error code:" + Convert.ToString(resultCode) + "!result code: " + resultCode);
                return false;
            }
        }

        void Logout()
        {
            int returnCode = 0;
            if (loginMode == LoginMode.Set)
            {
                TNCUDll.LogoutNCU(handle, 0, ref returnCode, timeout);
            }
            else
            {
                TNCUDll.LogoutNCU(handle, 1, ref returnCode, timeout);
            }
            IsLogin = false;            
        }

        internal void FindDevice()
        {
            int resultCode = -1;
            int result = -1;
            int timeout = 5000;
            byte[] dataByte = new byte[1024];
            int receiveLen = 0;
            int NCUs = 0;
            try
            {
                resultCode = TNCUDll.FindNCU(dataByte, ref receiveLen, ref NCUs, timeout);
                int startPosition = 0;
                if (resultCode == 0)
                {
                    for (int i = 0; i < NCUs; i++)
                    {
                        int infoLen = dataByte[startPosition];
                        SNCUInfo NCUInfo = (SNCUInfo)TCommon.BytesToStuct(dataByte, typeof(SNCUInfo), startPosition + 1);
                        string sSubDev = "";
                        SDeviceInfo deviceInfo;
                        for (int j = 0; j < NCUInfo.ItemCount; j++)
                        {
                            deviceInfo = (SDeviceInfo)TCommon.BytesToStuct(dataByte, typeof(SDeviceInfo), startPosition + 1 + Marshal.SizeOf(typeof(SNCUInfo)) + Marshal.SizeOf(typeof(SDeviceInfo)) * j);
                            if (deviceInfo.DeviceType == 255) continue;
                            sSubDev += "ID :" + deviceInfo.DeviceID + " type:" + deviceInfo.DeviceType + ";";
                        }
                        startPosition = startPosition + infoLen + 1;
                    }
                }
            }
            finally
            {
            }
        }

        internal List<LogData> ReadLog(LogType readType)
        {
            List<LogData> log = new List<LogData>();
            lock (this)
            {
                try
                {
                    if (!OpenConnect(false))
                        return null;
                    if (Login(LoginMode.ReceiveCardLog))
                    {
                        int resultCode = -1;
                        int result = 0;
                        int recordCount = 0;
                        byte[] dataByte = new byte[4096];

                        do
                        {
                            resultCode = TNCUDll.ReceiveRecord(handle, (int)readType, dataByte, ref recordCount, ref result, 3000);
                            if (resultCode == 0)
                            {
                                for (int k = 0; k < recordCount; k++)
                                {
                                    LogData data = new LogData(dataByte, k * 70);
                                    log.Add(data);
                                }
                            }                           
                        } while (resultCode == 0  && recordCount > 6);
                        Logout();
                        //resultCode = TNCUDll.ReceiveLog(handle, (int)readType, ref deviceId, date, time, ref status, cardId, ref cardCode, ref iEvent, ref result, timeout);
                        //if (resultCode == 0)
                        //{
                        //    log.Add(new LogData(iEvent, date, time, cardId, deviceId, status));
                        //}
                        //resultCode = TNCUDll.ReceiveLog(handle, (int)readType, ref deviceId, date, time, ref status, cardId, ref cardCode, ref iEvent, ref result, timeout);
                        //if (resultCode == 0)
                        //{
                        //    log.Add(new LogData(iEvent, date, time, cardId, deviceId, status));
                        //}
                    }
                }
                finally
                {
                    CloseConnect();
                }
            }
            return log;
            
        }

        internal string AddUserData(List<string> CardList)
        {
            lock (this)
            {
                if (!OpenConnect(true))
                {
                    CloseConnect();
                    return "斷線";
                }
                StringBuilder ErrorMessage = new StringBuilder();
                try
                {
                    Login(LoginMode.Set);

                    int resultCode = -1;
                    int result = -1;
                    byte[] ReceiveData = new byte[20];
                    int ReceiveLen = 0;
                    foreach (string Card in CardList)
                    {
                        string[] strArr = Card.Split(',');
                        string CardABA = strArr[0];
                        string CardWEG = strArr.Length > 1 ? strArr[1] : string.Empty;
                        string CardMessage = strArr.Length > 2 ? strArr[2] : string.Empty;
                        //string door = strArr.Length > 3 ? strArr[3] : "1111";

                        if (CardWEG != "" && CardWEG.Length == 10)
                        {
                            int weg1 = Convert.ToInt32(CardWEG.Substring(0, 5));
                            weg1 = weg1 & 255;
                            CardWEG = weg1.ToString("00000") + CardWEG.Substring(5, 5);
                        }
                        else
                        {
                            CardWEG = TCommon.GetWEG(CardABA);
                        }

                        byte[] SendData = GetUserDataByte(CardABA, CardMessage + "_a");
                        resultCode = TNCUDll.NCUSet(handle, 0x0201, SendData, SendData.Length, ReceiveData, ref ReceiveLen, ref result, timeout);
                        if (resultCode != 0)
                        {
                            string error = string.Empty;
                            if (resultCode == 1001 && result == 4)
                            {

                            }
                            else
                            {
                                if (resultCode == 1001 && result == 14)
                                {
                                    IsLogin = false;
                                    Login(LoginMode.Set);
                                }
                                else if (resultCode == 1001 && result != 4)
                                {
                                    error = GetErrorMessage(result);                                   
                                }
                                ErrorMessage.AppendLine(CardABA + "新增失敗!" + error);
                            }
                            
                        }
                        SendData = GetUserDataByte(CardWEG, CardMessage + "_w");

                        resultCode = TNCUDll.NCUSet(handle, 0x0201, SendData, SendData.Length, ReceiveData, ref ReceiveLen, ref result, timeout);
                        if (resultCode != 0)
                        {
                            string error = string.Empty;
                            if (resultCode == 1001 && result == 4)
                            {

                            }
                            else
                            {
                                if (resultCode == 1001 && result == 14)
                                {
                                    IsLogin = false;
                                    Login(LoginMode.Set);
                                }
                                else if (resultCode == 1001 && result != 4)
                                {
                                    error = GetErrorMessage(result);                                    
                                }
                                ErrorMessage.AppendLine(CardWEG + "新增失敗!" + error);
                            }
                        }
                    }

                }
                finally
                {
                    CloseConnect();
                }
                return ErrorMessage.ToString();
            }
        }

        string GetErrorMessage(int ErrorCode)
        {
            switch (ErrorCode)
            {
                case 1:
                    return "資料長度與合法卡長度不符";
                case 2:
                    return "合法卡長度不符";
                case 3:
                    return "已超過可容納的筆數";
                case 4:
                    return "卡號已存在";
                case 5:
                    return "系統參數設定值錯誤";
                case 6:
                    return "卡號不存在";
                case 10:
                    return "裝置無回應";
                case 14:
                    return "未登入";
                case 16:
                    return "已有登入者";
                case 17:
                    return "已超過可連線數量";
            }
            return ErrorCode.ToString();
        }

        byte[] GetUserDataByte(string CardNo, string Message,string Door)
        {
            byte[] NoBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(CardNo);
            byte[] MessageBytes = System.Text.UTF8Encoding.GetEncoding("BIG5").GetBytes(Message);
            byte[] SendData = new byte[42];
            SendData[0] = 129;
            for (int i = 0; i < 10; i++)
            {
                SendData[i + 1] = NoBytes[i];
            }
            for (int i = 0; i < 17; i++)
            {
                if (i < MessageBytes.Length)
                {
                    SendData[i + 15] = MessageBytes[i];
                }
            }
            for (int i = 0; i < 4; i++)
            {
                SendData[i + 32] = 255;
            }
            for (int i = 0; i < 4; i++)
            {
                if (i < Door.Length && Door[i] == '1')
                {
                    SendData[i + 38] = 255;
                }
                else
                {
                    SendData[i + 38] = 0;
                }
            }
            return SendData;
        }

        byte[] GetUserDataByte(string CardNo, string Message)
        {
            byte[] NoBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(CardNo);
            byte[] MessageBytes = System.Text.UTF8Encoding.GetEncoding("BIG5").GetBytes(Message);
            byte[] SendData = new byte[42];
            SendData[0] = 129;
            for (int i = 0; i < 10; i++)
            {
                SendData[i + 1] = NoBytes[i];
            }
            for (int i = 0; i < 17; i++)
            {
                if (i < MessageBytes.Length)
                {
                    SendData[i + 15] = MessageBytes[i];
                }
            }
            for (int i = 0; i < 4; i++)
            {
                SendData[i + 32] = 255;
            }
            for (int i = 0; i < 4; i++)
            {
                SendData[i + 38] = 255;
            }
            return SendData;
        }

        internal bool? AddUserData(string CardNo,string Message,string Door)
        {
            lock (this)
            {
                if (!OpenConnect(true))
                    return null;
                try
                {
                    Login(LoginMode.Set);

                    int resultCode = -1;
                    int result = -1;
                    //byte[] timeZone = new byte[30];
                    //string sTimeZone = Convert.ToString(gvwCardList.CurrentRow.Cells["cTimeZone"].Value);
                    //for (int i = 0; i < timeZone.Length; i++)
                    //{
                    //    timeZone[i] = 255;
                    //}
                    //resultCode = TNCUDll.InsertUserRecord(handle, 129, 0, 10, CardNo, 4, "", 17, 1, 1, "", 0, "", "", 0, 0, 0, 4,
                    //                                        timeZone, ref result, timeout);
                    byte[] ReceiveData = new byte[20];
                    int ReceiveLen = 0;
                    byte[] NoBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(CardNo);
                    byte[] MessageBytes = System.Text.UTF8Encoding.GetEncoding("BIG5").GetBytes(Message);
                    byte[] SendData = new byte[42];
                    SendData[0] = 129;
                    for (int i = 0; i < 10; i++)
                    {
                        SendData[i + 1] = NoBytes[i];
                    }
                    for (int i = 0; i < 17; i++)
                    {
                        if (i < MessageBytes.Length)
                        {
                            SendData[i + 15] = MessageBytes[i];
                        }
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        SendData[i + 32] = 255;
                    }
                    for (int i = 0; i < 4; i++)//門
                    {
                        if (i < Door.Length && Door[i] == '1')
                        {
                            SendData[i + 38] = 255;
                        }
                        else
                        {
                            SendData[i + 38] = 0;
                        }
                    }
                    resultCode = TNCUDll.NCUSet(handle, 0x0201, SendData, SendData.Length, ReceiveData, ref ReceiveLen, ref result, timeout);

                    if (resultCode == 1001  && (result == 4))
                    {
                        return true;
                    }                    
                    return resultCode == 0;
                }
                finally
                {
                    CloseConnect();
                }
            }
        }

        internal bool? DelUserData(string CardNO)
        {
            lock (this)
            {
                if (!OpenConnect(true))
                    return null;
                try
                {
                    Login(LoginMode.Set);
                    int returnCode = 0;
                    int resultCode = -1;
                    resultCode = TNCUDll.DeleteUserRecord(handle, 0, 10, CardNO, ref returnCode, timeout);
                    if (resultCode == 1001 && returnCode == 6)
                    {
                        return true;
                    }
                    return resultCode == 0;                     
                }
                finally
                {
                    CloseConnect();
                }
            }

        }

        internal List<CardData> ReadAllUserData()
        {
            lock (this)
            {
                try
                {
                    if (!OpenConnect(false))
                        return null;
                    int resultCode = -1;
                    int result = -1;
                    int dataLen = 0;
                    byte[] recordByte = new byte[20480];
                    if (!Login(LoginMode.ReceiveLegalCardLog))
                        return null;
                    List<CardData> cardData = new List<CardData>();

                    do
                    {
                        resultCode = TNCUDll.ReceiveAllUserRecord(handle, recordByte, ref dataLen, ref result, timeout);
                        if (resultCode == 0)
                        {
                            int startPoint = 2;
                            while (startPoint < dataLen)
                            {
                                CardData car = new CardData();
                                car.active = recordByte[startPoint++];
                                car.CarNum = TCommon.ByteArrayToString(recordByte, startPoint, 10);
                                car.Password = TCommon.ByteArrayToString(recordByte, startPoint + 10, 4);
                                car.Message = System.Text.UTF8Encoding.GetEncoding("BIG5").GetString(recordByte, startPoint + 14, 17);//9);
                                startPoint += 31;
                                if (recordByte[startPoint + 2] == 255)
                                {
                                    car.VoildDate = DateTime.MaxValue;
                                }
                                else
                                {
                                    car.VoildDate = new DateTime(TCommon.BCDByteToInt(recordByte[startPoint]) * 100 + TCommon.BCDByteToInt(recordByte[startPoint + 1])
                                        , TCommon.BCDByteToInt(recordByte[startPoint + 2]), TCommon.BCDByteToInt(recordByte[startPoint + 3]));
                                }
                                car.Holiday = recordByte[startPoint + 4];
                                car.sub = recordByte[startPoint + 5];
                                startPoint += 6;
                                for (int i = 0; i < 4; i++)
                                {
                                    car.doorTime[i] = recordByte[startPoint + i];
                                }
                                startPoint += 4;
                                cardData.Add(car);
                                Console.WriteLine(car.CarNum + " " + car.Message);
                            }
                        }
                        else
                        {
                            SaveMessageLog(IP, "ReceiveAllUserRecord error!error code:" + Convert.ToString(resultCode) + "!return code:" + result);
                            //if (cardData.Count != 0)
                            //    return cardData;
                            //else
                                return null;
                        }
                    } while (recordByte[0] == 12);
                    return cardData;
                }
                finally
                {
                    CloseConnect();
                }
            }
        }

        internal RAC2400Paramenter ReadParamenter()
        {
            lock (this)
            {
                OpenConnect(true);
                try
                {
                    Login(LoginMode.Set);

                    int resultCode = -1;
                    int result = -1;
                    byte[] dataBuffer = new byte[12];
                    int dataLength = 0;
                    resultCode = TNCUDll.ReadParamenter(handle, dataBuffer, ref dataLength, ref result, timeout);
                    if (resultCode == 0)
                    {
                        RAC2400Paramenter Param = new RAC2400Paramenter();
                        Param.MaxCradNum = dataBuffer[0];
                        Param.MaxAccommodate = dataBuffer[1];
                        Param.CardLen = (byte)(dataBuffer[2] & 0x7f);
                        Param.CompressModel = (dataBuffer[2] & 0x80) == 1;
                        Param.PWD = Convert.ToString(dataBuffer[3]);
                        Param.MsgLen = dataBuffer[4];
                        Param.DateField = (dataBuffer[5] == 1);
                        Param.Holiday = (dataBuffer[6] == 1);
                        Param.DualCard = (dataBuffer[7] == 1);
                        Param.DoorCount = dataBuffer[8];
                        Param.Record = dataBuffer[9] == 1;
                        return Param;
                    }
                    else
                    {
                        SaveMessageLog(IP, "ReadParamenter error!error code :" + Convert.ToString(resultCode) + "!return code:" + result);
                        return null;
                    }
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
                OpenConnect(true);
                try
                {
                    Login(LoginMode.Set);
                    int resultCode = -1;
                    int result = -1;
                    resultCode = TNCUDll.DeleteAllUserRecord(handle, ref result, timeout);
                    if (resultCode == 0)
                    {
                        CloseConnect();
                        return true;
                    }
                    else
                    {
                        SaveMessageLog(IP, "DeleteAllUserRecord error! ErrorCode:" + resultCode + " result:" + result);
                        CloseConnect();
                        return false;
                    }
                }
                finally
                {
                    CloseConnect();
                }
            }
        }

        internal bool SetDoorNum(byte num)
        {
            if (num > 16)
                throw new Exception("門數設定超過最大值16");
            lock (this)
            {
                OpenConnect(true);
                try
                {
                    Login(LoginMode.Set);
                    int resultCode = -1;
                    int result = -1;
                    byte[] dataBuffer = new byte[12];
                    int dataLength = 0;
                    resultCode = TNCUDll.ReadParamenter(handle, dataBuffer, ref dataLength, ref result, timeout);
                    if (resultCode == 0)
                    {
                        dataBuffer[8] = num;
                        resultCode = -1;
                        resultCode = TNCUDll.WriteParamenter(handle, dataBuffer, dataBuffer.Length, ref result, timeout);
                        if (resultCode == 0)
                        {
                            return true;
                        }
                        else
                        {
                            SaveMessageLog(IP, "WriteParamenter Error! ErrorCode :" + resultCode + " result :" + result);
                        }
                    }
                    else
                    {
                        SaveMessageLog(IP, "ReadParamenter Error! ErrorCode :" + resultCode + " result :" + result);
                    }
                }
                finally
                {
                    CloseConnect();
                }
            }
            return false;
        }

        internal DateTime ReadTime()
        {
            int resultCode = -1;
            int result = -1;
            byte[] dataDate = new byte[10];
            byte[] dataTime = new byte[10];
            lock (this)
            {
                try
                {
                    OpenConnect(true);
                    Login(LoginMode.Set);
                    resultCode = TNCUDll.ReadNCUDateTime(handle, dataDate, dataTime, ref result, timeout);
                    if (resultCode == 0)
                    {
                        return Convert.ToDateTime(TCommon.ByteArrayToString(dataDate) + " " + TCommon.ByteArrayToString(dataTime));
                    }
                    else
                    {
                        SaveMessageLog(IP, "ReadNCUDateTime error!error code :" + Convert.ToString(resultCode) + "!return code:" + result);
                        return DateTime.MinValue;
                    }
                }
                finally
                {
                    CloseConnect();
                }
            }
        }

        internal bool SetTime()
        {
            int resultCode = -1;
            int returnCode = 0;
            lock (this)
            {
                try
                {
                    OpenConnect(true);
                    Login(LoginMode.Set);
                    DateTime Time = DateTime.Now;
                    resultCode = TNCUDll.WriteNCUDateTime(handle, Time.ToString("yyyyMMdd"), Time.ToString("HHmmss"), ref returnCode, timeout);
                    if (returnCode != 0)
                    {
                        SaveMessageLog(IP, "WriteNCUDateTime error!error code :" + Convert.ToString(resultCode) + "!return code:" + returnCode);
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
            int resultCode = -1;
            int returnCode = 0;
            lock (this)
            {
                try
                {
                    OpenConnect(true);
                    Login(LoginMode.Set);
                    resultCode = TNCUDll.WriteNCUDateTime(handle, time.ToString("yyyyMMdd"), time.ToString("HHmmss"), ref returnCode, timeout);
                    if (returnCode != 0)
                    {
                        SaveMessageLog(IP, "WriteNCUDateTime error!error code :" + Convert.ToString(resultCode) + "!return code:" + returnCode);
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

        static internal void SaveMessageLog(string ip, string Message)
        {
            try
            {
                TCommon.SaveLog("IP:" + ip + ", " + Message);
            }
            catch
            {
                ;
            }
        }

        static internal List<SNCUInfo> FindNuc()
        {
            int resultCode = -1;
            int result = -1;
            int timeout = 5000;
            byte[] dataByte = new byte[1024];
            int receiveLen=0;
            int NCUs = 0;

            List<SNCUInfo> NcuList = new List<SNCUInfo>();
            try
            {
                resultCode = TNCUDll.FindNCU(dataByte, ref receiveLen, ref NCUs, timeout);
                int startPosition = 0;
                if (resultCode == 0)
                {
                    for (int i = 0; i < NCUs; i++)
                    {
                        int infoLen = dataByte[startPosition];
                        SNCUInfo NCUInfo = (SNCUInfo)TCommon.BytesToStuct(dataByte, typeof(SNCUInfo), startPosition + 1);
                        NcuList.Add(NCUInfo);
                        startPosition += infoLen + 1;
                    }
                }
                else
                {
                    SaveMessageLog("", "FindNCU error! ErrorCode :" + resultCode + " resultCode :" + result);
                }
                return NcuList;
            }
            finally
            {
               
            }
        }
    }

    public class LogData
    {
        //public LogData(int iEvent,byte[] iDate,byte[] iTime,byte[] iCradID,int iDeviceID,int iStatus)
        //{
        //    EventCode = iEvent;
        //    date = Convert.ToDateTime(TCommon.ByteArrayToString(iDate) + " " + TCommon.ByteArrayToString(iTime));
        //    CardNum = TCommon.ByteArrayToString(iCradID);
        //    DeviceID = iDeviceID;
        //    StatusCode = iStatus;
        //}

        public LogData(byte[] data, int start)
        {
            string tmp;
            tmp = System.Text.ASCIIEncoding.ASCII.GetString(data, start, 10).Replace("\0","");
            EventCode = tmp == "" ? 0 : Convert.ToInt32(tmp,16);
            
            date = Convert.ToDateTime(System.Text.ASCIIEncoding.ASCII.GetString(data, start + 10, 20));
            CardNum = System.Text.ASCIIEncoding.ASCII.GetString(data, start + 30, 20).Replace("\0", "");
            DeviceID = Convert.ToInt32(System.Text.ASCIIEncoding.ASCII.GetString(data, start + 50, 10).Replace("\0", ""), 16);
            StatusCode = Convert.ToInt32(System.Text.ASCIIEncoding.ASCII.GetString(data, start + 60, 10).Replace("\0", ""), 16);
        }

        public int EventCode;
        public DateTime date;
        public string CardNum;
        public int DeviceID;
        public int StatusCode;
    }

    public class CardData
    {
        public byte active {get;set;}
        public string CarNum { get; set; }
        public string Password { get; set; }
        public string Message { get; set; }
        public DateTime VoildDate { get; set; }
        public byte Holiday { get; set; }
        public byte sub { get; set; }
        public byte[] doorTime = new byte[16];
    }

    public class RAC2400Paramenter
    {
        public byte MaxCradNum;
        public byte MaxAccommodate;
        public byte CardLen;
        public bool CompressModel;
        public string PWD;
        public int MsgLen;
        public bool DateField;
        public bool Holiday;
        public bool DualCard;
        public byte DoorCount;
        public bool Record;
    }
}

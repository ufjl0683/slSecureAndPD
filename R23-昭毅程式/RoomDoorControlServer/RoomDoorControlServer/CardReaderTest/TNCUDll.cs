using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace RoomDoorControlServer
{
    internal class TNCUDll
    {
        #region DLL函数引用
        //Find NCU from NetWork
        [DllImport("HDACS.dll", EntryPoint = "hsFindNCU")]
        public static extern int FindNCU(byte[] byteBuffer, ref int receiveLength, ref int iNCUs, int timeout);
        //Broadcast Sets To NCU (Not supports Yet)
        [DllImport("HDACS.dll", EntryPoint = "hsBroadcastNCUSet")]
        public static extern int BroadcastNCUSet(string cNCUMacAddress, string cNCUPassWord, string cNCUNewIP, string cNCUNewSubmask, string cNCUNewGateway, int iTimeOut);
        //Open a NCU Connection
        //iMode: 0: Set Mode (Card,Control...etc)   1: Receive Mode(event,log...etc)
        [DllImport("HDACS.dll", EntryPoint = "hsNCUOpenSocket")]
        public static extern int NCUOpenSocket(ref UInt32 hComm, string NCUIP, int iMode, int iTimeOut);
        //Login To NCU.
        [DllImport("HDACS.dll", EntryPoint = "hsLoginNCU")]
        public static extern int LoginNCU(UInt32 hComm, int iMode, string cPassword, ref int iReturnCode, int iTimeOut);
        //Login Out From NCU
        [DllImport("HDACS.dll", EntryPoint = "hsLogoutNCU")]
        public static extern int LogoutNCU(UInt32 hComm, int iMode, ref int iReturnCode, int iTimeOut);
        //close NCU socket
        [DllImport("HDACS.dll", EntryPoint = "hsNCUCloseSocket")]
        public static extern int NCUCloseSocket(UInt32 hComm);
        //Receive Log
        [DllImport("HDACS.dll", EntryPoint = "hsReceiveLog")]
        public static extern int ReceiveLog(UInt32 hComm, int iType, ref int iDeviceID, byte[] cDate, byte[] cTime, ref int iStatus,
                                                            byte[] cCardNo, ref int iCardCode, ref int iEvent, ref int iReturnCode, int iTimeout);
        //new Receive Log
        [DllImport("HDACS.dll", EntryPoint = "hsReceiveRecord")]
        public static extern int ReceiveRecord(UInt32 hComm, int iType, byte[] cDate, ref int iRecordNum, ref int iResultCode, int iTimeout);
        //Get Door Statue
        [DllImport("HDACS.dll", EntryPoint = "hsReceiveDoorStatus")]
        public static extern int ReceiveDoorStatus(UInt32 THandle, ref int iDoorStatus, ref int iReturnCode, int iTimeOut);
        //Change NCU HDACS,SubMask,GateWay
        [DllImport("HDACS.dll", EntryPoint = "hsChangeIP")]
        public static extern int ChangeIP(UInt32 hComm, string cNCUIP, string cNCUSubMask, string cNCUGateWay, ref int iRecutrnCode, int iTimeOut);
        //Change NCU Login Password
        [DllImport("HDACS.dll", EntryPoint = "hsChangeNCUPassword")]
        public static extern int ChangeNCUPassword(UInt32 hComm, string cPassword, ref int iReturnCode, int iTimeOut);
        [DllImport("HDACS.dll", EntryPoint = "hsNCUReadTable")]
        public static extern int NCUReadTable(UInt32 hComm, int iTable, byte[] cReceiveData, ref int iReceiveLen, ref int iReturnCode, int iTimeOut);
        /*
         Write Table Set to NCU
         Params like function hsNCUReadTable;
        */
        [DllImport("HDACS.dll", EntryPoint = "hsNCUWriteTable")]
        public static extern int NCUWriteTable(UInt32 THandle, int iTable, byte[] cSendData, int iSendLen, ref int iRecutrnCode, int iTimeOut);
        //Read Parameters From NCU
        [DllImport("HDACS.dll", EntryPoint = "hsReadParamenter")]
        public static extern int ReadParamenter(UInt32 hComm, byte[] cParaData, ref int iParaLen, ref int iReturnCode, int iTimeOut);
        //Write Parameters To NCU
        [DllImport("HDACS.dll", EntryPoint = "hsWriteParamenter")]
        public static extern int WriteParamenter(UInt32 hComm, byte[] cParaData, int iParaLen, ref int iReturnCode, int iTimeOut);
        //Get NCU Polling Device ID
        [DllImport("HDACS.dll", EntryPoint = "hsGetDeivePollingID")]
        public static extern int GetDeivePollingID(UInt32 hComm, byte[] cPollData, ref int iPollLen, ref int iReturnCode, int iTimeOut);
        //setup NCU Polling Device ID
        [DllImport("HDACS.dll", EntryPoint = "hsSetDeivePollingID")]
        public static extern int SetDeivePollingID(UInt32 hComm, byte[] cPollData, int iPollLen, ref int iReturnCode, int iTimeOut);

        //Set NCU Polling TimeOut
        [DllImport("HDACS.dll", EntryPoint = "hsSetNCUPollingTimeout")]
        public static extern int SetNCUPollingTimeout(UInt32 hComm, int iPollTimeOut, ref int iReturnCode, int iTimeOut);

        //Get NCU Polling Timeout
        [DllImport("HDACS.dll", EntryPoint = "hsGetNCUPollingTimeout")]
        public static extern int GetNCUPollingTimeout(UInt32 hComm, ref int iPollTimeOut, ref int iReturnCode, int iTimeOut);
        //GetNCU Information
        [DllImport("HDACS.dll", EntryPoint = "hsGetNCUInfo")]
        public static extern int GetNCUInfo(UInt32 hComm, byte[] cInfoData, ref int iInfoLen, ref int iReturnCode, int iTimeOut);
        //Get NCU Device List
        [DllImport("HDACS.dll", EntryPoint = "hsGetNCUDeviceType")]
        public static extern int GetNCUDeviceType(UInt32 hComm, byte[] cTypeData, ref int iTypeLen, ref int iReturnCode, int iTimeOut);
        //Search NCU Device
        [DllImport("HDACS.dll", EntryPoint = "hsSearchNCUDevice")]
        public static extern int SearchNCUDevice(UInt32 hComm, int iStartID, int iEndID, byte[] cTypeData, ref int iTypeLen, ref int iReturnCode, int iTimeOut);
        //Init NCU
        [DllImport("HDACS.dll", EntryPoint = "hsInitialNCU")]
        public static extern int InitialNCU(UInt32 hComm, byte cInitFlag, ref int iReturnCode, int iTimeOut);
        //ReadNCUDateTime
        [DllImport("HDACS.dll", EntryPoint = "hsReadNCUDateTime")]
        public static extern int ReadNCUDateTime(UInt32 hComm, byte[] cDate, byte[] cTime, ref int iReturnCode, int iTimeOut);
        //WriteNCUDateTime
        [DllImport("HDACS.dll", EntryPoint = "hsWriteNCUDateTime")]
        public static extern int WriteNCUDateTime(UInt32 hComm, string cDate, string cTime, ref int iReturnCode, int iTimeOut);
        //Check NCU Online
        [DllImport("HDACS.dll", EntryPoint = "hsIsNCUOnLine")]
        public static extern int IsNCUOnLine(UInt32 hComm, ref int iReturnCode, int iTimeOut);
        //Read Device Params
        [DllImport("HDACS.dll", EntryPoint = "hsReadDevice")]
        public static extern int ReadDevice(UInt32 hComm, int iDeviceID, int iReadTimeOut, int iIndex, byte[] cReadData, ref int iReadLen, ref int iReturnCode, int iTimeOut);
        //Write Device Params
        [DllImport("HDACS.dll", EntryPoint = "hsWriteDevice")]
        public static extern int WriteDevice(UInt32 hComm, int iDeviceID, int iReadTimeOut, int iIndex, byte[] cSendData, int iSendLen, ref int iReturnCode, int iTimeOut);

        [DllImport("HDACS.dll", EntryPoint = "hsInsertUserRecord")]
        public static extern int InsertUserRecord(UInt32 hComm, int iType, int iCompressed, int iCardLen, string cCardNo, int iPassLen, string cPassword, int iDisplayLen, int iDisplayType, int iDisplayID, string cDisplayText,
                                                        int iLimitDate, string cStartDate, string cEndDate, int iHoliday, int iGroupType, int iGroup, int iDoors, byte[] cTimeZone, ref int iReturnCode, int iTimeOut);
        //like the hsInsertUserRecord. But The User Must Exists !
        [DllImport("HDACS.dll", EntryPoint = "hsUpdateUserRecord")]
        public static extern int UpdateUserRecord(UInt32 hComm, int iType, int iCompressed, int iCardLen, string cCardNo, int iPassLen, string cPassword,
                               int iDisplayLen, int iDisplayType, int iDisplayID, string cDisplayText, int iLimitDate, string cStartDate, string cEndDate, int iHoliday, int iGroupType,
                               int iGroup, int iDoors, byte[] cTimeZone, ref int iReturnCode, int iTimeOut);
        //Delete a user record Info
        [DllImport("HDACS.dll", EntryPoint = "hsDeleteUserRecord")]
        public static extern int DeleteUserRecord(UInt32 hComm, int iCompressed, int iCardLen, string cCardNo, ref int iReturnCode, int iTimeOut);
        //query exists user record information
        [DllImport("HDACS.dll", EntryPoint = "hsQueryUserRecord")]
        public static extern int QueryUserRecord(UInt32 hComm, int iCompressed, int iCardLen, string cCardNo, byte[] cReceiveData, ref int iReceiveLen, ref int iReturnCode, int iTimeout);
        //Insert Multi User Record into NCU
        [DllImport("HDACS.dll", EntryPoint = "hsInsertAllUserRecord")]
        public static extern int InsertAllUserRecord(UInt32 hComm, int iRecord, byte[] stRecord, ref int iReturnCode, int iTimeout);
        //Delete All User Record From NCU
        [DllImport("HDACS.dll", EntryPoint = "hsDeleteAllUserRecord")]
        public static extern int DeleteAllUserRecord(UInt32 hComm, ref int iReturnCode, int iTimeOut);
        //Get Legal Card
        [DllImport("HDACS.dll", EntryPoint = "hsReceiveAllUserRecord")]
        public static extern int ReceiveAllUserRecord(UInt32 hComm, byte[] cRecordData, ref int iRecordLen, ref int iReturnCode, int iTimeOut);
        [DllImport("HDACS.dll", EntryPoint = "hsNCUSet")]        
        public static extern int NCUSet(UInt32 hComm, int iFunction, byte[] cSendData, int iSendLen, byte[] cReceiveData, ref int iReceiveLen, ref int iReturnCode, int iTimeout);
        [DllImport("HDACS.dll", EntryPoint = "hacOpenChannelEX")]
        public static extern int OpenChanelEX(string sIP, UInt32 iPort, int iCheckStatus, ref UInt32 hComm, int iTimeout);
        [DllImport("HDACS.dll", EntryPoint = "hacCloseChannel")]
        public static extern int CloseChannelEX(UInt32 hComm);
        [DllImport("HDACS.dll", EntryPoint = "hacPolling")]
        public static extern int Polling(int iNodeID, int iPrevRecord, byte[] stRecord, ref int iRecord, UInt32 hComm, int iTimeout, int iCardType);
        [DllImport("HDACS.dll", EntryPoint = "hacAddCard")]
        public static extern int hacAddcard(int iNodeID, string cCardNO, int iCardLen, string cPassword, int iPassLen, int iTimeZone, byte cStatus, UInt32 hComm, int iTimeout);
        [DllImport("HDACS.dll",EntryPoint="hacGetParaData")]
        public static extern int hacGetParaData(int iNodeID,byte[] cFlashData,ref int iReceiveDataLen,UInt32 hComm,int Timeout);
        [DllImport("HDACS.dll", EntryPoint = "hacGetFlashData")]
        public static extern int hacGetFlashData(int iNodeID, byte[] cFlashDatg, ref int IReceiveDataLen,  int iFlashAddr,  int iFlashLen, UInt32 hComm, int Timeout);
        [DllImport("HDACS.dll", EntryPoint = "hacAddCardEX")]
        public static extern int hacAddCardEX(int iNodeID, string cCardNO, int iCardLen, string cPassword, int iPassLen, string cName, int iNameLen, int iTimeZone, byte cStatus, UInt32 hComm, int Timeout);
        [DllImport("HDACS.dll", EntryPoint = "hacDelCard")]
        public static extern int hacDelCard(int iNodeID, string cCardNo, int iCardLen, UInt32 hComm, int iTimeOut);
        [DllImport("HDACS.dll", EntryPoint = "hacGetDateTime")]
        public static extern int hacGetDateTime(int iNodeID, byte[] cDate, byte[] cTime, UInt32 hComm, int iTimeout);
        [DllImport("HDACS.dll", EntryPoint = "hacSetDateTime")]
        public static extern int hacSetDateTime(int iNodeID, string cDate, string cTime, UInt32 hComm, int iTimeout);
        [DllImport("HDACS.dll", EntryPoint = "hacInitial")]
        public static extern int hacInitial(int iNodeID, int iDeviceType, byte iClearFlag, UInt32 hComm, int iTimeout);
        [DllImport("HDACS.dll", EntryPoint = "hacGetSensor")]
        public static extern int hacGetSensor(int iNodeID, ref int iSensor, UInt32 hComm, int iTimeout);
        #endregion
    }

    
}

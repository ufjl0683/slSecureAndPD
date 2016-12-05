using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace slSecure.Web
{
    /// <summary>
    ///MCNSService 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
    // [System.Web.Script.Services.ScriptService]
    public class MCNSService : System.Web.Services.WebService, SecureService.ISecureServiceCallback
    {

        System.Collections.Generic.List<R23DoorInfo> d_R23DoorInfo = new List<R23DoorInfo>();
        [WebMethod]
        public string HelloWorld()
        {
            return   "Hello World";
        }
       

        [WebMethod]
        public RoomInfo[] GetAllRoom()
        {
            using (SecureDBEntities db = new SecureDBEntities())
            {
                return db.tblEngineRoomConfig.Select(n => new RoomInfo() { ERName = n.ERName, ERID = n.ERID ,LineID=n.LineID}).ToArray();
            }
        }

      

        [WebMethod]
        public AddCardInfo[] GetCardInfoByMCNSID(string MCNSID)
        {
            using (SecureDBEntities db = new SecureDBEntities())
            {
                var q = from n in db.vwMagneticCardDetail
                        where  n.MCNSID==MCNSID
                        group (int)n.ERID by new { n.ABA, n.Memo, n.StartDate, n.EndDate, n.Name } into g
                        select new  { g.Key.ABA, g,   g.Key.Memo,    g.Key.EndDate,  g.Key.StartDate, Name = g.Key.Name };

                System.Collections.Generic.List<AddCardInfo> list = new List<AddCardInfo>();
                foreach (var i in q)
                {
                    list.Add(new AddCardInfo() {  CardNo = i.ABA, EndDate = (DateTime)i.EndDate, StartDate = (DateTime)i.StartDate, MCNSID = i.Memo, Name = i.Name, ERIDs = i.g.Distinct().ToArray<int>() });
                }
                return list.ToArray();
            }
        }
        [WebMethod]
        public MagneticCardBasicInfo[] GetAllTempMagneticCardBasicInfo()
        {
            using (SecureDBEntities db = new SecureDBEntities())
            {
                var q = from n in db.tblMagneticCard
                        where n.Enable == "Y" 
                        select new MagneticCardBasicInfo { ABA = n.ABA, Type=n.Type, CardEndDate = n.CardEndDate, CardStartDate = n.CardStartDate, Company = n.Company, EmployeeNo = n.EmployeeNo, EndDate = n.EndDate, IDNumber = n.IDNumber, JobTitle = n.JobTitle, Mobile = n.Mobile, Name = n.Name, StartDate = n.StartDate, Tel = n.Tel };


                return q.ToArray();
                //System.Collections.Generic.List<MagneticCardBasicInfo> list = new List<MagneticCardBasicInfo>();
                //foreach (var i in q)
                //{
                //    list.Add(new AddCardInfo() { CardNo = i.ABA, EndDate = (DateTime)i.EndDate, StartDate = (DateTime)i.StartDate, MCNSID = i.Memo, Name = i.Name, ERIDs = i.g.Distinct().ToArray<int>() });
                //}
                //return list.ToArray();
            }
        }
        [WebMethod]
      public   MagneticCardBasicInfo[] GetMagneticCardBasicInfoByCompany(string CompanyName)
        {
            using (SecureDBEntities db = new SecureDBEntities())
            {
                var q = from n in db.tblMagneticCard
                        where n.Enable == "Y" && n.Company == CompanyName
                       
                        select new MagneticCardBasicInfo{  ABA=n.ABA,Type=n.Type ,CardEndDate=n.CardEndDate, CardStartDate=n.CardStartDate, Company=n.Company, EmployeeNo=n.EmployeeNo, EndDate=n.EndDate, IDNumber=n.IDNumber, JobTitle=n.JobTitle, Mobile=n.Mobile, Name=n.Name, StartDate=n.StartDate, Tel=n.Tel };


                return q.ToArray();
                //System.Collections.Generic.List<MagneticCardBasicInfo> list = new List<MagneticCardBasicInfo>();
                //foreach (var i in q)
                //{
                //    list.Add(new AddCardInfo() { CardNo = i.ABA, EndDate = (DateTime)i.EndDate, StartDate = (DateTime)i.StartDate, MCNSID = i.Memo, Name = i.Name, ERIDs = i.g.Distinct().ToArray<int>() });
                //}
                //return list.ToArray();
            }


        }
       


#if R23

         [WebMethod]
        public System.Collections.Generic.List<R23DoorInfo> GetR23DoorInfo()
        {
            clsDBComm commDB = new clsDBComm();

            DataTable dt = new DataTable();
            DataTable dtSelect = new DataTable();
            string cmd = "";

            SecureService.SecureServiceClient client = new SecureService.SecureServiceClient(new System.ServiceModel.InstanceContext(this), "CustomBinding_ISecureService");

            //client.SetR23EngineRoomRecovery("AT");覆歸
            cmd = string.Format("select * from tblEngineRoomConfig where ERID not in (18,22,23,24);"); //過濾無門禁功能 18國姓1號東口機房 22國姓1號隧道 23國姓2號隧道 24埔里隧道  
            dt = commDB.SelectDBData(cmd);
            try
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow ERData in dt.Rows)
                    {
                        R23DoorInfo GDI = new R23DoorInfo();
                        SecureService.PersonData[] RoomPerson;
                        RoomPerson = client.GetR23RoomPerson(ERData["ERNo"].ToString());//查看讀卡機人員清單；取最後一人刷進記錄(公司+姓名)

                        if (RoomPerson.Count() > 0)
                        {
                            string sComp = "";
                            string sName = "";

                            if (RoomPerson.Last().COMP == null)
                            {
                                sComp = "";
                            }
                            else
                            {
                                sComp = RoomPerson.Last().COMP;
                            }

                            if (RoomPerson.Last().NAME == null)
                            {
                                sName = "";
                            }
                            else
                            {
                                sName = RoomPerson.Last().NAME;
                            }


                            if (sComp != "")
                            {
                                if (sComp == "中工處南投工務段")
                                    sComp = sComp.Substring(3, 2);
                                else
                                    sComp = sComp.Substring(0, 2);
                            }
                            if (sName == "強制開門帳號")
                            {
                                sName = "強制開門";
                            }
                            if ((sName == "") && (sComp == ""))
                            {
                                GDI.StayDoorName = RoomPerson.Last().CARDNO;
                            }
                            else
                            {
                                GDI.StayDoorName = sComp + sName;
                            }
                        }
                        //else
                        //{
                        //    GDI.StayDoorName = "none";
                        //}

                        cmd = string.Format("select * from vwEntranceGuardDetail where ERName='{0}';", ERData["ERName"].ToString());

                        dtSelect = commDB.SelectDBData(cmd);

                        if (dtSelect.Rows.Count > 0)
                        {
                            bool SingleHardWareState1 = false;
                            bool SingleHardWareState0 = false;

                            foreach (DataRow vwEGDData in dtSelect.Rows)
                            {
                                SecureService.ControlStatus ControlConnect;
                                ControlConnect = client.GetR23ControlConnect(vwEGDData["ControlID"].ToString());//查詢機房硬體狀態

                                if (ControlConnect.connect)   //連線   
                                    SingleHardWareState1 = true;
                                if (!ControlConnect.connect)  //斷線
                                    SingleHardWareState0 = true;


                                byte[] ReaderStatus;

                                if ((vwEGDData["ControlType"].ToString() == "1") || (vwEGDData["ControlID"].ToString() == "AB-960N-1") || (vwEGDData["ControlID"].ToString() == "AG-960N-1"))
                                {
                                    if ((bool)vwEGDData["IsEnable"])
                                    {
                                        //查看機房ADAM之狀態
                                        if ((vwEGDData["ControlID"].ToString() == "AB-960N-1") || (vwEGDData["ControlID"].ToString() == "AG-960N-1"))//苗栗or中交控
                                        {
                                            //ReaderStatus = client.GetR23ReaderStatus(vwEGDData["ERNo"].ToString());//("AB"/"AG");
                                            if (RoomPerson.Count() > 0)
                                            {
                                                GDI.IsShowPerson = true;
                                                GDI.state = 1;//有人逗留 
                                                GDI.stateAlarm = 1;
                                            }
                                            else
                                            {
                                                GDI.IsShowPerson = false;
                                                GDI.state = 0;     //無人在內 
                                                GDI.stateAlarm = 2;//保全啟動
                                            }
                                        }
                                        else//非苗栗or中交控 無控制器 讀取讀卡機  
                                        {
                                            ReaderStatus = client.GetR23ReaderStatus(vwEGDData["ControlID"].ToString());//("AN-2400N-1");
                                            if (ReaderStatus != null)
                                            {
                                                if (ReaderStatus[32] != 0)
                                                {
                                                    if (RoomPerson.Count() > 0) //查讀卡機是否有人員逗留
                                                    {
                                                        GDI.IsShowPerson = true;
                                                        GDI.state = 1; //有人逗留 

                                                        if (ReaderStatus[32] == 2)//當收到狀態為「保全啟動」在此要強制把stateAlarm設定為「人員逗留」狀態
                                                            GDI.stateAlarm = 1;              //有人逗留 
                                                        else
                                                            GDI.stateAlarm = ReaderStatus[32];
                                                    }
                                                    else
                                                    {
                                                        if (ReaderStatus[32] == 1)           //門禁被開啟
                                                        {
                                                            GDI.StayDoorName = "門禁被開啟";
                                                            GDI.state = 1; //有人逗留 
                                                            GDI.IsShowPerson = true;
                                                        }
                                                        else
                                                        {
                                                            GDI.IsShowPerson = false;
                                                            GDI.state = 0; //無人在內
                                                        }
                                                        GDI.stateAlarm = ReaderStatus[32];               //保全警報  

                                                    }
                                                }
                                                else
                                                {
                                                    GDI.IsShowPerson = false;
                                                    GDI.state = 3;     //斷線
                                                    GDI.stateAlarm = ReaderStatus[32];               //保全警報 
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if ((SingleHardWareState1) && (SingleHardWareState0))
                            {
                                GDI.ERName = ERData["ERName"].ToString();
                                //為硬體異常(只要state有一個為true)
                                GDI.stateHardware = 1;
                            }
                            else if ((SingleHardWareState1) && (!SingleHardWareState0))
                            {
                                GDI.ERName = ERData["ERName"].ToString();
                                //為硬體正常
                                GDI.stateHardware = 0;
                            }
                            else
                            {
                                //全部顯示斷線
                                GDI.ERName = ERData["ERName"].ToString();
                                GDI.stateHardware = 2;
                                GDI.IsShowPerson = false;
                                GDI.state = 3;
                                GDI.stateAlarm = 0;
                            }
                            d_R23DoorInfo.Add(GDI);
                        }
                        //ControlConnect = client.GetR23ControlConnect("AA-2400N-1");
                        //RoomPerson = client.GetR23RoomPerson("AA");
                        //ReaderStatus = client.GetR23ReaderStatus("AA-2400N-1");

                        //if (RoomPerson.Count() > 0)
                        //{
                        //    string aa = RoomPerson.Last().COMP;
                        //}


                        //string status = ReaderStatus[32].ToString();

                        //GDI.ERName = row["ERName"].ToString();
                        //GDI.stateHardware = 0;
                        //GDI.IsShowPerson = false;
                        //GDI.StayDoorName = "";
                        //GDI.state = 0;
                        //GDI.stateAlarm = ReaderStatus[32];
                        //if (!d_R23DoorInfo.ContainsKey(row["ERID"].ToString()))
                        //{
                        //    d_R23DoorInfo.Add(row["ERID"].ToString(), GDI);
                        //}

                    }
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message + "," + ex.StackTrace;
                throw ex;
            }
            return d_R23DoorInfo;

        }
        [WebMethod]
        public string AddCard(AddCardInfo[] infos)
        {
            //string rooms = ""; string.Join(",", info.ERIDs);
            //  return "0: add " + info.CardNo + " to  " + rooms + "  success!";

            List<string> list = new List<string>();
            try
            {
                foreach (AddCardInfo info in infos)
                {

                    for (int i = 0; i < info.ERIDs.ToArray().Length; i++)
                    {
                        string roleid = R23Exchange(info.MCNSID, info.ERIDs[i], info.CardNo, info.Name, info.StartDate, info.EndDate);
                        list.Add(roleid);
                    }
                }

                SecureService.SecureServiceClient client = new SecureService.SecureServiceClient(new System.ServiceModel.InstanceContext(this));
                if(list.Count>0)
                client.NotifyDBChange(SecureService.DBChangedConstant.AuthorityChanged,string.Join("," ,list.ToArray()));

                return "0:success!";
            }
            catch (Exception ex)
            {
                return "-1:" + ex.Message + "," + ex.StackTrace;
            }
        }

        [WebMethod]
        public object[]  GetProgress()
        {

              SecureService.SecureServiceClient client = new SecureService.SecureServiceClient(new System.ServiceModel.InstanceContext(this));
              return  client.GetR23Progress();
        }

        [WebMethod]
        public CardSendingResult[] GetR23CardSendingResult(string mcnsid)
        {
            IQueryable<CardSendingResult> q;
            
            using (SecureDBEntities db = new SecureDBEntities())
            {
                 q = from n in db.vwCardCommandLog where n.MCNSID == mcnsid select new CardSendingResult() {
                     ABA=n.ABA,
                      ComandTypeName=n.CommandTypeName,
                       ERName=n.ERName,
                        IsSuccessName=n.IsSuccessName,
                         Memo=n.Memo,
                          MSNCID=n.MCNSID,
                           Name=n.Name,
                            TimeStamp=n.Timestamp

                };
                 if (q == null)
                     return new CardSendingResult[0];
                 return q.ToArray();
                
            }

           
        }


        #region ==== 用水用電功能Start =====
        #region "取得機房，並以 DataTable 方式回傳"
        public static DataTable GetAllERNameDate()
        {
            clsDBComm commDB = new clsDBComm();
            DataTable DT = new DataTable();

            try
            {
                string selectCmd = "";
                selectCmd += "select ERID";
                selectCmd += " From tblPowerMeter";
                DT = commDB.SelectDBData(selectCmd);
            }
            catch (Exception ex)
            {
                var message = ex.Message + "," + ex.StackTrace;
                throw ex;
            }
            return DT;
        }
        #endregion

        #region "取得本期起始日期，並以 DataTable 方式回傳"
        public static DataTable GetThisPeriodStartDay()
        {
            clsDBComm commDB = new clsDBComm();
            DataTable DT = new DataTable();

            try
            {
                string selectCmd = "";
                selectCmd += "select VariableValue";
                selectCmd += " From tblSysParameter";
                selectCmd += " where  VariableName='ThisPeriodStartDay'";
                DT = commDB.SelectDBData(selectCmd);
            }
            catch (Exception ex)
            {
                var message = ex.Message + "," + ex.StackTrace;
                throw ex;
            }
            return DT;
        }
        #endregion

        #region "取得本期結束日期，並以 DataTable 方式回傳"
        public static DataTable GetThisPeriodEndDay()
        {
            clsDBComm commDB = new clsDBComm();
            DataTable DT = new DataTable();

            try
            {
                string selectCmd = "";
                selectCmd += "select VariableValue";
                selectCmd += " From tblSysParameter";
                selectCmd += " where  VariableName='ThisPeriodEndDay'";
                DT = commDB.SelectDBData(selectCmd);
            }
            catch (Exception ex)
            {
                var message = ex.Message + "," + ex.StackTrace;
                throw ex;
            }
            return DT;
        }
        #endregion


        static DataTable dt = GetAllERNameDate();

        //static DataTable dtStartDay = GetThisPeriodStartDay();
        //static string sStartDay = dtStartDay.Rows[0]["VariableValue"].ToString() + " 00:00:00";
        //static DataTable dtEndDay = GetThisPeriodEndDay();
        //static string sEndDay = dtEndDay.Rows[0]["VariableValue"].ToString() + " 23:59:59";
        //static string s_year = DateTime.Now.AddYears(-1).Year.ToString();

        //public static void YearData()
        //{
        //    if (s_year == "2015")
        //        s_year = "2016";
        //}

        //計算去年度用電每月日平均累積值

            [WebMethod]
        public void PowerBaseValueDailyAvg(string s_year)
        {
            clsDBComm commDB = new clsDBComm();
            DataTable dtYear = new DataTable();
            bool IsSuccess = false;
            string cmd = "";

            //YearData();
            string sSelect = "(select  CASE MAX(kWh) - MIN(kWh) when 0 then MAX(kWh) - MIN(kWh) else CAST(( (MAX(kWh) - MIN(kWh))/30 ) AS decimal(18, 3)) END   AS KWAlarm from vwPowerMeter1HourLog where (1=1)  and year(Timestamp)= ";
            for (int ExeCount = 0; ExeCount < dt.Rows.Count; ExeCount++)
            {
                //先判別是否為新的年份
                cmd = string.Format("select * from tblPowerAndWaterMeterYearMonthDailyAvg where YearsNo='{0}' and ERID={1};", s_year, dt.Rows[ExeCount]["ERID"].ToString());
                dtYear = commDB.SelectDBData(cmd);
                if (dtYear.Rows.Count > 0)
                {
                    try
                    {
                        string text = "UPDATE tblPowerAndWaterMeterYearMonthDailyAvg SET  ";

                        text += " Power1MonthDailyAvg  = " + sSelect + s_year + " and month(Timestamp)=1 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += ",Power2MonthDailyAvg  = " + sSelect + s_year + " and month(Timestamp)=2 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += ",Power3MonthDailyAvg  = " + sSelect + s_year + " and month(Timestamp)=3 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += ",Power4MonthDailyAvg  = " + sSelect + s_year + " and month(Timestamp)=4 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += ",Power5MonthDailyAvg  = " + sSelect + s_year + " and month(Timestamp)=5 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += ",Power6MonthDailyAvg  = " + sSelect + s_year + " and month(Timestamp)=6 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += ",Power7MonthDailyAvg  = " + sSelect + s_year + " and month(Timestamp)=7 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += ",Power8MonthDailyAvg  = " + sSelect + s_year + " and month(Timestamp)=8 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += ",Power9MonthDailyAvg  = " + sSelect + s_year + " and month(Timestamp)=9 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += ",Power10MonthDailyAvg = " + sSelect + s_year + " and month(Timestamp)=10 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += ",Power11MonthDailyAvg = " + sSelect + s_year + " and month(Timestamp)=11 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += ",Power12MonthDailyAvg = " + sSelect + s_year + " and month(Timestamp)=12 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";

                        text += ",[PowerUpdateDate] = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                        text += ",[Memo] = 'SYS' ";
                        text += " WHERE [YearsNo] = " + s_year + " and [ERID] = " + dt.Rows[ExeCount]["ERID"].ToString();

                        IsSuccess = commDB.ModifyDBData(text);
                    }
                    catch (Exception ex)
                    {
                        var message = ex.Message + "," + ex.StackTrace;
                        throw ex;
                    }
                }
                else
                {
                    try
                    {
                        string text = "INSERT INTO tblPowerAndWaterMeterYearMonthDailyAvg (YearsNo,ERID,Power1MonthDailyAvg,Power2MonthDailyAvg,Power3MonthDailyAvg,Power4MonthDailyAvg,Power5MonthDailyAvg,Power6MonthDailyAvg,Power7MonthDailyAvg,Power8MonthDailyAvg,Power9MonthDailyAvg,Power10MonthDailyAvg,Power11MonthDailyAvg,Power12MonthDailyAvg,PowerUpdateDate,Memo)VALUES  ";

                        text += "(" + s_year + "," + dt.Rows[ExeCount]["ERID"].ToString();
                        text += "," + sSelect + s_year + " and month(Timestamp)=1 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += "," + sSelect + s_year + " and month(Timestamp)=2 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += "," + sSelect + s_year + " and month(Timestamp)=3 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += "," + sSelect + s_year + " and month(Timestamp)=4 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += "," + sSelect + s_year + " and month(Timestamp)=5 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += "," + sSelect + s_year + " and month(Timestamp)=6 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += "," + sSelect + s_year + " and month(Timestamp)=7 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += "," + sSelect + s_year + " and month(Timestamp)=8 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += "," + sSelect + s_year + " and month(Timestamp)=9 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += "," + sSelect + s_year + " and month(Timestamp)=10 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += "," + sSelect + s_year + " and month(Timestamp)=11 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += "," + sSelect + s_year + " and month(Timestamp)=12 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";

                        text += ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                        text += ",'SYS') ";


                        IsSuccess = commDB.ModifyDBData(text);
                    }
                    catch (Exception ex)
                    {
                        var message = ex.Message + "," + ex.StackTrace;
                        throw ex;
                    }
                }
            }
        }

        //計算去年度用水每月日平均累積值
        [WebMethod]
        public void WaterBaseValueDailyAvg(string s_year)
        {
            clsDBComm commDB = new clsDBComm();
            DataTable dtYear = new DataTable();
            bool IsSuccess = false;
            string cmd = "";

            //YearData();
            string sSelect = "(select  CASE MAX(CumulateValueAlarm) - MIN(CumulateValueAlarm) when 0 then MAX(CumulateValueAlarm) - MIN(CumulateValueAlarm) else CAST(( (MAX(CumulateValueAlarm) - MIN(CumulateValueAlarm))/30 ) AS decimal(18, 3)) END   AS CumulateValueAlarm from vwPowerMeter1HourLog where (1=1)and ERID != 4 and year(Timestamp)= ";
            for (int ExeCount = 0; ExeCount < dt.Rows.Count; ExeCount++)
            {
                //先判別是否為新的年份
                cmd = string.Format("select * from tblPowerAndWaterMeterYearMonthDailyAvg where YearsNo='{0}' and ERID={1};", s_year, dt.Rows[ExeCount]["ERID"].ToString());
                dtYear = commDB.SelectDBData(cmd);
                if (dtYear.Rows.Count > 0)
                {
                    try
                    {
                        string text = "UPDATE tblPowerAndWaterMeterYearMonthDailyAvg SET  ";
                        text += " Water1MonthDailyAvg  = " + sSelect + s_year + " and month(Timestamp)=1 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += ",Water2MonthDailyAvg  = " + sSelect + s_year + " and month(Timestamp)=2 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += ",Water3MonthDailyAvg  = " + sSelect + s_year + " and month(Timestamp)=3 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += ",Water4MonthDailyAvg  = " + sSelect + s_year + " and month(Timestamp)=4 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += ",Water5MonthDailyAvg  = " + sSelect + s_year + " and month(Timestamp)=5 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += ",Water6MonthDailyAvg  = " + sSelect + s_year + " and month(Timestamp)=6 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += ",Water7MonthDailyAvg  = " + sSelect + s_year + " and month(Timestamp)=7 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += ",Water8MonthDailyAvg  = " + sSelect + s_year + " and month(Timestamp)=8 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += ",Water9MonthDailyAvg  = " + sSelect + s_year + " and month(Timestamp)=9 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += ",Water10MonthDailyAvg = " + sSelect + s_year + " and month(Timestamp)=10 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += ",Water11MonthDailyAvg = " + sSelect + s_year + " and month(Timestamp)=11 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += ",Water12MonthDailyAvg = " + sSelect + s_year + " and month(Timestamp)=12 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";

                        text += ",[WaterUpdateDate] = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                        text += ",[Memo] = 'SYS' ";
                        text += " WHERE [YearsNo] = " + s_year + " and [ERID] = " + dt.Rows[ExeCount]["ERID"].ToString();

                        IsSuccess = commDB.ModifyDBData(text);
                    }
                    catch (Exception ex)
                    {
                        var message = ex.Message + "," + ex.StackTrace;
                        throw ex;
                    }
                }
                else
                {
                    try
                    {
                        string text = "INSERT INTO tblPowerAndWaterMeterYearMonthDailyAvg (YearsNo,ERID,Water1MonthDailyAvg,Water2MonthDailyAvg,Water3MonthDailyAvg,Water4MonthDailyAvg,Water5MonthDailyAvg,Water6MonthDailyAvg,Water7MonthDailyAvg,Water8MonthDailyAvg,Water9MonthDailyAvg,Water10MonthDailyAvg,Water11MonthDailyAvg,Water12MonthDailyAvg,WaterUpdateDate,Memo)VALUES  ";

                        text += "(" + s_year + "," + dt.Rows[ExeCount]["ERID"].ToString();
                        text += "," + sSelect + s_year + " and month(Timestamp)=1 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += "," + sSelect + s_year + " and month(Timestamp)=2 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += "," + sSelect + s_year + " and month(Timestamp)=3 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += "," + sSelect + s_year + " and month(Timestamp)=4 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += "," + sSelect + s_year + " and month(Timestamp)=5 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += "," + sSelect + s_year + " and month(Timestamp)=6 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += "," + sSelect + s_year + " and month(Timestamp)=7 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += "," + sSelect + s_year + " and month(Timestamp)=8 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += "," + sSelect + s_year + " and month(Timestamp)=9 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += "," + sSelect + s_year + " and month(Timestamp)=10 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += "," + sSelect + s_year + " and month(Timestamp)=11 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                        text += "," + sSelect + s_year + " and month(Timestamp)=12 and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";

                        text += ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                        text += ",'SYS') ";


                        IsSuccess = commDB.ModifyDBData(text);
                    }
                    catch (Exception ex)
                    {
                        var message = ex.Message + "," + ex.StackTrace;
                        throw ex;
                    }
                }


            }
        }

        //計算去年度用電每月累積值
        [WebMethod]
        public void PowerThisPeriodValue()
        {
            //DataTable dtStartDay = GetThisPeriodStartDay();
            //DateTime tmp_StartDate = Convert.ToDateTime(dtStartDay.Rows[0]["VariableValue"]);
            //string sStartDay = tmp_StartDate.ToString("yyyy-MM-dd HH:mm:ss");

            //DataTable dtEndDay = GetThisPeriodEndDay();
            //DateTime tmp_EndDate = Convert.ToDateTime(dtEndDay.Rows[0]["VariableValue"]);
            //string sEndDay = tmp_EndDate.ToString("yyyy-MM-dd HH:mm:ss");


            DataTable dtStartDay = GetThisPeriodStartDay();
            string sStartDay = dtStartDay.Rows[0]["VariableValue"].ToString() + " 00:00:00";
            DataTable dtEndDay = GetThisPeriodEndDay();
            string sEndDay = dtEndDay.Rows[0]["VariableValue"].ToString() + " 23:59:59";

            clsDBComm commDB = new clsDBComm();
            //DataTable DT = new DataTable();
            bool IsSuccess = false;

            string sSelect = "(select  CASE MAX(kWh) - MIN(kWh) when 0 then MAX(kWh) - MIN(kWh) else CAST(( (MAX(kWh) - MIN(kWh)) ) AS decimal(18, 3)) END   AS KWAlarm from vwPowerMeter1HourLog where (1=1) and  (Timestamp) >= '";
            for (int ExeCount = 0; ExeCount < dt.Rows.Count; ExeCount++)
            {
                try
                {
                    string text = "UPDATE tblPowerMeter SET  ";

                    text += " SysThisPeriodPower  = " + sSelect + sStartDay + "' and (Timestamp) <= '" + sEndDay + "' and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                    text += " WHERE [ERID] = " + dt.Rows[ExeCount]["ERID"].ToString();

                    IsSuccess = commDB.ModifyDBData(text);
                }
                catch (Exception ex)
                {
                    var message = ex.Message + "," + ex.StackTrace;
                    throw ex;
                }
            }
        }

        //計算去年度用水每月累積值
        [WebMethod]
        public void WaterThisPeriodValue()
        {
            //DataTable dtStartDay = GetThisPeriodStartDay();
            //DateTime tmp_StartDate = Convert.ToDateTime(dtStartDay.Rows[0]["VariableValue"]);
            //string sStartDay = tmp_StartDate.ToString("yyyy-MM-dd HH:mm:ss");

            //DataTable dtEndDay = GetThisPeriodEndDay();
            //DateTime tmp_EndDate = Convert.ToDateTime(dtEndDay.Rows[0]["VariableValue"]);
            //string sEndDay = tmp_EndDate.ToString("yyyy-MM-dd HH:mm:ss");

            DataTable dtStartDay = GetThisPeriodStartDay();
            string sStartDay = dtStartDay.Rows[0]["VariableValue"].ToString() + " 00:00:00";
            DataTable dtEndDay = GetThisPeriodEndDay();
            string sEndDay = dtEndDay.Rows[0]["VariableValue"].ToString() + " 23:59:59";

            clsDBComm commDB = new clsDBComm();
            //DataTable DT = new DataTable();
            bool IsSuccess = false;

            string sSelect = "(select  CASE MAX(CumulateValueAlarm) - MIN(CumulateValueAlarm) when 0 then MAX(CumulateValueAlarm) - MIN(CumulateValueAlarm) else CAST(( (MAX(CumulateValueAlarm) - MIN(CumulateValueAlarm)) ) AS decimal(18, 3)) END   AS CumulateValueAlarm from vwPowerMeter1HourLog where (1=1)and ERID != 4  and  (Timestamp) >= '";
            for (int ExeCount = 0; ExeCount < dt.Rows.Count; ExeCount++)
            {
                try
                {
                    string text = "UPDATE tblPowerMeter SET  ";
                    text += " SysThisPeriodWater  = " + sSelect + sStartDay + "' and (Timestamp) <= '" + sEndDay + "' and ERID=" + dt.Rows[ExeCount]["ERID"].ToString() + " GROUP BY ERID,ERName )";
                    text += " WHERE [ERID] = " + dt.Rows[ExeCount]["ERID"].ToString();

                    IsSuccess = commDB.ModifyDBData(text);
                }
                catch (Exception ex)
                {
                    var message = ex.Message + "," + ex.StackTrace;
                    throw ex;
                }
            }
        }

        #endregion ==== 用水用電功能End =====





        public static string GetWEG(string ABA)
        {
            uint aba = Convert.ToUInt32(ABA);
            uint weg1 = aba / 65536;
            uint weg2 = aba % 65536;
            return weg1.ToString("00000") + weg2.ToString("00000");
        }
        public string R23Exchange(string MCNSID, int ERID, string ABA, string Name, DateTime StartDate, DateTime EndDate)
        {
            int RoleID = 0;
            clsDBComm commDB = new clsDBComm();

            DataTable dt = new DataTable();
            DataTable dtSelect = new DataTable();
            string cmd = "";
            bool IsSuccess = false;

            string tempABA = "";
            tempABA = (Convert.ToUInt32(ABA)).ToString("0000000000");

            DateTime tmp_StartDate = StartDate;
            DateTime tmp_EndDate = EndDate;

            string s_StartDate = tmp_StartDate.ToString("yyyy-MM-dd HH:mm:ss");
            string s_EndDate = tmp_EndDate.ToString("yyyy-MM-dd HH:mm:ss");

            //先判斷(1)磁卡是否已存在，(2)是否已啟用，(3)磁卡實際可進機房的時間是否在磁卡有效期限的範圍
            cmd = string.Format("select * from tblMagneticCard where ABA='{0}' and Enable='Y'and '{1}' between CardStartDate and CardEndDate and '{2}' between CardStartDate and CardEndDate;", tempABA, s_StartDate, s_EndDate);
            dt = commDB.SelectDBData(cmd);
            if (dt.Rows.Count > 0)
            {
                //已存在，先更新-可進入機房時間起迄
                cmd = string.Format("UPDATE tblMagneticCard set StartDate ='{0}',EndDate='{1}',Type=2,MCNSID='{2}',NormalID=0,Timestamp='{3}' where ABA='{4}';", s_StartDate, s_EndDate, MCNSID, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), tempABA);
                IsSuccess = commDB.ModifyDBData(cmd);

                RoleID = int.Parse(dt.Rows[0]["RoleID"].ToString());

                //查詢已加的權限不重複加入
                cmd = string.Format("select * from vwEntranceGuardDetail where ERID={0};", ERID);
                dt = commDB.SelectDBData(cmd);
                foreach (DataRow row in dt.Rows)
                {
                    cmd = string.Format("select * from tblSysRoleAuthority where RoleID={0} and ControlID='{1}';", RoleID, row["ControlID"].ToString());
                    dtSelect = commDB.SelectDBData(cmd);
                    if (dtSelect.Rows.Count == 0)
                    {
                        cmd = string.Format("INSERT INTO tblSysRoleAuthority(RoleID,ControlID) VALUES ({0},'{1}');", RoleID, row["ControlID"].ToString());
                        IsSuccess = commDB.ModifyDBData(cmd);

                    }
                }
            }
            else
            {
                throw new System.Exception("(1)磁卡不存在(2)磁卡未啟用(3)磁卡實際可進機房的時間超過磁卡有效期限的範圍");
            }

            string sRoleID = "";
            sRoleID = Convert.ToString(RoleID);
            return sRoleID;
        }
#else

        [WebMethod]
        public void NotifyDbChange()
        {
            SecureService.SecureServiceClient client = new SecureService.SecureServiceClient(new System.ServiceModel.InstanceContext(this));
            client.NotifyDBChange(SecureService.DBChangedConstant.AuthorityChanged, "");
        }
        [WebMethod]
        public string AddCardWithoutNotify(AddCardInfo info)
        {
            try
            {
                string rooms = string.Join(",", info.ERIDs);

                for (int i = 0; i < info.ERIDs.ToArray().Length; i++)
                {
                    Exchange(info.MCNSID, info.ERIDs[i], info.CardNo, info.Name, info.StartDate, info.EndDate);

                }

          

                return "0:success!" + rooms;
            }
            catch (Exception ex)
            {
                return "-1:" + ex.Message + "," + ex.StackTrace;
            }
        }

         [WebMethod]
        public string AddCard(AddCardInfo info)
        {
            string rooms = string.Join(",", info.ERIDs);
          //  return "0: add " + info.CardNo + " to  " + rooms + "  success!";
            try
            {
              
                
                for (int i = 0; i < info.ERIDs.ToArray().Length; i++)
                {
                    Exchange(info.MCNSID, info.ERIDs[i], info.CardNo, info.Name, info.StartDate, info.EndDate);
                  
                }

                SecureService.SecureServiceClient client = new SecureService.SecureServiceClient(new System.ServiceModel.InstanceContext(this));
                client.NotifyDBChange(SecureService.DBChangedConstant.AuthorityChanged,"");

                return "0:success!"+rooms;
            }
            catch (Exception ex)
            {
                return "-1:" + ex.Message+","+ex.StackTrace;
            }

          
        }
#endif
         //public void Exchange(string Memo, int ERID, string ABA, string Name, DateTime StartDate, DateTime EndDate)
         //{
         //    int RoleID=0;

         //    clsDBComm commDB = new clsDBComm();

         //    DataTable dt = new DataTable();
         //    DataTable dtSelect = new DataTable();
         //    string cmd = "";
         //    bool IsSuccess = false;


         //    string tempABA = "";
         //    tempABA = (Convert.ToUInt32(ABA)).ToString("0000000000");

         //    DateTime tmp_StartDate = StartDate;
         //    DateTime tmp_EndDate = EndDate;

         //    string s_StartDate = tmp_StartDate.ToShortDateString() + " 00:00:00";
         //    string s_EndDate = tmp_EndDate.ToShortDateString() + " 23:59:59";

         //    //先判斷磁卡是否已存在
         //    cmd = string.Format("select * from tblMagneticCard where ABA='{0}';", tempABA);
         //    dt = commDB.SelectDBData(cmd);
         //    if (dt.Rows.Count > 0)
         //    {
         //        RoleID = int.Parse(dt.Rows[0]["RoleID"].ToString());

         //        ////0330刪除已存在，再加的
         //        //cmd = string.Format("select * from tblSysRoleAuthority where RoleID={0};", RoleID);
         //        //dtSelect = commDB.SelectDBData(cmd);
         //        //if (dtSelect.Rows.Count > 0)
         //        //{
         //        //    cmd = string.Format("delete from tblSysRoleAuthority where RoleID={0};", RoleID);
         //        //    IsSuccess = commDB.ModifyDBData(cmd);
         //        //}

         //        //查詢已加的權限不重複加入
         //        cmd = string.Format("select * from vwEntranceGuardDetail where ERID={0};", ERID);
         //        dt = commDB.SelectDBData(cmd);
         //        foreach (DataRow row in dt.Rows)
         //        {
         //            cmd = string.Format("select * from tblSysRoleAuthority where RoleID={0} and ControlID='{1}';", RoleID, row["ControlID"].ToString());
         //            dtSelect = commDB.SelectDBData(cmd);
         //            if (dtSelect.Rows.Count == 0)
         //            {
         //                cmd = string.Format("INSERT INTO tblSysRoleAuthority(RoleID,ControlID) VALUES ({0},'{1}');", RoleID, row["ControlID"].ToString());
         //                IsSuccess = commDB.ModifyDBData(cmd);

         //            }
         //        }
         //    }
         //    else
         //    {
         //        //1.取得磁卡ABA後，加入tblSysRole磁卡權限群組表
         //        cmd = string.Format("INSERT INTO tblSysRole(RoleName,UpdateDate) VALUES ('{0}','{1}');", tempABA, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
         //        IsSuccess = commDB.ModifyDBData(cmd);

         //        if (IsSuccess)
         //        {
         //            cmd = string.Format("select * from tblSysRole where RoleName='{0}';", tempABA);

         //            dt = commDB.SelectDBData(cmd);
         //            if (dt.Rows.Count > 0)
         //            {
         //                RoleID = int.Parse(dt.Rows[0]["RoleID"].ToString());
         //            }

         //            //2.取得磁卡對應的RoleID, 加入tblMagneticCard磁卡資料表
         //            cmd = string.Format("INSERT INTO tblMagneticCard(ABA,Name,StartDate,EndDate,Type,RoleID,Timestamp,Memo,NormalID) VALUES " +
         //                                "('{0}','{1}','{2}','{3}',{4},{5},'{6}','{7}',{8});",
         //                                 tempABA, Name, s_StartDate, s_EndDate, 2, RoleID, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "施工單號:" + Memo, 0);
         //            IsSuccess = commDB.ModifyDBData(cmd);
         //            if (IsSuccess)
         //            {
         //                //3.取得此機房的各讀卡機的ControlID及磁卡對應的RoleID，加入tblSysRoleAuthority磁卡權限表，查詢已加的權限不重複加入       
         //                cmd = string.Format("select * from vwEntranceGuardDetail where ERID={0};", ERID);
         //                dt = commDB.SelectDBData(cmd);

         //                foreach (DataRow row in dt.Rows)
         //                {
         //                    cmd = string.Format("select * from tblSysRoleAuthority where RoleID={0} and ControlID='{1}';", RoleID, row["ControlID"].ToString());
         //                    dtSelect = commDB.SelectDBData(cmd);
         //                    if (dtSelect.Rows.Count == 0)
         //                    {
         //                        cmd = string.Format("INSERT INTO tblSysRoleAuthority(RoleID,ControlID) VALUES ({0},'{1}');", RoleID, row["ControlID"].ToString());
         //                        IsSuccess = commDB.ModifyDBData(cmd);
         //                    }
         //                }
         //            }
         //        }
         //    }
         //}

         public void Exchange(string MCNSID, int ERID, string ABA, string Name, DateTime StartDate, DateTime EndDate)
         {
             int RoleID = 0;
             clsDBComm commDB = new clsDBComm();

             DataTable dt = new DataTable();
             DataTable dtSelect = new DataTable();
             string cmd = "";
             bool IsSuccess = false;

             string tempABA = "";
             tempABA = (Convert.ToUInt32(ABA)).ToString("0000000000");

             DateTime tmp_StartDate = StartDate;
             DateTime tmp_EndDate = EndDate;

             string s_StartDate = tmp_StartDate.ToString("yyyy-MM-dd HH:mm:ss");
             string s_EndDate = tmp_EndDate.ToString("yyyy-MM-dd HH:mm:ss");

             //先判斷(1)磁卡是否已存在，(2)是否已啟用，(3)磁卡實際可進機房的時間是否在磁卡有效期限的範圍
             cmd = string.Format("select * from tblMagneticCard where ABA='{0}' and Enable='Y'and '{1}' between CardStartDate and CardEndDate and '{2}' between CardStartDate and CardEndDate;", tempABA, s_StartDate, s_EndDate);
             dt = commDB.SelectDBData(cmd);
             if (dt.Rows.Count > 0)
             {
                 //已存在，先更新-可進入機房時間起迄
                 cmd = string.Format("UPDATE tblMagneticCard set StartDate ='{0}',EndDate='{1}',Type=2,MCNSID='{2}',NormalID=0,Timestamp='{3}' where ABA='{4}';", s_StartDate, s_EndDate, MCNSID, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), tempABA);
                 IsSuccess = commDB.ModifyDBData(cmd);

                 RoleID = int.Parse(dt.Rows[0]["RoleID"].ToString());

                 //查詢已加的權限不重複加入
                 cmd = string.Format("select * from vwEntranceGuardDetail where ERID={0};", ERID);
                 dt = commDB.SelectDBData(cmd);
                 foreach (DataRow row in dt.Rows)
                 {
                     cmd = string.Format("select * from tblSysRoleAuthority where RoleID={0} and ControlID='{1}';", RoleID, row["ControlID"].ToString());
                     dtSelect = commDB.SelectDBData(cmd);
                     if (dtSelect.Rows.Count == 0)
                     {
                         cmd = string.Format("INSERT INTO tblSysRoleAuthority(RoleID,ControlID) VALUES ({0},'{1}');", RoleID, row["ControlID"].ToString());
                         IsSuccess = commDB.ModifyDBData(cmd);

                     }
                 }
                 //寫入CardCommandLog，ABA,*,C
                 cmd = string.Format("INSERT INTO [SecureDB].[dbo].[tblCardCommandLog]([ABA],[ControlID],[CommandType],[CardType])VALUES('{0}','*','*','C')", tempABA);
                 IsSuccess = commDB.ModifyDBData(cmd);

             }
             else
             {
                 throw new System.Exception("(1)磁卡不存在(2)磁卡未啟用(3)磁卡實際可進機房的時間超過磁卡有效期限的範圍");
             }
         }
         public void SayHello(string hello)
         {
            // throw new NotImplementedException();
         }

         public void SecureDoorEvent(SecureService.DoorEventType evttype, SecureService.DoorBindingData doorBindingData)
         {
            // throw new NotImplementedException();
         }

         public void SecureAlarm(SecureService.AlarmData alarmdata)
         {
           //  throw new NotImplementedException();
         }

         public void ItemValueChangedEvenr(SecureService.ItemBindingData ItemBindingData)
         {
            // throw new NotImplementedException();
         }
    }

    public class  CardSendingResult{
      public string MSNCID {get;set;}
      public string ABA {get;set;}
      public DateTime? TimeStamp {get;set;}
      public string ERName{get;set;}
      public string Memo {get;set;}
      public string Name {get;set;}
      public string ComandTypeName{get;set;}
      public string IsSuccessName {get;set;}
    }

    public class MagneticCardBasicInfo
    {

      public string  Company {get;set;}
      public string ABA  {get;set;}
      public string  Name   {get;set;}
      public string IDNumber  {get;set;}
      public string EmployeeNo {get;set;}
      public string JobTitle  {get;set;}
      public string  Tel  {get;set;}
      public string   Mobile {get;set;}
      public DateTime? CardStartDate { get; set; }
      public  DateTime? CardEndDate {get;set;}
      public DateTime? StartDate { get; set; }
      public DateTime? EndDate { get; set; }
      public short? Type { get; set; }


    }


    public class RoomInfo
    {
        public int ERID { get; set; }
        public string ERName { get; set; }
        public string LineID { get; set; }
    }


    public class  AddCardInfo
    {
        public  string  MCNSID {get;set;}
        public string CardNo {get;set;}
        public  int[] ERIDs {get;set;}
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Name { get; set; }
    }


    public class R23DoorInfo
    {
        public string ERName { get; set; }
        public int stateHardware { get; set; }
        public bool IsShowPerson { get; set; }
        public string StayDoorName { get; set; }
        public int state { get; set; }
        public int stateAlarm { get; set; }
    }
    public class clsDBComm
    {
        //string sErrMsg;
        string sConnString;

        SqlConnection conn; //去抓 XML 連線字串
        SqlCommand cmd;     //放 SQL 語法
        //DB2DataReader dr;   //讀 SQL 結果
        SqlDataAdapter da;  //放置 SQL 結果
        System.ComponentModel.BackgroundWorker ModifyBW;
        bool doWork;
        bool haveError;
        Exception BWex;

        public clsDBComm()
        {
            ModifyBW = new System.ComponentModel.BackgroundWorker();
            ModifyBW.DoWork += new System.ComponentModel.DoWorkEventHandler(ModifyBW_DoWork);
            ModifyBW.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(ModifyBW_RunWorkerCompleted);
        }

        void ModifyBW_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            haveError = (e.Error != null);
            if (haveError)
            {
                BWex = e.Error;
            }
            doWork = false;

        }


        void ModifyBW_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e) //有例外也不可加上try catch ,繼續執行即可 By昭毅
        {
            cmd.ExecuteNonQuery();
        }

   
        internal DataTable SelectDBData(string selectCmd)
        {
            try
            {
                InitDB();
                da = new SqlDataAdapter(selectCmd, conn);
                DataTable DT = new DataTable();
                da.Fill(DT);
                da.Dispose();
                CloseDB();
                return DT;
            }
            catch (Exception ex)
            {
#if DEBUG
                //MessageBox.Show("資料庫存取錯誤：" + ex.Message + "\r\n" + ex.Source + "\r\n" + ex.StackTrace + "\r\n" + selectCmd);
#else
                //MessageBox.Show("資料庫繁忙中，請重新操作！"+ "\r\n\r\n\r\n" + ex.Message  + "\r\n" + selectCmd);
#endif
                CloseDB();
                return null;
            }
        }

        internal DataTable SelectDBData(string selectCmd, int timeout)
        {
            try
            {
                InitDB();
                da = new SqlDataAdapter(selectCmd, conn);
                da.SelectCommand.CommandTimeout = timeout;
                DataTable DT = new DataTable();
                da.Fill(DT);
                da.Dispose();
                CloseDB();
                return DT;
            }
            catch (Exception ex)
            {
                Comm.WriteErrorLog("資料庫存取錯誤：" + ex.Message + "\r\n" + ex.Source + "\r\n" + ex.StackTrace + "\r\n" + selectCmd);
                CloseDB();
                return null;
            }
        }

        internal DataTable SelectDBData(string selectCmd, SqlParameter[] Params)
        {
            try
            {
                InitDB();
                da = new SqlDataAdapter(selectCmd, conn);
                foreach (SqlParameter param in Params)
                {
                    da.SelectCommand.Parameters.Add(param);
                }
                DataTable DT = new DataTable();
                da.Fill(DT);
                da.Dispose();
                CloseDB();
                return DT;
            }
            catch (Exception ex)
            {
                Comm.WriteErrorLog("資料庫存取錯誤：" + ex.Message + "\r\n" + ex.Source + "\r\n" + ex.StackTrace + "\r\n" + selectCmd);
                CloseDB();
                return null;
            }
        }



        internal bool ModifyDBData(string modifyCmd)
        {
            try
            {
                InitDB();
                cmd = new SqlCommand(modifyCmd, conn);
                cmd.CommandTimeout = 120;

                //doWork = true;
                //System.Threading.AutoResetEvent wait = new System.Threading.AutoResetEvent(false);
                cmd.ExecuteNonQuery();
                //ModifyBW.RunWorkerAsync();
                //while (doWork)
                //{
                //    wait.WaitOne(20);
                //    //Application.DoEvents();
                //}
                //if (haveError)
                //{
                //    throw BWex;
                //    //MessageBox.Show(BWex.Message);
                //    //return false;
                //}
                CloseDB();
                return true;

            }
            catch (Exception ex)
            {
                Comm.WriteErrorLog("資料庫存取錯誤：" + ex.Message + "\r\n" + modifyCmd);
                CloseDB();
                return false;
            }
        }//

        internal bool ModifyDBData(string modifyCmd, bool ThrowEx)
        {
            try
            {
                InitDB();
                cmd = new SqlCommand(modifyCmd, conn);
                cmd.CommandTimeout = 120;

                doWork = true;
                System.Threading.AutoResetEvent wait = new System.Threading.AutoResetEvent(false);
                //cmd.ExecuteNonQuery();
                ModifyBW.RunWorkerAsync();
                while (doWork)
                {
                    wait.WaitOne(20);
                    //Application.DoEvents();
                }
                if (haveError)
                {
                    throw BWex;
                    //MessageBox.Show(BWex.Message);
                    //return false;
                }
                CloseDB();
                return true;

            }
            catch (Exception ex)
            {
                //MessageBox.Show("資料庫存取錯誤：" + ex.Message + "\r\n" + modifyCmd);
                CloseDB();
                if (ThrowEx)
                    throw ex;
                return false;
            }
        }

        #region ==== 資料庫初始化 ====
        //資料庫初始化
        private void InitDB()
        {
            sConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;  // "Database=SecureDB;User ID=secure;Password=secure;Server=192.192.85.64,1357";
            //sConnString = "Database=CPU;User ID=sa;Password=rabby;Server=(local)\\Rabby";
            conn = new SqlConnection(sConnString);
            conn.Open();
        }
        #endregion

        #region ==== 關閉資料庫 ====
        //關閉資料庫
        private void CloseDB()
        {
            if (conn != null)
                conn.Close();
        }
        #endregion
    }

    public class Comm
    {
        internal static void WriteErrorLog(string ErrorMessage)
        {
            try
            {
              //  System.IO.File.AppendAllText("Error.log", DateTime.Now.ToString() + " : " + ErrorMessage + "\r\n");
            }
            catch { }

        }
    }

}



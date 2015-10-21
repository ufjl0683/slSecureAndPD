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



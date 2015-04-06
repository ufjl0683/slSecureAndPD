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
                return db.tblEngineRoomConfig.Select(n => new RoomInfo() { ERName = n.ERName, ERID = n.ERID }).ToArray();
            }
        }

         [WebMethod]
        public string AddCard(AddCardInfo info)
        {
            string rooms = string.Join(",", info.ERIDs);
          //  return "0: add " + info.CardNo + " to  " + rooms + "  success!";
            try
            {
              
                
                for (int i = 0; i < info.ERIDs.Length; i++)
                {
                    Exchange(info.MCNSID, info.ERIDs[i], info.CardNo, info.Name, info.StartDate, info.EndDate);
                  
                }

                SecureService.SecureServiceClient client = new SecureService.SecureServiceClient(new System.ServiceModel.InstanceContext(this));
                client.NotifyDBChange(SecureService.DBChangedConstant.AuthorityChanged, "");

                return "0:success!"+rooms;
            }
            catch (Exception ex)
            {
                return "-1:" + ex.Message+","+ex.StackTrace;
            }

          
        }

         public void Exchange(string Memo, int ERID, string ABA, string Name, DateTime StartDate, DateTime EndDate)
         {
             clsDBComm commDB = new clsDBComm();

             DataTable dt = new DataTable();
             DataTable dtSelect = new DataTable();
             string cmd = "";
             bool IsSuccess = false;
             int RoleID = 0;
             string tempABA = "";
             tempABA = (Convert.ToUInt32(ABA)).ToString("0000000000");

             DateTime tmp_StartDate = StartDate;
             DateTime tmp_EndDate = EndDate;

             string s_StartDate = tmp_StartDate.ToShortDateString() + " 00:00:00";
             string s_EndDate = tmp_EndDate.ToShortDateString() + " 23:59:59";

           //  System.IO.File.AppendAllText(@".\MCNSExchange.log", DateTime.Now.ToString() + "  ---Start---" + "\r\n");

             //先判斷磁卡是否已存在
             cmd = string.Format("select * from tblMagneticCard where ABA='{0}';", tempABA);
             dt = commDB.SelectDBData(cmd);
             if (dt.Rows.Count > 0)
             {
                 RoleID = int.Parse(dt.Rows[0]["RoleID"].ToString());

               //  System.IO.File.AppendAllText(@".\MCNSExchange.log", DateTime.Now.ToString() + "  查詢tblMagneticCard磁卡已存在！" + "ABA=" + tempABA + "\r\n");
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
                         //if (IsSuccess)
                         //{
                         //    System.IO.File.AppendAllText(@".\MCNSExchange.log", DateTime.Now.ToString() + "  tblSysRoleAuthority寫入成功！" + "RoleID=" + RoleID.ToString() + ",ConrtolID=" + row["ControlID"].ToString() + "\r\n");
                         //}
                     }
                     //else
                     //{
                     //    System.IO.File.AppendAllText(@".\MCNSExchange.log", DateTime.Now.ToString() + "  查詢tblSysRoleAuthority已存在！" + "RoleID=" + RoleID.ToString() + ",ConrtolID=" + row["ControlID"].ToString() + "\r\n");
                     //}
                 }
             }
             else
             {
                 //1.取得磁卡ABA後，加入tblSysRole磁卡權限群組表
                 cmd = string.Format("INSERT INTO tblSysRole(RoleName,UpdateDate) VALUES ('{0}','{1}');", tempABA, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                 IsSuccess = commDB.ModifyDBData(cmd);

                 if (IsSuccess)
                 {
                  //   System.IO.File.AppendAllText(@".\MCNSExchange.log", DateTime.Now.ToString() + "  tblSysRole寫入成功！" + "ABA=" + tempABA + "\r\n");

                     cmd = string.Format("select * from tblSysRole where RoleName='{0}';", tempABA);

                     dt = commDB.SelectDBData(cmd);
                     if (dt.Rows.Count > 0)
                     {
                         RoleID = int.Parse(dt.Rows[0]["RoleID"].ToString());
                     //    System.IO.File.AppendAllText(@".\MCNSExchange.log", DateTime.Now.ToString() + "  tblSysRole查詢成功！" + "RoleID=" + dt.Rows[0][0].ToString() + "\r\n");
                     }

                     //2.取得磁卡對應的RoleID, 加入tblMagneticCard磁卡資料表
                     cmd = string.Format("INSERT INTO tblMagneticCard(ABA,Name,StartDate,EndDate,Type,RoleID,Timestamp,Memo,NormalID) VALUES " +
                                         "('{0}','{1}','{2}','{3}',{4},{5},'{6}','{7}',{8});",
                                          tempABA, Name, s_StartDate, s_EndDate, 2, RoleID, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Memo, 0);
                     IsSuccess = commDB.ModifyDBData(cmd);
                     if (IsSuccess)
                     {
                      //   System.IO.File.AppendAllText(@".\MCNSExchange.log", DateTime.Now.ToString() + "  tblMagneticCard寫入成功！" + "\r\n");

                         //3.取得此機房的各讀卡機的ControlID及磁卡對應的RoleID，加入tblSysRoleAuthority磁卡權限表，查詢已加的權限不重複加入       
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
                                 if (IsSuccess)
                                 {
                                //     System.IO.File.AppendAllText(@".\MCNSExchange.log", DateTime.Now.ToString() + "  tblSysRoleAuthority寫入成功！" + "RoleID=" + RoleID.ToString() + ",ConrtolID=" + row["ControlID"].ToString() + "\r\n");
                                 }
                             }
                             else
                             {
                             //    System.IO.File.AppendAllText(@".\MCNSExchange.log", DateTime.Now.ToString() + "  查詢tblSysRoleAuthority已存在！" + "RoleID=" + RoleID.ToString() + ",ConrtolID=" + row["ControlID"].ToString() + "\r\n");
                             }
                         }
                     }
                 }
             }
        //     System.IO.File.AppendAllText(@".\MCNSExchange.log", DateTime.Now.ToString() + "  ---End---" + "\r\n");

             //try
             //{
             //    if (System.IO.File.Exists(@".\MCNSExchange.log"))
             //    {
             //        System.IO.FileInfo info = new FileInfo(@".\MCNSExchange.log");
             //        if (info.Length > 1000000)
             //        {
             //            string text = System.IO.File.ReadAllText(@".\MCNSExchange.log");
             //            text = text.Substring(100000);
             //            System.IO.File.WriteAllText(@".\MCNSExchange.log", text);
             //        }
             //    }
             //}
             //catch
             //{ }
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




    public class RoomInfo
    {
        public int ERID { get; set; }
        public string ERName { get; set; }

    }


    public class  AddCardInfo
    {
        public  string  MCNSID {get;set;}
        public string CardNo {get;set;}
        public int [] ERIDs {get;set;}
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



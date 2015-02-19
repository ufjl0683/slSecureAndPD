﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    public class MCNSService : System.Web.Services.WebService
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
                return db.tblEngineRoomConfig.Select(n => new RoomInfo() { ERName = n.ERName, ERNO = n.ERNo }).ToArray();
            }
        }

         [WebMethod]
        public string AddCard(AddCardInfo info)
        {
            string rooms = string.Join(",", info.ERNOs);
            return "0: add " + info.CardNo + " to  " + rooms + "  success!"; 



        }



    }




    public class RoomInfo
    {
        public string ERNO { get; set; }
        public string ERName { get; set; }

    }


    public class  AddCardInfo
    {
        public  string  MCNSID {get;set;}
        public string CardNo {get;set;}
        public string [] ERNOs {get;set;}
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
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
                System.IO.File.AppendAllText("Error.log", DateTime.Now.ToString() + " : " + ErrorMessage + "\r\n");
            }
            catch { }

        }
    }

}



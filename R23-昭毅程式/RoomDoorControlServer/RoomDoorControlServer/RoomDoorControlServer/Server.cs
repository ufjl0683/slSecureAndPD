using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Tcp;

namespace RoomDoorControlServer
{
    public partial class Server : Form
    {
        HostAction hostAct = null;

        public Server()
        {
           

            if (System.IO.File.Exists(@".\Server.txt"))
            {
                System.Configuration.Configuration cfg = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);
                
                System.Data.EntityClient.EntityConnectionStringBuilder ecsb = new System.Data.EntityClient.EntityConnectionStringBuilder();
                ecsb.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["dbroomEntities"].ConnectionString;
                ecsb.ProviderConnectionString = System.IO.File.ReadLines(@".\Server.txt").First();
                System.Configuration.ConnectionStringsSection conSection = (System.Configuration.ConnectionStringsSection)cfg.GetSection("connectionStrings");
                conSection.ConnectionStrings["dbroomEntities"].ConnectionString = ecsb.ConnectionString;
                cfg.Save();
                System.Configuration.ConfigurationManager.RefreshSection("connectionStrings");
            }

            InitializeComponent();

            if (IsHostIp())
            {
                this.Text = "門禁管理伺服器";
                ServerConfing();
            }
            else
            {
                this.Text = "門禁管理";
            }
            this.dataGridView1.AutoGenerateColumns = false;
            cboType.SelectedIndex = 0;
            //DateTime time = card.ReadTime();
            //CardControl card = new CardControl("192.168.2.199");
            //List<LogData> Cardlog = card.ReadLog(LogType.CradLog);
            //List<LogData> EventLog = card.ReadLog(LogType.EventLog);
            //List<LogData> runTimeLog = card.ReadLog(LogType.RuntimeLog);
            //DateTime date = card.ReadTime();
            //card.SetTime(DateTime.Now);
            //RAC960CardControl Rac960 = new RAC960CardControl("192.168.2.198");
            //List<RAC960Record> data = Rac960.ReadRecord();
            //Rac960.readFlashData();
            //DateTime RacDate = Rac960.ReadTime();
                       
        }

        void ServerConfing()
        {
            System.Runtime.Remoting.Channels.BinaryServerFormatterSinkProvider sfsp = new System.Runtime.Remoting.Channels.BinaryServerFormatterSinkProvider();
            sfsp.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
            System.Collections.Hashtable props = new System.Collections.Hashtable();
            props["port"] = 9000;
            TcpChannel tcp = new TcpChannel(props, null, sfsp);

            
            System.Runtime.Remoting.Channels.ChannelServices.RegisterChannel(tcp, false);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(RoomObj), "RoomObj", WellKnownObjectMode.SingleCall);

            ReadServerData();
            DatabaseAccess.StartDatabaseAcces();
            hostAct = new HostAction();            
        }

        void ReadServerData()
        {
            dbroomEntities dbroom = new dbroomEntities();
            var ControllerData = from o in dbroom.tblControllerConfig select o;       
            foreach (var data in ControllerData)
            {
                try
                {
                    if (data.IP != "0.0.0.0")
                    {
                        
                        #region 控制器
                        switch (data.ControlType)
                        {
                            case 1:
                                if (!ServerData.RAC2400Controller.ContainsKey(data.ControlID))
                                {
                                    var Room = (from room in dbroom.tblEngineRoomConfig
                                                where room.ERID == data.ERID
                                                select room).First();
                                    string RoomName = Room.ERName;
                                    var readerDatas = from reader in dbroom.tblCardReaderConfig
                                                      where reader.ControlID == data.ControlID
                                                      select reader;
                                    byte[] readerType = new byte[4];
                                    int[] readerID = new int[4];
                                    byte[] readerIO = new byte[4];
                                    foreach (var readData in readerDatas)
                                    {
                                        byte NO = Convert.ToByte(readData.ReaderNO);
                                        byte io = 0;
                                        if (readData.RoomIO == "I")
                                            io = 1;
                                        else if (readData.RoomIO == "O")
                                            io = 2;
                                        readerIO[NO] = io;
                                        readerID[NO] = readData.ReaderID;
                                        readerType[NO] = Convert.ToByte(readData.ReaderType);
                                    }

                                    CardControl control = new CardControl(data.ControlID, data.IP, Room.ERNo, readerID, readerIO, readerType, data.ERID);
                                    ServerData.RAC2400Controller.Add(data.ControlID, control);
                                    ServerData.ThreadEndList.Add(control);
                                    System.Threading.Thread t = new System.Threading.Thread(control.HostAction);
                                    t.Start();
                                }
                                break;
                            case 2:
                                if (!ServerData.RAC960Controller.ContainsKey(data.ControlID))
                                {
                                    var Room = (from room in dbroom.tblEngineRoomConfig
                                                where room.ERID == data.ERID
                                                select room).First();
                                    string RoomName = Room.ERName;
                                    var readerData = (from reader in dbroom.tblCardReaderConfig
                                                      where reader.ControlID == data.ControlID
                                                      select reader).First();
                                    byte type = 0;
                                    if (readerData.RoomIO == "I")
                                        type = 1;
                                    else if (readerData.RoomIO == "O")
                                        type = 2;

                                    RAC960CardControl control = new RAC960CardControl(data.ControlID, data.IP, Room.ERNo, readerData.ReaderID, type, data.Loop == "1");
                                    ServerData.RAC960Controller.Add(data.ControlID, control);
                                    ServerData.ThreadEndList.Add(control);
                                    System.Threading.Thread t = new System.Threading.Thread(control.HostAction);
                                    t.Start();
                                }
                                break;
                            case 4:
                                if (!ServerData.ADAMController.ContainsKey(data.ControlID))
                                {
                                    //if (data.IP == "192.168.100.21")
                                    //{
                                    ADAMControl adam = new ADAMControl(data.ControlID, data.IP, data.Port);

                                    ServerData.ADAMController.Add(data.ControlID, adam);

                                    ServerData.ThreadEndList.Add(adam);
                                    System.Threading.Thread t = new System.Threading.Thread(adam.HostAction);
                                    t.Start();
                                    //}
                                }
                                break;
                        }
                        #endregion
                        if (!ServerData.ControlStatus.ContainsKey(data.ControlID))
                        {
                            try
                            {
                                var RoomStatus = (from o in dbroom.tblDeviceStateLog
                                                  where o.TypeID == 3 && o.ControlID == data.ControlID
                                                  orderby o.TimeStamp descending
                                                  select o).First();
                                ServerData.ControlStatus.Add(data.ControlID, new RoomInterface.ControlStatus(data.ControlID, RoomStatus.TypeCode == 0, RoomStatus.TimeStamp));
                            }
                            catch
                            {
                                ServerData.ControlStatus.Add(data.ControlID, new RoomInterface.ControlStatus(data.ControlID, false, DateTime.Now));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    TCommon.SaveLog(ex.Message + "\r\n" + ex.StackTrace);
                }
            }

            var UserData = from card in dbroom.tblMagneticCard
                           select card;
            foreach (var user in UserData)
            {
                if (!ServerData.PersonnelData.ContainsKey(user.ABA))
                {
                    ServerData.PersonnelData.Add(user.ABA, user.Name);
                }
                if (!ServerData.PersonnelCompData.ContainsKey(user.ABA))
                {
                    ServerData.PersonnelCompData.Add(user.ABA, user.Company);
                }
            }
            var RoomData = from o in dbroom.tblEngineRoomConfig select o;
            foreach (var room in RoomData)
            {
                if (!ServerData.RoomPerson.ContainsKey(room.ERNo))
                {
                    ServerData.RoomPerson.Add(room.ERNo, new List<RoomCardData>());
                    DateTime OneDay = DateTime.Now.AddDays(-1);
                    var RoomPersonData = from o in dbroom.tblEngineRoomLog where o.ERNo == room.ERNo && o.StartTime > OneDay select o;
                    List<RoomCardData> NewRoomCardData = new List<RoomCardData>();
                    foreach (var RoomPerson in RoomPersonData)
                    {
                        bool AddData = true;
                        bool InOut = RoomPerson.Endtime == null;
                        for (int i =0 ;i < NewRoomCardData.Count;i++)
                        {
                            if (NewRoomCardData[i].CardID == RoomPerson.ABA)
                            {
                                if (NewRoomCardData[i].LastTime < RoomPerson.StartTime)
                                {
                                    RoomCardData CardData = NewRoomCardData[i];
                                    CardData.LastTime = RoomPerson.StartTime;
                                    CardData.In = InOut;
                                }
                                AddData = false;
                                break;
                            }
                        }
                        if (AddData)
                            NewRoomCardData.Add(new RoomCardData(RoomPerson.ABA, ServerData.GetCardName(RoomPerson.ABA),ServerData.GetCarComp(RoomPerson.ABA), InOut, RoomPerson.StartTime,false));                   
                    }

                    for (int i = 0; i < NewRoomCardData.Count; i++)
                    {
                        if (NewRoomCardData[i].In)
                        {
                            ServerData.RoomPerson[room.ERNo].Add(NewRoomCardData[i]);
                        }
                    }

                    var adamControl = from o in dbroom.tblControllerConfig where o.ERID == room.ERID && o.ControlType == 4 select o.ControlID;
                    if (adamControl.Count() > 0)
                    {
                        ServerData.RoomControl.Add(room.ERNo, adamControl.First());
                    }
                }
            }

            var Group = from o in dbroom.tblSysRole select o;
            foreach (var role in Group)
            {
                if (!ServerData.GroupDataDic.ContainsKey(role.RoleID))
                {
                    GroupData newGroupData = new GroupData(role.RoleID);
                    var groupController = from o in dbroom.tblSysRoleAuthority where o.RoleID == role.RoleID && o.Enable == "Y" select o;
                    foreach(var Controller in groupController)
                    {
                        newGroupData.ControlID.Add(Controller.ControlID);
                    }
                    ServerData.GroupDataDic.Add(role.RoleID, newGroupData);
                }
            }

            var MagenticCard = from o in dbroom.tblMagneticCard select o;
            foreach (var card in MagenticCard)
            {
                if (!ServerData.GroupCardDic.ContainsKey(card.RoleID))
                {
                    ServerData.GroupCardDic.Add(card.RoleID, new List<GroupCardData>());
                }
                ServerData.GroupCardDic[card.RoleID].Add(new GroupCardData(card.ABA, card.StartDate, card.EndDate, card.RoleID));
            }

            var ControlCrads = from magentic in dbroom.tblMagneticCard
                               join role in dbroom.tblSysRoleAuthority on magentic.RoleID equals role.RoleID
                               where role.Enable == "Y" && magentic.Enable == "Y"
                               select new { ABA = magentic.ABA, controlID = role.ControlID };
            foreach (var data in ControlCrads)
            {
                if (!ServerData.ControlCardDic.ContainsKey(data.controlID))
                {
                    ServerData.ControlCardDic.Add(data.controlID, new List<string>());
                }
                ServerData.ControlCardDic[data.controlID].Add(data.ABA);
            }

            var ControlNames = from control in dbroom.tblControllerConfig
                              join door in dbroom.tblEntranceGuardConfig on control.EntranceCode equals door.EntranceCode
                              join room in dbroom.tblEngineRoomConfig on door.ERID equals room.ERID
                              select new { controlID = control.ControlID, doorName = door.Memo, roomName = room.ERName };
            foreach (var controlData in ControlNames)
            {
                if (!ServerData.ControlName.ContainsKey(controlData.controlID))
                {
                    ServerData.ControlName.Add(controlData.controlID, controlData.roomName + controlData.doorName);
                }
            }

   
        }

        bool IsHostIp()
        {
            try
            {
                dbroomEntities dbroom = new dbroomEntities();
                var HostData = (from o in dbroom.tblHostConfig select o).First();
                foreach (System.Net.IPAddress ip in System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName()))
                {
                    if (ip.ToString().Equals(HostData.IP))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private void btnLoadUserCard_Click(object sender, EventArgs e)
        {            
            if (!checkIP())
                return;
            btnLoadUserCard.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            dataGridView1.DataSource = null;
            string[] ColumnNames = { "CardNum", "Message"};
            DataTable DT = new DataTable();
            foreach (string columnName in ColumnNames)
            {
                DT.Columns.Add(columnName);
            }

            if (cboType.SelectedIndex == 0)
            {
                CardControl card = new CardControl(txtIP.Text);
                List<CardData> AllUser = card.ReadAllUserData();
                if (AllUser == null)
                {
                    MessageBox.Show("載入失敗");
                    this.Cursor = Cursors.Default;
                    btnLoadUserCard.Enabled = true;
                    return;
                }
                

                foreach (var data in AllUser)
                {
                    DataRow dr = DT.NewRow();
                    dr[0] = data.CarNum;
                    dr[1] = data.Message;
                    DT.Rows.Add(dr);
                }
                dataGridView1.DataSource = DT;
            }
            else
            {
                dataGridView1.DataSource = DT;
                RAC960CardControl card = new RAC960CardControl(txtIP.Text);
                bool end = false;
                int errorCount = 0;
                int index = 0;
                while (!end)
                {
                    string cardName = card.ReadUserData(index);
                    if (cardName == "")
                    {
                        end = true;
                        break;
                    }
                    if (cardName != null)
                    {
                        errorCount = 0;
                        DataRow dr = DT.NewRow();
                        string[] strArr = cardName.Split(',');
                        dr[0] = strArr[0];
                        dr[1] = strArr[1];
                        DT.Rows.Add(dr);
                        Application.DoEvents();
                    }
                    else
                    {
                        if (errorCount < 10)
                        {
                            errorCount++;
                        }
                        else
                        {
                            MessageBox.Show("read error too many, stop read card info");
                            end = true;
                            break;
                        }
                    }

                }
                //List<string> cardList =  card.ReadAllUserData();
                //if (cardList != null)
                //{
                //    foreach (var data in cardList)
                //    {
                //        DataRow dr = DT.NewRow();
                //        string[] strArr = data.Split(',');
                //        dr[0] = strArr[0];
                //        dr[1] = strArr[1];
                //        DT.Rows.Add(dr);
                //    }
                //    dataGridView1.DataSource = DT;
                //}
            }
            this.Cursor = Cursors.Default;
            btnLoadUserCard.Enabled = true;
        }

        bool checkIP()
        {
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmNewCard f = new FrmNewCard();
            f.StartPosition = FormStartPosition.CenterScreen;
            try
            {
                if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (cboType.SelectedIndex == 0)
                    {
                        CardControl card = new CardControl(txtIP.Text);
                        List<string> cardList = new List<string>(1);
                        string weg = f.WEG;
                        if (weg.Length < 10)
                        {
                            int AddLen = 10 - weg.Length;
                            for (int i = 0; i < AddLen; i++)
                            {
                                weg = "0" + weg;
                            }
                        }
                        cardList.Add(f.ABA + "," + weg + "," + f.Message);

                        string ErrorMessge = card.AddUserData(cardList);
                        if (ErrorMessge == "")
                        {
                            MessageBox.Show("下載成功");
                        }
                        else
                        {
                            MessageBox.Show(ErrorMessge);
                        }
                    }
                    else
                    {
                        RAC960CardControl card = new RAC960CardControl(txtIP.Text);
                        if (card.AddUserData(f.ABA, f.Message) == true)
                        {
                            MessageBox.Show("下載成功");
                        }
                        else
                        {
                            MessageBox.Show("下載失敗");
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("輸入資料錯誤");
            }
        }

       

        private void btnDelAll_Click(object sender, EventArgs e)
        {
            btnDelAll.Enabled = false;

            if (MessageBox.Show("確定刪除全部卡號?", string.Empty, MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
            {
                btnDelAll.Enabled = true;
                return;
            }

            if (cboType.SelectedIndex == 0)
            {
                CardControl card = new CardControl(txtIP.Text);

                if (card.DelAllUserData())
                {
                    MessageBox.Show("清除成功");
                }
                else
                {
                    MessageBox.Show("清除失敗");
                }
            }
            else
            {
                RAC960CardControl card = new RAC960CardControl(txtIP.Text);
                if (card.DelAllUserData())
                {
                    MessageBox.Show("清除成功");
                }
                else
                {
                    MessageBox.Show("清除失敗");
                }
            }
            btnDelAll.Enabled = true;
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            btnDel.Enabled = false;
            if (dataGridView1.SelectedRows.Count < 1)
            {
                MessageBox.Show("未選擇刪除卡號");
            }
            if (cboType.SelectedIndex == 0)
            {
                CardControl card = new CardControl(txtIP.Text);
                if (card.DelUserData(dataGridView1.SelectedRows[0].Cells[0].Value.ToString()) == true)
                {
                    MessageBox.Show("刪除成功");
                    dataGridView1.Rows.Remove(dataGridView1.SelectedRows[0]);
                }
                else
                {
                    MessageBox.Show("刪除失敗");
                }
            }
            else
            {
                RAC960CardControl card = new RAC960CardControl(txtIP.Text);
                if (card.DelUserData(dataGridView1.SelectedRows[0].Cells[0].Value.ToString()) == true)
                {
                    MessageBox.Show("刪除成功");
                    dataGridView1.Rows.Remove(dataGridView1.SelectedRows[0]);
                }
                else
                {
                    MessageBox.Show("刪除失敗");
                }
            }
            btnDel.Enabled = true;
        }

        private void tbnExcel_Click(object sender, EventArgs e)
        {
            ExcelDownLoad excel = new ExcelDownLoad(txtIP.Text, cboType.SelectedIndex);
            excel.StartPosition = FormStartPosition.CenterScreen;
            if (!excel.IsDisposed)
            {
                excel.ShowDialog();
            }
        }

        private void btnTime_Click(object sender, EventArgs e)
        {
            if (cboType.SelectedIndex == 0)
            {
                CardControl card = new CardControl(txtIP.Text);
                if (card.SetTime())
                {
                    MessageBox.Show("更新時間成功");
                }
                else
                {
                    MessageBox.Show("更新時間失敗");
                }
            }
            else
            {
                RAC960CardControl card = new RAC960CardControl(txtIP.Text);
                if (card.SetTime())
                {
                    MessageBox.Show("更新時間成功");
                }
                else
                {
                    MessageBox.Show("更新時間失敗");
                }
            }
        }

        private void Server_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (IEnd end in ServerData.ThreadEndList)
            {
                end.End = true;
            }
            if (hostAct != null)
                hostAct.Close();
            DatabaseAccess.EndDatabaseAcces();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            FrmOpenDoor f = new FrmOpenDoor();
            f.StartPosition = FormStartPosition.CenterScreen;
            f.Show();
        }

        private void NameIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

        private void Server_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                NIcon.Visible = true;
            }
            else
            {
                this.ShowInTaskbar = true;
                NIcon.Visible = false;
            }
        }       
    
    }

    
}

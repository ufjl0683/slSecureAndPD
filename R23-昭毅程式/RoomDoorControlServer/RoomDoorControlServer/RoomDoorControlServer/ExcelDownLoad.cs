using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace RoomDoorControlServer
{
    public partial class ExcelDownLoad : Form
    {
        DataTable ExcelDT;
        //int CardType;
        //string IP;

        public ExcelDownLoad(string ip,int cardType)
        {           
            
            InitializeComponent();
            txtIP.Text = ip;
            cboType.SelectedIndex = cardType;

            try
            {
                //LoadExcelSaveDataBase();
                ExcelDT = LoadExcel();

            }
            catch
            {
                MessageBox.Show("載入格式不符");
                this.Close();
            }
            cboRoom.SelectedIndex = 0;
        }

        DataTable LoadExcel()
        {
            OpenFileDialog LoadFileDialog = new OpenFileDialog();
            LoadFileDialog.Filter = "xls|*.xls";
            DataTable CardDT = new DataTable();
            CardDT.Columns.Add("ABA_num");
            CardDT.Columns.Add("WEG_num");
            CardDT.Columns.Add("Message");
            CardDT.Columns.Add("Add");
            CardDT.Columns.Add("Memo");
            if (LoadFileDialog.ShowDialog() == DialogResult.OK)
            {

                System.IO.FileInfo LoadFile = new System.IO.FileInfo(LoadFileDialog.FileName);
                if (!LoadFile.Exists)
                {
                    MessageBox.Show("檔案不存在");
                    return CardDT;
                }

                DataTable DT = null;



                string strConn = "Provider =Microsoft.Jet.OLEDB.4.0;Data Source=" + LoadFileDialog.FileName + ";Extended Properties='Excel 8.0;IMEX = 1;'";
                OleDbConnection Conn = new OleDbConnection(strConn);
                try
                {
                    Conn.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return CardDT;
                }

                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Jet\4.0\Engines\Excel", true);
                object keyValue = key.GetValue("TypeGuessRows");
                try
                {
                    key.SetValue("TypeGuessRows", 16);

                    DataSet ds = new DataSet();
                    //clsDBComm commDB = new clsDBComm();
                    DataTable dt = Conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                    if (dt.Rows[0]["TABLE_Name"].ToString().IndexOf("$") < 0)
                    {
                        dt.Rows[0]["TABLE_Name"] += "$";
                    }
                    int RowIndex = 0;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["TABLE_Name"].Equals("Sheet2$"))
                        {
                            RowIndex = i;
                            break;
                        }
                    }
                    string strSelect = "Select * From [" + dt.Rows[RowIndex]["TABLE_Name"].ToString() + "] ";
                    OleDbDataAdapter da = new OleDbDataAdapter(strSelect, strConn);

                    da.Fill(ds, dt.Rows[RowIndex]["TABLE_Name"].ToString());
                    da.Dispose();

                    Conn.Close();
                    Conn.Dispose();

                    //ds.Tables[dt.Rows[0]["TABLE_Name"].ToString()].Rows.RemoveAt(0);


                    DT = ds.Tables[dt.Rows[RowIndex]["TABLE_Name"].ToString()];
                    foreach (DataRow dr in DT.Rows)
                    {
                        if (dr[2] == DBNull.Value)
                            continue;
                        DataRow CardRow = CardDT.NewRow();
                        CardRow["ABA_num"] = dr[1].ToString() + dr[2].ToString();
                        CardRow["WEG_num"] = "00" + dr[3].ToString() + dr[4].ToString();
                        CardRow["Message"] = dr[6].Equals(DBNull.Value) ? dr[5] : dr[6].ToString();
                        CardRow["Add"] = dr[7];
                        CardRow["Memo"] = dr[8];
                        CardDT.Rows.Add(CardRow);
                    }

                }
                catch (Exception ex)
                {
                    Conn.Close();
                    Conn.Dispose();
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    key.SetValue("TypeGuessRows", keyValue);
                }
            }
            else
            {
                this.Close();
                return null;
            }
            return CardDT;
        }

        DataTable LoadExcelSaveDataBase()
        {
            OpenFileDialog LoadFileDialog = new OpenFileDialog();
            LoadFileDialog.Filter = "xls|*.xls";
            DataTable CardDT = new DataTable();
            CardDT.Columns.Add("ABA_num");
            CardDT.Columns.Add("WEG_num");
            CardDT.Columns.Add("Message");
            CardDT.Columns.Add("Add");
            CardDT.Columns.Add("Memo");
            if (LoadFileDialog.ShowDialog() == DialogResult.OK)
            {

                System.IO.FileInfo LoadFile = new System.IO.FileInfo(LoadFileDialog.FileName);
                if (!LoadFile.Exists)
                {
                    MessageBox.Show("檔案不存在");
                    return CardDT;
                }

                DataTable DT = null;



                string strConn = "Provider =Microsoft.Jet.OLEDB.4.0;Data Source=" + LoadFileDialog.FileName + ";Extended Properties='Excel 8.0;IMEX = 1;'";
                OleDbConnection Conn = new OleDbConnection(strConn);
                try
                {
                    Conn.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return CardDT;
                }

                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Jet\4.0\Engines\Excel", true);
                object keyValue = key.GetValue("TypeGuessRows");
                //try
                //{
                key.SetValue("TypeGuessRows", 16);

                DataSet ds = new DataSet();
                //clsDBComm commDB = new clsDBComm();
                DataTable dt = Conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                if (dt.Rows[0]["TABLE_Name"].ToString().IndexOf("$") < 0)
                {
                    dt.Rows[0]["TABLE_Name"] += "$";
                }
                int RowIndex = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["TABLE_Name"].Equals("'20111022_門禁發卡編號$'"))
                    {
                        RowIndex = i;
                        break;
                    }
                }
                string strSelect = "Select * From [" + dt.Rows[RowIndex]["TABLE_Name"].ToString() + "] ";
                OleDbDataAdapter da = new OleDbDataAdapter(strSelect, strConn);

                da.Fill(ds, dt.Rows[RowIndex]["TABLE_Name"].ToString());
                da.Dispose();

                Conn.Close();
                Conn.Dispose();

                //ds.Tables[dt.Rows[0]["TABLE_Name"].ToString()].Rows.RemoveAt(0);
                dbroomEntities dbroom = new dbroomEntities();
                //tblControllerConfig NewControll = new tblControllerConfig();
                //NewControll.ControlID = "TEST-1";
                //NewControll.ControlType = 1;
                //NewControll.ERId = 1;
                //dbroom.AddTotblControllerConfigs(NewControll);
                //tblEntranceGuardConfig newEgc = new tblEntranceGuardConfig();
                //newEgc.EntranceCode = "TEST-1-1";
                //newEgc.ERId = 1;
                //newEgc.Floor = 1;
                //newEgc.EntranceType = "1";
                //newEgc.Memo = "TEST";
                //dbroom.AddTotblEntranceGuardConfigs(newEgc);
                //tblCardReaderConfig newReader = new tblCardReaderConfig();
                //newReader.ReaderID = 1;
                //newReader.EntranceCode ="TEST-1-1";
                //newReader.ControlID = "TEST-1";
                //newReader.ReaderType = 1;
                //newReader.IP = "0.0.0.0";
                //newReader.Port = 6666;
                //dbroom.AddTotblCardReaderConfigs(newReader);


                DT = ds.Tables[dt.Rows[RowIndex]["TABLE_Name"].ToString()];
                foreach (DataRow dr in DT.Rows)
                {
                    if (dr[2] == DBNull.Value || dr[7] == DBNull.Value)
                        continue;
                    DataRow CardRow = CardDT.NewRow();
                    CardRow["ABA_num"] = dr[1].ToString() + dr[2].ToString();
                    CardRow["WEG_num"] = "00" + dr[3].ToString() + dr[4].ToString();
                    CardRow["Message"] = dr[6].Equals(DBNull.Value) ? dr[5] : dr[6].ToString();
                    CardRow["Add"] = dr[7];
                    CardRow["Memo"] = dr[8];
                    CardDT.Rows.Add(CardRow);

                    int roleID;
                    string GroupName = dr[5].Equals(DBNull.Value) ? "交控中心" : dr[5].ToString();
                    var data = from o in dbroom.tblSysRole where o.RoleName == GroupName select o;

                    if (data.Count() == 0)
                    {
                        tblSysRole newRole = new tblSysRole();
                        int num = (from o in dbroom.tblSysRole select o).Count() + 1;
                        newRole.RoleID = num;
                        newRole.RoleName = dr[5].Equals(DBNull.Value) ? "交控中心" : dr[5].ToString();
                        dbroom.tblSysRole.Add(newRole);
                        dbroom.SaveChanges();

                        roleID = num;
                    }
                    else
                    {
                        roleID = data.First().RoleID;
                    }

                    //tblSysUser newUser = new tblSysUser();
                    //dbroom.ExecuteFunction("delete from tblSysUsers;");
                    //int UserNum = (from o in dbroom.tblSysUsers select o).Count() + 1;
                    //newUser.UserId = UserNum.ToString();
                    //newUser.UserName = dr[6].Equals(DBNull.Value) ? string.Empty : dr[6].ToString();
                    //newUser.RoleId = roleID;
                    //newUser.Enable = "Y";
                    //dbroom.AddTotblSysUsers(newUser);
                    string aba = CardRow["ABA_num"].ToString();

                    var CardData = from o in dbroom.tblMagneticCard where o.ABA == aba select o;
                    if (CardData.Count() > 0)
                    {
                        var oldCard = CardData.First();
                        oldCard.RoleID = roleID;
                        oldCard.Name = dr[6].Equals(DBNull.Value) ? string.Empty : dr[6].ToString();
                        oldCard.Company = dr[5].Equals(DBNull.Value) ? string.Empty : dr[5].ToString();
                        
                    }
                    else
                    {

                        tblMagneticCard newCard = new tblMagneticCard();
                        newCard.MagneticID = (from o in dbroom.tblMagneticCard select o).Count() + 1;
                        newCard.ABA = aba;
                        newCard.WEG1 = "00" + dr[3].ToString();
                        newCard.WEG2 = dr[4].ToString();
                        newCard.RoleID = roleID;
                        newCard.Name = dr[6].Equals(DBNull.Value) ? string.Empty : dr[6].ToString();
                        newCard.Company = dr[5].Equals(DBNull.Value) ? string.Empty : dr[5].ToString();
                        newCard.Enable = "Y";
                        newCard.Memo = dr[8].ToString();
                        dbroom.tblMagneticCard.Add(newCard);
                    }
                    dbroom.SaveChanges();
                }
                //}
                //catch (Exception ex)
                //{
                //    Conn.Close();
                //    Conn.Dispose();
                //    MessageBox.Show(ex.Message);
                //}
                //finally
                //{
                key.SetValue("TypeGuessRows", keyValue);
                //}
            }
            else
            {
                this.Close();
                return null;
            }
            return CardDT;
        }


        private void cboRoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ExcelDT == null)
                return;
            dataGridView1.Rows.Clear();
            string room = cboRoom.SelectedItem.ToString();
            string Line = GetLine(room);
            try
            {
                foreach (DataRow dr in ExcelDT.Rows)
                {
                    if (dr[3].ToString() == "全區" || dr[3].ToString().Contains(room) || (dr[3].ToString().Contains("所有") && dr[3].ToString().Contains(Line)))
                    {
                        dataGridView1.Rows.Add(dr[0], dr[1], dr[2], dr[3], dr[4]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            
        }

        string GetLine(string room)
        {
            switch (room)
            {
                case "中港溪":
                case "苗栗":
                case "泰安":
                case "台中":
                case "彰化":
                case "員林":
                case "斗南":
                    return "國一";
                case "後龍":
                case "西湖":
                case "大甲":
                case "清水":
                case "彰化系統":
                case "草屯":
                case "南投":
                case "名間":
                case "林內":
                    return "國三";
                case "東草屯":
                case "國姓1西口":
                case "國姓2西口":
                case "埔里東口":
                    return "國六";
            }
            return string.Empty;
        }

        private void btnDownLoad_Click(object sender, EventArgs e)
        {
            btnDownLoad.Enabled = false;
            this.Cursor = Cursors.WaitCursor;
            if (cboType.SelectedIndex == 0)
            {
                CardControl card = new CardControl(txtIP.Text);
                List<string> CardList = new List<string>();
                foreach (DataGridViewRow dr in dataGridView1.Rows)
                {
                    try
                    {
                        if (dr.Cells[0].Value != null)
                        {
                            CardList.Add(dr.Cells[0].Value.ToString() + "," + dr.Cells[1].Value.ToString() + "," + dr.Cells[2].Value.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }                    
                }
                string ErrorMessage = card.AddUserData(CardList);
                if (ErrorMessage == "")
                {
                    MessageBox.Show("全部卡號下載成功");
                }
                else
                {
                    MessageBox.Show(ErrorMessage);
                }

            }
            else
            {
                RAC960CardControl card = new RAC960CardControl(txtIP.Text);
                List<string> CardList = new List<string>();
                foreach (DataGridViewRow dr in dataGridView1.Rows)
                {
                    try
                    {
                        if (dr.Cells[0].Value != null)
                        {
                            CardList.Add(dr.Cells[0].Value.ToString() + "," + dr.Cells[2].Value.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }  
                }
                string ErrorMessage = card.AddAllUserData(CardList);
                if (ErrorMessage == "")
                {
                    MessageBox.Show("全部卡號下載成功");
                }
                else
                {
                    MessageBox.Show(ErrorMessage);
                }
            }
            this.Cursor = Cursors.Default;
            btnDownLoad.Enabled = true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using System.ServiceModel.DomainServices.Client;
using slSecure.Web;
using slSecure;
using System.Threading.Tasks;
using slSecureLib;
using System.Windows.Data;
using slWCFModule;
using System.Reflection;
using System.Windows.Browser;

namespace slSecureLib.Forms.R13
{
    public partial class slSetMagneticCard : Page, slWCFModule.RemoteService.ISecureServiceCallback
    {
        slSecure.Web.SecureDBContext db = slSecure.DB.GetDB();
        List<TreeViewModel> TreeList;
        PagedCollectionView pageView;
        string actType, tempStartDate, tempEndDate, tempType, NewtempType;
        slWCFModule.MyClient client;
        System.Windows.Threading.DispatcherTimer myDispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        CheckBox chBkx;
        List<ColumnDetails> _PropertyDetails = new List<ColumnDetails>();
        string ReportViewer = "";

        public slSetMagneticCard()
        {
            InitializeComponent();
        }

        // 使用者巡覽至這個頁面時執行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                client = new MyClient("CustomBinding_ISecureService");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        //記得在void與private之間加入async
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                cb_PageSize_Query.SelectedIndex = 0;
                cb_PageSize_Qy.SelectedIndex = 0; 

                GetMagneticCardNormalGroup();
                GetEngineRoomConfig();

                QueryReportViewer();

                QueryMagneticCard();
                //QueryMagneticCardDetail();

                //自訂欄位
                LoadListWithClassProperties();
                //Bind the ColumnName collection to ListBox 
                MagneticCardColumns.ItemsSource = PropertyDetails;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());           
            }

        }

        void ShowMagneticCardData()
        {
            sp_MagneticCardData.Visibility = Visibility.Visible;
            sp_DataGrid.Visibility = Visibility.Collapsed;
            sp_MagneticCardQuery.Visibility = Visibility.Collapsed;
        }

        void ShowMagneticCardQuery()
        {
            sp_MagneticCardData.Visibility = Visibility.Collapsed;
            sp_DataGrid.Visibility = Visibility.Visible;
            sp_MagneticCardQuery.Visibility = Visibility.Visible;
        }

        //查詢磁卡資料  
        async void QueryMagneticCard()
        {
            ShowMagneticCardQuery();

            NewMagneticCard();

            var q = await db.LoadAsync<tblMagneticCard>(db.GetTblMagneticCardQuery());
            //var q = await db.LoadAsync<vwMagneticCard>(db.GetVwMagneticCardQuery());
            //dataGrid.ItemsSource = q;

            //分頁，但會選取DataGrid第一筆
            pageView = new PagedCollectionView(q);
            
            dataGrid.ItemsSource = pageView;


            //PagedCollectionView view = new PagedCollectionView(pageView);
            ////依屬性名稱再分组
            //view.GroupDescriptions.Add(new PropertyGroupDescription("Name"));
            //dataGrid.ItemsSource = view;
        }

        //查詢磁卡詳細資料  
        async void QueryMagneticCardDetail()
        {
            var q = await db.LoadAsync<vwMagneticCardDetail>(db.GetVwMagneticCardDetailQuery());
            //dataGridDetail.ItemsSource = q;

            //分頁，但會選取DataGrid第一筆 
            pageView = new PagedCollectionView(q);
            //pageView = new PagedCollectionView(q.OrderBy(a => a.ABA));
            //dataGridDetail.ItemsSource = pageView;

            //PagedCollectionView view = new PagedCollectionView(pageView);
            ////依屬性名稱再分组 
            //view.GroupDescriptions.Add(new PropertyGroupDescription("ABA"));

            dataGridDetail.ItemsSource = pageView;
        }

        async void QueryReportViewer()
        {
            var q = await db.LoadAsync<tblSysParameter>(from b in db.GetTblSysParameterQuery() where b.VariableName == "ReportViewer" select b);

            tblSysParameter bc = q.First();
            ReportViewer = bc.VariableValue;
        }

        async void NewMagneticCard()
        {
            actType = "New";

            txt_MagneticID.Text = "";
            txt_RoleID.Text = "";
            txt_ABA.IsEnabled = true;
            txt_ABA.Text = "";
            txt_Name.Text = "";
            cb_Type.SelectedIndex = 0;
            cb_NormalName.SelectedIndex = 0;
            txt_IDNumber.Text = "";
            txt_Company.Text = "";
            txt_EmployeeNo.Text = "";
            txt_JobTitle.Text = "";
            txt_Tel.Text = "";
            txt_Mobile.Text = "";
            dp_Timestamp.SelectedDate = DateTime.Now;
            dp_ReturnDate.SelectedDate = null;
            dp_StartDate.SelectedDate = DateTime.Now;
            dp_EndDate.SelectedDate = DateTime.Now.AddMonths(3);
            cb_Enable.SelectedIndex = 0;
            tb_Memo.Text = "";

            TreeList = await TreeViewModel.SetTree("各機房之讀卡機清單", true);
            tv_TreeView.ItemsSource = TreeList;

        }
        //取得定期卡設定下拉選單
        async void GetMagneticCardNormalGroup()
        {
            var q = await db.LoadAsync<tblMagneticCardNormalGroup>
                (from b in db.GetTblMagneticCardNormalGroupQuery()
                 select b);

            //cb_NormalName.ItemsSource = q;
            //cb_NormalName.ItemsSource = q.Select(qq => qq.NormalName);
            List<string> lNormalName = new List<string>();
            lNormalName.Add("請選擇");
            foreach (var MCData in q)
            {
                lNormalName.Add(MCData.NormalName);
            }
            cb_NormalName.ItemsSource = lNormalName;
        }
        //取得機房名稱清單下拉選單
        async void GetEngineRoomConfig()
        {
            var q = await db.LoadAsync<tblEngineRoomConfig>
                (from b in db.GetTblEngineRoomConfigQuery()
                 select b);

            List<string> lEngineRoomList = new List<string>();
            lEngineRoomList.Add("請選擇");
            foreach (var MCData in q)
            {
                lEngineRoomList.Add(MCData.ERName);
            }

            cb_ERName_Qy.ItemsSource = lEngineRoomList;

            cb_ERName_Qy.SelectedIndex = 0;
            cb_Type_Query.SelectedIndex = 0;
            cb_Date_Query.SelectedIndex = 0;

            cb_Type_Qy.SelectedIndex = 0;
            cb_Date_Qy.SelectedIndex = 0;
        }
        //新增磁卡資料
        async Task AddMagneticCardAsync()
        {
            //1.新增磁卡資料
            //2.新增磁卡權限群組資料
            string tempABA = "";
            tempABA = (Convert.ToUInt32(txt_ABA.Text)).ToString("0000000000");

            tblSysRole role = new tblSysRole()
            {
                // RoleID = bc2.RoleID + 1,
                RoleName = tempABA,
                UpdateDate = DateTime.Now
            };
            DateTime tmp_StartDate = (DateTime)dp_StartDate.SelectedDate;
            DateTime tmp_EndDate = (DateTime)dp_EndDate.SelectedDate;

            string s_StartDate = tmp_StartDate.ToShortDateString() + " 00:00:00";
            string s_EndDate = tmp_EndDate.ToShortDateString() + " 23:59:59";

            role.tblMagneticCard.Add(new tblMagneticCard()
            {
                //MagneticID = bc.MagneticID + 1,
                ABA = tempABA,//txt_ABA.Text,
                //WEG1 = GetWEG(tempABA).Substring(0, 5),
                //WEG2 = GetWEG(tempABA).Substring(5, 5),
                Name = txt_Name.Text,
                Type = (short)cb_Type.SelectedIndex,
                NormalID = cb_NormalName.SelectedIndex,
                IDNumber = txt_IDNumber.Text,
                Company = txt_Company.Text,
                EmployeeNo = txt_EmployeeNo.Text,
                JobTitle = txt_JobTitle.Text,
                Tel = txt_Tel.Text,
                Mobile = txt_Mobile.Text,
                Timestamp = (DateTime)dp_Timestamp.SelectedDate,
                ReturnDate = dp_ReturnDate.SelectedDate,
                //StartDate = dp_StartDate.SelectedDate,
                StartDate = Convert.ToDateTime(s_StartDate),
                //EndDate = (DateTime)dp_EndDate.SelectedDate,
                EndDate = Convert.ToDateTime(s_EndDate),
                Enable = cb_Enable.SelectedValue.ToString(),
                Memo = tb_Memo.Text,
                //RoleID = bc2.RoleID + 1
            });
            db.tblSysRoles.Add(
                role
               );
            try
            {
                bool res = await db.SubmitChangesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Role-Adding Data failed due to " + ex.Message);
            }
            //2.加入磁卡權限清單
            await AddGroup(role.RoleID);

            MessageBox.Show("新增磁卡成功!");
        }
        //修改磁卡資料
        async Task ModifyMagneticCardAsync()
        {
            //1.加入磁卡權限清單
            await AddGroup(int.Parse(txt_RoleID.Text));

            var magneticID = int.Parse(txt_MagneticID.Text);
            //2.修改磁卡資料
            var q = await db.LoadAsync<tblMagneticCard>(from b in db.GetTblMagneticCardQuery() where b.MagneticID == magneticID select b);
            tblMagneticCard bc = q.First();

            DateTime tmp_StartDate = (DateTime)dp_StartDate.SelectedDate;
            DateTime tmp_EndDate = (DateTime)dp_EndDate.SelectedDate;

            string s_StartDate = tmp_StartDate.ToShortDateString() + " 00:00:00";
            string s_EndDate = tmp_EndDate.ToShortDateString() + " 23:59:59";

            int iNormalName = 0;
            if (cb_Type.SelectedIndex == 1)
            {
                iNormalName = cb_NormalName.SelectedIndex;
                
            }
            else
            {
                iNormalName = 0;
            }
            //string s_ReturnDate = "";
            //if (dp_ReturnDate.SelectedDate != null)
            //{
            //    DateTime tmp_ReturnDate = (DateTime)dp_ReturnDate.SelectedDate;
            //    s_ReturnDate = tmp_ReturnDate.ToShortDateString() + " 23:59:59";
            //}

            //string tempABA = "";
            //tempABA = (Convert.ToUInt32(txt_ABA.Text)).ToString("0000000000");

            //bc.ABA = txt_ABA.Text;//tempABA;
            //bc.WEG1 = GetWEG(tempABA).Substring(0, 5);
            //bc.WEG2 = GetWEG(tempABA).Substring(5, 5);
            bc.Name = txt_Name.Text;
            bc.Type = (short)cb_Type.SelectedIndex;
            //bc.NormalID = cb_NormalName.SelectedIndex;
            bc.NormalID = iNormalName;
            bc.IDNumber = txt_IDNumber.Text;
            bc.Company = txt_Company.Text;
            bc.EmployeeNo = txt_EmployeeNo.Text;
            bc.JobTitle = txt_JobTitle.Text;
            bc.Tel = txt_Tel.Text;
            bc.Mobile = txt_Mobile.Text;
            bc.Timestamp = (DateTime)dp_Timestamp.SelectedDate;
            bc.ReturnDate = dp_ReturnDate.SelectedDate;
            //bc.StartDate = dp_StartDate.SelectedDate;
            bc.StartDate = Convert.ToDateTime(s_StartDate);
            //bc.EndDate = (DateTime)dp_EndDate.SelectedDate;
            bc.EndDate = Convert.ToDateTime(s_EndDate);
            bc.Enable = cb_Enable.SelectedValue.ToString();
            bc.Memo = tb_Memo.Text;
            try
            {
                bool res = await db.SubmitChangesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("MagneticCard-Data updation failed due to " + ex.Message);
            }
            MessageBox.Show("修改磁卡成功!");
        }
        //刪除磁卡資料
        async Task DeleteCardCommandLogAsync()
        {
            //非同步模擬成同步
            var q = await db.LoadAsync<tblCardCommandLog>(from b in db.GetTblCardCommandLogQuery() where ((b.ABA == txt_ABA.Text) && (b.ControlID =="*")) select b);
            if (q.Count() > 0)
            {
                tblCardCommandLog bc = q.First();

                db.tblCardCommandLogs.Remove(bc);
                try
                {
                    bool res = await db.SubmitChangesAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("MagneticCard-Data deletion failed due to " + ex.Message);
                }
            }
        }
        //修改磁卡日期，寫入CardCommandLog檔通知
        async Task InsertCardCommandLogAsync()
        {
            string sCardType = "";
            if (cb_Type.SelectedIndex == 4)
                sCardType = "P";
            else
                sCardType = "C";
            //string temp = "";
            //if (dp_StartDate.SelectedDate.ToString() != tempStartDate)
            //{
            //    temp = "起始日期由" + tempStartDate + "修改為" + dp_StartDate.SelectedDate.ToString();
            //}
            //if (((DateTime)dp_EndDate.SelectedDate).ToString() != tempEndDate)
            //{
            //    if (temp != "")
            //        temp += "；";
            //    temp += "到期日期由" + tempEndDate + "修改為" + ((DateTime)dp_EndDate.SelectedDate).ToString();
            //}
            db.tblCardCommandLogs.Add(
                new tblCardCommandLog()
                {
                    ABA = txt_ABA.Text,
                    ControlID = "*",
                    CommandType = "*",
                    CardType = sCardType//,
                    //Describe = temp
                }             
                );
            try
            {
                bool res = await db.SubmitChangesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("CardCommandLog-Data updation failed due to " + ex.Message);
            }
            //MessageBox.Show("修改磁卡日期成功!");
        }
        //刪除磁卡資料
        async Task DeleteMagneticCardAsync()
        {
            var magneticID = int.Parse(txt_MagneticID.Text);
            //非同步模擬成同步
            var q = await db.LoadAsync<tblMagneticCard>(from b in db.GetTblMagneticCardQuery() where b.MagneticID == magneticID select b);
            tblMagneticCard bc = q.First();

            db.tblMagneticCards.Remove(bc);
            try
            {
                bool res = await db.SubmitChangesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("MagneticCard-Data deletion failed due to " + ex.Message);
            }
        }
        //刪除磁卡權限資料
        async Task DeleteRoleAuthorityAsync()
        {
            var roleID = int.Parse(txt_RoleID.Text);
            //非同步模擬成同步
            var q = await db.LoadAsync<tblSysRoleAuthority>(from b in db.GetTblSysRoleAuthorityQuery() where b.RoleID == roleID select b);

            if (q.Count() > 0)
            {
                foreach (tblSysRoleAuthority st in q)
                {
                    db.tblSysRoleAuthorities.Remove(st);
                    try
                    {
                        bool res = await db.SubmitChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("RoleAuthority-Data deletion failed due to " + ex.Message);
                    }
                }


            }
        }
        //刪除磁卡權限群組資料
        async Task DeleteRoleAsync()
        {
            var roleID = int.Parse(txt_RoleID.Text);
            //非同步模擬成同步
            var q = await db.LoadAsync<tblSysRole>(from b in db.GetTblSysRoleQuery() where b.RoleID == roleID select b);
            tblSysRole bc = q.First();

            db.tblSysRoles.Remove(bc);
            try
            {
                bool res = await db.SubmitChangesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Role--Data deletion failed due to " + ex.Message);
            }
            MessageBox.Show("刪除磁卡成功!");
        }
        //確認磁卡ABA是否已存在
        async void CheckMagneticCard()
        {
            bool isPassedValidation = true;
            txt_ABA.ClearValidationError();

            if (string.IsNullOrEmpty(txt_ABA.Text) || txt_ABA.Text.IsNumberValid() == false)
            {
                txt_ABA.SetValidation("請輸入合法的整數");
                txt_ABA.RaiseValidationError();
                isPassedValidation = false;
            }

            if (isPassedValidation == true)
            {
                string sABA = "", sWEG1 = "", sWEG2 = "";

                if (txt_ABA.Text != "")
                {
                    uint iABA = Convert.ToUInt32(txt_ABA.Text);
                    sABA = iABA.ToString("0000000000");
                    string sWEG12 = GetWEG(sABA);

                    sWEG1 = sWEG12.Substring(0, 5);
                    sWEG2 = sWEG12.Substring(5, 5);

                    var q = await db.LoadAsync<tblMagneticCard>(from b in db.GetTblMagneticCardQuery() where b.ABA == sABA select b);
                    if (q.Count() > 0)
                    {
                        txt_ABA.SetValidation("磁卡(ABA編碼)已存在,不得重覆!");
                        txt_ABA.RaiseValidationError();
                    }
                }
            }
        }
        //由磁卡ABA去推算出WEG1及WEG2(R23)
        public static string GetWEG(string ABA)
        {
            uint aba = Convert.ToUInt32(ABA);
            uint weg1 = aba / 65536;
            uint weg2 = aba % 65536;
            return weg1.ToString("00000") + weg2.ToString("00000");
        }
        //加入磁卡權限清單
        async Task AddGroup(int RoleID)
        {
            if (RoleID != 0)
            {
                List<string> objList = new List<string>();
                if (TreeList != null && TreeList.Count > 0)
                {
                    objList = TreeViewModel.GetTree(TreeList.First());

                    //1.檢查treeview，若為空，則刪除DB全部RoleID的資料
                    var q = await db.LoadAsync<tblSysRoleAuthority>(from b in db.GetTblSysRoleAuthorityQuery() where b.RoleID == RoleID select b);
                    if (objList.Count() == 0)
                    {

                        foreach (tblSysRoleAuthority st in q)
                        {
                            db.tblSysRoleAuthorities.Remove(st);

                            try
                            {
                                bool res = await db.SubmitChangesAsync();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("RoleAuthority-Data deletion failed due to " + ex.Message);
                            }
                        }
                    }
                    else
                    {
                        //(1)若treeview有，而db沒有則新增
                        foreach (string id in objList)
                        {
                            if (q.Where(n => n.ControlID == id).Select(n => n.ControlID).FirstOrDefault() == null)
                            {
                                db.tblSysRoleAuthorities.Add(
                                      new tblSysRoleAuthority()
                                      {
                                          RoleID = RoleID,
                                          ControlID = id
                                      }
                                    );
                                //add new
                            }
                            else
                            {
                                //虛擬卡改一般卡要先刪一虛擬卡再加一般卡，反之亦然
                                //查看list和db皆有，已存在的，若有修改型別，需先CardCommandLog刪除，再寫入既有刪除型別+目前新增型別
                                if (cb_Type.SelectedIndex.ToString() == "4")
                                    NewtempType = "P";
                                else
                                    NewtempType = "C";

                                if (NewtempType != tempType)
                                {
                                    //刪除
                                    var q1 = await db.LoadAsync<tblCardCommandLog>(from b in db.GetTblCardCommandLogQuery() where ((b.ABA == txt_ABA.Text) && (b.ControlID == id)) select b);
                                    if (q1.Count() > 0)
                                    {
                                        tblCardCommandLog bc = q1.First();

                                        db.tblCardCommandLogs.Remove(bc);
                                        try
                                        {
                                            bool res = await db.SubmitChangesAsync();
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show("MagneticCard-Data deletion failed due to " + ex.Message);
                                        }
                                    }
                                    //新增-刪除的log檔
                                    db.tblCardCommandLogs.Add(
                                        new tblCardCommandLog()
                                        {
                                            ABA = txt_ABA.Text,
                                            ControlID = id,
                                            CommandType = "D",
                                            CardType = tempType
                                        }
                                        );
                                    try
                                    {
                                        bool res = await db.SubmitChangesAsync();
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show("CardCommandLog-Data updation failed due to " + ex.Message);
                                    }
                                    //新增
                                    db.tblCardCommandLogs.Add(
                                        new tblCardCommandLog()
                                        {
                                            ABA = txt_ABA.Text,
                                            ControlID = id,
                                            CommandType = "I",
                                            CardType = NewtempType
                                        }
                                        );
                                    try
                                    {
                                        bool res = await db.SubmitChangesAsync();
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show("CardCommandLog-Data updation failed due to " + ex.Message);
                                    }
                                }
                            }
                        }
                        //(2)若treeview沒有，而db有則刪除
                        foreach (tblSysRoleAuthority tbl in q)
                        {
                            if (objList.Where(n => n == tbl.ControlID).FirstOrDefault() == null)
                            {
                                db.tblSysRoleAuthorities.Remove(tbl);
                            }
                        }

                    }
                    try
                    {
                        bool res = await db.SubmitChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("RoleAuthority-Adding Data failed due to " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("請先選擇群組清單!");
            }
        }

        private void bu_New_Click(object sender, RoutedEventArgs e)
        {
            QueryMagneticCard();

            ShowMagneticCardData();
        }

        private async void bu_Add_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("是否確定儲存磁卡資料?", "儲存", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                if (txt_ABA.Text == "")
                {
                    MessageBox.Show("磁卡(ABA編碼)，不得為空值！");
                }
                else if (dp_Timestamp.SelectedDate.ToString() == "")
                {
                    MessageBox.Show("發卡日期，不得為空值！");
                }
                else if (dp_StartDate.SelectedDate.ToString() == "")
                {
                    MessageBox.Show("起始日期，不得為空值！");
                }
                else if (dp_EndDate.SelectedDate.ToString() == "")
                {
                    MessageBox.Show("到期日期，不得為空值！");
                }
                //else if (dp_EndDate.SelectedDate <= DateTime.Now)
                //{
                //    MessageBox.Show("到期日期，不得小於今天的日期！");
                //}
                else if (cb_Type.SelectedIndex <= 0)
                {
                    MessageBox.Show("請選擇「磁卡類型」");
                }
                else
                {
                    if (actType == "New")
                    {
                        await AddMagneticCardAsync();
                    }
                    else if (actType == "Update")
                    {
                        await ModifyMagneticCardAsync();
                        if ((dp_StartDate.SelectedDate.ToString() != tempStartDate) ||
                            (((DateTime)dp_EndDate.SelectedDate).ToString() != tempEndDate))
                        {
                            await DeleteCardCommandLogAsync();
                            await InsertCardCommandLogAsync();
                        }
                    }
                    
                    //有改變，通知Server
                    client.SecureService.NotifyDBChangeAsync(slWCFModule.RemoteService.DBChangedConstant.AuthorityChanged,"");
                    client.SecureService.NotifyDBChangeCompleted += (s, a) =>
                    {
                        if (a.Error != null)
                        {
                            MessageBox.Show(a.Error.Message);
                            return;
                        }
                    };
                    QueryMagneticCard();
                }
            }


            ShowMagneticCardQuery();
        }

        async void DelData()
        {
            var result = MessageBox.Show("是否確定刪除?", "提示刪除", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {

                //1.刪除磁卡權限清單
                await DeleteRoleAuthorityAsync();
                //2.刪除磁卡資料
                await DeleteMagneticCardAsync();
                //3.刪除磁卡權限群組
                await DeleteRoleAsync();

                //有改變，通知Server
                client.SecureService.NotifyDBChangeAsync(slWCFModule.RemoteService.DBChangedConstant.AuthorityChanged, "");
                client.SecureService.NotifyDBChangeCompleted += (s, a) =>
                {
                    if (a.Error != null)
                    {
                        MessageBox.Show(a.Error.Message);
                        return;
                    }

                };
                QueryMagneticCard();

            }
        }

        private  void bu_Del_Click(object sender, RoutedEventArgs e)
        {
            DelData();
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //透過txt_RoleID.Text來取TreeView資料
            if (dataGrid.CurrentColumn != null && txt_RoleID.Text != "")
            {
                //actType = "Update";
                //txt_ABA.IsEnabled = false;

                //tempStartDate = dp_StartDate.SelectedDate.ToString();
                //tempEndDate = ((DateTime)dp_EndDate.SelectedDate).ToString();
                //if (cb_Type.SelectedIndex.ToString() == "4")
                //    tempType = "P";
                //else
                //    tempType = "C";

                //var iRoleID = int.Parse(txt_RoleID.Text);
                //slSecure.Web.SecureDBContext db;
                //db = slSecure.DB.GetDB();
                //var RoleAuthorityData = await db.LoadAsync<tblSysRoleAuthority>(from b in db.GetTblSysRoleAuthorityQuery() where b.RoleID == iRoleID select b);

                //List<string> objList = new List<string>();
                //foreach (var RoleAuthority in RoleAuthorityData)
                //{
                //    objList.Add(RoleAuthority.ControlID);
                //}

                //bool CanSelect = TreeList.First().IsCanSelect;
                //TreeList.First().IsCanSelect = true;
                //TreeViewModel.SetTree(TreeList.First(), objList);
                //TreeList.First().IsCanSelect = CanSelect;
            }
        }

        private async void bu_Query_Click(object sender, RoutedEventArgs e)
        {
            //var q = await db.LoadAsync<vwMagneticCard>(db.GetVwMagneticCardQuery());
            var q = await db.LoadAsync<tblMagneticCard>(db.GetTblMagneticCardQuery());

            if (!string.IsNullOrEmpty(txt_ABA_Query.Text))
            {
                q = q.Where(a => a.ABA == txt_ABA_Query.Text);
            }

            if (!string.IsNullOrEmpty(txt_Name_Query.Text))
            {
                q = q.Where(a => a.Name == txt_Name_Query.Text);
            }

            if (!string.IsNullOrEmpty(txt_Company_Query.Text))
            {
                q = q.Where(a => a.Company == txt_Company_Query.Text);
            }
            

            //if (cb_Type_Query.SelectedIndex != 0)
            //{
            //    q = q.Where(a => a.TypeName == cb_Type_Query.SelectedValue.ToString());
            //}

            if (cb_Type_Query.SelectedIndex != 0)
            {
                q = q.Where(a => a.Type == cb_Type_Query.SelectedIndex);
            }

            //if (cb_ERName_Query.SelectedIndex != 0)
            //{
            //    q = q.Where(a => a.ERName == cb_ERName_Query.SelectedValue.ToString());
            //}

            if (cb_Date_Query.SelectedIndex != 0)
            {
                if (dp_StartDate_Query.SelectedDate != null || dp_EndDate_Query.SelectedDate != null)
                {
                    bool flag = Comm.checkTimes(dp_StartDate_Query.SelectedDate.ToString(), dp_EndDate_Query.SelectedDate.ToString());

                    if (cb_Date_Query.SelectedValue.ToString() == "StartDate")
                    {
                        if (flag == true)
                        {
                            q = q.Where(a => ((a.StartDate >= Comm.strTime) && (a.StartDate <= Comm.endTime)));
                        }
                    }
                    if (cb_Date_Query.SelectedValue.ToString() == "EndDate")
                    {
                        if (flag == true)
                        {
                            q = q.Where(a => ((a.EndDate >= Comm.strTime) && (a.EndDate <= Comm.endTime)));
                        }
                    }
                    if (cb_Date_Query.SelectedValue.ToString() == "Timestamp")
                    {
                        if (flag == true)
                        {
                            q = q.Where(a => ((a.Timestamp >= Comm.strTime) && (a.Timestamp <= Comm.endTime)));
                        }
                    }
                    if (cb_Date_Query.SelectedValue.ToString() == "ReturnDate")
                    {
                        if (flag == true)
                        {
                            q = q.Where(a => ((a.ReturnDate >= Comm.strTime) && (a.ReturnDate <= Comm.endTime)));
                        }
                    }
                }
            }

            //分頁，但會選取DataGrid第一筆
            pageView = new PagedCollectionView(q);
            //pageView = new PagedCollectionView(q.OrderBy(a => a.ABA));

            //PagedCollectionView view = new PagedCollectionView(pageView);
            ////依屬性名稱再分组
            //view.GroupDescriptions.Add(new PropertyGroupDescription("ABA"));

            //分頁，但會選取DataGrid第一筆
            
            dataGrid.ItemsSource = pageView;
        }

        private void cb_Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_Type.SelectedIndex >= 2)
            {
                cb_NormalName.Visibility = Visibility.Collapsed;
                bu_SetNormalName.Visibility = Visibility.Collapsed;
            }
            else
            {
                cb_NormalName.Visibility = Visibility.Visible;
                bu_SetNormalName.Visibility = Visibility.Visible;
            }
        }

        private void cb_Date_Query_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_Date_Query.SelectedIndex == 0)
            {
                sp_Date_Query.Visibility = Visibility.Collapsed;
            }
            else
            {
                sp_Date_Query.Visibility = Visibility.Visible;
            }
        }

        private void cb_Date_Qy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_Date_Qy.SelectedIndex == 0)
            {
                sp_Date_Qy.Visibility = Visibility.Collapsed;
            }
            else
            {
                sp_Date_Qy.Visibility = Visibility.Visible;
            }
        }


        private void txt_ABA_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (actType == "New")
            {
                CheckMagneticCard();
            }
        }

        private void bu_OpenDoor_Click(object sender, RoutedEventArgs e)
        {
            List<string> objList = new List<string>();
            if (TreeList != null && TreeList.Count > 0)
            {
                objList = TreeViewModel.GetTree(TreeList.First());

                foreach (string ControlID in objList)
                {
                    client.SecureService.ForceOpenDoorAsync(ControlID);
                    client.SecureService.ForceOpenDoorCompleted += (s, a) =>
                    {
                        if (a.Error != null)
                        {
                            MessageBox.Show(a.Error.Message);
                            return;
                        }
                    };
                }
            }

        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            client.Dispose();
        }

        public void SayHello(string hello)
        {
            //throw new NotImplementedException();
        }

        public void SecureDoorEvent(slWCFModule.RemoteService.DoorEventType evttype, slWCFModule.RemoteService.DoorBindingData doorBindingData)
        {
            //throw new NotImplementedException();
        }

        public void SecureAlarm(slWCFModule.RemoteService.AlarmData alarmdata)
        {
            //throw new NotImplementedException();
        }

        /// <summary> 
        /// The logic for hinding/Unhiding the columns from DataGrid 
        /// </summary> 
        /// <param name="sender"></param> 
        /// <param name="e"></param>
        void chBkx_Click(object sender, RoutedEventArgs e)
        {
            chBkx = sender as CheckBox;
            bool isChecked = Convert.ToBoolean(chBkx.IsChecked);
            if (isChecked == false)
            {
                foreach (DataGridColumn dgColumn in dataGridDetail.Columns)
                {
                    if (dgColumn.Header.ToString() == chBkx.Content.ToString())
                    {
                        dgColumn.Visibility = System.Windows.Visibility.Collapsed;
                        break;
                    }
                }
            }
            if (isChecked == true)
            {
                foreach (DataGridColumn dgColumn in dataGridDetail.Columns)
                {
                    if (dgColumn.Header.ToString() == chBkx.Content.ToString())
                    {
                        dgColumn.Visibility = System.Windows.Visibility.Visible;
                        break;
                    }
                }
            }
        }

        private void LoadListWithClassProperties()
        {
            List<string> tempColumnName = new List<string>();
            foreach (var c in dataGridDetail.Columns)
            {
                tempColumnName.Add(c.Header.ToString());
            }
            foreach (string pName in tempColumnName)
            { PropertyDetails.Add(new ColumnDetails() { IsSelected = true, ColumnName = pName }); }
        }

        public List<ColumnDetails> PropertyDetails
        {
            get { return _PropertyDetails; }
            set { _PropertyDetails = value; }
        }

        public class ColumnDetails
        {
            public bool IsSelected { get; set; }
            public string ColumnName { get; set; }
        }

        private void bu_Excel_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.ItemsSource != null)
            {
                ////目前應用程式是否為Out of Browser執行
                //if (Application.Current.IsRunningOutOfBrowser)
                //{
                //    //透過OOB匯出excel檔(不用此方法，要避免電腦沒有安裝Excel的問題)
                //    dataGrid.ExportToExcel();
                //}
                //else
                //{
                //    dataGrid.ToCSV();
                //}
                dataGrid.ToCSV();
            }
        }

        private void bu_Qy_Excel_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridDetail.ItemsSource != null)
            {
                ////目前應用程式是否為Out of Browser執行
                //if (Application.Current.IsRunningOutOfBrowser)
                //{
                //    //透過OOB匯出excel檔(不用此方法，要避免電腦沒有安裝Excel的問題)
                //    dataGridDetail.ExportToExcel();
                //}
                //else
                //{
                //    dataGridDetail.ToCSV();
                //}
                dataGridDetail.ToCSV();
            }
        }

        private void bu_ModifyMagneticCardData_Click(object sender, RoutedEventArgs e)
        {
            //vwMagneticCard data = dataGrid.SelectedItem as vwMagneticCard;

            tblMagneticCard data = dataGrid.SelectedItem as tblMagneticCard;
            if (dataGrid.CurrentColumn != null && data.ABA != null)
            {
                ShowMagneticCardData();

                actType = "Update";
                txt_ABA.IsEnabled = false;

                txt_MagneticID.Text = data.MagneticID.ToString();
                txt_RoleID.Text = data.RoleID.ToString();
                if (data.ABA!=null)
                    txt_ABA.Text = data.ABA;
                if (data.Name != null)
                    txt_Name.Text = data.Name;
                if (data.Type != null)
                    cb_Type.SelectedIndex = (int)data.Type;
                if (data.NormalID != null)
                   cb_NormalName.SelectedIndex = (int)data.NormalID;
                if (data.IDNumber != null)
                    txt_IDNumber.Text = data.IDNumber;
                if (data.Company != null)
                   txt_Company.Text = data.Company;
                if (data.EmployeeNo != null)
                   txt_EmployeeNo.Text = data.EmployeeNo;
                if (data.JobTitle != null)
                    txt_JobTitle.Text = data.JobTitle;
                if (data.Tel != null)
                    txt_Tel.Text = data.Tel;
                if (data.Mobile != null)
                    txt_Mobile.Text = data.Mobile;
                if (data.Timestamp != null)
                    dp_Timestamp.SelectedDate = data.Timestamp;
                if (data.ReturnDate != null)
                    dp_ReturnDate.SelectedDate = data.ReturnDate;
                if (data.StartDate != null)
                    dp_StartDate.SelectedDate = data.StartDate;
                if (data.EndDate != null)
                    dp_EndDate.SelectedDate = data.EndDate;
                if (data.Enable != null)
                    cb_Enable.SelectedValue = data.Enable;
                if (data.Memo != null)
                    tb_Memo.Text = data.Memo;

                QueryData();

                //MagneticCardData magneticCardData = new MagneticCardData(data as tblMagneticCard);
                //magneticCardData.Show();
            }
        }

        private void bu_DelMagneticCardData_Click(object sender, RoutedEventArgs e)
        {
            //vwMagneticCard data = dataGrid.SelectedItem as vwMagneticCard;

            tblMagneticCard data = dataGrid.SelectedItem as tblMagneticCard;
            if (dataGrid.CurrentColumn != null && data.ABA != null)
            {
                txt_MagneticID.Text = data.MagneticID.ToString();
                txt_RoleID.Text = data.RoleID.ToString();
                txt_ABA.Text = data.ABA;
                txt_Name.Text = data.Name;
                cb_Type.SelectedIndex = (int)data.Type;
                cb_NormalName.SelectedIndex = (int)data.NormalID;
                txt_IDNumber.Text = data.IDNumber;
                txt_Company.Text = data.Company;
                txt_EmployeeNo.Text = data.EmployeeNo;
                txt_JobTitle.Text = data.JobTitle;
                txt_Tel.Text = data.Tel;
                txt_Mobile.Text = data.Mobile;
                dp_Timestamp.SelectedDate = data.Timestamp;
                dp_ReturnDate.SelectedDate = data.ReturnDate;
                dp_StartDate.SelectedDate = data.StartDate;
                dp_EndDate.SelectedDate = data.EndDate;
                cb_Enable.SelectedValue = data.Enable;
                tb_Memo.Text = data.Memo;

                DelData();

                //MagneticCardData magneticCardData = new MagneticCardData(data as tblMagneticCard);
                //magneticCardData.Show();
            }

        }
        async void QueryData()
        {
            tempStartDate = dp_StartDate.SelectedDate.ToString();
            tempEndDate = ((DateTime)dp_EndDate.SelectedDate).ToString();
            if (cb_Type.SelectedIndex.ToString() == "4")
                tempType = "P";
            else
                tempType = "C";

            var iRoleID = int.Parse(txt_RoleID.Text);
            //slSecure.Web.SecureDBContext db;
            //db = slSecure.DB.GetDB();
            var RoleAuthorityData = await db.LoadAsync<tblSysRoleAuthority>(from b in db.GetTblSysRoleAuthorityQuery() where b.RoleID == iRoleID select b);

            List<string> objList = new List<string>();
            foreach (var RoleAuthority in RoleAuthorityData)
            {
                objList.Add(RoleAuthority.ControlID);
            }

            bool CanSelect = TreeList.First().IsCanSelect;
            TreeList.First().IsCanSelect = true;
            TreeViewModel.SetTree(TreeList.First(), objList);
            TreeList.First().IsCanSelect = CanSelect;
        }

        private void bu_Cancel_Click(object sender, RoutedEventArgs e)
        {
            ShowMagneticCardQuery();
        }

        private async void bu_Qy_Click(object sender, RoutedEventArgs e)
        {
            var q = await db.LoadAsync<vwMagneticCardDetail>(db.GetVwMagneticCardDetailQuery());
            
            if (!string.IsNullOrEmpty(txt_ABA_Qy.Text))
            {
                q = q.Where(a => a.ABA == txt_ABA_Qy.Text);
            }

            if (!string.IsNullOrEmpty(txt_Name_Qy.Text))
            {
                q = q.Where(a => a.Name == txt_Name_Qy.Text);
            }

            if (cb_Type_Qy.SelectedIndex != 0)
            {
                q = q.Where(a => a.TypeName == cb_Type_Qy.SelectedValue.ToString());
            }

            if (cb_ERName_Qy.SelectedIndex != 0)
            {
                q = q.Where(a => a.ERName == cb_ERName_Qy.SelectedValue.ToString());
            }

            if (cb_Date_Qy.SelectedIndex != 0)
            {
                if (dp_StartDate_Qy.SelectedDate != null || dp_EndDate_Qy.SelectedDate != null)
                {
                    bool flag = Comm.checkTimes(dp_StartDate_Qy.SelectedDate.ToString(), dp_EndDate_Qy.SelectedDate.ToString());

                    if (cb_Date_Qy.SelectedValue.ToString() == "StartDate")
                    {
                        if (flag == true)
                        {
                            q = q.Where(a => ((a.StartDate >= Comm.strTime) && (a.StartDate <= Comm.endTime)));
                        }
                    }
                    if (cb_Date_Qy.SelectedValue.ToString() == "EndDate")
                    {
                        if (flag == true)
                        {
                            q = q.Where(a => ((a.EndDate >= Comm.strTime) && (a.EndDate <= Comm.endTime)));
                        }
                    }
                    if (cb_Date_Qy.SelectedValue.ToString() == "Timestamp")
                    {
                        if (flag == true)
                        {
                            q = q.Where(a => ((a.Timestamp >= Comm.strTime) && (a.Timestamp <= Comm.endTime)));
                        }
                    }
                    if (cb_Date_Qy.SelectedValue.ToString() == "ReturnDate")
                    {
                        if (flag == true)
                        {
                            q = q.Where(a => ((a.ReturnDate >= Comm.strTime) && (a.ReturnDate <= Comm.endTime)));
                        }
                    }
                }
            }

            //分頁，但會選取DataGrid第一筆
            pageView = new PagedCollectionView(q);
            //pageView = new PagedCollectionView(q.OrderBy(a => a.ABA));

            //PagedCollectionView view = new PagedCollectionView(pageView);
            ////依屬性名稱再分组
            //view.GroupDescriptions.Add(new PropertyGroupDescription("ABA"));

            //分頁，但會選取DataGrid第一筆
            dataGridDetail.ItemsSource = pageView;
        }

        private void cb_PageSize_Qy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dp_DGDetail.PageSize = int.Parse(cb_PageSize_Qy.SelectedValue.ToString()); 
        }

        private void cb_PageSize_Query_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dp_DG.PageSize = int.Parse(cb_PageSize_Query.SelectedValue.ToString());
        }

        private void bu_SetNormalName_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/slSecureLib;component/Forms/slSetNormalGroup.xaml", UriKind.Relative));
        }

        private void bu_SysOpenDoor_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/slSecureLib;component/Forms/R13/slOpenDoor.xaml", UriKind.Relative));
        }

        private void bu_SysDoorPassword_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/slSecureLib;component/Forms/R13/slSetDoorPassword.xaml", UriKind.Relative));
        }

        private void bu_Report_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/slSecureLib;component/Forms/slReport.xaml", UriKind.Relative));
        }

        private void bu_SysSetData_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/slSecureLib;component/Forms/slSetSysData.xaml", UriKind.Relative));
        }

        private void bu_ItemGroup_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/slSecureLib;component/Forms/slItemGroup.xaml", UriKind.Relative));
        }

        private void bu_RetrieveRTU_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/slSecureLib;component/Forms/R13/slRetrieveRTU.xaml", UriKind.Relative));
        }

        private void bu_AddRoleAuthorityData_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/slSecureLib;component/Forms/R13/slAddSysRoleAuthority.xaml", UriKind.Relative));
        }

        private void bu_Back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        private void bu_ReportPrint_Click(object sender, RoutedEventArgs e)
        {
            string tmpABA = "", tmpName = "", tmpTypeName = "", tmpERName = "", tmpColumnDateName = "", tmpStrTime = "", tmpEndTime = "";
            if (!string.IsNullOrEmpty(txt_ABA_Qy.Text))
            {
                tmpABA = txt_ABA_Qy.Text;
            }

            if (!string.IsNullOrEmpty(txt_Name_Qy.Text))
            {
                tmpName = txt_Name_Qy.Text;
            }

            if (cb_Type_Qy.SelectedIndex != 0)
            {
                tmpTypeName = cb_Type_Qy.SelectedValue.ToString();
            }

            if (cb_ERName_Qy.SelectedIndex != 0)
            {
                tmpERName = cb_ERName_Qy.SelectedValue.ToString();
            }

            if (cb_Date_Qy.SelectedIndex != 0)
            {
                tmpColumnDateName = cb_Date_Qy.SelectedValue.ToString();

                if (dp_StartDate_Qy.SelectedDate != null || dp_EndDate_Qy.SelectedDate != null)
                {
                    bool flag = Comm.checkTimes(dp_StartDate_Qy.SelectedDate.ToString(), dp_EndDate_Qy.SelectedDate.ToString());

                    if (flag == true)
                    {
                        tmpStrTime = Comm.strTime.ToString("yyyy-MM-dd HH:mm:ss");
                        tmpEndTime = Comm.endTime.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }
            }
            string tmpIsShowABA = "", tmpIsShowTypeName = "", tmpIsShowNormalName = "", tmpIsShowName = "",
                   tmpIsShowIDNumber = "", tmpIsShowCompany = "", tmpIsShowEmployeeNo = "", tmpIsShowJobTitle = "",
                   tmpIsShowTel = "", tmpIsShowMobile = "", tmpIsShowStartDate = "", tmpIsShowEndDate = "",
                   tmpIsShowTimestamp = "", tmpIsShowReturnDate = "", tmpIsShowMemo = "", tmpIsShowERName = "",
                   tmpIsShowDoor = "";

            foreach (var c in dataGridDetail.Columns)
            {
                string tmpHeader = "";
                tmpHeader = c.Header.ToString();
                if ("磁卡(ABA)" == tmpHeader)
                {
                    if (c.Visibility == Visibility.Visible)
                    {
                        tmpIsShowABA = "1";
                    }
                    else
                    {
                        tmpIsShowABA = "0";
                    }
                }
                else if ("磁卡類型" == tmpHeader)
                {
                    if (c.Visibility == Visibility.Visible)
                    {
                        tmpIsShowTypeName = "1";
                    }
                    else
                    {
                        tmpIsShowTypeName = "0";
                    }
                }
                else if ("磁卡群組名稱" == tmpHeader)
                {
                    if (c.Visibility == Visibility.Visible)
                    {
                        tmpIsShowNormalName = "1";
                    }
                    else
                    {
                        tmpIsShowNormalName = "0";
                    }
                }
                else if ("姓名" == tmpHeader)
                {
                    if (c.Visibility == Visibility.Visible)
                    {
                        tmpIsShowName = "1";
                    }
                    else
                    {
                        tmpIsShowName = "0";
                    }
                }
                else if ("身份證號碼" == tmpHeader)
                {
                    if (c.Visibility == Visibility.Visible)
                    {
                        tmpIsShowIDNumber = "1";
                    }
                    else
                    {
                        tmpIsShowIDNumber = "0";
                    }
                }
                else if ("公司" == tmpHeader)
                {
                    if (c.Visibility == Visibility.Visible)
                    {
                        tmpIsShowCompany = "1";
                    }
                    else
                    {
                        tmpIsShowCompany = "0";
                    }
                }
                else if ("職工編號" == tmpHeader)
                {
                    if (c.Visibility == Visibility.Visible)
                    {
                        tmpIsShowEmployeeNo = "1";
                    }
                    else
                    {
                        tmpIsShowEmployeeNo = "0";
                    }
                }
                else if ("職位" == tmpHeader)
                {
                    if (c.Visibility == Visibility.Visible)
                    {
                        tmpIsShowJobTitle = "1";
                    }
                    else
                    {
                        tmpIsShowJobTitle = "0";
                    }
                }
                else if ("電話" == tmpHeader)
                {
                    if (c.Visibility == Visibility.Visible)
                    {
                        tmpIsShowTel = "1";
                    }
                    else
                    {
                        tmpIsShowTel = "0";
                    }
                }
                else if ("手機" == tmpHeader)
                {
                    if (c.Visibility == Visibility.Visible)
                    {
                        tmpIsShowMobile = "1";
                    }
                    else
                    {
                        tmpIsShowMobile = "0";
                    }
                }
                else if ("起始日期" == tmpHeader)
                {
                    if (c.Visibility == Visibility.Visible)
                    {
                        tmpIsShowStartDate = "1";
                    }
                    else
                    {
                        tmpIsShowStartDate = "0";
                    }
                }
                else if ("到期日期" == tmpHeader)
                {
                    if (c.Visibility == Visibility.Visible)
                    {
                        tmpIsShowEndDate = "1";
                    }
                    else
                    {
                        tmpIsShowEndDate = "0";
                    }
                }
                else if ("發卡日期" == tmpHeader)
                {
                    if (c.Visibility == Visibility.Visible)
                    {
                        tmpIsShowTimestamp = "1";
                    }
                    else
                    {
                        tmpIsShowTimestamp = "0";
                    }
                }
                else if ("實際已歸還日期" == tmpHeader)
                {
                    if (c.Visibility == Visibility.Visible)
                    {
                        tmpIsShowReturnDate = "1";
                    }
                    else
                    {
                        tmpIsShowReturnDate = "0";
                    }
                }
                else if ("備註" == tmpHeader)
                {
                    if (c.Visibility == Visibility.Visible)
                    {
                        tmpIsShowMemo = "1";
                    }
                    else
                    {
                        tmpIsShowMemo = "0";
                    }
                }
                else if ("機房名稱" == tmpHeader)
                {
                    if (c.Visibility == Visibility.Visible)
                    {
                        tmpIsShowERName = "1";
                    }
                    else
                    {
                        tmpIsShowERName = "0";
                    }
                }
                else if ("大門名稱" == tmpHeader)
                {
                    if (c.Visibility == Visibility.Visible)
                    {
                        tmpIsShowDoor = "1";
                    }
                    else
                    {
                        tmpIsShowDoor = "0";
                    }
                }
            }

            //透過HttpUtility.UrlEncode(參數名)對中文參數進行編碼   
            //var filePath = "http://" + Application.Current.Host.Source.Host + ":" + Application.Current.Host.Source.Port + 
            var filePath = ReportViewer + //"http://localhost:30553/ReportViewer" +
                 "/rp_vwMagneticCardDetailPage.aspx?" +
                 "ABA=" + HttpUtility.UrlEncode(tmpABA) + 
                 "&Name=" + HttpUtility.UrlEncode(tmpName) + 
                 "&TypeName=" + HttpUtility.UrlEncode(tmpTypeName) +
                 "&ERName=" +  HttpUtility.UrlEncode(tmpERName) +
                 "&ColumnDateName=" + HttpUtility.UrlEncode(tmpColumnDateName) + 
                 "&StrTime=" + HttpUtility.UrlEncode(tmpStrTime) +
                 "&EndTime=" + HttpUtility.UrlEncode(tmpEndTime) +
                 "&IsShowABA=" + HttpUtility.UrlEncode(tmpIsShowABA) +
                 "&IsShowTypeName=" + HttpUtility.UrlEncode(tmpIsShowTypeName) +
                 "&IsShowNormalName=" + HttpUtility.UrlEncode(tmpIsShowNormalName) +
                 "&IsShowName=" + HttpUtility.UrlEncode(tmpIsShowName) +
                 "&IsShowIDNumber=" + HttpUtility.UrlEncode(tmpIsShowIDNumber) +
                 "&IsShowCompany=" + HttpUtility.UrlEncode(tmpIsShowCompany) +
                 "&IsShowEmployeeNo=" + HttpUtility.UrlEncode(tmpIsShowEmployeeNo) +
                 "&IsShowJobTitle=" + HttpUtility.UrlEncode(tmpIsShowJobTitle) +
                 "&IsShowTel=" + HttpUtility.UrlEncode(tmpIsShowTel) +
                 "&IsShowMobile=" + HttpUtility.UrlEncode(tmpIsShowMobile) +
                 "&IsShowStartDate=" + HttpUtility.UrlEncode(tmpIsShowStartDate) +
                 "&IsShowEndDate=" + HttpUtility.UrlEncode(tmpIsShowEndDate) +
                 "&IsShowTimestamp=" + HttpUtility.UrlEncode(tmpIsShowTimestamp) +
                 "&IsShowReturnDate=" + HttpUtility.UrlEncode(tmpIsShowReturnDate) +
                 "&IsShowMemo=" + HttpUtility.UrlEncode(tmpIsShowMemo) +
                 "&IsShowERName=" + HttpUtility.UrlEncode(tmpIsShowERName) +
                 "&IsShowDoor=" + HttpUtility.UrlEncode(tmpIsShowDoor); 


            //Uri myURI = new Uri(System.Windows.Browser.HtmlPage.Document.DocumentUri, filePath);
            //System.Windows.Browser.HtmlPage.Window.Navigate(myURI);
            
            //silverlight內建web browser
            //wb1.Navigate(new Uri(filePath));
            //if (App.Current.IsRunningOutOfBrowser)
            //{
                MyHyperlinkButton button = new MyHyperlinkButton();
                button.NavigateUri = new Uri(filePath);
                button.TargetName = "_blank";
                button.ClickMe();
            //}
            //else
            //{
            //    System.Windows.Browser.HtmlPage.Window.Navigate(new Uri(System.Windows.Browser.HtmlPage.Document.DocumentUri, filePath), "_blank");
            //} 
        }


        public void ItemValueChangedEvenr(slWCFModule.RemoteService.ItemBindingData ItemBindingData)
        {
            throw new NotImplementedException();
        }

        private async void bu_SendAgain_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("是否確定重傳磁卡?", "重傳", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                var q = await db.LoadAsync<tblCardCommandLog>(from b in db.GetTblCardCommandLogQuery() where b.IsSuccess == false select b);

                foreach (tblCardCommandLog st in q)
                {
                    st.Timestamp = null;
                }

                try
                {
                    bool res = await db.SubmitChangesAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("tblCardCommandLog-Data updation failed due to " + ex.Message);
                }

                //有改變，通知Server
                client.SecureService.NotifyDBChangeAsync(slWCFModule.RemoteService.DBChangedConstant.AuthorityChanged, "");
                client.SecureService.NotifyDBChangeCompleted += (s, a) =>
                {
                    if (a.Error != null)
                    {
                        MessageBox.Show(a.Error.Message);
                        return;
                    }
                };
                MessageBox.Show("重傳磁卡完成!");
            }
        }
    }
}

public class MyHyperlinkButton : HyperlinkButton
{
    public void ClickMe()
    {
        base.OnClick();
    }
} 


                               
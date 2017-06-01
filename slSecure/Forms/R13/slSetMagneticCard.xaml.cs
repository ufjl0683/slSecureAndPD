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

namespace slSecure.Forms.R13
{
    public partial class slSetMagneticCard : Page,slWCFModule.RemoteService.ISecureServiceCallback
    {
        slSecure.Web.SecureDBContext db = slSecure.DB.GetDB();
        List<TreeViewModel> TreeList;
        string actType;
        MyClient client;
        public slSetMagneticCard()
        {
            InitializeComponent();
        }

        // 使用者巡覽至這個頁面時執行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string key;
            client = new MyClient(new System.ServiceModel.InstanceContext(this), "CustomBinding_ISecureService");
            client.ToServerHelloAsync();
            client.RegisterCompleted += (s, a) =>
                {
                    if (a.Error != null)
                    {
                        MessageBox.Show(a.Error.Message);
                        return;
                    }

                    key = a.Result;
                    
                };
            client.RegisterAsync(Guid.NewGuid().ToString());
        }

        //記得在void與private之間加入async
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GetMagneticCardNormalGroup();
            NewMagneticCard();
            QueryMagneticCard();
        }
        //查詢磁卡資料  
        async void QueryMagneticCard()
        {
            //db = slSecure.DB.GetDB();
            var q = await db.LoadAsync<tblMagneticCard>(db.GetTblMagneticCardQuery());
            dataGrid.ItemsSource = q;
            NewMagneticCard();
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
            dp_StartDate.SelectedDate = null;
            dp_EndDate.SelectedDate = null;
            cb_Enable.SelectedIndex = 0;
            tb_Memo.Text = "";

            TreeList = await TreeViewModel.SetTree("各機房之讀卡機清單", true);
            tv_TreeView.ItemsSource = TreeList;
        }
        //取得定期卡設定下拉選單
        async void GetMagneticCardNormalGroup()
        {
            //db = slSecure.DB.GetDB();
            var q = await db.LoadAsync<tblMagneticCardNormalGroup>
                (from b in db.GetTblMagneticCardNormalGroupQuery()
                 select b);

            cb_NormalName.ItemsSource = q;
            List<string> lNormalName = new List<string>();
            lNormalName.Add("請選擇");
            foreach (var MCData in q)
            {
                lNormalName.Add(MCData.NormalName);
            }
            cb_NormalName.ItemsSource = lNormalName;
        }
        //新增磁卡資料
        async Task AddMagneticCardAsync()
        {
            //TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool> ();

            //db = slSecure.DB.GetDB();
            //1.新增磁卡權限群組資料
            //var q2 = await db.LoadAsync<tblSysRole>(from b in db.GetTblSysRoleQuery() select b);
            //tblSysRole bc2 = q2.Last();


            string tempABA = "";
            tempABA = (Convert.ToUInt32(txt_ABA.Text)).ToString("0000000000");

            tblSysRole role = new tblSysRole()
            {
                // RoleID = bc2.RoleID + 1,
                RoleName = txt_ABA.Text,
                UpdateDate = DateTime.Now
            };

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
                StartDate = dp_StartDate.SelectedDate,
                EndDate = (DateTime)dp_EndDate.SelectedDate,
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

                //MessageBox.Show("Role-Data added Successfully!");
            }
            catch (Exception ex)
            {
                //   taskCompletionSource.TrySetException(ex);
                //    taskCompletionSource.SetResult(false);
                MessageBox.Show("Role-Adding Data failed due to " + ex.Message);
            }
            //2.加入磁卡權限清單
            await AddGroup(role.RoleID);

            //3.新增磁卡資料
            //var q = await db.LoadAsync<tblMagneticCard>(from b in db.GetTblMagneticCardQuery() select b);
            //tblMagneticCard bc = q.Last();

            //string tempABA = "";
            //tempABA = (Convert.ToInt32(txt_ABA.Text)).ToString("0000000000");

            //db.tblMagneticCards.Add(

            //   new tblMagneticCard()
            //   {
            //       MagneticID = bc.MagneticID + 1,
            //       ABA = tempABA,//txt_ABA.Text,
            //       //WEG1 = GetWEG(tempABA).Substring(0, 5),
            //       //WEG2 = GetWEG(tempABA).Substring(5, 5),
            //       Name = txt_Name.Text,
            //       Type = (short)cb_Type.SelectedIndex,
            //       NormalID = cb_NormalName.SelectedIndex,
            //       IDNumber = txt_IDNumber.Text,
            //       Company = txt_Company.Text,
            //       EmployeeNo = txt_EmployeeNo.Text,
            //       JobTitle = txt_JobTitle.Text,
            //       Tel = txt_Tel.Text,
            //       Mobile = txt_Mobile.Text,
            //       Timestamp = (DateTime)dp_Timestamp.SelectedDate,
            //       ReturnDate = dp_ReturnDate.SelectedDate,
            //       StartDate = dp_StartDate.SelectedDate,
            //       EndDate = (DateTime)dp_EndDate.SelectedDate,
            //       Enable = cb_Enable.SelectedValue.ToString(),
            //       Memo = tb_Memo.Text,
            //       RoleID = bc2.RoleID + 1
            //   }
            //   );
            //try
            //{
            //    bool res = await db.SubmitChangesAsync(); 
            //    //bool res = await db.SubmitChangesAsync();
            //    MessageBox.Show("MagneticCard-Data added Successfully!");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("MagneticCard-Adding Data failed due to " + ex.Message);
            //}

            // taskCompletionSource.SetResult(true);
            //  return taskCompletionSource.Task;

            MessageBox.Show("新增磁卡成功!");
        }
        //修改磁卡資料
        async Task ModifyMagneticCardAsync()
        {
            //db = slSecure.DB.GetDB();
            //1.加入磁卡權限清單
            await AddGroup(int.Parse(txt_RoleID.Text));

            var magneticID = int.Parse(txt_MagneticID.Text);
            //2.修改磁卡資料
            var q = await db.LoadAsync<tblMagneticCard>(from b in db.GetTblMagneticCardQuery() where b.MagneticID == magneticID select b);
            tblMagneticCard bc = q.First();

            string tempABA = "";
            tempABA = (Convert.ToUInt32(txt_ABA.Text)).ToString("0000000000");

            bc.ABA = txt_ABA.Text;//tempABA;
            //bc.WEG1 = GetWEG(tempABA).Substring(0, 5);
            //bc.WEG2 = GetWEG(tempABA).Substring(5, 5);
            bc.Name = txt_Name.Text;
            bc.Type = (short)cb_Type.SelectedIndex;
            bc.NormalID = cb_NormalName.SelectedIndex;
            bc.IDNumber = txt_IDNumber.Text;
            bc.Company = txt_Company.Text;
            bc.EmployeeNo = txt_EmployeeNo.Text;
            bc.JobTitle = txt_JobTitle.Text;
            bc.Tel = txt_Tel.Text;
            bc.Mobile = txt_Mobile.Text;
            bc.Timestamp = (DateTime)dp_Timestamp.SelectedDate;
            bc.ReturnDate = dp_ReturnDate.SelectedDate;
            bc.StartDate = dp_StartDate.SelectedDate;
            bc.EndDate = (DateTime)dp_EndDate.SelectedDate;
            bc.Enable = cb_Enable.SelectedValue.ToString();
            bc.Memo = tb_Memo.Text;
            try
            {
                bool res = await db.SubmitChangesAsync();
                //MessageBox.Show("MagneticCard-Data updated successfully!");
                MessageBox.Show("修改磁卡成功!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("MagneticCard-Data updation failed due to " + ex.Message);
            }
        }
        //刪除磁卡資料
        async Task DeleteMagneticCardAsync()
        {
            //db = slSecure.DB.GetDB();
            var magneticID = int.Parse(txt_MagneticID.Text);
            //非同步模擬成同步
            var q = await db.LoadAsync<tblMagneticCard>(from b in db.GetTblMagneticCardQuery() where b.MagneticID == magneticID select b);
            tblMagneticCard bc = q.First();

            db.tblMagneticCards.Remove(bc);
            try
            {
                bool res = await db.SubmitChangesAsync();
                //MessageBox.Show("MagneticCard-Data deleted successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("MagneticCard-Data deletion failed due to " + ex.Message);
            }
        }
        //刪除磁卡權限資料
        async Task DeleteRoleAuthorityAsync()
        {
            //db = slSecure.DB.GetDB();
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
                        client.NotifyDBChangeAsync(slWCFModule.RemoteService.DBChangedConstant.AuthorityChanged);
                        client.NotifyDBChangeCompleted += (s, a) =>
                        {
                            if (a.Error != null)
                            {
                                MessageBox.Show(a.Error.Message);
                                return;
                            }

                            //key = a.Result;

                        };
                        //MessageBox.Show("RoleAuthority-Data deleted successfully!");
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
            //db = slSecure.DB.GetDB();
            var roleID = int.Parse(txt_RoleID.Text);
            //非同步模擬成同步
            var q = await db.LoadAsync<tblSysRole>(from b in db.GetTblSysRoleQuery() where b.RoleID == roleID select b);
            tblSysRole bc = q.First();

            db.tblSysRoles.Remove(bc);
            try
            {
                bool res = await db.SubmitChangesAsync();
                //MessageBox.Show("Role-Data deleted successfully!");
                MessageBox.Show("刪除磁卡成功!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Role--Data deletion failed due to " + ex.Message);
            }
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
                //MessageBox.Show("通過驗證!!");
                string sABA = "", sWEG1 = "", sWEG2 = "";
                //db = slSecure.DB.GetDB();

                if (txt_ABA.Text != "")
                {
                    int iABA = Convert.ToInt32(txt_ABA.Text);
                    sABA = iABA.ToString("0000000000");
                    string sWEG12 = GetWEG(sABA);

                    sWEG1 = sWEG12.Substring(0, 5);
                    sWEG2 = sWEG12.Substring(5, 5);

                    var q = await db.LoadAsync<tblMagneticCard>(from b in db.GetTblMagneticCardQuery() where b.ABA == sABA select b);
                    if (q.Count() > 0)
                    {
                        txt_ABA.SetValidation("磁卡(ABA編碼)已存在,不得重覆!");
                        txt_ABA.RaiseValidationError();
                        //MessageBox.Show("磁卡(ABA編碼)已存在,不得重覆!");
                    }
                }
            }


        }
        //由磁卡ABA去推算出WEG1及WEG2(R23)
        public static string GetWEG(string ABA)
        {
            int aba = Convert.ToInt32(ABA);
            int weg1 = aba / 65536;
            int weg2 = aba % 65536;
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

                    //db = slSecure.DB.GetDB();
                    //1.先刪除全部與RoleID相關的資料
                    var q = await db.LoadAsync<tblSysRoleAuthority>(from b in db.GetTblSysRoleAuthorityQuery() where b.RoleID == RoleID select b);
                    if (objList.Count() == 0)
                    {

                        foreach (tblSysRoleAuthority st in q)
                        {
                            db.tblSysRoleAuthorities.Remove(st);

                            try
                            {
                                //bool res = await db.SubmitChangesAsync();
                                bool res = await db.SubmitChangesAsync();


                                
                                //MessageBox.Show("RoleAuthority-Data deleted successfully!");
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("RoleAuthority-Data deletion failed due to " + ex.Message);
                            }
                        }
                    }

                    else
                    {
                        // find is exist
                        // var qq = (from n in q where !objList.Contains(n.ControlID) select n);
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
                        }

                        foreach (tblSysRoleAuthority tbl in q)
                        {
                            if (objList.Where(n => n == tbl.ControlID).FirstOrDefault() == null)
                            {
                                db.tblSysRoleAuthorities.Remove(tbl);
                            }
                        }

                    }

                    ////2.再取得treeview資料並新增
                    //for (int i = 0; i < objList.Count; i++)
                    //{
                    //    var q2 = await db.LoadAsync<tblSysRoleAuthority>(from b in db.GetTblSysRoleAuthorityQuery() select b);

                    //    db.tblSysRoleAuthorities.Add(

                    //       new tblSysRoleAuthority()
                    //       {
                    //           RoleID = RoleID,
                    //           ControlID = objList[i]
                    //       }
                    //       );
                    try
                    {
                        bool res = await db.SubmitChangesAsync();
                        client.NotifyDBChangeAsync(slWCFModule.RemoteService.DBChangedConstant.AuthorityChanged);
                        client.NotifyDBChangeCompleted += (s, a) =>
                        {
                            if (a.Error != null)
                            {
                                MessageBox.Show(a.Error.Message);
                                return;
                            }

                            //key = a.Result;

                        };
                        //bool res = await db.SubmitChangesAsync();
                        //MessageBox.Show("RoleAuthority-Data added Successfully!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("RoleAuthority-Adding Data failed due to " + ex.Message);
                    }

                    //}

                }
            }
            else
            {
                MessageBox.Show("請先選擇群組清單!");
            }
        }

        private void bu_New_Click(object sender, RoutedEventArgs e)
        {
            //NewMagneticCard();

            QueryMagneticCard();
        }

        private async void bu_Add_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("是否確定儲存磁卡資料?", "儲存", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                if (actType == "New")
                {
                    if ((txt_ABA.Text == "") || (dp_Timestamp.SelectedDate.ToString() == "") || (dp_StartDate.SelectedDate.ToString() == "") || (dp_EndDate.SelectedDate.ToString() == ""))
                    {
                        MessageBox.Show("磁卡(ABA編碼),發卡日期/啟始日期/到期日期，不得為空值");
                    }
                    else
                    {
                        await AddMagneticCardAsync();
                        QueryMagneticCard();
                    }
                }
                else if (actType == "Update")
                {
                    if ((dp_Timestamp.SelectedDate.ToString() == "") || (dp_StartDate.SelectedDate.ToString() == "") || (dp_EndDate.SelectedDate.ToString() == ""))
                    {
                        MessageBox.Show("發卡日期/啟始日期/到期日期，不得為空值");
                    }
                    else
                    {
                        await ModifyMagneticCardAsync();
                        QueryMagneticCard();
                    }
                }
            }
        }

        private async void bu_Del_Click(object sender, RoutedEventArgs e)
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

                QueryMagneticCard();
            }
        }

        private void bu_SetNormalName_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new slSetNormalGroup();
        }

        private async void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //透過txt_RoleID.Text來取TreeView資料
            if (dataGrid.CurrentColumn != null && txt_RoleID.Text != "")
            {
                actType = "Update";
                txt_ABA.IsEnabled = false;

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
        }

        private void bu_Query_Click(object sender, RoutedEventArgs e)
        {
            //NewMagneticCard();
            QueryMagneticCard();
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

        private void txt_ABA_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (actType == "New")
            //{
            //    CheckMagneticCard();
            //}
        }



        public void SayHello(string hello)
        {
           // throw new NotImplementedException();
        }

        private void bu_OpenDoor_Click(object sender, RoutedEventArgs e)
        {
            List<string> objList = new List<string>();
            if (TreeList != null && TreeList.Count > 0)
            {
                objList = TreeViewModel.GetTree(TreeList.First());

                foreach (string ControlID in objList)
                {
                    client.ForceOpenDoorAsync(ControlID);
                    client.ForceOpenDoorCompleted += (s, a) =>
                    {
                        if (a.Error != null)
                        {
                            MessageBox.Show(a.Error.Message);
                            return;
                        }

                        //key = a.Result;

                    };
                    //client.NotifyDBChangeAsync(slWCFModule.RemoteService.DBChangedConstant.AuthorityChanged);
                    //client.NotifyDBChangeCompleted += (s, a) =>
                    //{
                    //    if (a.Error != null)
                    //    {
                    //        MessageBox.Show(a.Error.Message);
                    //        return;
                    //    }

                    //    //key = a.Result;

                    //};
                }
            }

        }
    }
    public class TempClass
    {
        public string ConrollID { get; set; }
    }
}

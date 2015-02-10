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
using System.Collections.ObjectModel;

namespace slSecureLib.Forms.R23
{
    public partial class slAddSysRoleAuthority : Page
    {
        slSecure.Web.SecureDBContext db = slSecure.DB.GetDB();
        List<TreeViewModel> TreeList;
        PagedCollectionView pageView;
        List<CheckBox> checkboxes = new List<CheckBox>();
        ObservableCollection<MagneticCard> magneticCardData = null;


        public slAddSysRoleAuthority()
        {
            InitializeComponent();
            //dp_DG.PageSize = 20;

            QueryMagneticCard();
            QueryEngineRoomList();
        }

        // 使用者巡覽至這個頁面時執行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void bu_Back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        //查詢磁卡資料  
        async void QueryMagneticCard()
        {
            //busyIndicator.IsBusy = true;
            ObservableCollection<MagneticCard> objMagneticCard = new ObservableCollection<MagneticCard>();

            var q = await db.LoadAsync<vwMagneticCard>(db.GetVwMagneticCardQuery());

            foreach (var vwMagneticCardData in q)
            {
                objMagneticCard.Add(new MagneticCard
                {
                    RoleID = vwMagneticCardData.RoleID,
                    ABA = vwMagneticCardData.ABA,
                    Name = vwMagneticCardData.Name,
                    Company = vwMagneticCardData.Company,
                    Memo = vwMagneticCardData.Memo
                });
            }
            //分頁，但會選取DataGrid第一筆
            pageView = new PagedCollectionView(q);

            dataGrid.ItemsSource = pageView;
            magneticCardData = objMagneticCard;
            //busyIndicator.IsBusy = false;
        }

        //查詢機房清單資料  
        async void QueryEngineRoomList()
        {
            TreeList = await TreeViewModel.SetTree("各機房之讀卡機清單", true);
            tv_TreeView.ItemsSource = TreeList;
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            bool check = chk.IsChecked.Value;
            foreach (CheckBox chkbox in checkboxes)
                chkbox.IsChecked = check;
        }

        private async void bu_Add_Click(object sender, RoutedEventArgs e)
        {
            List<string> objList = new List<string>();
            List<int> MagneticCardList = new List<int>();

            for (int i = 0; i < checkboxes.Count; i++)
            {
                if (checkboxes[i].IsChecked.Value)
                {
                    MagneticCardList.Add(magneticCardData[i].RoleID);
                }
            }
            var result = MessageBox.Show("是否確定新增機房與磁卡權限資料?", "新增", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                var q = await db.LoadAsync<tblSysRoleAuthority>(from b in db.GetTblSysRoleAuthorityQuery() select b);

                if (TreeList != null && TreeList.Count > 0)
                {
                    objList = TreeViewModel.GetTree(TreeList.First());

                    //1.檢查tblSysRoleAuthority是否已存在RoleID和ConrtolID
                    //foreach (tblSysRoleAuthority st in q)
                    //{
                    for (int i = 0; i < objList.Count; i++)
                    {
                        if (objList[i].ToString() != "")
                        {
                            for (int j = 0; j < MagneticCardList.Count; j++)
                            {

                                //if ((st.RoleID != MagneticCardList[j]) && st.ControlID != objList[i].ToString() && objList[i].ToString() !="")
                                //{

                                //MessageBox.Show(objList[i].ToString() + ":" + MagneticCardList[j].ToString());
                                //var qq = await db.LoadAsync<tblSysRoleAuthority>(from b in db.GetTblSysRoleAuthorityQuery() where b.RoleID == MagneticCardList[j] && b.ControlID == objList[i].ToString() select b);
                                q = q.Where(a => a.RoleID == MagneticCardList[j] && a.ControlID == objList[i].ToString());
                                if (q.Count() == 0)
                                {
                                    //MessageBox.Show(objList[i].ToString() + ":" + MagneticCardList[j].ToString());
                                    db.tblSysRoleAuthorities.Add(
                                          new tblSysRoleAuthority()
                                          {
                                              RoleID = MagneticCardList[j],
                                              ControlID = objList[i].ToString()
                                          }
                                        );
                                }

                                //}
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
                    //}

                    ////有改變，通知Server
                    //client.SecureService.NotifyDBChangeAsync(slWCFModule.RemoteService.DBChangedConstant.AuthorityChanged, "");
                    //client.SecureService.NotifyDBChangeCompleted += (s, a) =>
                    //{
                    //    if (a.Error != null)
                    //    {
                    //        MessageBox.Show(a.Error.Message);
                    //        return;
                    //    }

                    //};
                    MessageBox.Show("新增機房與磁卡權限資料成功!");
                }
            }
        }

        private async void bu_Del_Click(object sender, RoutedEventArgs e)
        {
            List<string> objList = new List<string>();
            List<int> MagneticCardList = new List<int>();

            for (int i = 0; i < checkboxes.Count; i++)
            {
                if (checkboxes[i].IsChecked.Value)
                {
                    MagneticCardList.Add(magneticCardData[i].RoleID);
                }
            }
            var result = MessageBox.Show("是否確定刪除機房與磁卡權限資料?", "刪除", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                var q = await db.LoadAsync<tblSysRoleAuthority>(from b in db.GetTblSysRoleAuthorityQuery() select b);

                if (TreeList != null && TreeList.Count > 0)
                {
                    objList = TreeViewModel.GetTree(TreeList.First());

                    //1.檢查tblSysRoleAuthority是否已存在RoleID和ConrtolID
                    //foreach (tblSysRoleAuthority st in q)
                    //{
                    for (int i = 0; i < objList.Count; i++)
                    {
                        if (objList[i].ToString() != "")
                        {
                            for (int j = 0; j < MagneticCardList.Count; j++)
                            {
                                //if ((st.RoleID == MagneticCardList[j]) && st.ControlID == objList[i].ToString() && objList[i].ToString() != "")
                                //{
                                //MessageBox.Show(objList[i].ToString() + ":" + MagneticCardList[j].ToString());
                                //非同步模擬成同步
                                //var qq = await db.LoadAsync<tblSysRoleAuthority>(from b in db.GetTblSysRoleAuthorityQuery() where b.RoleID == MagneticCardList[j] && b.ControlID == objList[i].ToString() select b);
                                q = q.Where(a => a.RoleID == MagneticCardList[j] && a.ControlID == objList[i].ToString());
                                if (q.Count() > 0)
                                {
                                    foreach (tblSysRoleAuthority stq in q)
                                    {
                                        db.tblSysRoleAuthorities.Remove(stq);
                                    }

                                }
                            }
                        }
                    }
                    try
                    {
                        bool res = await db.SubmitChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("RoleAuthority-Data deletion failed due to " + ex.Message);
                    }
                    //}
                }

                ////有改變，通知Server
                //client.SecureService.NotifyDBChangeAsync(slWCFModule.RemoteService.DBChangedConstant.AuthorityChanged, "");
                //client.SecureService.NotifyDBChangeCompleted += (s, a) =>
                //{
                //    if (a.Error != null)
                //    {
                //        MessageBox.Show(a.Error.Message);
                //        return;
                //    }

                //};
                MessageBox.Show("刪除機房與磁卡權限資料成功!");
            }
        }

        private void dataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            CheckBox chk = dataGrid.Columns[0].GetCellContent(e.Row) as CheckBox;
            chk.IsChecked = false;
            checkboxes.Add(chk);
        }
    }
    public class MagneticCard
    {
        public int RoleID { get; set; }
        public string ABA { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Memo { get; set; }
    }
}

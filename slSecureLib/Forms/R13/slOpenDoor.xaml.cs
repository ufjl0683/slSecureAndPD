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
using slWCFModule;
using System.Windows.Data;
using System.Collections.ObjectModel;

namespace slSecureLib.Forms.R13
{
    public partial class slOpenDoor : Page, slWCFModule.RemoteService.ISecureServiceCallback
    {
        slSecure.Web.SecureDBContext db = slSecure.DB.GetDB();
        List<TreeViewModel> TreeList;
        slWCFModule.MyClient client;
        System.Windows.Threading.DispatcherTimer myDispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        PagedCollectionView pageView;
        

        public slOpenDoor()
        {
            InitializeComponent();

            GetTreeView();
            QueryEngineRoomLogData();
            
            treeView_New();
        }

        async void treeView_New()
        {
            slSecure.Web.SecureDBContext db;
            db = slSecure.DB.GetDB();
            TreeViewItem tvItem;

            var ERNameData = await db.LoadAsync<tblEngineRoomConfig>(db.GetTblEngineRoomConfigQuery());
            string sERName, sReadCardName, sControlID;
            foreach (var tempERNameData in ERNameData)
            {
                sERName = tempERNameData.ERName;
                tvItem = new TreeViewItem();
                tvItem.Header = sERName;

                foreach (var tempEntranceGuardData in tempERNameData.tblEntranceGuardConfig)
                {
                    foreach (var tempControllerConfigData in tempERNameData.tblControllerConfig)
                    {
                        if (tempControllerConfigData.EntranceCode == tempEntranceGuardData.EntranceCode && (tempControllerConfigData.ControlType == 1 || tempControllerConfigData.ControlType == 2))
                        {
                            sControlID = tempControllerConfigData.ControlID;
                            sReadCardName = tempEntranceGuardData.Memo;

                            RadioButton ck1 = new RadioButton() { Content = sReadCardName, Tag = sControlID, GroupName = "Door" };
                            tvItem.Items.Add(ck1);
                            tvItem.Tag = sControlID;
                        }
                    }
                }
                tv_TreeView_New.Items.Add(tvItem);
            }
        }

        async void GetTreeView()
        {
            TreeList = await TreeViewModel.SetTree("各機房之讀卡機清單(一次只能選取一個門)", true);
            tv_TreeView.ItemsSource = TreeList;
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

        //查詢強制開門歷史資料  
        async void QueryEngineRoomLogData()
        {
            var q = await db.LoadAsync<vwEngineRoomLog>(from b in db.GetVwEngineRoomLogQuery() where b.TypeID == 8 && b.TypeCode == 17 select b);
            //dataGrid.ItemsSource = q;

            //分頁，但會選取DataGrid第一筆
            pageView = new PagedCollectionView(q.OrderByDescending(qq => qq.StartTime));
            dataGrid.ItemsSource = pageView;

            //PagedCollectionView view = new PagedCollectionView(pageView);
            ////依屬性名稱再分组
            //view.GroupDescriptions.Add(new PropertyGroupDescription("NormalID"));

        }

        private void bu_OpenDoor_Click(object sender, RoutedEventArgs e)
        {
            List<string> objList = new List<string>();
            //MessageBox.Show(tv_TreeView_New.SelectedValue.ToString());
            
            if (TreeList != null && TreeList.Count > 0)
            {

                objList = TreeViewModel.GetTree(TreeList.First());
                if (objList.Count > 1)
                {
                    MessageBox.Show("只能選取一個門!");
                }
                else if (objList.Count <= 0)
                {
                    MessageBox.Show("尚未選取任何一個門!");
                }
                else
                {
                    var result = MessageBox.Show("是否確定遠端開門?", "開門", MessageBoxButton.OKCancel);
                    if (result == MessageBoxResult.OK)
                    {
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
                                MessageBox.Show("遠端開門成功!");
                            };
                        }
                        
                        QueryEngineRoomLogData();
                    }
                }
            }
        }

        private void bu_Return_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            client.Dispose();
        }

        private void bu_Query_Click(object sender, RoutedEventArgs e)
        {
            QueryEngineRoomLogData();
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


        public void ItemValueChangedEvenr(slWCFModule.RemoteService.ItemBindingData ItemBindingData)
        {
            //throw new NotImplementedException();
        }
    }
}

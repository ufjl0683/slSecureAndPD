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
using System.Runtime.InteropServices.Automation;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.IO;

namespace slSecureLib.Forms.R13
{
    public partial class slRetrieveRTU : Page
    {
        slSecure.Web.SecureDBContext db = slSecure.DB.GetDB();
        PagedCollectionView pageView;
        string VideoRecord = "";

        public slRetrieveRTU()
        {
            InitializeComponent();

            GetEngineRoomConfig();
            QueryVwEngineRoomLog();

            QueryVideoRecord();
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

        async void QueryVideoRecord()
        {
            var q = await db.LoadAsync<tblSysParameter>(from b in db.GetTblSysParameterQuery() where b.VariableName == "VideoRecord" select b);
            
            tblSysParameter bc = q.First();
            VideoRecord = bc.VariableValue;
        }

        async void QueryVwEngineRoomLog()
        {
            var q = await db.LoadAsync<vwEngineRoomLog>(db.GetVwEngineRoomLogQuery());
            //dataGrid_EngineRoomLog.ItemsSource = q;

            var a = q.OrderByDescending(qq => qq.FlowID).Where
                (qq => (qq.TypeID == 8 && qq.TypeCode == 1) ||
                       (qq.TypeID == 8 && qq.TypeCode == 7) ||
                       (qq.TypeID == 8 && qq.TypeCode == 8) ||
                       (qq.TypeID == 8 && qq.TypeCode == 17));

            //分頁，但會選取DataGrid第一筆
            pageView = new PagedCollectionView(a);
            dataGrid.ItemsSource = pageView;
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

            cb_ERName.ItemsSource = lEngineRoomList;

            cb_ERName.SelectedIndex = 0;
        }

        private async void bu_Query_Click(object sender, RoutedEventArgs e)
        {

            if ((dp_StartDate.SelectedDate != null || dp_EndDate.SelectedDate != null))
            {
                bool flag = Comm.checkTimes(dp_StartDate.SelectedDate.ToString(), dp_EndDate.SelectedDate.ToString());

                if (flag == true)
                {

                    var q = await db.LoadAsync<vwEngineRoomLog>(db.GetVwEngineRoomLogQuery());
                    //dataGrid_EngineRoomLog.ItemsSource = q;

                    var a = q.OrderByDescending(qq => qq.FlowID).Where
                        (qq => (qq.TypeID == 8 && qq.TypeCode == 1) ||
                               (qq.TypeID == 8 && qq.TypeCode == 7) ||
                               (qq.TypeID == 8 && qq.TypeCode == 8) ||
                               (qq.TypeID == 8 && qq.TypeCode == 17));

                    var b = a.Where(qqq => (qqq.StartTime >= Comm.strTime) && (qqq.StartTime <= Comm.endTime));

                    //分頁，但會選取DataGrid第一筆
                    pageView = new PagedCollectionView(b);
                    dataGrid.ItemsSource = pageView;
                }
                else
                {
                    MessageBox.Show("起迄時間有誤，請重新輸入！");
                    return;
                }
            }
            else
            {
                QueryVwEngineRoomLog();
            }

        }

        private void bu_OpenNVRFile_Click(object sender, RoutedEventArgs e)
        {
            vwEngineRoomLog data = dataGrid.SelectedItem as vwEngineRoomLog;
            if (data.NVRFile != null)
            {
                NRVFile nvrFile = new NRVFile(data.ERName, data.Door, data.CardType, data.StartTime, data.NVRFile, VideoRecord);
                nvrFile.Show();
            }
            else
            {
                MessageBox.Show("無錄影事件播放檔！");
            }
        }
    }
}

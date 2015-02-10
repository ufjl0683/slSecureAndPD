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

namespace slSecureLib.Forms.R13
{
    public partial class slSetDoorPassword : Page
    {
        slSecure.Web.SecureDBContext db = slSecure.DB.GetDB();
        System.Windows.Threading.DispatcherTimer myDispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        PagedCollectionView pageView;

        public slSetDoorPassword()
        {
            InitializeComponent();

            //QuerySysParameter_DoorPasswordTimeCycle();
            QueryDoorPassword();
        }

        // 使用者巡覽至這個頁面時執行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
           
        }

        //查詢開門密碼資料 
        async void QueryDoorPassword()
        {
            var q = await db.LoadAsync<tblERDoorPassword>(from b in db.GetTblERDoorPasswordQuery() select b);
            //dataGrid.ItemsSource = q;

            //分頁，但會選取DataGrid第一筆
            pageView = new PagedCollectionView(q.OrderByDescending(qq => qq.Timestamp));
            dataGrid.ItemsSource = pageView;

            //PagedCollectionView view = new PagedCollectionView(pageView);
            ////依屬性名稱再分组
            //view.GroupDescriptions.Add(new PropertyGroupDescription("NormalID"));

        }


        private void bu_Return_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        private void bu_Query_Click(object sender, RoutedEventArgs e)
        {
            QueryDoorPassword();
        }

    }
}

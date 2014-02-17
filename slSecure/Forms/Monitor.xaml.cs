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
using ESRI.ArcGIS.Client.Geometry;
using Common.Info;
using slSecure.Controls;

namespace slSecure.Forms
{
    public partial class Monitor : Page
    
    {
        // XMax="13571592" XMin="13449261" YMax="2914860" YMin="2822872"
        ControlRoomInfo[] roomInfos = new ControlRoomInfo[]{
               new ControlRoomInfo(){ Name="機房一", Type="C", X=13517188.5934,Y=2884171.5108, AlarmStatus=1},
               new ControlRoomInfo(){ Name="機房二", Type="C", X=13520728.9173,Y=2884971.2735,AlarmStatus=2},
               new ControlRoomInfo(){ Name="機房三", Type="C", X=13527539.3889,Y=2885241.2222,AlarmStatus=0}

           };
        public Monitor()
        {
            InitializeComponent();
        }

        // 使用者巡覽至這個頁面時執行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
           
            foreach (ControlRoomInfo info in roomInfos)
            {
                mapctl.AddControllRoomPin(info);
              
            }
            lstMenu.ItemsSource = roomInfos;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void All_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void FilterItem_Load(object sender, RoutedEventArgs e)
        {
            (sender as Rectangle).Width = (cboFilter.Width.Equals(Double.NaN)) ? cboFilter.ActualWidth : cboFilter.Width;
        }

        private void ControlRoomMenu_Loaded(object sender, RoutedEventArgs e)
        {

        }
        WrapPanel wrap;
        private void wrap_Loaded(object sender, RoutedEventArgs e)
        {
            wrap = sender as WrapPanel;
            lstMenu_SizeChanged(null, null);
        }

        private void lstMenu_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (wrap == null)
                return;
            this.wrap.Width = lstMenu.Width.Equals(double.NaN) ? lstMenu.ActualWidth : lstMenu.Width;
            this.wrap.Height = lstMenu.Height.Equals(double.NaN) ? lstMenu.ActualHeight : lstMenu.Height;
        }

        private void ControlRoomMenu_MouseEnter(object sender, MouseEventArgs e)
        {
            ControlRoomInfo info = (sender as ControlRoomMenu).DataContext as ControlRoomInfo;
            this.mapctl.ZoomToLevel(15, new MapPoint(info.X, info.Y));
        }

        private void cboFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstMenu == null)
                return;
            Rectangle r = (sender as ComboBox).SelectedItem as Rectangle;
            if (r.Tag.ToString() == "ALL")
                lstMenu.ItemsSource = roomInfos;
            else if (r.Tag.ToString() == "WARNING")
                lstMenu.ItemsSource = roomInfos.Where(n => n.AlarmStatus == 1);
            else if(r.Tag.ToString()=="ALARM")
                lstMenu.ItemsSource = roomInfos.Where(n => n.AlarmStatus == 2);
            else if (r.Tag.ToString() == "NORMAL")
                lstMenu.ItemsSource = roomInfos.Where(n => n.AlarmStatus == 0);

        }

        private void ControlRoomMenu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.lstMenu.ItemsSource = null;
            this.NavigationService.Navigate(new Uri("/Forms/ControlRoom.xaml",UriKind.Relative));
          //  Common.Util.GetICommon().Navigate(new Uri("/slSecure;component/Forms/ControlRoom.xaml",UriKind.Relative));
        }

    }
}

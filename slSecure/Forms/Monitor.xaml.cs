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
using System.ServiceModel.DomainServices.Client;
using slSecure.Info;
using slSecure.Controls;
using slSecure.Web;
using slWCFModule;
using slWCFModule.RemoteService;
 

namespace slSecure.Forms
{
    public partial class Monitor : Page
    
    {
        // XMax="13571592" XMin="13449261" YMax="2914860" YMin="2822872"
        //ControlRoomInfo[] roomInfos = new ControlRoomInfo[]{
        //       new ControlRoomInfo(){ Name="機房一", Type="C", X=13517188.5934,Y=2884171.5108, AlarmStatus=1},
        //       new ControlRoomInfo(){ Name="機房二", Type="C", X=13520728.9173,Y=2884971.2735,AlarmStatus=2},
        //       new ControlRoomInfo(){ Name="機房三", Type="C", X=13527539.3889,Y=2885241.2222,AlarmStatus=0}

        //   };
        ControlRoomInfo[] roomInfos;
       /// PlaneDegreeInfo[] planeInfos;
        slSecure.Web.SecureDBContext db;
      // ObservableCollection
        System.Collections.ObjectModel.ObservableCollection<PlaneDegreeInfo> PlaneDegreeInfos;
        MyClient client;
        System.Windows.Threading.DispatcherTimer tmr = new System.Windows.Threading.DispatcherTimer();
        public Monitor()
        {
            InitializeComponent();
        }

        // 使用者巡覽至這個頁面時執行。
        protected  async override void OnNavigatedTo(NavigationEventArgs e)
        {

            db = new slSecure.Web.SecureDBContext();
            var q = from n in db.GetTblEngineRoomConfigQuery() select n;
                   
           
                     

            var res=await  db.LoadAsync<slSecure.Web.tblEngineRoomConfig>(q );
             roomInfos = (from n in res
                            select new ControlRoomInfo()
                            {
                                 ERID=n.ERID,
                                Type = "C",
                                AlarmStatus = 0,
                                Name = n.ERName,
                                X = n.GPSX,
                                Y = n.GPSY
                            }).ToArray();

            foreach (ControlRoomInfo info in roomInfos)
            {
                mapctl.AddControllRoomPin(info);
              
            }

            //var q1 = from n in db.GetTblERPlaneQuery() select n;
            //var res1= await db.LoadAsync<tblERPlane>(q1);

            //planeInfos = (from n in res1
            //             select new PlaneInfo()
            //             {
            //                  ERID=n.ERID,
            //                   AlarmStatus=0,
            //                    Name=n.PlaneName,
            //                    PlaneID=n.PlaneID,
            //                     Type="EP"
                                  
            //             }).ToArray();

            
                client = new MyClient("CustomBinding_ISecureService", false);
          
            client.SecureService.GetAllPlaneInfoCompleted += (s, a) =>
                {
                    if (a.Error != null)
                        return;
                  lstMenu.ItemsSource =   PlaneDegreeInfos=a.Result;

                  if (roomInfos != null)
                      foreach (ControlRoomInfo info in roomInfos)
                      {
                          try
                          {
                              info.AlarmStatus = PlaneDegreeInfos.Where(n => n.ERID == info.ERID).Max(n => n.AlarmStatus);

                          }
                          catch { ;}
                      }

                };
            if (!IsExit)
            {
                client.SecureService.GetAllPlaneInfoAsync();


                tmr.Interval = TimeSpan.FromSeconds(10);
                tmr.Tick += tmr_Tick;

                tmr.Start();
            }
          //  client.OnItemValueChangedEvent += client_OnItemValueChangedEvent;
        }

       
        void tmr_Tick(object sender, EventArgs e)
        {
          
            client.SecureService.GetAllPlaneInfoCompleted += (ss, aa) =>
            {
                if (aa.Error != null)
                    return;
                foreach (PlaneDegreeInfo info in aa.Result)
                {
                    PlaneDegreeInfo data = PlaneDegreeInfos.Where(n => n.PlaneID == info.PlaneID).FirstOrDefault();
                    if (data == null)
                        return;
                    data.AlarmStatus = info.AlarmStatus;
                   
                }
                this.cboFilter_SelectionChanged(this.cboFilter, null);
              if(roomInfos!=null)
                foreach (ControlRoomInfo info in roomInfos)
                {
                    try
                    {
                       // info.AlarmStatus = PlaneDegreeInfos.Where(n => n.ERID == info.ERID).Max(n => n.AlarmStatus);
                        string colorString="Gray";
                        int alarmstatus = -1;
                        foreach (PlaneDegreeInfo pdi in PlaneDegreeInfos.Where(n=>n.ERID==info.ERID))
                        {
                            if (colorString == "")
                            {
                                colorString = pdi.ColorString;
                                alarmstatus = pdi.AlarmStatus;
                            }
                            if (pdi.ColorString == "Red")
                            {
                                colorString = "Red";
                                info.AlarmStatus = 2;
                            }
                            else if (pdi.ColorString == "Yellow" && colorString != "Red")
                            {
                                colorString = "Yellow";
                                info.AlarmStatus = 1;
                            }
                            else if (pdi.ColorString == "Green" && colorString != "Red" && colorString != "Yellow")
                            {
                                colorString = "Green";
                                info.AlarmStatus = 0;
                            }
                         
                        }

                        info.ColorString = colorString;
                    }
                    catch { ;}
                }
                // PlaneDegreeInfos = aa.Result;
            };
            if(!IsExit)
            client.SecureService.GetAllPlaneInfoAsync();
        }

        void client_OnItemValueChangedEvent(ItemBindingData itemdata)
        {
            
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
            //this.wrap.Height = lstMenu.Height.Equals(double.NaN) ? lstMenu.ActualHeight : lstMenu.Height;
        }

        private void ControlRoomMenu_MouseEnter(object sender, MouseEventArgs e)
        {
            PlaneDegreeInfo info = (sender as ControlRoomMenu ).DataContext as PlaneDegreeInfo;
             ControlRoomInfo rinfo=( from n in roomInfos where n.ERID==info.ERID  select n).FirstOrDefault();;
            if(rinfo!=null)
            this.mapctl.ZoomToLevel(15, new MapPoint(rinfo.X, rinfo.Y));
        }

        private void cboFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstMenu == null  || PlaneDegreeInfos==null)
                return;
            Rectangle r = (sender as ComboBox).SelectedItem as Rectangle;
            if (r.Tag.ToString() == "ALL")
                lstMenu.ItemsSource = PlaneDegreeInfos;
            else if (r.Tag.ToString() == "WARNING")
                lstMenu.ItemsSource = PlaneDegreeInfos.Where(n => n.AlarmStatus >= 1);
            else if (r.Tag.ToString() == "ALARM")
                lstMenu.ItemsSource = PlaneDegreeInfos.Where(n => n.AlarmStatus == 2);
            else if (r.Tag.ToString() == "NORMAL")
                lstMenu.ItemsSource = PlaneDegreeInfos.Where(n => n.AlarmStatus == 0);

        }

        private void ControlRoomMenu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                PlaneDegreeInfo info = (sender as ControlRoomMenu).DataContext as PlaneDegreeInfo;
                this.lstMenu.ItemsSource = null;
                this.NavigationService.Navigate(new Uri("/Forms/ControlRoom.xaml?PlaneID=" + info.PlaneID, UriKind.Relative));
            }
            catch (Exception ex)
            {
                ;
            }
          //  Common.Util.GetICommon().Navigate(new Uri("/slSecure;component/Forms/ControlRoom.xaml",UriKind.Relative));
        }

        bool IsExit = false;
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            IsExit = true;
            client.Dispose();
            tmr.Stop();
          
        }

    }
}

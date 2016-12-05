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
using slSecure;
using slSecure.Info;
using slSecure.Web;
using System.ServiceModel.DomainServices.Client;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Common;
using slWCFModule;
using slWCFModule.RemoteService;
using slSecure.Controls;
using System.Windows.Browser;


namespace slSecure
{
    public partial class Main : Page
    {
        ObservableCollection< CCTVLockInfo> cctvLocks = new ObservableCollection< CCTVLockInfo>();
        DateTime LastOperationDatetime = System.DateTime.Now;
        System.Windows.Threading.DispatcherTimer tmr = new System.Windows.Threading.DispatcherTimer();
        //MessageInfo[] msgInfos = new MessageInfo[] { 
        //   new MessageInfo(){ DateTime=DateTime.Now, Type="A", AlarmStatus=2, Message="Message1Message1"},
        //   new MessageInfo(){ DateTime=DateTime.Now, Type="D", AlarmStatus=1, Message="Message1Message1"},
        //   new MessageInfo(){ DateTime=DateTime.Now, Type="A", AlarmStatus=0, Message="Message1Message1"}
        //};
        ObservableCollection<AlarmData> lstAlarm = new ObservableCollection<AlarmData>();
        MyClient client ;
        public Main()
        {
            InitializeComponent();
            Util.GetICommon().SetMain(this);
           // this.lstMessage.ItemsSource = msgInfos;

        }

        // 使用者巡覽至這個頁面時執行。
        protected  async  override void OnNavigatedTo(NavigationEventArgs e)
        {

#if R23
            this.chkIsWindowsAlarm.Visibility = Visibility;
#endif
            SecureDBContext db = new SecureDBContext();// DB.GetDB();
            try
            {
                EntityQuery<tblSysParameter> qry = db.GetTblSysParameterQuery().Where(n => n.VariableName == "TimeoutReturnPage");

                var result = await db.LoadAsync(qry);
                
                tblSysParameter sysparam = result.FirstOrDefault();
                if (sysparam != null)
                {


                    tmr.Interval = TimeSpan.FromMinutes(double.Parse(sysparam.VariableValue));
                    tmr.Tick += tmr_Tick;
                    tmr.Start();

                }

                //  Microsoft.Expression.Interactivity.Layout.FluidMoveBehavior bev = new Microsoft.Expression.Interactivity.Layout.FluidMoveBehavior();



                EntityQuery<vwUserMenuAllow> q = db.GetVwUserMenuAllowQuery().Where(n => n.UserID == Util.GetICommon().GetUserID() && n.IsAllow == true);
                var UserMenus = await db.LoadAsync(q);

                var res = from n in UserMenus group n by n.GroupName into g select new UserGroupMenu() { GroupMenu = g.Key, Menus = g.OrderBy(k => k.MenuOrder).ToList() };
                this.acdMenu.ItemsSource = res;
#if R23
                this.frameMain.Navigate(new Uri("/Forms/R23/Monitor.xaml", UriKind.Relative));
#else
            this.frameMain.Navigate(new Uri("/Forms/R23/Monitor.xaml", UriKind.Relative));
          //  this.frameMain.Navigate(new Uri("/Forms/Monitor.xaml", UriKind.Relative));
#endif

            }
            catch { ;}
           txtTitle.DataContext = new tblMenu() { MenuName = "門禁監控" };
            this.acdMenu.SelectedIndex =1;

             client = new MyClient("CustomBinding_ISecureService",true);
             client.OnRegistEvent +=async  (s) =>
                 {

                     await HookAlarmEvent();
                     client.OnAlarm += client_OnAlarm;


                 };

             this.txtCCTVCnt1.Text = this.NavigationContext.QueryString["username"];
          //   await client.RegistAndGetKey();
        
          
           
             //client.SecureService.HookAlarmEventCompleted += (s, a) =>
             //    {

             //    };



        }

        Task HookAlarmEvent( )
        {
            TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
            client.SecureService.HookAlarmEventCompleted += (s, a) =>
            {
                if (a.Error != null)
                {
                    taskCompletionSource.TrySetResult(false);

                }
                taskCompletionSource.TrySetResult(true);


            };
            client.SecureService.HookAlarmEventAsync(client.Key );

            return taskCompletionSource.Task;
        }

        async void client_OnAlarm(slWCFModule.RemoteService.AlarmData alarmdata)
        {

            while (lstAlarm.Count > 3)
                lstAlarm.RemoveAt(lstAlarm.Count - 1);

            lstAlarm.Insert(0,alarmdata);

          //  lstAlarm.OrderByDescending(n => n.TimeStamp).Take(8);
            //if (alarmdata.AlarmType == AlarmType.RTU || alarmdata.AlarmType== AlarmType.PD)
            //{
              
                this.alarmPlayer.Stop();
                this.alarmPlayer.Play();
            //}
                if (lstMessage.ItemsSource == null)
                    this.lstMessage.ItemsSource = lstAlarm;/* lstAlarm.OrderByDescending(n => n.TimeStamp).Take(4);*/
            if (alarmdata.IsForkCCTVEvent)
            {
                try
                {
                    if(alarmdata.CCTVBindingData!=null)
                    await AddCCTVAsync(alarmdata.CCTVBindingData.MjpegCgiString, alarmdata.CCTVBindingData.UserName, alarmdata.CCTVBindingData.Password, alarmdata);
                   
                
                }
                catch (Exception ex)
                {
                    MessageBox.Show("alarm:"+alarmdata.AlarmType+alarmdata.Description+ ex.Message + "," + ex.StackTrace);
                }
                }

            int cnt = 0;
            while (lstCCTVLock.Children.Count > 3)
            {
                if (cnt++ > 10)
                    break;
                CCTVLock1 cctvlock = lstCCTVLock.Children.Skip(3).FirstOrDefault() as CCTVLock1;
                if (cctvlock != null)
                {
                   
                    lstCCTVLock.Children.Remove(cctvlock);
                }
            }

            if(chkIsWindowsAlarm.IsChecked??false)
                MessageBox.Show(alarmdata.Description);
            

          //  this.lstMessage.Items.Add(alarmdata.Description);
        }

        void tmr_Tick(object sender, EventArgs e)
        {

           // this.txtCCTVCnt.Text = CCTVLock.CnCnt.ToString();
            if (DateTime.Now.Subtract(LastOperationDatetime) > TimeSpan.FromMinutes(1 ))
            {
               // tmr.Stop();
                //this.NavigationService.Navigate(new Uri("/Login.xaml", UriKind.Relative));
                
#if R23
                this.frameMain.Navigate(new Uri("/Forms/R23/Monitor.xaml", UriKind.Relative));
#else
                this.frameMain.Navigate(new Uri("/Forms/R23/Monitor.xaml", UriKind.Relative));
              //  this.frameMain.Navigate(new Uri("/Forms/Monitor.xaml", UriKind.Relative));
#endif
            }
            //throw new NotImplementedException();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.acdMenu.Visibility == Visibility.Visible)
                this.acdMenu.Visibility = System.Windows.Visibility.Collapsed;
            else
                this.acdMenu.Visibility = System.Windows.Visibility.Visible;
        }

        private void acdMenu_SelectedItemsChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            this.txtTitle.DataContext = (sender as Control).DataContext;
            vwUserMenuAllow menu = (sender as Control).DataContext as vwUserMenuAllow;

            if (menu.XAML.Trim().ToUpper().StartsWith("HTTP:"))
            {
                MyHyperlinkButton button = new MyHyperlinkButton();

                button.NavigateUri = new Uri(menu.XAML);

                button.TargetName = "_blank";

                button.ClickMe();

            }
            else if (menu.XAML.Trim().ToUpper().StartsWith("@HTTP:"))
            {
              //  this.frameMain.Navigate(new Uri("/DivHtmlPage.xaml?url=" + menu.XAML.TrimStart(new char[] { '@' }), UriKind.Relative));
                this.frameMain.Navigate(new Uri("/Forms/WebPage.xaml?url=" + menu.XAML.TrimStart(new char[]{'@'}), UriKind.Relative));

            }

            //  this.frameMain.Navigate(new Uri("/Forms/WebPage.xaml?url=" + menu.XAML, UriKind.Relative));
            // System.Windows.Browser.HtmlPage.Window.Navigate(new Uri(System.Windows.Browser.HtmlPage.Document.DocumentUri, filePath), "_blank");
            else
                this.frameMain.Navigate(new Uri(menu.XAML, UriKind.Relative));
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            tmr.Stop();
#if R23
            slWCFModule.SSOService.SsoWebServiceClient client = new slWCFModule.SSOService.SsoWebServiceClient();
            try
            {
                client.logoutAsync();
            }
            catch { ;}
#endif
            this.NavigationService.Navigate(new  Uri("/Login.xaml",UriKind.Relative));
          
        }

        private void Page_MouseMove(object sender, MouseEventArgs e)
        {
            
            LastOperationDatetime = DateTime.Now;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private async void BtbCCTVLock_Click(object sender, RoutedEventArgs e)
        {

            //cctvLocks.Add(new Source.CCTVLockInfo() { DateTime = DateTime.Now });
            //lstCCTVLock.ItemsSource = from n in cctvLocks orderby n.DateTime descending select n;




            if(lstCCTVLock.Children.Count>=3)
               this.lstCCTVLock.Children.RemoveAt(0);// lstCCTVLock.Children.First()
            //this.lstCCTVLock.Children.Add(new slSecure.Controls.CCTVLock1("http://10.2.10.124:80/snapcif","admin","pass",true) { Width = 250, Height =200 });
           // this.lstCCTVLock.Children.Add(new slSecure.Controls.CCTVLock1("http://192.192.85.20:11000/snapcif", "admin", "pass", true) { Width = 250, Height = 200 });
            
            
            //while (lstCCTVLock.Children.Count > 8)
            //{
            //    CCTVLock cctvlock = lstCCTVLock.Children.Skip(8).FirstOrDefault() as CCTVLock;
            //    if (cctvlock != null)
            //    {
            //        lstCCTVLock.Children.Remove(cctvlock);
            //    }
            //}
            
          //  MessageBox.Show("ok");
        }

        public Task<object> AddCCTVAsync(string url,string username,string pasword,AlarmData adata)
        {
            slSecure.Controls.CCTVLock1 cctv = new Controls.CCTVLock1(url, username, pasword,true) { Width = 320, Height = 240, Margin = new Thickness(10) };

                //new slSecure.Controls.CCTVLock(new Random().Next(1,40)) { Width = 250, Height =200, Margin = new Thickness(10) };
            System.Threading.Tasks.TaskCompletionSource<object> source = new TaskCompletionSource<object>();
            cctv.DataContext = adata;
            cctv.Click += cctv_Click;
            //Dispatcher.BeginInvoke(() =>
            //    {
                    this.lstCCTVLock.Children.Insert(0, cctv);
                //});
            source.SetResult(new object());
            return source.Task;
        }

        void cctv_Click(object sender, MouseButtonEventArgs e)
        {
            AlarmData data = (sender as Button).DataContext  as AlarmData;
            this.frameMain.Navigate(new Uri("/Forms/ControlRoom.xaml?PlaneID="+data.PlaneID, UriKind.Relative));
        }

        


        public void Navigate(Uri uri)
        {
            this.frameMain.Navigate(uri);
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.lstCCTVLock.Children.Clear();
            }
            catch
            {
                ;
            }
            try
            {
                client.Dispose();
            }
            catch { ;}
        }

        private void lstMessage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
         
        }

        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {


            AlarmData alarmdata = (sender as StackPanel).DataContext as AlarmData;
            //   client.Dispose();
           
            if (alarmdata.AlarmType == AlarmType.CARD) //未歸還卡片警告
            {
#if !R23
                this.frameMain.Navigate(new Uri("/slSecureLib;component/Forms/R13/slReport.xaml?IsShowNoReturnMagneticCard=1" , UriKind.Relative));
#else
                this.frameMain.Navigate(new Uri("/slSecureLib;component/Forms/R23/slReport.xaml?IsShowNoReturnMagneticCard=1" , UriKind.Relative));
#endif
            }
            else

                if (alarmdata.AlarmType == AlarmType.PD)
            {
               

                //PaneID is PDID for AlarmType=PD
#if  R23
                //button.NavigateUri = new Uri("http://" + App.Current.Host.Source.Host + ":" + App.Current.Host.Source.Port + "/R23/secure/focus?PDName=" + HttpUtility.UrlEncode(alarmdata.PlaneName));
                
                 // http://10.21.99.80/PD/secure/focus?PDName=PD-030008-S-M-32
                this.frameMain.Navigate(new Uri("/Forms/WebPage.xaml?url=" + "http://" + App.Current.Host.Source.Host + ":" + App.Current.Host.Source.Port + "/PD/secure/focus?PDName=" + HttpUtility.UrlEncode(alarmdata.PlaneName), UriKind.Relative));
#else
                      MyHyperlinkButton button = new MyHyperlinkButton();
                button.NavigateUri = new Uri("http://" + App.Current.Host.Source.Host + ":" + App.Current.Host.Source.Port + "/R13/secure/focus?PDName=" +   HttpUtility.UrlEncode(alarmdata.PlaneName));
                button.TargetName = "_blank";

                button.ClickMe();
#endif

            }
#if R23
                else if (alarmdata.AlarmType == AlarmType.PowerMeter) 
                {
                    
                     this.frameMain.Navigate(new Uri("/slSecureLib;component/Forms/R23/slPowerMeterAndWaterMeter.xaml?IsShowWaterOrPower=Power",UriKind.Relative));
                  
                }
                else if( alarmdata.AlarmType == AlarmType.WaterMeter)
                {
                     this.frameMain.Navigate(new Uri("/slSecureLib;component/Forms/R23/slPowerMeterAndWaterMeter.xaml?IsShowWaterOrPower=Water",UriKind.Relative));
                }
#endif

                else
                this.frameMain.Navigate(new Uri("/Forms/ControlRoom.xaml?PlaneID=" + alarmdata.PlaneID, UriKind.Relative));
            //this.frameMain.Navigate(new Uri("/Forms/ControlRoom.xaml", UriKind.Relative));
        }

        private void StackPanel_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
           

        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
#if R23
            this.frameMain.Navigate(new Uri("/Forms/R23/Monitor.xaml", UriKind.Relative));
           
#else
            this.frameMain.Navigate(new Uri("/Forms/R23/Monitor.xaml", UriKind.Relative));
          //  this.frameMain.Navigate(new Uri("/Forms/Monitor.xaml", UriKind.Relative));
#endif
            txtTitle.DataContext = new tblMenu() { MenuName = "門禁監控" };
        }

      

    }

    public class UserGroupMenu
    {
        public string GroupMenu { get; set; }
        public System.Collections.Generic.List<vwUserMenuAllow> Menus { get; set; }

    }

    public class MyHyperlinkButton : HyperlinkButton
    {

        public void ClickMe()
        {

            base.OnClick();

        }

    }

}

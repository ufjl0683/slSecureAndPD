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

namespace slSecure.Controls
{
    public partial class CCTVLock1 : UserControl
    {
        public event MouseButtonEventHandler Click;
        WebClient client = new WebClient();
        bool IsBeginRead = false;
        bool ISExit = false;
        Random rnd = new Random();
        string url, username, pwd;
        bool TimeLimit;
        DateTime DateTimeBegin;
        System.Windows.Threading.DispatcherTimer tmr = new System.Windows.Threading.DispatcherTimer();
        public CCTVLock1()
        {
            InitializeComponent();
            DateTimeBegin = System.DateTime.Now;
           
            
        }

        


         public CCTVLock1(string url):this()
        {
           
        }

           public CCTVLock1(string url,string username,string   pwd,bool TimeLimit):this()
            {
                this.url = url;
                this.username = username;
                this.pwd = pwd;
               this.TimeLimit=TimeLimit;
               tmr.Interval = TimeSpan.FromMilliseconds(300);
               tmr.Tick += tmr_Tick;
               tmr.Start();
            }

           //public CCTVLock1(string url, string username, string pwd, bool TimeLimit)
           //    : this()
           //{

           //}

       
        public CCTVLock1(int ch):this(){
       }

        public void SetCCTVTitleVisivle(bool visible)
        {

            this.btnTitle.Visibility = visible ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (this.Click != null)
                this.Click(sender, null);
        }

        void tmr_Tick(object sender, EventArgs e)
        {
            SwitchCCTV();
        }
        int imginx = 0;
        public void SwitchCCTV()
        {

         //   tmr.Start();
           BeginReadCCTV();
            // this.Visibility = System.Windows.Visibility.Visible;
        }
        public void DisMiss()
        {
            this.IsBeginRead = false;
            this.DataContext = null;
            tmr.Stop();
            // this.Visibility = Visibility.Collapsed;
        }
        void BeginReadCCTV()
        {
            //WebRequest.RegisterPrefix("http://", System.Net.Browser.WebRequestCreator.ClientHttp);
            //var client = new WebClient();
            //client.Credentials = new NetworkCredential("username", "password");
            //client.UseDefaultCredentials = false;
            //client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
            //client.DownloadStringAsync(new Uri("http://mark.mymonster.nl"));
            WebRequest.RegisterPrefix("http://", System.Net.Browser.WebRequestCreator.ClientHttp);
            IsBeginRead = true;
           // tblCCTV cctvinfo = this.DataContext as tblCCTV;
            client = new WebClient();
            client.Credentials = new NetworkCredential(username, pwd);
            client.OpenReadCompleted += new OpenReadCompletedEventHandler(client_OpenReadCompleted);
            client.OpenReadAsync(new Uri(url   , UriKind.Absolute));

           // imginx = (imginx + 1) % 90000;
           // cctvinfo.CCTV_INX = imginx;
            //  cctvinfo.CCTV_INX = (cctvinfo.CCTV_INX+1) % 90000;
            
        }
        
        void client_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                System.Windows.Media.Imaging.BitmapImage bmp = new System.Windows.Media.Imaging.BitmapImage();
                Dispatcher.BeginInvoke(() =>
                {
                    try
                    {
                        bmp.SetSource(e.Result);
                        this.cctv.Source = bmp;
                       
                    }
                    catch
                    {
                        ;
                    }
                    finally
                    {

                        if (ISExit || (this.TimeLimit && DateTime.Now.Subtract(DateTimeBegin) > TimeSpan.FromMinutes(3)    ))
                            tmr.Stop();
                        //    BeginReadCCTV();
                    }

                   

                }
                );

            }
            catch
            { ;}
        }

        //private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        //{
        //    //if (!IsDesignTime())
        //    //    BeginReadCCTV();

        //}

        private void LayoutRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            ISExit = true;
            tmr.Stop();
        }

        private void cctv_Unloaded(object sender, RoutedEventArgs e)
        {

            ISExit = true;

            this.DisMiss();

        }

        //public bool IsDesignTime()
        //{
        //    return DesignerProperties.GetIsInDesignMode(Application.Current.RootVisual);
        //}

    }
}

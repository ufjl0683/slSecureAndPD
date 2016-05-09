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

namespace slSecure
{
    public partial class MainPage : UserControl,Common.ICommon
    {
      
    
        public MainPage()
        {
            InitializeComponent();
            if (!App.Current.HasElevatedPermissions)
                MessageBox.Show(" No Elevated trust!");


     //       MessageBox.Show("http://" + App.Current.Host.Source.Host + ":" + App.Current.Host.Source.Port);


            if (Application.Current.IsRunningOutOfBrowser)
            {
                App.Current.CheckAndDownloadUpdateCompleted += Current_CheckAndDownloadUpdateCompleted;
                App.Current.CheckAndDownloadUpdateAsync();

                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            }

            //slWCFModule.MCNSService.MCNSServiceSoapClient client = new slWCFModule.MCNSService.MCNSServiceSoapClient();
            //client.GetR23DoorInfoAsync();
            
#if DEBUG
           this.frame.Navigate(new Uri("/Forms/Monitor.xaml", UriKind.Relative));
#else 


       
#if R23          
            slWCFModule.SSOService.SsoWebServiceClient client = new slWCFModule.SSOService.SsoWebServiceClient("SsoWebServicePort");
            client.checkAuthenticationCompleted += (s, a) =>
                {
                    if (a.Error != null)
                    {
                        this.frame.Navigate(new Uri("/Login.xaml", UriKind.Relative));
                        return;
                    }

                    if (a.Result.status)
                    {
                        var res = a.Result;
                        //已登入
                        if (res.userValue.roles.SingleOrDefault(n => n == "SVWS_Admin") != null)
                        {
                            (App.Current as App).UserID = "ssoadmin";
                            (App.Current as App).UserName = res.userValue.login;
                            this.frame.Navigate(new Uri(string.Format("/Main.xaml?userid={0}&username={1}", "ssoadmin", res.userValue.login), UriKind.Relative));
                        }
                        else if (res.userValue.roles.SingleOrDefault(n => n == "SVWS_User") != null)
                        {
                            (App.Current as App).UserID = "ssonormal";
                            (App.Current as App).UserName = res.userValue.login;

                            this.frame.Navigate(new Uri(string.Format("/Main.xaml?userid={0}&username={1}", "ssonormal", res.userValue.login), UriKind.Relative));
                            //   this.NavigationService.Navigate(new Uri(string.Format("/Main.xaml?userid={0}&username={1}", "ssoadmin", "SSO管理者"), UriKind.Relative));
                        }
                        else
                        {
                            MessageBox.Show("非授權帳號");
                        }
                    }
                    else
                        this.frame.Navigate(new Uri("/Login.xaml", UriKind.Relative));
                    //未登入

                };
            client.checkAuthenticationAsync();

            //normal r23 login procedure
           // this.frame.Navigate(new Uri("/Login.xaml", UriKind.Relative));
#else
            // this.Content=new slSecureLib.Forms.R23.slPowerMeterAndWaterMeter();
            this.frame.Navigate(new Uri("/Login.xaml", UriKind.Relative));
#endif




            
#endif
        }

        void Current_CheckAndDownloadUpdateCompleted(object sender, CheckAndDownloadUpdateCompletedEventArgs e)
        {
            if (e.UpdateAvailable)
            {
                MessageBox.Show("新版成程式可下載. " +
                    "請重新啟動本程式進行安裝.");
            }
            else if (e.Error != null &&
                e.Error is PlatformNotSupportedException)
            {
                MessageBox.Show("An application update is available, " +
                    "but it requires a new version of Silverlight. " +
                    "Visit the application home page to upgrade.");
            }
            else
            {
                //no new version available
            }

        }
        public string GetAppTiTle()
        {

            return (App.Current as App).AppName;
        }


        public string GetUserName()
        {
            return (App.Current as App).UserName;
        }

        public string GetUserID()
        {
            return (App.Current as App).UserID;
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {

        }


        public void Navigate(Uri uri)
        {
            (App.Current as App).main.Navigate(uri);
        }




        public void SetMain(object main)
        {
            (App.Current as App).main = main as Main;
        }
    }
}

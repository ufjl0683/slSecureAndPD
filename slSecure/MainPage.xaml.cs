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
            
            
#if DEBUG
           this.frame.Navigate(new Uri("/Forms/Monitor.xaml", UriKind.Relative));
#else
            this.frame.Navigate(new Uri("/Login.xaml", UriKind.Relative));
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

using slSecure;
using slSecure.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.DomainServices.Client;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace slSecure
{
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

              
             if (Application.Current.IsRunningOutOfBrowser)
                {
                 App.Current.CheckAndDownloadUpdateCompleted+=Current_CheckAndDownloadUpdateCompleted;
                    App.Current.CheckAndDownloadUpdateAsync();

                    Application.Current.MainWindow.WindowState = WindowState.Maximized;
                }
            
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


        private async void Button_Click(object sender, RoutedEventArgs e)
        {


            SecureDBContext db = new SecureDBContext();//DB.GetDB();

           
            EntityQuery<tblUser> q = db.GetTblUserQuery().Where(n => n.UserID == txtAccount.Text.Trim() && n.Password == pwdPassword.Password.Trim());
            
            
             //var  result= await db.LoadAsync<tblUser>(q);
            LoadOperation<tblUser> lo = db.Load(q);
            lo.Completed += (s, a) =>
                {
                    if (lo.Error != null)
                    {
                        MessageBox.Show(lo.Error.Message);
                        return;
                    }


                    tblUser user = lo.Entities.FirstOrDefault();
                    if (user == null)
                    {
                        MessageBox.Show("帳號密碼錯誤!");
                        return;
                    }
                    (App.Current as App).UserID = user.UserID;
                    (App.Current as App).UserName = user.UserName;



                    this.NavigationService.Navigate(new Uri(string.Format("/Main.xaml?userid={0}&username={1}", user.UserID, user.UserName), UriKind.Relative));


                };
           

           

            //tblUser user = result.FirstOrDefault();
            //if (user == null)
            //{
            //    MessageBox.Show("帳號密碼錯誤!");
            //    return;
            //}


            //(App.Current as App).UserID = user.UserID;
            //(App.Current as App).UserName = user.UserName;


           
            //this.NavigationService.Navigate(new Uri(string.Format("/Main.xaml?userid={0}&username={1}", user.UserID, user.UserName), UriKind.Relative));

        }
    }
}

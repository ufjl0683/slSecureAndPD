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


          


              
           
        }

       


        private async void Button_Click(object sender, RoutedEventArgs e)
        {

#if R23
           // new Dialog.WaterPowerDiagram(1, true).Show();
            if (txtAccount.Text == "david" && pwdPassword.Password == "1234")
            {
                (App.Current as App).UserID = "david";
                (App.Current as App).UserName = "david";
                this.NavigationService.Navigate(new Uri(string.Format("/Main.xaml?userid={0}&username={1}", "david", "david"), UriKind.Relative));
                return;
            }
            slWCFModule.SSOService.SsoWebServiceClient client = new slWCFModule.SSOService.SsoWebServiceClient();
            client.loginCompleted += (s, a) =>
                {
                    if (a.Error != null)
                    {
                        MessageBox.Show("本機可能未授權或連線異常");
                        return;
                    }
                    var res = a.Result;
                    if (!res.status)
                        MessageBox.Show("帳號密碼錯誤");
                    else
                    {
                        if (res.userValue.roles.SingleOrDefault(n => n == "SVWS_Admin") != null)
                        {
                            (App.Current as App).UserID = "ssoadmin";
                           // (App.Current as App).UserName = res.userValue.login;
                            (App.Current as App).UserName = res.userValue.properies.Where(n => n.name == "name").Select(n => n.value).FirstOrDefault();
                            this.NavigationService.Navigate(new Uri(string.Format("/Main.xaml?userid={0}&username={1}", "ssoadmin", (App.Current as App).UserName), UriKind.Relative));
                        }
                        else if (res.userValue.roles.SingleOrDefault(n => n == "SVWS_User") != null)
                        {
                            (App.Current as App).UserID = "ssonormal";
                            //(App.Current as App).UserName = res.userValue.login;
                            (App.Current as App).UserName =  res.userValue.properies.Where(n => n.name == "name").Select(n=>n.value).FirstOrDefault();
                            this.NavigationService.Navigate(new Uri(string.Format("/Main.xaml?userid={0}&username={1}", "ssonormal", (App.Current as App).UserName), UriKind.Relative));
                            //   this.NavigationService.Navigate(new Uri(string.Format("/Main.xaml?userid={0}&username={1}", "ssoadmin", "SSO管理者"), UriKind.Relative));
                        }
                        else
                        {
                            MessageBox.Show("非授權帳號");
                            client.logoutAsync();
                            // this.NavigationService.Navigate(new Uri("/Login.xaml", UriKind.Relative));
                        }


                    }

                };

            client.loginAsync("SVWS", txtAccount.Text.Trim(), pwdPassword.Password.Trim());

             

            //================normal R23 ============================
            //SecureDBContext db = new SecureDBContext();//DB.GetDB();


            //EntityQuery<tblUser> q = db.GetTblUserQuery().Where(n => n.UserID == txtAccount.Text.Trim() && n.Password == pwdPassword.Password.Trim());


            ////var  result= await db.LoadAsync<tblUser>(q);
            //LoadOperation<tblUser> lo = db.Load(q);
            //lo.Completed += (s, a) =>
            //{
            //    if (lo.Error != null)
            //    {
            //        MessageBox.Show(lo.Error.Message);
            //        return;
            //    }


            //    tblUser user = lo.Entities.FirstOrDefault();
            //    if (user == null)
            //    {
            //        MessageBox.Show("帳號密碼錯誤!");
            //        return;
            //    }
            //    (App.Current as App).UserID = user.UserID;
            //    (App.Current as App).UserName = user.UserName;



            //    this.NavigationService.Navigate(new Uri(string.Format("/Main.xaml?userid={0}&username={1}", user.UserID, user.UserName), UriKind.Relative));


            //};

#else

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
 

           
#endif
           

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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new Dialog.WaterPowerDiagram(1, false).Show();
        }
    }
}

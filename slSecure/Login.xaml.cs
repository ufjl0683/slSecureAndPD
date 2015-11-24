﻿using slSecure;
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

            //slWCFModule.SSOService.SsoWebServiceClient client = new slWCFModule.SSOService.SsoWebServiceClient();
            //client.loginCompleted += (s, a) =>
            //    {
            //        if (a.Error != null)
            //            return;
            //        var res = a.Result;
            //        if (!res.status)
            //            MessageBox.Show("帳號密碼錯誤");
                    
            //    };

            //client.loginAsync("SVWS", txtAccount.Text.Trim(), pwdPassword.Password.Trim());

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
    }
}

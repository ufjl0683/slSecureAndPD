using slSecure.Dialog;
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
    public partial class CCTV : UserControl,I_IO
    {



        public string Url
        {
            get { return (string)this.GetValue(UrlProperty); }
            set { this.SetValue(UrlProperty, value); }
        }
        public static readonly DependencyProperty UrlProperty = DependencyProperty.Register(
          "Url", typeof(string), typeof(CCTV), new PropertyMetadata("" , null));

        public string UserName
        {
            get { return (string)this.GetValue(UserNameProperty); }
            set { this.SetValue(UserNameProperty, value); }
        }
        public static readonly DependencyProperty UserNameProperty = DependencyProperty.Register(
          "UserName", typeof(string), typeof(CCTV), new PropertyMetadata("", null));

        public string Password
        {
            get { return (string)this.GetValue(PasswordProperty); }
            set { this.SetValue(PasswordProperty, value); }
        }
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register(
          "Password", typeof(string), typeof(CCTV), new PropertyMetadata("", null));
      
        public CCTV()
        {
            InitializeComponent();
        }

        public void SetBlind(bool IsBlind)
        {
            if (IsBlind)
                (this.Resources["stbBlind"] as Storyboard).Begin();
            else
                (this.Resources["stbBlind"] as Storyboard).Stop();
        }

        private bool _IsSelect;
        public bool IsSelect
        {
            get
            {
                return _IsSelect;
            }
            set
            {
                if (value != _IsSelect)
                {
                    _IsSelect = value;
                    if (value == true)
                    {
                        SelectLine.Visibility = System.Windows.Visibility.Visible;
                        //System.Windows.Threading.DispatcherTimer tmr = new System.Windows.Threading.DispatcherTimer();
                        //tmr.Interval = TimeSpan.FromSeconds(3);

                        //tmr.Tick += (s, e) =>
                        //{
                        //    tmr.Stop();
                        //    SetBlind(false);
                        //};
                        //tmr.Start();
                    }
                    else
                        SelectLine.Visibility = System.Windows.Visibility.Collapsed;

                   // SetBlind(value);
                }
            }
        }

        private void LayoutRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {


            if (Url != "" && UserName != "" && Password != "")
                new CCTVDialog(Url, UserName, Password).Show();
            else if (Url != "")
                new CCTVDialog(Url).Show();
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
          
           
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           

           
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           
        }
    }
}

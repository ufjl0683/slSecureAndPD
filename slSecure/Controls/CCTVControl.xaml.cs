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
    public partial class CCTVControl : UserControl
    {
        
        int ch;
        string UserName, Pwd, Url;
        int LoginType = 0;

      //  bool IsEnableCloseButton;

        public bool IsEnableCloseButton
        {
            set
            {
                if(value)
                    this.CancelButton.Visibility = System.Windows.Visibility.Visible;
                else
                    this.CancelButton.Visibility = System.Windows.Visibility.Collapsed;
            }
            get
            {
                if (this.CancelButton.Visibility == System.Windows.Visibility.Visible)
                    return true;
                else
                    return false;

            }
        }

        public CCTVControl()
        {
            InitializeComponent();
            LoginType = 0;
        }
        public CCTVControl(int ch):this()
        {

            this.ch = ch;
            LoginType = 1;
        }
        public CCTVControl(string url)
            : this()
        {
            this.Url = url;
            LoginType = 2;
        }

        public CCTVControl(string url, string username, string pwd)
            : this()
        {
            this.Url = url;
            this.UserName = username;
            this.Pwd = pwd;
            LoginType = 3;
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
         //   this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
         //
            //this.DialogResult = false;
            if (this.Parent is Grid)
            {
                (this.Parent as Grid).Children.Remove(this);
            }
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            CCTVLock cctv=null;
            switch(LoginType)
            {
            case 0:
                this.LayoutRoot.Children.Add(new Controls.CCTVLock(3));
                break;
             case 1:
                    this.LayoutRoot.Children.Add(new Controls.CCTVLock(ch));
                break;
             case 2:
                  cctv = new Controls.CCTVLock(Url );
              //  this.LayoutRoot.Children.Add(new Controls.CCTVLock(Url));
                break;

             case 3:
                  cctv = new Controls.CCTVLock(Url, UserName, Pwd);
                cctv.SetCCTVTitleVisivle(false);

             
                break;


        }

            cctv.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            cctv.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            cctv.Margin = new Thickness(0);
            cctv.SetValue(Grid.RowProperty, 0);
            this.LayoutRoot.Children.Add(cctv);
        }
    }
}

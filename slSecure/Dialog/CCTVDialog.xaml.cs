using slSecure.Controls;
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

namespace slSecure.Dialog
{
    public partial class CCTVDialog : ChildWindow
    {

        int ch;
        string UserName, Pwd, Url;
        int LoginType = 0;
        public CCTVDialog()
        {
            InitializeComponent();
            LoginType = 0;
        }
        public CCTVDialog(int ch):this()
        {

            this.ch = ch;
            LoginType = 1;
        }
        public CCTVDialog(string url)
            : this()
        {
            this.Url = url;
            LoginType = 2;
        }

        public CCTVDialog(string url,string username,string pwd)
            : this()
        {
            this.Url = url;
            this.UserName = username;
            this.Pwd = pwd;
            LoginType = 3;
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            switch(LoginType)
            {
            case 0:
                this.LayoutRoot.Children.Add(new Controls.CCTVLock(3));
                break;
             case 1:
                    this.LayoutRoot.Children.Add(new Controls.CCTVLock(ch));
                break;
             case 2:
                this.LayoutRoot.Children.Add(new Controls.CCTVLock(Url));
                break;

             case 3:
                CCTVLock cctv = new Controls.CCTVLock(Url, UserName, Pwd,false);
                cctv.SetCCTVTitleVisivle(false);
                this.LayoutRoot.Children.Add(cctv);
                break;


        }
        }
    }
}


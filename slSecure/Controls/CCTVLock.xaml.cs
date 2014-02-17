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
    public partial class CCTVLock : UserControl
    {
        public event MouseButtonEventHandler Click;
        public CCTVLock(int ch )
        {
            InitializeComponent();
#if DEBUG
            this.browser.Navigate(new Uri("http://localhost:65254/CCTVLock.aspx?ch=" + ch, UriKind.Absolute));
#else
            this.browser.Navigate(new Uri("http://192.192.85.40/sl/CCTVLock.aspx?ch=" + ch, UriKind.Absolute));
#endif
           // this.browser.Source=new Uri("http://localhost:65254/CCTVLock.aspx?ch="+ch,UriKind.Absolute);
        }

        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void browser_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           
        }

        private void LayoutRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (this.Click != null)
                this.Click(sender, null);
        }

       
    }
}

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
using System.Windows.Navigation;

namespace slSecure.Forms
{
    public partial class WebPage : Page
    {

       
        public WebPage()
        {
            InitializeComponent();
         
        }

        // 使用者巡覽至這個頁面時執行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                string url = this.NavigationContext.QueryString["url"].ToString();
                this.browser.Navigate(new Uri(url, UriKind.Absolute));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
              
             
           
        }

    }
}

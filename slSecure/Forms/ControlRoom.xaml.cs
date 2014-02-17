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
    public partial class ControlRoom : Page
    {
        public ControlRoom()
        {
            InitializeComponent();
        }

        // 使用者巡覽至這個頁面時執行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void DOOR_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void CCTV_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            new Dialog.CCTVDialog().Show();
        }

        private void ScrollViewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            viewBox.Width = scrollViewer.ViewportWidth;
            viewBox.Height = scrollViewer.ViewportHeight;
            //Canvas.Width = image.ActualWidth;
            //Canvas.Height = image.ActualHeight;
        }

        private void Viewbox_SizeChanged(object sender, SizeChangedEventArgs e)
        {
          
        }

    }
}

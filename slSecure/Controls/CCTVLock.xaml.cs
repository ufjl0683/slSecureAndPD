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
        MjpegProcessor.MjpegDecoder decoder;
        public CCTVLock()
        {
            InitializeComponent();

        }
        public CCTVLock(string url):this()
        {

            decoder = new MjpegProcessor.MjpegDecoder();
            decoder.FrameReady += decoder_FrameReady;
            decoder.Error += decoder_Error;
            //#	Result	Protocol	Host	URL	Body	Caching	Content-Type	Process	Comments	Custom	
            //24	 - 	HTTP	210.241.67.167	/abs2mjpg/mjpg?camera=14&resolution=352x240&1399178264829	-1			chrome:25404			

            

            decoder.ParseStream(new Uri(url, UriKind.Absolute));

        }
        public CCTVLock(string url,string username,string   pwd):this()
        {

            decoder = new MjpegProcessor.MjpegDecoder();
            decoder.FrameReady += decoder_FrameReady;
            decoder.Error += decoder_Error;
            //#	Result	Protocol	Host	URL	Body	Caching	Content-Type	Process	Comments	Custom	
            //24	 - 	HTTP	210.241.67.167	/abs2mjpg/mjpg?camera=14&resolution=352x240&1399178264829	-1			chrome:25404			

            

            decoder.ParseStream(new Uri(url, UriKind.Absolute),username,pwd);

        }

        public CCTVLock(int ch):this()
        {
            decoder = new MjpegProcessor.MjpegDecoder();
            decoder.FrameReady += decoder_FrameReady;
            decoder.Error += decoder_Error;
            //#	Result	Protocol	Host	URL	Body	Caching	Content-Type	Process	Comments	Custom	
            //24	 - 	HTTP	210.241.67.167	/abs2mjpg/mjpg?camera=14&resolution=352x240&1399178264829	-1			chrome:25404			

            //    decoder.ParseStream(new Uri("http://210.241.67.167/abs2mjpg/mjpg?resolution=352x240&camera="+ch, UriKind.Absolute));

            decoder.ParseStream(new Uri("http://210.241.67.167/abs2mjpg/mjpg?resolution=352x240&camera=" + ch, UriKind.Absolute));

            //  decoder.ParseStream(new Uri("http://192.192.161.17:90/axis-cgi/mjpg/video.cgi?resolution=CIF&camera=1", UriKind.Absolute),"admin","admin");
#if DEBUG
            //this.browser.Navigate(new Uri("http://localhost:65254/CCTVLock.aspx?ch=" + ch, UriKind.Absolute));
#else
            //  this.browser.Navigate(new Uri("http://192.192.85.40/sl/CCTVLock.aspx?ch=" + ch, UriKind.Absolute));
#endif
            // this.browser.Source=new Uri("http://localhost:65254/CCTVLock.aspx?ch="+ch,UriKind.Absolute);
           
        }

        void decoder_Error(object sender, MjpegProcessor.ErrorEventArgs e)
        {
            MessageBox.Show(e.Message);
        }

        void decoder_FrameReady(object sender, MjpegProcessor.FrameReadyEventArgs e)
        {
            this.cctv.Source = e.BitmapImage;
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

        private void cctv_Unloaded(object sender, RoutedEventArgs e)
        {
            if (decoder != null)
            {
                decoder.StopStream();
                decoder = null;
            }

           
        }

       
    }
}

using System;
using System.Windows;
using MjpegProcessor;

namespace MjpegProcessorTestWPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		readonly MjpegDecoder _mjpeg;

		public MainWindow()
		{
			InitializeComponent();
			_mjpeg = new MjpegDecoder();
			_mjpeg.FrameReady += mjpeg_FrameReady;
			_mjpeg.Error += _mjpeg_Error;
		}

		private void Start_Click(object sender, RoutedEventArgs e)
		{
			_mjpeg.ParseStream(new Uri("http://192.192.85.20:11000/getimage"),"admin","pass");
		}

		private void mjpeg_FrameReady(object sender, FrameReadyEventArgs e)
		{
			image.Source = e.BitmapImage;
		}

		void _mjpeg_Error(object sender, ErrorEventArgs e)
		{
			MessageBox.Show(e.Message);
		}
	}
}

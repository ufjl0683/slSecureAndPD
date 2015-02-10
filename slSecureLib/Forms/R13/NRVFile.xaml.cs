using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace slSecureLib.Forms.R13
{
    public partial class NRVFile : ChildWindow
    {
        string nvrFile = "";
        double status = 0;

        public NRVFile()
        {
            InitializeComponent();

        }
        public NRVFile(string ERName, string Door, string CardType, DateTime StartTime, string NVRFile, string VideoRecord)
        {
            InitializeComponent();

            txt_ERName.Text = ERName;
            txt_Door.Text = Door;
            txt_StartTime.Text = StartTime.ToLongDateString() + " " + StartTime.ToLongTimeString();
            txt_EventName.Text = CardType;
            nvrFile = NVRFile;

            string showNVRFile= "/VideoRecord/" + nvrFile.Trim();
            //media.Source = new Uri("http://192.192.85.64/secure/ClientBin" + showNVRFile,UriKind.Absolute);
            media.Source = new Uri(VideoRecord + showNVRFile, UriKind.Absolute);
           

            double totalSeconds = media.Position.TotalSeconds;      // 获取当前位置秒数
            double nowTotalSeconds = media.NaturalDuration.TimeSpan.TotalSeconds;  //获取文件总播放秒数

            nowTime.Text = totalSeconds.ToString();
            totalTime.Text = (Math.Round(nowTotalSeconds, 0)).ToString();

            //WebClient wc = new WebClient();
            //wc.OpenReadCompleted +=(s,a)=>
            //    {
            //        media.SetSource(a.Result as Stream);
            //    };
            //wc.OpenReadAsync(new Uri("http://192.192.85.64/secure/ClientBin" + showNVRFile,UriKind.Absolute));
        }



        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void PlayMedia_Click(object sender, RoutedEventArgs e)
        {
            media.Play();
        }

        private void PauseMedia_Click(object sender, RoutedEventArgs e)
        {
            media.Pause();
        }

        private void StopMedia_Click(object sender, RoutedEventArgs e)
        {
            media.Stop();

            timelineSlider.Value = 0;

            double totalSeconds = media.Position.TotalSeconds;      // 获取当前位置秒数
            double nowTotalSeconds = media.NaturalDuration.TimeSpan.TotalSeconds;  //获取文件总播放秒数

            nowTime.Text = totalSeconds.ToString();
            totalTime.Text = (Math.Round(nowTotalSeconds, 0)).ToString();
        }

        private void media_MediaOpened(object sender, RoutedEventArgs e)
        {
            this.timelineSlider.Minimum = 0;
            this.timelineSlider.Maximum = 100;
            double seconds = this.media.NaturalDuration.TimeSpan.TotalSeconds;
            for (int i = (int)this.timelineSlider.Minimum; i < (int)this.timelineSlider.Maximum; i++)
            {
                TimelineMarker marker = new TimelineMarker();
                double time = seconds / this.timelineSlider.Maximum * i;
                marker.Time = new TimeSpan(0, 0, (int)time);
                marker.Text = "marker";
                marker.Type = "marks";
                this.media.Markers.Add(marker);
            }
        }

        private void timelineSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //讀取影片秒數     
            double seconds = this.media.NaturalDuration.TimeSpan.TotalSeconds;
            double time = seconds / this.timelineSlider.Maximum * this.timelineSlider.Value;

            if (e.NewValue < e.OldValue) //判斷是否是使用者拖拉Slider     
            {
                media.Position = new TimeSpan(0, 0, (int)e.NewValue);
                time = status = e.NewValue; //reset flag     
            }

            //比較flag, 若比較小, 就return.     
            if (status > time) { return; }

            media.Position = new TimeSpan(0, 0, (int)time);    
        }

        private void media_MarkerReached(object sender, TimelineMarkerRoutedEventArgs e)
        {
            double time = e.Marker.Time.TotalSeconds;
            double seconds = this.media.NaturalDuration.TimeSpan.TotalSeconds;


            if ((int)seconds <= 0)
            {
                return;
            }
            double marker = (time * this.timelineSlider.Maximum / seconds);

            if (status > marker) { return; } else { status = marker; }
            this.timelineSlider.Value = marker;

            nowTime.Text = time.ToString();
            totalTime.Text = (Math.Round(seconds,0)).ToString();
        }
    }
}


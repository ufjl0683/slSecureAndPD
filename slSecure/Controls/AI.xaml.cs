﻿using slWCFModule.RemoteService;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace slSecure.Controls
{
    public partial class AI : UserControl,I_IO
    {


        public object Value
        {
            get { return  this.GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
          "Value", typeof(object), typeof(AI), new PropertyMetadata(0.0 , new PropertyChangedCallback(OnValueChanged)
      ));

        public static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
             
        }


        public int Degreee
        {
            get { return (int)this.GetValue(DegreeeProperty); }
            set { this.SetValue(DegreeeProperty, value); }
        }
        public static readonly DependencyProperty DegreeeProperty = DependencyProperty.Register(
          "Degreee", typeof(int), typeof(AI), new PropertyMetadata(0, new PropertyChangedCallback(OnDegreeeChanged)
      ));

        public static void OnDegreeeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            int Degreee = (int)e.NewValue;
            if (Degreee == 0)
            {
                (d as AI).txtContent.Foreground = new SolidColorBrush(Colors.Black);
            }
            else if (Degreee == 1)
                (d as AI).txtContent.Foreground = new SolidColorBrush(Colors.Orange);

            else
                (d as AI).txtContent.Foreground = new SolidColorBrush(Colors.Red);
        }

        public AI()
        {
            InitializeComponent();
           
            this.DataContextChanged += AI_DataContextChanged;
           
        }


      

        void AI_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ItemBindingData data = this.DataContext as ItemBindingData;

        
            if(data==null)
                return;


            if (data.IsAlarm && data.Degree > 0)
                this.SetBlind(true);
            else
                this.SetBlind(false);

            data.PropertyChanged += data_PropertyChanged;
            
            
           // throw new NotImplementedException();
        }

        void data_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ItemBindingData data = this.DataContext as ItemBindingData;

            if (e.PropertyName == "Degree" || e.PropertyName == "IsAlarm")
            {
                if (data.IsAlarm && data.Degree > 0)
                    this.SetBlind(true);
                else
                    this.SetBlind(false);
            }
           // throw new NotImplementedException();
        }


   

        public void SetBlind(bool IsBlind)
        {
            if (IsBlind)
                (this.Resources["stbBlind"] as Storyboard).Begin();
            else
                (this.Resources["stbBlind"] as Storyboard).Stop();
        }


        public string Content
        {
            get
            {
                return this.txtContent.Text;
            }
            set
            {
                this.txtContent.Text = value;
            }
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
                        System.Windows.Threading.DispatcherTimer tmr = new System.Windows.Threading.DispatcherTimer();
                        //tmr.Interval = TimeSpan.FromSeconds(3);

                        //tmr.Tick += (s, e) =>
                        //    {
                        //        tmr.Stop();
                        //        SetBlind(false);
                        //    };
                        //tmr.Start();
                    }
                    else
                        SelectLine.Visibility = System.Windows.Visibility.Collapsed;

                    //SetBlind(value);
                }
            }
        }

        private void mnuAttributeSettinh_Click(object sender, RoutedEventArgs e)
        {
            ItemBindingData data = this.DataContext as ItemBindingData;
            new slSecureLib.Forms.SingleSetItemConfig(data.ItemID).Show();
         //   NavigationService.Navigate(new Uri("/slSecureLib;component/Forms/slSingleSetItemConfig.xaml?ItemID="+data.ItemID, UriKind.Relative));
        }

        private void mnumnuAlarmHistory_Click(object sender, RoutedEventArgs e)
        {
            ItemBindingData data = this.DataContext as ItemBindingData;
            new slSecureLib.Forms.SingleReport(data.ItemID,data.Type).Show();
        }

        private void mnuChart_Click(object sender, RoutedEventArgs e)
        {
            ItemBindingData data = ((sender as MenuItem).DataContext as ItemBindingData);
            //if (data.Type != "AI")
            //    return;

            Dialog.TRDialog dialog = new Dialog.TRDialog(data.ItemID);
            //dialog.Width = 800;
            //dialog.Height = 600;
            dialog.Show();
        }

        private void mnuSupressAlarm_Click(object sender, RoutedEventArgs e)
        {
            slWCFModule.MyClient client = new slWCFModule.MyClient("CustomBinding_ISecureService");
            ItemBindingData data = ((sender as MenuItem).DataContext as ItemBindingData);
            if (!data.IsAlarm)
                return;
            client.SecureService.SupressAlarmCompleted += (s, a) =>
            {
                if (a.Error != null)
                    MessageBox.Show(a.Error.Message);
                client.Dispose();
            };
            client.SecureService.SupressAlarmAsync(data.ItemID);
        }
    }
}

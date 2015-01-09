﻿using System;
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
    }
}

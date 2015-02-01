using slWCFModule.RemoteService;
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
    public partial class DI : UserControl,I_IO
    {
        public DI()
        {
            InitializeComponent();
            this.DataContextChanged += DI_DataContextChanged;
        }

        void  DI_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ItemBindingData data = this.DataContext as ItemBindingData;


            if (data == null)
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

        public object Value
        {
            get { return (object)this.GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
          "Value", typeof(object), typeof(DI), new PropertyMetadata(false, new PropertyChangedCallback(OnValueChanged)
      ));
        public static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
        public void SetBlind(bool IsBlind)
        {
            if (IsBlind)
                (this.Resources["stbBlind"] as Storyboard).Begin();
            else
                (this.Resources["stbBlind"] as Storyboard).Stop();
        }

        public string Content{
            get
            {
                return this.txtContent.Text;
            }
            set
            {
                this.txtContent.Text = value;
            }
            }
        public int Degreee
        {
            get { return (int)this.GetValue(DegreeeProperty); }
            set { this.SetValue(DegreeeProperty, value); }
        }
        public static readonly DependencyProperty DegreeeProperty = DependencyProperty.Register(
          "Degreee", typeof(int), typeof(DI), new PropertyMetadata(0, new PropertyChangedCallback(OnDegreeeChanged)
      ));

        public static void OnDegreeeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            int Degreee = (int)e.NewValue;
            if (Degreee == 0)
            {
                (d as DI).ellipse.Fill = new SolidColorBrush(Colors.Green);
            }
            //else if (Degreee == 1)
            //    (d as DI).ellipse.Fill = new SolidColorBrush(Colors.Orange);

            else
                (d as DI).ellipse.Fill = new SolidColorBrush(Colors.Red);
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
                        //System.Windows.Threading.DispatcherTimer tmr = new System.Windows.Threading.DispatcherTimer();
                        //tmr.Interval = TimeSpan.FromSeconds(3);

                        //tmr.Tick += (s, e) =>
                        //{
                        //    tmr.Stop();
                        //    SetBlind(false);
                        //};
                        //tmr.Start();
                    }
                    else
                        SelectLine.Visibility = System.Windows.Visibility.Collapsed;

                  //  SetBlind(value);
                }
            }
        }
    }
}

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
    public delegate void MenuClickHandler(object sender, MenuItem item);
    public partial class DOOR : UserControl,I_IO
    {

        public event MenuClickHandler OnMenuEvent;
        
        public object Value
        {
            get { return this.GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
          "Value", typeof(object), typeof(DOOR), new PropertyMetadata(0 , new PropertyChangedCallback(OnValueChanged)
      ));

        public static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
        public DOOR()
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

        
        public void SetDefaultColor()
        {

            path.Stroke = new SolidColorBrush(Colors.Green);
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

                    //SetBlind(value);
                }
            }
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void mnuForceOpen_Click(object sender, RoutedEventArgs e)
        {
            if (this.OnMenuEvent != null)
                OnMenuEvent(this, sender as MenuItem);
        }
    }
}

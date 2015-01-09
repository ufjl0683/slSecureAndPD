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
    public partial class DO : UserControl, I_IO
    {
        public DO()
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

        public    string Content
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

                   
                }
            }
        }
    }
}

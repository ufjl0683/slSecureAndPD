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
    public partial class IOMenu : UserControl
    {
        public IOMenu()
        {
            InitializeComponent();
               this.DataContextChanged += IOMENU_DataContextChanged;
        }

        void IOMENU_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ItemBindingData data = this.DataContext as ItemBindingData;

            if (data.IsAlarm && data.Degree > 0)
                this.SetBlind(true);
            else
                this.SetBlind(false);
            
           // throw new NotImplementedException();
      
        }

        public void SetBlind(bool IsBlind)
        {
            if (IsBlind)
                (this.Resources["stbBlind"] as Storyboard).Begin();
            else
                (this.Resources["stbBlind"] as Storyboard).Stop();
        }
    }
}

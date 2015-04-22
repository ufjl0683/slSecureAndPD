using System;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace slSecure.Info
{
    public class ControlRoomInfo:INotifyPropertyChanged
    {
        public int ERID { get; set; }
        public string  Name { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public string Type { get; set; }  //P:Power C: Control

        private int _AlarmStatus;
        public int AlarmStatus {
            get
            {
                return _AlarmStatus;
            }
            set
            {
                if (value != _AlarmStatus)
                {
                    _AlarmStatus = value;
                    if (PropertyChanged != null)
                        this.PropertyChanged(this, new PropertyChangedEventArgs("AlarmStatus"));
                }
            }
        }

        string _ColorString;
        public string ColorString {
            get { return _ColorString; }
            set
            {
                if (value != _ColorString)
                {
                    _ColorString = value;
                    if (PropertyChanged != null)
                        this.PropertyChanged(this, new PropertyChangedEventArgs("ColorString"));
                }
            }
        
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

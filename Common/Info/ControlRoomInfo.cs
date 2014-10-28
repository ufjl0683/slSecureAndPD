using System;
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
    public class ControlRoomInfo
    {
        public string  Name { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public string Type { get; set; }  //P:Power C: Control
        public int AlarmStatus { get; set; }
    }
}

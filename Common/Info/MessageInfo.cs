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

namespace Common.Info
{
    public class MessageInfo
    {
        public DateTime DateTime { get; set; }
        public string Type { get; set; }  //A:analog,D:Digital  P:Power

        public int AlarmStatus { get; set; }
        public string Message { get; set; }

    }
}

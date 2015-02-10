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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;

namespace slSecureLib
{
    public class ConnectValue : System.Windows.Data.IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value.GetType() == typeof(Int16))
            {
                Int16 connect = (Int16)value;
                if (connect == 1)
                    return "定期卡";
                else if (connect == 2)
                    return "臨時卡";
                else if (connect == 3)
                    return "無限卡";
                else if (connect == 4)
                    return "虛擬卡";
            }
            if (value != null && value.GetType() == typeof(string))
            {
                string tempstring = (string)value;
                if (tempstring == "P")
                    return "虛擬卡";
                else if (tempstring == "C")
                    return "一般卡";
                else if (tempstring == "I")
                    return "新增";
                else if (tempstring == "D")
                    return "刪除";
            }
            
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

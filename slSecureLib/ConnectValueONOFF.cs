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
    public class ConnectValueONOFF : System.Windows.Data.IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value.GetType() == typeof(bool))
            {
                bool connect = (bool)value;
                if (connect)
                    return "成功";
                else
                    return "失敗";
            }
            if (value != null && value.GetType() == typeof(Int16))
            {
                Int16 connect = (Int16)value;
                if (connect == 1)
                    return "失敗";
                else
                    return "成功";
            }
            if (value != null && value.GetType() == typeof(string))
            {
                string tempstring = (string)value;
                if (tempstring == "Y")
                    return "是";
                else if (tempstring == "N")
                    return "否";
            }
            
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}


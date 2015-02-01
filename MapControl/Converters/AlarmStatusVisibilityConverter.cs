﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MapControl.Converters
{
    public class AlarmStatusVisibilityConverter:IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility res=Visibility.Visible;
            return res;
            switch (System.Convert.ToInt32(value))
            {
                case 0:
                    res = Visibility.Collapsed;
                    break;
                case 1:
                case 2:
                    res = Visibility.Visible;
                    break;
            }
            return res;
           

            //throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
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

namespace slSecure.Converters
{
    public class ColorStringValueConverter:IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string colorstr = value.ToString();
            switch (colorstr.Trim())
            {
                case "Red":
                    return new SolidColorBrush(Colors.Red);
                    break;
                case "Green":
                      return new SolidColorBrush(Colors.Green);
                    break;
                case "Yellow":
                case "Orange":
                    return new SolidColorBrush(Colors.Orange);
                    break;
                case "Gray":
                case "Grey":
                    return new SolidColorBrush(Colors.Gray);
                    break;
                default:
                    return new SolidColorBrush(Colors.Green);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

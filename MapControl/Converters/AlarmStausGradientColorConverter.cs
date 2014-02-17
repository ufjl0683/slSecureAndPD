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

namespace MapControl.Converters
{
    public class AlarmStausGradientColorConverter:IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int CURRENT_DEGREE = System.Convert.ToInt32(value);
            RadialGradientBrush gb = parameter as RadialGradientBrush;

            switch (CURRENT_DEGREE)
            {
                case 0:

                    //return gb;
                    RadialGradientBrush rb = new RadialGradientBrush();
                    //rb.GradientStops.Add(new GradientStop() { Color = Colors.Transparent, Offset = 0 });
                    return rb;
                case 1:
                    foreach (GradientStop s in gb.GradientStops)
                    {
                        Color c = Colors.Orange;
                        c.A = s.Color.A;
                        s.Color = c;
                    }
                    return gb;
                case 2:
                    foreach (GradientStop s in gb.GradientStops)
                    {
                        Color c = Colors.Red;
                        c.A = s.Color.A;
                        s.Color = c;
                    }
                    return gb;
                case 3:
                    foreach (GradientStop s in gb.GradientStops)
                    {
                        Color c = Colors.Red;
                        c.A = s.Color.A;
                        s.Color = c;
                    }
                    return gb;


                default:
                    return new RadialGradientBrush();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

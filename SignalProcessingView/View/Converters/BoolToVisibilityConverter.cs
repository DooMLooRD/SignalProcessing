using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace SignalProcessingView.View.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {

                if ((string)parameter == "Stop")
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
            else
            {
                if ((string)parameter == "Start")
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }


        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

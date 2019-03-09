using SignalProcessingView.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SignalProcessingView.View.Converters
{
    public class IsEnabledMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            foreach (object value in values)
            {
                if(!((TabViewModel)value).TabContent.HasData)
                {
                    return false;
                }
            }
            return true;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("BooleanAndConverter is a OneWay converter.");
        }

    }
}

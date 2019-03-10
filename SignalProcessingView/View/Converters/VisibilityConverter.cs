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
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            string param = (string)parameter;
            string arg = ((string)value).Substring(1, 3);
            switch (arg)
            {
                case "S01":
                    if (param == "A" || param == "T1" || param == "D" || param=="Probkowanie")
                        return Visibility.Visible;
                    break;
                case "S02":
                    if (param == "A" || param == "T1" || param == "D" || param == "Probkowanie")
                        return Visibility.Visible;
                    break;
                case "S03":
                    if (param == "A" || param == "T1" || param == "D" || param == "T" || param == "Probkowanie")
                        return Visibility.Visible;
                    break;
                case "S04":
                    if (param == "A" || param == "T1" || param == "D" || param == "T" || param == "Probkowanie")
                        return Visibility.Visible;
                    break;
                case "S05":
                    if (param == "A" || param == "T1" || param == "D" || param == "T" || param == "Probkowanie")
                        return Visibility.Visible;
                    break;
                case "S06":
                    if (param == "A" || param == "T1" || param == "D" || param == "T" || param == "Kw" || param == "Probkowanie")
                        return Visibility.Visible;
                    break;
                case "S07":
                    if (param == "A" || param == "T1" || param == "D" || param == "T" || param == "Kw" || param == "Probkowanie")
                        return Visibility.Visible;
                    break;
                case "S08":
                    if (param == "A" || param == "T1" || param == "D" || param == "T" || param == "Kw" || param == "Probkowanie")
                        return Visibility.Visible;
                    break;
                case "S09":
                    if (param == "A" || param == "T1" || param == "D" || param == "Ts" || param == "Probkowanie")
                        return Visibility.Visible;
                    break;
                case "S10":
                    if (param == "A" || param == "Ns" || param == "D" || param == "N1"  || param == "F")
                        return Visibility.Visible;
                    break;
                case "S11":
                    if (param == "A" || param == "N1" || param == "D" || param == "F" || param == "P")
                        return Visibility.Visible;
                    break;
            }
            return Visibility.Collapsed;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

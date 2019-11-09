using System;
using System.Windows.Data;

namespace IMSLibrary.UI.Converter
{
    [ValueConversion(typeof(string), typeof(bool))]
    public class BoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return false;
            if (value.ToString() == "Y") return true;
            else return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return "N";
            return (bool)value ? "Y" : "N";
        }
    }
}

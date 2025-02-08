using System;
using System.Globalization;
using System.Windows.Data;

namespace DiveLogApplication.Utilities.TimePickerHelper
{
    public class StringToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue)
                return intValue.ToString(); // Convert int to string for display

            return "0"; // Default value when binding first occurs
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string strValue && int.TryParse(strValue, out int result))
                return result; // Convert string back to int safely

            return 0; // Default value if conversion fails (e.g., empty string)
        }
    }
}

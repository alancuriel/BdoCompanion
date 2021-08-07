using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace uwpUI.Helpers
{
    class BooleanToCollapsedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (targetType == typeof(Visibility) && value is bool)
            {
                return (bool)value ? Visibility.Visible : Visibility.Collapsed;
            }
            throw new FormatException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

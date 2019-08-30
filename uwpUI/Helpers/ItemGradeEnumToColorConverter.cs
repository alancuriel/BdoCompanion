using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uwpUI.Core.Models;
using uwpUI.Services;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace uwpUI.Helpers
{
    public  class ItemGradeEnumToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(targetType == typeof(Brush) && value is ItemGrade)
            {
                switch(value)
                {
                    case ItemGrade.White:
                        if(ThemeSelectorService.Theme == Windows.UI.Xaml.ElementTheme.Default ||
                           ThemeSelectorService.Theme == Windows.UI.Xaml.ElementTheme.Light)
                        {
                            return new SolidColorBrush(Colors.Gray);
                        }
                        else
                        {
                            return new SolidColorBrush(Colors.White);
                        }
                    case ItemGrade.Green :
                        return new SolidColorBrush(Color.FromArgb(255, 95, 243, 105));
                    case ItemGrade.Blue :
                        return new SolidColorBrush(Color.FromArgb(255, 3, 145, 196));
                    case ItemGrade.Yellow :
                        return new SolidColorBrush(Color.FromArgb(255,246,194,50));
                    case ItemGrade.Orange :
                        return new SolidColorBrush(Color.FromArgb(255,211,90,0));
                }

            }

            throw new FormatException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

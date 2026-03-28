using Microsoft.UI.Xaml.Data;
using System;

namespace TeamNut
{
    public class StarConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool isFav = (bool)value;
            return isFav ? "⭐" : "☆";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return false;
        }
    }
}
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace TeamNut
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool boolValue)
            {
                if (parameter?.ToString() == "Inverse")
                {
                    return boolValue ? Visibility.Collapsed : Visibility.Visible;
                }
                return boolValue ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class InverseBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return true;
        }
    }

    public class EmptyStringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool isEmpty = string.IsNullOrWhiteSpace(value?.ToString());

            if (parameter?.ToString() == "Inverse")
            {
                return isEmpty ? Visibility.Collapsed : Visibility.Visible;
            }
            return isEmpty ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class IntGreaterThanZeroToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int intValue)
            {
                var visible = intValue > 0;
                if (parameter?.ToString() == "Inverse") return visible ? Visibility.Collapsed : Visibility.Visible;
                return visible ? Visibility.Visible : Visibility.Collapsed;
            }
            if (value is long longValue)
            {
                var visible = longValue > 0;
                if (parameter?.ToString() == "Inverse") return visible ? Visibility.Collapsed : Visibility.Visible;
                return visible ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

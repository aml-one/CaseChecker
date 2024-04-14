﻿using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CaseChecker.MVVM.Converters;

public class BoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool bol)
        {
            if (bol)
                return Visibility.Visible;
        }

        return Visibility.Hidden;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return false;
    }
}

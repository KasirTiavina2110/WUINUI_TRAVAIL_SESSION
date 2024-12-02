using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace class2
{
    public class IsAdherentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string role = value as string;
            return role == "adherent" ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

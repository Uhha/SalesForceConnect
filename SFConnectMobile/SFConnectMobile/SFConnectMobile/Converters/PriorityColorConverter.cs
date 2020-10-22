using ImplementationModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace SFConnectMobile.Converters
{
    public class PriorityColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() != typeof(Priority)) return null;
            var priority = (Priority)value;
            switch (priority)
            {
                case Priority.Low:
                    return (Color)Application.Current.Resources["Priority.Low"];
                case Priority.Med:
                    return (Color)Application.Current.Resources["Priority.Mid"];
                case Priority.High:
                    return (Color)Application.Current.Resources["Priority.High"];
                case Priority.Critical:
                    return (Color)Application.Current.Resources["Priority.Critical"];
                case Priority.NotApplicable:
                    return (Color)Application.Current.Resources["Priority.NotApplicable"];
                default:
                   // LoggerController.Log("No Color match for Case Priority " + priority.ToString());
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

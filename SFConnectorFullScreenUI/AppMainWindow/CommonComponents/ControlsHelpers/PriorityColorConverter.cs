using ImplementationModel;
using ImplementationModel.SalesForce;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace SFConnectorFullScreenUI
{
    public class PriorityColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() != typeof(Priority)) return null;
            var priority = (Priority) value;
            switch (priority)
            {
                case Priority.Low:
                    return (Brush)Application.Current.MainWindow.FindResource("Priority.Low.Brush");
                case Priority.Med:
                    return (Brush)Application.Current.MainWindow.FindResource("Priority.Mid.Brush");
                case Priority.High:
                    return (Brush)Application.Current.MainWindow.FindResource("Priority.High.Brush");
                case Priority.Critical:
                    return (Brush)Application.Current.MainWindow.FindResource("Priority.Critical.Brush");
                case Priority.NotApplicable:
                    return (Brush)Application.Current.MainWindow.FindResource("Priority.NotApplicable.Brush");
                default:
                    LoggerController.Log("No Color match for Case Priority " + priority.ToString());
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

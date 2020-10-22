using ImplementationModel;
using ImplementationModel.SalesForce;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace SFConnectorFullScreenUI
{
    public class StatusColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() != typeof(SFStatus)) return null;
            var status = value as SFStatus;
            switch (status.Value)
            {
                case SFStatusE.New:
                    return (Brush)Application.Current.MainWindow.FindResource("Status.New.Brush");
                case SFStatusE.InReview:
                    return (Brush)Application.Current.MainWindow.FindResource("Status.InReview.Brush");
                case SFStatusE.Reviewed:
                    return (Brush)Application.Current.MainWindow.FindResource("Status.Reviewed.Brush");
                case SFStatusE.InBacklog:
                    return (Brush)Application.Current.MainWindow.FindResource("Status.InBacklog.Brush");
                case SFStatusE.InProcess:
                    return (Brush)Application.Current.MainWindow.FindResource("Status.InProcess.Brush");
                case SFStatusE.InDevelopment:
                    return (Brush)Application.Current.MainWindow.FindResource("Status.InDevelopment.Brush");
                case SFStatusE.InQA:
                    return (Brush)Application.Current.MainWindow.FindResource("Status.InQA.Brush");
                case SFStatusE.AwaitingApproval:
                    return (Brush)Application.Current.MainWindow.FindResource("Status.AwaitingApproval.Brush");
                case SFStatusE.PendingRelease:
                    return (Brush)Application.Current.MainWindow.FindResource("Status.PendingRelease.Brush");
                case SFStatusE.OnHold:
                    return (Brush)Application.Current.MainWindow.FindResource("Status.OnHold.Brush");
                case SFStatusE.Closed:
                    return (Brush)Application.Current.MainWindow.FindResource("Status.Closed.Brush");
                case SFStatusE.Cancelled:
                    return (Brush)Application.Current.MainWindow.FindResource("Status.Cancelled.Brush");
                case SFStatusE.ReOpened:
                    return (Brush)Application.Current.MainWindow.FindResource("Status.ReOpened.Brush");
                default:
                    LoggerController.Log("No Color match for Case Status " + status.Value.ToString());
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

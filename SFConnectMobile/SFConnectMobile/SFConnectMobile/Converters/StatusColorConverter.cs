using ImplementationModel;
using ImplementationModel.SalesForce;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace SFConnectMobile.Converters
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
                    return (Color)Application.Current.Resources["Status.New"];
                case SFStatusE.InReview:
                    return (Color)Application.Current.Resources["Status.InReview"];
                case SFStatusE.Reviewed:
                    return (Color)Application.Current.Resources["Status.Reviewed"];
                case SFStatusE.InBacklog:
                    return (Color)Application.Current.Resources["Status.InBacklog"];
                case SFStatusE.InProcess:
                    return (Color)Application.Current.Resources["Status.InProcess"];
                case SFStatusE.InDevelopment:
                    return (Color)Application.Current.Resources["Status.InDevelopment"];
                case SFStatusE.InQA:
                    return (Color)Application.Current.Resources["Status.InQA"];
                case SFStatusE.AwaitingApproval:
                    return (Color)Application.Current.Resources["Status.AwaitingApproval"];
                case SFStatusE.PendingRelease:
                    return (Color)Application.Current.Resources["Status.PendingRelease"];
                case SFStatusE.OnHold:
                    return (Color)Application.Current.Resources["Status.OnHold"];
                case SFStatusE.Closed:
                    return (Color)Application.Current.Resources["Status.Closed"];
                case SFStatusE.Cancelled:
                    return (Color)Application.Current.Resources["Status.Cancelled"];
                case SFStatusE.ReOpened:
                    return (Color)Application.Current.Resources["Status.ReOpened"];
                default:
                   //LoggerController.Log("No Color match for Case Status " + status.Value.ToString());
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace SFConnectMobile.Converters
{
    public class QueueNameToGlyphConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string)) return "";
            var name = value as string;
            switch (name)
            {
                case "New and Critical":
                    return FontAwesome.FontAwesomeIcons.ExclamationTriangle;
                case "Reviewed":
                    return FontAwesome.FontAwesomeIcons.Eye;
                case "Backlog":
                    return FontAwesome.FontAwesomeIcons.Box;
                case "Development":
                    return "\uf7d9";
                case "QA / Approval / Release":
                    return FontAwesome.FontAwesomeIcons.RulerCombined;
                case "On Hold":
                    return FontAwesome.FontAwesomeIcons.StopCircle;
                case "Closed":
                    return FontAwesome.FontAwesomeIcons.TimesCircle;
                case "Review Financial Ops":
                    return FontAwesome.FontAwesomeIcons.PiggyBank;
                case "Review Client Services":
                    return FontAwesome.FontAwesomeIcons.PhoneSquare;
                case "Review Implementations":
                    return FontAwesome.FontAwesomeIcons.UserCog;
                case "Review Tax and Legal":
                    return FontAwesome.FontAwesomeIcons.Landmark;
                default:
                    return FontAwesome.FontAwesomeIcons.DotCircle;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using ImplementationModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace SFConnectorFullScreenUI
{
    public class AttachmentTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value is AttachmentType && !string.IsNullOrEmpty(parameter as string))
            {
                var param = parameter as string;
                switch ((AttachmentType)value)
                {
                    case AttachmentType.Excel:
                        if (param == "excel") { return Visibility.Visible; }
                        break;
                    case AttachmentType.Word:
                        if (param == "word") { return Visibility.Visible; }
                        break;
                    case AttachmentType.Text:
                        if (param == "text") { return Visibility.Visible; }
                        break;
                    case AttachmentType.Message:
                        if (param == "message") { return Visibility.Visible; }
                        break;
                    case AttachmentType.Pdf:
                        if (param == "pdf") { return Visibility.Visible; }
                        break;
                    case AttachmentType.Other:
                        if (param == "other") { return Visibility.Visible; }
                        break;
                    default:
                        return Visibility.Hidden; 
                }
            }
            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("ConvertBack is not Implemented.");
        }
    }
}

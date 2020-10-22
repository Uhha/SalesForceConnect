using ImplementationModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace SFConnectMobile.Converters
{
    public class FileExtensionToGlyphConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is AttachmentType)) return "";
            AttachmentType type = (AttachmentType) value;

            switch (type)
            {
                case AttachmentType.Excel:
                    return FontAwesome.FontAwesomeIcons.FileExcel;
                case AttachmentType.Word:
                    return FontAwesome.FontAwesomeIcons.FileWord;
                case AttachmentType.Text:
                    return FontAwesome.FontAwesomeIcons.FileAlt;
                case AttachmentType.Message:
                    return FontAwesome.FontAwesomeIcons.EnvelopeOpenText;
                case AttachmentType.Pdf:
                    return FontAwesome.FontAwesomeIcons.FilePdf;
                case AttachmentType.Image:
                    return FontAwesome.FontAwesomeIcons.FileImage;
                case AttachmentType.Other:
                    return FontAwesome.FontAwesomeIcons.FileAlt;
                default:
                    return FontAwesome.FontAwesomeIcons.FileAlt;

            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

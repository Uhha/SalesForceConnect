using ImplementationModel;
using ImplementationModel.SalesForce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SFConnectMobile.Views.WebViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomWebPage : ContentPage
    {
        public bool WebViewVisible { get; set; }
        public bool PictureVisible { get; set; }

        public CustomWebPage(string filepath, SFAttachment attachment)
        {
            InitializeComponent();
            switch (attachment.AttachmentType)
            {
                case AttachmentType.Excel:
                    break;
                case AttachmentType.Word:
                    break;
                case AttachmentType.Text:
                    break;
                case AttachmentType.Message:
                    break;
                case AttachmentType.Pdf:
                    customWebView.Uri = filepath;
                    WebViewVisible = true;
                    break;
                case AttachmentType.Image:
                    picture.Source = filepath;
                    PictureVisible = true;
                    break;
                case AttachmentType.Other:
                    break;
                default:
                    break;
            }
            
        }
    }
}
using SFLink;
using ImplementationModel;
using ImplementationModel.SalesForce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SFConnectorUI
{
    /// <summary>
    /// Interaction logic for TestControl.xaml
    /// </summary>
    public partial class AttachmentButton : UserControl
    {
        public Storyboard ShowAttachment;
        public Storyboard CloseAttachment;

        public AttachmentButton(IAttachment attachment, Thickness margin)
        {
            InitializeComponent();
            this.Margin = margin;
            this.Tag = attachment;
            this.ToolTip = ((SFAttachment)attachment).Name;
            switch (((SFAttachment)attachment).AttachmentType)
            {
                case SFAttachmentType.Excel:
                    excel.Visibility = Visibility.Visible;
                    break;
                case SFAttachmentType.Word:
                    word.Visibility = Visibility.Visible;
                    break;
                case SFAttachmentType.Text:
                    text.Visibility = Visibility.Visible;
                    break;
                case SFAttachmentType.Message:
                    message.Visibility = Visibility.Visible;
                    break;
                case SFAttachmentType.Pdf:
                    pdf.Visibility = Visibility.Visible;
                    break;
                case SFAttachmentType.Other:
                    other.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }

            ShowAttachment = FindResource("ShowAttachment") as Storyboard;
            CloseAttachment = FindResource("CloseAttachment") as Storyboard; 
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SFConnector.Instance.DownloadAndOpenAttachment((sender as UserControl).Tag as SFAttachment);
        }
    }
}

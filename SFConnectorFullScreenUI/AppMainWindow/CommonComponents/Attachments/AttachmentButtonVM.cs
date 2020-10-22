using ImplementationModel;
using ImplementationModel.SalesForce;
using SFLink;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFConnectorFullScreenUI
{
    public class AttachmentButtonVM : BaseViewModel
    {
        public IAttachment AttachmentModel { get; set; }
        public IAsyncCommand DownloadAndOpen { get; set; }

        public AttachmentButtonVM(IAttachment attachment)
        {
            AttachmentModel = attachment;
            DownloadAndOpen = new AsyncCommand(DoDownloadAndOpen, errorHandler: new RelayCommandErrorHandler());
        }

        private async Task DoDownloadAndOpen()
        {
            await Task.Run(() => SFConnector.Instance.DownloadAndOpenAttachment(AttachmentModel));
        }
    }
}

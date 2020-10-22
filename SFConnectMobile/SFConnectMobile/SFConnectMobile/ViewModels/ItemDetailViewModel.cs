using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ImplementationModel.SalesForce;
using SFConnectMobile.Base;
using SFConnectMobile.Interfaces;
using SFLink;
using Xamarin.Forms;

namespace SFConnectMobile.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public SFCase Item { get; set; }
        public IAsyncCommand BuildAttachments { get; set; }
        public IAsyncCommand BuildComments { get; set; }
        public Command<SFAttachment> DownloadAndOpen { get; set; }

        private bool _attachmentVisible;
        public bool AttachmentVisible {
            get { return _attachmentVisible; }
            set { SetProperty<bool>(ref _attachmentVisible, value); }
        }

        private bool _commentsVisible;
        public bool CommentsVisible
        {
            get { return _commentsVisible; }
            set { SetProperty<bool>(ref _commentsVisible, value); }
        }
     

        public string Priority { get; set; }
        public string Status { get; set; }
        public string SubStatus { get; set; }
        public string CreatedBy { get; set; }
        public string Date { get; set; }
        public string CaseOwner { get; set; }

        public ItemDetailViewModel(SFCase item = null)
        {
            Title = item?.CaseNumber;
            Priority = "Priority: " + item.Priority.ToString();
            Status = "Status: " + ((SFStatus) item.Status).Value.ToString();
            SubStatus = "SubStatus: " + item.SubStatusText;
            CreatedBy = "Created By: " + item.CreatedBy.GetFormattedName();
            Date = "Date: " + item.CreatedDate.ToString();
            CaseOwner = "Case Owner: " + item.CaseOwner.GetFormattedName();

            Item = item;
            BuildAttachments = new AsyncCommand(DoBuildAttachments);
            BuildComments = new AsyncCommand(DoBuildComments);
            DownloadAndOpen = new Command<SFAttachment>(async (SFAttachment attachment) => await DoDownloadAndOpen(attachment));

        }

        
        private async Task DoDownloadAndOpen(SFAttachment attachment)
        {
            await DependencyService.Get<IDonwloadAttachment>().DoDownloadAndOpen(attachment);
        }

        public async Task<string> DownloadAndGetURI(SFAttachment attachment)
        {
            return await DependencyService.Get<IDonwloadAttachment>().DownloadAndGetURI(attachment);
        }

        private async Task DoBuildAttachments()
        {
            try
            {
                Item.Attachments = await Task.Run(async () => await SFConnector.Instance.GetAttachmentsAsync(Item.Id));
            }
            catch (Exception)
            {
                throw;
            }
            AttachmentVisible = (Item.Attachments.Count > 0) ? true : false;
        }

        private async Task DoBuildComments()
        {
            try
            {
                Item.Comments = await Task.Run(async () => await SFConnector.Instance.GetCaseComments(Item.Id));
            }
            catch (Exception)
            {
                throw;
            }
            CommentsVisible = (Item.Comments.Count > 0) ? true : false;
        }
    }
}

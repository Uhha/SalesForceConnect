using ImplementationModel;
using ImplementationModel.SalesForce;
using PropertyChanged;
using SFLink;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFConnectorFullScreenUI
{
    public class CaseControlDetailsVM : BaseViewModel
    {
        public ICase CaseModel { get; set; }
        public string CaseCreator { get; set; }
        public string CaseOwner { get; set; }
        public string SubStatus { get; set; }
        public ObservableCollection<AttachmentButtonVM> Attachments { get; set; }
        public bool AttachmentsVisible { get; set; }

        public IAsyncCommand BuildAttachments { get; set; }

        public CaseControlDetailsVM(ICase caseModel)
        {
            CaseModel = caseModel;
            CaseCreator = caseModel.CreatedBy?.GetFormattedName();
            CaseOwner = caseModel.CaseOwner?.GetFormattedName();
            BuildAttachments = new AsyncCommand(DoBuildAttachments);
            BuildAttachments.ExecuteAsync().FireAndForgetSafeAsync(new RelayCommandErrorHandler());

            
            if (caseModel.GetType() == typeof(SFCase))
            {
                var enumValue = (caseModel as SFCase).SubStatus;
                SubStatus = ControlsHelper.GetDescription(enumValue);
            }
        }

        private async Task DoBuildAttachments()
        {
            CaseModel.Attachments = await Task.Run(() => SFConnector.Instance.GetAttachmentsAsync(CaseModel.Id));
            if (CaseModel.Attachments.Count > 0) AttachmentsVisible = true;

            Attachments = new ObservableCollection<AttachmentButtonVM>();
            foreach (var attachmentModel in CaseModel.Attachments)
            {
                Attachments.Add(new AttachmentButtonVM(attachmentModel));
            }
        }

        private string _tempDescription;
        private string _tempSubject;
        internal void HighlightCaseText(string text)
        {
            RevertSearchedTextToOriginal();
            _tempDescription = CaseModel.Description;
            _tempSubject = CaseModel.Subject;
            CaseModel.Description = CaseModel.Description.Replace(text, "|~S~|" + text + "|~E~|");
            CaseModel.Subject = CaseModel.Subject.Replace(text, "|~S~|" + text + "|~E~|");
        }

        internal void RevertSearchedTextToOriginal()
        {
            if (_tempDescription != null)
            {
                CaseModel.Description = _tempDescription;
            }
            if (_tempSubject != null)
            {
                CaseModel.Subject = _tempSubject;
            }
        }
    }
}

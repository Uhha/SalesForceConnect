using ImplementationModel.SalesForce;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ImplementationModel
{
    public interface IConnector
    {
        Task<IList<ICase>> GetCasesAsync(string casesQueue);
        Task<IList<ICase>> GetCasesFromArrayAsync(string[] casesArray);
        Task<IList<IAttachment>> GetAttachmentsAsync(string caseNo);
        void DownloadAndOpenAttachment(IAttachment attachment);
        Task<ICase> GetTheCase(string trimmedCaseNumber);
        Task<string[]> GetCasesQueues();
        Task<IList<SFComment>> GetCaseComments(string caseId);
        
    }

    
}

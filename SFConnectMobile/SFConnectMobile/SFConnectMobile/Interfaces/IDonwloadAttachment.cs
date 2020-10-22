using ImplementationModel.SalesForce;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SFConnectMobile.Interfaces
{
    public interface IDonwloadAttachment
    {
        Task DoDownloadAndOpen(SFAttachment attachment);
        Task<string> DownloadAndGetURI(SFAttachment attachment);
    }
}

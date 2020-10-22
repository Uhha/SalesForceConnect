using System;
using System.Collections.Generic;
using System.Text;

namespace ImplementationModel
{
    public interface ICase
    {
        CaseType CaseType { get; set; }
        string Id { get; set; }
        string CaseNumber { get; set; }
        string Subject { get; set; }
        string Description { get; set; }
        IPerson CreatedBy { get; set; }
        IPerson CaseOwner { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime? ClosedDate { get; set; }
        Priority Priority { get; set; }
        IStatus Status { get; set; }

        
        IList<IAttachment> Attachments { get; set; }

    }
}

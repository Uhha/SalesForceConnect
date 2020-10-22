using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ImplementationModel.SalesForce
{
    

    public class SFCase : BaseNotifyingModel, ICase
    {
        public CaseType CaseType { get; set; }
        public string Id { get; set; }
        public string CaseNumber { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public IStatus Status { get; set; }


        public string RecordTypeId { get; set; }
        public SFRecordType RecordType { get; set; }

        public SFSubStatus SubStatus { get; set; }

        public string SubStatusText => SubStatus
            .GetType()
            .GetMember(SubStatus.ToString())
            .FirstOrDefault()
            ?.GetCustomAttribute<DescriptionAttribute>()
            ?.Description;

        public IPerson CaseOwner { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public string CreatedById { get; set; }
        public IPerson CreatedBy { get; set; }
        public Priority Priority { get; set; }
        public IList<IAttachment> Attachments { get; set; }
        public IList<SFComment> Comments { get; set; }

        public SFCase()
        {
            Attachments = new List<IAttachment>();
        }

        [JsonConstructor]
        public SFCase(SFStatus status, SFPerson caseOwner, SFPerson createdBy, List<SFAttachment> attachments, List<SFComment> comments)
        {
            Status = status;
            CaseOwner = caseOwner;
            CreatedBy = createdBy;
            Attachments = new List<IAttachment>();
            Comments = comments;
        }
        //public string Screenshots_and_Images__c { get; set; }
    }

   

    

}

using ImplementationModel;
using ImplementationModel.SalesForce;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFLink
{
    internal class CaseDataHolder
    {
        public string Id { get; set; }
        public string CaseNumber { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }

        public string Status { get; set; }
        public string Type { get; set; }

        public string Priority { get; set; }

        public string RecordTypeId { get; set; }
        public SFRecordType RecordType { get; set; }

        public string Sub_Status__c { get; set; }
        
        public SFPerson Owner { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public string CreatedById { get; set; }
        public SFPerson CreatedBy { get; set; }

        //public string Screenshots_and_Images__c { get; set; }


        public SFCase Convert()
        {
            SFStatus status = new SFStatus();
            switch (this.Status)
            {
                case "New":
                    status.Value = SFStatusE.New;
                    break;
                case "In Review":
                    status.Value = SFStatusE.InReview;
                    break;
                case "Reviewed":
                    status.Value = SFStatusE.Reviewed;
                    break;
                case "In Backlog":
                    status.Value = SFStatusE.InBacklog;
                    break;
                case "In Process":
                    status.Value = SFStatusE.InProcess;
                    break;
                case "In Development":
                    status.Value = SFStatusE.InDevelopment;
                    break;
                case "In QA":
                    status.Value = SFStatusE.InQA;
                    break;
                case "Awaiting Approval":
                    status.Value = SFStatusE.AwaitingApproval;
                    break;
                case "Pending Release":
                    status.Value = SFStatusE.PendingRelease;
                    break;
                case "On Hold":
                    status.Value = SFStatusE.OnHold;
                    break;
                case "Closed":
                    status.Value = SFStatusE.Closed;
                    break;
                case "Cancelled":
                    status.Value = SFStatusE.Cancelled;
                    break;
                case "ReOpened":
                    status.Value = SFStatusE.ReOpened;
                    break;
                default:
                    break;
            }

            SFSubStatus substatus = SFSubStatus.Unknown;
            switch (Sub_Status__c)
            {
                case "New":
                    substatus = SFSubStatus.New;
                    break;
                case "In Review / Research":
                    substatus = SFSubStatus.InReviewResearch;
                    break;
                case "Dev/PCA Reviewed":
                    substatus = SFSubStatus.DevPcaReviewed;
                    break;
                case "In Dev Backlog":
                    substatus = SFSubStatus.InDevBacklog;
                    break;
                case "In Support Backlog":
                    substatus = SFSubStatus.InSupportBacklog;
                    break;
                case "Backlog Reviewed":
                    substatus = SFSubStatus.BacklogReviewed;
                    break;
                case "Under Sprint Consideration":
                    substatus = SFSubStatus.UnderSprintConsideration;
                    break;
                case "In Process":
                    substatus = SFSubStatus.InProcess;
                    break;
                case "In Sprint":
                    substatus = SFSubStatus.InSprint;
                    break;
                case "Pending Environment":
                    substatus = SFSubStatus.PendingEnvironment;
                    break;
                case "User Varification":
                    substatus = SFSubStatus.UserVarification;
                    break;
                case "In QA":
                    substatus = SFSubStatus.InQA;
                    break;
                case "Pending Release":
                    substatus = SFSubStatus.PendingRelease;
                    break;
                case "Scheduled":
                    substatus = SFSubStatus.Scheduled;
                    break;
                case "Waiting for Requestor":
                    substatus = SFSubStatus.WaitingForRequestor;
                    break;
                case "Waiting for Stakeholder":
                    substatus = SFSubStatus.WaitingForStakeholder;
                    break;
                case "Waiting Third Party":
                    substatus = SFSubStatus.WaitingThirdParty;
                    break;
                case "Closed":
                    substatus = SFSubStatus.Closed;
                    break;
                default:
                    break;
            }

            Priority priority = ImplementationModel.Priority.NotApplicable; 
            switch (Priority)
            {
                case "Low":
                    priority = ImplementationModel.Priority.Low;
                    break;
                case "Med":
                    priority = ImplementationModel.Priority.Med;
                    break;
                case "High":
                    priority = ImplementationModel.Priority.High;
                    break;
                case "Critical":
                    priority = ImplementationModel.Priority.Critical;
                    break;
            }

            return new SFCase
            {
                CaseType = CaseType.SF,
                Id = this.Id,
                CaseNumber = this.CaseNumber,
                Subject = this.Subject,
                Description = this.Description,

                Status = status,
                Type = this.Type,
                Priority = priority,
                RecordTypeId = this.RecordTypeId,
                RecordType = this.RecordType,

                SubStatus = substatus,

                CaseOwner = this.Owner,
                CreatedDate = this.CreatedDate,
                ClosedDate = this.ClosedDate,
                CreatedById = this.CreatedById,
                CreatedBy = this.CreatedBy,
            };
        }

        
    }

    
}

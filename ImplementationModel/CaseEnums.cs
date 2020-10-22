using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ImplementationModel
{
    public enum TabIdentifier
    {
        Salesforce,
        TFS,
        SplittedSF,
        SplittedTFS
    }

    public enum ActiveView
    {
        Normal,
        Long
    }

    public enum CaseType
    {
        Unknown,
        SF,
        TFS,
    }

    public enum Priority
    {
        Low,
        Med,
        High,
        Critical,
        NotApplicable
    }

    public enum SFStatusE
    {
        New,
        InReview,
        Reviewed,
        InBacklog,
        InProcess,
        InDevelopment,
        InQA,
        AwaitingApproval,
        PendingRelease,
        OnHold,
        Closed,
        Cancelled,
        ReOpened
    }

    public enum TFSStatusE
    {
        New,
        Active,
        Resolved,
        Closed
    }

    public enum SFSubStatus
    {
        [Description("")]
        UnSet,
        [Description("New")]
        New,
        [Description("In Review Research")]
        InReviewResearch,
        [Description("Dev Pca Reviewed")]
        DevPcaReviewed,
        [Description("In Dev Backlog")]
        InDevBacklog,
        [Description("In Support Backlog")]
        InSupportBacklog,
        [Description("Backlog Reviewed")]
        BacklogReviewed,
        [Description("Under Sprint Consideration")]
        UnderSprintConsideration,
        [Description("In Process")]
        InProcess,
        [Description("In Sprint")]
        InSprint,
        [Description("Pending Environment")]
        PendingEnvironment,
        [Description("User Varification")]
        UserVarification,
        [Description("In QA")]
        InQA,
        [Description("Pending Release")]
        PendingRelease,
        [Description("Scheduled")]
        Scheduled,
        [Description("Waiting For Requestor")]
        WaitingForRequestor,
        [Description("Waiting For Stakeholder")]
        WaitingForStakeholder,
        [Description("Waiting Third Party")]
        WaitingThirdParty,
        [Description("Closed")]
        Closed,
        [Description("Unknown")]
        Unknown
    }

    public enum AttachmentType
    {
        Excel,
        Word,
        Text,
        Message,
        Pdf,
        Image,
        Other
    }

    public enum SFSortByType
    {
        CaseNumber,
        DateOpened,
        PersonOpened
    }
    
    public enum SortBy
    {
        None,
        ID,
        Name,
        DateOpened,
        DateClosed,
    }

    public enum SortButtonState
    {
        Ascending,
        Descending,
        Inactive
    }

    public enum SortBarType
    {
        Horizontal,
        Vertical
    }

    public enum GroupBy
    {
        None,
        ID,
        Name,
        DateOpened,
        DateClosed,
    }

    public enum MenuItemType
    {
        NotSet = -1,
        Search,
        Queue,
        Review,
        Manual,
        Separator,
        StartingPage,
        ManualQueueAddButton
    }

}

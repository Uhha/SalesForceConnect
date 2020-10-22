using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ImplementationModel;
using ImplementationModel.SalesForce;
using NetCoreForce.Client;
using Newtonsoft.Json;

namespace SFLink
{
    public sealed class SFConnectorStub : IConnector
    {
        private static Lazy<SFConnectorStub> lazy = new Lazy<SFConnectorStub>(() => new SFConnectorStub());

        public static IConnector Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        public Task<IList<ICase>> GetCasesAsync(string casesListType)
        {
            string fileName = "";
            switch (casesListType)
            {
                case "New and Critical":
                    fileName = "critQ.json";
                    break;
                case "Reviewed":
                    fileName = "reviewQ.json";
                    break;
                case "Backlog":
                    fileName = "backlogQ.json";
                    break;
                case "Development":
                    fileName = "devQ.json";
                    break;
                case "QA / Approval / Release":
                    fileName = "qaQ.json";
                    break;
                case "On Hold":
                    fileName = "holdQ.json";
                    break;
                case "Closed":
                    fileName = "closedQ.json";
                    break;
                case "Review Financial Ops":
                    fileName = "finopQ.json";
                    break;
                case "Review Client Services":
                    fileName = "csQ.json";
                    break;
                case "Review Implementations":
                    fileName = "implQ.json";
                    break;
                case "Review Tax and Legal":
                    fileName = "tnlQ.json";
                    break;

                case "AllNotClosed":
                    fileName = "reviewQ.json";
                    break;

                default:
                    break;
            }



            IList<ICase> lc = new List<ICase>();
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith(fileName));

                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string result = reader.ReadToEnd();
                    var deobj = JsonConvert.DeserializeObject<IList<SFCase>>(result);
                    lc = deobj.ToList<ICase>();
                    return ConnectionHelper.FormTask(lc);
                }
            }
            catch (Exception e )
            {
                var ms = e.Message;
                var msi = e.InnerException?.Message;
            }

            return null;


        }

        public Task<IList<ICase>> GetCasesFromArrayAsync(string[] casesArray)
        {
            //Thread.Sleep(10000);
            IList<ICase> retList = new List<ICase>();

            foreach (var caseId in casesArray)
            {
                retList.Add(
                    new SFCase ()
                    {
                        Id = caseId,
                        CaseNumber = caseId,
                        Description = "Title " + caseId,
                        CaseOwner = new SFPerson
                        {
                            Id = "asdfew1231d",
                            Name = "Admin Support Queue"
                        },
                        Priority = Priority.Critical,
                        RecordType = new SFRecordType(),
                        RecordTypeId = "qweqwe32113",
                        Status = new ImplementationModel.SalesForce.SFStatus { Value = ImplementationModel.SFStatusE.InDevelopment },
                        Subject = "Subject" + caseId,
                        SubStatus = SFSubStatus.New,
                        Type = "Admin Supp Q Case",
                        CreatedBy = new SFPerson { Name = "Creator Name 2" }
                    }
                );
            }

            return ConnectionHelper.FormTask(retList);
        }

        public Task<IList<IAttachment>> GetAttachmentsAsync(string caseId)
        {
            IList<IAttachment> retlist = new List<IAttachment>();
            if (true)
            {
                retlist = new List<IAttachment>()
                    {
                        new SFAttachment()
                        {
                            Name = "Attachment Excel File",
                            AttachmentType = AttachmentType.Excel
                        },
                        new SFAttachment()
                        {
                            Name = "Attachment Word File",
                            AttachmentType = AttachmentType.Word
                        },
                        new SFAttachment()
                        {
                            Name = "Attachment Message File",
                            AttachmentType = AttachmentType.Message
                        },
                        new SFAttachment()
                        {
                            Name = "Attachment Text File",
                            AttachmentType = AttachmentType.Text
                        },
                        new SFAttachment()
                        {
                            Name = "Attachment Pdf File",
                            AttachmentType = AttachmentType.Pdf
                        },
                        new SFAttachment()
                        {
                            Name = "Attachment Other File",
                            AttachmentType = AttachmentType.Other
                        }
                    };
            }
            return ConnectionHelper.FormTask(retlist);
        }

        public void DownloadAndOpenAttachment(IAttachment attachment)
        {
        }

        public Task<ICase> GetTheCase(string trimmedCaseNumber)
        {
            //Thread.Sleep(2000);
            var sfcase = new SFCase
            {
                Id = "caseId",
                CaseNumber = trimmedCaseNumber,
                Description = "Lorem Ipsum is simplyas!",
                CaseOwner = new SFPerson
                {
                    Id = "asdfew1231d",
                    Name = "Admin Support Queue"
                },
                Priority = Priority.High,
                RecordType = new SFRecordType(),
                RecordTypeId = "qweqwe32113",
                Status = new ImplementationModel.SalesForce.SFStatus { Value = ImplementationModel.SFStatusE.New },
                Subject = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. since the 1500s, when an unknown printer took a galley  the printing and typesetting industry. since the 1500s, when an unknown printer took a galley the printing and typesetting industry. since the 1500s, when an unknown printer took a galley the printing and typesetting industry. since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset!",
                SubStatus = SFSubStatus.InSprint,
                Type = "Admin Supp Q Case"

            };

            return ConnectionHelper.FormTask<ICase>(sfcase);
        }



        public Task<string[]> GetCasesQueues()
        {
            //Thread.Sleep(3000);
            var queue = new string[]
            {
                "New and Critical",
                "Reviewed",
                "Backlog",
                "Development",
                "QA / Approval / Release",
                "On Hold",
                "Closed",
                "Review Financial Ops",
                "Review Client Services",
                "Review Implementations",
                "Review Tax and Legal"
            };
            return ConnectionHelper.FormTask(queue);
        }


        private static string RandomStrings(int countWords, Random rng)
        {
            const string allowedChars =
                "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz#@$^*()";
            var minLength = 4;
            var maxLength = 14;
            char[] chars = new char[maxLength];
            int setLength = allowedChars.Length;

            StringBuilder sb = new StringBuilder();

            while (countWords-- > 0)
            {
                int length = rng.Next(minLength, maxLength + 1);

                for (int i = 0; i < length; ++i)
                {
                    chars[i] = allowedChars[rng.Next(setLength)];
                }

                sb.Append(new string(chars, 0, length));
            }
            return sb.ToString();
        }

        public async Task<IList<SFComment>> GetCaseComments(string caseId)
        {
            IList<SFComment> sfCaseComments = new List<SFComment>();
                    SFPerson creator = new SFPerson { Id = "1", Name = "Alex Smith" };
                    sfCaseComments.Add(new SFComment() { ID = "2", CommentBody = "I don't know, but one of them Includes Negative Interest and the other one doesn't.", Creator = creator, DtCreated = "2019-01-01" });
                    sfCaseComments.Add(new SFComment() { ID = "3", CommentBody = "Currently there is a filter by region but would also like to have a filter by PCA. I would like it in all of the tabs for transfers to be sent and instructions sent awaiting transfer. thanks", Creator = creator, DtCreated = "2019-01-01" });
            return await ConnectionHelper.FormTask(sfCaseComments);
        }
    }
}

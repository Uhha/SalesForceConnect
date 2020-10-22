using GlobalConfiguration;
using ImplementationModel;
using ImplementationModel.SalesForce;
using NetCoreForce.Client;
using NetCoreForce.Client.Models;
using NetCoreForce.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFLink
{
    public sealed class SFConnector : IConnector
    {
        private static readonly string IsRelease = ConfigManager.IsRelease;
        private static bool Encrypt => bool.Parse(ConfigManager.Encrypt);
        private static readonly Lazy<IConnector> lazy = new Lazy<IConnector>(() => ConnectorPicker());

        private static IConnector ConnectorPicker()
        {
            bool isRelease = bool.Parse(IsRelease);
            if (isRelease)
            {
                return new SFConnector();
            }
            else
            {
                return new SFConnectorStub();
            }
        }

        public static IConnector Instance
        {
            get
            {
                return lazy.Value;
            }
        }


        private SFConnector() { }

        private readonly string SecurityToken = ConfigManager.SecurityToken;
        private readonly string ConsumerKey = ConfigManager.ConsumerKey;
        private readonly string ConsumerSecret = ConfigManager.ConsumerSecret;
        private readonly string Username = ConfigManager.Username;
        private readonly string Password = ConfigManager.Password +
            ConfigManager.SecurityToken;
        private readonly string IsSandboxUser = ConfigManager.IsSandboxUser;

        private ForceClient _client;

        private async Task<ForceClient> GetForceClient()
        {
            if (_client != null) return _client;

            var auth = new AuthenticationClient();
            
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            // Use SecurityProtocolType.Ssl3 if needed for compatibility reasons

            var url = IsSandboxUser.Equals("true", StringComparison.CurrentCultureIgnoreCase)
                ? "https://test.salesforce.com/services/oauth2/token"
                : "https://login.salesforce.com/services/oauth2/token";

            try
            {
                await auth.UsernamePasswordAsync(ConsumerKey, ConsumerSecret, Username, Password);
            }
            catch (Exception e)
            {
                var mm = e.Message;
                throw;
            }

            _client = new ForceClient(auth.AccessInfo.InstanceUrl, auth.ApiVersion, auth.AccessInfo.AccessToken);
            return _client;
        }

        public async Task<IList<ICase>> GetCasesAsync(string casesQueue)
        {
            string qry = FormQuery(casesQueue);
            if (string.IsNullOrEmpty(qry)) return new List<ICase>();

            IAsyncEnumerable<CaseDataHolder> result;
            try
            {
                var client = await GetForceClient();
                result = client.QueryAsync<CaseDataHolder>(qry);
            }
            catch (Exception e)
            {
                var ms = e.Message;
                throw;
            }

            IList<ICase> cases = new List<ICase>();
            using (IAsyncEnumerator<CaseDataHolder> contactsEnumerator = result.GetEnumerator())
            {
                // MoveNext() will execute the query and get the first batch of results.
                // Once the inital result batch has been exhausted, the remaining batches, if any, will be retrieved.
                while (await contactsEnumerator.MoveNext())
                {
                    CaseDataHolder acase = contactsEnumerator.Current;
                    cases.Add(acase.Convert());
                }
            }
            //var obj = JsonConvert.SerializeObject(cases);
            //Debug.WriteLine(obj);
            if (Encrypt) HideSensitive(cases);
            return cases;
        }

        private void HideSensitive(IList<ICase> ls)
        {
            string Transform(string t) => t.Replace('a', 'e').Replace('i', 'a').Replace('y', 'o').Replace('s', 'y')
                .Replace('n', 'k').Replace('k', 'x').Replace('h', 'g').Replace('d', 'r').Replace('x', 'd')
                .Replace('l', 't').Replace('r', 'l').Replace('v', 'z').Replace('b', 'v').Replace('u', 'i');

            foreach (var cs in ls)
            {
                cs.CaseOwner.Name = Transform(cs.CaseOwner.Name);
                cs.CreatedBy.Name = Transform(cs.CreatedBy.Name);
                cs.Subject = Transform(cs.Subject);
                cs.Description = Transform(cs.Description);
            }
        }

        private string FormQuery(string casesQueue)
        {
            if (string.IsNullOrEmpty(casesQueue)) return null; 
            string filter = "";
            switch (casesQueue)
            {
                case "New and Critical":
                    filter = @" WHERE RecordType.Name = 'A: Application Support & Development' 
                                    and (Owner.Name = 'Tatyana Ostromogilskaya' or Owner.Name = 'Alex Svinarenka' 
                                        or Owner.Name = 'ADMIN Support Queue')
                                    and (status in ('New', 'Re-opened') or (priority = 'Critical' and status <> 'Closed')) 
                                ";
                    break;
                case "Reviewed":
                    filter = @" WHERE RecordType.Name = 'A: Application Support & Development' 
                                    and (Owner.Name = 'Tatyana Ostromogilskaya' or Owner.Name = 'Alex Svinarenka' 
                                        or Owner.Name = 'ADMIN Support Queue')
                                    and status <> 'Closed'
                                    and status in ('In Review', 'Reviewed', 'In Process')
                                ";
                    break;
                case "Backlog":
                    filter = @" WHERE RecordType.Name = 'A: Application Support & Development' 
                                    and (Owner.Name = 'Tatyana Ostromogilskaya' or Owner.Name = 'Alex Svinarenka' 
                                        or Owner.Name = 'ADMIN Support Queue')
                                    and status <> 'Closed'
                                    and status in ('In Backlog')
                                ";
                    break;
                case "Development":
                    filter = @" WHERE RecordType.Name = 'A: Application Support & Development' 
                                    and (Owner.Name = 'Tatyana Ostromogilskaya' or Owner.Name = 'Alex Svinarenka' 
                                        or Owner.Name = 'ADMIN Support Queue')
                                    and status <> 'Closed'
                                    and status in ('In Development')
                                ";
                    break;
                case "QA / Approval / Release":
                    filter = @" WHERE RecordType.Name = 'A: Application Support & Development' 
                                    and (Owner.Name = 'Tatyana Ostromogilskaya' or Owner.Name = 'Alex Svinarenka' 
                                        or Owner.Name = 'ADMIN Support Queue')
                                    and status <> 'Closed'
                                    and status in ('In QA', 'Awaiting Approval', 'Pending Release')
                                ";
                    break;
                case "On Hold":
                    filter = @" WHERE RecordType.Name = 'A: Application Support & Development' 
                                    and (Owner.Name = 'Tatyana Ostromogilskaya' or Owner.Name = 'Alex Svinarenka' 
                                        or Owner.Name = 'ADMIN Support Queue')
                                    and status <> 'Closed'
                                    and status in ('On Hold')
                                ";
                    break;
                case "Closed":
                    filter = @" WHERE RecordType.Name = 'A: Application Support & Development' 
                                    and (Owner.Name = 'Tatyana Ostromogilskaya' or Owner.Name = 'Alex Svinarenka' 
                                        or Owner.Name = 'ADMIN Support Queue')
                                    and status = 'Closed'
                                ";
                    break;
                case "Review Financial Ops":
                    filter = @"WHERE status <> 'Closed' 
                                    and RecordType.Name = 'A: Application Support & Development' 
                                    and System__c = 'FS ADMIN'
                                    and Origin = 'Financial Ops'
                                ";
                    break;
                case "Review Client Services":
                    filter = @"WHERE status <> 'Closed' 
                                    and RecordType.Name = 'A: Application Support & Development' 
                                    and System__c = 'FS ADMIN'
                                    and Origin = 'Client Services'
                                ";
                    break;
                case "Review Implementations":
                    filter = @"WHERE status <> 'Closed' 
                                    and RecordType.Name = 'A: Application Support & Development' 
                                    and System__c = 'FS ADMIN'
                                    and Origin = 'Account Services'
                                ";
                    break;
                case "Review Tax and Legal":
                    filter = @"WHERE status <> 'Closed' 
                                    and RecordType.Name = 'A: Application Support & Development' 
                                    and System__c = 'FS ADMIN'
                                    and Origin = 'Tax & Legal Affairs'
                                ";
                    break;

                case "AllNotClosed":
                    filter = @" WHERE RecordType.Name = 'A: Application Support & Development' 
                                    and (Owner.Name = 'Tatyana Ostromogilskaya' or Owner.Name = 'Alex Svinarenka' 
                                        or Owner.Name = 'ADMIN Support Queue')
                                        and status <> 'Closed'
                                ";
                    break;
                case "All":
                    filter = @" WHERE RecordType.Name = 'A: Application Support & Development' 
                                    and (Owner.Name = 'Tatyana Ostromogilskaya' or Owner.Name = 'Alex Svinarenka' 
                                        or Owner.Name = 'ADMIN Support Queue')
                                ";
                    break;
                default:
                    filter = "where 1 = 0";
                    break;
            }

            string query = @"SELECT Id, CaseNumber, Subject, Description, 
                                    Status, 
                                    Sub_Status__c,
                                    Priority, 
                                    Type, 
                                    RecordTypeId, RecordType.Id, RecordType.Name,
                                    OwnerId, Owner.Id, Owner.Name, 
                                    CreatedDate,
                                    CreatedById, CreatedBy.Id, CreatedBy.Name,
                                    ClosedDate 
                                FROM CASE " + filter;
            return query;
        }

        public async Task<IList<ICase>> GetCasesFromArrayAsync(string[] casesArray)
        {
            string filter = "WHERE CaseNumber in " + "('" + string.Join("','", casesArray) + "')";
            string qry = @"SELECT Id, CaseNumber, Subject, Description, 
                                    Status, 
                                    Sub_Status__c,
                                    Priority, 
                                    Type, 
                                    RecordTypeId, RecordType.Id, RecordType.Name,
                                    OwnerId, Owner.Id, Owner.Name, 
                                    CreatedDate,
                                    CreatedById, CreatedBy.Id, CreatedBy.Name,
                                    ClosedDate 
                                FROM CASE " + filter;


            if (string.IsNullOrEmpty(qry)) return new List<ICase>();

            IAsyncEnumerable<CaseDataHolder> result;
            try
            {
                var client = await GetForceClient();
                result = client.QueryAsync<CaseDataHolder>(qry);
            }
            catch (Exception)
            {
                throw;
            }

            IList<ICase> cases = new List<ICase>();
            using (IAsyncEnumerator<CaseDataHolder> contactsEnumerator = result.GetEnumerator())
            {
                // MoveNext() will execute the query and get the first batch of results.
                // Once the inital result batch has been exhausted, the remaining batches, if any, will be retrieved.
                while (await contactsEnumerator.MoveNext())
                {
                    CaseDataHolder acase = contactsEnumerator.Current;
                    cases.Add(acase.Convert());
                }
            }
            if (Encrypt) HideSensitive(cases);
            return cases;
        }


        public async Task<ICase> GetTheCase(string trimmedCaseNumber)
        {
            var caseNumber = trimmedCaseNumber.PadLeft(8, '0');
            CaseDataHolder result;
            try
            {
                string qry = @"SELECT Id, CaseNumber, Subject, Description, 
                                    Status, 
                                    Sub_Status__c,
                                    Priority, 
                                    Type, 
                                    RecordTypeId, RecordType.Id, RecordType.Name,
                                    OwnerId, Owner.Id, Owner.Name, 
                                    CreatedDate,
                                    CreatedById, CreatedBy.Id, CreatedBy.Name
                                FROM CASE 
                                WHERE CaseNumber = '" + caseNumber + "'";
                var client = await GetForceClient();
                result = await client.QuerySingle<CaseDataHolder>(qry);
                return result.Convert();

            }
            catch (Exception)
            {
                throw;
            }
        }



        public async Task<IList<IAttachment>> GetAttachmentsAsync(string caseId)
        {
            IAsyncEnumerable<SFAttachment> result;
            try
            {
                string qry = @"SELECT Id, ParentId, Name, ContentType, 
                    Owner.Id, Owner.Name, 
                    CreatedBy.Id, CreatedBy.Name,
                    Description,
                    BodyLength, Body 
                    FROM Attachment where Attachment.ParentId = '" + caseId + "'";

                var client = await GetForceClient();
                result = client.QueryAsync<SFAttachment>(qry);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

            IList<IAttachment> attachments = new List<IAttachment>();
            using (IAsyncEnumerator<SFAttachment> contactsEnumerator = result.GetEnumerator())
            {
                // MoveNext() will execute the query and get the first batch of results.
                // Once the inital result batch has been exhausted, the remaining batches, if any, will be retrieved.
                while (await contactsEnumerator.MoveNext())
                {
                    SFAttachment attachment = contactsEnumerator.Current;
                    attachments.Add(attachment);
                }
            }
            return attachments;
        }

        public async void DownloadAndOpenAttachment(IAttachment attachment)
        {
            if (attachment.GetType() != typeof(SFAttachment)) return;
            var sfattachment = (SFAttachment)attachment;
            var client = await GetForceClient();
            string fileName = sfattachment.Name;
            string bodyUrl = client.InstanceUrl + sfattachment.Body;
            
            HttpWebRequest aRequest = (HttpWebRequest)WebRequest.Create(bodyUrl);
            aRequest.Headers.Add("Authorization: Bearer " + client.AccessToken);
            aRequest.Method = "GET";

            HttpWebResponse aResponse = (HttpWebResponse)aRequest.GetResponse();
           
            byte[] doc = null;
            using (MemoryStream ms = new MemoryStream())
            {
                aResponse.GetResponseStream().CopyTo(ms);
                doc = ms.ToArray();
            }

            var path = AppDomain.CurrentDomain.BaseDirectory;
            if (!Directory.Exists(path + "/temp")) Directory.CreateDirectory(path + "/temp");
            File.WriteAllBytes($"{path}/temp/{fileName}", doc);
            
            System.Diagnostics.Process.Start($"{path}/temp/{fileName}");
        }

        public async Task<byte[]> DownloadAttachment(IAttachment attachment)
        {
            if (attachment.GetType() != typeof(SFAttachment)) return null;
            var sfattachment = (SFAttachment)attachment;
            var client = await GetForceClient();
            string fileName = sfattachment.Name;
            string bodyUrl = client.InstanceUrl + sfattachment.Body;

            HttpWebRequest aRequest = (HttpWebRequest)WebRequest.Create(bodyUrl);
            aRequest.Headers.Add("Authorization: Bearer " + client.AccessToken);
            aRequest.Method = "GET";

            HttpWebResponse aResponse = (HttpWebResponse)aRequest.GetResponse();

            byte[] doc = null;
            using (MemoryStream ms = new MemoryStream())
            {
                aResponse.GetResponseStream().CopyTo(ms);
                doc = ms.ToArray();
            }
            return doc;
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

        public async Task<IList<SFComment>> GetCaseComments(string caseId)
        {
            IAsyncEnumerable<SfCaseComment> result;

            string qry = @"Select Id, ParentId,  IsPublished, CommentBody, CreatedBy.Id, CreatedBy.Name, CreatedDate
                FROM CaseComment
                WHERE ParentId = '" + caseId + "'";

            IList<SFComment> sfCaseComments = new List<SFComment>();
            try
            {
                var client = await (SFConnector.Instance as SFConnector).GetClient();
                result = client.QueryAsync<SfCaseComment>(qry);
            
            
                //CaseComments
                using (IAsyncEnumerator<SfCaseComment> contactsEnumerator = result.GetEnumerator())
                {
                    // MoveNext() will execute the query and get the first batch of results.
                    // Once the inital result batch has been exhausted, the remaining batches, if any, will be retrieved.
                    while (await contactsEnumerator.MoveNext())
                    {
                        SfCaseComment comment = contactsEnumerator.Current;
                        SFPerson creator = new SFPerson { Id = comment.CreatedBy?.Id, Name = comment.CreatedBy?.Name };
                        sfCaseComments.Add(new SFComment() { ID = comment.Id, CommentBody = comment.CommentBody, Creator = creator, DtCreated = comment.CreatedDate.ToString()});
                    }
                }

                //Status Changes
                qry = @"Select Id,  CreatedBy.Id, CreatedBy.Name, CreatedDate, OldValue, NewValue, Field
                    FROM CaseHistory 
                    WHERE Field = 'Status' and CaseId = '" + caseId + "'";

                IAsyncEnumerable<SfCaseHistory> result2;
                result2 = _client.QueryAsync<SfCaseHistory>(qry);
                using (IAsyncEnumerator<SfCaseHistory> contactsEnumerator = result2.GetEnumerator())
                {
                    while (await contactsEnumerator.MoveNext())
                    {
                        SfCaseHistory statusChange = contactsEnumerator.Current;
                        SFPerson creator = new SFPerson { Id = statusChange.CreatedBy?.Id, Name = statusChange.CreatedBy?.Name };
                        var text = $"Status Change from {statusChange.OldValue} to {statusChange.NewValue}.";
                        sfCaseComments.Add(new SFComment() { ID = statusChange.Id, CommentBody = text, Creator = creator, DtCreated = statusChange.CreatedDate.ToString() });
                    }
                }

                //Emails
                IAsyncEnumerable<SfEmailMessage> result3;
                qry = @"SELECT CreatedBy.Id, CreatedBy.FirstName, CreatedBy.LastName, FromName,Id,ParentId,Status,Subject,TextBody, ToAddress 
                        FROM EmailMessage where ParentId = '" + caseId + "'";

                result3 = _client.QueryAsync<SfEmailMessage>(qry);
                using (IAsyncEnumerator<SfEmailMessage> contactsEnumerator = result3.GetEnumerator())
                {
                    while (await contactsEnumerator.MoveNext())
                    {
                        SfEmailMessage email = contactsEnumerator.Current;
                        SFPerson creator = new SFPerson { Id = email.CreatedById, Name = email.CreatedBy.Name };
                        var text = $"From:{email.FromName} To: {email.ToAddress} Subject: {email.Subject}. Body: {email.TextBody}.";
                        sfCaseComments.Add(new SFComment() { ID = email.Id, CommentBody = text, Creator = creator, DtCreated = email.CreatedDate.ToString()});
                    }
                }

            }

            catch (Exception)
            {
                throw;
            }

            return sfCaseComments;
        }


        private async void DownloadAndOpenAttachment()
        {
            var client = await GetForceClient();
            string bodyUrl = "https://foundationsource--c.na32.content.force.com/servlet/rtaImage?eid=5003800000yDp53&amp;feoid=00N50000003V2wk&amp;refid=0EM38000000VNoT";

            //var imageToGet = 'https://c.<instance-id>.content.force.com/servlet/rtaImage?eid=<eid>&feoid=<feoid>&refid=<refid>'

            HttpWebRequest aRequest = (HttpWebRequest)WebRequest.Create(bodyUrl);
            aRequest.Headers.Add("Authorization: Bearer " + client.AccessToken);
            aRequest.Method = "GET";

            HttpWebResponse aResponse = (HttpWebResponse)aRequest.GetResponse();

            byte[] doc = null;
            using (MemoryStream ms = new MemoryStream())
            {
                aResponse.GetResponseStream().CopyTo(ms);
                doc = ms.ToArray();
            }
            var path = AppDomain.CurrentDomain.BaseDirectory;
            if (!Directory.Exists(path + "/temp")) Directory.CreateDirectory(path + "/temp");
            File.WriteAllBytes($"{path}/temp/image.jpg", doc);
        }

        //FOR TESTING
        public Task<ForceClient> GetClient()
        {
            return GetForceClient();
        }
        
    }

    
}

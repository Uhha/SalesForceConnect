using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImplementationModel.SalesForce
{
    public class SFAttachment : IAttachment
    {
        public string Id { get; set; }
        public string ParentId { get; set; }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                var dotIndex = value.LastIndexOf('.');
                if (dotIndex < 0) dotIndex = 0;
                var extension = value.Substring(dotIndex, value.Length - dotIndex);
                switch (extension)
                {
                    case ".xls":
                    case ".xlsx":
                        AttachmentType = AttachmentType.Excel;
                        break;
                    case ".rtf":
                    case ".doc":
                    case ".docx":
                        AttachmentType = AttachmentType.Word;
                        break;
                    case ".msg":
                        AttachmentType = AttachmentType.Message;
                        break;
                    case ".txt":
                    case ".csv":
                    case ".log":
                        AttachmentType = AttachmentType.Text;
                        break;
                    case ".pdf":
                        AttachmentType = AttachmentType.Pdf;
                        break;
                    case ".png":
                    case ".jpg":
                    case ".jpeg":
                    case ".bmp":
                        AttachmentType = AttachmentType.Image;
                        break;
                    default:
                        AttachmentType = AttachmentType.Other;
                        break;
                }

                Extention = extension;
            }
        }

        public string Extention { get; set; }
        public string ContentType { get; set; }
        public int BodyLength { get; set; }
        public string Body { get; set; } //base64
        public SFPerson Owner { get; set; }
        public SFPerson CreatedBy { get; set; }
        public string Description { get; set; }

        public AttachmentType AttachmentType { get; set; }
    }

    
}

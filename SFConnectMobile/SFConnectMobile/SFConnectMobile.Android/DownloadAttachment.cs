using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ImplementationModel.SalesForce;
using SFConnectMobile.Interfaces;
using SFLink;
using Xamarin.Essentials;
using Xamarin.Forms;
using Environment = System.Environment;

[assembly: Dependency(typeof(SFConnectMobile.Droid.DownloadAttachment))]
namespace SFConnectMobile.Droid
{
    public class DownloadAttachment: IDonwloadAttachment
    {
        public async Task<string> DownloadAndGetURI(SFAttachment attachment)
        {
            var array = await Task.Run(() => ((SFConnector)SFConnector.Instance).DownloadAttachment(attachment));

            string directory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            directory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string docName = attachment.Name;
            File.WriteAllBytes(Path.Combine(directory, docName), array);
            return Path.Combine(directory, docName);
        }


        public async Task DoDownloadAndOpen(SFAttachment attachment)
        {
            var array = await Task.Run(() => ((SFConnector)SFConnector.Instance).DownloadAttachment(attachment));

            string directory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            directory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string docName = attachment.Name;
            // In here we save the file but saved file doesn't shown for some reason! Thats why I'm struggling all day.
            File.WriteAllBytes(Path.Combine(directory, docName), array);

            //var bytes = File.ReadAllBytes(Path.Combine(directory, docName));
            //string externalStoragePath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, docName);
            //File.WriteAllBytes(externalStoragePath, bytes);



            //Android.Net.Uri uri = Android.Net.Uri.Parse("file://" + Path.Combine(directory, docName));
            //Intent intent = new Intent(Intent.ActionView);
            //intent.SetDataAndType(uri, "application/" + attachment.Extention.Substring(attachment.Extention.IndexOf('.') + 1));
            //intent.SetFlags(ActivityFlags.NewTask);
            //Android.App.Application.Context.StartActivity(intent);

            await Share(docName, docName, Path.Combine(directory, docName));

            //  Device.OpenUri(new Uri(Path.Combine(directory, docName)));
        }

        private Task Share(string title, string message, string filePath)
        {
            var extension = filePath.Substring(filePath.LastIndexOf(".", StringComparison.InvariantCultureIgnoreCase) + 1).ToLower();
            var contentType = string.Empty;

            Java.IO.File file = new Java.IO.File(filePath);
            Android.Net.Uri apkURI = FileProvider.GetUriForFile(Android.App.Application.Context, Android.App.Application.Context.ApplicationContext.PackageName + ".provider", file);

            switch (extension)
            {
                case ".txt":
                    contentType = "text/plain";
                    break;
                case ".doc":
                case ".docx":
                    contentType = "application/msword";
                    break;
                case ".pdf":
                    contentType = "application/pdf";
                    break;
                case ".xls":
                case ".xlsx":
                    contentType = "application/vnd.ms-excel";
                    break;
                case ".jpg":
                case ".jpeg":
                case ".png":
                    contentType = "image/jpeg";
                    break;
                default:
                    contentType = "*/*";
                    break;
            }

            var intent = new Intent(Intent.ActionSend);

            intent.SetFlags(ActivityFlags.GrantReadUriPermission);

            intent.SetType(contentType);
            intent.PutExtra(Intent.ExtraStream, apkURI);
            intent.PutExtra(Intent.ExtraText, string.Empty);
            intent.PutExtra(Intent.ExtraSubject, message ?? string.Empty);

            var chooserIntent = Intent.CreateChooser(intent, title ?? string.Empty);
            chooserIntent.SetFlags(ActivityFlags.ClearTop);
            chooserIntent.SetFlags(ActivityFlags.NewTask);

            Android.App.Application.Context.StartActivity(chooserIntent);

            return Task.FromResult(true);
        }
    }
}
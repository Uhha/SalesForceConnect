using Android.Content;
using SFConnectMobile.Droid;
using SFConnectMobile.Views.WebViews;
using System.Net;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomWebView), typeof(CustomWebViewRenderer))]
namespace SFConnectMobile.Droid
{
    public class CustomWebViewRenderer : WebViewRenderer
    {
        //protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        //{
        //    base.OnElementChanged(e);

        //    if (e.NewElement != null)
        //    {
        //        var customWebView = Element as CustomWebView;
        //        Control.Settings.AllowUniversalAccessFromFileURLs = true;
        //        Control.LoadUrl(string.Format("file:///android_asset/pdfjs/web/viewer.html?file={0}", string.Format("file:///android_asset/Content/{0}", WebUtility.UrlEncode(customWebView.Uri))));
        //    }
        //}
        public CustomWebViewRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                var pdfView = Element as CustomWebView;
                Control.Settings.AllowFileAccess = true;
                Control.Settings.AllowFileAccessFromFileURLs = true;
                Control.Settings.AllowUniversalAccessFromFileURLs = true;
                Control.LoadUrl(string.Format("file:///android_asset/pdfjs/web/viewer.html?file={0}", string.Format("file://{0}", WebUtility.UrlEncode(pdfView.Uri))));
            }
        }
    }
}

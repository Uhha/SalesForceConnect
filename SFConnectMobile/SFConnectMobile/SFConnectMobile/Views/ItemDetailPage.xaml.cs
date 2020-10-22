using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using SFConnectMobile.Models;
using SFConnectMobile.ViewModels;
using ImplementationModel.SalesForce;
using System.Collections.ObjectModel;
using SFConnectMobile.Views.WebViews;
using SFConnectMobile.Base;
using System.Linq;

namespace SFConnectMobile.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemDetailPage : ContentPage
    {
        private ItemDetailViewModel viewModel;
        private static readonly int HEADER_HEIGHT = 45;

        public ItemDetailPage(ItemDetailViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
            viewModel.BuildAttachments.ExecuteAsync().FireAndForgetSafeAsync(new GeneralErrorHandler());
            viewModel.BuildComments.ExecuteAsync().FireAndForgetSafeAsync(new GeneralErrorHandler());
            CommentsList.ItemTapped += CommentsList_ItemTapped;

            viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "AttachmentVisible" && AttachmentList != null)
            {
                double height = 0;
                var templatedItems = (AttachmentList as ListView).TemplatedItems;
                foreach (var item in templatedItems)
                {
                    height += (item as ViewCell).RenderHeight;
                }
                AttachmentListGrid.HeightRequest = height + HEADER_HEIGHT;
            }


            //if (e.PropertyName == "CommentsVisible" && CommentsList != null)
            //{
            //    double height = 0;
            //    var templatedItems = (CommentsList as ListView).TemplatedItems;
            //    foreach (var item in templatedItems)
            //    {
            //        var vc = (item as ViewCell);
            //        var gr = (vc.View as Grid);


            //        var sl = (gr.Children[0] as StackLayout);
            //        var newmeasure = sl.Measure(double.PositiveInfinity, double.PositiveInfinity, MeasureFlags.IncludeMargins);


            //        vc.ForceUpdateSize();

            //        newmeasure = sl.Measure(double.PositiveInfinity, double.PositiveInfinity, MeasureFlags.IncludeMargins);

            //        var sr = vc.View.Measure(double.PositiveInfinity, double.PositiveInfinity, MeasureFlags.IncludeMargins);
            //        var h5 = vc.View.Height;
            //        var h = (vc.View as Grid).Height;
            //        var h2 = vc.Height;
            //        var h3 = vc.RenderHeight;
            //        var h4 = (vc.View as Grid).HeightRequest;

            //        height += h;
            //    }
            //    CommentsListGrid.HeightRequest = height + HEADER_HEIGHT;
            //}
        }

        private void CommentsList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            if (sender is ListView lv) lv.SelectedItem = null;
        }

        private async void Attachment_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return;

            var item = e.SelectedItem as SFAttachment;
            if (sender is ListView lv) lv.SelectedItem = null;

            if (item == null)
                return;

            // viewModel.DownloadAndOpen.Execute(item);
            var uri = await viewModel.DownloadAndGetURI(item);
            await Navigation.PushAsync(new CustomWebPage(uri, item));

            AttachmentList.SelectedItem = null;
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using SFConnectMobile.Models;
using SFConnectMobile.Views;
using SFConnectMobile.ViewModels;
using ImplementationModel.SalesForce;
using System.IO;
using System.Diagnostics;

namespace SFConnectMobile.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemsPage : ContentPage
    {
        public ItemsViewModel viewModel;
        public Command OpenManualQueuesPage { get; set; }
        private HomeMenuItem _homeMenuItem { get; set; }

        public ItemsPage(HomeMenuItem menuItem)
        {
            InitializeComponent();
            _homeMenuItem = menuItem;
            OpenManualQueuesPage = new Command(DoOpenManualQueuesPage);
            BindingContext = viewModel = new ItemsViewModel(_homeMenuItem);

            if (viewModel?.Items.Count == 0)
                viewModel?.LoadItemsCommand.Execute(null);
        }

        private async void DoOpenManualQueuesPage(object obj)
        {
            SFCase sfCase = obj as SFCase;
            await Navigation.PushAsync(new ManualTabPickerPage(sfCase));
        }

        
    }
}
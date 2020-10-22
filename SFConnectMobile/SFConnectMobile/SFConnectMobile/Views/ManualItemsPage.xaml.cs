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
using SFConnectMobile.Services;

namespace SFConnectMobile.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ManualItemsPage : ContentPage
    {
        public ItemsViewModel viewModel;
        public Command<SFCase> RemoveManualCase { get; set; }
        private HomeMenuItem _homeMenuItem;
        
        public ManualItemsPage(HomeMenuItem homeMenuItem)
        {
            InitializeComponent();
            _homeMenuItem = homeMenuItem;
            RemoveManualCase = new Command<SFCase>(DoRemoveManualCase);
            BindingContext = viewModel = new ItemsViewModel(_homeMenuItem);

            if (viewModel?.Items.Count == 0)
                viewModel?.LoadItemsCommand.Execute(null);
        }

        private void Close_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "RemoveManualQueue", this.Title);
            MessagingCenter.Send(this, "OpenSideMenu");
            Navigation.InsertPageBefore(new StartingPage(), this);
            Navigation.PopAsync();
        }


        private void DoRemoveManualCase(SFCase sfcase)
        {
            _homeMenuItem.CasesList.Remove(sfcase.CaseNumber);
            viewModel.Items.Remove(sfcase);
            ManualMenuDataStore.Instance.SaveState();
        }
    }
}
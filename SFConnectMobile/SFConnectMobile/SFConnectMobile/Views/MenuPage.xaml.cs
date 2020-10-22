using SFConnectMobile.Models;
using SFConnectMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SFConnectMobile.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MenuPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }

        MenuViewModel viewModel;

        public MenuPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new MenuViewModel();
            viewModel.LoadItemsCommand.Execute(null);
           
            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null || ((HomeMenuItem)e.SelectedItem).MenuItemType == ImplementationModel.MenuItemType.Separator)
                    return;

                var homeMenuModel = ((HomeMenuItem)e.SelectedItem);
                await RootPage.NavigateFromMenu(homeMenuModel);
            };
            
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await RootPage.NavigateFromMenu(new HomeMenuItem { Id = 0, Title = "Starting Page", MenuItemType = ImplementationModel.MenuItemType.StartingPage });
        }

        private async void AddManualQueue_Clicked(object sender, EventArgs e)
        {
            string result = await DisplayPromptAsync("Enter Manual Queue Name", "");
            if (!string.IsNullOrEmpty(result))
            {
                await (viewModel as MenuViewModel).AddManualQueue(result);
            }
        }
    }
}
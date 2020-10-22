using ImplementationModel.SalesForce;
using SFConnectMobile.Models;
using SFConnectMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SFConnectMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManualTabPickerPage : ContentPage
    {
        private SFCase _sfCase;
        private ManualTabPickerViewModel _viewModel;

        public ManualTabPickerPage(SFCase sfCase)
        {
            InitializeComponent();
            _sfCase = sfCase;
            BindingContext = _viewModel = new ManualTabPickerViewModel();
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var menuItem = e.SelectedItem as HomeMenuItem;
            menuItem.CasesList.Add(_sfCase.CaseNumber);
            _viewModel.SaveChanges();
            Navigation.PopAsync();
        }

        private async void AddManualQueue_Clicked(object sender, EventArgs e)
        {
            string result = await DisplayPromptAsync("Enter Manual Queue Name", "");
            if (!string.IsNullOrEmpty(result))
            {
                MessagingCenter.Send(this, "AddManualQueue", result);
                _viewModel.LoadItems.Execute(null);
            }
        }
    }
}
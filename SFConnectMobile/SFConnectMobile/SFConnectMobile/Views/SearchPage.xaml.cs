using ImplementationModel.SalesForce;
using SFConnectMobile.Interfaces;
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
    public partial class SearchPage : ContentPage, ISearchPage
    {
        public SearchViewModel searchViewModel { get; set; }
        public Command OpenManualQueuesPage { get; set; }

        public SearchPage()
        {
            InitializeComponent();
            BindingContext = searchViewModel = new SearchViewModel();
            OpenManualQueuesPage = new Command(DoOpenManualQueuesPage);
            searchViewModel?.GetAllCasesCommand.Execute(null);
            SearchBarTextChanged += HandleSearchBarTextChanged;
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as SFCase;
            if (item == null)
                return;

            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        public event EventHandler<string> SearchBarTextChanged;

        public void OnSearchBarTextChanged(string text) => SearchBarTextChanged?.Invoke(this, text);

        private void HandleSearchBarTextChanged(object sender, string searchBarText)
        {
            searchViewModel?.SearchCommand.Execute(searchBarText);
        }

        private async void DoOpenManualQueuesPage(object obj)
        {
            SFCase sfCase = obj as SFCase;
            await Navigation.PushAsync(new ManualTabPickerPage(sfCase));
        }
    }
}
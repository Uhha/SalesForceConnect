using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using SFConnectMobile.Models;
using SFConnectMobile.Views;
using ImplementationModel.SalesForce;
using ImplementationModel;
using SFConnectMobile.Services;
using System.Linq;
using SFConnectMobile.Base;

namespace SFConnectMobile.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private HomeMenuItem _menuItem;
        private CasesDataStore _casesDataStore;
        private ObservableCollection<ICase> items;
        public ObservableCollection<ICase> Items
        {
            get { return items; }
            set { SetProperty(ref items, value); }
        }

        public Command LoadItemsCommand { get; set; }
        private string _queueName;
        public Command SortAsc { get; set; }
        public Command SortDesc { get; set; }

        public ItemsViewModel(HomeMenuItem menuItem)
        {
            Title = menuItem.Title;
            _queueName = menuItem.Title;
            _menuItem = menuItem;
            Items = new ObservableCollection<ICase>();
            _casesDataStore = new CasesDataStore();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            SortAsc = new Command(DoSortAsc, CanSort);
            SortDesc = new Command(DoSortDesc, CanSort);
            Items.CollectionChanged += Items_CollectionChanged;
        }

        
        private void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Items");
        }

        private bool _sorting = false;
        private bool CanSort(object arg)
        {
            return !_sorting;
        }

        private void DoSortAsc(object obj)
        {
            _sorting = true;
            Items = new ObservableCollection<ICase>(Items.OrderBy(o => int.Parse(o.CaseNumber)).ToList());
            _sorting = false;
        }

        private void DoSortDesc(object obj)
        {
            _sorting = true;
            Items = new ObservableCollection<ICase>(Items.OrderByDescending(o => int.Parse(o.CaseNumber)).ToList());
            _sorting = false;
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await _casesDataStore.GetCases(_queueName, _menuItem.MenuItemType, _menuItem.CasesList);
                if (items == null) return;
                foreach (var item in items)
                {
                    Items.Add(item);
                }
                DoSortDesc(null);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
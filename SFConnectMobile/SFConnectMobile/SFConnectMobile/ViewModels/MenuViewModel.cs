using ImplementationModel;
using SFConnectMobile.Base;
using SFConnectMobile.Models;
using SFConnectMobile.Services;
using SFConnectMobile.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SFConnectMobile.ViewModels
{
    public class MenuViewModel: BaseViewModel
    {
        public ObservableCollection<HomeMenuItem> MenuItems { get; set; }
        public Command LoadItemsCommand { get; set; }

        public MenuViewModel()
        {
            MenuItems = new ObservableCollection<HomeMenuItem>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            MessagingCenter.Subscribe<ManualItemsPage, string>(this, "RemoveManualQueue", async (sender, queueTitle) =>
                {
                    await RemoveManualQueue(MenuItems.FirstOrDefault(o => o.Title == queueTitle));
                });
            MessagingCenter.Subscribe<ManualTabPickerPage, string>(this, "AddManualQueue", async (sender, queueTitle) =>
            {
                await AddManualQueue(queueTitle);
            });
        }

        private async Task Refresh()
        {
            await ExecuteLoadItemsCommand(true);
        }

        private async Task ExecuteLoadItemsCommand(bool forceRefresh = false)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                MenuItems.Clear();
                var items = await  QueueMenuDataStore.Instance.GetQueueItemsAsync(forceRefresh);

                MenuItems.Add(new HomeMenuItem { Id = 0, Title = "Search", MenuItemType = MenuItemType.Search });
                MenuItems.Add(new HomeMenuItem { Id = 0, MenuItemType = MenuItemType.Separator });

                bool separatorInserted = false;
                foreach (var item in items)
                {
                    if (item.MenuItemType == MenuItemType.Review && !separatorInserted)
                    {
                        MenuItems.Add(new HomeMenuItem() { Id = 100, MenuItemType = MenuItemType.Separator });
                        separatorInserted = true;
                    }
                    MenuItems.Add(item);
                }

                MenuItems.Add(new HomeMenuItem { Id = 0, MenuItemType = MenuItemType.Separator });

                var manual = ManualMenuDataStore.Instance.GetManualItems(forceRefresh);
                foreach (var item in manual)
                {
                    MenuItems.Add(item);
                }

                MenuItems.Add(new HomeMenuItem { Id = 0, Title = "Add Manual Queue" , MenuItemType = MenuItemType.ManualQueueAddButton });
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


        internal async Task AddManualQueue(string queueName)
        {
            var menuItem = new HomeMenuItem { Title = queueName, CasesList = new List<string>(), MenuItemType = MenuItemType.Manual };
            ManualMenuDataStore.Instance.AddManualItem(menuItem);
            await Refresh();
        }

        internal async Task RemoveManualQueue(HomeMenuItem queue)
        {
            ManualMenuDataStore.Instance.RemoveManualItem(queue);
            await Refresh();
        }
    }
}

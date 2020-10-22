using ImplementationModel.SalesForce;
using SFConnectMobile.Base;
using SFConnectMobile.Models;
using SFConnectMobile.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace SFConnectMobile.ViewModels
{
    public class ManualTabPickerViewModel: BaseViewModel
    {
        public ObservableCollection<HomeMenuItem> Items { get; set; }
        public Command LoadItems { get; set; }

        public ManualTabPickerViewModel()
        {
            Items = new ObservableCollection<HomeMenuItem>();
            LoadItems = new Command(DoLoadItems);
            LoadItems.Execute(null);
        }

        private void DoLoadItems()
        {
            IsBusy = true;
            Items.Clear();
            try
            {
                var list = ManualMenuDataStore.Instance.GetManualItems();
                foreach (var item in list)
                {
                    Items.Add(item);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void SaveChanges()
        {
            ManualMenuDataStore.Instance.SaveState();
        }
    }
}

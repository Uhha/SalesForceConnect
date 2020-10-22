using ImplementationModel;
using SFConnectMobile.Base;
using SFConnectMobile.Services;
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
    public class SearchViewModel: BaseViewModel
    {
        private IList<ICase> _allCases;
        private CasesDataStore _casesDataStore;

        public Command GetAllCasesCommand { get; set; }

        private ObservableCollection<ICase> items;
        public ObservableCollection<ICase> Items
        {
            get { return items; }
            set { SetProperty(ref items, value); }
        }

        public Command<string> SearchCommand { get; set; }

        public bool IncludeClosedCases { get; set; }

        public SearchViewModel()
        {
            _allCases = new List<ICase>();
            _casesDataStore = new CasesDataStore();
            Items = new ObservableCollection<ICase>();
            GetAllCasesCommand = new Command(async () => await DoGetAllCases());
            SearchCommand = new Command<string>((text) => DoSearch(text));
        }

        private async Task DoGetAllCases()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                _allCases.Clear();
                _allCases = await _casesDataStore.GetAllCases(IncludeClosedCases);
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

        private void DoSearch(string text)
        {
            if (IsBusy)
                return;

            if (text.Length <= 3) return;

            IsBusy = true;

            try
            {
                Items.Clear();
                IList<ICase> bySubject = _allCases.Where<ICase>(o => o.Subject.Contains(text)).ToList();
                IList<ICase> byNumber = _allCases.Where<ICase>(o => o.CaseNumber.Contains(text)).ToList();
                var items = bySubject.Union(byNumber);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
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

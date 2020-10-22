using ImplementationModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFConnectorFullScreenUI
{
    public class SortBarPannelVM : BaseViewModel
    {
        private QueueComboBoxVM _queueComboBoxVM;
        public QueueComboBoxVM QueueComboBoxVM
        {
            get
            {
                return _queueComboBoxVM;
            }
            set
            {
                if (value != null)
                {
                    value.PropertyChanged += QueueComboBoxVM_PropertyChanged;
                }
                _queueComboBoxVM = value;
            }
        }
        public CasesCanvasVM CasesCanvasVM { get; set; }

        public string TextToSearchOnCases { get; set; }
        public string SearchCaseByNumber { get; set; }
        public void OnSearchCaseByNumberChanged()
        {
            CasesCanvasVM?.SearchCase.Execute(SearchCaseByNumber);
        }

        public IAsyncCommand<string> OpenSingleCase { get; set; }
        public IAsyncCommand<string> FindCasesText { get; set; }
        public RelayCommand SearchTextLostFocus { get; set; }
        public RelayCommand ChangeViewToList { get; set; }
        

        private bool _isOpeningSingleCase;

        public SortBarPannelVM(string tabHeader)
        {
            if (QueueComboBoxVM == null) QueueComboBoxVM = new QueueComboBoxVM();
            if (CasesCanvasVM == null) CasesCanvasVM = new CasesCanvasVM(tabHeader);
            if (OpenSingleCase == null) OpenSingleCase = new AsyncCommand<string>(DoOpenSingleCase, CanOpenSingleCase, new RelayCommandErrorHandler());
            if (FindCasesText == null) FindCasesText = new AsyncCommand<string>(DoFindCasesText);
            if (SearchTextLostFocus == null) SearchTextLostFocus = new RelayCommand(DoSearchTextLostFocus);
            if (ChangeViewToList == null) ChangeViewToList = new RelayCommand(DoChangeViewToList);
        }

        private void DoChangeViewToList()
        {
           
            if (CasesCanvasVM.ActiveView == ActiveView.Normal)
            {
                CasesCanvasVM.ActiveView = ActiveView.Long;
            }
            else if (CasesCanvasVM.ActiveView == ActiveView.Long)
            {
                CasesCanvasVM.ActiveView = ActiveView.Normal;
            }
        }

        public SortBarPannelVM(CasesTab tab) : this(tab.TabIdentifier)
        {
            var customTab = tab.CasesLists.FirstOrDefault(o => o.CasesQueueName == null);
            if (customTab != null)
            {
                CasesCanvasVM.BuildCasesFromStorage.ExecuteAsync(customTab.Cases);//.FireAndForgetSafeAsync();
                CasesCanvasVM.BuildLabelsFromStorage(customTab.Labels);
            }
        }
       
        public async void PopulateCases(string[] cases) 
        {
            await CasesCanvasVM.BuildCasesFromArray(cases);
        }


        private bool CanOpenSingleCase(string caseNumber)
        {
            //var asd = (CasesCanvasVM != null) ? CasesCanvasVM.IsAbleToOpenNotFoundCase : true;
            return !_isOpeningSingleCase;
        }

        private async Task DoOpenSingleCase(string caseNumber)
        {
            _isOpeningSingleCase = true;
            await CasesCanvasVM.OpenSingleCase(caseNumber);
            _isOpeningSingleCase = false;

        }

        private async Task DoFindCasesText(string text)
        {
            CasesCanvasVM?.SearchCasesText.ExecuteAsync(text).FireAndForgetSafeAsync();
        }
        private void DoSearchTextLostFocus()
        {
            if (string.IsNullOrEmpty(TextToSearchOnCases))
            {
                CasesCanvasVM?.SearchCasesText.ExecuteAsync(null).FireAndForgetSafeAsync();
            }
        }

        private async void QueueComboBoxVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (QueueComboBoxVM == null || CasesCanvasVM == null) return;
            if (e.PropertyName == nameof(QueueComboBoxVM.SelectedQueueItem))
            {
                var param = QueueComboBoxVM.SelectedQueueItem;
                QueueComboBoxVM.BusySpinnerVM.Spin = true;
                await CasesCanvasVM.BuildCases.ExecuteAsync(param);
                QueueComboBoxVM.BusySpinnerVM.Spin = false;
            }
        }

        internal void SortCases(bool descending, SortBy sortBy = SortBy.None)
        {
            if (sortBy != SortBy.None) CasesCanvasVM.SortBy = sortBy;
            CasesCanvasVM.SortCases.Execute(descending);
        }

        internal void SetGroupBy(GroupBy groupBy)
        {
            CasesCanvasVM.GroupBy = groupBy;
        }
    }
}

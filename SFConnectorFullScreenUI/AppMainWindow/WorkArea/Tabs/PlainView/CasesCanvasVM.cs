using ImplementationModel;
using SFLink;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SFConnectorFullScreenUI
{
    public class CasesCanvasVM : BaseViewModel
    {
        private string _tabHeader;
        public string TabHeader
        {
            get
            {
                return _tabHeader;
            }
            set
            {
                if (_tabHeader != null && _tabHeader != value)
                {
                    CasesStatesOperations.RenameTab(_tabHeader, value);
                }
                _tabHeader = value;
            }
        }

        public ActiveView ActiveView { get; set; }

        private string _previousQueue;
        public ObservableCollection<CaseControlVM> CaseControls { get; set; }
        public ObservableCollection<CellLabelVM> CellLabels { get; set; }
        public CaseControlVM SelectedCaseControl { get; set; }

        internal void RemoveLabel(CellLabelVM cellLabelVM)
        {
            CasesStatesOperations.RemoveLabel(cellLabelVM.Guid, _previousQueue, TabHeader);
            CellLabels.Remove(cellLabelVM);
        }

        //internal CaseControlVM[,] CaseControlsArray = new CaseControlVM[0, 0];

        //internal CellLabelVM[,] CellLabelArray = new CellLabelVM[0, 0];

        private bool _isBusyBuildCases;

        public IAsyncCommand<string> BuildCases { get; private set; }
        public IAsyncCommand<Dictionary<string, CaseInfo>> BuildCasesFromStorage { get; private set; }
        public RelayCommand<bool> SortCases { get; private set; }

        public RelayCommand<string> SearchCase { get; private set; }
        

        private bool _isBusySearching;
        private IEnumerable<CaseControlVM> _dimmedCases;
        public IAsyncCommand<string> SearchCasesText { get; private set; }
        public static int ConrnerGap = 300;


        public int CanvasHeight { get; set; }

        
        public int CanvasWidth { get; set; }

        public int CaseHeight { get; set; }
        public void OnCaseHeightChanged()
        {
            CanvasHeight = (CaseControls.Max(o => o.Top as int?) ?? 0) + ConrnerGap;
            RefreshSizes();
        }
        public int CaseWidth { get; set; }
        public void OnCaseWidthChanged()
        {
            CanvasWidth = (CaseControls.Max(o => o.Left as int?) ?? 0) + ConrnerGap;
            RefreshSizes();
        }

        public int LongCaseHeight { get; set; }
        public void OnLongCaseHeightChanged()
        {
            CanvasHeight = (CaseControls.Max(o => o.Top as int?) ?? 0) + ConrnerGap;
            RefreshSizes();
        }
        public int LongCaseWidth { get; set; }
        public void OnLongCaseWidthChanged()
        {
            CanvasWidth = (CaseControls.Max(o => o.Left as int?) ?? 0) + ConrnerGap;
            RefreshSizes();
        }

        public bool IsAbleToOpenNotFoundCase { get; private set; }

        private void RefreshSizes()
        {
            CaseControls.ToList().ForEach(o => o.RefreshPositionFromIndex());
        }

        public CasesCanvasVM(string tabHeader)
        {
            TabHeader = tabHeader;
            BuildCases = new AsyncCommand<string>(BuildCasesAsync, CanBuildCases, new RelayCommandErrorHandler());
            BuildCasesFromStorage = new AsyncCommand<Dictionary<string, CaseInfo>>(DoBuildCasesFromStorage, errorHandler: new RelayCommandErrorHandler());
            SortCases = new RelayCommand<bool>(Sort, CanSortCases);
            SearchCase = new RelayCommand<string>(DoSearchCase, CanSearchCase);
            SearchCasesText = new AsyncCommand<string>(DoSearchCasesText);
            CaseControls = new ObservableCollection<CaseControlVM>();
            CellLabels = new ObservableCollection<CellLabelVM>();
            CaseWidth = ConfigurableParameters.Instance.CaseWidth;
            CaseHeight = ConfigurableParameters.Instance.CaseHeight;
            LongCaseWidth = ConfigurableParameters.Instance.LongCaseWidth;
            LongCaseHeight = ConfigurableParameters.Instance.LongCaseHeight;
            ActiveView = ActiveView.Normal;
        }

        internal void AddCellLabel(Point position)
        {
            Index index = Index.GetCellCornerPoint((int)position.X, (int)position.Y, CaseWidth, CaseHeight);
            var label = new CellLabelVM(this, index.X, index.Y);
            CellLabels.Add(label);
            LoggerController.Log($"Added Label to {index.X.ToString()}, {index.Y.ToString()}");
        }

        internal async Task BuildCasesFromArray(string[] cases)
        {
            IList<ICase> casesData;
            try
            {
                _isBusyBuildCases = true;
                casesData = await Task.Run(() => SFConnector.Instance.GetCasesFromArrayAsync(cases));
            }
            finally
            {
                _isBusyBuildCases = false;
            }
            LoggerController.Log(casesData.Count + $" cases Retrived.");
            foreach (var item in casesData)
            {
                var caseViewModel = new CaseControlVM(item, this);
                CaseControls.Add(caseViewModel);
            }
        }
        internal void BuildLabelsFromStorage(Dictionary<string, LabelInfo> labels)
        {
            foreach (LabelInfo item in labels.Values)
            {
                var success = Guid.TryParse(item.Guid, out Guid guid);
                if (!success) continue;
                CellLabels.Add(new CellLabelVM(this, item.Left, item.Top, item.Width, item.Height, item.Note, guid));
            }
        }

        internal async Task DoBuildCasesFromStorage(Dictionary<string, CaseInfo> cases)
        {
            IList<ICase> casesData;
            try
            {
                _isBusyBuildCases = true;
                casesData = await Task.Run(() => SFConnector.Instance.GetCasesFromArrayAsync(cases.Keys.ToArray()));
            }
            finally
            {
                _isBusyBuildCases = false;
            }
            LoggerController.Log(casesData.Count + $" cases Retrived.");
            foreach (var item in casesData)
            {
                var caseViewModel = new CaseControlVM(item, this);
                caseViewModel.Index = cases[item.CaseNumber].Index;
                CaseControls.Add(caseViewModel);
            }
        }

        private async Task BuildCasesAsync(string selection)
        {
            if (!string.IsNullOrEmpty(_previousQueue))
            {
                UpdateCasesPositions();
            }
            CaseControls.Clear();
            CellLabels.Clear();
            IList<ICase> casesData;
            try
            {
                _isBusyBuildCases = true;
                casesData = await Task.Run(() => SFConnector.Instance.GetCasesAsync(selection));
            }
            finally
            {
                _isBusyBuildCases = false;
            }
            _previousQueue = selection;
            LoggerController.Log(casesData.Count + $" cases Retrived.");
            foreach (var item in casesData)
            {
                var caseViewModel = new CaseControlVM(item, this);
                CaseControls.Add(caseViewModel);
                CaseInfo positionInfo = null;
                try
                {
                    positionInfo = CasesStatesOperations.GetCaseInfo(item.CaseNumber, selection, TabHeader); 
                }
                catch (Exception e)
                {
                    LoggerController.Log(e.Message);
                }
                if (positionInfo != null)
                    caseViewModel.Index = positionInfo.Index;
            }

            try
            {
                //Add Labels from JSON
                var caseslist = CasesStatesOperations.GetCasesList(selection, TabHeader);
                foreach (LabelInfo item in caseslist.Labels.Values)
                {
                    var success = Guid.TryParse(item.Guid, out Guid guid);
                    if (!success) continue;
                    CellLabels.Add(new CellLabelVM(this, item.Left, item.Top, item.Width, item.Height, item.Note, guid));
                }
            }
            catch (Exception e)
            {
                LoggerController.Log(e.Message);
            }
        }

        private bool CanBuildCases(string selection)
        {
            return !_isBusyBuildCases;
        }

        private bool CanSearchCase(string caseNumber)
        {
            return !_isBusySearching;
        }

        private void DoSearchCase(string caseNumber)
        {
            _isBusySearching = true;
            try
            {
                caseNumber = caseNumber.TrimStart('0');
                var foundCases = CaseControls.Where(o => o.CaseModel.CaseNumber.TrimStart('0').StartsWith(caseNumber.TrimStart('0')));

                DimmCasesInverted(foundCases);

                if (!string.IsNullOrEmpty(caseNumber) && foundCases.Count() == 0)
                    IsAbleToOpenNotFoundCase = true;
                else
                    IsAbleToOpenNotFoundCase = false;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _isBusySearching = false;
            }
        }

        private void UnDimmAllCases()
        {
            if (_dimmedCases != null)
            {
                foreach (var item in _dimmedCases)
                {
                    item.SearchDim = false;
                }
            }
        }

        private void DimmCasesInverted(IEnumerable<CaseControlVM> dimmAllButThis)
        {
            UnDimmAllCases();
            var invert = CaseControls.Except(dimmAllButThis);
            if (invert.Count() > 0)
            {
                _dimmedCases = invert;
            }
            else { _dimmedCases = null; }

            if (_dimmedCases != null)
            {
                foreach (var item in _dimmedCases)
                {
                    item.SearchDim = true;
                }
            }

        }

        internal async Task OpenSingleCase(string caseNumber)
        {
            ICase caseData;
            try
            {
                _isBusyBuildCases = true;
                caseData = await Task.Run(() => SFConnector.Instance.GetTheCase(caseNumber));
            }
            finally
            {
                _isBusyBuildCases = false;
            }
            LoggerController.Log(caseNumber + $" {CaseType.SF.ToString()} case Retrived.");
            if (caseData == null) return;
            var caseViewModel = new CaseControlVM(caseData, this);
            CaseControls.Add(caseViewModel);
            caseViewModel.Index = new Index(0,0);
        }

        internal async Task DoSearchCasesText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                UnDimmAllCases();
                foreach (var item in CaseControls)
                {
                    item.RevertSearchedTextToOriginal();
                }
                return;
            }
            var found = CaseControls.Where(o => (o.CaseModel.Subject != null && o.CaseModel.Subject.Contains(text))  || 
                (o.CaseModel.Description != null && o.CaseModel.Description.Contains(text)));
            DimmCasesInverted(found);
            foreach(var item in found)
            {
                item.HighlightCaseText(text);
            }
        }

        
        internal void UpdateCasesPositions()
        {
            CasesStatesOperations.UpdateCasesPositions(CaseControls.ToList(), _previousQueue , TabHeader);
            CasesStatesOperations.UpdateLabelsPositions(CellLabels.ToList(), _previousQueue, TabHeader);
        }

        internal void RemoveCase(CaseControlVM caseControl)
        {
            CaseControls.Remove(caseControl);
            CasesStatesOperations.RemoveCase(caseControl, _previousQueue, TabHeader);
        }


        #region SortControl
        public GroupBy GroupBy;
        public SortBy SortBy;
        private bool _isBusySortCases;
        public bool SwapAxis { get; internal set; }
        
        private void Sort(bool descending)
        {
            LoggerController.Log("Case Type: " + CaseType.SF.ToString() + " Sort By: " + SortBy.ToString() + ((descending) ? " Descending" : " Ascending"));
            CaseControls.ToList().ForEach(o => o.CloseDetails());
            _isBusySortCases = true;
            IOrderedEnumerable<IGrouping<object, CaseControlVM>> sorted = default(IOrderedEnumerable<IGrouping<object, CaseControlVM>>);

            if (descending)
            {
                sorted = CaseControls.OrderByDescending(Order(SortBy)).GroupBy(Group(GroupBy)).OrderBy(g => g.Key);
            }
            else
            {
                sorted = CaseControls.OrderBy(Order(SortBy)).GroupBy(Group(GroupBy)).OrderBy(g => g.Key);
            }
            UpdatedOrderedCases(sorted);
            _isBusySortCases = false;
        }

        private bool CanSortCases(bool descending)
        {
            return !_isBusySortCases;
        }

        private Func<CaseControlVM, object> Group(GroupBy groupBy)
        {
            switch (groupBy)
            {
                case GroupBy.None:
                    return o => "None";
                case GroupBy.ID:
                    return o => o.CaseModel.CaseNumber;
                case GroupBy.Name:
                    return o => o.CaseModel.CreatedBy?.GetLastName();
                case GroupBy.DateOpened:
                    return o => o.CaseModel.CreatedDate;
                case GroupBy.DateClosed:
                    return o => o.CaseModel.ClosedDate;
            }
            return o => "None";
        }

        private Func<CaseControlVM, object> Order(SortBy sortBy)
        {
            switch (sortBy)
            {
                case SortBy.None:
                    return o => "None";
                case SortBy.ID:
                    return o => o.CaseModel.CaseNumber;
                case SortBy.Name:
                    return o => o.CaseModel.CreatedBy?.GetLastName();
                case SortBy.DateOpened:
                    return o => o.CaseModel.CreatedDate;
                case SortBy.DateClosed:
                    return o => o.CaseModel.ClosedDate;
            }
            return o => "None";
        }

        internal void UpdatedOrderedCases(IOrderedEnumerable<IGrouping<object, CaseControlVM>> sorted)
        {
            int i = 0;
            int j = 0;
            foreach (var group in sorted)
            {
                foreach (var caseControlVM in group)
                {
                    if (SwapAxis)
                    {
                        //TODO:
                        //caseControl.CloseDetails();
                        caseControlVM.Index = new Index(j++, i);
                    }
                    else
                    {
                        //TODO:
                        //caseControl.CloseDetails();
                        caseControlVM.Index = new Index(i, j++);
                    }
                }
                j = 0;
                i++;
            }
        }

        #endregion
    }
}

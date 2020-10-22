using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SFConnectorFullScreenUI
{
    public class WorkAreaVM: BaseViewModel
    {
        public static ObservableCollection<TabsContainer> Tabs { get; set; }
        public ICommand AddEmptyTab { get; set; }

        public WorkAreaVM()
        {
            Tabs = new ObservableCollection<TabsContainer>();
            TabsContainer.Container = Tabs;

            try
            {
                foreach (var tab in CasesStatesOperations.GetTabs())
                {
                    Tabs.Add(new TabsContainer(new SortBarPannelVM(tab)));
                }
            }
            catch (Exception e)
            {
                LoggerController.Log(e.Message);
            }
            AddEmptyTab = new RelayCommand(DoAddEmptyTab);
        }

        private void DoAddEmptyTab()
        {
            var tabIdent = GenerateNewTabName();
            Tabs.Add(new TabsContainer (new SortBarPannelVM(tabIdent)));
        }

        public static void AddTabWithCases(ObservableCollection<CaseControlVM> cases)
        {
            if (cases.Count == 0) return;
            var tabIdent = GenerateNewTabName();
            var content = new SortBarPannelVM(tabIdent);
            content.PopulateCases((cases.Select(o => o.CaseModel.CaseNumber)).ToArray());
            Tabs.Add(new TabsContainer(content));
            CasesStatesOperations.UpdateCasesPositions(cases.ToList(), null, tabIdent);
            cases.Clear();
        }

        private static string GenerateNewTabName()
        {
            int newFolderCounter = 1;
            bool exists = true;
            while (exists)
            {
                exists = Tabs.Any(o => o.Content.CasesCanvasVM.TabHeader == $"New Tab {newFolderCounter}");
                if (exists)
                {
                    newFolderCounter++;
                }
            }
            return $"New Tab {newFolderCounter}";
        }
    }

    public class TabsContainer : BaseViewModel
    {
        public SortBarPannelVM Content { get; set; }
        public bool ShowEditTab { get; set; }
        public ICommand TriggerEditMode { get; set; }
        public ICommand ApplyChange { get; set; }
        public ICommand CloseTab { get; set; }

        public static ObservableCollection<TabsContainer> Container;



        public TabsContainer(SortBarPannelVM sortBarPannelVM)
        {
            Content = sortBarPannelVM;
            TriggerEditMode = new RelayCommand(DoTriggerEditMode);
            ApplyChange = new RelayCommand(DoApplyChange);
            CloseTab = new RelayCommand(DoCloseTab);
        }

        private void DoCloseTab()
        {
            Container.Remove(this);
            CasesStatesOperations.RemoveTab(this.Content.CasesCanvasVM.TabHeader);
        }

        private void DoApplyChange()
        {
            ShowEditTab = false;
        }

        private void DoTriggerEditMode()
        {
            ShowEditTab = true;
        }
    }
}

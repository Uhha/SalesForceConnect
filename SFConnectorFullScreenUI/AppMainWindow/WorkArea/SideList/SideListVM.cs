using ImplementationModel;
using ImplementationModel.SalesForce;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SFConnectorFullScreenUI
{
    public class SideListVM: BaseViewModel
    {
        public ObservableCollection<CaseControlVM> Cases { get; set; }
        public ICommand CreateTab { get; set; }

        //TODO: Binding Doesn't work because of the STATIC
        public int SideListWidth { get; set; }

        private SideListVM()
        {
            Cases = new ObservableCollection<CaseControlVM>();
            CreateTab = new RelayCommand(DoCreateTab);
            SideListWidth = 0;
        }

        private void DoCreateTab()
        {
            WorkAreaVM.AddTabWithCases(Cases);
            SideListWidth = 0;
        }

        private static SideListVM _instance;
        public static SideListVM Instance
        {
            get
            {
                return _instance ?? (_instance = new SideListVM());
            }
        }

        public static void AddCaseToSideList(CaseControlVM caseControlVM)
        {
            bool existing = Instance.Cases.Any(o => o.CaseModel.Id == caseControlVM.CaseModel.Id);
            if (!existing)
                Instance.Cases.Add(caseControlVM);

            CheckIfEmpty();
        }

        public static void RemoveCaseFromSideList(CaseControlVM caseControlVM)
        {
            bool existing = Instance.Cases.Any(o => o.CaseModel.Id == caseControlVM.CaseModel.Id);
            if (existing)
                Instance.Cases.Remove(Instance.Cases.First(o => o.CaseModel.Id == caseControlVM.CaseModel.Id));

            CheckIfEmpty();
        }

        private static void CheckIfEmpty()
        {
            if (Instance.Cases.Count > 0)
            {
                Instance.SideListWidth = ConfigurableParameters.Instance.SideListCaseWidth;
            }
            else
            {
                Instance.SideListWidth = 0;
            }

        }
    }
}

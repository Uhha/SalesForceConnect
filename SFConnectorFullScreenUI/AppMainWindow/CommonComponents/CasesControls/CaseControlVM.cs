using ImplementationModel;
using ImplementationModel.SalesForce;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace SFConnectorFullScreenUI
{
    public class CaseControlVM : BaseViewModel
    {
        public ICase CaseModel { get; set; }
        public string CaseCreatorLastName { get; set; }
        public CasesCanvasVM CasesCanvasVM { get; set; }
        public RelayCommand AddToSideList { get; set; }
        public RelayCommand CloseCase { get; set; }
        

        public CaseControlDetailsVM CaseControlDetailsVM { get; set; }
        public bool DetailsVisible { get; set; }

        public RelayCommand ShowDetails { get; set; }

        public bool SearchDim { get; set; }

        public bool Selected { get; set; }

        public int Left { get; set; } 

        public int Top { get; set; }

        private static int _maxZIndex;
        public int ZIndex { get; set; }

       

        private Index _index;
        internal Index Index
        {
            get { return _index; }
            set
            {
                _index = value;
                if (value != null)
                {
                    var (x, y) = Index.GetPositionFromIndex(value, CasesCanvasVM.CaseWidth, CasesCanvasVM.CaseHeight);
                    CasesCanvasVM.CanvasHeight = (CasesCanvasVM.CaseControls.Max(o => o.Top as int?) ?? 0) + CasesCanvasVM.ConrnerGap;
                    CasesCanvasVM.CanvasWidth = (CasesCanvasVM.CaseControls.Max(o => o.Left as int?) ?? 0) + CasesCanvasVM.ConrnerGap;
                    Left = x;
                    Top = y;
                }
            }
        }
        public CaseControlVM(ICase caseModel, CasesCanvasVM casesCanvasVM)
        {
            if (caseModel == null) return;
            CaseModel = caseModel;
            CaseControlDetailsVM = new CaseControlDetailsVM(caseModel);
            DetailsVisible = false;
            ShowDetails = new RelayCommand(DoShowDetails);

            ZIndex = _maxZIndex++;

            CaseCreatorLastName = caseModel.CreatedBy?.GetLastName();
            CasesCanvasVM = casesCanvasVM;
            AddToSideList = new RelayCommand(AddCaseToSideList);
            CloseCase = new RelayCommand(DoCloseCase);
        }

        private void DoCloseCase()
        {
            CasesCanvasVM.RemoveCase(this);
        }

        public void RefreshPositionFromIndex()
        {
            var (x, y) = Index.GetPositionFromIndex(Index, CasesCanvasVM.CaseWidth, CasesCanvasVM.CaseHeight);
            Left = x;
            Top = y;
        }

        private void DoShowDetails()
        {
            if (!DetailsVisible) PutOnTop();
            DetailsVisible = !DetailsVisible;
        }

        public void PutOnTop()
        {
            ZIndex = _maxZIndex++;
        }

        public void CloseDetails()
        {
            DetailsVisible = false;
        }

        private void AddCaseToSideList()
        {
            SideListVM.AddCaseToSideList(this);
        }

        internal void HighlightCaseText(string text)
        {
            CaseControlDetailsVM.HighlightCaseText(text);
        }
        internal void RevertSearchedTextToOriginal()
        {
            CaseControlDetailsVM.RevertSearchedTextToOriginal();
        }
    }
}

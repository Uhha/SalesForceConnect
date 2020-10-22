using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFConnectorFullScreenUI
{
    public class CellLabelVM: BaseViewModel
    {
        public Guid Guid { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public CasesCanvasVM CasesCanvasVM { get; set; }
        public RelayCommand CloseCellLabel { get; set; }
        public string Note { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        private const int CELLGAP = 5;
        //private Index _index;
        //internal Index Index
        //{
        //    get { return _index; }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            var (x, y) = Index.GetPositionFromIndex(value, CasesCanvasVM.CaseWidth, CasesCanvasVM.CaseHeight);
        //            Left = x;
        //            Top = y;
        //        }
        //        _index = value;
        //    }
        //}

        public CellLabelVM(CasesCanvasVM casesCanvasVM, int left, int top)
        {
            CasesCanvasVM = casesCanvasVM;
            CloseCellLabel = new RelayCommand(DoCloseCellLabel);
            Width = ConfigurableParameters.Instance.CaseWidth;
            Height = ConfigurableParameters.Instance.CaseHeight;
            Left = left;
            Top = top;
            Guid = Guid.NewGuid();
        }

        public CellLabelVM(CasesCanvasVM casesCanvasVM, int left, int top, int width, int height, string note, Guid guid) 
            : this(casesCanvasVM, left, top)
        {
            Width = width;
            Height = height;
            Note = note;
            Guid = guid;
        }

        private void DoCloseCellLabel()
        {
            CasesCanvasVM.RemoveLabel(this);
        }

        
    }


}

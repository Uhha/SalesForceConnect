using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFConnectorFullScreenUI
{
    public class BusySpinnerVM : BaseViewModel
    {
        public bool Spin { get; set; }
        //public OperationCompletedMarkVM OperationCompletedMarkVM { get; internal set; }

        public BusySpinnerVM()
        {

        }

        public void StartSpinning()
        {
            Spin = true;
        }

        public void StopSpinning(bool successful = true)
        {
            Spin = false;
            //OperationCompletedMarkVM.StartAnimation = successful;
        }
    }
}

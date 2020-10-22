using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SFConnectorFullScreenUI
{
    /// <summary>
    /// Interaction logic for BusySpinner.xaml
    /// </summary>
    public partial class BusySpinner : UserControl
    {
        OperationCompletedMark _completedMark;

        public BusySpinner()
        {
            InitializeComponent();
            //datacontext.OperationCompletedMarkVM = _completedMark?.DataContext as OperationCompletedMarkVM;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _completedMark = (OperationCompletedMark) base.Template.FindName("completedMark", this);
        }

        //private void OperationFinished(bool successful)
        //{
        //    _completedMark?.OperationFinished(successful);
        //}

        
    }
    
}

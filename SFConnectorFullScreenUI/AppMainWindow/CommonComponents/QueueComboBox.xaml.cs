using ImplementationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SFConnectorFullScreenUI
{
    /// <summary>
    /// Interaction logic for QueueComboBox.xaml
    /// </summary>
    public partial class QueueComboBox : UserControl
    {
        public QueueComboBox()
        {
            InitializeComponent();
            DataContext = new QueueComboBoxVM();
            //datacontext.BusySpinnerVM = (spinner.DataContext as BusySpinnerVM);
            //datacontext.BuildQueue.ExecuteAsync().FireAndForgetSafeAsync();

            //Dispatcher.BeginInvoke(DispatcherPriority.DataBind,
            //    new Action(delegate ()
            //    {
            //        var datacontext = DataContext as QueueComboBoxVM;
            //        datacontext.BusySpinnerVM = (spinner.DataContext as BusySpinnerVM);
            //        datacontext.BuildQueue.ExecuteAsync().FireAndForgetSafeAsync();
            //    }));
        }

      
    }
}

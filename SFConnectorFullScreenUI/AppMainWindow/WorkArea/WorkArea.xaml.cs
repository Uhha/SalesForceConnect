using ImplementationModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for WorkArea.xaml
    /// </summary>
    public partial class WorkArea : TabControl
    {
        

        public WorkArea()
        {
            InitializeComponent();
            DataContext = new WorkAreaVM();
            //Dispatcher.BeginInvoke(DispatcherPriority.DataBind,
            //    new Action(delegate () {
            //        var dataContext = new WorkAreaVM();
            //        DataContext = dataContext;
            //        SideListVM.WorkAreaVM = dataContext;
            //    }));
        }
        private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }
    }
}

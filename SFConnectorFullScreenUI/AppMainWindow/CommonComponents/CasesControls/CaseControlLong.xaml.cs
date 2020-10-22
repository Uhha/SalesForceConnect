using ImplementationModel;
using ImplementationModel.SalesForce;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SFConnectorFullScreenUI
{
    /// <summary>
    /// Interaction logic for CaseControlLong.xaml
    /// </summary>
    public partial class CaseControlLong : UserControl, INotifyPropertyChanged
    {

        public string SubStatus { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }



        public CaseControlLong()
        {
            InitializeComponent();
            Loaded += CaseControlLong_Loaded;
        }

        private void CaseControlLong_Loaded(object sender, RoutedEventArgs e)
        {
            var caseControlVM = DataContext as CaseControlVM;

            if (caseControlVM.CaseModel.GetType() == typeof(SFCase))
            {
                var enumValue = (caseControlVM.CaseModel as SFCase).SubStatus;
                SubStatus = ControlsHelper.GetDescription(enumValue);
            }
        }
    }
}

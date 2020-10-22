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
    /// Interaction logic for CaseControlDetails.xaml
    /// </summary>
    public partial class CaseControlDetails : UserControl, INotifyPropertyChanged
    {
        public double CalculatedHeight { get; set; }
        public double CaseDetailsWidth { get; set; }
        

        public CaseControlDetails()
        {
            InitializeComponent();
            CaseDetailsWidth = ConfigurableParameters.Instance.CaseDetailsWidth;
            SetCalculatedHeight();
            
        }

        private void SetCalculatedHeight()
        {
            caseControlDetails.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            UpdateLayout();
            var height = stackPanel.DesiredSize.Height;
            //set height to a highest 25 cell
            CalculatedHeight = Math.Floor(height / ConfigurableParameters.Instance.CaseHeight) * ConfigurableParameters.Instance.CaseHeight + ConfigurableParameters.Instance.CaseHeight;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        
    }
}

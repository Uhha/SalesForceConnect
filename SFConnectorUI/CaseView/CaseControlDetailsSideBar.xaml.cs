using SFLink;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace SFConnectorUI.CaseView
{
    /// <summary>
    /// Interaction logic for UserControlTest.xaml
    /// </summary>
    public partial class CaseControlDetailsSideBar : UserControl, INotifyPropertyChanged
    {


        public event PropertyChangedEventHandler PropertyChanged;

        public CaseControlDetailsSideBar(CaseControl caseControl)
        {
            
            InitializeComponent();
            this.DataContext = this;

            InitializeDetails(caseControl);
        }

        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }


        private void InitializeDetails(CaseControl caseControl)
        {

            Canvas.SetLeft(this, caseControl.Left);
            Canvas.SetTop(this, caseControl.Top);
            Canvas.SetZIndex(this, caseControl.ZIndex - 1);

            subject.Text = caseControl.Case.Subject;
            this.ToolTip = caseControl.Case.Description;

            caseControl.PropertyChanged += CaseControl_PropertyChanged;
        }

        private void CaseControl_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Left") Canvas.SetLeft(this, (sender as CaseControl).Left);
            if (e.PropertyName == "Top") Canvas.SetTop(this, (sender as CaseControl).Top);
        }
    }
}

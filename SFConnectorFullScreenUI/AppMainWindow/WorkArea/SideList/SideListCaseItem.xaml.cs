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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SFConnectorFullScreenUI
{
    /// <summary>
    /// Interaction logic for CaseControl.xaml
    /// </summary>
    public partial class SideListCaseItem : UserControl, INotifyPropertyChanged
    {
        public int CaseHeight { get; set; }
        public int SideListCaseWidth { get; set; }
        public int SideListCaseHeight { get; set; }

        private Brush _caseBackgroundColor;
        public Brush CaseBackgroundColor
        {
            get
            {
                return _caseBackgroundColor;
            }
            set
            {
                _caseBackgroundColor = value;
                NotifyPropertyChanged("CaseBackgroundColor");
            }
        }

        private Brush _priorityColor;
        public Brush PriorityColor
        {
            get
            {
                return _priorityColor;
            }
            set
            {
                _priorityColor = value;
                NotifyPropertyChanged("PriorityColor");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        public SideListCaseItem()
        {
            InitializeComponent();
            Loaded += SideListCaseItem_Loaded;
        }

        private void SideListCaseItem_Loaded(object sender, RoutedEventArgs e)
        {
            var caseControlVM = DataContext as CaseControlVM;

            CaseHeight = ConfigurableParameters.Instance.CaseHeight;
            SideListCaseWidth = ConfigurableParameters.Instance.SideListCaseWidth;
            SideListCaseHeight = ConfigurableParameters.Instance.SideListCaseHeight;

            SetPriorityColor(caseControlVM.CaseModel);
            SetBackgroundColor(caseControlVM.CaseModel);
        }

        private void SetPriorityColor(ICase caseData)
        {
            switch (caseData.Priority)
            {
                case Priority.Low:
                    PriorityColor = (Brush)FindResource("Priority.Low.Brush");
                    break;
                case Priority.Med:
                    PriorityColor = (Brush)FindResource("Priority.Mid.Brush");
                    break;
                case Priority.High:
                    PriorityColor = (Brush)FindResource("Priority.High.Brush");
                    break;
                case Priority.Critical:
                    PriorityColor = (Brush)FindResource("Priority.Critical.Brush");
                    break;
                case Priority.NotApplicable:
                    PriorityColor = (Brush)FindResource("Priority.NotApplicable.Brush");
                    break;
                default:
                    break;
            }
        }

        private void SetBackgroundColor(ICase caseData)
        {
            if (caseData.GetType() == typeof(SFCase))
            {
                switch ((caseData.Status as SFStatus).Value)
                {
                    case SFStatusE.New:
                        CaseBackgroundColor = new SolidColorBrush(Color.FromRgb(85, 255, 0));
                        break;
                    case SFStatusE.InReview:
                        CaseBackgroundColor = new SolidColorBrush(Color.FromRgb(196, 255, 0));
                        break;
                    case SFStatusE.Reviewed:
                        CaseBackgroundColor = new SolidColorBrush(Color.FromRgb(160, 209, 0));
                        break;
                    case SFStatusE.InBacklog:
                        CaseBackgroundColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                        break;
                    case SFStatusE.InProcess:
                        CaseBackgroundColor = new SolidColorBrush(Color.FromRgb(240, 232, 0));
                        break;
                    case SFStatusE.InDevelopment:
                        CaseBackgroundColor = new SolidColorBrush(Color.FromRgb(0, 255, 255));
                        break;
                    case SFStatusE.InQA:
                        CaseBackgroundColor = new SolidColorBrush(Color.FromRgb(181, 64, 210));
                        break;
                    case SFStatusE.AwaitingApproval:
                        CaseBackgroundColor = new SolidColorBrush(Color.FromRgb(98, 64, 210));
                        break;
                    case SFStatusE.PendingRelease:
                        CaseBackgroundColor = new SolidColorBrush(Color.FromRgb(32, 233, 213));
                        break;
                    case SFStatusE.OnHold:
                        CaseBackgroundColor = new SolidColorBrush(Color.FromRgb(183, 0, 0));
                        break;
                    case SFStatusE.Closed:
                        CaseBackgroundColor = new SolidColorBrush(Color.FromRgb(0, 62, 88));
                        break;
                    case SFStatusE.Cancelled:
                        CaseBackgroundColor = new SolidColorBrush(Color.FromRgb(87, 9, 0));
                        break;
                    case SFStatusE.ReOpened:
                        CaseBackgroundColor = new SolidColorBrush(Color.FromRgb(237, 68, 249));
                        break;
                    default:
                        break;
                }
            }
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SideListVM.RemoveCaseFromSideList(DataContext as CaseControlVM);
        }
    }
   
}

using ImplementationModel.SalesForce;
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
    public partial class CaseControlDetails : UserControl, INotifyPropertyChanged
    {
        public Storyboard DetailsOpen { get; set; }
        public Storyboard DetailsClose { get; set; }

        private double _calculatedHeight;
        public double CalculatedHeight
        {
            get
            {
                return _calculatedHeight;
            }
            set
            {
                _calculatedHeight = value;
                OnPropertyChanged("CalculatedHeight");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public CaseControlDetails(CaseControl caseControl)
        {
            
            InitializeComponent();
            this.DataContext = this;

            InitializeDetails(caseControl);
            BuildAttachments(caseControl);
        }

        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void BuildAttachments(CaseControl caseControl)
        {
            int sideMarginLeft = 8;
            int sideMarginRight = 182;

            //8,0,182,-6
            foreach (var attachment in caseControl.Case.Attachments)
            {
                attachmentsGrid.Children.Add(new AttachmentButton(attachment, new Thickness(sideMarginLeft, 0, sideMarginRight, -6)));
                sideMarginLeft += 17;
                sideMarginRight -= 17;
            }
        }

        private void InitializeDetails(CaseControl caseControl)
        {
            DetailsOpen = FindResource("DetailsOpen") as Storyboard;
            DetailsClose = FindResource("DetailsClose") as Storyboard;

            Canvas.SetLeft(this, caseControl.Left);
            Canvas.SetTop(this, caseControl.Top);
            Canvas.SetZIndex(this, caseControl.ZIndex - 1);

            subject.Text = caseControl.Case.Subject;
            description.Text = caseControl.Case.Description;

            if (caseControl.Case.GetType() == typeof(SFCase))
            {
                owner.Content = (caseControl.Case as SFCase).Owner?.Name;
                substatus.Content = (caseControl.Case as SFCase).SubStatus;
                creator.Content = (caseControl.Case as SFCase).CreatedBy?.Name;
                dtcreated.Content = (caseControl.Case as SFCase).CreatedDate;



                switch ((caseControl.Case as SFCase).Priority)
                {
                    case "Low":
                        priority.Background = new SolidColorBrush(Color.FromRgb(8, 203, 17));
                        break;
                    case "Med":
                        priority.Background = new SolidColorBrush(Color.FromRgb(232, 255, 1));
                        break;
                    case "High":
                        priority.Background = new SolidColorBrush(Color.FromRgb(255, 105, 1));
                        break;
                    case "Critical":
                        priority.Background = new SolidColorBrush(Color.FromRgb(255, 1, 1));
                        break;
                    default:
                        break;
                }
            }
            caseControl.PropertyChanged += CaseControl_PropertyChanged;
            stackPanel.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            CalculatedHeight = stackPanel.DesiredSize.Height;
        }

        private void CaseControl_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Left") Canvas.SetLeft(this, (sender as CaseControl).Left);
            if (e.PropertyName == "Top") Canvas.SetTop(this, (sender as CaseControl).Top);
        }
    }
}

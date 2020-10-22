using NetCoreForce.Models;
using SFConnectorUI.StateFiles;
using SFLink;
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
using TFSLink;
using ImplementationModel.TFS;

namespace SFConnectorUI.CaseView
{
    /// <summary>
    /// Interaction logic for CaseControl.xaml
    /// </summary>
    public partial class CaseControl : UserControl, INotifyPropertyChanged
    {
        public ICase Case { get; set; }
        public string CaseNumber { get; set; }
        private int _left;
        public int Left
        {
            get
            {
                return _left;
            }
            set
            {
                _left = value;
                NotifyPropertyChanged("Left");
            }
        }

        private int _top; 
        public int Top
        {
            get
            {
                return _top;
            }
            set
            {
                _top = value;
                NotifyPropertyChanged("Top");
            }
        }

        private static int _maxZIndex;

        private int _zIndex;
        public int ZIndex
        {
            get
            {
                return _zIndex;
            }
            set
            {
                _zIndex = value;
                NotifyPropertyChanged("ZIndex");
            }
        }

        private Brush _backGroundColor;
        public Brush BackGroundColor
        {
            get
            {
                return _backGroundColor;
            }
            set
            {
                _backGroundColor = value;
                NotifyPropertyChanged("BackGroundColor");
            }
        }

        

        private CaseControlDetails _userControlDetails;


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }

        private Border _marker;

        public CaseControl(ICase sfCase, int left = 0, int top = 0, bool marked = false)
        {
            InitializeComponent();
            this.DataContext = this;

            Case = sfCase;

            CaseNumber = Case.CaseNumber;

            Left = left;
            Top = top;
            if (marked)
            {
                CreateMarkedDot();
            }

            ZIndex = _maxZIndex++;

            if (sfCase.GetType() == typeof(SFCase))
            {
                switch ((Case.Status as SFStatus).Value)
                {
                    case SFStatusE.New:
                        BackGroundColor = new SolidColorBrush(Color.FromRgb(85, 255, 0));
                        break;
                    case SFStatusE.InReview:
                        BackGroundColor = new SolidColorBrush(Color.FromRgb(196, 255, 0));
                        break;
                    case SFStatusE.Reviewed:
                        BackGroundColor = new SolidColorBrush(Color.FromRgb(160, 209, 0));
                        break;
                    case SFStatusE.InBacklog:
                        BackGroundColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                        break;
                    case SFStatusE.InProcess:
                        BackGroundColor = new SolidColorBrush(Color.FromRgb(240, 232, 0));
                        break;
                    case SFStatusE.InDevelopment:
                        BackGroundColor = new SolidColorBrush(Color.FromRgb(0, 255, 255));
                        break;
                    case SFStatusE.InQA:
                        BackGroundColor = new SolidColorBrush(Color.FromRgb(181, 64, 210));
                        break;
                    case SFStatusE.AwaitingApproval:
                        BackGroundColor = new SolidColorBrush(Color.FromRgb(98, 64, 210));
                        break;
                    case SFStatusE.PendingRelease:
                        BackGroundColor = new SolidColorBrush(Color.FromRgb(32, 233, 213));
                        break;
                    case SFStatusE.OnHold:
                        BackGroundColor = new SolidColorBrush(Color.FromRgb(183, 0, 0));
                        break;
                    case SFStatusE.Closed:
                        BackGroundColor = new SolidColorBrush(Color.FromRgb(0, 62, 88));
                        break;
                    case SFStatusE.Cancelled:
                        BackGroundColor = new SolidColorBrush(Color.FromRgb(87, 9, 0));
                        break;
                    case SFStatusE.ReOpened:
                        BackGroundColor = new SolidColorBrush(Color.FromRgb(237, 68, 249));
                        break;
                    default:
                        break;
                }
            }
            else
            {
                BackGroundColor = new SolidColorBrush(Color.FromRgb(181, 64, 210));
            }
            OperationCompleted += CaseControl_OperationCompleted;
        }

        

        bool captured = false;
        double x_shape, x_canvas, y_shape, y_canvas;
        UIElement source = null;
        private bool _showingDetails;
        private bool _operationInProcess;
        private CaseControlDetailsSideBar _userControlDetailsSideBar;

        private event EventHandler OperationCompleted;

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            source = (UIElement)sender;
            Mouse.Capture(source);
            captured = true;
            ZIndex = _maxZIndex++;
            if (_userControlDetails != null) Canvas.SetZIndex(_userControlDetails, _maxZIndex - 2);

            x_shape = Canvas.GetLeft(source);
            x_canvas = e.GetPosition((Canvas)Parent).X;
            y_shape = Canvas.GetTop(source);
            y_canvas = e.GetPosition((Canvas)Parent).Y;
        }

        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_operationInProcess) return;
            _operationInProcess = true;
            var animation = this.FindResource("Boop") as Storyboard;
            animation.Begin();
            TriggerCaseDetails();
            
        }

        private void CaseControl_OperationCompleted(object sender, EventArgs e)
        {
            _operationInProcess = false;
        }

        public void AnimateHighlight()
        {
            var animation = this.FindResource("Highlight") as Storyboard;
            animation.Begin();
        }

        public void TriggerCaseDetails(bool forceClose = false)
        {
            CloseSideBarDetails();
            if (forceClose) { CloseDetails(); return; }
            if (_showingDetails) { CloseDetails(); }
            else OpenDetails();
            
        }

        private async void OpenDetails()
        {
            var parent = this.Parent as Canvas;
            if (Case.GetType() == typeof(SFCase))
            {
                var attachments = await SFConnector.Instance.GetAttachmentsAsync(Case.Id);
                Case.Attachments = attachments;
            }
            else
            {
                var attachments = await TFSConnector.Instance.GetAttachmentsAsync(Case.Id);
                Case.Attachments = attachments;
            }

            _userControlDetails = new CaseControlDetails(this);
            _userControlDetails.DetailsClose.Completed += DetailsAnimationClose_Completed;
            _userControlDetails.DetailsOpen.Completed += DetailsAnimationOpen_Completed;
            parent.Children.Add(_userControlDetails);
            _userControlDetails.DetailsOpen.Begin();
            _showingDetails = true;
        }

        private void CloseDetails()
        {
            if (!_showingDetails) return;
            mainBorder.CornerRadius = new CornerRadius(8, 8, 8, 8);
            mainBorder.BorderThickness = new Thickness(1, 1, 1, 1);
            foreach (var attachment in _userControlDetails.attachmentsGrid.Children)
            {
                (attachment as AttachmentButton).CloseAttachment.Begin();
            }
            _userControlDetails.DetailsClose.Begin();
            _showingDetails = false;
        }

        private void DetailsAnimationOpen_Completed(object sender, EventArgs e)
        {
            mainBorder.CornerRadius = new CornerRadius(8, 0, 0, 0);
            mainBorder.BorderThickness = new Thickness(1, 1, 0, 0);
            _userControlDetails.DetailsOpen.Completed -= DetailsAnimationOpen_Completed;
            foreach (var attachment in _userControlDetails.attachmentsGrid.Children)
            {
                (attachment as AttachmentButton).ShowAttachment.Begin();
            }
            OperationCompleted?.Invoke(this, new EventArgs());
        }

        private void DetailsAnimationClose_Completed(object sender, EventArgs e)
        {
            var parent = _userControlDetails.Parent as Canvas;
            parent.Children.Remove(_userControlDetails);
            _userControlDetails.DetailsClose.Completed -= DetailsAnimationClose_Completed;
            _userControlDetails = null;
            OperationCompleted?.Invoke(this, new EventArgs());
        }

        private void UserControl_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_marker == null)
            {
                CreateMarkedDot();
                CasesStatesOperations.TriggerMarked(CaseNumber, true);
            }
            else
            {
                mainGrid.Children.Remove(_marker);
                _marker = null;
                CasesStatesOperations.TriggerMarked(CaseNumber, false);
            }
         
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            var animation = this.FindResource("Highlight") as Storyboard;
            animation.Stop();
        }

        private void CreateMarkedDot()
        {
            _marker = new Border()
            {
                Name = "Marker",
                Width = 10,
                Height = 10,
                CornerRadius = new CornerRadius(6),
                Background = Brushes.Red,
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1, 1, 1, 1),
                Margin = new Thickness(49, -5, -5, 13)
            };
            mainGrid.Children.Add(_marker);
        }

       
        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (captured)
            {
                double x = e.GetPosition((Canvas)Parent).X;
                double y = e.GetPosition((Canvas)Parent).Y;
                x_shape += x - x_canvas;
                Left = (int)x_shape;
                x_canvas = x;
                y_shape += y - y_canvas;
                Top = (int)y_shape;
                y_canvas = y;
            }
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            CasesStatesOperations.UpdateCasePosition(CaseNumber, Top, Left);
            Mouse.Capture(null);
            captured = false;
        }


        internal void OpenSideBarDetails()
        {
            var parent = this.Parent as Canvas;

            _userControlDetailsSideBar = new CaseControlDetailsSideBar(this);
            parent.Children.Add(_userControlDetailsSideBar);
            //_showingDetailsSideBar = true;
        }

        internal void CloseSideBarDetails()
        {
            var parent = this.Parent as Canvas;
            parent.Children.Remove(_userControlDetailsSideBar);
            _userControlDetailsSideBar = null;
            //_showingDetailsSideBar = false;
        }
    }

}

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
    public partial class CaseControl : UserControl, INotifyPropertyChanged
    {
        private bool _animateCaseHover;
        public bool AnimateCaseHover
        {
            get
            {
                return _animateCaseHover;
            }
            set
            {
                _animateCaseHover = value;
                NotifyPropertyChanged("AnimateCaseHover");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        public CaseControl()
        {
            InitializeComponent();
        }

        bool captured = false;
        double x_shape, x_canvas, y_shape, y_canvas;
        UIElement source = null;

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            source = (UIElement)sender;

            AnimateCaseHover = true;

            Mouse.Capture(source);
            captured = true;

            (DataContext as CaseControlVM).PutOnTop();

            x_shape = Canvas.GetLeft(this);
            x_canvas = e.GetPosition((Canvas)Parent).X;
            y_shape = Canvas.GetTop(this);
            y_canvas = e.GetPosition((Canvas)Parent).Y;
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (captured)
            {
                Mouse.Capture(null);
                captured = false;
                AnimateCaseHover = false;

                var contentPresenter = VisualTreeHelper.GetParent(this);
                var caseControlVM = VisualTreeHelper.GetParent(contentPresenter);
                var contentPresenter2 = VisualTreeHelper.GetParent(caseControlVM);
                var canvas = (Canvas) VisualTreeHelper.GetParent(contentPresenter2);

                int x = (int)e.GetPosition(canvas).X;
                int y = (int)e.GetPosition(canvas).Y;
                var index = Index.GetIndexFromPosition(x, y, 
                    (DataContext as CaseControlVM).CasesCanvasVM.CaseWidth, (DataContext as CaseControlVM).CasesCanvasVM.CaseHeight);
                var viewmodel = DataContext as CaseControlVM;
                viewmodel.Index = index;
            }

        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (captured)
            {
                var datacontext = (DataContext as CaseControlVM);

                int x = (int)e.GetPosition((Canvas)Parent).X;
                int y = (int)e.GetPosition((Canvas)Parent).Y;

                x_shape += x - x_canvas;
                datacontext.Left = (int)x_shape;
                x_canvas = x;
                y_shape += y - y_canvas;
                datacontext.Top = (int)y_shape;
                y_canvas = y;


                //TODO HIGHLIGHT BAR
                var highlightthisOne = InsideCellPosition();

            }
        }

        private int InsideCellPosition()
        {
            var mousePosition = Mouse.GetPosition((Canvas)this.Parent);
            var (x, y) = Index.GetPositionInsideCaseFromGlobal((int)mousePosition.X, (int)mousePosition.Y, 
                (DataContext as CaseControlVM).CasesCanvasVM.CaseWidth, (DataContext as CaseControlVM).CasesCanvasVM.CaseHeight);
            return (y < ConfigurableParameters.Instance.CaseHeight / 2) ? 1 : -1;
        }


    }
   
}

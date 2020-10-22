using ImplementationModel;
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
using System.Windows.Threading;

namespace SFConnectorFullScreenUI
{
    /// <summary>
    /// Interaction logic for FieldArea.xaml
    /// </summary>
    public partial class CasesCanvas : UserControl
    {
        public CasesCanvas()
        {
            InitializeComponent();
            
            //Dispatcher.BeginInvoke(DispatcherPriority.DataBind,
            //    new Action(delegate () { 
            //        ViewPort = new Rect(0, 0, ConfigurableParameters.Instance.CaseWidth, ConfigurableParameters.Instance.CaseHeight);
            //        ViewBox = new Rect(0, 0, ConfigurableParameters.Instance.CaseWidth, ConfigurableParameters.Instance.CaseHeight);
            //        CaseWidth = ConfigurableParameters.Instance.CaseWidth;
            //        CaseHeight = ConfigurableParameters.Instance.CaseHeight;
            //    }));
        }


        private Point _scrollMousePoint;
        private double _hOff = 1;
        private double _vOff = 1;
        private Selector _selector;

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender as Grid).IsMouseDirectlyOver) return;
            scrollViewerContentGrid.CaptureMouse();
            _scrollMousePoint = e.GetPosition(scrollViewer);
            _vOff = scrollViewer.VerticalOffset;
            _hOff = scrollViewer.HorizontalOffset;

            if (e.ClickCount == 2 && !Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                var position = Mouse.GetPosition((System.Windows.IInputElement)sender);
                (DataContext as CasesCanvasVM).AddCellLabel(position);
            }

            if (e.ClickCount == 2 && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                var position = Mouse.GetPosition((System.Windows.IInputElement)sender);
                _selector = new Selector(selectorCanvas, position);
            }

        }

        private void CalculateIntersections(Rect selector)
        {
            foreach (var caseControl in (DataContext as CasesCanvasVM).CaseControls)
            {
                caseControl.Selected = false;
                var caseRect = new Rect(caseControl.Left, caseControl.Top, 
                    caseControl.CasesCanvasVM.CaseWidth, caseControl.CasesCanvasVM.CaseHeight);
                if (selector.IntersectsWith(caseRect))
                {
                    caseControl.Selected = true;
                }
            }
            
        }

        private void Grid_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (scrollViewerContentGrid.IsMouseCaptured)
            {
                var newOffsetV = _vOff + (_scrollMousePoint.Y - e.GetPosition(scrollViewer).Y);
                scrollViewer.ScrollToVerticalOffset(newOffsetV);
                var newOffsetH = _hOff + (_scrollMousePoint.X - e.GetPosition(scrollViewer).X);
                scrollViewer.ScrollToHorizontalOffset(newOffsetH);
            }
        }

        private void Grid_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            scrollViewerContentGrid.ReleaseMouseCapture();
        }

      
        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_selector == null) return;
            CalculateIntersections(_selector.GetRect());
            _selector = null;
        }
    }
}

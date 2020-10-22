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
    /// Interaction logic for CellLabel.xaml
    /// </summary>
    public partial class CellLabel : UserControl
    {

        HitType MouseHitType = HitType.None;

        // True if a drag is in progress.
        private bool DragInProgress = false;

        // The drag's last point.
        private Point LastPoint;


        public CellLabel()
        {
            InitializeComponent();

            Dispatcher.BeginInvoke(DispatcherPriority.Render,
                new Action(delegate ()
                {
                    note.Focus();
                }));
            
        }

        // Return a HitType value to indicate what is at the point.
        private HitType SetHitType(Point point)
        {
            var viewModel = DataContext as CellLabelVM;
            double left = viewModel.Left;
            double top = viewModel.Top;
            double right = left + viewModel.Width;
            double bottom = top + viewModel.Height;
            if (point.X < left) return HitType.None;
            if (point.X > right) return HitType.None;
            if (point.Y < top) return HitType.None;
            if (point.Y > bottom) return HitType.None;

            const double GAP = 10;
            if (point.X - left < GAP)
            {
                // Left edge.
                if (point.Y - top < GAP) return HitType.UL;
                if (bottom - point.Y < GAP) return HitType.LL;
                return HitType.L;
            }
            else if (right - point.X < GAP)
            {
                // Right edge.
                if (point.Y - top < GAP) return HitType.UR;
                if (bottom - point.Y < GAP) return HitType.LR;
                return HitType.R;
            }
            if (point.Y - top < GAP) return HitType.T;
            if (bottom - point.Y < GAP) return HitType.B;
            return HitType.Body;
        }

        // Set a mouse cursor appropriate for the current hit type.
        private void SetMouseCursor()
        {
            // See what cursor we should display.
            Cursor desired_cursor = Cursors.Arrow;
            switch (MouseHitType)
            {
                case HitType.None:
                    desired_cursor = Cursors.Arrow;
                    break;
                case HitType.Body:
                    desired_cursor = Cursors.ScrollAll;
                    break;
                case HitType.UL:
                case HitType.LR:
                    desired_cursor = Cursors.SizeNWSE;
                    break;
                case HitType.LL:
                case HitType.UR:
                    desired_cursor = Cursors.Arrow;
                    //desired_cursor = Cursors.SizeNESW;
                    break;
                case HitType.T:
                    desired_cursor = Cursors.Hand;
                    break;
                case HitType.B:
                    desired_cursor = Cursors.SizeNS;
                    break;
                case HitType.L:
                case HitType.R:
                    desired_cursor = Cursors.SizeWE;
                    break;
            }

            // Display the desired cursor.
            if (Cursor != desired_cursor) Cursor = desired_cursor;
        }

        // The part of the rectangle the mouse is over.
        private enum HitType
        {
            None, Body, UL, UR, LR, LL, T, B, L, R
        };

        UIElement source = null;
        bool captured = false;
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            source = (UIElement)sender;
            Mouse.Capture(source);
            captured = true;

            var canvas = GetCanvas();

            MouseHitType = SetHitType(Mouse.GetPosition(canvas));
            SetMouseCursor();
            if (MouseHitType == HitType.None) return;

            LastPoint = Mouse.GetPosition(canvas);
            DragInProgress = true;
        }

        // If a drag is in progress, continue the drag.
        // Otherwise display the correct cursor.
        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            var canvas = GetCanvas();
            if (DragInProgress && captured)
            {

                // See how much the mouse has moved.
                Point point = Mouse.GetPosition(canvas);
                double offset_x = point.X - LastPoint.X;
                double offset_y = point.Y - LastPoint.Y;

                // Get the rectangle's current position.
                var viewModel = DataContext as CellLabelVM;
                double new_x = viewModel.Left;
                double new_y = viewModel.Top;
                double new_width = viewModel.Width;
                double new_height = viewModel.Height;

                // Update the rectangle.
                switch (MouseHitType)
                {
                    case HitType.Body:
                        new_x += offset_x;
                        new_y += offset_y;
                        break;
                    case HitType.UL:
                        new_x += offset_x;
                        new_y += offset_y;
                        new_width -= offset_x;
                        new_height -= offset_y;
                        break;
                    case HitType.UR:
                        new_y += offset_y;
                        new_width += offset_x;
                        new_height -= offset_y;
                        break;
                    case HitType.LR:
                        new_width += offset_x;
                        new_height += offset_y;
                        break;
                    case HitType.LL:
                        new_x += offset_x;
                        new_width -= offset_x;
                        new_height += offset_y;
                        break;
                    case HitType.L:
                        new_x += offset_x;
                        new_width -= offset_x;
                        break;
                    case HitType.R:
                        new_width += offset_x;
                        break;
                    case HitType.B:
                        new_height += offset_y;
                        break;
                    case HitType.T:
                        //reposition the label
                        new_y += offset_y;
                        new_x += offset_x;
                        //new_height -= offset_y;
                        break;
                }

                // Don't use negative width or height.
                if ((new_width > 0) && (new_height > 0))
                {
                    
                    // Update the rectangle.
                    //viewModel.Left = UpdateMeasure(viewModel.Left, new_x, true, out bool changed1);
                    //viewModel.Top = UpdateMeasure(viewModel.Top, new_y, false, out bool changed2); 
                    //viewModel.Width = UpdateMeasure(viewModel.Width, new_width, true, out bool changed3);  
                    //viewModel.Height = UpdateMeasure(viewModel.Height, new_height, false, out bool changed4);

                    viewModel.Left = (int)new_x;
                    viewModel.Top = (int)new_y;
                    viewModel.Width = (int)new_width;
                    viewModel.Height = (int)new_height;

                    // Save the mouse's new location.
                    LastPoint = point;
                }
            }
            else
            {
                MouseHitType = SetHitType(Mouse.GetPosition(canvas));
                SetMouseCursor();
            }
        }

        //private int UpdateMeasure(int current, double measure, bool isWidth, out bool changed)
        //{
        //    changed = false;
        //    if (isWidth)
        //    {
        //        var widths = ((int)measure) / ConfigurableParameters.Instance.CaseWidth;
        //        var reminder = ((int)measure) % ConfigurableParameters.Instance.CaseWidth;
        //        if (reminder > ConfigurableParameters.Instance.CaseWidth / 2)
        //        {
        //            widths += 1;
        //            changed = true;
        //        }
        //        return widths * ConfigurableParameters.Instance.CaseWidth;
        //    }
        //    else
        //    {
        //        var heights = ((int)measure) / ConfigurableParameters.Instance.CaseHeight;
        //        var reminder = ((int)measure) % ConfigurableParameters.Instance.CaseHeight;
        //        if (heights < current / ConfigurableParameters.Instance.CaseHeight)
        //        {
        //            heights -= 1;
        //            changed = true;
        //        }
        //        else if (reminder > ConfigurableParameters.Instance.CaseHeight / 2)
        //        {
        //            heights += 1;
        //            changed = true;
        //        }
        //        return heights * ConfigurableParameters.Instance.CaseHeight;
        //    }
        //}
        private Canvas GetCanvas()
        {
            var contentPresenter = VisualTreeHelper.GetParent(this);
            var caseControlVM = VisualTreeHelper.GetParent(contentPresenter);
            var contentPresenter2 = VisualTreeHelper.GetParent(caseControlVM);
            return (Canvas)VisualTreeHelper.GetParent(contentPresenter2);
        }

        // Stop dragging.
        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (captured)
            {
                Mouse.Capture(null);
                captured = false;
                DragInProgress = false;
            }
        }
    }
}

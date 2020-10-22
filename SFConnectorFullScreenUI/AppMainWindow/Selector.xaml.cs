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
    /// Interaction logic for Selector.xaml
    /// </summary>
    public partial class Selector : Border, INotifyPropertyChanged
    {
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
                OnPropertyChanged("Left");
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
                OnPropertyChanged("Top");
            }
        }
        private int _width;
        public int SelectorWidth
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
                OnPropertyChanged("Width");
            }
        }
        private int _height;
        public int SelectorHeight
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
                OnPropertyChanged("Height");
            }
        }

        private Canvas SelectorCanvas;
        private Point _originalZeroPoint;

        public Selector(Canvas selectorCanvas, Point position)
        {
            InitializeComponent();
            SelectorCanvas = selectorCanvas;
            SelectorCanvas.Children.Add(this);

            DataContext = this;
            Left = (int)position.X;
            Top = (int)position.Y;
            _originalZeroPoint = Mouse.GetPosition(SelectorCanvas);
            Mouse.Capture(this);
            _source = this;
        }

        UIElement _source = null;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

       
        // If a drag is in progress, continue the drag.
        // Otherwise display the correct cursor.
        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            
            Point point = Mouse.GetPosition(SelectorCanvas);

            if (point.X > _originalZeroPoint.X)
            {
                SelectorWidth = (int) (point.X - _originalZeroPoint.X);
                Left = (int)_originalZeroPoint.X;
            }

            if (point.Y > _originalZeroPoint.Y)
            {
                SelectorHeight = (int)(point.Y - _originalZeroPoint.Y);
                Top = (int)_originalZeroPoint.Y;
            }

            if (point.X < _originalZeroPoint.X)
            {
                SelectorWidth = (int) (_originalZeroPoint.X - point.X);
                Left = (int)point.X;
            }

            if (point.Y < _originalZeroPoint.Y)
            {
                SelectorHeight = (int)(_originalZeroPoint.Y - point.Y);
                Top = (int)point.Y;
            }


        }

        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);
            SelectorCanvas.Children.Clear();
        }

        public Rect GetRect()
        {
            return new Rect(Left, Top, SelectorWidth, SelectorHeight);
        }
    }
}

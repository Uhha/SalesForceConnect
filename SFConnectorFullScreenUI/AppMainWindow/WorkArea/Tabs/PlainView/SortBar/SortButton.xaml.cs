using ImplementationModel;
using System;
using System.Collections;
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
    /// Interaction logic for SortButton.xaml
    /// </summary>
    public partial class SortButton : Button, INotifyPropertyChanged
    {
        public SortButton()
        {
            InitializeComponent();
            DataContext = this;
            ButtonState = SortButtonState.Inactive;
        }

        private double _scaleValue;
        public double ScaleValue
        {
            get
            {
                return _scaleValue;
            }
            set
            {
                _scaleValue = value;
                NotifyPropertyChanged("ScaleValue");
            }
        }

        private SortButtonState _buttonState;
        public SortButtonState ButtonState
        {
            get
            {
                return _buttonState;
            }
            set
            {
                _buttonState = value;
                switch (value)
                {
                    case SortButtonState.Ascending:
                        ArrowVisibility = Visibility.Visible;
                        ContentVisibility = Visibility.Hidden;
                        ScaleValue = 1;
                        Descending = false;
                        break;
                    case SortButtonState.Descending:
                        ArrowVisibility = Visibility.Visible;
                        ContentVisibility = Visibility.Hidden;
                        ScaleValue = -1;
                        Descending = true;
                        break;
                    case SortButtonState.Inactive:
                        ArrowVisibility = Visibility.Hidden;
                        ContentVisibility = Visibility.Visible;
                        break;
                    default:
                        break;
                }
            }
        }

        private bool _descending;
        public bool Descending
        {
            get
            {
                return _descending;
            }
            set
            {
                _descending = value;
                NotifyPropertyChanged("Descending");
            }
        }

        private Visibility _arrowVisibility;
        public Visibility ArrowVisibility
        {
            get
            {
                return _arrowVisibility;
            }
            set
            {
                _arrowVisibility = value;
                NotifyPropertyChanged("ArrowVisibility");
            }
        }

        private Visibility _contentVisibility;
        public Visibility ContentVisibility
        {
            get
            {
                return _contentVisibility;
            }
            set
            {
                _contentVisibility = value;
                NotifyPropertyChanged("ContentVisibility");
            }
        }

        public SortBy SortBy { get; set;}

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        private void RightButton_Click(object sender, MouseButtonEventArgs e)
        {
            ButtonState = SortButtonState.Inactive;
        }
    }
}

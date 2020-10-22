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
    /// Interaction logic for OperationCompletedMark.xaml
    /// </summary>
    public partial class OperationCompletedMark : UserControl, INotifyPropertyChanged
    {
        private Storyboard _operationFinished;
        private Brush _successBrush;
        private Brush _unSuccessBrush;

        public event PropertyChangedEventHandler PropertyChanged;

        private Brush _fillColor;
        public Brush FillColor
        {
            get
            {
                return _fillColor;
            }
            set
            {
                _fillColor = value;
                NotifyPropertyChanged("FillColor");
            }
        }

        private void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        public OperationCompletedMark()
        {
            InitializeComponent();
            var datacontext = new OperationCompletedMarkVM();
            datacontext.PropertyChanged += Datacontext_PropertyChanged;
            DataContext = datacontext;
            _operationFinished = this.FindResource("CenterPointAnimation") as Storyboard;

            _successBrush =(Brush) FindResource("OperationCompletedSuccessfulBrush");
            _unSuccessBrush = (Brush)FindResource("OperationCompletedUnSuccessfulBrush");
        }

        private void Datacontext_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "StartAnimation")
            {
                OperationFinished((DataContext as OperationCompletedMarkVM).StartAnimation);
            }
        }

        public void OperationFinished(bool successful)
        {
            FillColor = (successful) ? _successBrush : _unSuccessBrush;
            _operationFinished.Begin();
        }
    }
}

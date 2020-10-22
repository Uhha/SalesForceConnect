using ImplementationModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SFConnectorFullScreenUI
{
    /// <summary>
    /// Interaction logic for ArrayTreckerWindow.xaml
    /// </summary>
    public partial class ArrayTreckerWindow : Window, INotifyPropertyChanged
    {
        

        private int _winHeight;
        public int WinHeight
        {
            get
            {
                return _winHeight;
            }
            set
            {
                _winHeight = value;
                NotifyPropertyChanged("WinHeight");
            }
        }

        private int _winWidth;
        public int WinWidth
        {
            get
            {
                return _winWidth;
            }
            set
            {
                _winWidth = value;
                NotifyPropertyChanged("WinWidth");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertName));
        }

        private const int SCALE = 10;

        public ArrayTreckerWindow()
        {
            InitializeComponent();
            DataContext = this;
            //WinWidth = MainCaseController.GetNumberOfCellsHorizontal(CaseType.SF) * SCALE;
           // WinHeight = MainCaseController.GetNumberOfCellsVertical(CaseType.SF) * SCALE;
        }

        private void Refresh()
        {
            //<Border Background="White" Width="1" Height="1" Canvas.Top="3" Canvas.Left="2"></Border>
            canvas.Children.Clear();

            Type type = typeof(UserControl);
            FieldInfo info = type.GetField("CaseControls", BindingFlags.Public | BindingFlags.Instance);
            var value = info.GetValue(null) as CaseControl[,];

            for (int i = 0; i < value.GetLength(0); i++)
            {
                for (int j = 0; j < value.GetLength(1); j++)
                {
                    if (value[i, j] == null) continue;
                    var border = new Border()
                    {
                        Background = Brushes.White,
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(1),
                        Width = SCALE,
                        Height = SCALE
                    };
                    Canvas.SetTop(border, j*SCALE);
                    Canvas.SetLeft(border, i*SCALE);
                    canvas.Children.Add(border);
                }
            }
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Refresh();
        }

    }
}

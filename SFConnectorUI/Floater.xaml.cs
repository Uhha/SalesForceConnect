using ImplementationModel;
using SFConnectorUI.CaseView;
using SFLink;
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
using TFSLink;

namespace SFConnectorUI
{
    /// <summary>
    /// Interaction logic for Floater.xaml
    /// </summary>
    public partial class Floater : UserControl, INotifyPropertyChanged
    {
        private string _selectedCaseQueue;
        public string SelectedCaseQueue
        {
            get
            {
                return _selectedCaseQueue;
            }
            set
            {
                _selectedCaseQueue = value;
                OnPropertyChanged("SelectedCaseQueue");
            }
        }

        private string _selectedCaseTFSQueue;
        public string SelectedCaseTFSQueue
        {
            get
            {
                return _selectedCaseTFSQueue;
            }
            set
            {
                _selectedCaseTFSQueue = value;
                OnPropertyChanged("SelectedCaseTFSQueue");
            }
        }

        private SFSortByType _sortByType;
        public SFSortByType SortByType
        {
            get
            {
                return _sortByType;
            }
            set
            {
                _sortByType = value;
                OnPropertyChanged("SortByTypeType");
            }
        }

        private string _caseNumberToSearch;
        public string CaseNumberToSearch
        {
            get
            {
                return _caseNumberToSearch;
            }
            set
            {
                _caseNumberToSearch = value;
                OnPropertyChanged("CaseNumberToSearch");
            }
        }

        private ICommand _enterKeyCommand;
        public ICommand EnterKeyCommand
        {
            get
            {
                return _enterKeyCommand
                    ?? (_enterKeyCommand = new ActionCommand(async () =>
                    {
                        var trimmedNumber = CaseNumberToSearch.TrimStart('0');
                        var foundCase = MainWindow.CaseControls.FirstOrDefault(o => o.CaseNumber.TrimStart('0') == trimmedNumber);
                        if (foundCase != null)
                        {
                            foundCase.AnimateHighlight();
                        }
                        else
                        {
                            var parent = Parent as Canvas;
                            var mainWindow = parent.Parent as MainWindow;
                            var createdCase = await mainWindow.OpenCasePreview(trimmedNumber);
                        }
                    }));
            }
        }

        

        public event PropertyChangedEventHandler PropertyChanged;

        bool captured = false;
        double x_shape, x_canvas, y_shape, y_canvas;
        UIElement source = null;

        private void OnPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));

            if (info == "SortByTypeType")
            {
                switch (SortByType)
                {
                    case SFSortByType.CaseNumber:
                        CaseSorter.Sort(o => o.Case.CaseNumber);
                        break;
                    //case SFSortByType.DateOpened:
                    //    CaseSorter.Sort(o => o.Case.CreatedDate);
                    //    break;
                    //case SFSortByType.PersonOpened:
                    //    CaseSorter.Sort(o => o.Case.CreatedBy.Name);
                    //    break;
                    default:
                        break;
                }
            }
        }

        public Floater()
        {
            InitializeComponent();
            DataContext = this;

            var queues = SFConnector.Instance.GetCasesQueues();
            casesLists.ItemsSource = queues.Result; //Enum.GetValues(typeof(SFCasesListType)).Cast<SFCasesListType>();
            casesLists.SelectedItem = queues.Result[0];
            SelectedCaseQueue = queues.Result[0];

            var tfsQueues = TFSConnector.Instance.GetCasesQueues();
            casesListsTFS.ItemsSource = tfsQueues.Result;
            casesListsTFS.SelectedItem = tfsQueues.Result[0];
            SelectedCaseTFSQueue = tfsQueues.Result[0];
            

            sortOrderList.ItemsSource = Enum.GetValues(typeof(SFSortByType)).Cast<SFSortByType>();
        }


        private void CasesLists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            SelectedCaseQueue = (string)comboBox.SelectedItem;
        }

        private void Floater_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);
            captured = false;
        }

        private void Floater_MouseDown(object sender, MouseButtonEventArgs e)
        {
            source = (UIElement)sender;
            Mouse.Capture(source);
            captured = true;

            x_shape = Canvas.GetLeft(source);
            x_canvas = e.GetPosition((Canvas)Parent).X;
            y_shape = Canvas.GetTop(source);
            y_canvas = e.GetPosition((Canvas)Parent).Y;
        }

        private void CasesListsTFS_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            SelectedCaseTFSQueue = (string)comboBox.SelectedItem;
        }

        private void SortOrder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            SortByType = (SFSortByType)comboBox.SelectedItem;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CaseSorter.SortInOneLine(o => o.Case.CaseNumber);
        }

        private void Floater_MouseMove(object sender, MouseEventArgs e)
        {
            if (captured)
            {
                double x = e.GetPosition((Canvas)Parent).X;
                double y = e.GetPosition((Canvas)Parent).Y;
                x_shape += x - x_canvas;
                Canvas.SetLeft(this, (int)x_shape);
                x_canvas = x;
                y_shape += y - y_canvas;
                Canvas.SetTop(this, (int)y_shape);
                y_canvas = y;
            }
        }
    }

    public class ActionCommand : ICommand
    {
        private readonly Action _action;

        public ActionCommand(Action action)
        {
            _action = action;
        }

        public void Execute(object parameter)
        {
            _action();
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}

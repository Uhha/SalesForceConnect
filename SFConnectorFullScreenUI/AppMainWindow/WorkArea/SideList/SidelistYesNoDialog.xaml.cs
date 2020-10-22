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
    /// Interaction logic for YesNoDialog.xaml
    /// </summary>
    public partial class SidelistYesNoDialog : Window, INotifyPropertyChanged
    {
        private ICommand _createTab;
        public ICommand CreateTab {
            get
            {
                return _createTab;
            }
            set
            {
                _createTab = value;
                NotyfyPropertChanged("CreateTab");
            }
        }

        private void NotyfyPropertChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public SidelistYesNoDialog()
        {
            InitializeComponent();
            DataContext = this;
            CreateTab = new RelayCommand(DoCreateTab);

            Application curApp = Application.Current;
            Window mainWindow = curApp.MainWindow;
            this.Left = mainWindow.Left + (mainWindow.Width - this.ActualWidth) / 2 - (this.Width / 2);
            this.Top = mainWindow.Top + (mainWindow.Height - this.ActualHeight) / 2 - (this.Height / 2);
            this.Show();
            this.tabName.Focus();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void DoCreateTab()
        {
            if (string.IsNullOrEmpty(tabName.Text)) return;
            //SideListVM.CreateNewTabFromCases(tabName.Text);
            //SideList.Dialog = null;
            this.Close();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            DoCreateTab();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            //SideList.Dialog = null;
            this.Close();
        }
    }
}

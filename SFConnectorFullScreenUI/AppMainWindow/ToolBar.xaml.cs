using ImplementationModel;
using SFConnectorFullScreenUI.Data.PositionsState;
using SFLink;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace SFConnectorFullScreenUI.AppMainWindow
{
    /// <summary>
    /// Interaction logic for ToolBar.xaml
    /// </summary>
    public partial class ToolBar : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        private void OnPropertyChanged(string propertName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertName));
        }

        public ToolBar()
        {
            InitializeComponent();

            DataContext = this;
            Loaded += ToolBar_Loaded;
            tfsQueue.DropDownOpened += TfsQueue_DropDownOpened;
            sfQueue.DropDownOpened += SfQueue_DropDownOpened;
        }

        private void SfQueue_DropDownOpened(object sender, EventArgs e)
        {
            if ((sender as ComboBox).Items.Count == 0)
            {
                BuildSFQueuesAsync();
            }
        }

        private void TfsQueue_DropDownOpened(object sender, EventArgs e)
        {
            if((sender as ComboBox).Items.Count == 0)
            {
                BuildTFSQueuesAsync();
            }
        }

        private void ToolBar_Loaded(object sender, RoutedEventArgs e)
        {
            BuildTFSQueuesAsync();
            BuildSFQueuesAsync();
        }

        private async Task BuildTFSQueuesAsync()
        {
            TFSSpinner.StartSpinning();
            bool success;
            string[] tfsQueues = default(string[]);
            try
            {
                tfsQueues = await Task.Run(() => ConnectionChecker.TryNTimesAsync<string[]>(TFSConnector.Instance.GetCasesQueues, 1, "Generating TFS Queues"));
                success = true;
            }
            catch (Exception ex)
            {
                LoggerController.Log(ex);
                success = false;
            }
            tfsQueue.ItemsSource = tfsQueues;
            TFSSpinner.StopSpinning(success);
        }

        private async Task BuildSFQueuesAsync()
        {
            SFSpinner.StartSpinning();
            bool success;
            string[] sfQueues = default(string[]);
            try
            {
                sfQueues = await Task.Run(() => ConnectionChecker.TryNTimesAsync<string[]>(SFConnector.Instance.GetCasesQueues, 1, "Generating SalesForce Queues"));
                success = true;
            }
            catch (Exception ex)
            {
                LoggerController.Log(ex);
                success = false;
            }
            sfQueue.ItemsSource = sfQueues;
            SFSpinner.StopSpinning(success);
        }

        private async void tfsQueue_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            var selection = (string)comboBox.SelectedItem;
            bool success;
            TFSSpinner.StartSpinning();
            try
            {
                LoggerController.Log("Building TFS Cases");
                await MainCaseController.BuildTFSCasesAsync(selection);
                success = true;
            }
            catch (Exception ex)
            {
                LoggerController.Log(ex);
                success = false;
            }
            TFSSpinner.StopSpinning(success);
        }

        private async void sfQueue_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            var selection = (string)comboBox.SelectedItem;
            bool success;
            SFSpinner.StartSpinning();
            try
            {
                LoggerController.Log("Building SF Cases");
                await MainCaseController.BuildSFCasesAsync(selection);
                success = true;
            }
            catch (Exception ex)
            {
                LoggerController.Log(ex);
                success = false;
            }
            SFSpinner.StopSpinning(success);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CasesStatesOperations.SaveState();
        }
    }
}

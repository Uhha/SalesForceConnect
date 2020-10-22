using ImplementationModel;
using SFConnectorUI.CaseView;
using SFConnectorUI.StateFiles;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<CaseControl> CaseControls = new List<CaseControl>();
        public static int ScreenResolutionWidth = (int)SystemParameters.FullPrimaryScreenWidth;
        public static int ScreenResolutionHeight = (int)SystemParameters.FullPrimaryScreenHeight;

        private GroupDottedRectangle _dottedRect;

        private CaseControl _casePreview;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            
            floater.PropertyChanged += Floater_PropertyChanged;
            FeedCasesAsync("Default");
        }

        

        private void Floater_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedCaseQueue")
            {
                var floater = sender as Floater;
                FeedCasesAsync((string)floater.casesLists.SelectedValue);
            }
            else if (e.PropertyName == "SelectedCaseTFSQueue")
            {
                var floater = sender as Floater;
                FeedTFSCasesAsync((string)floater.casesListsTFS.SelectedValue);
            }
            else if (e.PropertyName == "SortByTypeType")
            {
                ClearSortRectangle();
            }
        }

        

        public async void FeedCasesAsync(string casesQueue)
        {
            ClearCaseControls();
            var cases = await SFConnector.Instance.GetCasesAsync(casesQueue);

            foreach (var sfcase in cases)
            {
                CaseInfo caseInfo = CasesStatesOperations.GetCaseInfo(sfcase.CaseNumber);
                var formedCase = new CaseControl(sfcase, caseInfo.Left, caseInfo.Top, caseInfo.Marked);
                LayoutRoot.Children.Add(formedCase);
                CaseControls.Add(formedCase);
            }

            //LayoutRoot.Children.Add(new TestControl());
        }

        private async void FeedTFSCasesAsync(string casesTFSQueue)
        {
            var cases = await TFSConnector.Instance.GetCasesAsync(casesTFSQueue);

            foreach (var tfscase in cases)
            {
                CaseInfo caseInfo = CasesStatesOperations.GetCaseInfo(tfscase.CaseNumber);
                var formedCase = new CaseControl(tfscase, caseInfo.Left, caseInfo.Top, caseInfo.Marked);
                LayoutRoot.Children.Add(formedCase);
                CaseControls.Add(formedCase);
            }
        }

        private void ClearCaseControls()
        {
            foreach (var item in CaseControls)
            {
                LayoutRoot.Children.RemoveCase(item);
            }
            CaseControls.Clear();
            ClearSortRectangle();
        }

        public async Task<CaseControl> OpenCasePreview(string trimmedCaseNumber)
        {
            if (_casePreview != null)
            {
                LayoutRoot.Children.RemoveCase(_casePreview);
                _casePreview = null;
            }
            var sfcase = await SFConnector.Instance.GetTheCase(trimmedCaseNumber);
            if (sfcase == null) return null;
            _casePreview = new CaseControl(sfcase, ScreenResolutionWidth / 2, ScreenResolutionHeight / 2);
            LayoutRoot.Children.Add(_casePreview);
            _casePreview.TriggerCaseDetails();
            return _casePreview;
        }

        public void DrawSortRectangle(int left, int top, int right, int bot)
        {
            _dottedRect = new GroupDottedRectangle(floater.SortByType , left, top, right, bot);
            this.LayoutRoot.Children.Add(_dottedRect);
        }

        public void ClearSortRectangle()
        {
            if (_dottedRect != null) this.LayoutRoot.Children.Remove(_dottedRect);
        }
    }
}

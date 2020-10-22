using ImplementationModel;
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

namespace SFConnectorFullScreenUI
{
    /// <summary>
    /// Interaction logic for FieldAreaList.xaml
    /// </summary>
    public partial class FieldAreaList : UserControl
    {
        private string _tabIdentifier { get; set; }
        private CaseType CaseType { get; set; }
        private bool _dataContextLoaded;
        private bool _columnsLoaded;


        public FieldAreaList()
        {
            InitializeComponent();
            //Empty DataContext preload to avoid System.Windows.Data Error: 40 : BindingExpression path error
            DataContext = new FieldAreaListVM(CaseType.Unknown);
            Loaded += FieldAreaList_Loaded;
            Application.Current.Exit += CurrentOnExit;
        }

        private void FieldAreaList_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_dataContextLoaded)
            {
                DataContext = new FieldAreaListVM(CaseType);
                _dataContextLoaded = true;
            }

            if (_tabIdentifier == null)
                _tabIdentifier = this.Name;

            if (!_columnsLoaded)
                LoadColumnState();
        }

        private void LoadColumnState()
        {
            try
            {
                var columnState = ColumnSettingsOperations.LoadColumnState(_tabIdentifier);
                if (columnState == null) return;

                int newIndex = 0;
                foreach (var col in columnState)
                {
                    int oldIndex = 0;
                    for (int i = 0; i < casesGridView.Columns.Count; i++)
                    {
                        if (casesGridView.Columns[i].Header.ToString().Equals(col.ColumnName))
                        {
                            oldIndex = i;
                            break;
                        }

                    }
                    casesGridView.Columns[oldIndex].Width = col.Width;
                    casesGridView.Columns.Move(oldIndex, newIndex++);
                }
                _columnsLoaded = true;
            }
            catch (Exception e)
            {
                LoggerController.Log(e, "Unable to read columns setting file or file has invalid formatting");
            }
        }

        private void CurrentOnExit(object sender, ExitEventArgs e)
        {
            Application.Current.Exit -= CurrentOnExit;
            ColumnSettingsOperations.UpdateColumnState(_tabIdentifier, casesGridView.Columns);
        }

        #region DependencyProperty

        public static readonly DependencyProperty FiledAreaTypeProperty = DependencyProperty.Register("FiledAreaType", typeof(CaseType),
         typeof(FieldAreaList), new PropertyMetadata(CaseType.Unknown, new PropertyChangedCallback(OnFiledAreaTypeChanged)));

        public CaseType FiledAreaType
        {
            get { return (CaseType)GetValue(FiledAreaTypeProperty); }
            set { SetValue(FiledAreaTypeProperty, value); }
        }

        private static void OnFiledAreaTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FieldAreaList UserControlFieldArea = d as FieldAreaList;
            UserControlFieldArea.OnFiledAreaTypeChanged(e);
        }

        private void OnFiledAreaTypeChanged(DependencyPropertyChangedEventArgs e)
        {
            CaseType = (CaseType)e.NewValue;
        }

        #endregion
    }
}

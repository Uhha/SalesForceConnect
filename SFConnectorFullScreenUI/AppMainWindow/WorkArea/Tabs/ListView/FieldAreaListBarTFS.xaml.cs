using ImplementationModel;
using SFLink;
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
    /// Interaction logic for FieldAreaListBar.xaml
    /// </summary>
    public partial class FieldAreaListBarTFS : UserControl
    {
        public FieldAreaListBarTFS()
        {
            InitializeComponent();
            DataContext = new FieldAreaListBarVM();
            queueComboBox.Loaded += QueueComboBox_Loaded;
            Loaded += FieldAreaListBarTFS_Loaded;
        }

        private void FieldAreaListBarTFS_Loaded(object sender, RoutedEventArgs e)
        {
            var fieldAreaList = TryToFindCasesCanvas();

            if (fieldAreaList != null)
            {
                fieldAreaList.Loaded += FieldAreaList_Loaded;
            }

        }

        private void FieldAreaList_Loaded(object sender, RoutedEventArgs e)
        {
            var dataContext = (DataContext as FieldAreaListBarVM);
            dataContext.FieldAreaListVM = TryToFindCasesCanvas()?.DataContext as FieldAreaListVM;
        }

        private void QueueComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var dc = (this.DataContext as FieldAreaListBarVM);
            var qcdc = (QueueComboBoxVM)queueComboBox.DataContext;
            dc.QueueComboBoxVM = qcdc;
        }


        private FieldAreaList TryToFindCasesCanvas()
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(Parent); i++)
            {
                var child = VisualTreeHelper.GetChild(Parent, i);
                if (child.GetType() == typeof(FieldAreaList))
                {
                    return child as FieldAreaList;
                }
            }
            return null;
        }


    }
}

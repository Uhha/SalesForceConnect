using ImplementationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for GroupButton.xaml
    /// </summary>
    public partial class GroupButton : ToggleButton
    {
        public GroupBy GroupBy { get; set; }

        public GroupButton()
        {
            InitializeComponent();
        }
    }
}

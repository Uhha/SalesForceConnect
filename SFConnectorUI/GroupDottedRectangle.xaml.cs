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

namespace SFConnectorUI
{
    /// <summary>
    /// Interaction logic for GroupDottedRectangle.xaml
    /// </summary>
    public partial class GroupDottedRectangle : UserControl
    {
        public GroupDottedRectangle(SFSortByType sortByType, int left, int top, int right, int bot)
        {
            InitializeComponent();

            const int rectGap = 15;
           
            Height = bot - top + (2 * rectGap);
            Width = right - left + (2 * rectGap);
            Canvas.SetLeft(this, left - rectGap);
            Canvas.SetTop(this, top - rectGap);

            label.Content = sortByType.ToString();
        }
    }
}

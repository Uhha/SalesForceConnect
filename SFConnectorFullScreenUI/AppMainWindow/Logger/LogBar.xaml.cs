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
    /// Interaction logic for LogBar.xaml
    /// </summary>
    public partial class LogBar : UserControl
    {
        public LogBar()
        {
            InitializeComponent();
        }

        public void Log(string text)
        {
            log.Text = text + Environment.NewLine + log.Text;
        }
    }
}

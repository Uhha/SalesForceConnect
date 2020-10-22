using ImplementationModel;
using SFLink;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Threading;

namespace SFConnectorFullScreenUI
{
    /// <summary>
    /// Interaction logic for SortBar.xaml
    /// </summary>
    public partial class SortBarPannel : UserControl, INotifyPropertyChanged
    {
        private SortButton _lastSortButtonClicked;

        public SortBarPannel()
        {
            InitializeComponent();

            //Dispatcher.BeginInvoke(DispatcherPriority.DataBind,
            //    new Action(delegate ()
            //    {
            //        (DataContext as SortBarPannelVM).ActualWidth = this.ActualWidth;

            //    }));

        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            SizeChanged += SortBarPannel_SizeChanged;
        }

        private void SortBarPannel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            (DataContext as SortBarPannelVM).CasesCanvasVM.LongCaseWidth = (int)this.ActualWidth;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var tbutton in groupButtonsStackPannel.Children.OfType<ToggleButton>())
            {
                if (tbutton == sender) continue;
                tbutton.IsChecked = false;
            }
            (DataContext as SortBarPannelVM).SetGroupBy((sender as GroupButton).GroupBy);
            ResortWithLastUsedParameters();
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            (DataContext as SortBarPannelVM).SetGroupBy(GroupBy.None);
            ResortWithLastUsedParameters();
        }

        private void SortButton_Click(object sender, RoutedEventArgs e)
        {
            var sortButton = sender as SortButton;
            sortButton.ButtonState = (sortButton.ButtonState == SortButtonState.Ascending) ?
               sortButton.ButtonState = SortButtonState.Descending : sortButton.ButtonState = SortButtonState.Ascending;

            var parent = sortButton.Parent as StackPanel;
            //Reset All Button States
            foreach (var button in parent.Children.OfType<SortButton>())
            {
                if (button == sender) continue;
                button.ButtonState = SortButtonState.Inactive;
            }
            _lastSortButtonClicked = sortButton;
            (DataContext as SortBarPannelVM).SortCases(sortButton.Descending, sortButton.SortBy);
        }

        private void ResortWithLastUsedParameters()
        {
            if (_lastSortButtonClicked == null) return;
            var parent = _lastSortButtonClicked.Parent as StackPanel;
            (DataContext as SortBarPannelVM).SortCases(_lastSortButtonClicked.Descending, _lastSortButtonClicked.SortBy);
        }

        private void SwapAxis_Checked(object sender, RoutedEventArgs e)
        {
            (DataContext as SortBarPannelVM).CasesCanvasVM.SwapAxis = true;
            ResortWithLastUsedParameters();
        }

        private void SwapAxis_Unchecked(object sender, RoutedEventArgs e)
        {
            (DataContext as SortBarPannelVM).CasesCanvasVM.SwapAxis = false;
            ResortWithLastUsedParameters();
        }

        
    }


}

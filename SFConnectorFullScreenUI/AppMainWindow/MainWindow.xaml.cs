using System;
using System.Collections.Generic;
using System.Configuration;
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
using ImplementationModel;

namespace SFConnectorFullScreenUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        

        //TEST
        //private ArrayTreckerWindow _arrayTreckerWindow;

        //private CellOpacityController _cellOpacityController;

        public MainWindow()
        {
            //Increase max window area by 6px to accomodate erroneous calculations by default app settings
            //with window style = none, plus 1px to overlap area - looks better
            MaxHeight = SystemParameters.WorkArea.Height + 7;
            MaxWidth = SystemParameters.WorkArea.Width + 7;
            InitializeComponent();
            DataContext = this;

            this.CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, this.OnCloseWindow));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, this.OnMaximizeWindow, this.OnCanResizeWindow));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, this.OnMinimizeWindow, this.OnCanMinimizeWindow));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, this.OnRestoreWindow, this.OnCanResizeWindow));

            Loaded += MainWindow_Loaded;
            Application.Current.Exit += CurrentOnExit;

            //this.Closed += MainWindow_Closed;
            //Cell Opacity has some Issues perfomance
            //_cellOpacityController = new CellOpacityController(this);
            
        }

        private void CurrentOnExit(object sender, ExitEventArgs e)
        {
            Application.Current.Exit -= CurrentOnExit;
            foreach (var tab in WorkAreaVM.Tabs)
            {
                tab.Content.CasesCanvasVM.UpdateCasesPositions();
            }
            
            CasesStatesOperations.SaveState();
            ColumnSettingsOperations.SaveState();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoggerController.Init(logger, Dispatcher);
            

            //TEST

            //_arrayTreckerWindow = new ArrayTreckerWindow();
            //_arrayTreckerWindow.Show();
        }

        /// <summary>
        /// TitleBar_MouseDown - Drag if single-click, resize if double-click
        /// </summary>
        private void TitleBar_LeftMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2 )
            {
                AdjustWindowSize();
            }
            else
            {
                Application.Current.MainWindow.DragMove();
            }
        }

        private void AdjustWindowSize()
        {
            if (this.WindowState == WindowState.Maximized)
            {
                OnRestoreWindow(this, null);
            }
            else
            {
                OnMaximizeWindow(this, null);
            }
        }

        private void OnCanResizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.ResizeMode == ResizeMode.CanResize || this.ResizeMode == ResizeMode.CanResizeWithGrip;
        }

        private void OnCanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.ResizeMode != ResizeMode.NoResize;
        }

        private void OnCloseWindow(object target, ExecutedRoutedEventArgs e)
        {
            //SystemCommands.CloseWindow(_arrayTreckerWindow);
            SystemCommands.CloseWindow(this);
        }

        private void OnMaximizeWindow(object target, ExecutedRoutedEventArgs e)
        {
            Maximize.Visibility = Visibility.Collapsed;
            Restore.Visibility = Visibility.Visible;

            //Correct Window Cutout
            RootWindow.Margin = new Thickness(6, 6, 0, 0);

            //Remove Window Outline on FullScreen
            this.BorderBrush = null;
            this.BorderThickness = new Thickness(0);

            SystemCommands.MaximizeWindow(this);
        }

        private void OnMinimizeWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        private void OnRestoreWindow(object target, ExecutedRoutedEventArgs e)
        {
            Maximize.Visibility = Visibility.Visible;
            Restore.Visibility = Visibility.Collapsed;

            //Correct Window Cutout
            RootWindow.Margin = new Thickness(0);

            //Apply Window Outline
            this.BorderBrush = (SolidColorBrush)Application.Current.Resources["AccentBrush"];
            this.BorderThickness = new Thickness(1);
               
            SystemCommands.RestoreWindow(this);
        }
    }
}

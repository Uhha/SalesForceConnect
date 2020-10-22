using ImplementationModel.SalesForce;
using SFConnectMobile.ViewModels;
using SFConnectMobile.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SFConnectMobile.CustomUIHelpers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CaseCell : ViewCell
    {
        public bool HasLeftButton { get; set; }
        public static readonly BindableProperty HasLeftButtonProperty = BindableProperty.Create(
                                                propertyName: "HasLeftButton",
                                                returnType: typeof(bool),
                                                declaringType: typeof(CaseCell),
                                                defaultValue: false,
                                                defaultBindingMode: BindingMode.TwoWay,
                                                propertyChanged: HasLeftButtonPropertyChanged);

        private static void HasLeftButtonPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CaseCell)bindable;
            control.HasLeftButton = (bool)newValue;
            //control.LeftButton1.IsVisible = (bool)newValue;
        }

        public bool HasRightButton { get; set; }
        public static readonly BindableProperty HasRightButtonProperty = BindableProperty.Create(
                                                propertyName: "HasRightButton",
                                                returnType: typeof(bool),
                                                declaringType: typeof(CaseCell),
                                                defaultValue: false,
                                                defaultBindingMode: BindingMode.TwoWay,
                                                propertyChanged: HasRightButtonPropertyChanged);

        private static void HasRightButtonPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CaseCell)bindable;
            control.HasRightButton = (bool)newValue;
            //control.RightButton1.IsVisible = (bool)newValue;
        }

        public ListView ParentListView { get; set; }
        public static readonly BindableProperty ParentListViewProperty = BindableProperty.Create(
                                                propertyName: "ParentListView",
                                                returnType: typeof(ListView),
                                                declaringType: typeof(CaseCell),
                                                defaultValue: null,
                                                defaultBindingMode: BindingMode.TwoWay,
                                                propertyChanged: ParentListViewPropertyChanged);

        private static void ParentListViewPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CaseCell)bindable;
            control.ParentListView = newValue as ListView;
        }

        public ContentPage ParentContentPage { get; set; }
        public static readonly BindableProperty ParentContentPageProperty = BindableProperty.Create(
                                                propertyName: "ParentContentPage",
                                                returnType: typeof(ContentPage),
                                                declaringType: typeof(CaseCell),
                                                defaultValue: null,
                                                defaultBindingMode: BindingMode.TwoWay,
                                                propertyChanged: ParentContentPageChanged);

        private static void ParentContentPageChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CaseCell)bindable;
            control.ParentContentPage = newValue as ContentPage;
        }

        public Button LeftButton { get; set; }
        public static readonly BindableProperty LeftButtonProperty = BindableProperty.Create(
                                                propertyName: "LeftButton",
                                                returnType: typeof(Button),
                                                declaringType: typeof(CaseCell),
                                                defaultValue: null,
                                                defaultBindingMode: BindingMode.TwoWay,
                                                propertyChanged: LeftButtonChanged);

        private static void LeftButtonChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CaseCell)bindable;
            var thebutton = newValue as Button;
            control.LeftButton = thebutton;
            control.buttonHolder.Children.Add(thebutton);
            thebutton.SetValue(Grid.ColumnProperty, 0);
        }

        public Button RightButton { get; set; }
        public static readonly BindableProperty RightButtonProperty = BindableProperty.Create(
                                                propertyName: "RightButton",
                                                returnType: typeof(Button),
                                                declaringType: typeof(CaseCell),
                                                defaultValue: null,
                                                defaultBindingMode: BindingMode.TwoWay,
                                                propertyChanged: RightButtonChanged);

        private static void RightButtonChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CaseCell)bindable;
            var thebutton = newValue as Button;
            control.RightButton = thebutton;
            control.buttonHolder.Children.Add(thebutton);
            thebutton.SetValue(Grid.ColumnProperty, 2);
        }

        public CaseCell()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_prevListItem != null)
            {
                (_prevListItem as Grid).Margin = new Thickness(0, 0, 0, 5);
            }
        }

        private static object _prevListItem;

        private void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            try
            {
                if (!HasLeftButton && !HasRightButton) return;

                if (_prevListItem != null && sender != _prevListItem)
                {
                    (_prevListItem as Grid).Margin = new Thickness(0, 0, 0, 5);
                }

                _prevListItem = sender;

                var grid = sender as Grid;
                var stack = grid.Children.FirstOrDefault(o => o.GetType() == typeof(StackLayout));
                switch (e.StatusType)
                {
                    case GestureStatus.Started:
                        {
                            if (ParentListView != null) ParentListView.IsPullToRefreshEnabled = false;
                        }
                        break;

                    case GestureStatus.Running:
                        {
                            var _gestureX = e.TotalX;
                            if (!HasRightButton && _gestureX + grid.Margin.Right < 0)
                            {
                                return;
                            }
                            if (!HasLeftButton && _gestureX + grid.Margin.Left > 0)
                            {
                                return;
                            }

                            _gestureX = Math.Min(_gestureX, 100);
                            _gestureX = Math.Max(_gestureX, -100);
                            stack.Margin = new Thickness(_gestureX, 0, (-_gestureX), 0);
                        }
                        break;

                    case GestureStatus.Completed:
                    case GestureStatus.Canceled:
                        {
                            if (ParentListView != null) ParentListView.IsPullToRefreshEnabled = true;
                            var leftmargin = (stack.Margin.Left + grid.Margin.Left < 0) ? -100 : 100;
                            leftmargin = (stack.Margin.Left + grid.Margin.Left < 50 && stack.Margin.Left + grid.Margin.Left > -50) ? 0 : leftmargin;
                            grid.Margin = new Thickness(leftmargin, 0, -leftmargin, 5);
                            stack.Margin = new Thickness(0);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                throw ex;
            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var view = sender as View;

            var item = view.BindingContext as SFCase;
            if (item == null)
                return;

            await ParentContentPage.Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));
            ParentListView.SelectedItem = null;
        }

    }
}
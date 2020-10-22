using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using SFConnectMobile.Models;
using SFConnectMobile.ViewModels;

namespace SFConnectMobile.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : MasterDetailPage
    {
        Dictionary<HomeMenuItem, NavigationPage> MenuPages = new Dictionary<HomeMenuItem, NavigationPage>();
        public MainPage()
        {
            InitializeComponent();
            MasterBehavior = MasterBehavior.Popover;
            MessagingCenter.Subscribe<ManualItemsPage>(this, "OpenSideMenu", (arg) => this.IsPresented = true);
        }

        public async Task NavigateFromMenu(HomeMenuItem homeMenuItem)
        {
            if (!MenuPages.ContainsKey(homeMenuItem))
            {
                ContentPage subPage = new ContentPage();

                switch (homeMenuItem.MenuItemType)
                {
                    case ImplementationModel.MenuItemType.StartingPage:
                    case ImplementationModel.MenuItemType.NotSet:
                    default:
                        subPage = new StartingPage();
                        break;
                    case ImplementationModel.MenuItemType.Search:
                        subPage = new SearchPage();
                        break;
                    case ImplementationModel.MenuItemType.Queue:
                    case ImplementationModel.MenuItemType.Review:
                        subPage = new ItemsPage(homeMenuItem) ;
                        break;
                    case ImplementationModel.MenuItemType.Manual:
                        subPage = new ManualItemsPage(homeMenuItem);
                        break;
                    case ImplementationModel.MenuItemType.ManualQueueAddButton:
                        break;
                    case ImplementationModel.MenuItemType.Separator:
                        break;
                    
                }

                MenuPages.Add(homeMenuItem, new NavigationPage(subPage));
            }

            var newPage = MenuPages[homeMenuItem];

            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;

                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(100);

                IsPresented = false;
            }
        }
    }
}
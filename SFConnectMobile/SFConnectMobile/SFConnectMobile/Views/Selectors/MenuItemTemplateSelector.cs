using ImplementationModel;
using SFConnectMobile.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SFConnectMobile.Views.Selectors
{
    public class MenuItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SearchMenuItemTemplate { get; set; }
        public DataTemplate QueueMenuItemTemplate { get; set; }
        public DataTemplate ReviewMenuItemTemplate { get; set; }
        public DataTemplate ManualMenuItemTemplate { get; set; }
        public DataTemplate SeparatorItemTemplate { get; set; }
        public DataTemplate AddManualItemButtonTemplate { get; set; }
        

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var menuItem = item as HomeMenuItem;

            switch (menuItem.MenuItemType)
            {
                case MenuItemType.Search:
                    return SearchMenuItemTemplate;
                case MenuItemType.Queue:
                    return QueueMenuItemTemplate;
                case MenuItemType.Review:
                    return ReviewMenuItemTemplate;
                case MenuItemType.Manual:
                    return ManualMenuItemTemplate;
                case MenuItemType.Separator:
                    return SeparatorItemTemplate;
                case MenuItemType.ManualQueueAddButton:
                    return AddManualItemButtonTemplate;
                case MenuItemType.NotSet:
                    return QueueMenuItemTemplate;
                default:
                    return QueueMenuItemTemplate;
            }
        }
    }
}

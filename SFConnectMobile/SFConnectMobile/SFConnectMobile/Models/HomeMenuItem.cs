using ImplementationModel;
using SFConnectMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFConnectMobile.Models
{
    public class HomeMenuItem
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public MenuItemType MenuItemType { get; set; } = MenuItemType.NotSet;
        public IList<string> CasesList { get; set; }

        public HomeMenuItem()
        {
            CasesList = new List<string>();
        }
    }
}

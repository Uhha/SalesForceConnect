using System;
using System.Collections.Generic;
using System.Text;

namespace SFConnectMobile.Interfaces
{
    public interface ISearchPage
    {
        void OnSearchBarTextChanged(string text);
        event EventHandler<string> SearchBarTextChanged;
    }
}

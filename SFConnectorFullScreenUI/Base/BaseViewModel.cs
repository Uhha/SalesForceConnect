﻿using PropertyChanged;
using System.ComponentModel;

namespace SFConnectorFullScreenUI
{
    [AddINotifyPropertyChangedInterface]
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
    }

}

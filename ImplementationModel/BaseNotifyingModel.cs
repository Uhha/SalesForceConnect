using System.ComponentModel;
using PropertyChanged;

namespace ImplementationModel
{
    [AddINotifyPropertyChangedInterface]
    public class BaseNotifyingModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

    }
}

using SFLink;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SFConnectorFullScreenUI
{
    public class FieldAreaListBarVM : BaseViewModel
    {
        private QueueComboBoxVM _queueComboBoxVM;
        public QueueComboBoxVM QueueComboBoxVM {
            get
            {
                return _queueComboBoxVM;
            }
            set
            {
                _queueComboBoxVM = value;
                _queueComboBoxVM.PropertyChanged += _queueComboBoxVM_PropertyChanged;
            }
        }

        public FieldAreaListVM FieldAreaListVM { get; set; }

        private async void _queueComboBoxVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (QueueComboBoxVM == null || FieldAreaListVM == null) return;
            if (e.PropertyName == nameof(QueueComboBoxVM.SelectedQueueItem))
            {
                var param = QueueComboBoxVM.SelectedQueueItem;
                QueueComboBoxVM.BusySpinnerVM.Spin = true;
                await FieldAreaListVM.BuildCases.ExecuteAsync(param);
                QueueComboBoxVM.BusySpinnerVM.Spin = false;
            }
        }

        public FieldAreaListBarVM()
        {
            
        }

       
    }
}

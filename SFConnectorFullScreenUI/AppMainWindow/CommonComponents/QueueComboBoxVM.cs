using ImplementationModel;
using SFLink;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFConnectorFullScreenUI
{
    public class QueueComboBoxVM : BaseViewModel
    {

        public string[] QueueItems { get; set; }
        public string SelectedQueueItem { get; set; }

        public IAsyncCommand BuildQueue { get; set; }
        public BusySpinnerVM BusySpinnerVM { get; set; }

        private bool _isBusyBuildQueues;

        public QueueComboBoxVM()
        {
            BusySpinnerVM = new BusySpinnerVM();
            BuildQueue = new AsyncCommand(BuildQueueAsync, CanBuildQueues, new RelayCommandErrorHandler());
            BuildQueue.ExecuteAsync().FireAndForgetSafeAsync();
        }

        private async Task<bool> BuildQueueAsync()
        {
            bool success;
            QueueItems = default(string[]);
            try
            {
                _isBusyBuildQueues = true;
                BusySpinnerVM.StartSpinning();
                QueueItems = await Task.Run(() => SFConnector.Instance.GetCasesQueues());
                success = true;
            }
            catch (Exception ex)
            {
                LoggerController.Log(ex);
                success = false;
            }
            finally
            {
                _isBusyBuildQueues = false;
            }
            BusySpinnerVM.StopSpinning(success);
            return success;
        }

        private bool CanBuildQueues()
        {
            if (QueueItems != null && QueueItems.Length > 0) return false;
            return !_isBusyBuildQueues;
        }
    }
}

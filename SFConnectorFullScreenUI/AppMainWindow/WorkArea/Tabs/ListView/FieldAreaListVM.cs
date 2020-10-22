using ImplementationModel;
using ImplementationModel.SalesForce;
using SFLink;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SFConnectorFullScreenUI
{
    public class FieldAreaListVM : BaseViewModel
    {
        private CaseType _caseType;
        private bool _isBusyBuildCases;

        public ObservableCollection<ICase> Cases { get; set; }
        public ICase SelectedCase { get; set; }
        public IAsyncCommand<string> BuildCases { get; private set; }



        public FieldAreaListVM(CaseType caseType)
        {
            _caseType = caseType;
            Cases = new ObservableCollection<ICase>();
            SelectedCase = default(ICase);
            BuildCases = new AsyncCommand<string>(BuildCasesAsync, CanBuildCases, new RelayCommandErrorHandler());
        }

        private async Task BuildCasesAsync(string selection)
        {
            IList<ICase> casesData;
            try
            {
                _isBusyBuildCases = true;
                if (_caseType == CaseType.SF)
                {
                    casesData = await Task.Run(() => SFConnector.Instance.GetCasesAsync(selection));
                }
                else
                {
                    throw new ArgumentException($"CaseType is not set to a proper type: {_caseType.ToString()}");
                }
            }
            finally
            {
                _isBusyBuildCases = false;
            }
            LoggerController.Log(casesData.Count + $" {_caseType.ToString()} cases Retrived.");
            Cases = new ObservableCollection<ICase>(casesData);
            SelectedCase = (Cases.Count > 0) ? Cases[0] : default(ICase);
        }

        private bool CanBuildCases(string selection)
        {
            return !_isBusyBuildCases;
        }

        
    }
}

using ImplementationModel;
using Newtonsoft.Json;
using SFConnectorUI.CaseView;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SFConnectorUI.StateFiles
{
    public static class CasesStatesOperations
    {
        private static readonly string _stateJson = "CasesStates.json";
        public static string ActiveListQueue = "Default";
        private static string _baseDir = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string _stateDir = "StateFiles";
        private static readonly string _fullFilePath = Path.Combine(_baseDir, _stateDir, _stateJson);

        private static CasesState _casesState;
        private static CasesList _activeList;

        static CasesStatesOperations()
        {

            if (!Directory.Exists(Path.Combine(_baseDir, _stateDir)))
                Directory.CreateDirectory(Path.Combine(_baseDir, _stateDir));
            if (File.Exists(_fullFilePath))
            {
                var jsonFile = File.ReadAllText(_fullFilePath);
                _casesState = JsonConvert.DeserializeObject<CasesState>(jsonFile);

                RescalePositions();
            }
            else
            {
                _casesState = new CasesState()
                {
                    CasesLists = new List<CasesList>()
                    {
                        new CasesList()
                        {
                            CasesQueue = "Default",
                            Cases = new Dictionary<string, CaseInfo>()
                        }
                    }

                };
            }
            _activeList = _casesState.CasesLists
                .Where(o => o.CasesQueue == ActiveListQueue)
                .FirstOrDefault();
        }

        private static void RescalePositions()
        {

            if (_casesState.ScreenResolutionHeight != (int)SystemParameters.FullPrimaryScreenHeight ||
                _casesState.ScreenResolutionWidth != (int)SystemParameters.FullPrimaryScreenWidth)
            {
                var cases = _casesState.CasesLists.Where(o => o.CasesQueue == ActiveListQueue).FirstOrDefault().Cases;
                foreach (var item in cases.Keys)
                {
                    cases[item].Left = (int)((((cases[item].Left * 100.0) / _casesState.ScreenResolutionWidth) / 100.0) * SystemParameters.FullPrimaryScreenWidth);
                    cases[item].Top = (int)((((cases[item].Top * 100.0) / _casesState.ScreenResolutionHeight) / 100.0) * SystemParameters.FullPrimaryScreenHeight);
                }
            }

            _casesState.ScreenResolutionHeight = (int)SystemParameters.FullPrimaryScreenHeight;
            _casesState.ScreenResolutionWidth = (int)SystemParameters.FullPrimaryScreenWidth;
        }

        public static void SaveState()
        {
            string currentState = JsonConvert.SerializeObject(_casesState);
            if (File.Exists(_fullFilePath))
            {
                File.Delete(_fullFilePath);
            }
            File.AppendAllText(_fullFilePath, currentState);
        }

        public static CaseInfo GetCaseInfo(string caseNumber)
        {
            if (!_activeList.Cases.ContainsKey(caseNumber))
            {
                SetDefaultCasePosition(caseNumber);
            }
            return _activeList.Cases[caseNumber];
        }

        private static void SetDefaultCasePosition(string caseNumber)
        {
            _activeList.Cases.Add(caseNumber, new CaseInfo()
            {
                Left = (int)SystemParameters.FullPrimaryScreenWidth / 2,
                Top = (int)SystemParameters.FullPrimaryScreenHeight / 2
            }
                );
        }

        public static void UpdateCasePosition(string caseNumber, int top, int left)
        {
            if (!_activeList.Cases.ContainsKey(caseNumber))
            {
                SetDefaultCasePosition(caseNumber);
            }
            _activeList.Cases[caseNumber].Left = left;
            _activeList.Cases[caseNumber].Top = top;
        }

        public static void TriggerMarked(string caseNumber, bool value)
        {
            if (!_activeList.Cases.ContainsKey(caseNumber))
            {
                SetDefaultCasePosition(caseNumber);
            }
            _activeList.Cases[caseNumber].Marked = value;
        }

    }
}

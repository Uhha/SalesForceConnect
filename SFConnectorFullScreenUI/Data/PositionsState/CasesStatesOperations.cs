using ImplementationModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SFConnectorFullScreenUI
{
    public static class CasesStatesOperations
    {
        private static readonly string _stateJson = "CasesStates.json";
        private static readonly string _baseDir = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string _stateDir = "Data/PositionsState";
        private static readonly string _fullFilePath = Path.Combine(_baseDir, _stateDir, _stateJson);

        private static CasesState _casesState;

        internal static void RenameTab(string oldTabIdentifier, string newTabIdentifier)
        {
            var tab = _casesState.CasesTabs.FirstOrDefault(o => o.TabIdentifier == oldTabIdentifier);
            if (tab != null)
            {
                tab.TabIdentifier = newTabIdentifier;
            }
        }

        static CasesStatesOperations()
        {

            if (!Directory.Exists(Path.Combine(_baseDir, _stateDir)))
                Directory.CreateDirectory(Path.Combine(_baseDir, _stateDir));
            if (File.Exists(_fullFilePath))
            {
                var jsonFile = File.ReadAllText(_fullFilePath);
                _casesState = JsonConvert.DeserializeObject<CasesState>(jsonFile);
            }
            if (_casesState == null)
                _casesState = new CasesState();
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

        public static IList<CasesTab> GetTabs()
        {
            if (_casesState.CasesTabs == null)
            {
                _casesState.CasesTabs = new List<CasesTab>() { new CasesTab() { TabIdentifier = "New Tab", CasesLists = new List<CasesList>() } };
            }
            return _casesState.CasesTabs;
        }

        public static CaseInfo GetCaseInfo(string caseNumber, string caseQueueName, string tabIdentifier)
        {
            var list = GetCasesList(caseQueueName, tabIdentifier);

            if (list.Cases.ContainsKey(caseNumber))
            {
                return list.Cases[caseNumber];
            }
            else
            {
                var caseInfo = new CaseInfo { Index = new Index(0, 0) };
                list.Cases.Add(caseNumber, caseInfo);
                return caseInfo;
            }
        }

        public static void UpdateCasesPositions(List<CaseControlVM> cases, string caseQueueName, string tabIdentifier)
        {
            var list = GetCasesList(caseQueueName, tabIdentifier);
            foreach (CaseControlVM item in cases)
            {
                if (list.Cases.ContainsKey(item.CaseModel.CaseNumber))
                {
                    list.Cases[item.CaseModel.CaseNumber].Index = item.Index;
                }
                else
                {
                    list.Cases.Add(item.CaseModel.CaseNumber, new CaseInfo() { Index = item.Index });
                }
            }
        }

        internal static void RemoveTab(string tabIdentifier)
        {
            var tab = _casesState.CasesTabs.FirstOrDefault(o => o.TabIdentifier == tabIdentifier);
            if (tab != null)
            {
                _casesState.CasesTabs.Remove(tab);
            }
        }

        internal static void RemoveLabel(Guid guid, string caseQueueName, string tabIdentifier)
        {
            var casesList = GetCasesList(caseQueueName, tabIdentifier);
            casesList.Labels.Remove(guid.ToString());
        }

        internal static void RemoveCase(CaseControlVM casecontrol, string caseQueueName, string tabIdentifier)
        {
            var list = GetCasesList(caseQueueName, tabIdentifier);
            list.Cases.Remove(casecontrol.CaseModel.CaseNumber);
        }

        public static CasesList GetCasesList(string caseQueueName, string tabIdentifier)
        {
            //TODO:Cenvert to async
            if (_casesState.CasesTabs == null) _casesState.CasesTabs = new List<CasesTab>();
            
            var tab = _casesState.CasesTabs.FirstOrDefault(ct => ct.TabIdentifier == tabIdentifier);
            if (tab == null)
            {
                tab = new CasesTab
                {
                    TabIdentifier = tabIdentifier,
                    CasesLists = new List<CasesList>
                    {
                        new CasesList
                        {
                            CasesQueueName = caseQueueName,
                            Cases = new Dictionary<string, CaseInfo>(),
                            Labels = new Dictionary<string, LabelInfo>()
                        }
                    }
                };
                _casesState.CasesTabs.Add(tab);
            }

            var list = tab.CasesLists.FirstOrDefault(o => o.CasesQueueName == caseQueueName);

            if (list == null)
            {
                list = new CasesList
                {
                    CasesQueueName = caseQueueName,
                    Cases = new Dictionary<string, CaseInfo>(),
                    Labels = new Dictionary<string, LabelInfo>()
                };
                tab.CasesLists.Add(list);
            }
            if (list.Labels == null) list.Labels = new Dictionary<string, LabelInfo>();

            return list;
        }

        internal static void UpdateLabelsPositions(List<CellLabelVM> labels, string caseQueueName, string tabIdentifier)
        {
            var list = GetCasesList(caseQueueName, tabIdentifier);

            foreach (CellLabelVM item in labels)
            {
                if (list.Labels.ContainsKey(item.Guid.ToString()))
                {
                    var label = list.Labels[item.Guid.ToString()];
                    label.Note = item.Note;
                    label.Width = item.Width;
                    label.Height = item.Height;
                    label.Left = item.Left;
                    label.Top = item.Top;
                }
                else
                {
                    list.Labels.Add(item.Guid.ToString(), new LabelInfo() { Left = item.Left, Top = item.Top,
                        Width = item.Width, Height = item.Height,
                        Note = item.Note, Guid = item.Guid.ToString() });
                }
            }
        }
     
    }
}

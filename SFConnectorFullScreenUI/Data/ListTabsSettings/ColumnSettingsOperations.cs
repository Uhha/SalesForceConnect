using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SFConnectorFullScreenUI
{
    public static class ColumnSettingsOperations
    {
        private static readonly string _settingsJson = "ColumnsSettings.json";
        private static readonly string _baseDir = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string _stateDir = "Data/ListTabsSettings";
        private static readonly string _fullFilePath = Path.Combine(_baseDir, _stateDir, _settingsJson);

        private static ColumnsSettingsState _columnsSettings;

        static ColumnSettingsOperations()
        {

            if (!Directory.Exists(Path.Combine(_baseDir, _stateDir)))
                Directory.CreateDirectory(Path.Combine(_baseDir, _stateDir));
            if (File.Exists(_fullFilePath))
            {
                var jsonFile = File.ReadAllText(_fullFilePath);
                _columnsSettings = JsonConvert.DeserializeObject<ColumnsSettingsState>(jsonFile);
            }
            else
            {
                _columnsSettings = new ColumnsSettingsState();
            }
        }

        internal static List<ColumnPreset> LoadColumnState(string tabIdentifier)
        {
            if (_columnsSettings.ColumnsPresets == null) _columnsSettings.ColumnsPresets = new Dictionary<string, List<ColumnPreset>>();
            if (!_columnsSettings.ColumnsPresets.ContainsKey(tabIdentifier)) return null;
            return _columnsSettings.ColumnsPresets[tabIdentifier];
        }

        internal static void UpdateColumnState(string tabIdentifier, GridViewColumnCollection columns)
        {
            List<ColumnPreset> columnPreset = new List<ColumnPreset>();
            int i = 0;
            foreach (var col in columns)
            {
                columnPreset.Add(new ColumnPreset
                {
                    OrderNumber = i++,
                    ColumnName = col.Header.ToString(),
                    Width = (int)col.ActualWidth
                });
            }
            if (_columnsSettings.ColumnsPresets.ContainsKey(tabIdentifier))
            {
                _columnsSettings.ColumnsPresets[tabIdentifier] = columnPreset;
            }
            else
            {
                _columnsSettings.ColumnsPresets.Add(tabIdentifier, columnPreset);
            }
        }

        internal static void SaveState()
        {
            string currentState = JsonConvert.SerializeObject(_columnsSettings);
            if (File.Exists(_fullFilePath))
            {
                File.Delete(_fullFilePath);
            }
            File.AppendAllText(_fullFilePath, currentState);
        }
    }
}

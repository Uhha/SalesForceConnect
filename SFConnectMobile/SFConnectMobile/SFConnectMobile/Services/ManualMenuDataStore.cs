using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImplementationModel;
using ImplementationModel.SalesForce;
using Newtonsoft.Json;
using SFConnectMobile.Models;

namespace SFConnectMobile.Services
{
    public class ManualMenuDataStore
    {
        private static readonly ManualMenuDataStore _instance = new ManualMenuDataStore();

        static ManualMenuDataStore()
        {
        }
        private ManualMenuDataStore()
        {
            _items = GetManualItems();
        }

        public static ManualMenuDataStore Instance
        {
            get { return _instance; }
        }

        private IList<HomeMenuItem> _items;

        public IList<HomeMenuItem> GetManualItems(bool forceRefresh = false)
        {
            if (!forceRefresh && _items != null && _items.Count() > 0) return _items;
            else
            {
                InitList();
            }
            return _items;
        }


        private void InitList()
        {
            ManualQueuesJson msj = ReadData();
            List<HomeMenuItem> menuItems = new List<HomeMenuItem>();
            if (msj == null)
            {
                _items = menuItems;
                return;
            }
            foreach (var item in msj.ManualQueues)
            {
                menuItems.Add(new HomeMenuItem() { Title = item.QueueName, CasesList = item.CasesNumbers.ToList(), MenuItemType = MenuItemType.Manual });
            }
            _items = menuItems;
        }

        internal void SaveState()
        {
            SaveData();
        }

        public void AddManualItem(HomeMenuItem homeMenuItem)
        {
            if (_items == null) InitList();
            _items.Add(homeMenuItem);
            SaveData();
        }

        public void RemoveManualItem(HomeMenuItem homeMenuItem)
        {
            if (_items == null) InitList();
            //Should search by title???
            _items.Remove(homeMenuItem);
            SaveData();
        }

        private ManualQueuesJson ReadData()
        {
            try
            {
                var savedManualQueues = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ManualQueues.json");

                if (savedManualQueues == null || !File.Exists(savedManualQueues))
                {
                    return null;
                }
                var file = File.ReadAllText(savedManualQueues);

                return JsonConvert.DeserializeObject(file, typeof(ManualQueuesJson)) as ManualQueuesJson;

            }
            catch (Exception)
            {
                return null;
            }
        }

        private void SaveData()
        {
            try
            {
                if (_items == null) return;
                ManualQueuesJson mqj = new ManualQueuesJson() { ManualQueues = new ManualQueue[_items.Count()] };
                int i = 0;
                foreach (var item in _items)
                {
                    mqj.ManualQueues[i] = new ManualQueue() { QueueName = item.Title , CasesNumbers = item.CasesList.ToArray()};
                    i++;
                }

                var filePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ManualQueues.json");
                var serialized = JsonConvert.SerializeObject(mqj);
                File.WriteAllText(filePath, serialized);
            }
            catch (Exception)
            {
                return;
            }

        }

        private class ManualQueuesJson
        {
            public ManualQueue[] ManualQueues { get; set; }
        }

        private class ManualQueue
        {
            public string QueueName { get; set; }
            public string[] CasesNumbers { get; set; }
        }
    }

    
    
    
}
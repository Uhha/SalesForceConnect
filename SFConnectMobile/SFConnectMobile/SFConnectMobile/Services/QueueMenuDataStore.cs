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
    public class QueueMenuDataStore 
    {
        private static readonly QueueMenuDataStore _instance = new QueueMenuDataStore();
        public static QueueMenuDataStore Instance
        {
            get { return _instance; }
        } 
        static QueueMenuDataStore(){}
        private QueueMenuDataStore(){}

        private List<HomeMenuItem> _items;

        public async Task<IEnumerable<HomeMenuItem>> GetQueueItemsAsync(bool forceRefresh = false)
        {
            if (!forceRefresh && _items != null) return _items; 
            var queues = await SFLink.SFConnector.Instance.GetCasesQueues();
            int i = 0;
            _items = new List<HomeMenuItem>();
            foreach (var queue in queues)
            {
                var menuItemType = (queue.StartsWith("Review ")) ? MenuItemType.Review : MenuItemType.Queue;
                _items.Add(new HomeMenuItem() { Id = i++, Title = queue, MenuItemType = menuItemType });
            }
            return _items;
        }
    }
}
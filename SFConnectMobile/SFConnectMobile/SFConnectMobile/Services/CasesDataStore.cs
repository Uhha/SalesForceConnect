using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImplementationModel;
using ImplementationModel.SalesForce;
using SFConnectMobile.Models;

namespace SFConnectMobile.Services
{
    public class CasesDataStore
    {
        private IList<ICase> _items;

        public CasesDataStore()
        {
        }


        public async Task<IList<ICase>> GetAllCases(bool IncludeClosedCases)
        {
            //TODO: refactor later with cacheing
            if (IncludeClosedCases)
            {
                return await SFLink.SFConnector.Instance.GetCasesAsync("All");
            }
            else return await SFLink.SFConnector.Instance.GetCasesAsync("AllNotClosed");
        }

        public async Task<IList<ICase>> GetCases(string queueName, MenuItemType menuItemType, IList<string> casesList, bool forceRefresh = false)
        {
            if (forceRefresh || _items == null) await PopulateItems(queueName, menuItemType, casesList);
            return _items;
        }

        private async Task<bool> PopulateItems(string queueName, MenuItemType menuItemType, IList<string> casesList)
        {
            if (menuItemType == MenuItemType.Manual)
            {
                if (casesList != null && casesList.Count() > 0)
                {
                    _items = await SFLink.SFConnector.Instance.GetCasesFromArrayAsync(casesList.ToArray());
                }
                else _items = new List<ICase>();
            }
            else
                _items = await SFLink.SFConnector.Instance.GetCasesAsync(queueName);

            return await Task.FromResult(true);
        }


    }
}
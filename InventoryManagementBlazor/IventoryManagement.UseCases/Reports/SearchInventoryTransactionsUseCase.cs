using IventoryManagement.CoreBusiness;
using IventoryManagement.UseCases.PluginInterfaces;
using IventoryManagement.UseCases.Reports.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IventoryManagement.UseCases.Reports
{
    public class SearchInventoryTransactionsUseCase : ISearchInventoryTransactionsUseCase
    {
        private readonly IInventoryTransactionRepositoy _inventoryTransactionRepositoy;

        public SearchInventoryTransactionsUseCase(IInventoryTransactionRepositoy inventoryTransactionRepositoy)
        {
            _inventoryTransactionRepositoy = inventoryTransactionRepositoy;
        }

        public async Task<IEnumerable<InventoryTransaction>> ExecuteAsync(
            string inventoryName,
            DateTime? dateFrom,
           DateTime? dateTo,
            InventoryTransactionType? transactionType
            )
        {
            if (dateTo.HasValue)
            {
                dateTo = dateTo.Value.AddDays(1);
            }

            return await _inventoryTransactionRepositoy.GetInventoryTransactionsAsync(
                inventoryName,
                dateFrom,
                dateTo,
                transactionType
                );
        }
    }
}

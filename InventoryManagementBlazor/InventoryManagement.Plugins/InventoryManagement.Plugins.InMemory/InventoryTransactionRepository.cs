using IventoryManagement.CoreBusiness;
using IventoryManagement.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Plugins.InMemory
{
    public class InventoryTransactionRepository : IInventoryTransactionRepositoy
    {
        private readonly IInventoryRepositoy _inventoryRepositoy;

        public InventoryTransactionRepository(IInventoryRepositoy inventoryRepositoy)
        {
            _inventoryRepositoy = inventoryRepositoy;
        }

        public List<InventoryTransaction> _inventoryTransactions = new List<InventoryTransaction>();

        public async Task<IEnumerable<InventoryTransaction>> GetInventoryTransactionsAsync(string inventoryName,
            DateTime? dateFrom, DateTime? dateTo, InventoryTransactionType? transactionType)
        {
            var inventories = (await _inventoryRepositoy.GetInventoriesByNameAsync(string.Empty)).ToList();

            var query = from it in _inventoryTransactions
                        join inv in inventories on it.InventoryId equals inv.InventoryId
                        where
                             (string.IsNullOrWhiteSpace(inventoryName) || inv.InventoryName.ToLower().IndexOf(inventoryName.ToLower()) >= 0)
                             &&
                             (!dateFrom.HasValue || it.TransactionDate >= dateFrom.Value.Date) &&
                             (!dateTo.HasValue || it.TransactionDate <= dateTo.Value.Date) &&
                             (!transactionType.HasValue || it.ActivityType == transactionType)
                        select new InventoryTransaction
                        {
                            Inventory = inv,
                            InventoryTransactionId = it.InventoryTransactionId,
                            PONumber = it.PONumber,
                            ProductionNumber = it.ProductionNumber,
                            InventoryId = it.InventoryId,
                            QuantityBefore = it.QuantityBefore,
                            ActivityType = it.ActivityType,
                            QuantityAfter= it.QuantityAfter,
                            TransactionDate= it.TransactionDate,
                            DoneBy= it.DoneBy,
                            UnitPrice= it.UnitPrice,
                        };

            return query;
        }

        public Task ProduceAsync(string productionNumber, Inventory inventory, int quantityToCosume, string doneBy, decimal price)
        {
            _inventoryTransactions.Add(new InventoryTransaction
            {
                ProductionNumber = productionNumber,
                InventoryId = inventory.InventoryId,
                QuantityBefore = inventory.Quantity,
                ActivityType = InventoryTransactionType.ProduceProduct,
                QuantityAfter = inventory.Quantity - quantityToCosume,
                TransactionDate = DateTime.UtcNow,
                DoneBy = doneBy,
                UnitPrice = price,
            });

            return Task.CompletedTask;
        }

        public Task PurchaseAsync(string poNumber, Inventory inventory, int quantity, string doneBy, decimal price)
        {
            _inventoryTransactions.Add(new InventoryTransaction
            {
                PONumber = poNumber,
                InventoryId = inventory.InventoryId,
                QuantityBefore = inventory.Quantity,
                ActivityType = InventoryTransactionType.PurchaseInventory,
                QuantityAfter = inventory.Quantity + quantity,
                TransactionDate = DateTime.UtcNow,
                DoneBy = doneBy,
                UnitPrice = price,
            });

            return Task.CompletedTask;
        }
    }
}

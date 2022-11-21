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
        public List<InventoryTransaction> _inventoryTransactions = new List<InventoryTransaction>();

        public void ProduceAsync(string productionNumber, Inventory inventory, int quantityToCosume, string doneBy, decimal price)
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
        }

        public void PurchaseAsync(string poNumber, Inventory inventory, int quantity, string doneBy, decimal price)
        {
            _inventoryTransactions.Add(new InventoryTransaction
            {
                PONumber = poNumber,
                InventoryId= inventory.InventoryId,
                QuantityBefore = inventory.Quantity,
                ActivityType = InventoryTransactionType.PurchaseInventory,
                QuantityAfter = inventory.Quantity + quantity,
                TransactionDate = DateTime.UtcNow,
                DoneBy = doneBy,
                UnitPrice = price,
            });
        }
    }
}

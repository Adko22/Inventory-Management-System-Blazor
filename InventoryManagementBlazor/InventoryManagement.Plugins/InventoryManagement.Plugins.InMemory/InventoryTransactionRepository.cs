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

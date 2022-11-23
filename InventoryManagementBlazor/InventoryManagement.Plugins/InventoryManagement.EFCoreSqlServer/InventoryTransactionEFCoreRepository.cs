using InventoryManagement.EFCoreSqlServer;
using IventoryManagement.CoreBusiness;
using IventoryManagement.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Plugins.EFCoreSqlServer
{
    public class InventoryTransactionEFCoreRepository : IInventoryTransactionRepositoy
    {
        private readonly IDbContextFactory<IMSContex> _contextFactory;

        public InventoryTransactionEFCoreRepository(IDbContextFactory<IMSContex> contextFactory)
        {
            _contextFactory = contextFactory;
        }


        public async Task<IEnumerable<InventoryTransaction>> GetInventoryTransactionsAsync(string inventoryName,
            DateTime? dateFrom, DateTime? dateTo, InventoryTransactionType? transactionType)
        {
            using var _db = _contextFactory.CreateDbContext();

            var query = from it in _db.InventoryTransactions
                        join inv in _db.Inventories on it.InventoryId equals inv.InventoryId
                        where
                             (string.IsNullOrWhiteSpace(inventoryName) || inv.InventoryName.ToLower().IndexOf(inventoryName.ToLower()) >= 0)
                             &&
                             (!dateFrom.HasValue || it.TransactionDate >= dateFrom.Value.Date) &&
                             (!dateTo.HasValue || it.TransactionDate <= dateTo.Value.Date) &&
                             (!transactionType.HasValue || it.ActivityType == transactionType)
                        select it;

            return await query.Include(x => x.Inventory).ToListAsync();
        }

        public async Task ProduceAsync(string productionNumber, Inventory inventory, int quantityToCosume, string doneBy, decimal price)
        {
            using var _db = _contextFactory.CreateDbContext();

            _db.InventoryTransactions.Add(new InventoryTransaction
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

            await _db.SaveChangesAsync();
        }

        public async Task PurchaseAsync(string poNumber, Inventory inventory, int quantity, string doneBy, decimal price)
        {
            using var _db = _contextFactory.CreateDbContext();

            _db.InventoryTransactions.Add(new InventoryTransaction
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

            await _db.SaveChangesAsync();
        }
    }
}

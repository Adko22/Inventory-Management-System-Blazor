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
    public class ProductTransactionEFCoreRepository : IProductTransactionRepository
    {
        private readonly IProductRepository _productRepository;
        private readonly IInventoryTransactionRepositoy _inventoryTransactionRepository;
        private readonly IInventoryRepositoy _inventoryRepository;
        private readonly IDbContextFactory<IMSContex> _contextFactory;

        public ProductTransactionEFCoreRepository(IProductRepository productRepository,
            IInventoryTransactionRepositoy inventoryTransactionRepository,
            IInventoryRepositoy inventoryRepository,IDbContextFactory<IMSContex> contextFactory)
        {
            _productRepository = productRepository;
            _inventoryTransactionRepository = inventoryTransactionRepository;
            _inventoryRepository = inventoryRepository;
            _contextFactory = contextFactory;
        }

        public async Task ProduceAsync(string productionNumber, Product product, int quantity, string doneBy)
        {
            var prod = await _productRepository.GetProductByIdAsync(product.ProductId);

            if (prod != null)
            {
                foreach (var pi in prod.ProductInventories)
                {
                    if (pi.Inventory != null)
                    {
                        await _inventoryTransactionRepository.ProduceAsync(productionNumber, pi.Inventory,
                            pi.InventoryQuantity * quantity, doneBy, -1);

                        var inv = await _inventoryRepository.GetInvetoryByIdAsync(pi.InventoryId);
                        inv.Quantity -= pi.InventoryQuantity * quantity;

                        await _inventoryRepository.UpdateInventoryAsync(inv);
                    }
                }
            }
            using var _db = _contextFactory.CreateDbContext();

            _db.ProductTransactions.Add(new ProductTransaction
            {
                ProductionNumber = productionNumber,
                ProductId = product.ProductId,
                QuantityBefore = product.Quantity,
                ActivityType = ProductTransactionType.ProduceProduct,
                QuantityAfter = product.Quantity + quantity,
                TransactionDate = DateTime.UtcNow,
                DoneBy = doneBy,
            });

            await _db.SaveChangesAsync();
        }

        public async Task SellProductAsync(string salesOrderNumber, Product product, int quantity, decimal unitPrice, string doneBy)
        {
            using var _db = _contextFactory.CreateDbContext();

            _db.ProductTransactions.Add(new ProductTransaction
            {
                ActivityType = ProductTransactionType.SellProduct,
                SONumber = salesOrderNumber,
                ProductId = product.ProductId,
                QuantityBefore = product.Quantity,
                QuantityAfter = product.Quantity - quantity,
                TransactionDate = DateTime.UtcNow,
                DoneBy = doneBy,
                UnitPrice = unitPrice
            });

            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductTransaction>> GetProductTransactionsAsync(string productName,
            DateTime? dateFrom, DateTime? dateTo, ProductTransactionType? transactionType)
        {
            using var _db = _contextFactory.CreateDbContext();

            var query = from pt in _db.ProductTransactions
                        join prod in _db.Products on pt.ProductId equals prod.ProductId
                        where
                             (string.IsNullOrWhiteSpace(productName) || prod.ProductName.ToLower().IndexOf(productName.ToLower()) >= 0)
                             &&
                             (!dateFrom.HasValue || pt.TransactionDate >= dateFrom.Value.Date) &&
                             (!dateTo.HasValue || pt.TransactionDate <= dateTo.Value.Date) &&
                             (!transactionType.HasValue || pt.ActivityType == transactionType)
                        select pt;

            return await query.Include(x => x.Product).ToListAsync();
        }
    }
}

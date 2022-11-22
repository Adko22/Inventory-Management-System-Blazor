using IventoryManagement.CoreBusiness;
using IventoryManagement.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Plugins.InMemory
{
    public class ProductTransactionRepository : IProductTransactionRepository
    {
        private List<ProductTransaction> _productTransactions = new List<ProductTransaction>();

        private readonly IProductRepository _productRepository;
        private readonly IInventoryTransactionRepositoy _inventoryTransactionRepository;
        private readonly IInventoryRepositoy _inventoryRepository;

        public ProductTransactionRepository(IProductRepository productRepository,
            IInventoryTransactionRepositoy inventoryTransactionRepository,
            IInventoryRepositoy inventoryRepository)
        {
            _productRepository = productRepository;
            _inventoryTransactionRepository = inventoryTransactionRepository;
            _inventoryRepository = inventoryRepository;
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
                        _inventoryTransactionRepository.ProduceAsync(productionNumber, pi.Inventory, pi.InventoryQuantity * quantity, doneBy, -1);

                        var inv = await _inventoryRepository.GetInvetoryByIdAsync(pi.InventoryId);
                        inv.Quantity -= pi.InventoryQuantity * quantity;

                        await _inventoryRepository.UpdateInventoryAsync(inv);
                    }
                }
            }

            _productTransactions.Add(new ProductTransaction
            {
                ProductionNumber = productionNumber,
                ProductId = product.ProductId,
                QuantityBefore = product.Quantity,
                ActivityType = ProductTransactionType.ProduceProduct,
                QuantityAfter = product.Quantity + quantity,
                TransactionDate = DateTime.UtcNow,
                DoneBy = doneBy,
            });
        }

        public Task SellProductAsync(string salesOrderNumber, Product product, int quantity, decimal unitPrice, string doneBy)
        {
            _productTransactions.Add(new ProductTransaction
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

            return Task.CompletedTask;
        }

        public async Task<IEnumerable<ProductTransaction>> GetProductTransactionsAsync(string productName,
            DateTime? dateFrom, DateTime? dateTo, ProductTransactionType? transactionType)
        {
            var products = (await _productRepository.GetProductsByName(string.Empty)).ToList();

            var query = from pt in _productTransactions
                        join prod in products on pt.ProductId equals prod.ProductId
                        where
                             (string.IsNullOrWhiteSpace(productName) || prod.ProductName.ToLower().IndexOf(productName.ToLower()) >= 0)
                             &&
                             (!dateFrom.HasValue || pt.TransactionDate >= dateFrom.Value.Date) &&
                             (!dateTo.HasValue || pt.TransactionDate <= dateTo.Value.Date) &&
                             (!transactionType.HasValue || pt.ActivityType == transactionType)
                        select new ProductTransaction
                        {
                            Product = prod,
                            ProductTransactionId = pt.ProductTransactionId,
                            SONumber = pt.SONumber,
                            ProductionNumber = pt.ProductionNumber,
                            ProductId = pt.ProductId,
                            QuantityBefore = pt.QuantityBefore,
                            ActivityType = pt.ActivityType,
                            QuantityAfter = pt.QuantityAfter,
                            TransactionDate = pt.TransactionDate,
                            DoneBy = pt.DoneBy,
                            UnitPrice = pt.UnitPrice,
                        };

            return query;
        }
    }
}

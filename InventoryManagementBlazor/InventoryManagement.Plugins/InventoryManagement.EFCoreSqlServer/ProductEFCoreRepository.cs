using InventoryManagement.EFCoreSqlServer;
using IventoryManagement.CoreBusiness;
using IventoryManagement.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Plugins.EFCoreSqlServer
{
    public class ProductEFCoreRepository : IProductRepository
    {
        private readonly IDbContextFactory<IMSContex> _contextFactory;

        public ProductEFCoreRepository(IDbContextFactory<IMSContex> contextFactory)
        {
           _contextFactory= contextFactory;
        }

        public async Task AddProductAsync(Product product)
        {
            using var _db = _contextFactory.CreateDbContext();

            _db.Products.Add(product);
            FlagInventoryUnChanged(product, _db);

            await _db.SaveChangesAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            using var _db = _contextFactory.CreateDbContext();

            return await _db.Products.Include(x => x.ProductInventories)
                .ThenInclude(x => x.Inventory)
                .FirstOrDefaultAsync(x=>x.ProductId == productId);
        }

        public async Task<IEnumerable<Product>> GetProductsByName(string name)
        {
            using var _db = _contextFactory.CreateDbContext();

            return await _db.Products.Where(x => x.ProductName.ToLower().IndexOf(name.ToLower()) >= 0).ToListAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            using var _db = _contextFactory.CreateDbContext();

            var prod = await _db.Products.Include(x => x.ProductInventories).FirstOrDefaultAsync(x => x.ProductId == product.ProductId);

            if (prod != null)
            {
                prod.ProductName = product.ProductName;
                prod.Price = product.Price;
                prod.Quantity = product.Quantity;
                prod.ProductInventories = product.ProductInventories;

                FlagInventoryUnChanged(product, _db);

                await _db.SaveChangesAsync();
            }
        }

        private void FlagInventoryUnChanged(Product product, IMSContex db)
        {
            if (product?.ProductInventories != null && product.ProductInventories.Count > 0)
            {
                foreach (var prodInv in product.ProductInventories)
                {
                    if (prodInv.Inventory != null)
                    {
                        db.Entry(prodInv.Inventory).State = EntityState.Unchanged;
                    }
                }
            }
        }
    }
}

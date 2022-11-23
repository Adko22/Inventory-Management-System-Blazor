using InventoryManagement.EFCoreSqlServer;
using IventoryManagement.CoreBusiness;
using IventoryManagement.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Plugins.EFCoreSqlServer
{
    public class InventoryEFCoreRepositoy : IInventoryRepositoy
    {
        private readonly IDbContextFactory<IMSContex> _contextFactory;

        public InventoryEFCoreRepositoy(IDbContextFactory<IMSContex> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task AddInventoryAsync(Inventory inventory)
        {
            using var _db = _contextFactory.CreateDbContext();

            _db.Inventories.Add(inventory);

            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Inventory>> GetInventoriesByNameAsync(string name)
        {
            using var _db = _contextFactory.CreateDbContext();

            return await _db.Inventories.Where(x => x.InventoryName.ToLower().IndexOf(name.ToLower()) >= 0).ToListAsync();
        }

        public async Task<Inventory> GetInvetoryByIdAsync(int inventoryId)
        {
            using var _db = _contextFactory.CreateDbContext();

            var inv = await _db.Inventories.FindAsync(inventoryId);

            if (inv != null)
            {
                return inv;
            }

            return new Inventory();
        }

        public async Task UpdateInventoryAsync(Inventory inventory)
        {
            using var _db = _contextFactory.CreateDbContext();

            var inv = await _db.Inventories.FindAsync(inventory.InventoryId);

            if (inv != null)
            {
                inv.InventoryName = inventory.InventoryName;
                inv.Price = inventory.Price;
                inv.Quantity = inventory.Quantity;

                await _db.SaveChangesAsync();
            }
        }
    }
}
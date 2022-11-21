using IventoryManagement.CoreBusiness;
using IventoryManagement.UseCases.Activities.Interfaces;
using IventoryManagement.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IventoryManagement.UseCases.Activities
{
    public class PurchaseInventoryUseCase : IPurchaseInventoryUseCase
    {
        private readonly IInventoryTransactionRepositoy _inventoryTransanctionRepository;
        private readonly IInventoryRepositoy _inventoryRepository;

        public PurchaseInventoryUseCase(IInventoryTransactionRepositoy inventoryTransanctionRepository, IInventoryRepositoy inventoryRepository)
        {
            _inventoryTransanctionRepository = inventoryTransanctionRepository;
            _inventoryRepository = inventoryRepository;
        }

        public async Task ExecuteAsync(string poNumber, Inventory inventory, int quantity, string doneBy)
        {
            _inventoryTransanctionRepository.PurchaseAsync(poNumber, inventory, quantity, doneBy, inventory.Price);

            inventory.Quantity += quantity;
            await _inventoryRepository.UpdateInventoryAsync(inventory);
        }

    }
}

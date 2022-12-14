using IventoryManagement.CoreBusiness;
using IventoryManagement.UseCases.Inventories.Interfaces;
using IventoryManagement.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IventoryManagement.UseCases.Inventories
{
    public class AddInventoryUseCase : IAddInventoryUseCase
    {
        private readonly IInventoryRepositoy _inventoryRepository;

        public AddInventoryUseCase(IInventoryRepositoy inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task ExecuteAsync(Inventory inventory)
        {
            await _inventoryRepository.AddInventoryAsync(inventory);
        }
    }
}

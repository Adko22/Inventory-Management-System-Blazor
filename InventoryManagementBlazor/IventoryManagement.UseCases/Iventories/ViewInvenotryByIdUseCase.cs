using IventoryManagement.CoreBusiness;
using IventoryManagement.UseCases.Iventories.Interfaces;
using IventoryManagement.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IventoryManagement.UseCases.Iventories
{
    public class ViewInvenotryByIdUseCase : IViewInvenotryByIdUseCase
    {
        private readonly IInventoryRepositoy _inventoryRepository;

        public ViewInvenotryByIdUseCase(IInventoryRepositoy inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<Inventory> ExecuteAsync(int inventoryId)
        {
            return await _inventoryRepository.GetInvetoryByIdAsync(inventoryId);
        }
    }
}

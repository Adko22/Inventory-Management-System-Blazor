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
    public class ViewInventoriesByNameUseCase : IViewInventoriesByNameUseCase
    {
        private readonly IInventoryRepositoy _invetoryRepository;

        public ViewInventoriesByNameUseCase(IInventoryRepositoy invetoryRepository)
        {
            _invetoryRepository = invetoryRepository;
        }

        public async Task<IEnumerable<Inventory>> ExecuteAsync(string name = "")
        {
            return await _invetoryRepository.GetInventoriesByNameAsync(name);
        }
    }
}

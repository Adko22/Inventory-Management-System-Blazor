using IventoryManagement.CoreBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IventoryManagement.UseCases.PluginInterfaces
{
    public interface IInventoryRepositoy
    {
        Task<IEnumerable<Inventory>> GetInventoriesByNameAsync(string name);
    }
}

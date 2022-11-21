using IventoryManagement.CoreBusiness;

namespace IventoryManagement.UseCases.Inventories.Interfaces
{
    public interface IAddInventoryUseCase
    {
        Task ExecuteAsync(Inventory inventory);
    }
}
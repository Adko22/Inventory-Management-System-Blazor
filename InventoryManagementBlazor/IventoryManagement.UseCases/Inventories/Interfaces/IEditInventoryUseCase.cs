using IventoryManagement.CoreBusiness;

namespace IventoryManagement.UseCases.Inventories.Interfaces
{
    public interface IEditInventoryUseCase
    {
        Task ExecuteAsync(Inventory inventory);
    }
}
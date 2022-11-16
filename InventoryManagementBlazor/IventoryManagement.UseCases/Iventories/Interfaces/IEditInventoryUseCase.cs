using IventoryManagement.CoreBusiness;

namespace IventoryManagement.UseCases.Iventories.Interfaces
{
    public interface IEditInventoryUseCase
    {
        Task ExecuteAsync(Inventory inventory);
    }
}
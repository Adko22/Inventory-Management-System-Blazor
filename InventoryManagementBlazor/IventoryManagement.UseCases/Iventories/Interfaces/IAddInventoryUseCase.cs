using IventoryManagement.CoreBusiness;

namespace IventoryManagement.UseCases.Iventories.Interfaces
{
    public interface IAddInventoryUseCase
    {
        Task ExecuteAsync(Inventory inventory);
    }
}
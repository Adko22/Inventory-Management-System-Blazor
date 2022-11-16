using IventoryManagement.CoreBusiness;

namespace IventoryManagement.UseCases.Iventories.Interfaces
{
    public interface IViewInvenotryByIdUseCase
    {
        Task<Inventory> ExecuteAsync(int inventoryId);
    }
}
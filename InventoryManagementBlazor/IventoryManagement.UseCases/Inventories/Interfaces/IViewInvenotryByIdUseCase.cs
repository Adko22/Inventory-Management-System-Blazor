using IventoryManagement.CoreBusiness;

namespace IventoryManagement.UseCases.Inventories.Interfaces
{
    public interface IViewInvenotryByIdUseCase
    {
        Task<Inventory> ExecuteAsync(int inventoryId);
    }
}
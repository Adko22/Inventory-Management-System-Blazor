using IventoryManagement.CoreBusiness;

namespace IventoryManagement.UseCases.Activities.Interfaces
{
    public interface IPurchaseInventoryUseCase
    {
        Task ExecutAsync(string poNumber, Inventory inventory, int quantity, string doneBy);
    }
}
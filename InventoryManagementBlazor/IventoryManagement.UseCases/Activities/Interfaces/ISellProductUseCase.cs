using IventoryManagement.CoreBusiness;

namespace IventoryManagement.UseCases.Activities.Interfaces
{
    public interface ISellProductUseCase
    {
        Task ExecuteAsync(string salesOrderNumber, Product product, int quantity, decimal unitPrice, string doneBy);
    }
}
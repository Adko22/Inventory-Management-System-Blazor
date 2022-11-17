using IventoryManagement.CoreBusiness;

namespace IventoryManagement.UseCases.Products.Interfaces
{
    public interface IEditProductUseCase
    {
        Task ExecuteAsync(Product product);
    }
}
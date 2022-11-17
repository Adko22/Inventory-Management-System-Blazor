using IventoryManagement.CoreBusiness;

namespace IventoryManagement.UseCases.Products.Interfaces
{
    public interface IAddProductUseCase
    {
        Task ExecuteAsync(Product product);
    }
}
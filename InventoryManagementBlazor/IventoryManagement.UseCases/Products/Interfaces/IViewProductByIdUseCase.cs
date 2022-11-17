using IventoryManagement.CoreBusiness;

namespace IventoryManagement.UseCases.Products.Interfaces
{
    public interface IViewProductByIdUseCase
    {
        Task<Product?> ExecuteAsync(int porductId);
    }
}
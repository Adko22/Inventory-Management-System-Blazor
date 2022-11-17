using IventoryManagement.CoreBusiness;

namespace IventoryManagement.UseCases.Products.Interfaces
{
    public interface IViewProductsByNameUseCase
    {
        Task<IEnumerable<Product>> ExecuteAsync(string name = "");
    }
}
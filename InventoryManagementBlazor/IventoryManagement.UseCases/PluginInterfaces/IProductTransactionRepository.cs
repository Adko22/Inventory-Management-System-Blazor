using IventoryManagement.CoreBusiness;

namespace IventoryManagement.UseCases.PluginInterfaces
{
    public interface IProductTransactionRepository
    {
        Task ProduceAsync(string productionNumber, Product product, int quantity, string doneBy);
        Task SellProductAsync(string salesOrderNumber, Product product, int quantity, decimal unitPrice, string doneBy);
    }
}
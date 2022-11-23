using IventoryManagement.CoreBusiness;

namespace IventoryManagement.UseCases.PluginInterfaces
{
    public interface IInventoryTransactionRepositoy
    {
        Task PurchaseAsync(string poNumber, Inventory inventory, int quantity, string doneBy, decimal price);
        Task ProduceAsync(string productionNumber, Inventory inventory, int quantityToCosume, string doneBy, decimal price);
        Task<IEnumerable<InventoryTransaction>> GetInventoryTransactionsAsync(string inventoryName, 
            DateTime? dateFrom, DateTime? dateTo, InventoryTransactionType? transactionType);
    }
}
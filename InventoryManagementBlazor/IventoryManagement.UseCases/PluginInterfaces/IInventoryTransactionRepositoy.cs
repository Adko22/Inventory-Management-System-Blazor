using IventoryManagement.CoreBusiness;

namespace IventoryManagement.UseCases.PluginInterfaces
{
    public interface IInventoryTransactionRepositoy
    {
        void PurchaseAsync(string poNumber, Inventory inventory, int quantity, string doneBy, decimal price);
        void ProduceAsync(string productionNumber, Inventory inventory, int quantityToCosume, string doneBy, decimal price);
    }
}
using IventoryManagement.CoreBusiness;

namespace IventoryManagement.UseCases.Iventories.Interfaces
{
    public interface IViewInventoriesByNameUseCase
    {
        Task<IEnumerable<Inventory>> ExecuteAsync(string name = "");
    }
}
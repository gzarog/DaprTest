using DaprBlocks.Models;
using Inventory.Models;

namespace InventoryService.Interfaces
{
    public interface IInventoryRepository
    {
        Task<bool> UpdateStateInventory(Order order);
        Task<InventoryModel> GetInventoryItemFromState(string itemName);
        Task InitDataToState();
    }
}

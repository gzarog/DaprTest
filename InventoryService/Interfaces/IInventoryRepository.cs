using DaprBlocks.Models;
using Inventory.Models;

namespace InventoryService.Interfaces
{
    public interface IInventoryRepository
    {
        bool UpdateInventory(Order order);
        Task<bool> UpdateStateInventory(Order order);
        Task<InventoryModel> GetInventoryItemFromState(string itemName);
        InventoryModel GetInventory(string itemName);
    }
}

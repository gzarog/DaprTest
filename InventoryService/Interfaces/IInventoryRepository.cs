using DaprBlocks.Models;
using Inventory.Models;

namespace InventoryService.Interfaces
{
    public interface IInventoryRepository
    {
        bool UpdateInventory(Order order);
        InventoryModel GetInventory(string itemName);
    }
}

using DaprBlocks.Models;
using InventoryService.Interfaces;
using Inventory.Models;

namespace InventoryService.Repositories
{
    public class InMemoryInventoryRepository : IInventoryRepository
    {
        private readonly Dictionary<string, InventoryModel> _inventoryDb = new();

        public InMemoryInventoryRepository()
        {
            // Initialize with some dummy data
            _inventoryDb.Add("Item1", new InventoryModel { ItemName = "Item1", Quantity = 100 });
            _inventoryDb.Add("Item2", new InventoryModel { ItemName = "Item2", Quantity = 200 });
            // Add more items as needed
        }

        public bool UpdateInventory(Order order)
        {
            if (_inventoryDb.ContainsKey(order.ItemName))
            {
                if (_inventoryDb[order.ItemName].Quantity >= order.Quantity)
                {
                    _inventoryDb[order.ItemName].Quantity -= order.Quantity;
                    return true;
                }
            }
            return false;
        }

        public InventoryModel GetInventory(string itemName)
        {
            return _inventoryDb.ContainsKey(itemName) ? _inventoryDb[itemName] : null;
        }
    }
}

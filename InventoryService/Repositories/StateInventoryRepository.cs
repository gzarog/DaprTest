using DaprBlocks.Models;
using InventoryService.Interfaces;
using Inventory.Models;
using Dapr.Client;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace InventoryService.Repositories
{
    public class StateInventoryRepository : IInventoryRepository
    {
        private readonly Dictionary<string, InventoryModel> _inventoryDb = new();
        private readonly DaprClient daprClient;
        private const string stateStoreName = "redis-state";

        public StateInventoryRepository(
            DaprClient daprClient
            )
        {
            this.daprClient = daprClient;

            _ = InitDataToState();
        }

        private async Task InitDataToState()
        {
            // Initialize with some dummy state  data
            await daprClient.SaveStateAsync(stateStoreName, "Item1", new InventoryModel { ItemName = "Item1", Quantity = 100 });
            await daprClient.SaveStateAsync(stateStoreName, "Item2", new InventoryModel { ItemName = "Item2", Quantity = 200 });
            
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


        public async Task<InventoryModel> GetInventoryItemFromState(string itemName)
        {
            return await daprClient.GetStateAsync<InventoryModel>(stateStoreName, itemName); 
        }

        public async Task<bool> UpdateStateInventory(Order order)
        {

            var inventoryItem = await GetInventoryItemFromState(order.ItemName);

            if (inventoryItem != null)
            {
                if (inventoryItem.Quantity >= order.Quantity)
                {
                    inventoryItem.Quantity -= order.Quantity;

                    await daprClient.SaveStateAsync(stateStoreName, inventoryItem.ItemName, inventoryItem);
                    return true;
                }

            }
            return false;
        }

    }
}

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
        private readonly DaprClient _daprClient;
        private readonly ILogger<StateInventoryRepository> _log;
        private const string stateStoreName = "redis-state";
        
        public StateInventoryRepository(
                DaprClient daprClient,
             ILogger<StateInventoryRepository> log
            )
        {
            _daprClient = daprClient;
           _log = log;
        }

        public async Task InitDataToState()
        {
            // Initialize with some dummy state  data
            _log.LogInformation("Init State Data");
            await _daprClient.SaveStateAsync(stateStoreName, "Item1", new InventoryModel { ItemName = "Item1", Quantity = 100 });
            await _daprClient.SaveStateAsync(stateStoreName, "Item2", new InventoryModel { ItemName = "Item2", Quantity = 200 });
            
        }
        

        public async Task<InventoryModel> GetInventoryItemFromState(string itemName)
        {
            return await _daprClient.GetStateAsync<InventoryModel>(stateStoreName, itemName); 
        }

        public async Task<bool> UpdateStateInventory(Order order)
        {

            var inventoryItem = await GetInventoryItemFromState(order.ItemName);

            if (inventoryItem != null)
            {
                if (inventoryItem.Quantity >= order.Quantity)
                {
                    inventoryItem.Quantity -= order.Quantity;

                    await _daprClient.SaveStateAsync(stateStoreName, inventoryItem.ItemName, inventoryItem);
                    _log.LogInformation("Update State Inventory Success");
                    return true;
                }

            }
            _log.LogInformation("Update State Inventory Fail");
            return false;
        }

    }
}

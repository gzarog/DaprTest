using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using DaprBlocks.Models;
using Inventory.Models;
using System.Collections.Concurrent;
using InventoryService.Repositories;
using InventoryService.Interfaces;

namespace Inventory.Controllers
{
    public class InventoryController :ControllerBase
    {
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryController(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        [Topic("redis-pubsub", "updateinventory")]
        [Topic("rabbitmq-pubsub", "updateinventory")]
        [HttpPost("UpdateInventory")]
        public async Task<IActionResult> UpdateInventory([FromBody] Order order, [FromServices] DaprClient daprClient)
        {
            var inventoryUpdated = await _inventoryRepository.UpdateStateInventory(order);
            return Ok(inventoryUpdated);
        }
        
        [HttpGet("{itemName}")]
        public async Task<IActionResult> GetInventory(string itemName)
        {
            var inventory = await _inventoryRepository.GetInventoryItemFromState(itemName);
            if (inventory == null)
            {
                return NotFound();
            }
            return Ok(inventory);
        }

    }
}

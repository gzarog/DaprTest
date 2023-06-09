﻿using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using DaprBlocks.Models;
using System.Net.Sockets;
using System.Threading;

namespace Orders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly DaprClient _daprClient;

        public OrdersController(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            var inventoryUpdated = false;
            // Assuming the method on the InventoryService is named "UpdateInventory"
            // and the application id of the InventoryService is "inventoryservice"
            try
            {
                
                if (order.Method.Equals("http") ) 
                {
                    
                    inventoryUpdated = await _daprClient.InvokeMethodAsync<Order, bool>(HttpMethod.Post, "inventoryservice", "UpdateInventory", order);
                }

                if (order.Method.Equals("redis"))
                {
                    await _daprClient.PublishEventAsync("redis-pubsub", "updateinventory", order).ConfigureAwait(true);
                    inventoryUpdated = true;
                }


                if (order.Method.Equals("rabbit"))
                {
                    await _daprClient.PublishEventAsync("rabbitmq-pubsub", "updateinventory", order).ConfigureAwait(true);
                    inventoryUpdated = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                
            }
            

            if (inventoryUpdated)
            {
                return Ok("Order Placed");
            }
            else
            {
                return BadRequest("Failed to update inventory");
            }
        }
    }
}

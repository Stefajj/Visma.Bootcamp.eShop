using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.DTO;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.Models;
using Visma.Bootcamp.eShop.ApplicationCore.Services.Interfaces;

namespace Visma.Bootcamp.eShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /*
            Pre kazdu operaciu v tomto controlleri, nezabudnite pridat
            [SwaggerOperation(
                summary: "Kratky popis endpointu",
                description: "Trosku dlhsi popis endpointu, kludne aj detaily business logiky",
                OperationId = "JedinecneIdOperacie",
                Tags = new[] { "Order API" })]
        */

        // Navrh metod do tohto controllera:
        // - GET {order_id}
        // - DELETE {order_id]
        // - PUT {order_id}

        // [HttpGet]
        // [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OrderDto>))]
        // [SwaggerOperation(
        //     summary: "Retrieve all orders from the system",
        //     description: "Return all orders",
        //     OperationId = "GetOrders",
        //     Tags = new[] { "Order API" })]
        // public async Task<IActionResult> GetOrdersAsync(CancellationToken ct, int? pageSize = null)
        // {
        //     List<OrderDto> listOfOrders = await _orderService.GetAllAsync(pageSize,ct);
        //     if(pageSize != null)
        //     {
        //         var pagedList = new PagedListModel<OrderDto>(listOfOrders, pageSize.Value);
        //         return Ok(pagedList);
        //     }
        //     return Ok(listOfOrders);
        // }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrderDto))]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [SwaggerOperation(
            summary: "Create new order",
            description: "Create new order in the database",
            OperationId = "CreateOrder",
            Tags = new[] { "Order API" })]
        public async Task<IActionResult> CreateOrderAsync(BasketDto basketDto,CancellationToken ct){
            var orderDto = await _orderService.CreateAsync(basketDto, ct);
            return StatusCode(StatusCodes.Status201Created,orderDto);
        }

        [HttpPut("{order_id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [SwaggerOperation(
            summary: "Update existing order",
            description: "Update order",
            OperationId = "UpdateOrder",
            Tags = new[] { "Order API" })]
        public async Task<IActionResult> UpdateOrderAsync(
            [Required, FromRoute(Name = "order_id")] Guid? orderId,
            [Required, FromBody] OrderModel orderModel,
            CancellationToken ct
        )
        {
            await _orderService.UpdateAsync(orderId,orderModel,ct);
            return NoContent();
        }

        [HttpDelete("{order_id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            summary: "Delete existing order",
            description: "Delete order from the database",
            OperationId = "OrderCatalog",
            Tags = new[] { "Order API" })]
        public async Task<IActionResult> DeleteOrder(
            [Required, FromRoute(Name = "order_id")] Guid? orderId,
            CancellationToken ct
        )
        {
            _orderService.DeleteAsync(orderId, ct);
            return NoContent();
        }
    }
}

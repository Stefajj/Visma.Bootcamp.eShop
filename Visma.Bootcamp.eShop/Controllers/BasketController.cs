using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.DTO;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.Models;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.Models.Errors;
using Visma.Bootcamp.eShop.ApplicationCore.Services.Interfaces;

namespace Visma.Bootcamp.eShop.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpPost("{basket_id}/items")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(BasketDto))]
        [SwaggerOperation(
            summary: "Add product to the basket",
            description: "Associate given productId with actual basket of the user, which is represented by basketId",
            OperationId = "AddToBasket",
            Tags = new[] { "Basket Management" })]
        public async Task<IActionResult> AddToBasketAsync(
            [Required, FromRoute(Name = "basket_id")] Guid? basketId,
            [Bind, FromBody] BasketItemModel model,
            CancellationToken ct)
        {
            BasketDto basket = _basketService.AddProduct(basketId, model);
            return StatusCode(201, basket);
        }

        [HttpGet("{basket_id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BasketDto))]
        [SwaggerOperation(
            summary: "Retrieve basket",
            description: "Returns basket by given basketId with all products",
            OperationId = "GetBasket",
            Tags = new[] { "Basket API" })]
        public async Task<IActionResult> GetBasketAsync(
            [Required, FromRoute(Name = "basket_id")] Guid? basketId,
            CancellationToken ct)
        {
        //     var basketDto = new BasketDto
        //     {
        //         BasketId = Guid.NewGuid(),
        //         Items = new List<BasketItemDto>
        //         {
        //             new BasketItemDto
        //             {
        //                 Product = new ProductDto{
        //                     ProductId = Guid.NewGuid(),
        //                     Name = "test product #1",
        //                     Description = "test decription #1",
        //                     Price = 128.34M
        //                 },
        //                 Quantity = 5
        // },
        //             new BasketItemDto{
        //                 Product = new ProductDto{
        //                    ProductId = Guid.NewGuid(),
        //                 Name = "test product #2",
        //                 Description = "test description #2",
        //                 Price = 25.99M
        //                 },
        //                 Quantity = 3
        //             },
        //             new BasketItemDto{
        //                 Product = new ProductDto{
        //                    ProductId = Guid.NewGuid(),
        //                 Name = "test product #3",
        //                 Description = "test description #3",
        //                 Price = 49.99M
        //                 },
        //                 Quantity = 2
        //             }
        //         }
        //     };
            var basket = _basketService.Get(basketId);
            return Ok(basket);
        }

        [HttpPut("{basket_id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestError))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundError))]
        [SwaggerOperation(
            summary: "Update basket",
            description: "Update basket based on given basketId with items collection and their quantities",
            OperationId = "UpdateBasket",
            Tags = new[] { "Basket Management" })]
        public async Task<IActionResult> UpdateBasketAsync(
            [Required, FromRoute(Name = "basket_id")] Guid? basketId,
            [FromBody, Bind] BasketModel model,
            CancellationToken ct)
        {
            _basketService.Update(basketId, model);
            return NoContent();
        }

        [HttpDelete("{basket_id}/items/{item_id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestError))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundError))]
        [SwaggerOperation(
            summary: "Remove item from basket",
            description: "Remove item from the basket by given basketId and itemId",
            OperationId = "RemoveItemFromBasket",
            Tags = new[] { "Basket Management" })]
        public async Task<IActionResult> DeleteFromBasketAsync(
            [Required, FromRoute(Name = "basket_id")] Guid? basketId,
            [Required, FromRoute(Name = "item_id")] Guid? itemId,
            CancellationToken ct)
        {
            _basketService.DeleteProduct(basketId, itemId);
            return NoContent();
        }
    }
}

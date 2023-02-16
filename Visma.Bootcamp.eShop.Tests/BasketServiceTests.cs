using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.Models;
using Visma.Bootcamp.eShop.ApplicationCore.Infrastructure;
using Visma.Bootcamp.eShop.ApplicationCore.Services;
using Visma.Bootcamp.eShop.ApplicationCore.Services.Interfaces;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.DTO;
using Visma.Bootcamp.eShop.ApplicationCore.Exceptions;

namespace Visma.Bootcamp.eShop.Tests
{
    [TestFixture]
    public class BasketServiceTests
    {

        private ICacheManager _cacheManager;
        private IProductService _productService;
        private IBasketService _basketService;

        [SetUp]
        public void SetUp()
        {
            _cacheManager = Substitute.For<ICacheManager>();
            _productService = Substitute.For<IProductService>();
            _basketService = new BasketService(_cacheManager,_productService);
        }

         [Test]
    public async Task AddItemAsync_ValidInput_AddsItemToBasket()
    {
        // Arrange
        var basketId = Guid.NewGuid();
        var model = new BasketItemModel { ProductId = Guid.NewGuid(), Quantity = 5 };
        var product = new ProductDto { PublicId = model.ProductId.Value, Name = "Test Product", Description = "Test Description", Price = 10 };
        var basket = new BasketDto { BasketId = basketId, Items = new List<BasketItemDto>() };

        _cacheManager.GetAsync<BasketDto>(basketId, Arg.Any<CancellationToken>()).Returns(Task.FromResult(basket));
        _cacheManager.SetAsync(Arg.Any<BasketDto>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        _productService.GetAsync(model.ProductId.Value, Arg.Any<CancellationToken>()).Returns(Task.FromResult(product));

        var expected = new BasketDto
        {
            BasketId = basketId,
            Items = new List<BasketItemDto>
            {
                new BasketItemDto
                {
                    Product = product,
                    Quantity = model.Quantity
                }
            }
        };

        // Act
        var result = await _basketService.AddItemAsync(basketId, model);

        // Assert
        Assert.AreEqual(result.BasketId, expected.BasketId);
        Assert.AreEqual(result.Items.Count, expected.Items.Count);

        await _cacheManager.Received(1).SetAsync(result, Arg.Any<CancellationToken>());
    }
        [Test]
        public async void AddItemAsync_InvalidQuantity_ThrowsBadRequest(){
            // Arrange
            var basketId = Guid.NewGuid();
            var model = new BasketItemModel { ProductId = Guid.NewGuid(), Quantity = 21 };
            var product = new ProductDto { PublicId = model.ProductId.Value, Name = "Test Product", Description = "Test Description", Price = 10 };
            var basket = new BasketDto { BasketId = basketId, Items = new List<BasketItemDto>() };

            _cacheManager.GetAsync<BasketDto>(basketId, Arg.Any<CancellationToken>()).Returns(Task.FromResult(basket));
            _cacheManager.SetAsync(Arg.Any<BasketDto>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
            _productService.GetAsync(model.ProductId.Value, Arg.Any<CancellationToken>()).Returns(Task.FromResult(product));

            var expected = new BasketDto
            {
                BasketId = basketId,
                Items = new List<BasketItemDto>
                {
                    new BasketItemDto
                    {
                        Product = product,
                        Quantity = model.Quantity
                    }
                }   
            };

            // Act
            var result = await _basketService.AddItemAsync(basketId, model);

            // Assert
            Assert.ThrowsAsync<BadRequestException>(()=>_basketService.AddItemAsync(basketId,model));

            await _cacheManager.Received(1).SetAsync(result, Arg.Any<CancellationToken>());
        }

        [Test]
        public async Task AddItemAsync_QuantityExceedsLimit_ThrowsUnprocessableEntityException()
        {
            //arrange
            //act
            //assert    
        }

        [Test]
        public async Task DeleteItemAsync_BasketDoesNotExist()
        {
            //arrange
            //act
            Assert.ThrowsAsync<NotFoundException>(() => _basketService.DeleteItemAsync(Guid.NewGuid(), Guid.NewGuid()));
            //assert
        }
    }
}
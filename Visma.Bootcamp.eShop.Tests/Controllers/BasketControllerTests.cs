using System;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.Models;
using Visma.Bootcamp.eShop.ApplicationCore.Services.Interfaces;
using Visma.Bootcamp.eShop.Controllers;
using Xunit;

namespace Visma.Bootcamp.eShop.Tests.Controllers
{
    public class BasketControllerTests
    {
        private IBasketService _basketService;

        public BasketControllerTests()
        {
            _basketService = Substitute.For<IBasketService>();
        }

        private BasketController CreateBasketController()
        {
            return new BasketController(_basketService);
        }

        [Fact]
        public async Task AddToBasketAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var basketController = this.CreateBasketController();
            Guid? basketId = null;
            BasketItemModel model = null;
            CancellationToken ct = default(global::System.Threading.CancellationToken);

            // Act
            var result = await basketController.AddToBasketAsync(
                basketId,
                model,
                ct);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task GetBasketAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var basketController = this.CreateBasketController();
            Guid? basketId = null;
            CancellationToken ct = default(global::System.Threading.CancellationToken);

            // Act
            var result = await basketController.GetBasketAsync(
                basketId,
                ct);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task UpdateBasketAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var basketController = this.CreateBasketController();
            Guid? basketId = null;
            BasketModel model = null;
            CancellationToken ct = default(global::System.Threading.CancellationToken);

            // Act
            var result = await basketController.UpdateBasketAsync(
                basketId,
                model,
                ct);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task DeleteFromBasketAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var basketController = this.CreateBasketController();
            Guid? basketId = null;
            Guid? itemId = null;
            CancellationToken ct = default(global::System.Threading.CancellationToken);

            // Act
            var result = await basketController.DeleteFromBasketAsync(
                basketId,
                itemId,
                ct);

            // Assert
            Assert.True(false);
        }
    }
}

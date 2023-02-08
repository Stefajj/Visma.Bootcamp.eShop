using System.Linq;
using System;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.DTO;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.Models;
using Visma.Bootcamp.eShop.ApplicationCore.Exceptions;
using Visma.Bootcamp.eShop.ApplicationCore.Infrastructure;
using Visma.Bootcamp.eShop.ApplicationCore.Services.Interfaces;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Visma.Bootcamp.eShop.ApplicationCore.Services
{
    internal class BasketService : IBasketService
    {
        private readonly CacheManager _cache;
        private readonly ILogger<BasketService> _logger;
        public BasketService(CacheManager cache, ILogger<BasketService> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public BasketDto AddProduct(Guid? basketId, BasketItemModel model)
        {
            var basket = getBasket(basketId);

            var product = _cache.Get<ProductDto>(model.ProductId.Value);

            if (product == null) {
                _logger.LogWarning("Product with ID {ProductId} not found",model.ProductId);
                throw new NotFoundExceptions(typeof(ProductDto),model.ProductId.Value);
            }

            var existingProduct = basket.Items.SingleOrDefault(x => x.Product.ProductId == model.ProductId.Value);

            if(existingProduct == null){
                _logger.LogDebug("Product with ID {ProductId} doesn't exist in basket, creating new one",model.ProductId);
                basket.Items.Add(new BasketItemDto
                {
                    Product = product,
                    Quantity = model.Quantity
                });
            }else {
                //
                _logger.LogDebug("Updating product with ID {ProductId} with quantity", model.ProductId);
                existingProduct.Quantity += model.Quantity;
            }

            _cache.Set<BasketDto>(basket);
            return basket;
        }

        public void DeleteProduct(Guid? basketId, Guid? itemId)
        {
            var basket = getBasket(basketId);
            BasketItemDto item = basket.Items.SingleOrDefault(x => x.Product.ProductId == itemId);

            if (item == null) {
                _logger.LogDebug("Didn't delete item because {itemId} doesn't exist", itemId);
                throw new NotFoundExceptions(typeof(BasketItemDto),itemId.Value);
            }

            basket.Items.Remove(item);
            _cache.Set<BasketDto>(basket);
        }

        public BasketDto Get(Guid? basketId)
        {
            return getBasket(basketId);
        }

        public void Update(Guid? basketId, BasketModel model)
        {
            var basket = getBasket(basketId);
            foreach (var modelItem in model.Items) 
            {
                BasketItemDto basketItem = basket.Items.SingleOrDefault(x => x.Product.ProductId == modelItem.ProductId.Value);
                if (basketItem == null) 
                {
                    continue;
                }
                basketItem.Quantity = modelItem.Quantity;
            }


            _cache.Set(basket);
        }

        private BasketDto getBasket(Guid? basketId) { 
             var basket = _cache.Get<BasketDto>(basketId.Value);
            
            if (basket == null) {
                _logger.LogWarning("Basket with ID {Id} doesn't exist, creating new one",basketId);
                basket = new BasketDto{
                    BasketId = basketId.Value,
                    Items = new List<BasketItemDto>()
                };
            }

            _cache.Set(basket);
            return basket;
        }
    }
}

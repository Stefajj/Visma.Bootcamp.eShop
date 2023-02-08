using System;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.DTO;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.Models;
using Visma.Bootcamp.eShop.ApplicationCore.Exceptions;
using Visma.Bootcamp.eShop.ApplicationCore.Infrastructure;
using Visma.Bootcamp.eShop.ApplicationCore.Services.Interfaces;

namespace Visma.Bootcamp.eShop.ApplicationCore.Services
{
    internal class BasketService : IBasketService
    {
        private readonly CacheManager _cache;
        public BasketService(CacheManager cache)
        {
            _cache = cache;
        }

        public BasketDto AddProduct(Guid? basketId, BasketItemModel model)
        {
            var basket = _cache.Get<BasketDto>(basketId.Value);

            if (basket == null) {
                basket = new BasketDto
                {
                    BasketId = basketId.Value
                };

                _cache.Set(basket);
            }

            var product = _cache.Get<ProductDto>(model.ProductId.Value);

            if (product == null) {
                throw new NotFoundExceptions(typeof(ProductDto),model.ProductId.Value);
            }

            basket.Items.Add(product);
            _cache.Set<BasketDto>(basket);
            return basket;
        }

        public void DeleteProduct(Guid? basketId, Guid? itemId)
        {
            throw new NotImplementedException();
        }

        public BasketDto Get(Guid? basketId)
        {
            throw new NotImplementedException();
        }

        public void Update(Guid? basketId, BasketModel model)
        {
            throw new NotImplementedException();
        }
    }
}

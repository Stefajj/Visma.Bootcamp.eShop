using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.DTO;

namespace Visma.Bootcamp.eShop.ApplicationCore.Infrastructure
{
    public class CacheManager
    {
        private readonly IDictionary<Type, string> _domainTypes;
        private readonly IMemoryCache _cache;

        public CacheManager(IMemoryCache cache)
        {
            _cache = cache;
            _domainTypes = new Dictionary<Type, string>
            {
                { typeof(ProductDto), "Products" },
                { typeof(CatalogDto), "Catalogs" },
                { typeof(OrderDto), "Orders" },
                { typeof(BasketDto), "Baskets" }
            };
        }

        public void Set<T>(T item) where T : ICacheableDto
        {
            string category = GetCategoryOrThrow(typeof(T));
            if (_cache.TryGetValue(category, out List<T> cachedItems))
            {
                T existingItem = cachedItems.SingleOrDefault(x => x.Id == item.Id);
                if (existingItem != null) {
                    cachedItems.Remove(existingItem);
                }
                cachedItems.Add(item);
            }
            else
            {
                cachedItems = new List<T> { item };
            }

            _cache.Set(category, cachedItems, TimeSpan.FromHours(24));
        }

        public List<T> Get<T>() where T : ICacheableDto
        {
            string category = GetCategoryOrThrow(typeof(T));
            return _cache.GetOrCreate(category, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24);
                return new List<T>();
            });
        }

        public T Get<T>(Guid itemId) where T : ICacheableDto
        {
            string category = GetCategoryOrThrow(typeof(T));
            List<T> items = Get<T>(); // get all items for given category
            return items.SingleOrDefault(x => x.Id == itemId);
        }

        private string GetCategoryOrThrow(Type itemType)
        {
            return !_domainTypes.ContainsKey(itemType)
                ? throw new Exception("Unsupported type of cacheable object")
                : _domainTypes[itemType];
        }
    }
}

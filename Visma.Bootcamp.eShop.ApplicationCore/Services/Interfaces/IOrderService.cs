using System.Threading;
using System;
using System.Threading.Tasks;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.DTO;
using System.Collections.Generic;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.Models;

namespace Visma.Bootcamp.eShop.ApplicationCore.Services.Interfaces
{
    public interface IOrderService
    {
        #region Order Operations
        Task<OrderDto> GetAsync(Guid? orderId,CancellationToken ct = default);
        // Task<List<OrderDto>> GetOrdersByIdAsync(Guid? userId, CancellationToken ct = default);
        Task<List<OrderDto>> GetAllAsync(int? pageSize = null, CancellationToken ct = default);
        Task<OrderDto> CreateAsync(BasketDto basket, CancellationToken ct = default);
        Task<OrderDto> UpdateAsync(Guid? orderId, OrderModel model, CancellationToken ct = default);
        void DeleteAsync(Guid? orderId, CancellationToken ct = default);
        #endregion

        #region Order Item Operations
        Task<OrderItemDto> AddItemAsync(Guid? orderId,OrderItemModel model, CancellationToken ct = default);
        void DeleteItemAsync(Guid? orderId, Guid? productId, CancellationToken ct = default);
        void cancelOrderAsync(Guid? orderId, CancellationToken ct);
        #endregion
    }
}

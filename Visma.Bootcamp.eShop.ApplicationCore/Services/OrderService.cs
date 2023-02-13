using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Visma.Bootcamp.eShop.ApplicationCore.Database;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.Domain;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.DTO;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.Models;
using Visma.Bootcamp.eShop.ApplicationCore.Exceptions;
using Visma.Bootcamp.eShop.ApplicationCore.Services.Interfaces;

namespace Visma.Bootcamp.eShop.ApplicationCore.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationContext _context;

        public OrderService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<OrderItemDto> AddItemAsync(Guid? orderId, OrderItemModel model, CancellationToken ct = default)
        {
            Order order = await _context.Orders
                .AsNoTracking()
                .Include(x => x.Items)
                    .ThenInclude(x => x.product)
                .SingleOrDefaultAsync(x => x.PublicId == orderId, ct);
            
            if (order == null)
            {
                throw new NotFoundException($"Order with ID: {orderId} not found");
            }

            Product product = await _context.Products
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.PublicId == model.ProductId, ct);
            
            if (product == null)
            {
                throw new NotFoundException($"Product with ID: {model.ProductId} not found");
            }

            OrderItem item = order.Items.SingleOrDefault(x => x.ProductId == product.Id);
            if (item == null)
            {
                item = model.ToDomain();
                item.ProductId = product.Id;
                item.Price = product.Price;

                order.Items.Add(item);
                _context.Orders.Update(order);
            }
            else
            {
                item.Quantity += model.Quantity;
                _context.OrderItems.Update(item);
            }

            await _context.SaveChangesAsync(ct);
            OrderItemDto dto = item.ToDto();
            return dto;
        }

        public async void cancelOrderAsync(Guid? orderId, CancellationToken ct)
        {
            var order = await _context.Orders.SingleOrDefaultAsync(x => x.PublicId==orderId);
            if(order == null){
                throw new NotFoundException("Does not exist");
            }

            order.status = OrderStatus.Cancelled;
            await _context.SaveChangesAsync(ct);
        }

        public async Task<OrderDto> CreateAsync(BasketDto basket, CancellationToken ct = default)
        {
            var order = new Order
            {
                CreateDate = DateTime.UtcNow,
                PublicId = Guid.NewGuid(),
                Items = new List<OrderItem>()
            };

            foreach (BasketItemDto basketItem in basket.Items)
            {
                Product product = await _context.Products
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.PublicId == basketItem.Product.PublicId, ct);

                if (product == null)
                {
                    throw new NotFoundException($"Product with ID: {basketItem.Product.PublicId} not found");
                }

                var orderItem = new OrderItem
                {
                    Quantity = basketItem.Quantity,
                    Price = basketItem.Product.Price,
                    ProductId = product.Id
                };

                order.Items.Add(orderItem);
            }

            await _context.Orders.AddAsync(order, ct);
            await _context.SaveChangesAsync(ct);

            await _context.Entry(order)
                .Collection(x => x.Items)
                .Query()
                .Include(x => x.product)
                .LoadAsync();

            OrderDto dto = order.ToDto(includeItems: true);
            return dto;
        }

        public async void DeleteAsync(Guid? orderId, CancellationToken ct = default)
        {
            var order = await _context.Orders.AsNoTracking().SingleOrDefaultAsync(o => o.PublicId == orderId);
            if(order == null)
            {
                throw new NotFoundException($"Order with ID {orderId} was not found");
            }
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

        public async void DeleteItemAsync(Guid? orderId, Guid? productId, CancellationToken ct = default)
        {
            var order = await _context.Orders.AsNoTracking().SingleOrDefaultAsync(o => o.PublicId == orderId,ct);
            var product = await _context.Products.AsNoTracking().SingleOrDefaultAsync(p => p.PublicId == productId,ct);
            if(order == null)
            {
                throw new NotFoundException($"Order with ID {orderId} does not exist.");
            }
            if(product == null)
            {
                throw new NotFoundException($"Product with ID {productId} does not exist.");
            }
            var productToDelete = order.Items.SingleOrDefault(i => i.ProductId == product.Id);

            if(productToDelete == null)
            {
                throw new NotFoundException($"Product with ID {productId} was not found in order with ID {orderId}");
            }

            _context.OrderItems.Remove(productToDelete);
            await _context.SaveChangesAsync(ct);
        }

        public async Task<List<OrderDto>> GetAllAsync(int? pageSize = null, CancellationToken ct = default)
        {
            var query = _context.Orders.AsNoTracking().AsQueryable();

            if( pageSize > 0)
            {
                query = query.Take(pageSize.Value);
            }
            List<Order> orders = await query.ToListAsync(ct);
            List<OrderDto> ordersDto = orders.Select(o => o.ToDto()).ToList();
            return ordersDto;
        }

        public async Task<OrderDto> GetAsync(Guid? orderId, CancellationToken ct = default)
        {
            var order = await _context.Orders
                .AsNoTracking()
                .Include(x => x.Items)
                .ThenInclude(x => x.product)
                .SingleOrDefaultAsync(o => o.PublicId == orderId);

            if(order == null){
                throw new Exception($"Order with ID {orderId} does not exist");
            }

            return order.ToDto();
        }

        public Task<OrderDto> UpdateAsync(Guid? orderId, OrderModel model, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.Domain;

namespace Visma.Bootcamp.eShop.ApplicationCore.Entities.Models
{
    public class OrderModel
    {
        public List<OrderItemModel> Items { get; set; }

        // public OrderItem ToDomain()
        // {
        //     return new OrderItem()
        //     {
        //         Quantity = this.Quantity
        //     };
        // }
    }
    
}
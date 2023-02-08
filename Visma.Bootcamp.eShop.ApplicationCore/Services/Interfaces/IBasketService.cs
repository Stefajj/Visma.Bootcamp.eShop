using System;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.DTO;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.Models;

namespace Visma.Bootcamp.eShop.ApplicationCore.Services.Interfaces
{
    public interface IBasketService
    {
        #region Product

        BasketDto AddProduct(Guid? basketId,BasketItemModel model);
        void DeleteProduct(Guid? basketId, Guid? itemId);
        
        #endregion

        #region Basket

        BasketDto Get(Guid? basketId);
        void Update(Guid? basketId,BasketModel model);
        
        #endregion
    }
}

﻿using System;

namespace Visma.Bootcamp.eShop.ApplicationCore.Entities.DTO
{
    public class OrderDto : ICacheableDto
    {
        public Guid Id => Guid.NewGuid();
    }
}

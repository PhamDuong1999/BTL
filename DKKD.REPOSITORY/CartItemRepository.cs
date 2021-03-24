using DKKD.MODELS;
using Microsoft.EntityFrameworkCore;
using Portal.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace DKKD.REPOSITORY
{
    public interface ICartItemRepository : IRepository<CartItem>
    {

    }
    public class CartItemRepository : Repository<CartItem>, ICartItemRepository
    {
        public CartItemRepository(ShopContext dbContext) : base(dbContext)
        {
        }
    }
}

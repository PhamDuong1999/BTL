using Microsoft.EntityFrameworkCore;
using Portal.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace DKKD.MODELS
{
    public interface IShopContextFactory : IDbContextFactory<ShopContext>
    {

    }
    public class ShopContextFactory : IShopContextFactory
    {
        private readonly DbContextOptions<ShopContext> _options;
        private ShopContext _context;
        public ShopContextFactory(DbContextOptions<ShopContext> options)
        {
            _options = options;
        }
        public ShopContext GetContext()
        {
            if (_context == null) _context = new ShopContext(_options);
            return _context;
        }
    }
}

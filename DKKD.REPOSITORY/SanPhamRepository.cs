using DKKD.MODELS;
using Microsoft.EntityFrameworkCore;
using Portal.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace DKKD.REPOSITORY
{
    public interface ISanPhamRepository : IRepository<SanPham>
    {
    }
    public class SanPhamRepository : Repository<SanPham>, ISanPhamRepository
    {
        public SanPhamRepository(ShopContext dbContext) : base(dbContext)
        {
        }
    }
}

using DKKD.MODELS;
using Microsoft.EntityFrameworkCore;
using Portal.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace DKKD.REPOSITORY
{
    public interface IDonHangReposiotry : IRepository<DonHang>
    {

    }
    public class DonHangRepository : Repository<DonHang>, IDonHangReposiotry
    {
        public DonHangRepository(ShopContext dbContext) : base(dbContext)
        {
        }
    }
}

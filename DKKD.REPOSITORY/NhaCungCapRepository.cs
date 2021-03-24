using DKKD.MODELS;
using Portal.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace DKKD.REPOSITORY
{
    public interface INhaCungCapRepository : IRepository<NhaCungCap>
    {

    }
    public class NhaCungCapRepository : Repository<NhaCungCap>, INhaCungCapRepository
    {
        public NhaCungCapRepository(ShopContext dbContext) : base(dbContext)
        {
        }
    }
}

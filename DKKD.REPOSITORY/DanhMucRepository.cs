using DKKD.MODELS;
using Microsoft.EntityFrameworkCore;
using Portal.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace DKKD.REPOSITORY
{
    public interface IDanhMucRepository : IRepository<DanhMuc>
    {

    }
    public class DanhMucRepository : Repository<DanhMuc>, IDanhMucRepository
    {
        public DanhMucRepository(ShopContext dbContext) : base(dbContext)
        {
        }
    }
}

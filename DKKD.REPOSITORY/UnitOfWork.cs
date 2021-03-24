using DKKD.MODELS;
using Microsoft.EntityFrameworkCore.Storage;
using Portal.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DKKD.REPOSITORY
{
    public interface IUnitOfWork : IDisposable
    {
        public INhaCungCapRepository NhaCungCapRepository { get; }
        public IDanhMucRepository DanhMucRepository { get; }
        public ISanPhamRepository SanPhamRepository { get; }
        public IDonHangReposiotry DonHangReposiotry { get; }
        public ICartItemRepository CartItemRepository { get; }
        Task CreateTransaction();
        void Commit();
        void Rollback();
        Task SaveChange();
    }
    public class UnitOfWork : IUnitOfWork
    {
        ShopContext _dbContext;
        IDbContextTransaction _transaction;

        public UnitOfWork(IDbContextFactory<ShopContext> dbContextFactory, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContextFactory.GetContext();
            NhaCungCapRepository = new NhaCungCapRepository(_dbContext);
            DanhMucRepository = new DanhMucRepository(_dbContext);
            SanPhamRepository = new SanPhamRepository(_dbContext);
            DonHangReposiotry = new DonHangRepository(_dbContext);
            CartItemRepository = new CartItemRepository(_dbContext);
        }
        


        private bool disposedValue = false; // To detect redundant calls

        public INhaCungCapRepository NhaCungCapRepository { get; }
        public IDanhMucRepository DanhMucRepository { get; }
        public ISanPhamRepository SanPhamRepository { get; }

        public IDonHangReposiotry DonHangReposiotry { get; }

        public ICartItemRepository CartItemRepository { get; }

        public async Task CreateTransaction()
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync();
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public async Task SaveChange()


        {
            await _dbContext.SaveChangesAsync();
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

    }
}

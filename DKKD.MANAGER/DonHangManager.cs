using DKKD.MODELS;
using DKKD.REPOSITORY;
using DKKD.UTILS;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKKD.MANAGER
{
    public interface IDonHangManager
    {
        Task<List<DonHang>> GetList(string name, int status, int pageSize = 10, int pageNumber = 1);
        Task<List<CartItem>> GetListCTDH(int id);
        Task Create(DonHang inputModel);
        Task Update(DonHang inputModel);
        Task Delete(int id);
        Task<DonHang> FindById(int id);
    }
    public class DonHangManager : IDonHangManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DonHang> _logger;

        public DonHangManager(IUnitOfWork unitOfWork, ILogger<DonHang> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Create(DonHang inputModel)
        {
            try
            {
                var data = await _unitOfWork.DonHangReposiotry.Add(inputModel);
                if (inputModel.ListCTDH != null)
                {
                    foreach (var item in inputModel.ListCTDH)
                    {
                        item.MaDH = data.ID;
                    }
                    await _unitOfWork.CartItemRepository.BulkInsert(inputModel.ListCTDH);
                }
                await _unitOfWork.SaveChange();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task Delete(int id)
        {
            var data = await _unitOfWork.DonHangReposiotry.Get(c => c.ID == id);
            await _unitOfWork.DonHangReposiotry.Delete(data);
            await _unitOfWork.SaveChange();
        }

        public async Task<DonHang> FindById(int id)
        {
            return await _unitOfWork.DonHangReposiotry.Get(x => x.ID == id);
        }

        public async Task<List<DonHang>> GetList(string name, int status, int pageSize = 10, int pageNumber = 1)
        {
            try
            {
                if (status == (int)StatusEnum.All)
                {
                    var rs = (await _unitOfWork.DonHangReposiotry.FindBy(x => (x.TrangThai != (int)StatusEnum.IsDelete)
                      && (string.IsNullOrEmpty(name) || x.TenKH.ToLower().Contains(name.ToLower())))).ToList();
                    return rs;
                }
                else
                {
                    var rs = (await _unitOfWork.DonHangReposiotry.FindBy(x => (x.TrangThai == status)
                      && (string.IsNullOrEmpty(name) || x.TenKH.ToLower().Contains(name.ToLower())))).ToList();
                    return rs;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<CartItem>> GetListCTDH(int id)
        {
            var rs = (await _unitOfWork.CartItemRepository.FindBy(x => x.MaDH == id)).ToList();
            foreach(var item in rs)
            {
                var sp = await _unitOfWork.SanPhamRepository.Get(x => x.ID == item.MaSP);
                item.TenSP = sp.TenSP;
                item.Avatar = sp.Avatar;
            }
            return rs;
        }

        public async Task Update(DonHang inputModel)
        {
            try
            {
                await _unitOfWork.DonHangReposiotry.Update(inputModel);
                await _unitOfWork.SaveChange();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, MessageConst.UPDATE_FAIL, null);
                //_unitOfWork.Rollback();
                throw new Exception(MessageConst.UPDATE_FAIL);
            }
        }
    }
}

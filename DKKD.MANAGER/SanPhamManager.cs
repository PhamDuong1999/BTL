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
    public interface ISanPhamManager
    {
        Task Create(SanPham inputModel);
        Task<SanPham> FindById(int id);
        Task Update(SanPham inputModel);

        Task<List<SanPham>> Get_list(string name, int status, int pageSize = 10, int pageNumber = 0);
        Task Delete(int id);

        Task<List<NhaCungCap>> GetListNCC();
        Task<List<DanhMuc>> GetListDM();
        Task<List<SanPham>> Getlist(string name, int idth, int iddm);
        Task<List<SanPham>> GetlistNew();
        Task<List<SanPham>> LaySPTU(int id);
        Task<List<SanPham>> LaySPSale();
        Task<List<SanPham>> LaySPHot();
    }
    public class SanPhamManager : ISanPhamManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SanPham> _logger;

        public SanPhamManager(IUnitOfWork unitOfWork, ILogger<SanPham> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Create(SanPham inputModel)
        {
            try
            {
                await _unitOfWork.SanPhamRepository.Add(inputModel);
                await _unitOfWork.SaveChange();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Delete(int id)
        {
            var data = await _unitOfWork.SanPhamRepository.Get(x => x.ID == id);
            await _unitOfWork.SanPhamRepository.Delete(data);
            await _unitOfWork.SaveChange();
        }

        public async Task<SanPham> FindById(int id)
        {
            return await _unitOfWork.SanPhamRepository.Get(x => x.ID == id);
        }

        public async Task<List<SanPham>> Getlist(string name = "", int idth = -1, int iddm = -1)
        {
            try
            {
                List<SanPham> data = new List<SanPham>();
                if (idth == -1 && iddm != -1)
                {
                    data = (await _unitOfWork.SanPhamRepository.FindBy(x => x.MaDM == iddm && x.TrangThai == 1
                    && (string.IsNullOrEmpty(name) || x.TenSP.ToLower().Contains(name.ToLower())))).ToList();
                    return data;
                }
                if (iddm == -1 && idth != -1)
                {
                    data = (await _unitOfWork.SanPhamRepository.FindBy(x => x.MaNCC == idth && x.TrangThai == 1
                    && (string.IsNullOrEmpty(name) || x.TenSP.ToLower().Contains(name.ToLower())))).ToList();
                    return data;
                }
                if (iddm == -1 && idth == -1)
                {
                    data = (await _unitOfWork.SanPhamRepository.FindBy(x => x.TrangThai == 1
                    && (string.IsNullOrEmpty(name) || x.TenSP.ToLower().Contains(name.ToLower())))).ToList();
                    return data;
                }
                data = (await _unitOfWork.SanPhamRepository.FindBy(x => x.MaNCC == idth && x.MaDM == iddm && x.TrangThai == 1
                    && (string.IsNullOrEmpty(name) || x.TenSP.ToLower().Contains(name.ToLower())))).ToList();
                return data;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DanhMuc>> GetListDM()
        {
            return (await _unitOfWork.DanhMucRepository.FindBy(x => x.TrangThai != (int)StatusEnum.IsDelete)).ToList();
        }

        public async Task<List<NhaCungCap>> GetListNCC()
        {
            return (await _unitOfWork.NhaCungCapRepository.FindBy(x => x.TrangThai != (int)StatusEnum.IsDelete)).ToList();
        }

        public async Task<List<SanPham>> GetlistNew()
        {
            return (await _unitOfWork.SanPhamRepository.FindBy(x => x.TrangThai == (int)StatusEnum.Active)).OrderBy(x => x.NgayTao).Take(4).ToList();
        }

        public async Task<List<SanPham>> Get_list(string name, int status, int pageSize = 10, int pageNumber = 0)
        {
            try
            {
                if (status == (int)StatusEnum.All)
                {
                    var rs = (await _unitOfWork.SanPhamRepository.FindBy(x => (x.TrangThai != (int)StatusEnum.IsDelete)
                      && (string.IsNullOrEmpty(name) || x.TenSP.ToLower().Contains(name.ToLower())))).ToList();
                    foreach (var item in rs)
                    {
                        var ncc = await _unitOfWork.NhaCungCapRepository.Get(x => x.ID == item.MaNCC);
                        item.TenNCC = ncc.TenNCC;
                        var dm = await _unitOfWork.DanhMucRepository.Get(x => x.ID == item.MaDM);
                        item.TenDM = dm.TenDM;
                    }
                    return rs;
                }
                else
                {
                    var rs = (await _unitOfWork.SanPhamRepository.FindBy(x => (x.TrangThai != (int)StatusEnum.IsDelete)
                      && (string.IsNullOrEmpty(name) || x.TenSP.ToLower().Contains(name.ToLower())))).ToList();
                    foreach (var item in rs)
                    {
                        var ncc = await _unitOfWork.NhaCungCapRepository.Get(x => x.ID == item.MaNCC);
                        item.TenNCC = ncc.TenNCC;
                        var dm = await _unitOfWork.DanhMucRepository.Get(x => x.ID == item.MaDM);
                        item.TenDM = dm.TenDM;
                    }
                    return rs;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<SanPham>> LaySPHot()
        {
            try
            {
                List<SanPham> data = (await _unitOfWork.SanPhamRepository.FindBy(x => x.TrangThai == 1)).ToList();
                List<SanPham> rs = new List<SanPham>();
                foreach (var item in data)
                {
                    var donhang = (await _unitOfWork.CartItemRepository.FindBy(x => x.MaSP == item.ID)).ToList();
                    int sl = donhang.Sum(x => x.SoLuong);
                    item.sldaban = (int)sl;
                    rs.Add(item);
                }
                foreach (var item in rs)
                {
                    var ncc = await _unitOfWork.NhaCungCapRepository.Get(x => x.ID == item.MaNCC);
                    item.TenNCC = ncc.TenNCC;
                    var dm = await _unitOfWork.DanhMucRepository.Get(x => x.ID == item.MaDM);
                    item.TenDM = dm.TenDM;
                }
                List<SanPham> result = rs.OrderByDescending(x => x.sldaban).ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<SanPham>> LaySPSale()
        {
            return (await _unitOfWork.SanPhamRepository.FindBy(x => x.TrangThai == 1)).OrderByDescending(x => x.Sale).ToList();
        }

        public async Task<List<SanPham>> LaySPTU(int id)
        {
            var data = await _unitOfWork.SanPhamRepository.Get(x => x.ID == id);
            var list = (await _unitOfWork.SanPhamRepository.FindBy(x => x.MaNCC == data.MaNCC && x.TrangThai == 1 && x.ID != id)).ToList();
            return list;
        }

        public async Task Update(SanPham inputModel)
        {
            try
            {
                await _unitOfWork.SanPhamRepository.Update(inputModel);
                await _unitOfWork.SaveChange();
                //_unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, MessageConst.UPDATE_FAIL, null);
                //_unitOfWork.Rollback();
                throw new Exception(MessageConst.UPDATE_FAIL);
            }
        }
    }
}

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
    public interface INhaCungCapManager
    {
        Task Create(NhaCungCap inputModel);
        Task<NhaCungCap> FindById(int id);
        Task Update(NhaCungCap inputModel);

        Task<List<NhaCungCap>> Get_list(string name, int status, int pageSize = 10, int pageNumber = 0);
        Task Delete(int id);

        Task<NhaCungCap> FindByName(string tenxh);
    }
    public class NhaCungCapManager : INhaCungCapManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<NhaCungCap> _logger;

        public NhaCungCapManager(IUnitOfWork unitOfWork, ILogger<NhaCungCap> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Create(NhaCungCap inputModel)
        {
            try
            {
                await _unitOfWork.NhaCungCapRepository.Add(inputModel);
                await _unitOfWork.SaveChange();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task Delete(int id)
        {
            var data = await _unitOfWork.NhaCungCapRepository.Get(x => x.ID == id);
            await _unitOfWork.NhaCungCapRepository.Delete(data);
            await _unitOfWork.SaveChange();
        }

        public async Task<NhaCungCap> FindById(int id)
        {
            return await _unitOfWork.NhaCungCapRepository.Get(x => x.ID == id);
        }

        public async Task<NhaCungCap> FindByName(string tenxh)
        {
            return await _unitOfWork.NhaCungCapRepository.Get(x => x.TenNCC.ToLower().Contains(tenxh.ToLower()));
        }

        public async Task<List<NhaCungCap>> Get_list(string name, int status, int pageSize = 10, int pageNumber = 0)
        {
            try
            {
                if (status == (int)StatusEnum.All)
                {
                    var rs = (await _unitOfWork.NhaCungCapRepository.FindBy(x => (x.TrangThai != (int)StatusEnum.IsDelete)
                      && (string.IsNullOrEmpty(name) || x.TenNCC.ToLower().Contains(name.ToLower())))).ToList();
                    return rs;
                }
                else
                {
                    var rs = (await _unitOfWork.NhaCungCapRepository.FindBy(x => (x.TrangThai ==status)
                      && (string.IsNullOrEmpty(name) || x.TenNCC.ToLower().Contains(name.ToLower())))).ToList();
                    return rs;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task Update(NhaCungCap inputModel)
        {
            try
            {
                await _unitOfWork.NhaCungCapRepository.Update(inputModel);
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

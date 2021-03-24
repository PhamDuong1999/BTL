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
    public interface IDanhMucManager
    {
        Task Create(DanhMuc inputModel);
        Task<DanhMuc> FindById(int id);
        Task Update(DanhMuc inputModel);

        Task<List<DanhMuc>> Get_list(string name, int status, int pageSize=10, int pageNumber=1);
        Task Delete(int id);
    }
    public class DanhMucManager : IDanhMucManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DanhMuc> _logger;

        public DanhMucManager(IUnitOfWork unitOfWork, ILogger<DanhMuc> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Create(DanhMuc inputModel)
        {
            try
            {
                await _unitOfWork.DanhMucRepository.Add(inputModel);
                await _unitOfWork.SaveChange();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task Delete(int id)
        {
            var data = await _unitOfWork.DanhMucRepository.Get(x => x.ID == id);
            await _unitOfWork.DanhMucRepository.Delete(data);
            await _unitOfWork.SaveChange();
        }

        public async Task<DanhMuc> FindById(int id)
        {
            return await _unitOfWork.DanhMucRepository.Get(x => x.ID == id);
        }

        public async Task<List<DanhMuc>> Get_list(string name, int status, int pageSize = 10, int pageNumber = 1)
        {
            try
            {
                if (status == (int)StatusEnum.All)
                {
                    var rs = (await _unitOfWork.DanhMucRepository.FindBy(x => (x.TrangThai != (int)StatusEnum.IsDelete)
                      && (string.IsNullOrEmpty(name) || x.TenDM.ToLower().Contains(name.ToLower())))).ToList();
                    return rs;
                }
                else
                {
                    var rs = (await _unitOfWork.DanhMucRepository.FindBy(x => (x.TrangThai == status)
                      && (string.IsNullOrEmpty(name) || x.TenDM.ToLower().Contains(name.ToLower())))).ToList();
                    return rs;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Update(DanhMuc inputModel)
        {
            try
            {
                await _unitOfWork.DanhMucRepository.Update(inputModel);
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

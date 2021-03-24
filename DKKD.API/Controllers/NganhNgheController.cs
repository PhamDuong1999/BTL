using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DKKD.MANAGER;
using DKKD.MODELS;
using DKKD.UTILS;

namespace DKKD.API.Controllers
{
    [ApiController]
    [Route("api/nganh-nghe")]
    public class NganhNgheController : Controller
    {
        private readonly INganhNgheManager _manager;
        public NganhNgheController(INganhNgheManager NganhNgheManager)
        {
            this._manager = NganhNgheManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("get-list")]
        public async Task<IActionResult> GetList(string name = "", string code = "", int status = -1)
        {
            try
            {
               
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] NganhNghe inputModel)
        {
            try
            {
                var existTen = await _manager.FindByName(inputModel.TenNganhNghe);
                if (existTen != null)
                {
                    throw new Exception($"Tên { MessageConst.EXIST }");
                }
                var existMa = await _manager.FindByCode(inputModel.MaNganhNghe);
                if (existMa != null)
                {
                    throw new Exception($"Mã { MessageConst.EXIST }");
                }
                inputModel.NgayTao = DateTime.Now;
                await _manager.Create(inputModel);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] NganhNghe inputModel)
        {
            try
            {
                var data = await _manager.FindById(inputModel.Id);
                if (data == null)
                {
                    throw new Exception(MessageConst.DATA_NOT_FOUND);
                }
                inputModel.NgaySua = DateTime.Now;
                inputModel.NgayTao = data.NgayTao;
                await _manager.Update(inputModel);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] NganhNghe inputModel)
        {
            try
            {
                var data = await _manager.FindById(inputModel.Id);
                if (data == null)
                {
                    throw new Exception(MessageConst.DATA_NOT_FOUND);
                }
                data.NgaySua = DateTime.Now;
                data.TrangThai = 3;
                await _manager.Update(data);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("delete-all")]
        public async Task<IActionResult> Update_Delete([FromBody] string inputModels)
        {

            try
            {
                var list = inputModels.Split(',').Select(Int32.Parse).ToList();
                var data = await _manager.GetIdXoaNhieu(list);
                data.ForEach(c => c.TrangThai = (byte)StatusEnum.IsDelete);
                await _manager.XoaNhieu(data);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpGet("find-by-id")]
        public async Task<IActionResult> FindById(int id)
        {
            try
            {
                var data = await _manager.FindById(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}

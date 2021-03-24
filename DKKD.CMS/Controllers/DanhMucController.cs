using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using DKKD.MODELS;
using DKKD.UTILS;
using DKKD.MANAGER;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.IO;

namespace DKKD.CMS.Controllers
{
    [Route("danh-muc")]
    public class DanhMucController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IDanhMucManager DanhMuc;

        public DanhMucController(IConfiguration config, IHostingEnvironment hostingEnvironment, IDanhMucManager DanhMuc)
        {
            _config = config;
            _hostingEnvironment = hostingEnvironment;
            this.DanhMuc = DanhMuc;
        }

        [HttpGet("danh-sach")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("get-list")]
        public async Task<IActionResult> GetList(string name, int status)
        {
            var data = await DanhMuc.Get_list(name, status);
            return PartialView("GetList", data);
        }
        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            return PartialView("Create");
        }
        [HttpGet("update")]
        public async Task<IActionResult> Update(int id)
        {
            var data = await DanhMuc.FindById(id);
            return PartialView("Update",data);
        }
        [HttpPost("create-or-update")]
        public async Task<IActionResult> Create_Or_Update(DanhMuc inputModel)
        {
            try
            {
                if (inputModel.ID == 0)
                {
                    await DanhMuc.Create(inputModel);
                    return Json(new { Result = true, Message = "Thêm mới dữ liệu thành công" });
                }
                else
                {
                    var data = await DanhMuc.FindById(inputModel.ID);
                    if (data == null)
                    {
                        throw new Exception(MessageConst.DATA_NOT_FOUND);
                    }
                    data.TenDM = inputModel.TenDM;
                    data.TrangThai = inputModel.TrangThai;
                    await DanhMuc.Update(data);
                    return Json(new { Result = true, Message = "Cập nhật dữ liệu thành công" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = ex.Message });
            }
        }
        [HttpGet("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await DanhMuc.FindById(id);
            return PartialView(data);
        }
        [HttpPost("delete-data")]
        public async Task<IActionResult>DeleteData(DanhMuc inputModel)
        {
            var data = await DanhMuc.FindById(inputModel.ID);
            if (data == null)
            {
                throw new Exception(MessageConst.DATA_NOT_FOUND);
            }
            data.TrangThai = 3;
            await DanhMuc.Update(data);
            return Json(new { Result = true, Message = "Xóa dữ liệu thành công" });
        }
        [HttpGet("danh-sach-da-xoa")]
        public async Task<IActionResult> DanhSachDaXoa()
        {
            return View();
        }
        [HttpGet("get-list-delete")]
        public async Task<IActionResult> GetListDelete(string name, int status)
        {
            var data = await DanhMuc.Get_list(name, status);
            return PartialView(data);
        }
        [HttpPost("revert")]
        public async Task<IActionResult> revert(int id)
        {
            var data = await DanhMuc.FindById(id);
            if (data == null)
            {
                throw new Exception(MessageConst.DATA_NOT_FOUND);
            }
            data.TrangThai = 2;
            await DanhMuc.Update(data);
            return Json(new { Result = true, Message = "Khôi phục dữ liệu thành công" });
        }
    }
}

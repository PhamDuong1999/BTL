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
    [Route("nha-cung-cap")]
    public class NhaCungCapController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly INhaCungCapManager nhaCungCap;

        public NhaCungCapController(IConfiguration config, IHostingEnvironment hostingEnvironment, INhaCungCapManager nhaCungCap)
        {
            _config = config;
            _hostingEnvironment = hostingEnvironment;
            this.nhaCungCap = nhaCungCap;
        }

        [HttpGet("danh-sach")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("get-list")]
        public async Task<IActionResult> GetList(string name, int status)
        {
            var data = await nhaCungCap.Get_list(name, status);
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
            var data = await nhaCungCap.FindById(id);
            return PartialView("Update",data);
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create(IList<IFormFile> files, string NCCArr)
        {
            try
            {
                string host = this._hostingEnvironment.WebRootPath;
                NCCArr = NCCArr.Replace("\\\"", "");
                NhaCungCap NCCObj =
                    JsonConvert.DeserializeObject<NhaCungCap>(NCCArr);
                NhaCungCap inputModel = new NhaCungCap()
                {
                    Logo = "",
                    TenNCC = NCCObj.TenNCC,
                    TrangThai = NCCObj.TrangThai
                };
                foreach (IFormFile source in files)
                {
                    if (source.Length > 11534336)
                    {
                        throw new Exception("Dung lượng file quá 11 MB");
                    }
                    string filename = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.Trim('"');
                    filename = $"{Guid.NewGuid().ToString()}{Path.GetExtension(filename)}";
                    using (FileStream output = System.IO.File.Create(this.GetPathAndFilenameDVVT(filename)))
                        await source.CopyToAsync(output);
                    inputModel.Logo = $"{GetPathAndFilenameDVVT(filename)}";
                    inputModel.Logo = inputModel.Logo.Replace(host, "");
                }
                await nhaCungCap.Create(inputModel);
                return Json(new { Result = true, Message = "Thêm mới dữ liệu thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = ex.Message });
            }
        }

        private string GetPathAndFilenameDVVT(string filename)
        {
            string currentDate = DateTime.Now.ToString("hhmmssddMMyyyy");
            var folder = $"{this._hostingEnvironment.WebRootPath}/{_config["NCC"].ToString()}/{currentDate}";
            var filePath = $"{folder}/{filename}";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            return filePath;
        }
        [HttpPost("update")]
        public async Task<IActionResult> Update(IList<IFormFile> files, string NCCArr)
        {
            try
            {
                string host = this._hostingEnvironment.WebRootPath;
                NCCArr = NCCArr.Replace("\\\"", "");
                NhaCungCap NCCObj =
                    JsonConvert.DeserializeObject<NhaCungCap>(NCCArr);
                NhaCungCap inputModel = new NhaCungCap()
                {
                    ID = NCCObj.ID,
                    TenNCC=NCCObj.TenNCC,
                    Logo = "",
                    TrangThai = NCCObj.TrangThai

                };
                if (files.Count() > 0)
                {
                    foreach (IFormFile source in files)
                    {
                        if (source.Length > 11534336)
                        {
                            throw new Exception("Dung lượng file quá 11 MB");
                        }
                        string filename = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.Trim('"');
                        filename = $"{Guid.NewGuid().ToString()}{Path.GetExtension(filename)}";
                        using (FileStream output = System.IO.File.Create(this.GetPathAndFilenameDVVT(filename)))
                            await source.CopyToAsync(output);
                        inputModel.Logo = $"{GetPathAndFilenameDVVT(filename)}";
                        inputModel.Logo = inputModel.Logo.Replace(host, "");
                    }
                }
                if (files.Count() == 0)
                {
                    inputModel.Logo = NCCObj.Logo;
                }
                await nhaCungCap.Update(inputModel);
                return Json(new { Result = true, Message = "Cập nhật dữ liệu thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = ex.Message });
            }
        }
        [HttpGet("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await nhaCungCap.FindById(id);
            return PartialView(data);
        }
        [HttpPost("delete-data")]
        public async Task<IActionResult>DeleteData(NhaCungCap inputModel)
        {
            var data = await nhaCungCap.FindById(inputModel.ID);
            if (data == null)
            {
                throw new Exception(MessageConst.DATA_NOT_FOUND);
            }
            data.TrangThai = 3;
            await nhaCungCap.Update(data);
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
            var data = await nhaCungCap.Get_list(name, status);
            return PartialView(data);
        }
        [HttpPost("revert")]
        public async Task<IActionResult> revert(int id)
        {
            var data = await nhaCungCap.FindById(id);
            if (data == null)
            {
                throw new Exception(MessageConst.DATA_NOT_FOUND);
            }
            data.TrangThai = 2;
            await nhaCungCap.Update(data);
            return Json(new { Result = true, Message = "Khôi phục dữ liệu thành công" });
        }
    }
}

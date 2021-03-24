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
    [Route("san-pham")]
    public class SanPhamController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ISanPhamManager SanPham;

        public SanPhamController(IConfiguration config, IHostingEnvironment hostingEnvironment, ISanPhamManager SanPham)
        {
            _config = config;
            _hostingEnvironment = hostingEnvironment;
            this.SanPham = SanPham;
        }

        [HttpGet("danh-sach")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("get-list")]
        public async Task<IActionResult> GetList(string name, int status)
        {
            var data = await SanPham.Get_list(name, status);
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
            var data = await SanPham.FindById(id);
            return PartialView("Update",data);
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create(IList<IFormFile> files, string NCCArr)
        {
            try
            {
                string host = this._hostingEnvironment.WebRootPath;
                NCCArr = NCCArr.Replace("\\\"", "");
                SanPham NCCObj =
                    JsonConvert.DeserializeObject<SanPham>(NCCArr);
                SanPham inputModel = new SanPham()
                {
                    Avatar = "",
                    TenSP = NCCObj.TenSP,
                    GioiThieu = NCCObj.GioiThieu,
                    MoTa = NCCObj.MoTa,
                    SoLuong = NCCObj.SoLuong,
                    DonGia = NCCObj.DonGia,
                    Sale = NCCObj.Sale,
                    NgayTao = DateTime.Now,
                    MaNCC=NCCObj.MaNCC,
                    MaDM=NCCObj.MaDM,
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
                    inputModel.Avatar = $"{GetPathAndFilenameDVVT(filename)}";
                    inputModel.Avatar = inputModel.Avatar.Replace(host, "");
                }
                await SanPham.Create(inputModel);
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
            var folder = $"{this._hostingEnvironment.WebRootPath}/{_config["SP"].ToString()}/{currentDate}";
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
                SanPham NCCObj =
                    JsonConvert.DeserializeObject<SanPham>(NCCArr);
                SanPham inputModel = new SanPham()
                {
                    ID = NCCObj.ID,
                    Avatar = "",
                    TenSP = NCCObj.TenSP,
                    GioiThieu = NCCObj.GioiThieu,
                    MoTa = NCCObj.MoTa,
                    SoLuong = NCCObj.SoLuong,
                    DonGia = NCCObj.DonGia,
                    Sale = NCCObj.Sale,
                    NgayTao = DateTime.Now,
                    MaNCC = NCCObj.MaNCC,
                    MaDM = NCCObj.MaDM,
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
                        inputModel.Avatar = $"{GetPathAndFilenameDVVT(filename)}";
                        inputModel.Avatar = inputModel.Avatar.Replace(host, "");
                    }
                }
                if (files.Count() == 0)
                {
                    inputModel.Avatar = NCCObj.Avatar;
                }
                await SanPham.Update(inputModel);
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
            var data = await SanPham.FindById(id);
            return PartialView(data);
        }
        [HttpPost("delete-data")]
        public async Task<IActionResult>DeleteData(SanPham inputModel)
        {
            var data = await SanPham.FindById(inputModel.ID);
            if (data == null)
            {
                throw new Exception(MessageConst.DATA_NOT_FOUND);
            }
            data.TrangThai = 3;
            await SanPham.Update(data);
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
            var data = await SanPham.Get_list(name, status);
            return PartialView(data);
        }
        [HttpPost("revert")]
        public async Task<IActionResult> revert(int id)
        {
            var data = await SanPham.FindById(id);
            if (data == null)
            {
                throw new Exception(MessageConst.DATA_NOT_FOUND);
            }
            data.TrangThai = 2;
            await SanPham.Update(data);
            return Json(new { Result = true, Message = "Khôi phục dữ liệu thành công" });
        }
        [HttpGet("get-ncc")]
        public async Task<IActionResult> GetNCC()
        {
            var data = await SanPham.GetListNCC();
            return Json(new { Data = data });
        }
        [HttpGet("get-dm")]
        public async Task<IActionResult> GetDM()
        {
            var data = await SanPham.GetListDM();
            return Json(new { Data = data });
        }
        [HttpGet("thong-ke")]
        public async Task<IActionResult> ThongKe()
        {
            var data = await SanPham.LaySPHot();
            return View(data);
        }
    }
}

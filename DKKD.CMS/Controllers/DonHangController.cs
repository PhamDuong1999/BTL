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
    [Route("don-hang")]
    public class DonHangController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IDonHangManager DonHang;

        public DonHangController(IConfiguration config, IHostingEnvironment hostingEnvironment, IDonHangManager DonHang)
        {
            _config = config;
            _hostingEnvironment = hostingEnvironment;
            this.DonHang = DonHang;
        }

        [HttpGet("danh-sach")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("get-list")]
        public async Task<IActionResult> GetList(string name, int status)
        {
            var data = await DonHang.GetList(name, status);
            return PartialView("GetList", data);
        }
        [HttpGet("get-list-ctdh")]
        public async Task<IActionResult> GetListCTDH(int id)
        {
            var data = await DonHang.GetListCTDH(id);
            return PartialView(data);
        }
        [HttpPost("change-status")]
        public async Task<IActionResult> Change(int status, int id)
        {
            try
            {
                var data = await DonHang.FindById(id);
                if (data == null)
                {
                    throw new Exception(MessageConst.DATA_NOT_FOUND);
                }
                data.TrangThai = status;
                await DonHang.Update(data);
                return Json(new { Result = true, Message = "Cập nhật thành công !" });

            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = ex.Message });
            }
        }
    }
}

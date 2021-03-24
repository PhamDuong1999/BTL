using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using DKKD.MODELS;
using DKKD.UTILS;
using X.PagedList.Mvc.Core;
using X.PagedList;
using DKKD.MANAGER;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.IO;
using Microsoft.Extensions.Logging;

namespace DKKD.WEBSITE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IDanhMucManager DanhMuc;
        private readonly INhaCungCapManager NhaCungCap;
        private readonly ISanPhamManager SanPham;

        public HomeController(ILogger<HomeController> logger, IConfiguration config, IHostingEnvironment hostingEnvironment, IDanhMucManager danhMuc, INhaCungCapManager nhaCungCap, ISanPhamManager sanPham)
        {
            _logger = logger;
            _config = config;
            _hostingEnvironment = hostingEnvironment;
            DanhMuc = danhMuc;
            NhaCungCap = nhaCungCap;
            SanPham = sanPham;
        }

        public async Task<IActionResult> Index()
        {
            var ListThuongHieu = await NhaCungCap.Get_list("", 1);
            var lsSPNew = await SanPham.GetlistNew();
            var lsSPSale = await SanPham.LaySPSale();
            var data= await SanPham.LaySPHot();
            var lsSPHot = data.Skip(0).Take(8).ToList();
            ViewData["lsth"] = ListThuongHieu;
            ViewData["lssphot"] = lsSPHot;
            ViewData["lsspnew"] = lsSPNew;
            ViewData["lsspsale"] = lsSPSale;
            return View();
        }
        [HttpGet("menus")]
        public async Task<IActionResult> Header()
        {
            var ListThuongHieu = await NhaCungCap.Get_list("", 1);
            var ListDanhMuc = await DanhMuc.Get_list("", 1);
            ViewData["lsth"] = ListThuongHieu;
            ViewData["lsdm"] = ListDanhMuc;
            return PartialView();
        }
        [HttpGet("danh-sach-san-pham")]
        public async Task<IActionResult> DanhSachSanPham(string name = "", int idth = -1, int iddm = -1, int page = 1,int pageSize=16)
        {
            if(name == "" || name == null)
            {
                name = null;
            }
            ViewData["name"] = name;
            if (idth != -1)
            {
                var th = await NhaCungCap.FindById(idth);
                ViewData["tenth"] = th.TenNCC;
            }
            else
            {
                ViewData["tenth"] = null;
            }
            if (iddm != -1)
            {
                var dm = await DanhMuc.FindById(iddm);
                ViewData["tendm"] = dm.TenDM;
            }
            else
            {
                ViewData["tendm"] = null;
            }
            var data = await SanPham.Getlist(name, idth, iddm);
            ViewBag.page = page;
            ViewBag.totalPage = Math.Ceiling((double)data.Count() / pageSize);
            ViewBag.pageSize = pageSize;
            ViewBag.name = name;
            return View(data.ToPagedList(page, pageSize));
        }
        [HttpGet("chi-tiet-san-pham")]
        public async Task<IActionResult> ChiTietSanPham(int id)
        {
            var data = await SanPham.FindById(id);
            var lssp = await SanPham.LaySPTU(id);
            ViewData["lssp"] = lssp;
            return View(data);
        }

    }
}

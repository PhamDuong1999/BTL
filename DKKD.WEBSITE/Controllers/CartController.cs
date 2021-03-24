using DKKD.MODELS;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DKKD.UTILS;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using DKKD.MANAGER;

namespace DKKD.WEBSITE.Controllers
{
    public class CartController : Controller
    {
        private readonly ShopContext shopContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private readonly IDonHangManager DonHang;

        public CartController(ShopContext shopContext, IHttpContextAccessor httpContextAccessor, IDonHangManager donHang)
        {
            this.shopContext = shopContext;
            _httpContextAccessor = httpContextAccessor;
            DonHang = donHang;
        }

        public List<CartItem> Carts
        {
            get
            {
                List<CartItem> data = UTILS.SessionExtensions.Get<List<CartItem>>(_session, UTILS.SessionExtensions.SesscionCart);
                if (data == null)
                {
                    data = new List<CartItem>();
                }
                return data;
            }
        }
        public async Task<IActionResult> Index()
        {
            return View(Carts);
        }
        public IActionResult Total()
        {
            List<CartItem> cart = UTILS.SessionExtensions.Get<List<CartItem>>(_session, UTILS.SessionExtensions.SesscionCart);
            if (cart == null)
            {
                return Json(new { data = 0 });
            }
            return Json(new { data = cart.Count});
        }
        public IActionResult Remove(int id)
        {
            List<CartItem> cart = UTILS.SessionExtensions.Get<List<CartItem>>(_session, UTILS.SessionExtensions.SesscionCart);
            CartItem itemRemove = cart.SingleOrDefault(x => x.MaSP == id);
            int index = cart.IndexOf(itemRemove);
            cart.RemoveAt(index);
            UTILS.SessionExtensions.Set(_session, UTILS.SessionExtensions.SesscionCart, cart);
            return RedirectToAction("Index");
        }
        public IActionResult Update(int id, int soluong)
        {
            try
            {
                List<CartItem> cart = UTILS.SessionExtensions.Get<List<CartItem>>(_session, UTILS.SessionExtensions.SesscionCart);
                CartItem item = cart.SingleOrDefault(x => x.MaSP == id);
                item.SoLuong = soluong;
                UTILS.SessionExtensions.Set(_session, UTILS.SessionExtensions.SesscionCart, cart);
                return Json(new { Result = true, Message = "Cập nhật thành công." });
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = ex.Message });
            }
        }
        public IActionResult AddToCart(int id, int sl)
        {
            var myCart = Carts;
            var item = myCart.SingleOrDefault(p => p.MaSP == id);
            if (item == null)
            {
                var sanpham = shopContext.SanPham.SingleOrDefault(p => p.ID == id);
                item = new CartItem
                {
                    MaSP = id,
                    TenSP = sanpham.TenSP,
                    DonGia = sanpham.DonGia.Value - (sanpham.DonGia.Value * sanpham.Sale.Value / 100),
                    SoLuong = sl,
                    Avatar = sanpham.Avatar
                };
                myCart.Add(item);
            }
            else
            {
                item.SoLuong += sl;
            }
            UTILS.SessionExtensions.Set(_session, UTILS.SessionExtensions.SesscionCart, myCart);
            return Json(new { Soluong = Carts.Sum(c => c.SoLuong) });
            //return RedirectToAction("Index");
        }
        //[HttpPost("dat-hang")]
        public async Task<IActionResult> Create(DonHang inputModel)
        {
            try
            {
                List<CartItem> cart = UTILS.SessionExtensions.Get<List<CartItem>>(_session, UTILS.SessionExtensions.SesscionCart);
                inputModel.NgayTao = DateTime.Now;
                inputModel.TrangThai = 2;
                inputModel.ListCTDH = cart;
                await DonHang.Create(inputModel);
                UTILS.SessionExtensions.Set<List<CartItem>>(_session, UTILS.SessionExtensions.SesscionCart,null);
                return Json(new { result = true, message = "Đặt hàng thành công." });
            }
            catch(Exception ex)
            {
                return Json(new { result = false, message = "Đặt hàng thất bại." });
            }
        }
    }
}

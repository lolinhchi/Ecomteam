using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TeamBee.Connect_Data;
using TeamBee.Helper;
using TeamBee.Models;
using TeamBee.ViewModels;

namespace TeamBee.Controllers
{
    public class HomeController : Controller
    {

        private readonly TeamBeeDbContext _context;

        public HomeController(TeamBeeDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ItemProductsViewModel Product = new ItemProductsViewModel(_context);

            // Top 3 sản phẩm giảm giá nhiều nhất
            ViewBag.TopSale = Product.GetProductsDescending("Sale",3);

            // Top 3 sản phẩm đánh giá tốt  nhất
            ViewBag.TopRated = Product.GetProductsDescending("Stars", 3);

            // Top 3 sản phẩm mới nhất
            ViewBag.TopNew = Product.GetProductsDescending("New", 3);

            // Top 8 sản phẩm giảm giá nhiều nhất
            ViewBag.TopSale8 = Product.GetProductsDescending("Sale", 8);

            // Top 8 sản phẩm đánh giá tốt  nhất
            ViewBag.TopBuy8 = Product.GetProductsDescending("Orders", 8);

            // Top 8 sản phẩm mới nhất
            ViewBag.TopNew8 = Product.GetProductsDescending("New", 8);
            // lấy Category
            ViewBag.Category = _context.Categorys.ToList();
            // lấy tất sp
            ViewBag.AllProduct = Product.GetProducts("all");
            // lấy quảng cáo
            ViewBag.Trai = _context.QuangCaos.Where(p => p.Status == 0).SingleOrDefault();
            ViewBag.Phai = _context.QuangCaos.Where(p => p.Status == 1).SingleOrDefault();
            // map giỏ hàng

            ViewBag.Cart = GetGioHang;
            ViewBag.Total = GetGioHang.Sum(p => p.SoLuong * p.Gia);
            ViewBag.Soluong = GetGioHang.Sum(p => p.SoLuong);

            return View();
        }

        public IActionResult Blog()
        {
            ItemProductsViewModel itemProductsViewModel = new ItemProductsViewModel(_context);
            ViewBag.AllProduct = itemProductsViewModel.GetProducts("all");

            // lấy Category
            ViewBag.Category = _context.Categorys.ToList();
            return View();
        }


        public List<CartItem> GetGioHang
        {
            get
            {
                List<CartItem> myCart = SessionHelper.Get<List<CartItem>>(HttpContext.Session, "cart");
                if (myCart == default(List<CartItem>))
                {
                    myCart = new List<CartItem>();
                }
                return myCart;
            }
        }

    }
}

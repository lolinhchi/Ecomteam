using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using TeamBee.Connect_Data;
using TeamBee.Helper;
using TeamBee.Models;
using TeamBee.ViewModels;
using X.PagedList;

namespace TeamBee.Controllers
{
    public class ProductController : Controller
    {
        private readonly TeamBeeDbContext _context;

        public ProductController(TeamBeeDbContext context)
        {
            _context = context;
        }
        [Route("~/{order_by}")]
        public IActionResult Index(string order_by, int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 9;
            ItemProductsViewModel itemProductsViewModel = new ItemProductsViewModel(_context);
            // Lấy tất cả sản phẩm
            if (order_by == "tat-ca-san-pham")
            {


                var query = itemProductsViewModel.GetQueryProducts("all").ToPagedList(pageNumber, pageSize);

                ViewBag.All = query;
            }
            else
            {


                //ViewBag.All = itemProductsViewModel.GetQueryProducts(order_by);
                var query = itemProductsViewModel.GetQueryProducts(order_by).ToPagedList(pageNumber, pageSize);

                ViewBag.All = query;
            }
            // Top 3 sản phẩm mới nhất
            ViewBag.TopNew = itemProductsViewModel.GetProductsDescending("New", 3);
            // lấy Category
            ViewBag.Category = _context.Categorys.ToList();


            ViewBag.AllProduct = itemProductsViewModel.GetProducts("all");

            // map cart
            ViewBag.Cart = GetGioHang;
            ViewBag.Total = GetGioHang.Sum(p => p.SoLuong * p.Gia);
            ViewBag.Soluong = GetGioHang.Sum(p => p.SoLuong);
            ViewBag.OrderBy = order_by;
            return View(ViewBag.All);
        }


        [Route("~/{urlcate}/{urlpro}")]
        public async Task<IActionResult> SingleProduct(string urlcate, string urlpro)
        {
            CommentViewModels commentViewModels = new CommentViewModels(_context);

            ItemProductsViewModel itemProductsViewModel = new ItemProductsViewModel(_context);
            // map cart
            ViewBag.Cart = GetGioHang;
            ViewBag.Total = GetGioHang.Sum(p => p.SoLuong * p.Gia);
            ViewBag.Soluong = GetGioHang.Sum(p => p.SoLuong);

            // lấy cate
            var Cate = await (from cate in _context.Categorys where cate.URL.Trim() == urlcate.Trim() select cate.Id).SingleOrDefaultAsync();
            if (Cate > 0)
            {

                // lấy sp
                ViewBag.Result = await (from p in _context.Products where p.Category_Id == Cate && p.URL.Trim() == urlpro select p).SingleOrDefaultAsync();


                Product pro = await (from p in _context.Products where p.URL.Trim() == urlpro.Trim() select p).SingleOrDefaultAsync();
                // tăng số lương khi xem
                pro.Views += 1;
                _context.Products.Update(pro);
                _context.SaveChanges();

                ViewBag.Comment = commentViewModels.GetComments(pro.Id);
                // lấy sp liên quan

                ViewBag.sanphamlienquan = itemProductsViewModel.GetSanPhamLienQuan(pro.Category_Id, pro.Id);

                ViewBag.AllProduct = itemProductsViewModel.GetProducts("all");
                // lấy Category
                ViewBag.Category = _context.Categorys.ToList();
            }

            return View(await (from p in _context.Products where p.Category_Id == Cate && p.URL.Trim() == urlpro select p).SingleOrDefaultAsync());
        }
        [HttpGet]
        public IActionResult SearchHeader(string key, int? page, string value = null)
        {
            ItemProductsViewModel itemProductsViewModel = new ItemProductsViewModel(_context);
            // Top 3 sản phẩm mới nhất
            ViewBag.TopNew = itemProductsViewModel.GetProductsDescending("New", 3);
            // lấy Category
            ViewBag.Category = _context.Categorys.ToList();




            ViewBag.AllProduct = itemProductsViewModel.GetProducts("all");
            // map cart
            ViewBag.Cart = GetGioHang;
            ViewBag.Total = GetGioHang.Sum(p => p.SoLuong * p.Gia);
            ViewBag.Soluong = GetGioHang.Sum(p => p.SoLuong);
            int pageNumber = page ?? 1;
            int pageSize = 9;
            if (value != null)
            {
                ViewBag.All = itemProductsViewModel.SearchProductSort(key, value).ToPagedList(pageNumber, pageSize);
            }
            else
            {
                ViewBag.All = itemProductsViewModel.SearchProduct(key).ToPagedList(pageNumber, pageSize);
            }
            ViewBag.Key = key;
            ViewBag.Value = value;
            if (value != null)
            {
                return PartialView("_resultSeach");
            }
            else
            {
                return View();
            }
        }


        public IActionResult SortBy(string Category, string value, int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 9;
            ItemProductsViewModel itemProductsViewModel = new ItemProductsViewModel(_context);
            var query = itemProductsViewModel.Sort(Category, value);
            ViewBag.All = query.ToPagedList(pageNumber, pageSize);
            ViewBag.Category = Category;
            ViewBag.Value = value;
            return PartialView("_resultSort", ViewBag.All);
        }

        public IActionResult Search(string key)
        {
            ItemProductsViewModel itemProductsViewModel = new ItemProductsViewModel(_context);
            ViewBag.All = itemProductsViewModel.SearchProduct(key);

            return PartialView("_resultSeach", ViewBag.all);
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
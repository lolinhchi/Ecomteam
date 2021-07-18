using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeamBee.Connect_Data;
using TeamBee.Models;
using TeamBee.ViewModels;

namespace TeamBee.Controllers
{
    public class QuangCaoController : Controller
    {
        private readonly TeamBeeDbContext _context;
        private readonly IHostingEnvironment _hostingEnviroment;
        public QuangCaoController(TeamBeeDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnviroment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            ViewBag.Result = _context.QuangCaos.ToList();
            return View();
        }
        public IActionResult SuaQuangCao (int id)
        {
            return View(_context.QuangCaos.Where(p => p.Id == id).SingleOrDefault());
        }
        [HttpPost]
        public IActionResult SuaQuangCao(QuangCaoViewModel quangCao )
        {
           

            if (HttpContext.Session.GetInt32("admin") > 0)
            {
                QuangCao EditQuangCao = _context.QuangCaos.Where(p => p.Id == quangCao.Id).SingleOrDefault();
                //Product data = vm.Product;
               // Product productedit = _context.Products.Where(p => p.Id == quangCao.Id).FirstOrDefault();


                // Tải hình ảnh sản phẩm lên thư mục wwwroot/uploads
                if (quangCao.Anh != null)
                {
                    // Xóa ảnh cũ
                    string old_image = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imgVETA", EditQuangCao.Anh);
                    if (System.IO.File.Exists(old_image))
                    {
                        System.IO.File.Delete(old_image);
                    }
                    EditQuangCao.Anh = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                    string uniqueName = EditQuangCao.Anh; // Tạo tên hình ảnh theo chuỗi ngày tháng lúc đăng ảnh
                    string newpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imgVETA"); // Trỏ đường dẫn đến thư mục wwwroot/uploads
                    newpath = Path.Combine(newpath, uniqueName); // Trỏ đường dẫn đến tên hình ảnh
                    newpath = newpath + Path.GetExtension(quangCao.Anh.FileName); // Gắn đuôi (loại file) cho hình
                    quangCao.Anh.CopyTo(new FileStream(newpath, FileMode.Create)); // Copy hình từ nguồn sang wwwroot/uploads
                    EditQuangCao.Anh += Path.GetExtension(quangCao.Anh.FileName);
                }

                EditQuangCao.Ten = quangCao.Ten;
                EditQuangCao.PhanTram = quangCao.PhanTram;
                EditQuangCao.LienKet = quangCao.LienKet;
                _context.QuangCaos.Update(EditQuangCao);
                _context.SaveChanges();
                return Redirect("~/QuangCao/index");
            }
            else
            {
                return RedirectToAction("login", "admin");
            }
        }
    }
}
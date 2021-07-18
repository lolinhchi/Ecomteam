using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamBee.Connect_Data;
using TeamBee.Models;
using TeamBee.ViewModels;

namespace TeamBee.Controllers
{
    public class BlogController : Controller
    {
        private TeamBeeDbContext _context;
     

        public BlogController(TeamBeeDbContext context)
        {
            _context = context;
        }
        [Route("~/Blog/bloglist/{category}")]
        public async Task<IActionResult> bloglist(string category)
        {
            ItemProductsViewModel itemProductsViewModel = new ItemProductsViewModel(_context);
            ViewBag.AllProduct = itemProductsViewModel.GetProducts("all");

            // lấy Category
            ViewBag.Category = _context.Categorys.ToList();

            BlogItemDetailViewModel blogItemDetailViewModel = new BlogItemDetailViewModel(_context);
            ViewBag.allblog =  blogItemDetailViewModel.getAllBlogItem(category);

            return View();
        }
        [Route("~/Blog/{url_CateBlog}/{url_BlogDetail}")]
        public async Task<IActionResult> blogdetail(string url_CateBlog, string url_BlogDetail)
        {
           
          
            BlogItemDetailViewModel blogItemDetailViewModel = new BlogItemDetailViewModel(_context);
            //lấy blogdetail
            ViewBag.Detail = _context.BlogItems.ToList();
            ViewBag.GetNewBlog = blogItemDetailViewModel.GetBlogDescending("New",3);

            BlogItemViewModel blogItemViewModel = new BlogItemViewModel();

            var Cate = await(from cate in _context.Blogs where cate.URL.Trim() == url_CateBlog.Trim() select cate.Id).SingleOrDefaultAsync();
            if (Cate > 0)
            {

                // lấy sp
                ViewBag.Result_bl = await(from b_detail in _context.BlogItems where b_detail.Category_BlogId == Cate && b_detail.URL.Trim() == url_BlogDetail select b_detail).SingleOrDefaultAsync();


                BlogItem blogItems = await(from p in _context.BlogItems where p.URL.Trim() == url_BlogDetail.Trim() select p).SingleOrDefaultAsync();


                // lấy sp liên quan

               //ViewBag.bloglienquan = blogItemDetailViewModel.GetBlogLienQuan(blogItems.Category_BlogId, blogItems.Id);

               
                // lấy Category
                ViewBag.Category = _context.Blogs.ToList();
            }

            return View(await(from p in _context.BlogItems where p.Category_BlogId == Cate && p.URL.Trim() == url_BlogDetail select p).SingleOrDefaultAsync());
        }
    }
}
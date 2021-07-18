using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeamBee.Connect_Data;
using TeamBee.Models;
using TeamBee.ViewModels;

namespace TeamBee.Controllers
{
    public class CommentController : Controller
    {
        private readonly TeamBeeDbContext _context;

        public CommentController(TeamBeeDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddReview( int idProduct, int stars, string content)
        {
            //test
            CommentViewModels commentViewModels = new CommentViewModels(_context);

            Comment comment = new Comment
            {
                User_Id = (int)HttpContext.Session.GetInt32("id"),
                Product_Id = idProduct,
                Stars = stars,
                Content = content,
                DateCreate = DateTime.Now,
                DateModify = DateTime.Now
            };

            _context.Comments.Add(comment);
            _context.SaveChanges();
            var tongsao = _context.Comments.Where(p => p.Product_Id == idProduct).Sum(p => p.Stars);
            var sl = _context.Comments.Where(p => p.Product_Id == idProduct).Count();
            var pro = _context.Products.Where(p => p.Id == comment.Product_Id).SingleOrDefault();
            pro.Stars = Math.Round(tongsao / sl);
            _context.Products.Update(pro);
            _context.SaveChanges();
            ViewBag.Comment = commentViewModels.GetComments(idProduct);
            return PartialView("_commentresult", ViewBag.Comment);
        }
    }
}
 
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TeamBee.Connect_Data;
using TeamBee.Models;

namespace TeamBee.ViewModels
{
    public class BlogItemDetailViewModel
    {
        private readonly TeamBeeDbContext _context;
        private readonly IHostingEnvironment _hostingEnviroment;
        public BlogItemDetailViewModel(TeamBeeDbContext context)
        {
            _context = context;
            // _hostingEnviroment = hostingEnvironment;
        }
        public int Id { get; set; }


        [ForeignKey("Blog_Category")]
        public int Category_BlogId { get; set; }
        public Blog Blog_Category { get; set; }


        public string Title { get; set; }


        public string Content { get; set; }





        [Display(Name = "Ảnh đại diện")]
        public string Thumbnail { get; set; } = "#";



        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày tạo")]
        public DateTime DateCreate { get; set; }

        public String BlogUrl { get; set; }
        public String CateUrl { get; set; }
        public BlogItemDetailViewModel() { }
        public List<BlogItemDetailViewModel> getAllBlogItem(string category)
        {
            List<BlogItemDetailViewModel> blogitem = new List<BlogItemDetailViewModel>();

            if (category == "all")
            {
                var query = (from b in _context.Blogs
                             join bli in _context.BlogItems
                             on b.Id equals bli.Category_BlogId
                             orderby bli.DateCreate descending

                             select new BlogItemDetailViewModel
                             {
                                 Id = bli.Id,
                                 Title = bli.Title,
                                 DateCreate = bli.DateCreate,
                                 Thumbnail = bli.Thumbnail,

                                 BlogUrl = bli.URL,
                                 CateUrl = b.URL,
                                 Content = bli.Content,

                             });
                blogitem = query.ToList();
            }
            else
            {
                var query = (from b in _context.Blogs
                             join bli in _context.BlogItems
                             on b.Id equals bli.Category_BlogId
                             where b.URL.Trim() == category.Trim()
                             orderby bli.DateCreate descending

                             select new BlogItemDetailViewModel
                             {
                                 Id = bli.Id,
                                 Title = bli.Title,
                                 DateCreate=bli.DateCreate,
                                 Thumbnail = bli.Thumbnail,

                                 BlogUrl = bli.URL,
                                 CateUrl = b.URL,
                                 Content = bli.Content,

                             });
                blogitem = query.ToList();
            }

            return blogitem;
        }
        public List<BlogItemDetailViewModel> GetBlogLienQuan(int idcate, int iditem)
        {
            List<BlogItemDetailViewModel> blogitem = new List<BlogItemDetailViewModel>();


            var query = (from b in _context.BlogItems
                         join c in _context.Blogs
                         on b.Category_BlogId equals c.Id
                         where b.Id != iditem &&
                         b.Category_BlogId == idcate

                         select new BlogItemDetailViewModel
                         {
                             DateCreate = b.DateCreate,
                             Id = b.Id,
                             Title = b.Title,
                             Content = b.Content,
                             Thumbnail = b.Thumbnail,
                             BlogUrl = b.URL,
                             CateUrl = c.URL,
                         });
            blogitem = query.Take(4).ToList();
            return blogitem;
        }


        // blog top
        public List<BlogItemDetailViewModel> GetBlogDescending(string name, int sl)
        {
            List<BlogItemDetailViewModel> Blog_item = new List<BlogItemDetailViewModel>();

            if (name == "New")
            {
                var query = (from b in _context.BlogItems
                             join c in _context.Blogs
                             on b.Category_BlogId equals c.Id
                             orderby b.Id descending

                             select new BlogItemDetailViewModel
                             {
                                 Id = b.Id,
                                 Title = b.Title,
                                 Content = b.Content,
                                 Thumbnail = b.Thumbnail,
                                 BlogUrl = b.URL,
                                 CateUrl = c.URL,
                                 DateCreate = b.DateCreate
                             });
                Blog_item = query.Take(sl).ToList();
            }

            return Blog_item;
        }

    }





}

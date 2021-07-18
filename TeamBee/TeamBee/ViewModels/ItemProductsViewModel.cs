using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TeamBee.Connect_Data;

namespace TeamBee.ViewModels
{
    public class ItemProductsViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; }

        [Display(Name = "Mô tả sản phẩm")]
        public string Description { get; set; } = "Sản phẩm này hiện chưa có mô tả";

        [Display(Name = "Đơn giá")]
        public int Price { get; set; } = 0;

        [Range(0.0, 100.0)]
        [Display(Name = "Phần trăm khuyến mãi")]
        public double SalePrice { get; set; } = 0.0;

        [Display(Name = "Ảnh đại diện")]
        public string Thumbnail { get; set; } = "#";
        //11
        [Range(0.0, 5.0)]
        [Display(Name = "Đánh giá trung bình")]
        public double Stars { get; set; } = 5.0;

        [Display(Name = "Lượt xem")]
        public int Views { get; set; } = 0;

        [Display(Name = "Lượt đặt hàng")]
        public int Orders { get; set; } = 0;
        public string CateUrl { get; set; }
        public string NameUrl { get; set; }
        public int Status { get; set; }
        public ItemProductsViewModel() { }
        private readonly TeamBeeDbContext _context;

        public ItemProductsViewModel(TeamBeeDbContext context)
        {
            _context = context;
        }

        public List<ItemProductsViewModel> SearchProductSort(string key, string value)
        {
            List<ItemProductsViewModel> product = new List<ItemProductsViewModel>();

            if (value == "Name")
            {
                var query = (from p in _context.Products
                             join c in _context.Categorys
                             on p.Category_Id equals c.Id
                             where p.Name.ToUrlFriendly().Contains(key.ToUrlFriendly())
                             orderby p.Name descending

                             select new ItemProductsViewModel
                             {
                                 Id = p.Id,
                                 Name = p.Name,
                                 Price = p.Price,
                                 SalePrice = p.SalePrice,
                                 Thumbnail = p.Thumbnail,
                                 Stars = p.Stars,
                                 Views = p.Views,
                                 Orders = p.Orders,
                                 NameUrl = p.URL,
                                 CateUrl = c.URL,
                                 Description = p.Description,
                                 Status = p.Status
                             }).Where(p => p.Status == 1);
                product = query.ToList();
            }
            else if (value == "Price")
            {
                var query = (from p in _context.Products
                             join c in _context.Categorys
                             on p.Category_Id equals c.Id
                             where p.Name.ToUrlFriendly().Contains(key.ToUrlFriendly())
                             orderby (p.Price - ((Double)p.SalePrice / 100) * p.Price) descending

                             select new ItemProductsViewModel
                             {
                                 Id = p.Id,
                                 Name = p.Name,
                                 Price = p.Price,
                                 SalePrice = p.SalePrice,
                                 Thumbnail = p.Thumbnail,
                                 Stars = p.Stars,
                                 Views = p.Views,
                                 Orders = p.Orders,
                                 NameUrl = p.URL,
                                 CateUrl = c.URL,
                                 Description = p.Description,
                                 Status = p.Status
                             }).Where(p => p.Status == 1);
                product = query.ToList();
            }
            else if (value == "Tang")
            {
                var query = (from p in _context.Products
                             join c in _context.Categorys
                             on p.Category_Id equals c.Id
                             where p.Name.ToUrlFriendly().Contains(key.ToUrlFriendly())
                             orderby (p.Price - ((Double)p.SalePrice / 100) * p.Price) ascending

                             select new ItemProductsViewModel
                             {
                                 Id = p.Id,
                                 Name = p.Name,
                                 Price = p.Price,
                                 SalePrice = p.SalePrice,
                                 Thumbnail = p.Thumbnail,
                                 Stars = p.Stars,
                                 Views = p.Views,
                                 Orders = p.Orders,
                                 NameUrl = p.URL,
                                 CateUrl = c.URL,
                                 Description = p.Description,
                                 Status = p.Status
                             }).Where(p => p.Status == 1);
                product = query.ToList();
            }
            else
            {
                var query = (from p in _context.Products
                             join c in _context.Categorys
                             on p.Category_Id equals c.Id
                             where p.Name.ToUrlFriendly().Contains(key.ToUrlFriendly())
                             select new ItemProductsViewModel
                             {
                                 Id = p.Id,
                                 Name = p.Name,
                                 Price = p.Price,
                                 SalePrice = p.SalePrice,
                                 Thumbnail = p.Thumbnail,
                                 Stars = p.Stars,
                                 Views = p.Views,
                                 Orders = p.Orders,
                                 NameUrl = p.URL,
                                 CateUrl = c.URL,
                                 Description = p.Description,
                                 Status = p.Status
                             }).Where(p => p.Status == 1);
                product = query.ToList();
            }
            return product;
        }


        // sort sp theo category va value

        public List<ItemProductsViewModel> Sort(string cate, string value)
        {
            List<ItemProductsViewModel> product = new List<ItemProductsViewModel>();

            if (cate == "tat-ca-san-pham")
            {
                if (value == "Name")
                {
                    var query = (from p in _context.Products
                                 join c in _context.Categorys
                                 on p.Category_Id equals c.Id
                                 orderby p.Name descending

                                 select new ItemProductsViewModel
                                 {
                                     Id = p.Id,
                                     Name = p.Name,
                                     Price = p.Price,
                                     SalePrice = p.SalePrice,
                                     Thumbnail = p.Thumbnail,
                                     Stars = p.Stars,
                                     Views = p.Views,
                                     Orders = p.Orders,
                                     NameUrl = p.URL,
                                     CateUrl = c.URL,
                                     Description = p.Description,
                                     Status = p.Status
                                 }).Where(p => p.Status == 1);
                    product = query.ToList();
                }
                else if (value == "Price")
                {
                    var query = (from p in _context.Products
                                 join c in _context.Categorys
                                 on p.Category_Id equals c.Id
                                 orderby (p.Price - ((Double)p.SalePrice / 100) * p.Price) descending

                                 select new ItemProductsViewModel
                                 {
                                     Id = p.Id,
                                     Name = p.Name,
                                     Price = p.Price,
                                     SalePrice = p.SalePrice,
                                     Thumbnail = p.Thumbnail,
                                     Stars = p.Stars,
                                     Views = p.Views,
                                     Orders = p.Orders,
                                     NameUrl = p.URL,
                                     CateUrl = c.URL,
                                     Description = p.Description,
                                     Status = p.Status
                                 }).Where(p => p.Status == 1);
                    product = query.ToList();
                }
                else if (value == "Tang")
                {
                    var query = (from p in _context.Products
                                 join c in _context.Categorys
                                 on p.Category_Id equals c.Id

                                 orderby (p.Price - ((Double)p.SalePrice / 100) * p.Price) ascending

                                 select new ItemProductsViewModel
                                 {
                                     Id = p.Id,
                                     Name = p.Name,
                                     Price = p.Price,
                                     SalePrice = p.SalePrice,
                                     Thumbnail = p.Thumbnail,
                                     Stars = p.Stars,
                                     Views = p.Views,
                                     Orders = p.Orders,
                                     NameUrl = p.URL,
                                     CateUrl = c.URL,
                                     Description = p.Description,
                                     Status = p.Status
                                 }).Where(p => p.Status == 1);
                    product = query.ToList();
                }
                else
                {
                    var query = (from p in _context.Products
                                 join c in _context.Categorys
                                 on p.Category_Id equals c.Id


                                 select new ItemProductsViewModel
                                 {
                                     Id = p.Id,
                                     Name = p.Name,
                                     Price = p.Price,
                                     SalePrice = p.SalePrice,
                                     Thumbnail = p.Thumbnail,
                                     Stars = p.Stars,
                                     Views = p.Views,
                                     Orders = p.Orders,
                                     NameUrl = p.URL,
                                     CateUrl = c.URL,
                                     Description = p.Description,
                                     Status = p.Status
                                 }).Where(p => p.Status == 1);
                    product = query.ToList();
                }
            }

            else
            {
                if (value == "Name")
                {
                    var query = (from p in _context.Products
                                 join c in _context.Categorys
                                 on p.Category_Id equals c.Id
                                 where c.URL == cate.Trim()
                                 orderby p.Name descending

                                 select new ItemProductsViewModel
                                 {
                                     Id = p.Id,
                                     Name = p.Name,
                                     Price = p.Price,
                                     SalePrice = p.SalePrice,
                                     Thumbnail = p.Thumbnail,
                                     Stars = p.Stars,
                                     Views = p.Views,
                                     Orders = p.Orders,
                                     NameUrl = p.URL,
                                     CateUrl = c.URL,
                                     Description = p.Description,
                                     Status = p.Status
                                 }).Where(p => p.Status == 1);
                    product = query.ToList();
                }
                else if (value == "Price")
                {
                    var query = (from p in _context.Products
                                 join c in _context.Categorys
                                 on p.Category_Id equals c.Id
                                 where c.URL == cate.Trim()
                                 orderby (p.Price - ((Double)p.SalePrice / 100) * p.Price) descending

                                 select new ItemProductsViewModel
                                 {
                                     Id = p.Id,
                                     Name = p.Name,
                                     Price = p.Price,
                                     SalePrice = p.SalePrice,
                                     Thumbnail = p.Thumbnail,
                                     Stars = p.Stars,
                                     Views = p.Views,
                                     Orders = p.Orders,
                                     NameUrl = p.URL,
                                     CateUrl = c.URL,
                                     Description = p.Description,
                                     Status = p.Status
                                 }).Where(p => p.Status == 1);
                    product = query.ToList();
                }
                else if (value == "Tang")
                {
                    var query = (from p in _context.Products
                                 join c in _context.Categorys
                                 on p.Category_Id equals c.Id
                                 where c.URL == cate.Trim()
                                 orderby (p.Price - ((Double)p.SalePrice / 100) * p.Price) ascending

                                 select new ItemProductsViewModel
                                 {
                                     Id = p.Id,
                                     Name = p.Name,
                                     Price = p.Price,
                                     SalePrice = p.SalePrice,
                                     Thumbnail = p.Thumbnail,
                                     Stars = p.Stars,
                                     Views = p.Views,
                                     Orders = p.Orders,
                                     NameUrl = p.URL,
                                     CateUrl = c.URL,
                                     Description = p.Description,
                                     Status = p.Status
                                 }).Where(p => p.Status == 1);
                    product = query.ToList();
                }
                else
                {
                    var query = (from p in _context.Products
                                 join c in _context.Categorys
                                 on p.Category_Id equals c.Id
                                 where c.URL == cate.Trim()

                                 select new ItemProductsViewModel
                                 {
                                     Id = p.Id,
                                     Name = p.Name,
                                     Price = p.Price,
                                     SalePrice = p.SalePrice,
                                     Thumbnail = p.Thumbnail,
                                     Stars = p.Stars,
                                     Views = p.Views,
                                     Orders = p.Orders,
                                     NameUrl = p.URL,
                                     CateUrl = c.URL,
                                     Description = p.Description,
                                     Status = p.Status
                                 }).Where(p => p.Status == 1);
                    product = query.ToList();
                }
            }
            return product;
        }


        public List<ItemProductsViewModel> SearchProduct(string key)
        {
            List<ItemProductsViewModel> product = new List<ItemProductsViewModel>();

            if (key == null)
            {
                var query = (from p in _context.Products
                             join c in _context.Categorys
                             on p.Category_Id equals c.Id


                             select new ItemProductsViewModel
                             {
                                 Id = p.Id,
                                 Name = p.Name,
                                 Price = p.Price,
                                 SalePrice = p.SalePrice,
                                 Thumbnail = p.Thumbnail,
                                 Stars = p.Stars,
                                 Views = p.Views,
                                 Orders = p.Orders,
                                 NameUrl = p.URL,
                                 CateUrl = c.URL,
                                 Description = p.Description,
                                 Status = p.Status
                             }).Where(p => p.Status == 1);
                product = query.ToList();
            }
            else
            {
                var query = (from p in _context.Products
                             join c in _context.Categorys
                             on p.Category_Id equals c.Id
                             where p.Name.ToUrlFriendly().Contains(key.ToUrlFriendly())

                             select new ItemProductsViewModel
                             {
                                 Id = p.Id,
                                 Name = p.Name,
                                 Price = p.Price,
                                 SalePrice = p.SalePrice,
                                 Thumbnail = p.Thumbnail,
                                 Stars = p.Stars,
                                 Views = p.Views,
                                 Orders = p.Orders,
                                 NameUrl = p.URL,
                                 CateUrl = c.URL,
                                 Description = p.Description,
                                 Status = p.Status
                             }).Where(p => p.Status == 1);
                product = query.ToList();
            }
            return product;
        }

        // lấy product theo cate
        public List<ItemProductsViewModel> GetProducts(string urlcate)
        {
            List<ItemProductsViewModel> product = new List<ItemProductsViewModel>();

            if (urlcate == "all")
            {
                var query = (from p in _context.Products
                             join c in _context.Categorys
                             on p.Category_Id equals c.Id


                             select new ItemProductsViewModel
                             {
                                 Id = p.Id,
                                 Name = p.Name,
                                 Price = p.Price,
                                 SalePrice = p.SalePrice,
                                 Thumbnail = p.Thumbnail,
                                 Stars = p.Stars,
                                 Views = p.Views,
                                 Orders = p.Orders,
                                 NameUrl = p.URL,
                                 CateUrl = c.URL,
                                 Description = p.Description,
                                 Status = p.Status
                             }).Where(p => p.Status == 1);
                product = query.ToList();
            }
            else
            {
                var query = (from p in _context.Products
                             join c in _context.Categorys
                             on p.Category_Id equals c.Id
                             where c.URL == urlcate.Trim()
                             select new ItemProductsViewModel
                             {
                                 Id = p.Id,
                                 Name = p.Name,
                                 Price = p.Price,
                                 SalePrice = p.SalePrice,
                                 Thumbnail = p.Thumbnail,
                                 Stars = p.Stars,
                                 Views = p.Views,
                                 Orders = p.Orders,
                                 NameUrl = p.URL,
                                 CateUrl = c.URL,
                                 Description = p.Description,
                                 Status = p.Status
                             }).Where(p => p.Status == 1);
                product = query.ToList();
            }
            return product;
        }

        public IOrderedQueryable<ItemProductsViewModel> GetQueryProducts(string urlcate)
        {


            if (urlcate == "all")
            {
                var query = (from p in _context.Products
                             join c in _context.Categorys
                             on p.Category_Id equals c.Id


                             select new ItemProductsViewModel
                             {
                                 Id = p.Id,
                                 Name = p.Name,
                                 Price = p.Price,
                                 SalePrice = p.SalePrice,
                                 Thumbnail = p.Thumbnail,
                                 Stars = p.Stars,
                                 Views = p.Views,
                                 Orders = p.Orders,
                                 NameUrl = p.URL,
                                 CateUrl = c.URL,
                                 Description = p.Description,
                                 Status = p.Status
                             }).AsNoTracking().Where(p => p.Status == 1).OrderBy(p => p.Id);
                return query;
            }
            else
            {
                var query = (from p in _context.Products
                             join c in _context.Categorys
                             on p.Category_Id equals c.Id
                             where c.URL == urlcate.Trim()
                             select new ItemProductsViewModel
                             {
                                 Id = p.Id,
                                 Name = p.Name,
                                 Price = p.Price,
                                 SalePrice = p.SalePrice,
                                 Thumbnail = p.Thumbnail,
                                 Stars = p.Stars,
                                 Views = p.Views,
                                 Orders = p.Orders,
                                 NameUrl = p.URL,
                                 CateUrl = c.URL,
                                 Description = p.Description,
                                 Status = p.Status
                             }).AsNoTracking().Where(p => p.Status == 1).OrderBy(p => p.Id);
                return query;
            }

        }

        // product top
        public List<ItemProductsViewModel> GetProductsDescending(string name, int sl)
        {
            List<ItemProductsViewModel> product = new List<ItemProductsViewModel>();


            if (name == "Views")
            {
                var query = (from p in _context.Products
                             join c in _context.Categorys
                             on p.Category_Id equals c.Id
                             orderby p.Views descending

                             select new ItemProductsViewModel
                             {
                                 Id = p.Id,
                                 Name = p.Name,
                                 Price = p.Price,
                                 SalePrice = p.SalePrice,
                                 Thumbnail = p.Thumbnail,
                                 Stars = p.Stars,
                                 Views = p.Views,
                                 Orders = p.Orders,
                                 NameUrl = p.URL,
                                 CateUrl = c.URL,
                                 Description = p.Description,
                                 Status = p.Status
                             }).Where(p => p.Status == 1);
                product = query.Take(sl).ToList();
            }
            else if (name == "Stars")
            {
                var query = (from p in _context.Products
                             join c in _context.Categorys
                             on p.Category_Id equals c.Id
                             orderby p.Stars descending

                             select new ItemProductsViewModel
                             {
                                 Id = p.Id,
                                 Name = p.Name,
                                 Price = p.Price,
                                 SalePrice = p.SalePrice,
                                 Thumbnail = p.Thumbnail,
                                 Stars = p.Stars,
                                 Views = p.Views,
                                 Orders = p.Orders,
                                 NameUrl = p.URL,
                                 CateUrl = c.URL,
                                 Description = p.Description,
                                 Status = p.Status
                             }).Where(p => p.Status == 1);
                product = query.Take(sl).ToList();
            }
            else if (name == "New")
            {
                var query = (from p in _context.Products
                             join c in _context.Categorys
                             on p.Category_Id equals c.Id
                             orderby p.Id descending

                             select new ItemProductsViewModel
                             {
                                 Id = p.Id,
                                 Name = p.Name,
                                 Price = p.Price,
                                 SalePrice = p.SalePrice,
                                 Thumbnail = p.Thumbnail,
                                 Stars = p.Stars,
                                 Views = p.Views,
                                 Orders = p.Orders,
                                 NameUrl = p.URL,
                                 CateUrl = c.URL,
                                 Description = p.Description,
                                 Status = p.Status
                             }).Where(p => p.Status == 1);
                product = query.Take(sl).ToList();
            }
            else if (name == "Sale")
            {
                var query = (from p in _context.Products
                             join c in _context.Categorys
                             on p.Category_Id equals c.Id
                             orderby p.SalePrice descending

                             select new ItemProductsViewModel
                             {
                                 Id = p.Id,
                                 Name = p.Name,
                                 Price = p.Price,
                                 SalePrice = p.SalePrice,
                                 Thumbnail = p.Thumbnail,
                                 Stars = p.Stars,
                                 Views = p.Views,
                                 Orders = p.Orders,
                                 NameUrl = p.URL,
                                 CateUrl = c.URL,
                                 Description = p.Description,
                                 Status = p.Status
                             }).Where(p => p.Status == 1);
                product = query.Take(sl).ToList();
            }
            else if (name == "Orders")
            {
                var query = (from p in _context.Products
                             join c in _context.Categorys
                             on p.Category_Id equals c.Id
                             orderby p.Orders descending

                             select new ItemProductsViewModel
                             {
                                 Id = p.Id,
                                 Name = p.Name,
                                 Price = p.Price,
                                 SalePrice = p.SalePrice,
                                 Thumbnail = p.Thumbnail,
                                 Stars = p.Stars,
                                 Views = p.Views,
                                 Orders = p.Orders,
                                 NameUrl = p.URL,
                                 CateUrl = c.URL,
                                 Description = p.Description,
                                 Status = p.Status
                             });
                product = query.Take(sl).Where(p => p.Status == 1).ToList();
            }

            return product;
        }


        public List<ItemProductsViewModel> GetSanPhamLienQuan(int idcate, int idpro)
        {
            List<ItemProductsViewModel> product = new List<ItemProductsViewModel>();


            var query = (from p in _context.Products
                         join c in _context.Categorys
                         on p.Category_Id equals c.Id
                         where p.Id != idpro &&
                         p.Category_Id == idcate

                         select new ItemProductsViewModel
                         {
                             Id = p.Id,
                             Name = p.Name,
                             Price = p.Price,
                             SalePrice = p.SalePrice,
                             Thumbnail = p.Thumbnail,
                             Stars = p.Stars,
                             Views = p.Views,
                             Orders = p.Orders,
                             NameUrl = p.URL,
                             CateUrl = c.URL,
                             Description = p.Description,
                             Status = p.Status
                         }).Where(p => p.Status == 1);
            product = query.Take(4).ToList();
            return product;
        }

    }

}

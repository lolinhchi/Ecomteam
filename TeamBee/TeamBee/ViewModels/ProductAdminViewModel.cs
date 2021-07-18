using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamBee.Connect_Data;

namespace TeamBee.ViewModels
{
    public class ProductAdminViewModel
    {
        public int Id { get; set; }
        public string Ten { get; set; }
        public string Anh { get; set; }
        public string Loai { get; set; }
        public int Gia { get; set; }
        public Double Sale { get; set; }
        public int Status { get; set; }
        public ProductAdminViewModel() { }
        private readonly TeamBeeDbContext _context;

        public ProductAdminViewModel(TeamBeeDbContext context)
        {
            _context = context;
        }

        public List<ProductAdminViewModel> GetProductAdminViewModel()
        {
            List<ProductAdminViewModel> product = new List<ProductAdminViewModel>();

            var query = (from p in _context.Products
                         join c in _context.Categorys
                         on p.Category_Id equals c.Id


                         select new ProductAdminViewModel
                         {
                             Id = p.Id,
                             Ten = p.Name,
                             Gia = p.Price,
                             Sale = p.SalePrice,
                             Anh = p.Thumbnail,
                             Loai = c.Name,
                             Status =p.Status
                             
                         });
            product = query.ToList();


            return product;
        }
    }
}
